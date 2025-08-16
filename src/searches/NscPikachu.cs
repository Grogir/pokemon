using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using static SearchCommon;
using static RbyIGTChecker<BlueCb>;

class NscPikachu
{
    static RbyIntroSequence Intro = new RbyIntroSequence(RbyStrat.PalHold);
    const string State = "basesaves/blue/manip/nscpikachu.gqs";
    const string Forest = "UUUURRRRUUUULLULLLU" + "URUUUUUU" + "UUUURRRRRRRRUUUUUUUUUUUUUUUUUAUUUUUUUUUUURUUUUUULLLLLLALLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDLDDDLLLLUUULU";

    public static void DebugStack(GameBoy gb, int n = 20)
    {
        int addr = gb.PC;
        if(addr >= 0x4000) addr |= gb.CpuRead("hLoadedROMBank") << 16;
        Console.WriteLine(gb.SYM.Contains(addr) ? gb.SYM[addr] : $"{addr:x5}");

        for(int i = 0; gb.SP + i <= 0xdfff; i += 2)
        {
            string line = "";
            addr = gb.CpuReadLE<ushort>(gb.SP + i);
            if(addr < 3) continue;
            if(addr < 0x4000)
            {
                if(gb.ROM[addr - 3] == 0xcd && gb.SYM.Contains(addr)) line += " " + gb.SYM[addr];
            }
            else
            {
                for(int b = 1; b <= 0x2b; ++b)
                {
                    int addr2 = addr | b << 16;
                    if(gb.ROM[addr2 - 3] == 0xcd && gb.SYM.Contains(addr2)) line += $" {addr2:x5} {gb.SYM[addr2]}";
                }
            }
            if(line != "") Console.WriteLine($"{i,2} {gb.SP + i:x4} {addr:x4}{line}");
        }
    }

    public static void Check()
    {
        Check(1, "UUUULLLUUUAUURRRRAUUAUUUUULLLLLUURUUUUAUUU", 2);

        Check(1);
        Check(2);
        Check(3);
    }

