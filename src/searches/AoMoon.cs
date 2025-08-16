using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using static SearchCommon;
using static RbyIGTChecker<Ao>;

class AoMoon
{
    const string State = "basesaves/ao/moon0.gqs";
    const string CurrentPath = "UUUUUULLLLLLLLALDD" + "UURRRRRUAUUUUUURRRRRRRRRRUUUUUUURRRRDDDDDDDDDDDRDDDDDDRRRRRURRRR" + "UUUUAUUUUR"
    + "ULUUUUUUUUUUULLUUUUUUUULLLDLLLLLLLLLLLLADDDDDDD"
    + "DDLALLALLALLAL" + "RRRUUULAUR" + "DDADLLLAD" + "RARRARRARRARUU"
    + "DDLADDDDLLLLLLLLUUUUUUUUUUUUUULLLULL"
    + "RRADDADDDDDDDDDDDDDRRRRRRRRRRRRRRRAR"
    + "UUURRRRRRADDRRRRRRUURRRDDDDDDDDLLLLDDDDDDDDDLLLLLLLLLLLLLLLLLLLLLLUUUUAUUUU" // 3166
    ;

    public static void Check()
    {
        RbyIntroSequence intro = new RbyIntroSequence();
        // string path = "RRRUUUUUUURRARRRRUURRRRRRRUUUUUUURRRDDDDDDDDDDDRDDDDDDRRRRRURRRR";
        // string path = "ULUUUUUUUUUUULLUUUUUUUULLLDLLLLLLLLLLLLDDDDDDD";
        // string path = "ULUUUUUUUUUUULLUUUUUUUULLLDLALLLDDLLLLLLLLDDDDD";
        // string path = "LALLALLLLALDAD" + "RRRUUULAUR" + "DDADLLLAD" + "RARRARRARRRAUU";
        // string path = "RUUURRARRDDRRRRRRRUURRARDDDDDDDDLLLLDDDDDDDDDLLLLLLLLLLLALLLLLLALLLLLUUUUUUUUAUU"; // 3165 3169

        CheckIGT(new CheckIGTParameters() {
            StatePath = State, Intro = intro, Path = CurrentPath, TargetPoke = "PARAS"
            // , MemeBall = AoNido.CustomBall
            // , NumFrames = 3600
            // , NumFrames = 1, NumThreads = 1
        } );
    }

    public static void CheckFile()
    {
        Paths paths = new Paths();
        foreach(string line in System.IO.File.ReadAllLines("paths.txt"))
        {
            string path = Regex.Match(line, "[^ ]+").ToString();
            int success = int.Parse(Regex.Match(line, "([0-9]+)/58").Groups[1].Value);
            int cost = int.Parse(Regex.Match(line, "c:([0-9]+)").Groups[1].Value);
            paths.Add(new Path(path, success, cost));
        }
        paths.PrintAll();
    }

    public static void Search()
    {
        Check();
        Search(new RbyIntroSequence());
    }

    public static void R3Moon()
    {
        string rt3Moon = "RRRRRRRRURRUUUUUARRRRRRRRRRRRDDDDDRRRRRRRARUURRUUUUUUUUUURRRRUUUUUUUUUURRRRRU"
        + "UUUUUULLLLLALLLLDD"
        + "RRRRUURRRARRUUUUUUURRRRRRRAUUUUUUURRRDRDDDDDDDADDDDDDDDADRRRRRURRRR"
        + "UUUUUUUUR"
        + "ULUUUUUAUUUUUULLLUUUUUUUULLLLLLDDLALLLLLLLDDDDDD"
        + "LALLALLALLALDD"
        + "RRRUUULAUR"
        + "DDADLALLAD"
        + "RARRARRARRARUU"
        + "DDLDDDDLLLLLLLULUUUUULUUUUUUUULLLUL"
        + "DADDRAR"
        + "DRRDDDDDDDDDDRRRARRRRRRRRRRDR"
        + "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUUU"
        ;

        Red[] gbs = MultiThread.MakeThreads<Red>(32);
        gbs[0].LoadState("basesaves/red/manip/rt3moon.gqs");
        var igt = Red.IGTCheckParallel(gbs, new RbyIntroSequence(RbyStrat.PalHold), 60, gb => {
            return gb.Execute(SpacePath(rt3Moon),
                (gb.Maps[59][ 5, 31], gb.PickupItem),
                (gb.Maps[59][34, 31], gb.PickupItem),
                (gb.Maps[59][35, 23], gb.PickupItem),
                (gb.Maps[61][28,  5], gb.PickupItem),
                (gb.Maps[59][ 3,  2], gb.PickupItem)
            ) == gb.OverworldLoopAddress;
        });
        RNGDebug(igt);
    }

