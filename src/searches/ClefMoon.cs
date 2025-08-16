using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using static SearchCommon;
using static RbyIGTChecker<Blue>;

class ClefMoon
{
    // const string State = "basesaves/blue/manip/moonentrance.gqs";
    const string State = "basesaves/blue/manip/moonentrancebirdtm.gqs";
    // const string State = "basesaves/blue/manip/moonentrancenobird.gqs";
    // const string State = "basesaves/blue/manip/moonentrancenobirdtm2.gqs";
    const string CurrentPath = ""
    + "UUUUUUUUUUUUURARRRRRRUUUUUUURRRDDDDDDDDDDDRDDDDDDRRRRRRURRRUAUUUAUUUURLUUUUUUUUUUUULUUUUUUUULLLLDLLLLLLLLLLLLDDDDDDD"
    + "LALLALLALLALDD"
    + "RRRAUUAULAURDDADDLLL"
    + "RARRARRARRARUU"
    + "LDDDDDDLLLALLLLLLLLLLLUUAUUUUUUUAUUUUURRDDR"
    + "DDDDDDDDDDDDRRRRRRRRRRRRRRRAR"
    + "UUURRRRRDDRRRRRRRUURRRDDDDDDDDLLLLDDDDDDDADDLLLLLLLLLLLLLLLLL"
    ;

    public static void Search()
    {
        // Search(new RbyIntroSequence(RbyStrat.NoPal));
        // ClefairySearch(new RbyIntroSequence(), 16, 0);
        // ClefairySearch(16, 48);
        CheckIGT(new CheckIGTParameters() {
            StatePath = State,
            Intro = new RbyIntroSequence(),
            Path = CurrentPath + "LLLLUAUUUUAUUUUUULU",
            // Seconds = 60,
            MemeBall = gb => {
                return gb.Selectball(1);
                // gb.ClearText();
                // // gb.Press(Joypad.A | Joypad.Down, Joypad.A | Joypad.Right, Joypad.B, Joypad.A | Joypad.Right, Joypad.B, Joypad.A | Joypad.Down);
                // gb.Press(Joypad.A | Joypad.Down, Joypad.B, Joypad.A, Joypad.A | Joypad.Down);
                // return gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"];
            }
        });

        // CheckFile();

        // CheckIGT(new CheckIGTParameters() {
        //     StatePath = State,
        //     Intro = new RbyIntroSequence(),
        //     // Path = CurrentPath + "S_BLLALLLLLLRRRRRLUUUAUUUUULULURAUUUUUS_BAD", // 886/900 [51-5], 297/300 [52-56]
        //     Path = CurrentPath + "S_BLLALLLLLLRRRRRLLLUAUUUUUUUUURUUALUS_BUAR", // 760/780 [56-8], 299/300 [0-4]
        //     NumFrames = 13,
        //     Seconds = 60,
        //     StartFrame = 56,
        //     CheckDV = true,
        //     MemeBall = gb => {
        //         gb.ClearText();
        //         // gb.Press(Joypad.A | Joypad.Down, Joypad.A | Joypad.Right, Joypad.B, Joypad.A | Joypad.Right, Joypad.B, Joypad.A | Joypad.Down);
        //         gb.Press(Joypad.A | Joypad.Down, Joypad.B, Joypad.A, Joypad.A | Joypad.Down);
        //         return gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"];
        //     }
        // });

        // Blue gb = new Blue();
        // gb.LoadState(State);
        // // gb.Record("test");
        // new RbyIntroSequence().Execute(gb);
        // gb.CpuWriteBE<ushort>("wPartyMon1HP", 1);
        // RbyMap moon1 = gb.Maps[59];
        // RbyMap moon2 = gb.Maps[60];
        // RbyMap moon3 = gb.Maps[61];
        // gb.Execute(SpacePath(CurrentPath + "S_BLLALLLLLLRRRRRLLLUAUUUUUUUUURUUALUS_BUAR"),
        //     (moon1[34, 31], gb.PickupItem),
        //     (moon1[35, 23], gb.PickupItem),
        //     (moon3[28,  5], gb.PickupItem),
        //     (moon1[ 2,  3], gb.PickupItem),
        //     (moon1[ 3,  2], gb.PickupItem)
        // );
        // // gb.SaveState("basesaves/blue/manip/yoloballbirdredbar.gqs");
        // // gb.Dispose();
        // gb.ClearText();
        // gb.Press(Joypad.A | Joypad.Down);
        // FindYoloball(gb, (info, success) => { if(success) Trace.WriteLine(info + ": " + success); });
    }

