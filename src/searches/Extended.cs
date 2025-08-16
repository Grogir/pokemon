using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

using static System.Threading.Interlocked;
using static SearchCommon;
using static RbyIGTChecker<Red>;
using System.Dynamic;
using System.Data;

class Extended
{
    static bool CheckEncounter(int address, Red gb, string pokename, IGTResult res)
    {
        if(address != gb.WildEncounterAddress)
            return false;

        res.Mon = gb.EnemyMon;
        if(res.Mon.Species.Name != pokename)
            return false;

        res.Yoloball = gb.Yoloball(0, Joypad.B);
        return res.Yoloball;
    }
    static bool CheckNoEncounter(int address, Red gb, IGTResult res)
    {
        if(address != gb.WildEncounterAddress)
            return true;

        res.Mon = gb.EnemyMon;
        return false;
    }

    static void BuildStates()
    {
        if(System.IO.File.Exists("basesaves/red/manip/ext/nido_0_0.gqs"))
            return;

        System.IO.Directory.CreateDirectory("basesaves/red/manip/ext");
        const int numThreads = 16;
        RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.NoPal);
        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];
        // if (numThreads == 1) gb.Record("test");

        gb.LoadState("basesaves/red/manip/nido.gqs");
        gb.HardReset();
        intro.ExecuteUntilIGT(gb);
        byte[] igtState = gb.SaveState();

        const int numFrames = 3598;
        MultiThread.For(numFrames, gbs, (gb, f) =>
        {
            if((f + 1) * 100 / numFrames > f * 100 / numFrames) Console.WriteLine("%");

            gb.LoadState(igtState);
            byte sec = (byte) (f / 60);
            byte frame = (byte) (f % 60);
            gb.CpuWrite("wPlayTimeMinutes", 5);
            gb.CpuWrite("wPlayTimeSeconds", sec);
            gb.CpuWrite("wPlayTimeFrames", frame);
            intro.ExecuteAfterIGT(gb);

            int ret;
            ret = gb.Execute(SpacePath(NidoPath));

            if(!CheckEncounter(ret, gb, "NIDORANM", new IGTResult()))
                return;

            gb.ClearText(Joypad.B);
            gb.Press(Joypad.A);
            gb.RunUntil("_Joypad");
            gb.AdvanceFrame();
            gb.SaveState("basesaves/red/manip/ext/nido_" + sec + "_" + frame + ".gqs");
        });
    }

    static void LogRNG()
    {
        RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.NoPal);
        RedCb gb = new RedCb();
        // gb.Record("test");

        gb.LoadState("basesaves/red/manip/nido.gqs");
        // gb.LoadState(@"C:\Users\PY\AppData\Roaming\gambatte\saves\Pokemon Red_4.gqs");

        gb.HardReset();
        intro.ExecuteUntilIGT(gb);
        // gb.CpuWrite("wPlayTimeMinutes", 4);
        // gb.CpuWrite("wPlayTimeSeconds", 58);
        // gb.CpuWrite("wPlayTimeFrames", 0);
        gb.CpuWrite("wPlayTimeMinutes", 5);
        gb.CpuWrite("wPlayTimeSeconds", 43);
        gb.CpuWrite("wPlayTimeFrames", 1);
        intro.ExecuteAfterIGT(gb);
        var wp = ReadWeedlePaths()[357 - 1];
        gb.CpuWriteBE("wPartyMon1Attack", (ushort) wp.Atk);
        gb.CpuWriteBE("wPartyMon1Defense", (ushort) wp.Def);
        // gb.CpuWriteBE("wPartyMon1Speed", (ushort) 10);
        // gb.CpuWriteBE("wPartyMon1Special", (ushort) 12);
        gb.CpuWriteBE("wPartyMon1HP", (ushort) wp.HP);
        gb.CpuWriteBE("wPartyMon1MaxHP", (ushort) wp.MaxHP);
        // gb.CpuWriteBE("wPartyMon1DVs", (ushort) 0x6B0F);
        // gb.CpuWriteBE(gb.SYM["wPartyMon1Exp"] + 1, (ushort) 217);
        // gb.CpuWriteBE("wPartyMon1HPExp", (ushort) 85);
        // gb.CpuWriteBE("wPartyMon1AttackExp", (ushort) 94);
        // gb.CpuWriteBE("wPartyMon1DefenseExp", (ushort) 89);
        // gb.CpuWriteBE("wPartyMon1SpeedExp", (ushort) 101);
        // gb.CpuWriteBE("wPartyMon1SpecialExp", (ushort) 100);

        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1HP"));
        // Trace.WriteLine(gb.CpuRead("wPartyMon1BoxLevel"));
        // Trace.WriteLine(gb.CpuRead("wPartyMon1Status"));
        // Trace.WriteLine(gb.CpuRead("wPartyMon1Type1"));
        // Trace.WriteLine(gb.CpuRead("wPartyMon1Type2"));
        // Trace.WriteLine(gb.CpuRead("wPartyMon1CatchRate"));
        // Trace.WriteLine(gb.CpuReadBE<uint>("wPartyMon1Moves").ToString("X"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1OTID"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>(gb.SYM["wPartyMon1Exp"] + 1));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1HPExp"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1AttackExp"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1DefenseExp"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1SpeedExp"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1SpecialExp"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1DVs").ToString("X"));
        // Trace.WriteLine(gb.CpuReadBE<uint>("wPartyMon1PP").ToString("X"));
        // Trace.WriteLine(gb.CpuRead("wPartyMon1Level"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1MaxHP"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1Attack"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1Defense"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1Speed"));
        // Trace.WriteLine(gb.CpuReadBE<ushort>("wPartyMon1Special"));

        // gb.CallbackHandler.SetCallback(gb.SYM["VBlank"], (gb) =>
        // {
        //     Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} {gb.CpuRead("hRandomAdd"):x2}{gb.CpuRead("hRandomSub"):x2}");
        // });
        gb.Execute(SpacePath("LLLULLUAULALDLDLLDADDADLALLALUUAU"));
        gb.Yoloball(0, Joypad.B);
        gb.Hold(Joypad.B, "ManualTextScroll"); // all right
        // gb.AdvanceFrame(); // all right late
        // Trace.Listeners.Add(new TextWriterTraceListener(File.CreateText("allrightlate.txt"), "allrightlate.txt"));
        gb.ClearText(Joypad.B, 2);
        gb.Hold(Joypad.B, "ManualTextScroll"); // will be
        // gb.AdvanceFrame(); // will be late
        gb.ClearText(Joypad.B, 4);
        gb.Hold(Joypad.B, "ManualTextScroll"); // give a
        gb.ClearText(Joypad.B, 1);
        // gb.AdvanceFrame(); // yesno late
        gb.ClearText(Joypad.B, 1); // yesno
        gb.Press(Joypad.A);
        gb.RunUntil("_Joypad");
        // Trace.Listeners.Remove("allrightlate.txt");
        byte[] state = gb.SaveState();
        // return;

        // for(int f = -2; f <= 10; ++f)
        for(int f = 5; f <= 5; ++f)
        {
            int p = FramePath(f);
            string name1 = $"{wp.Atk % 10}{wp.Def % 10}{wp.HP % 10}{wp.MaxHP % 10}p{wp.P}" + (f == 3 ? "b" : "") + ".txt";
            // string name = "path" + p + (f == 2 ? "c" : "b") + ".txt";
            Trace.Listeners.Add(new TextWriterTraceListener(System.IO.File.CreateText(name1), name1));
            gb.LoadState(state);
            if(f < 0)
                gb.AdvanceFrames(f + 2);
            else
            {
                gb.AdvanceFrames(f + 1);
                gb.Press(Joypad.A);
            }
            gb.Press(Joypad.Start);
            string path;
            if(p < 1)
                path = BasePathToGirl;
            else
                path = Pidgey[p];
            gb.Execute(SpacePath(path));
            if(p >= 1)
                gb.Yoloball(0, Joypad.B);
            if(p >= 1 && p <= 6)
            {
                gb.ClearText(Joypad.A);
                gb.Press(Joypad.B);
        gb.CallbackHandler.SetCallback(gb.SYM["VBlank"], (gb) =>
        {
            Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} {gb.CpuRead("hRandomAdd"):x2}{gb.CpuRead("hRandomSub"):x2}");
        });
                // gb.Execute(SpacePath(Forest[p]), (gb.Maps[51][25, 12], gb.PickupItem));
                gb.Execute(SpacePath(wp.Path.P), (gb.Maps[51][25, 12], gb.PickupItem));

                gb.Press(Joypad.A);
                gb.ClearText(1);
                gb.Inject(Joypad.None);
                Trace.Listeners.Remove(name1);
                byte[] state2 = gb.SaveState();
                for(int weedleframe = 0; weedleframe < 4; ++weedleframe)
                {
                    gb.LoadState(state2);
                    string name2 = $"{wp.Atk % 10}{wp.Def % 10}{wp.HP % 10}{wp.MaxHP % 10}p{wp.P}" + (f == 3 ? "b" : "") + "t" + weedleframe + ".txt";
                    gb.AdvanceFrames(weedleframe);
                    gb.Inject(Joypad.B);
                    gb.AdvanceFrame(Joypad.B);
                    Trace.Listeners.Add(new TextWriterTraceListener(System.IO.File.CreateText(name2), name2));
                    gb.ClearText(Joypad.B);
                    gb.Press(Joypad.A, Joypad.Down, Joypad.A); // t1
                    // gb.ClearText(Joypad.A, 1);
                    DoTurn(gb);
                    gb.ClearText(Joypad.A);
                    gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t2
                    DoTurn(gb);
                    gb.ClearText(Joypad.A);
                    gb.Press(Joypad.A, Joypad.Up, Joypad.A); // t3
                    DoTurn(gb);
                    gb.ClearText(Joypad.A);
                    gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t4
                    DoTurn(gb);
                    gb.ClearText(Joypad.A);
                    gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t5
                    DoTurn(gb);
                    gb.ClearText(Joypad.A);
                    gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t6
                    DoTurn(gb);
                    if(gb.EnemyMon.HP != 0) continue;

                    gb.ClearTextUntil(Joypad.B, gb.SYM["EnterMap"]);
                    // gb.AdvanceFrames(30);
                // gb.SaveState(name2+"3a.gqs");
                // gb.SaveState(name2+"2.gqs");
                //     gb.ClearText(Joypad.B);
                // gb.SaveState(name2+"3.gqs");
                // gb.Execute("L");
                // gb.SaveState(name2+"4.gqs");
                    gb.Execute(SpacePath(wp.PostFight+"UUUUU"));
                    Trace.Listeners.Remove(name2);
                    // return;
                }
            }
        }
    }

    class IGTStateResult : IGTResult
    {
        public byte[] State;
    }
    static Dictionary<string, IGTStateResult[]> PersistentStates;
    static Red[] PersistentGbs;

    static List<IGTResult> CheckIGTPersistent(int framesToWait, string path, bool extended = false, int numFrames = 60, int numThreads = 16, int startFrame = 0, int seconds = 1)
    {
        BuildStates();
        if(PersistentGbs == null)
        {
            PersistentGbs = MultiThread.MakeThreads<Red>(numThreads);
            PersistentStates = new Dictionary<string, IGTStateResult[]>();
        }
        Red[] gbs = PersistentGbs;

        int totalNumFrames = numFrames * seconds;
        if(PersistentStates.Count * totalNumFrames > 2000 * 60) // unreasonable memory usage
        {
            // PersistentStates = PersistentStates.Where(x => x.Key.Length < 48).ToDictionary(x => x.Key, x => x.Value);
            int[] lengthCount = new int[1000];
            foreach((string key, IGTStateResult[] states) in PersistentStates)
                lengthCount[key.Length]++;
            int maxUnique = 0;
            int l = 0;
            while(l < lengthCount.Length && lengthCount[l] < 2)
            {
                if(lengthCount[l] == 1)
                    maxUnique = l;
                ++l;
            }
            PersistentStates = PersistentStates.Where(x => x.Key.Length <= maxUnique).ToDictionary(x => x.Key, x => x.Value);
        }

        if(!PersistentStates.ContainsKey(""))
        {
            IGTStateResult[] results = new IGTStateResult[totalNumFrames];
            MultiThread.For(totalNumFrames, gbs, (gb, iterator) =>
            {
                int igt = startFrame + iterator % numFrames + iterator / numFrames * 60;
                if(IgnoredFrames.Contains(igt % 60))
                    return;

                IGTStateResult res = results[iterator] = new IGTStateResult();

                res.IGTSec = (byte)(igt / 60);
                res.IGTFrame = (byte)(igt % 60);
                try
                {
                    gb.LoadState("basesaves/red/manip/ext/nido_" + res.IGTSec + "_" + res.IGTFrame + ".gqs");
                } catch(System.IO.FileNotFoundException)
                {
                    return;
                }

                gb.AdvanceFrames(framesToWait);
                gb.Press(Joypad.A, Joypad.Start);
                gb.AdvanceFrames(40);

                res.Tile = gb.Tile;
                res.Map = gb.Map;
                res.State = gb.SaveState();
            });
            PersistentStates[""] = results;
        }
        if(numThreads == 1)
            gbs[0].Record("test");

        path = path.Replace("_B", "");
        int step = path.Length;
        while(!PersistentStates.ContainsKey(path.Substring(0, step)))
            --step;

        if(step < path.Length)
        {
            for(int i = step + 1; i <= path.Length; ++i)
                PersistentStates[path.Substring(0, i)] = new IGTStateResult[totalNumFrames];

            MultiThread.For(totalNumFrames, gbs, (gb, f) =>
            {
                int curstep = step;
                IGTStateResult prev = PersistentStates[path.Substring(0, curstep)][f];
                if(prev != null && prev.State != null)
                    gb.LoadState(prev.State);
                while(curstep < path.Length)
                {
                    string curpath = path.Substring(0, curstep + 1);

                    if(prev == null || prev.State == null) // we already have a result
                    {
                        PersistentStates[curpath][f] = prev;
                    }
                    else
                    {
                        string action = path[curstep].ToString().Replace("S", "S_B");
                        IGTStateResult res = PersistentStates[curpath][f] = new IGTStateResult();

                        res.IGTSec = prev.IGTSec;
                        res.IGTFrame = prev.IGTFrame;

                        int address = gb.Execute(action, (gb.Maps[51][25, 12], gb.PickupItem));
                        if(address == gb.OverworldLoopAddress)
                            res.State = gb.SaveState();
                        else if(CheckEncounter(address, gb, "PIDGEY", res) && extended)
                        {
                            gb.ClearText(Joypad.A);
                            gb.Press(Joypad.B);
                            res.State = gb.SaveState();
                        }
                        res.Tile = gb.Tile;
                        res.Map = gb.Map;
                        prev = res;
                    }
                    ++curstep;
                }
            });
        }

        List<IGTResult> final = new List<IGTResult>(PersistentStates[path]);
        final.RemoveAll(x => x == null);
        return final;
    }

    static void ClearIGTPersistent()
    {
        PersistentStates = null;
        PersistentGbs = null;
    }

    static List<IGTResult> CheckIGT(int framesToWait, string path, string forest, int numFrames = 60, int numThreads = 16, int startFrame = 0, bool verbose = true)
    {
        BuildStates();
        RedCb[] gbs = MultiThread.MakeThreads<RedCb>(numThreads);
        if(numThreads == 1)
            gbs[0].Record("test");
        List<IGTResult> results = new List<IGTResult>();

        MultiThread.For(numFrames, gbs, (gb, f) =>
        {
            if(verbose && numFrames >= 100 && (f + 1) * 100 / numFrames > f * 100 / numFrames) Console.WriteLine("%");

            f += startFrame;
            if(IgnoredFrames.Contains(f % 60))
                return;

            IGTResult res = new IGTResult();

            res.IGTSec = (byte) (f / 60);
            res.IGTFrame = (byte) (f % 60);
            string state = "basesaves/red/manip/ext/nido_" + res.IGTSec + "_" + res.IGTFrame + ".gqs";
            if(System.IO.File.Exists(state))
                gb.LoadState(state);
            else
                return;

            gb.AdvanceFrames(framesToWait);
            gb.Press(Joypad.A, Joypad.Start);

            var npcTracker = new NpcTracker<RedCb>(gb.CallbackHandler);

            int address = gb.Execute(SpacePath(path));
            // address = DecideMovement(gb, res, npcTracker);

            CheckEncounter(address, gb, "PIDGEY", res);

            res.Info = npcTracker.GetMovement((1, 1), (1, 7));
            res.Tile = gb.Tile;
            res.Map = gb.Map;

            if(res.Yoloball && forest != null)
            {
                IGTResult ext = res.Extended = new IGTResult();

                gb.ClearText(Joypad.A);
                gb.Press(Joypad.B);
                address = gb.Execute(SpacePath(forest), (gb.Maps[51][25, 12], gb.PickupItem));

                CheckNoEncounter(address, gb, ext);

                ext.Info = npcTracker.GetMovement((50, 2), (51, 1), (51, 8)) + " " + gb.EnemyMon.DVs;
                ext.Tile = gb.Tile;
                ext.Map = gb.Map;
            }

            lock(results)
                results.Add(res);
        });
        gbs[0].Dispose();
        if(verbose)
            Console.WriteLine();

        return results;
    }
    static List<IGTResult> CheckIGT(int framesToWait, string path, int numFrames = 60, int numThreads = 16, int startFrame = 0, bool verbose = true)
    {
        return CheckIGT(framesToWait, path, null, numFrames, numThreads, startFrame, verbose);
    }

    static int DecideMovement(Red gb, IGTResult res, NpcTracker<RedCb> npcTracker)
    {
        res.Info = " path";
        int address;
        string npc1, npc2;
        npc1 = npcTracker.GetMovement((1, 1));
        if(npc1 == "dD")
            gb.Execute(SpacePath("RUUUUUU"));
        else
            gb.Execute(SpacePath("UUUUUUR"));
        npc1 = npcTracker.GetMovement((1, 1));
        if(npc1 == "uL")
            gb.Execute(SpacePath("UUAUU"));
        else if(npc1 == "rL" || npc1 == "rD")
            gb.Execute(SpacePath("AUUUU"));
        else
            gb.Execute(SpacePath("UUUU"));
        npc1 = npcTracker.GetMovement((1, 1));
        npc2 = npcTracker.GetMovement((1, 7));
        if(npc1 == "r" && npc2 == "R")
            address = gb.Execute(SpacePath(Pidgey[1].Substring(40)));
        else if(npc1 == "uL" && npc2 == "R")
            address = gb.Execute(SpacePath(Pidgey[2].Substring(41)));
        else if((npc1 == "rL" || npc1 == "rD") && npc2 == "L")
            address = gb.Execute(SpacePath(Pidgey[3].Substring(41)));
        else if(npc1 == "uU" && npc2 == "L")
            address = gb.Execute(SpacePath(Pidgey[4].Substring(40)));
        else if(npc1 == "d" && npc2 == "R")
            address = gb.Execute(SpacePath(Pidgey[5].Substring(40)));
        else if(npc1 == "d" && npc2 == "L")
            address = gb.Execute(SpacePath(Pidgey[6].Substring(40)));
        else if(npc1 == "dUR" && npc2 == "R")
            address = gb.Execute(SpacePath(Pidgey[7].Substring(40)));
        else if(npc1 == "dD" && npc2 == "R")
            address = gb.Execute(SpacePath("LUUUUUUUUUUUUUUUUUUUUULLLUUUUURRRRUUUAU"));
        else if(npc1 == "r" && npc2 == "L")
            address = gb.Execute(SpacePath(Pidgey[8].Substring(40)));
        else if(npc1 == "dr" && npc2 == "R")
            address = gb.Execute(SpacePath(Pidgey[9].Substring(40)));
        else if(npc1 == "u" && npc2 == "R")
            address = gb.Execute(SpacePath(Pidgey[10].Substring(40)));
        else
        {
            res.Info = "";
            if(npc2.Contains("R"))
                address = gb.Execute(SpacePath("LUUUUUUU"));
            else
                address = gb.Execute(SpacePath("UUUUUUU"));
        }
        return address;
    }

    enum PrintFlags
    {
        None = 0,
        List = 1,
        Level = 2,
        Tile = 4,
        Info = 8,
        Default = List | Level | Tile | Info,
        NoEnc = 16,
        All = List | Level | Tile | Info | NoEnc,
    }
    static string ResultInfo(IGTResult res, PrintFlags flags = PrintFlags.Info)
    {
        string line = "";
        if((flags & PrintFlags.Info) != 0 && res.Info != null)
            line += res.Info + " ";
        if(res.Mon == null)
        {
            if((flags & PrintFlags.Tile) != 0)
                line += "@" + res.Tile;
            else if((flags & PrintFlags.NoEnc) != 0)
                line += "No encounter";
        }
        else
        {
            line += res.Mon.Species.Name;
            if((flags & PrintFlags.Level) != 0)
                line += " " + res.Mon.Level;
            if((flags & PrintFlags.Tile) != 0)
                line += " @" + res.Tile;
            if(res.Mon.Species.Name == "PIDGEY")
            {
                if(res.Yoloball)
                    line += " captured";
                else
                    line += " failedtocapture";
            }
        }
        if(res.Extended != null)
        {
            line += ", " + ResultInfo(res.Extended, flags | PrintFlags.NoEnc);
        }
        return line;
    }
    static Dictionary<string, int> GetIGTSummary(List<IGTResult> results, PrintFlags flags = PrintFlags.Info)
    {
        results.Sort(delegate (IGTResult a, IGTResult b) {
            return (a.IGTSec * 60 + a.IGTFrame).CompareTo(b.IGTSec * 60 + b.IGTFrame);
        });

        Dictionary<string, int> summary = new Dictionary<string, int>();
        foreach(IGTResult res in results)
        {
            string line = ResultInfo(res, flags);
            if((flags & PrintFlags.List) != 0)
                Trace.WriteLine((results.Count > 60 ? $"{res.IGTSec,2} " : "") + $"{res.IGTFrame,2} " + line);
            if(!summary.ContainsKey(line))
                summary.Add(line, 1);
            else
                summary[line]++;
        }
        if((flags & PrintFlags.List) != 0)
            Trace.WriteLine("");

        return summary;
    }

    static void DisplayIGTResults(List<IGTResult> results, int frame = -1, PrintFlags flags = PrintFlags.Default)
    {
        if(frame >= 0)
            Trace.WriteLine("PATH " + FramePath(frame) + " (frame " + frame + ")");

        Dictionary<string, int> summary = GetIGTSummary(results, flags);

        foreach(var item in summary.OrderByDescending(x => x.Value))
        {
            Trace.WriteLine(item.Value + "/" + results.Count + " " + (item.Key != "" ? item.Key : "No encounter"));
        }
    }

    static List<DFState<RbyMap, RbyTile>> SearchPidgey(int framesToWait, string path, int numThreads = 14, int numFrames = 57, int success = -1, int maxcost = 10)
    {
        BuildStates();
        StartWatch();

        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];
        if(numThreads == 1)
            gb.Record("test");
        Elapsed("threads");

        IGTResults states = new IGTResults(numFrames);
        MultiThread.For(states.Length, gbs, (gb, i) =>
        {
            int f = i;
            for(int s = 0; s < 60; ++s)
                foreach(int skip in IgnoredFrames)
                    if(f >= skip + 60 * s)
                        ++f;

            gb.LoadState("basesaves/red/manip/ext/nido_" + (f / 60) + "_" + (f % 60) + ".gqs");

            gb.AdvanceFrames(framesToWait);
            gb.Press(Joypad.A);
            gb.Press(Joypad.Start);

            int ret = gb.Execute(SpacePath(path));
            states[i] = new IGTState(gb, false, f);
        });
        Elapsed("states");

        RbyMap viridian = gb.Maps[1];
        RbyMap route2 = gb.Maps[13];
        viridian.Sprites.Remove(18, 9);
        viridian.Sprites.Remove(17, 5);
        RbyTile startTile = gb.Tile;
        RbyTile[] endTiles = { route2[8, 48] };
        RbyTile[] encounterTiles = { route2[6, 48], route2[7, 48], route2[8, 48], route2[7, 49], route2[8, 49], route2[8, 50] };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, endTiles[0], Action.Right | Action.Left | Action.Up | Action.Down | Action.A | Action.StartB);
        // Pathfinding.DebugDrawEdges(gb, viridian, 0);

        var results = new List<DFState<RbyMap, RbyTile>>();
        var parameters = new DFParameters<Red, RbyMap, RbyTile>()
        {
            MaxCost = maxcost,
            SuccessSS = success >= 0 ? success : Math.Max(1, states.Length - 3),// amount of yoloball success for found
            EndTiles = endTiles,
            EncounterCallback = gb => gb.EnemyMon.Species.Name == "PIDGEY" && gb.Yoloball() && encounterTiles.Any(t => t.X == gb.Tile.X && t.Y == gb.Tile.Y),
            FoundCallback = state =>
            {
                results.Add(state);
                Trace.WriteLine(startTile.PokeworldLink + "/" + state.Log + " Captured: " + state.IGT.TotalSuccesses + " Failed: " + (state.IGT.TotalFailures - state.IGT.TotalRunning) + " NoEnc: " + state.IGT.TotalRunning + " Cost: " + state.WastedFrames);
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states, 0);
        Elapsed("search");

        return results;
    }

    static List<DFState<RbyMap, RbyTile>> SearchForest(int framesToWait, string pidgeypath, string forestpath, int numThreads = 14, int numFrames = 57, int success = -1, int maxcost = 10)
    {
        BuildStates();
        StartWatch();

        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];
        if(numThreads == 1)
            gb.Record("test");
        Elapsed("threads");

        IGTResults states = new IGTResults(numFrames);
        MultiThread.For(states.Length, gbs, (gb, i) =>
        {
            int f = i;
            for(int s = 0; s < 60; ++s)
                foreach(int skip in IgnoredFrames)
                    if(f >= skip + 60 * s)
                        ++f;

            gb.LoadState("basesaves/red/manip/ext/nido_" + (f / 60) + "_" + (f % 60) + ".gqs");

            gb.AdvanceFrames(framesToWait);
            gb.Press(Joypad.A);
            gb.Press(Joypad.Start);

            int ret = gb.Execute(SpacePath(pidgeypath));

            CheckEncounter(ret, gb, "PIDGEY", new IGTResult());
            gb.ClearText(Joypad.A);
            gb.Press(Joypad.B);

            if(forestpath != null)
                ret = gb.Execute(SpacePath(forestpath), (gb.Maps[51][25, 12], gb.PickupItem));

            states[i] = new IGTState(gb, false, f);
        });
        Elapsed("states");

        RbyMap route2 = gb.Maps[13];
        RbyMap gate = gb.Maps[50];
        RbyMap forest = gb.Maps[51];
        forest.Sprites.Remove(25, 11);
        Action actions = Action.Right | Action.Left | Action.Up | Action.Down | Action.A | Action.StartB;
        RbyTile startTile = gb.Tile;
        RbyTile[] endTiles = { forest[1, 19] };
        RbyTile[] blockedTiles = {
            forest[26, 12],
            forest[16, 10], forest[18, 10],
            forest[16, 15], forest[18, 15],
            forest[11, 15], forest[12, 15],
            forest[11, 4], forest[12, 4],
            forest[6, 4], forest[8, 4],
            forest[6, 15], forest[8, 15],
            forest[2, 19], forest[1, 18]
        };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, endTiles[0], actions, blockedTiles);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, gate[5, 1], actions, blockedTiles);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, route2[3, 44], actions, blockedTiles);
        for(int x = 4; x <= 7; ++x) for(int y = 45; y <= 46; ++y) route2[x, y].RemoveEdge(0, Action.Up);
        route2[3, 44].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = gate[4, 7], NextEdgeset = 0, Cost = 0 });
        gate[4, 7].GetEdge(0, Action.Right).Cost = 0;
        gate[4, 7].RemoveEdge(0, Action.A);
        gate[4, 7].RemoveEdge(0, Action.StartB);
        gate[5, 1].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = forest[17, 47], NextEdgeset = 0, Cost = 0 });
        forest[25, 12].RemoveEdge(0, Action.A);
        forest[25, 13].RemoveEdge(0, Action.A);
        forest[1, 19].RemoveEdge(0, Action.A);

        bool addResults = true;
        var results = new List<DFState<RbyMap, RbyTile>>();
        var parameters = new DFParameters<Red, RbyMap, RbyTile>()
        {
            MaxCost = maxcost,
            SuccessSS = success >= 0 ? success : Math.Max(1, states.Length - 3),
            EndTiles = endTiles,
            TileCallbacks = new (Tile<RbyMap, RbyTile>, Action<Red>)[] { (forest[25, 12], gb => gb.PickupItem()) },
            EncounterCallback = gb => gb.EnemyMon.Species.Name == "CATERPIE" && endTiles.Any(t => t.X == gb.Tile.X && t.Y == gb.Tile.Y),
            // EncounterCallback = gb => gb.EnemyMon.Species.Name == "CATERPIE",
            FoundCallback = state =>
            {
                if(addResults) results.Add(state);
                Trace.WriteLine(startTile.PokeworldLink + "/" + state.Log + " Success: " + state.IGT.TotalSuccesses + " Failed: " + (state.IGT.TotalFailures - state.IGT.TotalRunning) + " NoEnc: " + state.IGT.TotalRunning + " Cost: " + state.WastedFrames);
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states, 0);
        Elapsed("search");

        return results;
    }

    static List<SFState<RbyMap, RbyTile>> SingleSearchForest(int framesToWait, string pidgeypath, string forestpath, int numThreads, int maxcost, int _hp, int _maxhp)
    {
        int p = FramePath(framesToWait);
        StartWatch();

        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];
        if(numThreads == 1)
            gb.Record("test"); // gb.Show();
        Elapsed("threads");

        RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.NoPal);
        gb.LoadState("basesaves/red/manip/nido.gqs");
        gb.HardReset();
        intro.ExecuteUntilIGT(gb);
        gb.CpuWrite("wPlayTimeMinutes", 5);
        gb.CpuWrite("wPlayTimeSeconds", (byte) 30);
        gb.CpuWrite("wPlayTimeFrames", (byte) 2);
        intro.ExecuteAfterIGT(gb);
        gb.CpuWriteBE("wPartyMon1HP", (ushort) _hp);
        gb.CpuWriteBE("wPartyMon1MaxHP", (ushort) _maxhp);
        gb.Execute(SpacePath(NidoPath));
        gb.Yoloball(0, Joypad.B);
        gb.ClearText(Joypad.B);
        gb.Press(Joypad.A);
        gb.RunUntil("_Joypad");
        gb.AdvanceFrame();
        gb.AdvanceFrames(framesToWait);
        gb.Press(Joypad.A);
        gb.Press(Joypad.Start);
        int ret = gb.Execute(SpacePath(pidgeypath));
        CheckEncounter(ret, gb, "PIDGEY", new IGTResult());
        gb.ClearText(Joypad.A);
        gb.Press(Joypad.B);
        if(forestpath != null)
            ret = gb.Execute(SpacePath(forestpath), (gb.Maps[51][25, 12], gb.PickupItem));
        IGTState state = new IGTState(gb, false, 2);
        Elapsed("states");

        RbyMap route2 = gb.Maps[13];
        RbyMap gate = gb.Maps[50];
        RbyMap forest = gb.Maps[51];
        forest.Sprites.Remove(25, 11);
        Action actions = Action.Right | Action.Left | Action.Up | Action.Down | Action.A | Action.StartB;
        RbyTile startTile = gb.Tile;
        // RbyTile[] endTiles = { forest[2, 19] };
        RbyTile[] endTiles = { forest[1, 19] }; // caterpie
        RbyTile[] blockedTiles = {
            forest[26, 12],
            forest[16, 10], forest[18, 10],
            forest[16, 15], forest[18, 15],
            forest[11, 15], forest[12, 15],
            forest[11, 4], forest[12, 4],
            forest[6, 4], forest[8, 4],
            forest[6, 15], forest[8, 15],
            forest[2, 19], forest[1, 18] // caterpie
        };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, endTiles[0], actions, blockedTiles);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, gate[5, 1], actions, blockedTiles);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, route2[3, 44], actions, blockedTiles);
        for(int x = 4; x <= 7; ++x) for(int y = 45; y <= 46; ++y) route2[x, y].RemoveEdge(0, Action.Up);
        route2[3, 44].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = gate[4, 7], NextEdgeset = 0, Cost = 0 });
        gate[4, 7].GetEdge(0, Action.Right).Cost = 0;
        gate[4, 7].RemoveEdge(0, Action.A);
        gate[4, 7].RemoveEdge(0, Action.StartB);
        gate[5, 1].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = forest[17, 47], NextEdgeset = 0, Cost = 0 });
        forest[25, 12].RemoveEdge(0, Action.A);
        forest[25, 13].RemoveEdge(0, Action.A);
        forest[2, 19].RemoveEdge(0, Action.A);
        forest[2, 20].RemoveEdge(0, Action.A);

        const int weedleframes = 3;
        var results = new List<SFState<RbyMap, RbyTile>>();
        // (int, int)[][] hplists = {
        //     new (int, int)[] {(12, 21), (13, 21), (14, 22), (14, 23)}, // p2 g
        //     new (int, int)[] {(14, 21), (15, 21), (16, 21), (17, 21), (19, 21), (21, 21), (13, 22), (15, 22), (16, 22), (17, 22), (18, 22), (22, 22), (16, 23), (17, 23), (18, 23), (19, 23), (23, 23)}, // p2 y
        //     new (int, int)[] {(18, 21), (19, 22), (20, 22), (15, 23), (20, 23), (21, 23)}, // p2 r
        // };
        // (int, int)[][] hplists = {
        //     new (int, int)[] {(15, 21), (16, 21), (16, 22), (16, 23)}, // p3 y
        //     new (int, int)[] {(18, 21), (19, 21), (18, 22), (19, 22), (14, 21), (15, 22), (19, 23)}, // p3 r
        //     new (int, int)[] {(17, 21), (21, 21), (17, 22), (22, 22), (17, 23), (18, 23), (23, 23)}, // p3 g
        //     new (int, int)[] {(20, 22), (13, 22), (20, 23), (21, 23)}, // p3 b
        //     new (int, int)[] {(12, 21), (13, 21), (14, 22), (14, 23)}, // p3 x
        //     new (int, int)[] {(15, 23)} // p3 w
        // };
        (int, int)[][] hplists = {
            new (int, int)[] {(12, 21), (13, 21), (15, 21), (16, 21), (17, 21), (21, 21), (14, 22), (16, 22), (17, 22), (22, 22), (14, 23), (16, 23), (17, 23), (18, 23), (23, 23)}, // p4 y
            new (int, int)[] {(14, 21), (18, 21), (19, 21), (13, 22), (15, 22), (18, 22), (19, 22), (20, 22), (15, 23), (19, 23), (20, 23), (21, 23)}, // p4 r
        };
        // (int, int)[][] hplists = {
        //     new (int, int)[] {(12, 21), (13, 21), (14, 21), (15, 21), (16, 21), (17, 21), (21, 21), (14, 22), (15, 22), (16, 22), (17, 22), (22, 22), (14, 23), (16, 23), (17, 23), (18, 23), (23, 23)}, // p5 y
        //     new (int, int)[] {(18, 21), (19, 22)}, // p5 g
        //     new (int, int)[] {(19, 21), (13, 22), (18, 22), (20, 22), (19, 23), (20, 23), (21, 23)}, // p5 r
        //     new (int, int)[] {(15, 23)}, // p5 b
        // };
        // (int, int)[][] hplists = {
        //     new (int, int)[] {(12, 21), (13, 21), (15, 21), (16, 21), (17, 21), (21, 21), (14, 22), (16, 22), (17, 22), (22, 22), (14, 23), (16, 23), (17, 23), (18, 23), (23, 23)}, // p6 y
        //     new (int, int)[] {(14, 21), (18, 21), (19, 21), (13, 22), (15, 22), (18, 22), (19, 22), (20, 22), (15, 23), (19, 23), (20, 23), (21, 23)}, // p6 r
        // };
        (int, int)[] hplist = null;
        foreach(var set in hplists)
            foreach((int h, int m) in set)
                if(_hp == h && _maxhp == m)
                    hplist = set;

        var wpaths = ReadWeedlePaths();
        var stats = new List<(int hp, int maxhp, int atk, int def)>();
        foreach((int hp, int maxhp) in hplist)
        {
            foreach(var wp in wpaths)
                if(wp.P == p && wp.HP == hp && wp.MaxHP == maxhp && (wp.S9 < 9 || wp.S15 < 12))
                    stats.Add((hp, maxhp, wp.Atk, wp.Def));
        }
        Paths[] paths = new Paths[stats.Count];
        for(int i = 0; i < stats.Count; ++i) paths[i] = new Paths();
        string link = "https://gunnermaniac.com/pokeworld?local=51#" + (startTile.X + 13) + "/" + (startTile.Y + 11) + "/";
        var parameters = new SFParameters<Red, RbyMap, RbyTile>()
        {
            MaxCost = maxcost,
            EndTiles = endTiles,
            TileCallback = (forest[25, 12], gb => gb.PickupItem()),
            EncounterCallback = gb => gb.EnemyMon.Species.Name == "CATERPIE" && endTiles.Any(t => t.X == gb.Tile.X && t.Y == gb.Tile.Y),
            FoundCallback = (state, gb) =>
            {
                Trace.WriteLine(link + new SearchCommon.Path(state.Log));
                GetIGTSummary(CheckIGT(framesToWait, Pidgey[p], state.Log, 5, 5, 60 * 30), PrintFlags.All);
            }
            /*FoundCallback = (state, gb) =>
            {
                // if(state.Log == "UUUULLLLLUUURUUUUUUUUUURRURRURRRRUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDLDDDDDLLLLUUU")
                // {
                //     Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} {gb.CpuRead("hRandomAdd"):x2}{gb.CpuRead("hRandomSub"):x2}");
                // }
                gb.LoadState(state.IGT.State);
                gb.Press(Joypad.A);
                gb.ClearText(1);
                gb.Inject(Joypad.None);
                byte[] textboxstate = gb.SaveState();
                byte[][] battlestates = new byte[3][];
                for(int weedleframe = 0; weedleframe < weedleframes; ++weedleframe)
                {
                    gb.LoadState(textboxstate);
                    gb.AdvanceFrames(weedleframe);
                    gb.Inject(Joypad.B);
                    gb.AdvanceFrame(Joypad.B);
                    gb.ClearText(Joypad.B, 1);
                    battlestates[weedleframe] = gb.SaveState();
                }
                for(int i = 0; i < stats.Count; ++i)
                {
                    int hp = stats[i].hp;
                    int weedleframe;
                    for(weedleframe = 0; weedleframe < weedleframes; ++weedleframe)
                    {
                        gb.LoadState(battlestates[weedleframe]);
                        gb.CpuWriteBE("wPartyMon1HP", (ushort) hp);
                        gb.CpuWriteBE("wPartyMon1MaxHP", (ushort) stats[i].maxhp);
                        gb.CpuWriteBE("wPartyMon1Attack", (ushort) stats[i].atk);
                        gb.CpuWriteBE("wPartyMon1Defense", (ushort) stats[i].def);
                        gb.ClearText(Joypad.B);
                        gb.Press(Joypad.A, Joypad.Down, Joypad.A); // t1
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp) break;
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t2
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp - 3 || gb.BattleMon.Poisoned) break;
                        gb.Press(Joypad.A, Joypad.Up, Joypad.A); // t3
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp - 6 || gb.BattleMon.Poisoned || gb.EnemyMon.HP > 21) break;
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t4
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp - 6 || gb.BattleMon.Poisoned || gb.EnemyMon.HP > 15) break;
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t5
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp - 6 || gb.BattleMon.Poisoned || gb.EnemyMon.HP > 8) break;
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t6
                        DoTurn(gb);
                        if(gb.BattleMon.HP < hp - 6 || gb.BattleMon.Poisoned || gb.EnemyMon.HP != 0) break;
                    }
                    if(weedleframe == weedleframes)
                    {
                        Path p = new Path(state.Log);
                        Console.WriteLine(link + p);
                        paths[i].Add(p);
                    }
                }
            }*/
        };

        SingleFrameSearch.StartSearch(gbs, parameters, startTile, 0, state, 0);
        Elapsed("search");

        // for(int i = 0; i < stats.Count; ++i)
        // {
        //     foreach(Path path in paths[i])
        //     {
        //         var r = Weedle(p, path.P, 30, false, stats[i].atk, stats[i].def, stats[i].hp, stats[i].maxhp);
        //         path.I = r.info;
        //         path.SS = r.s9 * 100 + r.s15 * 10 + r.s1 + r.s2;
        //     }
        // }
        Elapsed("check");

        // for(int i = 0; i < stats.Count; ++i)
        // {
        //     Trace.WriteLine(stats[i].atk + " " + stats[i].def + " " + stats[i].hp + "/" + stats[i].maxhp + " p" + p + " (" + maxcost + ")");
        //     paths[i].RemoveAll(p => p.SS < 770);
        //     paths[i].PrintAll(link);
        // }
        return results;
    }

    const string NidoPath = "LLLULLUAULALDLDLLDADDADLALLALUUAU";
    const string BasePath = "DRRUUURRRRRRRRRRRRRRRRRRRRRUR";
    const string BasePathToGirl = BasePath + "UUUUUUR";
    const string BasePathToSignL = BasePathToGirl + "UUUULUUUUUU";
    const string BasePathToSignR = BasePathToGirl + "UUUUUUUUUUL";
    static string[] Pidgeyv3 = { "",
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUUUUUUUUUULUUUUUUUUUUUUUUUUUUUUULLLUUUUURRRRUUUSUUUUULLLLLU",      // 1
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUUUUUUUUUUUUUUUUUUUUUUUUUULUUUUULLLUUUUUUARRARRSUUUUUUULLLLLU",    // 2
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUUUUUUUUUUUUUUUUUUUUUUUUUULUUUUULLLUUUUUUARRARRSUUUUUUULLLLLU",    // 2
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUUUUUUUUUUUUUUUUUUUUUUUUUULUUUUULLLUUUUURRRARUSUUUUUUULLLLLU",     // 3
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUUUUUUUUUULUUUUUUUUUUUUUUUUUUUUULLLUUUUUSURRRRUUUUUUULLLLLU",      // 4
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUAUUAUUUUUUUUUUUUUUUUUUUUUUULUUUUULLLUUUUURRRUUAURSUUUUUUUULLLLLU",// 5
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUAUUAUUUUUUULUUUUUUUUUUUUUUUUUUUUULLLUUUUUURRARRAUUUUUUUULLLLLU",  // 6
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUUUUUUUUUULUUUUUUUUUUUUUUUUUUUUULLLUUUUURRRRUUUAUUUUULLLLLU",      // 7
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUUUUUUUUUULUUUUUUUUUUUUUUUUUUUUULLLUUUUUARRRRSUUAUUUUUULLLLLU",    // 8
        "DRRRRUUURRRRRRRRRRRRRRRRRRRURRUUUUUUUUUULUUUUUUUUUUUUUUUUUUUUULLLUUUUURRRRUUUUUUUULLLLLU"        // 9
    };
    static string[] Pidgey = { "",
        BasePath + "UUUUUURUUUULUUUUUUAUUUUUUUUUUUUULLLUUUUUUUUUUURRR",          // 1
        BasePath + "UUUUUURUUAUULUUUAUUUUUUUUUUUUUUAUULLLUUUUUUURRRRUAUUU",      // 2
    //  BasePath + "UUUUUURUUAUULUUUAUUUUUUUUUUUUUUAUULLLUUUUUUURRRRUAUUU",      // 2b
        BasePath + "UUUUUURAUUUUUUUUUUUUUUUUUUUULUAUULLLUUUUUUUUUURRRARU",       // 3
        BasePath + "UUUUUURUUUUUUUUUULAUUUUAUUUUAUUUAUULLLUUUUUUUUAURRRRU",      // 4
        BasePath + "UUUUUURUUUULUUUUUUAUUUUUUUUAUUUAUULLLUUUUUUAUURRUUAURR",     // 5
    //  BasePath + "UUUUUURUUUUUUUUUULAUUUUUUUUUUUUUULLLUUAUUUAURRRRAUU",        // 6
        BasePath + "UUUUUURUUUUUUUUUULAUUUUUUUUUUUUULLLAUUUUUUUARRRRAUU",        // 6 "normal turn"
    //  BasePath + "UUUUUURUUUULUUUUUUAUUUUUUUUUUUAUULLLUUUUUUUURRRRU",          // 7 dUR "2A"
    //  BasePath + "UUUUUURUUUULUUUAUUUUUUUUUUUUUUUULLLUUUUUUUARRRRUAUU",        // 7 dUR "fence"
    //  BasePath + "UUUUUURUULUUUUUAUUUUUUUUUUUUUUAUULLLUUUUUUURRRRUAUU",        // 7 dUR "girl turn"
    //  BasePath + "RUUUUUUUUUULUUUUUUUUUUUUUUUUUUUUULLLUUUUURRRRUUUAU",         // 7 dD "3.0"
    //  BasePath + "UAUUUUURUULUUUUUUUUAUUUUUUUUUUUUULLLUUUUUUURRRRUAUU",        // 7 "universal"
        BasePath + "UUUUUURUUUULAUUAUUUUAUUUUUUUUAUUUAUULLLUUUUUUURRRRUU",       // 7 dUR "perfect"
    //  BasePath + "UUUUUURUUUUUUUUUULAUUUUUUUUUUUUUULLLUUUUUURRRRUU",           // 7 dD "new"
        BasePath + "UUUUUURUUUUUUUUUULUUUUUUUUUUUUULLLUUUUUUURRRRUUU",           // 8 "0A"
    //  BasePath + "UUUUUURUUUUUUUUUULUUUUUUUUUUUUUULLLUUUUUUUURRRRUAU",         // 8 "1A"
        BasePath + "UUUUUURUUUULUUUUUUUUUUUUUUUUUAUUUURLALLLAUUUUUUUUURRR",      // 9 "extrastep" (c=40)
    //  BasePath + "UUUUUURUUUULUUUUUUUUUUUUUUUUUUULLLUUUUUUUSUUUURRRAR",        // 9 "startflash" (c=55)
    //  BasePath + "UUUUUURRUUUUUUUULLUUUUUUUUUUUUUUULLLUUUUUUUUUURRRRU",        // 9 "0A" 53/54
    //  BasePath + "UUUUUURUUUAULUUUAUUUUUUUUUAUUUUUUUULLULUUAUUUUUUARRRAUR",    // 9 "6A"
    //  BasePath + "UUUUUURUAUUAULAUUAUUAUUUUUUUUUUUUUUUULLALUAUUAUUAUURRRUUUAR",// 9 "10A"
        BasePath + "UUUUUURUUUULUUAUUUUUUUUUUUUUUUUULLLAUUUUUUUUUARRRRAUU",      // 10 "4A"
    //  BasePath + "UUUUUURUUUULUUUUUUAUUUUAUUUUAUUUAUUALLLAUUUUUUUUUARRRRAUU",  // 10 "8A"
    //  BasePath + "UUUUUURUUUULUUUUUUUUUUUUUUUUUAUUURLLLLUUUUUUUUUUARRR",       // 10 "extrastep" (c=36)
    };
    static string[] Forest = { "",
        "RUULLLLLUUU" + "RUUUUUUU" + "UUUURRRRRURRRUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUALLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDDDLDLLLLUUU",       // 1
        "UUUULLLLLU" + "UUUUURUU" + "UUAUURUARRRRRRRUUUUUUUUUUUAUUAUUUUUUUUUUUUUUUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDDDLDLLLLUUU",     // 2
        "UUUAULLLLLU" + "RUUUUUUU" + "UUURURRURRRRRUAUUUUUUUUUUUUUUUUUUAUUUUUUUUUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDDDDLLLLLUAUU",     // 3
        "UUUUULLLLLU" + "UUUUUURU" + "UUUURURRRRRRRAUUUAUUUAUUUUUUUUUUUUUUUAUUUUUUUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDADDADDADDDDDDDDDDLLLLLAUUU",// 4
        "UUUULLLLLU" + "UUUURUUU" + "UUUURRRRRRRURAUUUUUAUUUUUUAUUUUUUUUUUUUUUUUUAUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDDDDLLLLLAUUU",    // 5
        "UUUULALLLLUUU" + "RUUUUUUU" + "UUURURRRRRRRUUUUUUUAUUUUAUUUUUUUUUUUUUAUUUAUUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDDDLDLLLLUUU",  // 6
        null, null, null, null
    };
    static string[] Caterpie = { "",
        Forest[1].Substring(0,104) + "DDADLDDDDDDD",
        Forest[2].Substring(0,120) + "DDDDDDDLUU",
        Forest[3].Substring(0,119) + "DADDDDLRLLLLDUALLUUU",
        Forest[4].Substring(0,107) + "DDDDDDDDDDDDDLS_BAD",
        Forest[5].Substring(0,120) + "DDDLDDD",
        Forest[6].Substring(0,123) + "LADDADDDDDAU",
    };
    static SortedSet<int> IgnoredFrames = new SortedSet<int> { 33, 36, 37 };

    static int PathFrame(int path)
    {
        return path > 2 ? path + 1 : path;
    }
    static int FramePath(int frame)
    {
        return frame > 2 ? frame - 1 : frame;
    }

    static void IgnoreNpcIgts(int path, string p7 = null)
    {
        IgnoredFrames = new SortedSet<int> { 33, 36, 37 };
        if(path < 2)
        {
            IgnoredFrames.Add(14 - path);
            IgnoredFrames.Add(15 - path);
        }
        else
        {
            IgnoredFrames.Add((13 - path) % 60);
            IgnoredFrames.Add((14 - path) % 60);
            if(path == 7)
            {
                if(p7 == "dUR")
                {
                    IgnoredFrames.Add(12);
                    IgnoredFrames.Add(52);
                    IgnoredFrames.Add(53);
                    IgnoredFrames.Add(54);
                    IgnoredFrames.Add(55);
                }
                else if(p7 == "dD")
                {
                    for(int i = 0; i <= 11; ++i)
                        IgnoredFrames.Add(i);
                    for(int i = 13; i <= 51; ++i)
                        IgnoredFrames.Add(i);
                    for(int i = 56; i <= 59; ++i)
                        IgnoredFrames.Add(i);
                }
            }
        }
        IgnoredFrames.Add(34);
    }
    static void PathUnknownMovements()
    {
        for(int frame = 1; frame <= 11; ++frame)
        {
            List<IGTResult> res;
            res = CheckIGT(frame, BasePath, 60);
            DisplayIGTResults(res, frame, PrintFlags.Info);
            Trace.WriteLine("");
        }
    }
    static void PathMovement3600(int frame)
    {
        var res = CheckIGT(frame, BasePathToSignR, 3600);
        var ds = new Dictionary<string, int[]>();
        var df = new Dictionary<string, int[]>();
        foreach(var r in res)
        {
            if(!ds.ContainsKey(r.Info))
            {
                ds.Add(r.Info, new int[60]);
                df.Add(r.Info, new int[60]);
            }
            ds[r.Info][r.IGTSec]++;
            df[r.Info][r.IGTFrame]++;
        }
        foreach(var x in ds)
        {
            Trace.WriteLine(x.Key);
            for(int i = 0; i < 60; ++i)
                Trace.WriteLine("s" + i + ": " + x.Value[i]);
        }
        foreach(var x in df)
        {
            Trace.WriteLine(x.Key);
            for(int i = 0; i < 60; ++i)
                Trace.WriteLine("f" + i + ": " + x.Value[i]);
        }
    }
    static void CheckPathsInFile(int frame, string basepath, bool extended, int numFrames, string expectedResult)
    {
        Paths paths = new Paths();
        foreach(string line in System.IO.File.ReadAllLines("paths.txt"))
        {
            string path = Regex.Match(line, @"/([LRUDSA_B]+) ").Groups[1].Value;
            if(path == "") continue;
            // List<IGTResult> igt = CheckIGTPersistent(frame, basepath + path, extended, numFrames, 16, 0, 60);
            // int success = GetIGTSummary(igt).GetValueOrDefault(expectedResult);
            // paths.Add(new Path(path, success));
            Trace.WriteLine(path);
            GetIGTSummary(CheckIGT(frame, basepath, path, 5, 5, 60), PrintFlags.All);

            // var r = Weedle(frame, path, 30, false, 12, 14, 16, 22);
            // Console.WriteLine(r.s9);
            // paths.Add(new Path(path, r.s15 + r.s9, 0, r.info));
        }
        // paths.PrintAll(Link(basepath));
        paths.PrintAll("https://gunnermaniac.com/pokeworld?local=51#21/59/");
    }
    static void ResultsIGT(List<DFState<RbyMap, RbyTile>> results, int frame, string basepath, bool extended, int numFrames, string expectedResult)
    {
        Paths paths = new Paths();
        foreach(var res in results)
        {
            List<IGTResult> igt = CheckIGTPersistent(frame, basepath + res.Log, extended, numFrames);
            int success = GetIGTSummary(igt).GetValueOrDefault(expectedResult);
            paths.Add(new SearchCommon.Path(res.Log, success));
        }
        paths.PrintAll(Link(basepath));
        Elapsed("igt");
    }
    static RbyTile EndTile(string path)
    {
        return SearchCommon.EndTile(new Red().Maps[33][33, 11], path);
    }
    static string Link(string path)
    {
        return SearchCommon.Link(new Red().Maps[33][33, 11], path);
    }

    static RedCb[] WeedleGbs = null;
    static Dictionary<string, byte[]> WeedleStates = new Dictionary<string, byte[]>();

    static bool LoadForestState(Red gb, int hp, int maxhp, int path, int igts, int igtf)
    {
        string state = hp + "/" + maxhp + " p" + path + " " + igts + "," + igtf;
        if(!WeedleStates.ContainsKey(state))
        {
            RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.NoPal);
            gb.LoadState("basesaves/red/manip/nido.gqs");
            gb.HardReset();
            intro.ExecuteUntilIGT(gb);
            gb.CpuWrite("wPlayTimeMinutes", 5);
            gb.CpuWrite("wPlayTimeSeconds", (byte) igts);
            gb.CpuWrite("wPlayTimeFrames", (byte) igtf);
            intro.ExecuteAfterIGT(gb);
            gb.CpuWriteBE("wPartyMon1HP", (ushort) hp);
            gb.CpuWriteBE("wPartyMon1MaxHP", (ushort) maxhp);
            gb.Execute(SpacePath(NidoPath));
            gb.Yoloball(0, Joypad.B);
            gb.ClearText(Joypad.B);
            gb.Press(Joypad.A);
            gb.RunUntil("_Joypad");
            gb.AdvanceFrame();
            gb.AdvanceFrames(PathFrame(path));
            gb.Press(Joypad.A);
            gb.Press(Joypad.Start);
            if(gb.Execute(SpacePath(Pidgey[path])) != gb.WildEncounterAddress) return false;
            if(gb.EnemyMon.Species.Name != "PIDGEY") return false;
            if(!gb.Yoloball(0, Joypad.B)) return false;
            gb.ClearText(Joypad.A);
            gb.Press(Joypad.B);
            lock(WeedleStates) { WeedleStates[state] = gb.SaveState(); }
        }
        else
        {
            gb.LoadState(WeedleStates[state]);
        }
        return true;
    }
    static (string info, int s1, int s2, int s15, int s9, string npc) Weedle(int p, string forest, int igts, bool print, int atk, int def, int hp, int maxhp, List<byte[]> successes = null)
    {
        const int igtf1 = 0, igtf2 = 4;
        const int numigt = igtf2 - igtf1 + 1;
        const int weedleframes = 3;
        string[] result = new string[numigt * weedleframes];
        int score1 = 0;
        int score2 = 0;
        int score15 = 0;
        int score9 = 0;
        // if(WeedleGbs == null) WeedleGbs = MultiThread.MakeThreads<RedCb>(1); WeedleGbs[0].Record("test");
        // if(WeedleGbs == null) WeedleGbs = MultiThread.MakeThreads<RedCb>(numigt);
        RedCb gb = new RedCb();
        SortedSet<string> goodnpcs = new SortedSet<string>();
        // MultiThread.For(numigt, WeedleGbs, (gb, it) =>
        for(int it = 0; it < numigt; ++it)
        {
            int igtf = igtf1 + it;
            if(!LoadForestState(gb, hp, maxhp, p, igts, igtf))
                continue;

            var npcTracker = new NpcTracker<RedCb>(gb.CallbackHandler);
            int adr = gb.Execute(SpacePath(forest), (gb.Maps[51][25, 12], gb.PickupItem));
            string npcs = npcTracker.GetMovement((50, 2), (51, 1), (51, 8));
            if(adr == gb.WildEncounterAddress) {
                if(print) result[it * weedleframes] = igtf + " " + hp + "/" + maxhp + " " + npcs + " " + gb.EnemyMon.ToString();
                continue;
            }
            gb.CpuWriteBE("wPartyMon1Attack", (ushort) atk);
            gb.CpuWriteBE("wPartyMon1Defense", (ushort) def);
            gb.Press(Joypad.A);
            gb.ClearText(1);
            gb.Inject(Joypad.None);
            byte[] weedlestate = gb.SaveState();
            for(int weedleframe = 0; weedleframe < weedleframes; ++weedleframe)
            {
                gb.LoadState(weedlestate);
                string log = "";
                if(print) log = igtf + " " + hp + "/" + maxhp + " " + npcs + " (" + weedleframe + ")";
                gb.AdvanceFrames(weedleframe);
                gb.Inject(Joypad.B);
                gb.AdvanceFrame(Joypad.B);
                gb.ClearText(Joypad.B);
                int lasthp = hp;
                bool crit = false;
                gb.Press(Joypad.A, Joypad.Down, Joypad.A); // t1
                log += LogTurn(gb);
                gb.ClearText(Joypad.A);
                if(gb.BattleMon.HP == hp) Increment(ref score1);
                if(gb.BattleMon.HP <= lasthp - 5) crit = true;
                lasthp = gb.BattleMon.HP;
                gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t2
                log += LogTurn(gb);
                gb.ClearText(Joypad.A);
                if(gb.BattleMon.HP <= lasthp - 5) crit = true;
                lasthp = gb.BattleMon.HP;
                if(gb.BattleMon.HP == hp) Increment(ref score2);
                gb.Press(Joypad.A, Joypad.Up, Joypad.A); // t3
                log += LogTurn(gb);
                gb.ClearText(Joypad.A);
                if(gb.BattleMon.HP <= lasthp - 5) crit = true;
                lasthp = gb.BattleMon.HP;
                gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t4
                log += LogTurn(gb);
                gb.ClearText(Joypad.A);
                if(gb.BattleMon.HP <= lasthp - 5) crit = true;
                lasthp = gb.BattleMon.HP;
                gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t5
                log += LogTurn(gb);
                gb.ClearText(Joypad.A);
                if(gb.BattleMon.HP <= lasthp - 5) crit = true;
                gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t6
                log += LogTurn(gb);
                if(gb.EnemyMon.HP == 0 && !gb.BattleMon.Poisoned && gb.BattleMon.HP >= hp - 6 && !crit)
                {
                    Increment(ref score15);
                    if(igtf > igtf1 && igtf < igtf2) Increment(ref score9);
                    lock(goodnpcs) { goodnpcs.Add(npcs); }
                    if(successes != null) successes.Add(gb.SaveState());
                }
                if(print) result[it * weedleframes + weedleframe] = log;
            }
        }//);
        if(print) foreach(string s in result) Trace.WriteLine(s);
        string npc = "";
        foreach(string s in goodnpcs) if(npc == "") npc = s; else npc += " ou " + s;
        string info = "1:" + score1 + " 2:" + score2 + " 6:" + score15 + "-" + score9;
        if(print) Trace.WriteLine(info);
        return (info, score1, score2, score15, score9, npc);
    }

    public static void Check(int path)
    {
        int frame = PathFrame(path);
        string p = Pidgey[path];
        string f = Forest[path];
        // IgnoreNpcIgts(path);

        DisplayIGTResults(CheckIGT(frame, p, f, 3600), frame, PrintFlags.Default);
    }

    public static void SearchPidgey(int path)
    {
        int frame = PathFrame(path);
        string basepath = BasePathToGirl;
        IgnoreNpcIgts(path);

        var results = SearchPidgey(frame, basepath, 4, 4, 4, 2);
        ResultsIGT(results, frame, basepath, false, 60 * 2, "PIDGEY captured");

        ElapsedTotal("search + igt");
    }

    public static void SearchForest(int path)
    {
        int frame = PathFrame(path);
        // string basepath = Forest[path].Substring(0,123);
        IgnoreNpcIgts(path);

        // var results = SearchForest(frame, Pidgey[path], basepath, 8, 8, 6, 100);
        // ResultsIGT(results, frame, Pidgey[path] + basepath, true, 60, "CATERPIE");
        var results = SearchForest(frame, Pidgey[path], null, 5, 5, 5, 14);
        ResultsIGT(results, frame, Pidgey[path], true, 5, "CATERPIE");

        ElapsedTotal("search + igt");
    }

    static List<WeedlePath> ReadWeedlePaths()
    {
        List<WeedlePath> paths = new List<WeedlePath>();
        foreach(string line in System.IO.File.ReadAllLines("wpaths.txt"))
        {
            var r1 = Regex.Match(line, @"([0-9]+) ([0-9]+) ([0-9]+)/([0-9]+) p([0-9]+)");
            var r2 = Regex.Match(line, @" - ([0-9]+)/([0-9]+) - (.*) - (.*) - [^LRUDSA_B]*/([LRUDSA_B]+) - ([LRUDSA_B]*)");
            if(r2.Success)
                paths.Add(new WeedlePath(int.Parse(r1.Groups[1].Value), int.Parse(r1.Groups[2].Value), int.Parse(r1.Groups[3].Value), int.Parse(r1.Groups[4].Value), int.Parse(r1.Groups[5].Value), r2.Groups[5].Value, int.Parse(r2.Groups[1].Value), int.Parse(r2.Groups[2].Value), r2.Groups[3].Value, r2.Groups[4].Value, r2.Groups[6].Value));
            else
                paths.Add(new WeedlePath(int.Parse(r1.Groups[1].Value), int.Parse(r1.Groups[2].Value), int.Parse(r1.Groups[3].Value), int.Parse(r1.Groups[4].Value), int.Parse(r1.Groups[5].Value), ""));
        }
        return paths;
    }
    static void WriteWeedlePaths(List<WeedlePath> paths, bool js = false)
    {
        foreach(WeedlePath p in paths.OrderBy(p => p.P).ThenBy(p => p.MaxHP).ThenBy(p => p.HP).ThenBy(p => p.Atk).ThenBy(p => p.Def))
            if(js == false || p.S9 > 7)
                Trace.WriteLine(p.ToString(js));
    }
    public class WeedlePath
    {
        public int HP, MaxHP, Atk, Def, P, S15, S9;
        public string Npcs, Igts, PostFight;
        public SearchCommon.Path Path;
        public WeedlePath(int atk, int def, int hp, int maxhp, int p, string path, int s15 = 0, int s9 = 0, string npcs = "", string igts = "", string postfight = "")
        {
            Atk = atk;
            Def = def;
            HP = hp;
            MaxHP = maxhp;
            P = p;
            Path = new SearchCommon.Path(path);
            if(npcs == "" && path != "")
            {
                var r = Weedle(p, path, 30, false, atk, def, hp, maxhp);
                s15 = r.s15;
                s9 = r.s9;
                npcs = r.npc;
            }
            S15 = s15;
            S9 = s9;
            Npcs = npcs;
            Igts = igts;
            PostFight = postfight;
        }
        public string ToString(bool js = false)
        {
            RbyTile t = EndTile(Pidgey[P]);
            if(js) return $"[{Atk}, {Def}, {HP}, {MaxHP}, {P}, {S15}, {S9}, '{Npcs}', '{Igts}', 'https://gunnermaniac.com/pokeworld?local=51#{t.X + 13}/{t.Y + 11}/{Path.P}{PostFight}'],";
            if(S15 == 0) return $"{Atk} {Def} {HP}/{MaxHP} p{P}";
            return $"{Atk} {Def} {HP}/{MaxHP} p{P} - {S15}/{S9} - {Npcs} - {Igts} - https://gunnermaniac.com/pokeworld?local=51#{t.X + 13}/{t.Y + 11}/{Path.P} - {PostFight}";
        }
    }

    void RecheckAll()
    {
        var stats = new List<(int atk, int def, int hp, int maxhp, int p)>();
        for(int p = 2; p <= 6; ++p)
            for(int maxhp = 21; maxhp <= 23; ++maxhp)
                for(int hp = maxhp - 9; hp <= maxhp; ++hp)
                    if(hp != maxhp - 1)
                        for(int atk = 11; atk <= 12; ++atk)
                            for(int def = 12; def <= 14; ++def)
                                stats.Add((atk, def, hp, maxhp, p));
        Paths[] paths = new Paths[stats.Count];
        string[] costs = new string[stats.Count];
        for(int i = 0; i < stats.Count; ++i) paths[i] = new Paths();
        int curindex = -1;
        foreach(string line in System.IO.File.ReadAllLines("weedlesearch.txt"))
        {
            var r1 = Regex.Match(line, @"([0-9]+) ([0-9]+) ([0-9]+)/([0-9]+) p([0-9]+) \(([0-9 ]+)\)");
            // var r2 = Regex.Match(line, @"/([LRUDSA_B]+)");
            var r2 = Regex.Match(line, @"/([LRUDSA_B]+) ([0-9]+)");
            if(r1.Success)
            {
                var curstat = (int.Parse(r1.Groups[1].Value), int.Parse(r1.Groups[2].Value), int.Parse(r1.Groups[3].Value), int.Parse(r1.Groups[4].Value), int.Parse(r1.Groups[5].Value));
                curindex = stats.FindIndex(s => s == curstat);
                costs[curindex] += " " + r1.Groups[6].Value;
            }
            else if(r2.Success)
            {
                paths[curindex].Add(new SearchCommon.Path(r2.Groups[1].Value, int.Parse(r2.Groups[2].Value)));
            }
        }
        // for(int i = 0; i < stats.Count; ++i)
        // {
        //     Trace.WriteLine(stats[i].atk + " " + stats[i].def + " " + stats[i].hp + "/" + stats[i].maxhp + " p" + stats[i].p + " (" + costs[i][1..] + ")");
        //     Paths list = new Paths();
        //     foreach(Path p in paths[i])
        //     {
        //         var r = Weedle(stats[i].p, p.P, 30, false, stats[i].atk, stats[i].def, stats[i].hp, stats[i].maxhp);
        //         list.Add(new Path(p.P, r.s9 * 100 + r.s15 * 10 + r.s1 + r.s2, 0, r.info));
        //     }
        //     list.RemoveAll(p => p.SS < 770);
        //     RbyTile t = EndTile(Pidgey[stats[i].p]);
        //     list.PrintAll($"https://gunnermaniac.com/pokeworld?local=51#{t.X + 13}/{t.Y + 11}/");
        // }
        List<WeedlePath> currentpaths = ReadWeedlePaths();
        for(int i = 0; i < stats.Count; ++i)
        {
            SearchCommon.Path cur = paths[i].Find(p => p.P == currentpaths[i].Path.P);
            if(paths[i].Count == 0) {
                if(currentpaths[i].Path.P != "")
                    currentpaths[i] = new WeedlePath(stats[i].atk, stats[i].def, stats[i].hp, stats[i].maxhp, stats[i].p, "");
            }
            else if(cur == null)
                currentpaths[i] = new WeedlePath(stats[i].atk, stats[i].def, stats[i].hp, stats[i].maxhp, stats[i].p, paths[i][0].P);
            else if(cur.SS < paths[i][0].SS || cur.A > paths[i][0].A || cur.T > paths[i][0].T)
                currentpaths[i] = new WeedlePath(stats[i].atk, stats[i].def, stats[i].hp, stats[i].maxhp, stats[i].p, paths[i][0].P);
        }
        WriteWeedlePaths(currentpaths);
    }
    void CheckWeedleIgtSecond()
    {
        List<WeedlePath> paths = ReadWeedlePaths();
        foreach(WeedlePath p in paths)
        {
            // if(p.P < 4 || p.P == 4 && (p.MaxHP < 23 || p.HP < 18)) continue;
            if(p.Path.P != "" && p.Igts == "")
            {
                string igt = "";
                // for(int s = 0; s < 60; ++s)
                // {
                //     var r = Weedle(p.P, p.Path.P, s, false, p.Atk, p.Def, p.HP, p.MaxHP);
                //     if(r.s9 < p.S9 || r.s15 < p.S15)
                //         Console.WriteLine(s + " " + r.info);
                //     if(r.s9 < 7)
                //         igt += " " + s;
                // }
                SortedSet<int> set = new SortedSet<int>();
                System.Threading.Tasks.Parallel.For(0, 60, s => {
                    var r = Weedle(p.P, p.Path.P, s, false, p.Atk, p.Def, p.HP, p.MaxHP);
                    if(r.s9 < p.S9 || r.s15 < p.S15)
                        Console.WriteLine(s + " " + r.info);
                    if(r.s9 < 7)
                        lock(set) { set.Add(s); }
                });
                foreach(int n in set) igt += " " + n;
                Console.WriteLine(igt[1..]);
                p.Igts = igt[1..];
            }
            Trace.WriteLine(p.ToString());
        }
    }

    public Extended()
    {
        // RecheckAll();
        // WriteWeedlePaths(ReadWeedlePaths(), true);
        // LogRNG();

        // SingleSearchForest(PathFrame(4), Pidgey[4], null, 16, 8, 22, 22);
        // SingleSearchForest(PathFrame(4), Pidgey[4], null, 16, 8, 19, 22);


        // int score = 0;
        // Red gb = new Red();
        // // foreach(byte[] s in states)
        // for(int i = 0; i < states.Count; ++i)
        // {
        //     gb.LoadState(states[i]);
        //     gb.ClearText(Joypad.B);
        //     int adr = gb.Execute(SpacePath("LAUUUAUUUUUUUUUU"));
        //     if(adr == gb.OverworldLoopAddress) score++;
        //     Trace.WriteLine(i + ": " + (adr == gb.OverworldLoopAddress ? "ok" : gb.EnemyMon.ToString() + " " + gb.Tile));
        // }
        // Trace.WriteLine(score + "/" + states.Count);


        // Red[] gbs = MultiThread.MakeThreads<Red>(15);
        // Red gb = gbs[0];
        // var wpaths = ReadWeedlePaths();
        // // var wp = wpaths[357 - 1];
        // foreach(var wp in wpaths)
        // {
        //     wp.PostFight = "";
        //     if(wp.S15 > 0)
        //     {
        //         List<byte[]> weedlestates = new List<byte[]>();
        //         Weedle(wp.P, wp.Path.P, 30, false, wp.Atk, wp.Def, wp.HP, wp.MaxHP, weedlestates);

        //         IGTResults states = new IGTResults(weedlestates.Count);
        //         MultiThread.For(states.Length, gbs, (gb, i) =>
        //         {
        //             gb.LoadState(weedlestates[i]);
        //             gb.ClearTextUntil(Joypad.B, gb.SYM["EnterMap"]);
        //             gb.AdvanceFrames(35);
        //             // gb.ClearText(Joypad.B);
        //             states[i] = new IGTState(gb, false, i);
        //         });

        //         Action actions = Action.Right | Action.Left | Action.Up | Action.Down | Action.A | Action.StartB;
        //         RbyTile[] endTiles = { gb.Maps[51][1, 6] };
        //         gb.AdvanceFrames(5);
        //         Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, endTiles[0], actions);

        //         Paths paths = new Paths();
        //         var parameters = new DFParameters<Red, RbyMap, RbyTile>()
        //         {
        //             MaxCost = 14,
        //             SuccessSS = states.Length,
        //             EndTiles = endTiles,
        //         };
        //         parameters.FoundCallback = state =>
        //         {
        //             Path p = new Path(state.Log, state.IGT.TotalRunning);
        //             paths.Add(p);
        //             Console.WriteLine(p);
        //             if(state.WastedFrames < parameters.MaxCost) parameters.MaxCost = state.WastedFrames;
        //         };
        //         DepthFirstSearch.StartSearch(gbs, parameters, gb.Tile, 0, states);

        //         if(paths.Count > 0)
        //             wp.PostFight = paths.OrderByDescending(p => p.SS).ThenBy(p => p.C).ThenBy(p => p.S).ThenBy(p => p.A).ThenBy(p => p.T).First().P;
        //     }
        //     Trace.WriteLine(wp.ToString());
        // }





        // foreach(WeedlePath p in paths)
        // {
        //     if(p.Path.P != "")
        //     {
        //         var r = Weedle(p.P, p.Path.P, 30, false, p.Atk, p.Def, p.HP, p.MaxHP);
        //         if(r.s9 != p.S9 || r.s15 != p.S15) Console.WriteLine($"{p.Atk} {p.Def} {p.HP}/{p.MaxHP} p{p.P} - {p.S15}/{p.S9} -> {r.s15}/{r.s9}");
        //         p.S9 = r.s9;
        //         p.S15 = r.s15;
        //     }
        //     Trace.WriteLine(p.ToString());
        // }

        int path = 3;
        // CheckPathsInFile(PathFrame(path), Pidgey[path], true, 5, "CATERPIE");
        // Check(path);
        // SearchForest(path);
        // SingleSearchForest(PathFrame(path), Pidgey[path], null, 16, 20, 22, 22);
        // string forest = "UAUULALLLLUAUUUUUURUUUUUURRRRRRRURUUUUUAUUUUUUUUUUUUUUUUUUUUUAUUUUUUULLLLLALLLDDDADDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDADDDLDDDADLLLLLUUU"; // 2
        string forest = "UUUAULLLLLURUUUUUUUUUURURRURRRRRUAUUUUUUUUUUUUUUUUUUAUUUUUUUUUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDADDDDLRLLLLDUALLUUU"; // 3
        // string forest = "UUUUULLLLLAUUUUUURAUUUUUURRRRURRRRUUUAUUUUUUUUUUUUUUUUUUUUUUUUUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDLDDDLLLLUULU"; // 4
        // string forest = "UULALLLLUUUUUUURAUUAUUUUURRRUURRARRRUUUUUUUUUUUUUUUUUUUUUUUUUUAUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDDDDLLLLLLUUU"; // 5
        // string forest = "UAUUAUUULLLALLAUURUUUUUUUUURURUURRRRRRUUUUUUUUUUUUUUUUUUUUUUUUUUUAUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDLDDDADDLLLLLUAUU"; // 6
        for(int s = 0; s < 60; ++s)
        {
            Trace.WriteLine(s);
            GetIGTSummary(CheckIGT(PathFrame(path), Pidgey[path], forest, 5, 5, 60 * s), PrintFlags.All);
        }
    }
}
