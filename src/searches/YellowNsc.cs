using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

using static SearchCommon;
using static RbyIGTChecker<Yellow>;

class YellowNsc
{
    public static void THEmanip(int mappath)
    {
        int numFrames = 60*60;
        int numThreads = 16;
        RbyIntroSequence intro = new RbyIntroSequence();
        YellowCb[] gbs = MultiThread.MakeThreads<YellowCb>(numThreads);
        YellowCb gb = gbs[0];

        gb.LoadState("basesaves/yellow/themanip.gqs");
        gb.HardReset();
        if(numThreads == 1)
            gb.Record("test");
        intro.ExecuteUntilIGT(gb);
        byte[] igtState = gb.SaveState();

        var full = new List<string>();
        var results = new Dictionary<string, int>();

        MultiThread.For(numFrames, gbs, (gb, f) =>
        {
            if(f % 60 < 14 || f % 60 > 17) return;
            string info;
            gb.LoadState(igtState);
            gb.CpuWrite("wPlayTimeMinutes", 8);
            gb.CpuWrite("wPlayTimeSeconds", (byte) (f / 60));
            gb.CpuWrite("wPlayTimeFrames", (byte) (f % 60));

            intro.ExecuteAfterIGT(gb);

            var npctracker = new NpcTracker<YellowCb>(gb.CallbackHandler);
            gb.Execute(SpacePath("LLLLL") + " S_A_B_S " + SpacePath("RRRRRRUUUUUUUUAUUAUUURARU" + "URRRRUUAUURLLALLLDADUAURUA"));
            if(gb.XCoord == 3 && gb.YCoord == 1)
            {
                gb.AdvanceFrames(10 + mappath, Joypad.A);
                gb.ClearText(Joypad.A);
                gb.Execute(SpacePath("RRRDRDDDLDL"));
                for(int i = 0; i < 6; ++i) { gb.RunUntil(gb.SYM["VBlank"] + 1); gb.RunFor(1); }
                gb.Execute(SpacePath("RDLLLD"));
                gb.RunUntil("JoypadOverworld");
                info = $"girl:{npctracker.GetMovement((44, 2))}, bird:{npctracker.GetMovement((44, 3))}, man:{npctracker.GetMovement((44, 1))} -> {gb.CpuRead(0xC331):X}";
                if(gb.CpuRead(0xC331) == 0xFE)
                {
                    // gb.SaveState("basesaves/yellow/postthe_f"+f+"_p"+mappath+".gqs");
                    if(gb.Execute(SpacePath("LLAUUUUULUUUUUUUUUUUUUUULUURDLUUUUULLLUUUUURRRRAUUUUU")) == gb.WildEncounterAddress)
                        info += ", " + gb.EnemyMon.Species.Name + " L" + gb.EnemyMon.Level + " " + gb.Tile;
                    else
                        info += ", no enc " + gb.Tile;
                }
            }
            else info = $"girl:{npctracker.GetMovement((44, 2))}, bird:{npctracker.GetMovement((44, 3))}, fail at {gb.XCoord},{gb.YCoord}";

            lock(results)
            {
                full.Add($"{f / 60,2} {f % 60,2}: {info}");
                if(!results.ContainsKey(info))
                    results.Add(info, 1);
                else
                    results[info]++;
            }
        });
        full.Sort();
        foreach(string line in full)
            Trace.WriteLine(line);
        Trace.WriteLine("");
        // foreach(var kv in results)
        //     Trace.WriteLine(kv.Key + ": " + kv.Value);
    }

    public static void RecordTHEmanip(int mappath)
    {
        YellowCb gb = new YellowCb();

        ulong lasttime = 0;
        gb.CallbackHandler.SetCallback(gb.SYM["VBlank"], gb => {
            if(gb.CpuReadBE<uint>("wPlayTimeMaxed") != 0) {
                // Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} {gb.CpuRead("hRandomAdd"):x2}{gb.CpuRead("hRandomSub"):x2}");
                if(gb.EmulatedSamples - lasttime > 40000) Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} " + (gb.EmulatedSamples - lasttime) + " +" + (float)(gb.EmulatedSamples - lasttime - GameBoy.SamplesPerFrame) / GameBoy.SamplesPerFrame);
                lasttime = gb.EmulatedSamples;
            }
        });

