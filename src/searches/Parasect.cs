using System.Diagnostics;

using static RbyIGTChecker<Red>;
using static SearchCommon;

class Parasect
{
    const string State = "basesaves/red/manip/parasect_p1topsign.gqs";
    // const string State = "basesaves/red/manip/parasect_p1beforesign.gqs";
    // const string State = "basesaves/red/manip/parasect_pp.gqs";
    // const string State = "basesaves/red/manip/parasect_ph.gqs";

    public static void Check()
    {
        bool MemeBall(Red gb)
        {
            gb.ClearText();
            gb.Press(Joypad.A | Joypad.Down, Joypad.A | Joypad.Right, Joypad.B, Joypad.Select, Joypad.A | Joypad.Right, Joypad.B, Joypad.A | Joypad.Down);
            return gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"];
        }
        void Bide(Red gb, Joypad j)
        {
            gb.ClearText(j);
            gb.Press(Joypad.A, Joypad.Down, Joypad.A);
            gb.ClearText(Joypad.B);
            gb.Press(Joypad.A);
            gb.ClearText(Joypad.B);
            gb.Press(Joypad.A);
            gb.ClearText(Joypad.B);
            gb.Press(Joypad.A);
            gb.ClearText(Joypad.B);
        }

        // CheckIGT("basesaves/red/manip/parasect_p1topsign.gqs", new RbyIntroSequence(), "RU" + "UUUUUUUUUUUUURRRRRRRUUUUUUURRRR" + "DDDDDDDDDDDDLLLLLLLLLLLL" + "URURUUURDRDRRRRRRAUU" + "DDDDLALLLLLLLLLL", "PARAS", 60, true);
        // CheckIGT("basesaves/red/manip/parasect_p1topsign.gqs", new RbyIntroSequence(), "RUUUUUUUUUUUUUURRRRRRRUUUUUUUUUURRRRDDD" + "DDDDDDDDDDDDLLLLLLLLLLLL" + "RRRARUURRAURRRAURU" + "LADDLLLLLLLLADDAD", "PARAS", 60, true, false, Verbosity.Full, false, -1, PotionBackout);
        // CheckIGT("basesaves/red/manip/parasect_p1topsign.gqs", new RbyIntroSequence(), "RUUUUUUUUUUUUUURRRRRRRUUUUUUUUUURRRRDDD" + "DDDDDDDDDDDDLLLLLLLLLLLL" + "RRRARUURRAURRRAURU" + "LADDLLLLLLLLADDAD", "PARAS", 60, true, false, Verbosity.Full, true, -1);
        CheckIGT("basesaves/red/manip/parasect_p1topsign.gqs", new RbyIntroSequence(), "RRLUUUUUUUUUUUUUURRRRRRRUUUUUUUUURRRRDD" + "DDDDDDDDDDLADDALLALLALLLLLLL" + "RRURURRRRRRARUAUU" + "LADDALLLLLDDDL", "PARAS", 60, true, false, Verbosity.Full, false, -1);
        // CheckIGT("basesaves/red/manip/parasect_p1topsign.gqs", new RbyIntroSequence(), "RRLUUUUUUUUUUUUUURRRRRRRUUUUUUUUURRRRDD" + "DDDDDDDDDDLADDALLALLALLLLLLL" + "RRURURRRRRRARUAUU" + "LADDALLLLLDDDL", "PARAS", 60, true, false, Verbosity.Full, true, -1, MemeBall);
        // CheckIGT("basesaves/red/manip/parasect_pp.gqs", new RbyIntroSequence(RbyStrat.Pal), "RRRRRRRRRRRRUUUUUUUUUUUAUDADDADDDDDDDDDDDDD", "PARAS", 60);
        // CheckIGT("basesaves/red/manip/parasect_pp.gqs", new RbyIntroSequence(RbyStrat.Pal), "RRRRRRRRRRRRUUUUUUUUUUUUDDDDDDADDADDDDDDDD", "GEODUDE", 60, true, false, Verbosity.Full, false, -1, gb => { Bide(gb, Joypad.None); return gb.EnemyMon.HP == 0; }, 0, 1, 1);
        // CheckIGT("basesaves/red/manip/parasect_pp.gqs", new RbyIntroSequence(RbyStrat.Pal), "RRRRRRRRRRRRUUUUUUUUUUUUDDDDDDADDADDDDDDDD", "GEODUDE", 60, true, false, Verbosity.Full, false, -1, gb => { Bide(gb, Joypad.B); return gb.EnemyMon.HP == 0; }, 0, 1, 16);
        // CheckIGT("basesaves/red/manip/parasect_ph.gqs", new RbyIntroSequence(), "DDDDDADDDDDDDRRRRRRRRRRRRRRRR" + "RRUUURRRRDDRRRRRRUURRRDDDDDDDDDDLLLLDDDDDDDLLLALLALLLLLLLLLLLLLLLLLLUUUUAUUUUUUUUUUURDLUR", "GEODUDE", 60, true);
        // CheckIGT("basesaves/red/manip/parasect_ph.gqs", new RbyIntroSequence(), "DDDDDDDDDDDADRRRRRRRRRRRRRRRRRUURURRRRDDRRRRRUAURRRRDDDDDDDDLLLLDDDDDDDDDLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUULUUUU", "PARAS", 60);
        // CheckIGT("basesaves/red/manip/parasect_pn.gqs", new RbyIntroSequence(RbyStrat.PalHold), "LLLLLLLLLLDDDDARRAD" + "RRR", "PARAS", 60);

        // Red r = new Red();
        // r.LoadState("basesaves/red/manip/parasect_test.gqs");
        // r.ClearText();
        // r.Press(Joypad.A | Joypad.Down);
        // FindYoloball(r, (info, success) => { Trace.WriteLine(info + ": " + success); });
        // return;

        // for(RbyStrat pal = RbyStrat.NoPal; pal <= RbyStrat.NoPal; ++pal)
        //     SearchParas(new RbyIntroSequence(pal));
        // CheckFile();
    }