    public static void Search(RbyIntroSequence intro, int numThreads = 16, int numFrames = 60)
    {
        StartWatch();

        Ao[] gbs = MultiThread.MakeThreads<Ao>(numThreads);
        Ao gb = gbs[0];

        RbyMap moon1 = gb.Maps[59];
        RbyMap moon2 = gb.Maps[60];
        RbyMap moon3 = gb.Maps[61];

        gb.LoadState(State);
        IGTResults states = Ao.IGTCheckParallel(gbs, intro, numFrames, gb =>
            gb.Execute(SpacePath(CurrentPath),
                (moon1[ 5, 31], gb.PickupItem),
                (moon1[34, 31], gb.PickupItem),
                (moon1[35, 23], gb.PickupItem),
                (moon3[28,  5], gb.PickupItem),
                (moon1[ 2,  3], gb.PickupItem),
                (moon1[ 3,  2], gb.PickupItem)
            ) == gb.OverworldLoopAddress
        ).Purge();

        if(numThreads == 1) gb.Show();

        // RNGDebug(states); return;

        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A;
        RbyTile startTile = gb.Tile;
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, moon1[5, 31], actions, moon1[6, 31]);
        moon1[5, 30].RemoveEdge(0, Action.A);
        moon1[5, 30].GetEdge(0, Action.Down).NextEdgeset = 1;
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 1, moon1[34, 31], actions, moon1[34, 32], moon1[5, 28], moon1[6, 28], moon1[7, 28]);
        moon1[5, 31].RemoveEdge(1, Action.A);
        moon1[33, 31].RemoveEdge(1, Action.A);
        moon1[33, 31].GetEdge(1, Action.Right).NextEdgeset = 2;
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 2, moon1[35, 23], actions, moon1[35, 24]);
        moon1[34, 31].RemoveEdge(2, Action.A);
        moon1[34, 23].RemoveEdge(2, Action.A);
        moon1[34, 23].GetEdge(2, Action.Right).NextEdgeset = 3;
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 3, moon3[28, 5], actions, moon1[35, 21], moon2[18, 10], moon2[19, 10], moon2[20, 10], moon2[21, 10], moon2[22, 10], moon2[23, 10], moon2[24, 10]);
        moon1[35, 23].RemoveEdge(3, Action.A);
        moon1[34, 10].RemoveEdge(3, Action.Left);
        moon1[34, 9].RemoveEdge(3, Action.Left);
        moon1[34, 8].RemoveEdge(3, Action.Left);
        moon1[26, 3].RemoveEdge(3, Action.Down);
        moon1[27, 3].RemoveEdge(3, Action.Down);
        moon1[28, 3].RemoveEdge(3, Action.Down);
        moon1[24, 3].RemoveEdge(3, Action.Left);
        moon3[28, 6].GetEdge(3, Action.Left).Cost = 0;
        moon3[28, 6].RemoveEdge(3, Action.Up);
        moon3[27, 6].RemoveEdge(3, Action.Right);
        moon3[27, 5].RemoveEdge(3, Action.A);
        moon3[27, 5].GetEdge(3, Action.Right).NextEdgeset = 4;
        moon1.Sprites.Remove(2, 2);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 4, moon1[2, 2], actions, moon1[3, 3], moon2[18, 10], moon2[19, 10], moon2[20, 10], moon2[21, 10], moon2[22, 10], moon2[23, 10], moon2[24, 10]);
        moon3[28, 5].RemoveEdge(4, Action.A);
        for(int x = 10; x <= 11; ++x) for(int y = 4; y <= 10; ++y) { moon1[x, y].RemoveEdge(4, Action.Left); moon1[x, y].RemoveEdge(4, Action.A); }
        for(int x = 3; x <= 9; ++x) for(int y = 9; y <= 10; ++y) { moon1[x, y].RemoveEdge(4, Action.Left); }
        moon1[4, 2].RemoveEdge(4, Action.A);
        moon1[4, 2].GetEdge(4, Action.Left).NextEdgeset = 5;
        moon1[2, 4].RemoveEdge(4, Action.A);
        moon1[2, 4].GetEdge(4, Action.Up).NextEdgeset = 5;
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 5, moon3[10, 17], actions, moon3[33, 23], moon3[34, 23], moon3[35, 23], moon3[36, 23]);
        moon1[3, 2].RemoveEdge(5, Action.A);
        moon1[2, 3].RemoveEdge(5, Action.A);
        // Pathfinding.DebugDrawEdges(gb, moon3, 5);

        Paths results = new Paths();
        var parameters = new DFParameters<Ao, RbyMap, RbyTile>()
        {
            SuccessSS = 54,
            // RNGSS = 54,
            // RNGRange = 3,
            MaxCost = 20,
            // MaxTurns = 10,
            EncounterCallback = gb => gb.EnemyMon.Species.Name == "PARAS" && gb.Tile.Y < 22 && gb.Tile.X < 12,
            // EndTiles = new RbyTile[] { moon1[5, 31] }, EndEdgeSet = 1,
            // EndTiles = new RbyTile[] { moon1[34, 31] }, EndEdgeSet = 2,
            // EndTiles = new RbyTile[] { moon1[35, 23] }, EndEdgeSet = 3,
            // EndTiles = new RbyTile[] { moon2[25, 9] }, EndEdgeSet = 3,
            // EndTiles = new RbyTile[] { moon3[28, 5] }, EndEdgeSet = 4,
            // EndTiles = new RbyTile[] { moon1[17, 12] }, EndEdgeSet = 4,
            // EndTiles = new RbyTile[] { moon1[3, 3] }, EndEdgeSet = 5,
            // EndTiles = new RbyTile[] { moon3[21, 17] }, EndEdgeSet = 5,
            // EndTiles = new RbyTile[] { moon3[22, 17] }, EndEdgeSet = 5,
            // EndTiles = new RbyTile[] { moon3[15, 31] }, EndEdgeSet = 5,
            EndTiles = new RbyTile[] { moon3[10, 17] }, EndEdgeSet = 5,
            TileCallbacks = new (Tile<RbyMap, RbyTile>, Action<Ao>)[] {
                (moon1[5, 31], gb => gb.PickupItem()),
                (moon1[34, 31], gb => gb.PickupItem()),
                (moon1[35, 23], gb => gb.PickupItem()),
                (moon3[28, 5], gb => gb.PickupItem()),
                (moon1[2, 3], gb => gb.PickupItem()),
                (moon1[3, 2], gb => gb.PickupItem()),
            },
            LogStart = startTile.PokeworldLink + "/",
            FoundCallback = state =>
            {
                Path p = new Path(state.Log, state.IGT.TotalSuccesses, state.WastedFrames, RNGSummary(state.IGT));
                Trace.WriteLine(p);
                results.Add(p);
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 5, states);
        results.CleanPrintAll();
        Elapsed("search");
    }
}
