using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class DFParameters<Gb, M, T> where Gb : PokemonGame
                                    where M : Map<M, T>
                                    where T : Tile<M, T> {

    public bool PruneAlreadySeenStates = true;
    public int MaxCost = 0;
    public int SuccessSS = 1;
    public int RNGSS = -1;
    public int RNGRange = 9;
    public int MaxTurns = -1;
    public string LogStart = "";
    public T[] EndTiles = null;
    public int EndEdgeSet = 0;
    public Func<Gb, bool> EncounterCallback = null;
    public Action<DFState<M, T>> FoundCallback = state => Console.WriteLine(state.Log);
    public (Tile<M, T> Tile, Action<Gb> Function)[] TileCallbacks;
}

public class DFState<M, T> where M : Map<M, T>
                           where T : Tile<M, T> {

    public T Tile;
    public int EdgeSet;
    public int WastedFrames = 0;
    public int APressCounter;
    public int Turns = -1;
    public Action LastDir;
    public IGTResults IGT;
    public string Log;

    public override int GetHashCode() {
        unchecked {
            const int prime = 92821;
            int hash = prime + Tile.Map.Id;
            hash = hash * prime + Tile.X;
            hash = hash * prime + Tile.Y;
            hash = hash * prime + IGT.MostCommonHRA;
            hash = hash * prime + IGT.MostCommonHRS;
            hash = hash * prime + IGT.MostCommonDivider;
            hash = hash * prime + IGT.TotalSuccesses;
            hash = hash * prime + IGT.TotalRunning;
            return hash;
        }
    }
}

public class SeenResults : Dictionary<string, int>
{
    public new bool Add(string path, int success) {
        foreach(var kv in this)
            if(path.StartsWith(kv.Key) && success == kv.Value)
                return false;
        this[path] = success;
        return true;
    }
}

public static class DepthFirstSearch {

    public static void StartSearch<Gb, M, T>(Gb[] gbs, DFParameters<Gb, M, T> parameters, T startTile, int startEdgeSet, IGTResults initialState, int APressCounter = 1) where Gb : PokemonGame
                                                                                                                                                                      where M : Map<M, T>
                                                                                                                                                                      where T : Tile<M, T> {
        foreach(var igt in initialState.IGTs) igt.Success = false;
        RecursiveSearch(gbs, parameters, new DFState<M, T> {
            Tile = startTile,
            EdgeSet = startEdgeSet,
            Log = parameters.LogStart,
            APressCounter = APressCounter,
            IGT = initialState,
        }, new HashSet<int>(), new SeenResults());
    }

    private static void RecursiveSearch<Gb, M, T>(Gb[] gbs, DFParameters<Gb, M, T> parameters, DFState<M, T> state, HashSet<int> seenStates, SeenResults seenResults) where Gb : PokemonGame
                                                                                                                                             where M : Map<M, T>
                                                                                                                                             where T : Tile<M, T> {

        if(parameters.EndTiles != null && state.EdgeSet == parameters.EndEdgeSet && parameters.EndTiles.Any(t => t.X == state.Tile.X && t.Y == state.Tile.Y && t.Map.Id == state.Tile.Map.Id)) {
            if(parameters.EncounterCallback == null)
                parameters.FoundCallback(state);
            return;
        }

        if(parameters.PruneAlreadySeenStates && !seenStates.Add(state.GetHashCode()))
            return;

        foreach(Edge<M, T> edge in state.Tile.Edges[state.EdgeSet].OrderBy(x => x.Action != state.LastDir)) { // try the same direction first
            if(state.WastedFrames + edge.Cost > parameters.MaxCost) continue;
            if((edge.Action & Action.A) != 0 && state.APressCounter > 0) continue;
            if(edge.Action == Action.StartB && state.APressCounter == 2) continue;
            Action moving = edge.Action & (Action.Up | Action.Down | Action.Left | Action.Right);
            if(parameters.MaxTurns >= 0 && state.Turns == parameters.MaxTurns && moving != 0 && moving != state.LastDir) continue;

            // IGTResults results = PokemonGame.IGTCheckParallel<Gb>(gbs, state.IGT, gb => gb.Execute(edge.Action) == gb.OverworldLoopAddress, parameters.NoEncounterSS);
            IGTResults results = new IGTResults(state.IGT.Length);
            MultiThread.For(state.IGT.Length, gbs, (gb, f) => {
                IGTState prev = state.IGT[f];
                IGTState igt;
                if(prev.Running) { // we're in the overworld, execute action
                    gb.LoadState(prev.State);
                    int ret = gb.Execute(edge.Action);

                    if(parameters.TileCallbacks != null && ret == gb.OverworldLoopAddress)
                        foreach(var callback in parameters.TileCallbacks)
                            if(edge.NextTile == callback.Tile)
                                callback.Function(gb);

                    igt = new IGTState(gb, prev.Success, prev.IGTStamp);

                    if(ret != gb.OverworldLoopAddress) {
                        if(ret == gb.WildEncounterAddress)
                            igt.Success = parameters.EncounterCallback != null ? parameters.EncounterCallback(gb) : false;
                        // else Console.WriteLine("movement failed on frame " + f);
                        igt.Running = false;
                    }
                } else {
                    igt = prev; // this frame is done, just reference previous state
                }
                results[f] = igt;
            });

            // for(int i = 0; i < results.Length; ++i) // clusters
            // {
            //     if(!results[i].Running)
            //         for(int n = 2; n <= parameters.SuccessSS; ++n)
            //             if(!results[(i + n) % results.Length].Running)
            //                 for(int j = i + 1; j < i + n; ++j)
            //                     results[j % results.Length].Running = false;
            // }

            DFState<M, T> newState = new DFState<M, T>() {
                Tile = edge.NextTile,
                EdgeSet = edge.NextEdgeset,
                Log = state.Log + edge.Action.LogString(),
                IGT = results,
                WastedFrames = state.WastedFrames + edge.Cost,
            };

            int totalSuccesses = results.TotalSuccesses;
            int totalRunning = results.TotalRunning;

            if(totalSuccesses >= parameters.SuccessSS) // success
                if(seenResults.Add(newState.Log, totalSuccesses))
                    parameters.FoundCallback(newState);

            if(totalRunning > 0 && totalRunning + totalSuccesses >= parameters.SuccessSS) { // success still possible
                if(parameters.RNGSS <= 0 || results.RNGSuccesses(parameters.RNGRange) >= parameters.RNGSS) {
                    newState.APressCounter = edge.Action == Action.A ? 2 : Math.Max(state.APressCounter - 1, 0);
                    newState.LastDir = moving != 0 ? moving : state.LastDir;
                    if(parameters.MaxTurns >= 0) newState.Turns = newState.LastDir != state.LastDir ? state.Turns + 1 : state.Turns;

                    RecursiveSearch(gbs, parameters, newState, seenStates, seenResults);
                }
            }
        }
    }
}