    public static void CheckFile()
    {
        Paths paths = new Paths();
        RbyIntroSequence intro = new RbyIntroSequence();
        foreach(string line in System.IO.File.ReadAllLines("paths.txt"))
        {
            string path = System.Text.RegularExpressions.Regex.Match(line, @"/([LRUDSA_B]+) ").Groups[1].Value;
            if(path == "") { intro = new RbyIntroSequence(line); continue; }
            // int cost = int.Parse(System.Text.RegularExpressions.Regex.Match(line, "cost: ([0-9]+)").Groups[1].Value);
            int s = CheckIGT(new CheckIGTParameters() {
                StatePath = State,
                Intro = intro,
                Path = path,
                NumFrames = 60,
                // TargetPoke = "GEODUDE",
                TargetPoke = "PARAS",
                Verbose = Verbosity.Nothing,
                // MemeBall = gb => { Bide(gb, Joypad.None); return gb.EnemyMon.HP == 0; },
                MemeBall = gb => { return gb.EnemyMon.Species.Name == "PARAS" && gb.EnemyMon.Level == 12 && gb.EnemyMon.DVs == 0xffef; },
            });
            SearchCommon.Path p = new SearchCommon.Path(path, s, 0, intro[0].LogString());
            Trace.WriteLine(p);
            paths.Add(p);
        }
        paths.CleanPrintAll("https://gunnermaniac.com/pokeworld?local=59#13/27/");
    }
    
