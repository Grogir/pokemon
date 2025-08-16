using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class SFParameters<Gb, M, T> where Gb : PokemonGame
                                    where M : Map<M, T>
                                    where T : Tile<M, T> {

    public bool PruneAlreadySeenStates = true;
    public int MaxCost = 0;
    public string LogStart = "";
    public T[] EndTiles = null;
    public int EndEdgeSet = 0;
    public Func<Gb, bool> EncounterCallback = null;
    public Action<SFState<M, T>, Gb> FoundCallback;
    public (Tile<M, T> Tile, Action<Gb> Callback) TileCallback;
}

public class SFState<M, T> where M : Map<M, T>
                           where T : Tile<M, T> {

    public T Tile;
    public int EdgeSet;
    public int WastedFrames;
    public Action LastDir;
    public int APressCounter;
    public IGTState IGT;
    public string Log;

    public override int GetHashCode() {
        unchecked {
            const int prime = 92821;
            int hash = prime + Tile.Map.Id;
            hash = hash * prime + Tile.X;
            hash = hash * prime + Tile.Y;
            hash = hash * prime + IGT.HRA;
            hash = hash * prime + IGT.HRS;
            hash = hash * prime + IGT.Divider;
            return hash;
        }
    }
}

public static class SingleFrameSearch {

    public static void StartSearch<Gb, M, T>(Gb[] gbs, SFParameters<Gb, M, T> parameters, T startTile, int startEdgeSet, IGTState initialState, int APressCounter = 1) where Gb : PokemonGame
                                                                                                                                                                      where M : Map<M, T>
                                                                                                                                                                      where T : Tile<M, T> {
        if(parameters.FoundCallback == null)
            parameters.FoundCallback = (state, gb) => Console.WriteLine(state.Log);

        MaxThreads = gbs.Length;
        AvailableThreads = MaxThreads - 1;
        ThreadParameters = new Queue<(object, object, object)>();
        ThreadParameters.Enqueue((
            parameters,
            new SFState<M, T> {
                Tile = startTile,
                EdgeSet = startEdgeSet,
                WastedFrames = 0,
                Log = parameters.LogStart,
                APressCounter = APressCounter,
                IGT = initialState,
            },
            new HashSet<int>()
        ));

        for(int i = 1; i < MaxThreads; ++i)
            new Thread(StartThread<Gb, M, T>).Start(gbs[i]);
        StartThread<Gb, M, T>(gbs[0]);
    }

    static int AvailableThreads;
    static int MaxThreads;
    static object AvailableThreadsLock = new object();
    static Queue<(object, object, object)> ThreadParameters;

    private static void StartThread<Gb, M, T>(object gb) where Gb : PokemonGame where M : Map<M, T> where T : Tile<M, T> {
        while(AvailableThreads < MaxThreads) {
            (object, object, object)? parameters = null;

            lock(ThreadParameters)
                if(ThreadParameters.Count > 0)
                    parameters = ThreadParameters.Dequeue();

            if(parameters != null) {
                RecursiveSearch((Gb) gb, (SFParameters<Gb, M, T>) parameters.Value.Item1, (SFState<M, T>) parameters.Value.Item2, (HashSet<int>) parameters.Value.Item3);
                lock(AvailableThreadsLock) AvailableThreads++;
            }
        }
    }

    private static void RecursiveSearch<Gb, M, T>(Gb gb, SFParameters<Gb, M, T> parameters, SFState<M, T> state, HashSet<int> seenStates) where Gb : PokemonGame
                                                                                                                                             where M : Map<M, T>
                                                                                                                                             where T : Tile<M, T> {

        if(parameters.EndTiles != null && state.EdgeSet == parameters.EndEdgeSet && parameters.EndTiles.Any(t => t.X == state.Tile.X && t.Y == state.Tile.Y && t.Map.Id == state.Tile.Map.Id)) {
            if(parameters.EncounterCallback == null)
                parameters.FoundCallback(state, gb);
            return;
        }

        if(parameters.PruneAlreadySeenStates) {
            int hash = state.GetHashCode();
            if(seenStates.Contains(hash)) return;
            lock(seenStates) seenStates.Add(hash);
        }

        foreach(Edge<M, T> edge in state.Tile.Edges[state.EdgeSet].OrderBy(x => x.Action != state.LastDir)) {
            if(state.WastedFrames + edge.Cost > parameters.MaxCost) continue;
            if((edge.Action & Action.A) != 0 && state.APressCounter > 0) continue;
            if(edge.Action == Action.StartB && state.APressCounter == 2) continue;

            SFState<M, T> newState = new SFState<M, T>() {
                Tile = edge.NextTile,
                EdgeSet = edge.NextEdgeset,
                Log = state.Log + edge.Action.LogString(),
                WastedFrames = state.WastedFrames + edge.Cost,
            };

            gb.LoadState(state.IGT.State);
            int ret = gb.Execute(edge.Action);

            if(parameters.TileCallback.Tile != null && ret == gb.OverworldLoopAddress && edge.NextTile == parameters.TileCallback.Tile)
                parameters.TileCallback.Callback(gb);

            newState.IGT = new IGTState(gb, state.IGT.Success, state.IGT.IGTStamp);

            if(ret != gb.OverworldLoopAddress) {
                if(ret == gb.WildEncounterAddress)
                    if(parameters.EncounterCallback != null && parameters.EncounterCallback(gb))
                        parameters.FoundCallback(newState, gb);
                continue;
            }

            newState.APressCounter = edge.Action == Action.A ? 2 : Math.Max(state.APressCounter - 1, 0);
            Action moving = edge.Action & (Action.Up | Action.Down | Action.Left | Action.Right);
            newState.LastDir = moving != 0 ? moving : state.LastDir;

            bool callThread = false;
            lock(AvailableThreadsLock) {
                if(AvailableThreads > 0) {
                    AvailableThreads--;
                    callThread = true;
                }
            }
            if(callThread)
                lock(ThreadParameters) ThreadParameters.Enqueue((parameters, newState, seenStates));
            else
                RecursiveSearch(gb, parameters, newState, seenStates);
        }
    }
}
