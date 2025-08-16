using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;

using static SearchCommon;
using static RbyIGTChecker<Red>;

class MistyMoon
{
    const string State = "basesaves/red/manip/mistymoon.gqs";
    const string CurrentPath = "RRRRRRRRURRUUUUUARRRRRRRRRRRRDDDDDRRRRRRRARUURRUUUUUUUUUURRRRUUUUUUUUUURRRRRU"
    + "UUUUUUUAUUUUUURRRRRRUUUUUUUUUUUURRRRRRRRRRRUUUUUUAULLLLLLDDLALLLLLLLDADDADDAD"
    + "DDLALLALLALLAL"
    + "RRRUUULUR"
    + "DDDDLLAL"
    + "RARRARRARRARUU"
    + "DDDDDLDALLLLLLLUUUUUUUUUULLLUUL"
    + "RARDDDDDDDADDADDDRARRRRRRRRRRRRR"
    + "UUAURARRRRRADDRRRRRUURRRARDDDDADDDDLLLLDDDDDDADDDALLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUUUUUU"
    ;
    const string IGTPath = "RRRRRRRRURRUUUUUARRRRRRRRRRRRDDDDDRRRRRRRARUURRUUUUUUUUUURRRRUUUUUUUUUURRRRRU"
    + "UUUUUUUAUUUUUURRRRRRUUUUUUUUUUUURRRRRRRRRRRUUUUUUAULLLLLLDDLALLLLLLLDADDADDAD"
    + "LLLLLLLLDD"
    + "RRRUUULAUR"
    + "DDDDLLL"
    + "RRRRRRRRUU"
    + "DDDDDDLLLLLLLLLLLUUUUUUUUUUUUL"
    + "RRDDDDDDDDDDDDRRRRRRRRRRRRRR"
    + "RRAUUURRRDDRRRRRRUURRRRDDDDDDDDLLLLDDDDDDDDDLLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUUUUUU"
    ;

    public static void Search()
    {
        // Search(new RbyIntroSequence(RbyStrat.PalHold), 2, 60, 2);
        // Search(new RbyIntroSequence(RbyStrat.PalHold));

        CheckIGT(new CheckIGTParameters() {
            StatePath = State,
            Intro = new RbyIntroSequence(RbyStrat.PalHold),
            Path = CurrentPath,
            NumFrames = 3600,
            MemeBall = gb => false
        });
        CheckIGT(new CheckIGTParameters() {
            StatePath = State,
            Intro = new RbyIntroSequence(RbyStrat.PalHold),
            Path = IGTPath,
            NumFrames = 2,
            MemeBall = gb => false,
            StartFrame = 36,
            Seconds = 60
        });

        // Red gb = new Red();
        // gb.LoadState(State);
        // gb.Record("test");
        // gb.HardReset();
        // new RbyIntroSequence(RbyStrat.PalHold).Execute(gb);
        // gb.Execute(SpacePath(CurrentPath), (gb.Maps[61][28, 5], gb.PickupItem));
        // gb.Dispose();
    }

    public static void Search(RbyIntroSequence intro, int numThreads = 16, int numFrames = 60, int success = 58)
    {
        StartWatch();
        Trace.WriteLine(intro);
        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        RbyMap route4 = gb.Maps[15];
        RbyMap moon1 = gb.Maps[59];
        RbyMap moon2 = gb.Maps[60];
        RbyMap moon3 = gb.Maps[61];

        gb.LoadState(State);
        // IGTResults states = Red.IGTCheckParallel(gbs, intro, numFrames);
        IGTResults states = Red.IGTCheckParallel(gbs, intro, numFrames, gb =>
            gb.Execute(SpacePath(CurrentPath), (moon3[28, 5], gb.PickupItem)) == gb.OverworldLoopAddress
        );
        states[36].Success = false;
        states[37].Success = false;
        states = states.Purge(true);
        // RNGDebug(states); return;

        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A;
        RbyTile startTile = gb.Tile;
        // Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, route4[18, 6], actions);
        // route4[18, 6].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = moon1[14, 35], NextEdgeset = 0, Cost = 0 });
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, moon3[28, 5], actions, moon2[18, 10], moon2[19, 10], moon2[20, 10], moon2[21, 10], moon2[22, 10], moon2[23, 10], moon2[24, 10]);
        for(int x = 22; x <= 30; ++x) for(int y = 11; y <= 14; ++y) { moon1[x, y].RemoveEdge(0, Action.Up); }
        moon1[26, 3].RemoveEdge(0, Action.Down);
        moon1[27, 3].RemoveEdge(0, Action.Down);
        moon1[28, 3].RemoveEdge(0, Action.Down);
        moon1[24, 3].RemoveEdge(0, Action.Left);
        moon3[28, 6].GetEdge(0, Action.Left).Cost = 0;
        moon3[28, 6].RemoveEdge(0, Action.Up);
        moon3[27, 6].RemoveEdge(0, Action.Right);
        moon3[27, 5].RemoveEdge(0, Action.A);
        moon3[27, 5].GetEdge(0, Action.Right).NextEdgeset = 1;
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 1, moon3[10, 17], actions, moon2[18, 10], moon2[19, 10], moon2[20, 10], moon2[21, 10], moon2[22, 10], moon2[23, 10], moon2[24, 10], moon3[33, 23], moon3[34, 23], moon3[35, 23], moon3[36, 23]);
        moon3[28, 5].RemoveEdge(1, Action.A);
        moon1[10, 11].RemoveEdge(1, Action.Up);
        moon1[11, 11].RemoveEdge(1, Action.Up);
        for(int x = 3; x <= 9; ++x) for(int y = 9; y <= 10; ++y) { moon1[x, y].RemoveEdge(1, Action.Left); }

        Paths results = new Paths();
        var parameters = new DFParameters<Red, RbyMap, RbyTile>()
        {
            MaxCost = 6,
            // RNGSS = 56,
            // RNGRange = 3,
            // MaxTurns = 10,
            SuccessSS = success,
            // EndTiles = new RbyTile[] { moon2[25, 9] }, EndEdgeSet = 0,
            // EndTiles = new RbyTile[] { moon3[28, 5] }, EndEdgeSet = 1,
            // EndTiles = new RbyTile[] { moon2[17, 11] }, EndEdgeSet = 1,
            // EndTiles = new RbyTile[] { moon2[5, 5] }, EndEdgeSet = 1,
            // EndTiles = new RbyTile[] { moon3[21, 17] }, EndEdgeSet = 1,
            EndTiles = new RbyTile[] { moon3[10, 17] }, EndEdgeSet = 1,
            TileCallbacks = new (Tile<RbyMap, RbyTile>, Action<Red>)[] {
                (moon3[28, 5], gb => gb.PickupItem()),
            },
            LogStart = startTile.PokeworldLink + "/",
            FoundCallback = state =>
            {
                Path p = new Path(state.Log, state.IGT.TotalRunning, state.WastedFrames, RNGSummary(state.IGT));
                Trace.WriteLine(p);
                results.Add(p);
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states);
        results.CleanPrintAll();
        Elapsed("search");
    }
}