    public static void Search(RbyIntroSequence intro, int numThreads = 16, int numFrames = 60, int success = 54, int cost = 6)
    // void Search(RbyIntroSequence intro, int numThreads = 10, int numFrames = 10, int success = 8, int cost = 6)
    {
        StartWatch();
        Trace.WriteLine(intro);

        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        gb.LoadState(State);
        IGTResults states = Red.IGTCheckParallel(gbs, intro, numFrames);

        RbyMap moon1 = gb.Maps[59];
        RbyMap moon2 = gb.Maps[60];
        RbyMap moon3 = gb.Maps[61];
        // RbyMap route4 = gb.Maps[15];
        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A | Action.StartB;
        RbyTile startTile = gb.Tile;
        // RbyTile[] blockedTiles = { moon2[6, 15], moon2[7, 15], moon3[33, 23], moon3[34, 23], moon3[35, 23], moon3[36, 23] };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, moon1[25, 32], actions);
        // Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, moon3[10, 17], actions, blockedTiles);

        var parameters = new DFParameters<Red, RbyMap, RbyTile>()
        {
            MaxCost = cost,
            SuccessSS = success,
            // MaxTurns = 13 + 4,
            EncounterCallback = gb => gb.EnemyMon.Species.Name == "GEODUDE" && gb.EnemyMon.Level == 10 && gb.Tile.Y >= 28,
            // EncounterCallback = gb => gb.EnemyMon.Species.Name == "PARAS" && gb.Tile.Y >= 28 && gb.Yoloball(1),
            // EncounterCallback = gb => gb.EnemyMon.Species.Name == "GEODUDE" && gb.EnemyMon.Level == 10 && gb.Tile.X == 10 && gb.Tile.Y <= 19,
            // EncounterCallback = gb => gb.EnemyMon.Species.Name == "PARAS" && gb.Tile.X == 10 && gb.Tile.Y <= 19,
            FoundCallback = state =>
            {
                Trace.WriteLine("https://gunnermaniac.com/pokeworld?local=60#13/27/" + new SearchCommon.Path(state.Log, state.IGT.TotalSuccesses, state.WastedFrames));
                // Trace.WriteLine("https://gunnermaniac.com/pokeworld?local=61#5/5/" + new SearchCommon.Path(state.Log, CheckIGT(State, intro, state.Log, "PARAS", 60, false, false, Verbosity.Nothing), state.WastedFrames));
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states);
        Elapsed("search");
    }
    
    public static void SearchParas(RbyIntroSequence intro, int numThreads = 16, int cost = 104+20)
    {
        StartWatch();
        Trace.WriteLine(intro);

        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];

        gb.LoadState(State);
        IGTResults states = Red.IGTCheckParallel(gbs, intro, 60);
        // IGTResults states = Red.IGTCheckParallel(gbs, intro, 60, gb => gb.Execute(SpacePath("RU" + "UUUUUUUUUUUUURRRRRRRUUUUUUURRRAR" + "DDDDDDDDDDDDLLLLLLLLLLLL")) == gb.OverworldLoopAddress);
        // IGTResults states = Red.IGTCheckParallel(gbs, intro, 60, gb => gb.Execute(SpacePath("RRUUUUUUUUUUUUUUUURRRRRRUUUUUUURRRRARDDDDDDDDDDDDLLLLLLLLLLLL")) == gb.OverworldLoopAddress);
        // RNGDebug(states); return;
        if(numThreads == 1) gb.Record("test");

        RbyMap route4 = gb.Maps[15];
        RbyMap moon1 = gb.Maps[59];
        RbyMap moon2 = gb.Maps[60];
        RbyMap moon3 = gb.Maps[61];
        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A | Action.StartB;
        RbyTile startTile = gb.Tile;
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, moon3[25, 22], actions, moon3[24, 22]);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, route4[18, 6], actions);
        route4[18, 6].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = moon1[14, 35], NextEdgeset = 0, Cost = 0 });
        moon3[25, 23].RemoveEdge(0, Action.A);
        moon3[25, 23].GetEdge(0, Action.Up).NextEdgeset = 1;
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 1, moon3[15, 27], actions);
        moon3[25, 22].RemoveEdge(1, Action.A);
        // Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, moon3[10, 17], actions, blockedTiles);
        // Pathfinding.DebugDrawEdges(gb, gb.Maps[59], 0); return;

        Paths paths = new Paths();
        // string link = "https://gunnermaniac.com/pokeworld?local=59#12/38/";
        string link = "https://gunnermaniac.com/pokeworld?local=61#15/27/";
        var parameters = new SFParameters<Red, RbyMap, RbyTile>()
        {
            MaxCost = cost,
            // MaxTurns = 13 + 4,
            TileCallback = (moon3[25, 22], gb => gb.PickupItem()),
            // EndTiles = new RbyTile[] { moon3[16, 27] }, EndEdgeSet = 0,
            EndTiles = new RbyTile[] { moon3[15, 26], moon3[16, 27] }, EndEdgeSet = 1,
            EncounterCallback = gb => gb.EnemyMon.Species.Name == "PARAS" && gb.EnemyMon.Level == 12 && gb.Tile.X <= 18
                && gb.EnemyMon.DVs.Attack >= 14 && gb.EnemyMon.DVs.Defense >= 14 && gb.EnemyMon.DVs.Speed >= 14
                ,
            FoundCallback = (state, gb) =>
            {
                if(state.EdgeSet == 0) return;
                SearchCommon.Path p = new SearchCommon.Path(state.Log, 0, state.WastedFrames, gb.EnemyMon.DVs.ToString());
                Trace.WriteLine(link + p);
                paths.Add(p);
            }
        };

        SingleFrameSearch.StartSearch(gbs, parameters, startTile, 0, states[0]);
        Elapsed("search");
        paths.CleanPrintAll(link);
    }
}