    public static void Record()
    {
        BlueCb gb = new BlueCb();
        gb.LoadState("basesaves/blue/manip/nscf2_2.gqs");
        gb.Record("test");
        ulong lasttime = 0;
        gb.CallbackHandler.SetCallback(gb.SYM["VBlank"], gb => {
            if(gb.CpuReadBE<uint>("wPlayTimeMaxed") != 0) {
            Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} {gb.CpuRead("hRandomAdd"):x2}{gb.CpuRead("hRandomSub"):x2}");
            if(gb.EmulatedSamples - lasttime > 40000) Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} " + (gb.EmulatedSamples - lasttime) + " +" + (float)(gb.EmulatedSamples - lasttime - GameBoy.SamplesPerFrame) / GameBoy.SamplesPerFrame);
            lasttime = gb.EmulatedSamples;
            }
        });
        RbyStrat.GfSkip.Execute(gb);
        RbyStrat.Hop0.Execute(gb);
        RbyStrat.Title0.Execute(gb);
        RbyStrat.Continue.Execute(gb);
        RbyStrat.Continue.Execute(gb);
        gb.Execute(SpacePath(Forest));
        gb.ClearText();
        gb.Press(Joypad.A, Joypad.Down, Joypad.A);
        gb.ClearText(Joypad.B, 5);
        gb.Execute(SpacePath("LLLLAUUUUUUUUUUUUUUUUUUUUUUUULLUUUUUUUUUUUUU"));
        gb.Execute(SpacePath("UUUULLLUUUAUURRRRAUUAUUUUULLLLLUURUUUUAUUU")); //f2e
        gb.ClearText(1);
        gb.Inject(Joypad.None); gb.AdvanceFrames(1); gb.Inject(Joypad.B); // 2nd frame
        gb.AdvanceFrame(Joypad.B);
        gb.ClearText(Joypad.B);
        gb.Press(Joypad.A, Joypad.Select, Joypad.A);
        gb.ClearText(Joypad.A);
        gb.Press(Joypad.A, Joypad.Select, Joypad.A);
        gb.ClearText(Joypad.B);
        gb.Press(Joypad.A, Joypad.Select, Joypad.A);
        gb.ClearText(Joypad.B, 99, gb.SYM["EnemyCanExecuteMove"] + 0x7);
        gb.ClearText(Joypad.A);
        gb.Press(Joypad.A, Joypad.Select, Joypad.A);
        gb.ClearText(Joypad.A, 2);
    }

    public static void Test()
    {
        Blue gb = new Blue();
        // gb.Show();
        // byte[] state = System.IO.File.ReadAllBytes("basesaves/blue/manip/nscspeedtie.gqs");
        byte[] state = System.IO.File.ReadAllBytes("basesaves/blue/manip/nscmash.gqs");
        // byte[] state = System.IO.File.ReadAllBytes("basesaves/blue/manip/nsccrash.gqs");
        for(int i = 0; i < 1000; ++i)
        {
            gb.LoadState(state);
            gb.AdvanceFrame();
            state = gb.SaveState();
            string igt = $"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2}";
            gb.AdvanceFrame(Joypad.A);
            // gb.Hold(Joypad.A, gb.SYM["ManualTextScroll"]); if(gb.BattleMon.HP == 19 && gb.EnemyMon.HP == 0) // test speedtie
            if(gb.RunUntil("JoypadOverworld", "WaitForTextScrollButtonPress") == gb.SYM["WaitForTextScrollButtonPress"]) // test mash
            // gb.AdvanceFrames(50); if(gb.Map.Id == 118) // test crash
                Trace.WriteLine(igt + " success");
            else
                Trace.WriteLine(igt + " f");
        }
    } 

    public static int Check(int path, string p = "", int tar = -1, bool turn1only = false, string tarmvt = "", bool verbose = true)
    {
        Trace.WriteLine(path + " " + p);
        int numThreads = 30;
        int numFrames = 60;
        SortedDictionary<int, string> manipResult = new SortedDictionary<int, string>();

        BlueCb[] gbs = MultiThread.MakeThreads<BlueCb>(numThreads);
        BlueCb gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        gb.LoadState(State);
        Intro.ExecuteUntilIGT(gb);
        byte[] igtState = gb.SaveState();

        int[] success = {0, 0, 0, 0};

        MultiThread.For(numFrames, gbs, (gb, f) =>
        {
            gb.MakeIGTState(Intro, igtState, f);
            if(tar != -1 && f%60 != (tar+58)%60 && f%60 != (tar+59)%60 && f%60 != tar && f%60 != (tar+1)%60 && f%60 != (tar+2)%60) return;

            var npcTracker = new NpcTracker<BlueCb>(gb.CallbackHandler);
            int addr = gb.Execute(SpacePath(Forest));
            string info = npcTracker.GetMovement((50, 2), (51, 1), (51, 8));
            if(addr == gb.WildEncounterAddress)
            {
                info += " L" + gb.EnemyMon.Level + " " + gb.EnemyMon.Species.Name + " " + gb.Tile;
                if(gb.Tile.X == 1 && gb.Tile.Y == 18 && gb.EnemyMon.Species.Name == "PIKACHU")
                {
                    const Joypad hold = Joypad.B;
                    gb.ClearText();
                    gb.Press(Joypad.A, Joypad.Down, Joypad.A);
                    gb.AdvanceFrames(60, Joypad.A);
                    gb.ClearText(hold, 2);
                    info += " HP:" + gb.BattleMon.HP;
                    if(gb.BattleMon.HP == 0)
                    {
                        gb.Hold(hold, "ManualTextScroll");
                        gb.AdvanceFrames(path - 1, hold);
                        gb.ClearText(hold, 3);
                        gb.Execute(SpacePath("LLLLAUUUUUUUUUUUUUUUUUUUUUUUULLUUUUUUUUUUUUU"));
                        string mvt = npcTracker.GetMovement((1, 7));
                        info += " " + mvt;
                        if(tarmvt == "" || mvt == tarmvt)
                        {
                            if(p != "")
                            {
                                addr = gb.Execute(SpacePath(p));
                            }
                            else
                            {
                                if(path == 1 && mvt == "R") addr = gb.Execute(SpacePath("UUUULLLUUUUURRAURRAUUUUUUULLLLLURUUUUUUU")); // 1 R
                                if(path == 1 && mvt == "L") addr = gb.Execute(SpacePath("UUUULLLUUUUURRRUUURUUUALLLLLAUUUUAURAUUUUU")); // 1 L
                                if(path == 2 && mvt == "RL") addr = gb.Execute(SpacePath("UUUULLLAUUUUAURRRRUAUUUUUAUULALLLLUUUUUUURU")); // 2 RL
                                if(path == 2 && mvt == "RLL") addr = gb.Execute(SpacePath("UUUULLLUUUUURRRRUUUUUUUULALLLLUUUUUUURAU")); // 2 RLL
                                if(path == 3 && mvt == "RL") addr = gb.Execute(SpacePath("UUUULLLUUUUUUURRRRUUUUUULLLLLURUUUUAUUAU")); // 3 RL
                                if(path == 3 && mvt == "LR") addr = gb.Execute(SpacePath("UUUULLLUUUUUUUURRRRUUUULLLLLUURAUUUUUUAU")); // 3 LR
                            }

                            if(addr == gb.WildEncounterAddress)
                                info += " L" + gb.EnemyMon.Level + " " + gb.EnemyMon.Species.Name + " " + gb.Tile;
                            else
                            {
                                gb.ClearText(1);
                                gb.Inject(Joypad.None);
                                byte[] state = gb.SaveState();
                                for(int i = 0; i < 2; ++i)
                                {
                                    gb.LoadState(state);
                                    info += " (" + i + ")";
                                    gb.AdvanceFrames(i);
                                    gb.Inject(Joypad.B);
                                    gb.AdvanceFrame(Joypad.B);
                                    gb.ClearText(Joypad.B);
                                    gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t1

                                    info += LogTurn(gb);
                                    gb.ClearText(Joypad.A);

                                    if(turn1only)
                                    {
                                        if(gb.EnemyMon.HP < 13 && !gb.BattleMon.Poisoned) System.Threading.Interlocked.Increment(ref success[i]);
                                    }
                                    else
                                    {
                                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t2
                                        info += LogTurn(gb);
                                        if(gb.EnemyMon.HP == 0) {
                                            System.Threading.Interlocked.Increment(ref success[i]);
                                            gb.ClearText(Joypad.B);
                                            gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t3
                                            info += LogTurn(gb);
                                            gb.ClearText(Joypad.A);
                                            gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t4
                                            info += LogTurn(gb);
                                            if(gb.EnemyMon.HP == 0) System.Threading.Interlocked.Increment(ref success[i + 2]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else info += " " + gb.Tile;
            lock(manipResult)
            {
                if(verbose) manipResult.Add(f, info);
            }
        });

        foreach((int frame, string value) in manipResult)
            Trace.WriteLine($"{frame / 60:d2}:{frame % 60:d2} {value}");
        if(success[0] + success[1] > 0) Trace.WriteLine(success[0] + " + " + success[1] + " = " + (success[0] + success[1]) + " (" + success[2] + " + " + success[3] + " = " + (success[2] + success[3]) + ")");

        return 0;
    }

    // static string LogTurn(BlueCb gb)
    // {
    //     byte move;
    //     int addr;
    //     string info = "";
    //     if(gb.Hold(Joypad.A, "MainInBattleLoop.enemyMovesFirst", "MainInBattleLoop.playerMovesFirst") == gb.SYM["MainInBattleLoop.enemyMovesFirst"])
    //     {
    //         addr = gb.Hold(Joypad.A, gb.SYM["MoveMissed"], gb.SYM["ManualTextScroll"], gb.SYM["MainInBattleLoop.enemyMovesFirst"] + 0x11);
    //         move = gb.CpuRead(gb.SYM["wEnemySelectedMove"]);
    //         if(move > 0) info += " " + gb.Moves[move].Name;
    //         if(addr == gb.SYM["MoveMissed"]) info += " Miss";
    //         if(gb.CpuRead(gb.SYM["wCriticalHitOrOHKO"]) > 0) info += " Crit";
    //         gb.ClearText(Joypad.A, 99, gb.SYM["ExecutePlayerMove"]);
    //         addr = gb.Hold(Joypad.A, gb.SYM["MoveMissed"], gb.SYM["ManualTextScroll"], gb.SYM["MainInBattleLoop.AIActionUsedEnemyFirst"] + 0xC);
    //         move = gb.CpuRead(gb.SYM["wPlayerSelectedMove"]);
    //         if(move > 0) info += ", " + gb.Moves[move].Name;
    //         if(addr == gb.SYM["MoveMissed"]) info += " Miss";
    //         if(gb.CpuRead(gb.SYM["wCriticalHitOrOHKO"]) > 0) info += " Crit";
    //     }
    //     else
    //     {
    //         addr = gb.Hold(Joypad.A, gb.SYM["MoveMissed"], gb.SYM["ManualTextScroll"], gb.SYM["MainInBattleLoop.playerMovesFirst"] + 0x3);
    //         move = gb.CpuRead(gb.SYM["wPlayerSelectedMove"]);
    //         if(move > 0) info += " " + gb.Moves[move].Name;
    //         if(addr == gb.SYM["MoveMissed"]) info += " Miss";
    //         if(gb.CpuRead(gb.SYM["wCriticalHitOrOHKO"]) > 0) info += " Crit";
    //         if(gb.ClearText(Joypad.A, 99, gb.SYM["ExecuteEnemyMove"], gb.SYM["HandleEnemyMonFainted"]) == gb.SYM["ExecuteEnemyMove"])
    //         {
    //             move = gb.CpuRead(gb.SYM["wEnemySelectedMove"]);
    //             if(move > 0) info += ", " + gb.Moves[move].Name;
    //             gb.Hold(Joypad.B, gb.SYM["EnemyCanExecuteMove"] + 0x7);
    //             addr = gb.Hold(Joypad.A, gb.SYM["MoveMissed"], gb.SYM["ManualTextScroll"], gb.SYM["MainInBattleLoop.playerMovesFirst"] + 0x27);
    //             if(addr == gb.SYM["MoveMissed"]) info += " Miss";
    //             if(gb.CpuRead(gb.SYM["wCriticalHitOrOHKO"]) > 0) info += " Crit";
    //         }
    //     }
    //     info += " [HP:" + gb.BattleMon.HP + ";" + gb.EnemyMon.HP + "]";
    //     if(gb.BattleMon.Poisoned) info += " ***PSN***";
    //     return info;
    // }

    public static void SearchBC1(int numThreads = 25, int numFrames = 60, int path = 1, string npc = "")
    {
        StartWatch();

        BlueCb[] gbs = MultiThread.MakeThreads<BlueCb>(numThreads);
        BlueCb gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        gb.LoadState(State);
        IGTResults states = BlueCb.IGTCheckParallel(gbs, Intro, numFrames, gb =>
        {
            if(gb.Execute(SpacePath(Forest)) == gb.WildEncounterAddress && gb.Tile.X == 1 && gb.Tile.Y == 18 && gb.EnemyMon.Species.Name == "PIKACHU")
            {
                const Joypad hold = Joypad.B;
                gb.ClearText();
                gb.Press(Joypad.A, Joypad.Down, Joypad.A);
                gb.AdvanceFrames(60, Joypad.A);
                gb.Hold(hold, "ManualTextScroll");
                gb.AdvanceFrames(path - 1, hold);
                gb.ClearText(hold, 1);
                if(gb.BattleMon.HP == 0)
                {
                    gb.ClearText(hold, 4);
                    var npcTracker = new NpcTracker<BlueCb>(gb.CallbackHandler);
                    gb.Execute(SpacePath("LLLLAUUUUUUUUUUUUUUUUUUUUUUUULLUUUUUUUUUUUUU"));
                    gb.CallbackHandler.SetCallback(0, null);
                    if(npc == "" || npcTracker.GetMovement((1, 7)) == npc)
                        return true;
                }
            }
            return false;
        }).Purge();

        for(int i = 0; i < states.Length; ++i) states[i].Success = false;
        for(int i = 1, count = 1; i < states.Length + 5; ++i)
        {
            int cur = i % states.Length, prec = (i - 1) % states.Length;
            if(states[cur].IGTFrame == (states[prec].IGTFrame + 1) % 60 && Math.Abs(states[cur].HRA - states[prec].HRA) < 10) ++count; else count = 1;
            if(count == 5) for(int j = i - 4; j <= i; ++j) states[j % states.Length].Success = true;
            else if(count > 5) states[cur].Success = true;
        }
        states = states.Purge();
        // for(int i = 0; i < states.Length; ++i) Trace.WriteLine(states[i].IGTFrame + " " + states[i].HRA + " " + states[i].HRS + " " + states[i].Divider + " " + states[i].Dsum); return;

        RbyMap route2 = gb.Maps[13];
        RbyMap gate = gb.Maps[50];
        RbyMap forest = gb.Maps[51];
        Action actions = Action.Right | Action.Up | Action.Left | Action.A;
        RbyTile startTile = gb.Tile;
        RbyTile[] endTiles = { forest[17, 47] };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, endTiles[0], actions);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, gate[5, 1], actions);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, route2[3, 44], actions);
        for(int x = 4; x <= 7; ++x) for(int y = 45; y <= 46; ++y) route2[x, y].RemoveEdge(0, Action.Up);
        for(int y = 58; y <= 60; ++y) route2[7, y].RemoveEdge(0, Action.Left);
        route2[3, 44].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = gate[4, 7], NextEdgeset = 0, Cost = 0 });
        gate[4, 7].GetEdge(0, Action.Right).Cost = 0;
        gate[4, 7].RemoveEdge(0, Action.A);
        gate[5, 1].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = forest[17, 47], NextEdgeset = 0, Cost = 0 });

        int threshold = states.Length * 2 * 4 / 10;
        var parameters = new DFParameters<BlueCb,RbyMap,RbyTile>()
        {
            EndTiles = endTiles,
            LogStart = "https://gunnermaniac.com/pokeworld?map=13#7/61/",
            // MaxCost = 4,
            // SuccessSS = states.Length * 9 / 10,
            // FoundCallback = (state) =>
            // {
            //     int success = state.IGT.TotalRunning * 2;
            //     MultiThread.For(state.IGT.Length * 2, gbs, (gb, f) =>
            //     {
            //         if(success < threshold) return;
            //         IGTState igt = state.IGT[f % state.IGT.Length];
            //         if(!igt.Running) return;
            //         gb.LoadState(igt.State);
            //         gb.ClearText(1);
            //         gb.Inject(Joypad.None);
            //         gb.AdvanceFrames(f / state.IGT.Length);
            //         gb.Inject(Joypad.B);
            //         gb.AdvanceFrame(Joypad.B);
            //         gb.ClearText(Joypad.B);
            //         gb.Press(Joypad.A, Joypad.Select, Joypad.A);
            //         gb.ClearText(Joypad.B);
            //         if(gb.EnemyMon.HP >= 13 || gb.BattleMon.Poisoned) System.Threading.Interlocked.Decrement(ref success);
            //     });
            //     if(success >= threshold)
            //     {
            //         Trace.WriteLine(state.Log + " NoEnc: " + state.IGT.TotalRunning + "/" + state.IGT.Length + " Crit: " + success + "/" + state.IGT.TotalRunning);
            //         threshold = success;
            //     }
            // }
            MaxCost = 8,
            SuccessSS = 5,
            FoundCallback = (state) =>
            {
                int l = state.IGT.Length;
                string[] infotable = new string[l];
                MultiThread.For(l * 1, gbs, (gb, f) =>
                {
                    IGTState igt = state.IGT[f % l];
                    if(!igt.Running) return;
                    gb.LoadState(igt.State);
                    gb.ClearText(1);
                    gb.AdvanceFrame(Joypad.B);
                    gb.ClearText(Joypad.B);
                    gb.Press(Joypad.A, Joypad.Select, Joypad.A); // bc1 t1
                    gb.ClearText(Joypad.A);
                    if(!gb.BattleMon.Poisoned)
                    {
                        string info = "";
                        if(gb.EnemyMon.HP < 13) info += "T1"; else info += "T2";
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // bc1 t2
                        gb.ClearText(Joypad.B, 2);
                        if(gb.EnemyMon.HP == 0 && !gb.BattleMon.Poisoned)
                        {
                            gb.ClearText(Joypad.B);
                            gb.Press(Joypad.A, Joypad.Select, Joypad.A); // bc2 t3
                            gb.ClearText(Joypad.B, 99, gb.SYM["EnemyCanExecuteMove"] + 0x7);
                            gb.ClearText(Joypad.A);
                            if(!gb.BattleMon.Poisoned)
                            {
                                if(gb.EnemyMon.HP < 13) info += " T3"; else info += " T4";
                                if(gb.BattleMon.SpeedModifider < 7) info += " SS";
                                gb.Press(Joypad.A, Joypad.Select, Joypad.A); // bc2 t4
                                gb.ClearText(Joypad.B, 2);
                                if(gb.EnemyMon.HP == 0 && !gb.BattleMon.Poisoned)
                                {
                                    igt.Success = true;
                                    infotable[f] = info;
                                }
                            }
                        }
                    }
                });
                if(state.IGT.TotalSuccesses >= 5)
                {
                    int streak = -1;
                    for(int i = 0; i < l; ++i)
                    {
                        if(state.IGT[i].Success && state.IGT[(i + 1) % l].Success && state.IGT[(i + 2) % l].Success && state.IGT[(i + 3) % l].Success && state.IGT[(i + 4) % l].Success
                        && state.IGT[(i + 4) % l].IGTFrame == (state.IGT[i].IGTFrame + 4) % 60
                        && infotable[i] == infotable[(i + 1) % l] && infotable[i] == infotable[(i + 2) % l] && infotable[i] == infotable[(i + 3) % l] && infotable[i] == infotable[(i + 4) % l])
                        {
                            streak = (i + 2) % l;
                            if(streak != -1)
                            {
                                string info = "[" + state.IGT[streak].IGTFrame + "] " + infotable[streak];

                                gb.LoadState(state.IGT[streak].State);
                                gb.ClearText(1);
                                gb.Inject(Joypad.None);
                                gb.AdvanceFrames(1);
                                gb.Inject(Joypad.B);
                                gb.AdvanceFrame(Joypad.B);
                                gb.ClearText(Joypad.B);
                                gb.Press(Joypad.A, Joypad.Select, Joypad.A); // bc1 t1
                                gb.ClearText(Joypad.A);
                                if(gb.EnemyMon.HP < 13 && !gb.BattleMon.Poisoned) info += " F2T1";
                                gb.Press(Joypad.A, Joypad.Select, Joypad.A); // bc1 t2
                                gb.ClearText(Joypad.B, 2);
                                if(gb.EnemyMon.HP == 0 && !gb.BattleMon.Poisoned)
                                {
                                    info += " F2T2";
                                    gb.ClearText(Joypad.B);
                                    gb.Press(Joypad.A, Joypad.Select, Joypad.A); // bc2 t3
                                    gb.ClearText(Joypad.B, 99, gb.SYM["EnemyCanExecuteMove"] + 0x7);
                                    gb.ClearText(Joypad.A);
                                    if(gb.EnemyMon.HP < 13 && !gb.BattleMon.Poisoned) info += " F2T3";
                                    gb.Press(Joypad.A, Joypad.Select, Joypad.A); // bc2 t4
                                    gb.ClearText(Joypad.B, 2);
                                    if(gb.EnemyMon.HP == 0 && !gb.BattleMon.Poisoned) info += " F2T4";
                                }

                                string successframes = "";
                                for(int j = 0; j < l; ++j) if(state.IGT[j].Success) successframes += " " + state.IGT[j].IGTFrame;

                                Trace.WriteLine(state.Log + " " + info + " frames:" + successframes);
                            }
                        }
                    }
                }
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states, 0);
        Elapsed("search");
    }

    public static void SearchPika(int numThreads = 16, int frame = 37)
    {
        StartWatch();

        BlueCb[] gbs = MultiThread.MakeThreads<BlueCb>(numThreads);
        BlueCb gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        gb.LoadState(State);
        IGTState state = gb.IGTCheck(Intro, 1, () => gb.Execute(SpacePath("UUUURRRRUUUULLULLLU" + "URUUUUUU" + "UUUURRRRRRRR")) != 0, 0, frame)[0];

        RbyMap route2 = gb.Maps[13];
        RbyMap gate = gb.Maps[50];
        RbyMap forest = gb.Maps[51];
        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A | Action.StartB;
        RbyTile startTile = gb.Tile;
        RbyTile[] endTiles = { forest[1, 18] };
        RbyTile[] blockedTiles = {
            forest[16, 10], forest[18, 10],
            forest[16, 15], forest[18, 15],
            forest[11, 15], forest[12, 15],
            forest[11, 4], forest[12, 4],
            forest[6, 4], forest[8, 4],
            forest[6, 13], forest[8, 13],
            // forest[6, 15], forest[8, 15],
        };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, forest[1, 19], actions, blockedTiles);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, gate[5, 1], actions, blockedTiles);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, route2[3, 44], actions, blockedTiles);
        for(int x = 4; x <= 7; ++x) for(int y = 45; y <= 46; ++y) route2[x, y].RemoveEdge(0, Action.Up);
        for(int y = 58; y <= 60; ++y) route2[7, y].RemoveEdge(0, Action.Left);
        route2[3, 44].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = gate[4, 7], NextEdgeset = 0, Cost = 0 });
        gate[4, 7].GetEdge(0, Action.Right).Cost = 0;
        gate[4, 7].RemoveEdge(0, Action.A);
        gate[4, 7].RemoveEdge(0, Action.StartB);
        gate[5, 1].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = forest[17, 47], NextEdgeset = 0, Cost = 0 });
        forest[1, 19].RemoveEdge(0, Action.A);
        forest[25, 12].RemoveEdge(0, Action.A);
        forest[1, 19].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = forest[1, 18], NextEdgeset = 0, Cost = 0 });

        var parameters = new SFParameters<BlueCb,RbyMap,RbyTile>()
        {
            MaxCost = 14,
            EndTiles = endTiles,
            EncounterCallback = gb =>
            {
                if(gb.Tile == endTiles[0] && gb.EnemyMon.Species.Name == "PIKACHU")
                {
                    gb.ClearText();
                    gb.Press(Joypad.A, Joypad.Down, Joypad.A);
                    gb.AdvanceFrames(60, Joypad.A);
                    gb.ClearText(Joypad.B, 1);
                    if(gb.BattleMon.HP == 0)
                        return true;
                }
                return false;
            },
            LogStart = startTile.PokeworldLink + "/",
            FoundCallback = (state, gb) =>
            {
                Trace.WriteLine(new Path(state.Log));
            }
        };

        SingleFrameSearch.StartSearch(gbs, parameters, startTile, 0, state, 0);
        Elapsed("search");
    }
}