    public static void CheckFile()
    {
        Blue[] gbs = MultiThread.MakeThreads<Blue>(16);
        Blue gb = gbs[0];
        RbyMap moon1 = gb.Maps[59];
        RbyMap moon2 = gb.Maps[60];
        RbyMap moon3 = gb.Maps[61];
        gb.LoadState(State);
        IGTResults states = Blue.IGTCheckParallel(gbs, new RbyIntroSequence(), 60, gb =>
            gb.Execute(SpacePath(CurrentPath),
                (moon1[34, 31], gb.PickupItem),
                (moon1[35, 23], gb.PickupItem),
                (moon3[28,  5], gb.PickupItem),
                (moon1[ 2,  3], gb.PickupItem),
                (moon1[ 3,  2], gb.PickupItem)
            ) == gb.OverworldLoopAddress
        );
        Paths paths = new Paths();
        foreach(string line in System.IO.File.ReadAllLines("paths.txt"))
        {
            string path = Regex.Match(line, @"/([LRUDSA_B]+) ").Groups[1].Value;
            int cost = int.Parse(Regex.Match(line, "cost: ([0-9]+)").Groups[1].Value);
            bool[] results = new bool[60];
            MultiThread.For(60, gbs, (gb, it) => {
                if(!states[it].Success) return;
                gb.LoadState(states[it].State);
                int adr = gb.Execute(SpacePath(path));
                results[it] = adr == gb.WildEncounterAddress && gb.EnemyMon.Species.Name == "CLEFAIRY" && gb.EnemyMon.Level == 12 && gb.EnemyMon.DVs == 0xffef;
            });
            int streak = 0, max = 0;
            for(int i = 0; i < 80; ++i)
            {
                if(results[i % 60]) streak++; else streak = 0;
                if(streak > max) max = streak;
            }
            Path p = new Path(path, max, cost);
            Trace.WriteLine(p);
            paths.Add(p);
        }
        paths.CleanPrintAll();
    }