        gb.LoadState("basesaves/yellow/themanipdebutc.gqs");
        gb.Record("test");
        gb.HardReset();
        new RbyIntroSequence().Execute(gb);

        gb.Execute(SpacePath("LLLLL") + " S_A_B_S " + SpacePath("RRRRRRUUUUUUUUAUUAUUURARU" + "URRRRUUAUURLLALLLDADUAURUA"));
        gb.AdvanceFrames(10 + mappath, Joypad.A);
        gb.ClearText(Joypad.A);
        gb.Execute(SpacePath("RRRDRDDDLDL"));
        for(int i = 0; i < 6; ++i) { gb.RunUntil(gb.SYM["VBlank"] + 1); gb.RunFor(1); }
        gb.Execute(SpacePath("RDLLLD"));
        gb.RunUntil("JoypadOverworld");
        Trace.WriteLine($"-> {gb.CpuRead(0xC331):X}");
    }

    public static void OldManips()
    {
        CheckIGT(State, new RbyIntroSequence(), OldForest, null, 3600);
        YellowCb gb = new YellowCb();
        ulong lasttime = 0;
        gb.CallbackHandler.SetCallback(gb.SYM["VBlank"], gb => {
            if(gb.CpuReadBE<uint>("wPlayTimeMaxed") != 0) {
                // Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} {gb.CpuRead("hRandomAdd"):x2}{gb.CpuRead("hRandomSub"):x2}");
                if(gb.EmulatedSamples - lasttime > 40000) Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} " + (gb.EmulatedSamples - lasttime) + " +" + (float)(gb.EmulatedSamples - lasttime - GameBoy.SamplesPerFrame) / GameBoy.SamplesPerFrame);
                lasttime = gb.EmulatedSamples;
            }
        });
        gb.LoadState(State);
        gb.Record("test");
        gb.HardReset();
        new RbyIntroSequence().Execute(gb);
        gb.Execute(SpacePath(OldForest), (gb.Maps[51][25, 12], gb.PickupItem));

        CheckIGT(new CheckIGTParameters() {
            StatePath = "basesaves/yellow/nscpidgeotto_p4.gqs",
            Intro = new RbyIntroSequence(),
            Path = OldPidgeotto,
            NumFrames = 3600,
            MemeBall = Down,
        });
        gb.LoadState("basesaves/yellow/nscpidgeotto_r2.gqs");
        gb.Record("test");
        gb.HardReset();
        new RbyIntroSequence().Execute(gb);
        gb.Execute(SpacePath(OldPidgeotto));
        gb.ClearText();
        gb.Press(Joypad.A, Joypad.Select, Joypad.A);
        gb.ClearText(Joypad.A, 3);
        gb.Execute(SpacePath("LLLLLUUU"));
    }

    const string BasePath = "RUAUUUUUUULLLLLURUUUUUUUUUURRUURARRRRRRUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUULLLLLALLADDADDDDALLADLLLUUUUUUUUUAUUUULLLLL";
    const string OldForest = BasePath + "LDDDDADDDDDDDDDDADDDDADDLL";
    const string OldPidgeotto = "SLLULLUUUU";
    const string State = "basesaves/yellow/nscforest.gqs";
    const string StateR4 = "basesaves/yellow/nscforest_r4_1d.gqs";
    const string StateP4 = "basesaves/yellow/nscforest_p4_7d.gqs";
    static Func<Yellow, bool> Down = gb => { gb.ClearText(Joypad.B); gb.Press(Joypad.A, Joypad.Down, Joypad.A); gb.ClearText(Joypad.A, 1); return gb.BattleMon.HP == 0; };
    static Func<Yellow, bool> Select = gb => { gb.ClearText(Joypad.B); gb.Press(Joypad.A, Joypad.Select, Joypad.A); gb.ClearText(Joypad.A, 1); return gb.BattleMon.HP == 0; };

    public static void Forest()
    {
        YellowCb gb = new YellowCb();
        ulong lasttime = 0;
        gb.CallbackHandler.SetCallback(gb.SYM["VBlank"], gb => {
            if(gb.CpuReadBE<uint>("wPlayTimeMaxed") != 0) {
                if(gb.EmulatedSamples - lasttime > 40000) Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} " + (gb.EmulatedSamples - lasttime) + " +" + (float)(gb.EmulatedSamples - lasttime - GameBoy.SamplesPerFrame) / GameBoy.SamplesPerFrame);
                lasttime = gb.EmulatedSamples;
            }
        });
        gb.LoadState(StateR4);
        gb.Record("test");
        gb.HardReset();
        new RbyIntroSequence().Execute(gb);
        gb.Execute(SpacePath(BasePath + "LLDDDDADDADDADDADDDDARDDALDDDLLLLAUULAUU"), (gb.Maps[51][25, 12], gb.PickupItem));
        gb.ClearText();
        gb.Press(Joypad.A, Joypad.Down, Joypad.A);
        gb.ClearText(Joypad.A, 4);
        gb.Execute(SpacePath("LLLLLUUU"));


        var p = new CheckIGTParameters() {
            Intro = new RbyIntroSequence(),
            Path = BasePath + "LLDDDDADDADDADDADDDDARDDALDDDLLLLAUULAUU",
            NumFrames = 3,
            StartFrame = 52,
            Seconds = 60,
            // StatePath = State, MemeBall = Select, NameLength = 1,
            StatePath = StateR4, MemeBall = Down, NameLength = 1,
            // StatePath = StateP4, MemeBall = Select, NameLength = 7,
        };
        CheckIGT(p);
    }

    public static void CheckFile()
    {
        RbyIntroSequence intro = new RbyIntroSequence();
        Yellow[] gbs = MultiThread.MakeThreads<Yellow>(16);
        Yellow gb = gbs[0];
        gb.LoadState(State);
        foreach(string line in System.IO.File.ReadAllLines("paths.txt"))
        {
            string path = Regex.Match(line, @"/([LRUDSA_B]+) ").Groups[1].Value;
            Trace.WriteLine(path);
            IGTResults states = Yellow.IGTCheckParallel(gbs, new RbyIntroSequence(), 60, gb =>
                gb.Execute(SpacePath(BasePath + path), (gb.Maps[51][25, 12], gb.PickupItem)) == gb.WildEncounterAddress
                && gb.Tile == gb.Maps[51][1, 18]
                && gb.EnemyMon.Species.Name == "PIDGEOTTO"
            );
            var p = new CheckIGTParameters() {
                Intro = new RbyIntroSequence(),
                Path = BasePath + path,
                Verbose = Verbosity.Nothing,
                NumThreads = 20,
            };
            for(int f1 = 0; f1 < 60; ++f1)
            {
                int f2 = (f1 + 1) % 60;
                int f3 = (f1 + 2) % 60;
                if(states[f1].Success && states[f2].Success && states[f3].Success)
                {
                    for(int i = 0; i < 4; ++i)
                    {
                        if(i == 0 || i == 1) p.StatePath = StateP4; else p.StatePath = StateR4;
                        if(i == 0 || i == 2) p.MemeBall = Down; else p.MemeBall = Select;
                        for(int name = 1; name <= 10; ++name)
                        {
                            p.NameLength = name;
                            p.StartFrame = f2;
                            p.NumFrames = 1;
                            p.Seconds = 20;
                            p.FullResults = null;
                            if(CheckIGT(p) >= 5)
                            {
                                p.StartFrame = f1;
                                p.NumFrames = 3;
                                p.Seconds = 60;
                                p.FullResults = new List<IGTResult>();
                                int total = CheckIGT(p);
                                int clusters = 0;
                                for(int j = 0; j < 60; ++j)
                                    if(p.FullResults[j * 3].Yoloball && p.FullResults[j * 3 + 1].Yoloball && p.FullResults[j * 3 + 2].Yoloball)
                                        ++clusters;
                                if(clusters >= 10) Trace.WriteLine("cluster: " + f1 + " " + f2 + " " + f3 + (i == 0 || i == 1 ? " p" : " r") + (i == 0 || i == 2 ? " down" : " sel") + name + " total: " + total + " full: " + clusters);
                            }
                        }
                    }
                }
            }
        }
    }

    public static void SearchForest(int numThreads = 16)
    {
        StartWatch();

        Yellow[] gbs = MultiThread.MakeThreads<Yellow>(numThreads);
        Yellow gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        gb.LoadState(State);
        IGTResults states = Yellow.IGTCheckParallel(gbs, new RbyIntroSequence(), 60, gb =>
            gb.Execute(SpacePath(BasePath), (gb.Maps[51][25, 12], gb.PickupItem)) == gb.OverworldLoopAddress
        );
        for(int i = 0; i < states.Length; ++i) states.IGTs[i].Running = states.IGTs[i].Success;

        RbyMap forest = gb.Maps[51];
        // forest.Sprites.Remove(25, 11);
        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A | Action.StartB;
        RbyTile startTile = gb.Tile;
        RbyTile[] endTiles = { forest[1, 18] };
        // RbyTile[] blockedTiles = {
        //     forest[26, 12],
        //     forest[16, 10], forest[18, 10],
        //     forest[16, 15], forest[18, 15],
        //     forest[11, 15], forest[12, 15],
        //     forest[11, 6], forest[12, 6],
        //     forest[6, 6], forest[8, 6],
        //     forest[6, 15], forest[8, 15],
        //     forest[2, 19], forest[1, 18]
        // };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, forest[1, 19], actions);
        // forest[25, 12].RemoveEdge(0, Action.A);
        // forest[25, 13].RemoveEdge(0, Action.A);
        for(int y = 4; y <= 12; ++y)
        {
            forest[7, y].RemoveEdge(0, Action.Left);
            forest[8, y].RemoveEdge(0, Action.Left);
        }
        forest[1, 19].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = forest[1, 18], NextEdgeset = 0, Cost = 0 });

        var parameters = new DFParameters<Yellow, RbyMap, RbyTile>()
        {
            MaxCost = 50,
            SuccessSS = 3,
            EndTiles = endTiles,
            // TileCallbacks = new (Tile<RbyMap, RbyTile>, Action<Yellow>)[] { (forest[25, 12], gb => gb.PickupItem()) },
            EncounterCallback = gb => gb.Tile == endTiles[0] && gb.EnemyMon.Species.Name == "PIDGEOTTO",
            LogStart = startTile.PokeworldLink + "/",
            FoundCallback = state =>
            {
                int l = state.IGT.Length;
                for(int i = 0; i < l; ++i)
                {
                    if(state.IGT[i].Success && state.IGT[(i + 1) % l].Success && state.IGT[(i + 2) % l].Success && state.IGT[(i + 3) % l].Success)
                    {
                        string successframes = "";
                        for(int j = 0; j < l; ++j) if(state.IGT[j].Success) successframes += " " + state.IGT[j].IGTFrame;
                        Trace.WriteLine(state.Log + " s: " + state.IGT.TotalSuccesses + " c: " + state.WastedFrames + " frames:" + successframes);
                        break;
                    }
                }
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states);
        Elapsed("search");
    }

    public static void SearchPostThe(int numThreads = 15)
    {
        StartWatch();

        Yellow[] gbs = MultiThread.MakeThreads<Yellow>(numThreads);
        Yellow gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        IGTResults states = new IGTResults(15);
        int s = 0;
        for(int f = 14; f <= 17; ++f)
        {
            for(int p = 1; p <= 4; ++p)
            {
                if(!(p == 1 && f == 16))
                {
                    gb.LoadState("basesaves/yellow/postthe_f" + f + "_p" + p + ".gqs");
                    gb.AdvanceFrames(12);
                    gb.RunUntil("JoypadOverworld");
                    states[s++] = new IGTState(gb, true, s);
                }
            }
        }

        RbyMap route2 = gb.Maps[13];
        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A;
        RbyTile startTile = gb.Tile;
        RbyTile[] endTiles = { route2[8, 47] };
        RbyTile[] blockedTiles = {
            route2[4, 51],
            route2[5, 51],
            route2[6, 51],
            route2[7, 51],
        };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, endTiles[0], actions, blockedTiles);

        var parameters = new DFParameters<Yellow, RbyMap, RbyTile>()
        {
            MaxCost = 4,
            SuccessSS = 15,
            EndTiles = endTiles,
            TileCallbacks = new (Tile<RbyMap, RbyTile>, Action<Yellow>)[] { (route2[7, 60], gb => gb.Execute("R D L U U")) },
            LogStart = startTile.PokeworldLink + "/",
            FoundCallback = state =>
            {
                Trace.WriteLine(state.Log + " s: " + state.IGT.TotalRunning + " c: " + state.WastedFrames);
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states);
        Elapsed("search");
    }
}