    public static void MoonSearch(RbyIntroSequence intro, int numThreads = 16)
    {
        StartWatch();
        Trace.WriteLine(intro);
        Blue[] gbs = MultiThread.MakeThreads<Blue>(numThreads);
        Blue gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        // RbyMap route4 = gb.Maps[15];
        RbyMap moon1 = gb.Maps[59];
        RbyMap moon2 = gb.Maps[60];
        RbyMap moon3 = gb.Maps[61];

        gb.LoadState(State);
        // IGTResults states = Blue.IGTCheckParallel(gbs, intro, 60);
        IGTResults states = Blue.IGTCheckParallel(gbs, intro, 60, gb =>
            gb.Execute(SpacePath(CurrentPath),
                (moon1[34, 31], gb.PickupItem),
                (moon1[35, 23], gb.PickupItem),
                (moon3[28,  5], gb.PickupItem),
                (moon1[ 2,  3], gb.PickupItem),
                (moon1[ 3,  2], gb.PickupItem)
            ) == gb.OverworldLoopAddress
        );
        states[36].Success = false;
        states[37].Success = false;
        states = states.Purge();
        // RNGDebug(states); return;

        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A;
        RbyTile startTile = gb.Tile;
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
        var parameters = new DFParameters<Blue, RbyMap, RbyTile>()
        {
            MaxCost = 6,
            RNGSS = 55,
            RNGRange = 3,
            // MaxTurns = 20,
            SuccessSS = 55,
            // EndTiles = new RbyTile[] { moon1[5, 31] }, EndEdgeSet = 1,
            // EndTiles = new RbyTile[] { moon1[34, 31] }, EndEdgeSet = 2,
            // EndTiles = new RbyTile[] { moon1[35, 23] }, EndEdgeSet = 3,
            // EndTiles = new RbyTile[] { moon2[25, 9] }, EndEdgeSet = 3,
            // EndTiles = new RbyTile[] { moon3[28, 5] }, EndEdgeSet = 4,
            // EndTiles = new RbyTile[] { moon2[17, 11] }, EndEdgeSet = 4,
            // EndTiles = new RbyTile[] { moon1[17, 12] }, EndEdgeSet = 4,
            // EndTiles = new RbyTile[] { moon1[3, 3] }, EndEdgeSet = 5,
            // EndTiles = new RbyTile[] { moon3[21, 17] }, EndEdgeSet = 5,
            // EndTiles = new RbyTile[] { moon3[22, 17] }, EndEdgeSet = 5,
            EndTiles = new RbyTile[] { moon3[15, 31] }, EndEdgeSet = 5,
            // EndTiles = new RbyTile[] { moon3[10, 17] }, EndEdgeSet = 5,
            TileCallbacks = new (Tile<RbyMap, RbyTile>, Action<Blue>)[] {
                (moon1[34, 31], gb => gb.PickupItem()),
                (moon1[35, 23], gb => gb.PickupItem()),
                (moon3[28, 5], gb => gb.PickupItem()),
                (moon1[2, 3], gb => gb.PickupItem()),
                (moon1[3, 2], gb => gb.PickupItem()),
            },
            LogStart = startTile.PokeworldLink + "/",
            FoundCallback = state =>
            {
                Path p = new Path(state.Log, state.IGT.TotalRunning, state.WastedFrames, RNGSummary(state.IGT));
                Trace.WriteLine(p);
                results.Add(p);
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 5, states);
        results.CleanPrintAll();
        Elapsed("search");
    }

    public static void ClefairySearch(RbyIntroSequence intro, int numThreads = 16, int igtframe = 0)
    {
        StartWatch();
        Trace.WriteLine(intro);
        Blue[] gbs = MultiThread.MakeThreads<Blue>(numThreads);
        Blue gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        RbyMap moon1 = gb.Maps[59];
        RbyMap moon2 = gb.Maps[60];
        RbyMap moon3 = gb.Maps[61];

        gb.LoadState(State);
        IGTState state = gb.IGTCheck(intro, 60, () =>
            gb.Execute(SpacePath(CurrentPath),
                (moon1[34, 31], gb.PickupItem),
                (moon1[35, 23], gb.PickupItem),
                (moon3[28,  5], gb.PickupItem),
                (moon1[ 2,  3], gb.PickupItem),
                (moon1[ 3,  2], gb.PickupItem)
            ) == gb.OverworldLoopAddress
        )[igtframe];

        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A | Action.StartB | Action.PokedexFlash;
        RbyTile startTile = gb.Tile;
        RbyTile[] blockedTiles = { moon3[16, 31] };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, moon3[10, 17], actions, blockedTiles);
        for(int x = 7; x <= 15; ++x) { moon3[x, 31].RemoveEdge(0, Action.Down); }
        // Pathfinding.DebugDrawEdges(gb, moon3, 0);

        var parameters = new SFParameters<Blue, RbyMap, RbyTile>()
        {
            // MaxCost = 350, // 200
            // EncounterCallback = gb => gb.EnemyMon.Species.Name == "CLEFAIRY"
            //     && gb.EnemyMon.Level == 12
            //     && gb.Tile.X < 12
            //     && gb.EnemyMon.DVs.Attack >= 14 && gb.EnemyMon.DVs.Defense >= 14 && gb.EnemyMon.DVs.Speed >= 14 && gb.EnemyMon.DVs.Special >= 14
            //     ,
            MaxCost = 10,
            EncounterCallback = gb => gb.EnemyMon.Species.Name == "PARAS"
                && gb.EnemyMon.Level == 12
                && gb.Tile.X < 12
                && gb.Tile.Y < 22
                ,
            LogStart = startTile.PokeworldLink + "/",
            FoundCallback = (state, gb) =>
            {
                Trace.WriteLine(state.Log + " " + gb.EnemyMon.Species.Name + " L" + gb.EnemyMon.Level + " dvs: " + gb.EnemyMon.DVs + " cost: " + state.WastedFrames);
            }
        };

        SingleFrameSearch.StartSearch(gbs, parameters, startTile, 0, state);
        Elapsed("search");
    }
}
