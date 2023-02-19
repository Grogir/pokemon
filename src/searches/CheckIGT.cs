using System;
using System.Collections.Generic;
using System.Diagnostics;

using static RbyIGTChecker<Red>;

class CheckIGT
{
    public static void AltNidos()
    {
        string nido = "LLLULLUAULALDLDLLDADDADLALLALUUAU"; // standard
        // string nido = "LLLULLUAULALDLDLLADDADDLALLALUUAU"; // turn-a
        // string nido = "LLLULLUAULALDLDLLADDDADLALLALUUAU"; // both
        // string nido = "LLLULLUAULALDLDLLDADDDLALLALUUAU"; // igt
        // string nido = "LDUALLULLLLAULLLLLADDADDLADLAUUAU"; // palette1
        // string nido = "LDUALLULLLLAULLLLDADDADLLADLAUUAU"; // palette2
        // string nido = "LDUAULLLLLAULLLLADDADDDLALLALUUAU"; // palette3
        // string nido = "LLLULLLLAUDAULLLADLADDDADLALLUAUU"; // alt1
        // string nido = "LLLULLLAULADULLLADLLDADDADLALUAUU"; // alt2
        // string nido = "LLLULLLAULADULLLADLADDDADLALLUUAU"; // alt3
        // string nido = "LLLULULLLARLALLLADLDALDADLADLUUAU"; // alt4
        // string nido = "ULLLLLUAUDALLLDLLADDDADLALLALUUAU"; // weird alt
        // string nido = "LLLAUUULLLLDALLLDDADDADLALLALUAUU"; //
        RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.NoPal);
        CheckIGT("basesaves/red/manip/nido.gqs", intro, nido, "NIDORANM", 3600, true);
    }

    public static void PostHiker()
    {
        RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.NoPal);

        string poy = "DDDDDDDDDDDARRRRRRRRRRRRRRRRD";
        // poy += "UUURRRRRDDRRRRRRRUURRRDDDDDDDDLLDDDDDDDDDLLLLLLLLLLLLLLLLLLLLLLLUUUALUUUUUUUUUUU";
        poy += "UUURRRRRDDRRRRRRRUURRRDDDDDDDDDDLLDDDDDDDLLLLLLLLLLLLLLLLLLLLLLLUUUALUUUUUUUUUUU"; // late turn
        CheckIGT("basesaves/red/manip/posthiker.gqs", intro, poy, "PARAS", 3600, false, true);
        CheckIGT("basesaves/red/manip/posthiker_redbar.gqs", intro, poy, "PARAS", 3600, false, true);

        string posthiker = "DDDDDDDDDDDADRRRRRRRRRRRRRRRR";
        posthiker += "UUURRRRRDDRRRRRRUURRRRDDDDDDDDLLLLDDDDDDDDDALLLALLLLLLLLLLLLLALLLALL";
        CheckIGT("basesaves/red/manip/posthiker.gqs", intro, posthiker + "UAUUUUAUUUULUUUU", "PARAS", 3600);
        CheckIGT("basesaves/red/manip/posthiker_redbar.gqs", intro, posthiker + "UAUUUUULUUUUUUAUU", "PARAS", 3600);
    }

    public static void Rt3Moon()
    {
        string rt3Moon = "RRRRRRRRURRUUUUUARRRRRRRRRRRRDDDDDRRRRRRRARUURRUUUUUUUUUURRRRUUUUUUUUUURRRRRU";
        // string rt3Moon = "RRRRRRRURRRUUUUUARRRRRRRRRRRRDDDDDRRRRRRRARUURRUUUUUUUUUURRRRUUUUUUUUUURRRRRU"; // 1 early
        rt3Moon += "UUUUUULLLLLALLLLDD";
        rt3Moon += "RRRRUURRRARRUUUUUUURRRRRRRAUUUUUUURRRDRDDDDDDDADDDDDDDDADRRRRRURRRR";
        rt3Moon += "UUUUUUUUR";
        rt3Moon += "ULUUUUUAUUUUUULLLUUUUUUUULLLLLLDDLALLLLLLLDDDDDD";
        rt3Moon += "LALLALLALLALDD";
        rt3Moon += "RRRUUULAUR";
        rt3Moon += "DDADLALLAD";
        // rt3Moon += "DDADDALLAL"; // alt post mp
        rt3Moon += "RARRARRARRARUU";
        rt3Moon += "DDLDDDDLLLLLLLULUUUUULUUUUUUUULLLUL";
        rt3Moon += "DADDRAR";
        // rt3Moon += "DRRDDDDDDDDDDRRRARRRRRRRRRRDR";
        // rt3Moon += "DRRDDDDDDDDDDRRRARRRRRRRRRRRD"; // slayer
        // rt3Moon += "DRRDDDDDDDDDADRRRRRRRRRRRRRDR"; // 4 early
        // rt3Moon += "DRRDDDDDDDDDDARRRRRRRRRRRRRDR"; // 3 early
        rt3Moon += "DRRDDDDDDDDDDRARRRRRRRRRRRRDR"; // 2 early
        // rt3Moon += "DRRDDDDDDDDDDRRARRRRRRRRRRRDR"; // 1 early
        // rt3Moon += "DRRDDDDDDDDDDRRRRARRRRRRRRRDR"; // 1 late
        // rt3Moon += "DRRDDDDDDDDDDRRRRRARRRRRRRRDR"; // 2 late
        // rt3Moon += "DRRDDDDDDDDDDRRRRRRARRRRRRRDR"; // 3 late
        // rt3Moon += "DRRDDDDDDDDDDRRRRRRRARRRRRRDR"; // 4 late
        // rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUUU";
        // rt3Moon += "RRUUURARRRDDRRRRRUAURRARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUUU"; // alt b2f bad (3025)
        // rt3Moon += "RRUUURARRRDDRRRRRUAURRARRDDDDDDDDALLLLDDDDDDADDDLLLALLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUUU"; // alt b2f good (3213)
        // rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLLALLLLLUUUUAUUALUUUUUUUU"; // 7 1 late
        rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLLLALLLLUUUUAUUALUUUUUUUU"; // 7 2 late
        // rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLLLLALLLUUUUAUUALUUUUUUUU"; // 7 3 late
        // rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLALLLLLLLUUUUAUUALUUUUUUUU"; // 7 1 early
        // rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDADDDLLLALLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUUU"; // 5 1 early
        // rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDDADLLLALLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUUU"; // 5 1 late
        // rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLALLLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUUU"; // 6 1 early
        // rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLLALLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUUU"; // 6 1 late
        // rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUDD"; // clef mvt
        RbyIntroSequence rt3MoonIntro = new RbyIntroSequence(RbyStrat.PalHold);
        CheckIGT("basesaves/red/manip/rt3moon.gqs", rt3MoonIntro, rt3Moon, "PARAS", 3600);
    }

    public static void Rt3MoonBackups(int frame = 36)
    {
        string rt3Moon = "RRRRRRRRURRUUUUUARRRRRRRRRRRRDDDDDRRRRRRRARUURRUUUUUUUUUURRRRUUUUUUUUUURRRRRU";
        rt3Moon += "UUUUUULLLLLLLLLDD";
        // rt3Moon += "RRRRUURRRRRUUUUUUURRRRRRURUUUUUURRRDDDDDDDDDRDDDDDDDDRRRRRRRRURUUUUUUUURULUUUUUUUUUUUULLULUUUUUULLDLLLLDDDLLLLLLLLDDDD";
        rt3Moon += "RRRRUURRRRRUUUUUUURRRRRRURUUUUUURRRDDDDDDDDDRDDDDDDDDRRRRRRRRURUUUUUUUURULUUUUUUUUUUUULLULUUUUUULLLLLLDDDDLLLLLLLLDDDD"; // alt
        rt3Moon += "LALLLLLLLDD";
        rt3Moon += "RARRAUUULUR";
        rt3Moon += "DDDDLLL";
        rt3Moon += "RRRRRRRRUU";
        rt3Moon += "DDDDDDLLLLLLUUAUUAUUUUUUUUUUULALLALLLLL";
        rt3Moon += "DADDRAR";
        rt3Moon += "DDDDDDDDDDDDRRRRRRRRRRRRRRRR";
        // if(frame == 36) rt3Moon += "UUURRRRRRDDRRARRRUURRRRDDDDDDDDDDLLALLDDDDDDDLLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUUUUUU";
        // if(frame == 36) rt3Moon += "UUURRRRRRDDRRARRRUURRRRDDDDDDDDDDLLLLADDDDDDDLLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUUUUUU"; // alt
        // if(frame == 36) rt3Moon += "UUURRRRRRDDRRARRRUURRRRDDDDDDDDLLLLADDDDDDDDDLLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUUUUUU"; // alt
        if(frame == 36) rt3Moon += "UUURRRRRRDDRRARRRUURRRRDDDDDDDDALLLLDDDDDDDDDLLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUUUUUU"; // alt
        if(frame == 37) rt3Moon += "UUURRRRRRDDARRRRARRUURRRDDDDDDDDLLLLDDDDDADDDDLLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUUUUU";
        RbyIntroSequence rt3MoonIntro = new RbyIntroSequence(RbyStrat.PalHold);
        CheckIGT("basesaves/red/manip/rt3moon.gqs", rt3MoonIntro, rt3Moon, "PARAS", 60, false, false, Verbosity.Summary, false, -1, null, frame, 60, 16);
        CheckIGT("basesaves/red/manip/rt3moon.gqs", rt3MoonIntro, rt3Moon, "PARAS", 60, false, frame == 36, Verbosity.Summary, true, -1, null, frame, 60, 16);
    }

    public static void Rt3MoonFrame54Backup()
    {
        string rt3Moon = "RRRRRRRRURRUUUUUARRRRRRRRRRRRDDDDDRRRRRRRARUURRUUUUUUUUUURRRRUUUUUUUUUURRRRRU";
        rt3Moon += "UUUUUULLLLLALLLLDD";
        rt3Moon += "RRRRUURRRARRUUUUUUURRRRRRRAUUUUUUURRRDRDDDDDDDADDDDDDDDADRRRRRURRRR";
        rt3Moon += "UUUUUUUUR";
        rt3Moon += "ULUUUUUAUUUUUULLLUUUUUUUULLLLLLDDLALLLLLLLDDDDDD";
        rt3Moon += "LALLALLALLALDD";
        rt3Moon += "RRRUUULAUR";
        rt3Moon += "DDADLALLAD";
        rt3Moon += "RARRARRARRARUU";
        rt3Moon += "DDLDDDDLLLLLLLULUUUUULUUUUUUUULLLUL";
        rt3Moon += "DADDRAR";
        rt3Moon += "DRRDDDDDDDDDDARRRRRRRRRRRRRDR";
        rt3Moon += "RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLLLALLLLUUUUAUUALUUUUUUUU";
        RbyIntroSequence rt3MoonIntro = new RbyIntroSequence(RbyStrat.PalHold);
        bool memeBall(Red gb)
        {
            if(gb.EnemyMon.Species.Name != "PARAS") return false;
            gb.RunUntil("WaitForTextScrollButtonPress");
            // gb.AdvanceFrames(1); // missing textbox
            gb.Press(Joypad.B);
            gb.RunUntil("HandleMenuInput");
            gb.Press(Joypad.A | Joypad.Down, Joypad.A | Joypad.Down | Joypad.Right);
            if(gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"]) return true;
            gb.ClearText(Joypad.B);
            // gb.Press(Joypad.A, Joypad.Select, Joypad.B, Joypad.A, Joypad.Select, Joypad.A);
            // gb.Press(Joypad.A, Joypad.A | Joypad.Up, Joypad.B, Joypad.Select, Joypad.A | Joypad.Down);
            gb.Press(Joypad.A, Joypad.A | Joypad.Up, Joypad.B, Joypad.B | Joypad.Left, Joypad.A, Joypad.Select, Joypad.A | Joypad.Down);
            return gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"];
        }
        CheckIGT("basesaves/red/manip/rt3moon_slot2.gqs", rt3MoonIntro, rt3Moon, "PARAS", 60, false, false, RbyIGTChecker<Red>.Verbosity.Full, false, -1, memeBall, 54, 60); // yoloball igt
        CheckIGT("basesaves/red/manip/rt3moon_slot2.gqs", rt3MoonIntro, rt3Moon, "PARAS", 60, false, false, RbyIGTChecker<Red>.Verbosity.Full, false, -1, memeBall, 0, 1); // normal igt
        // return;
        for(ushort h = 7; h <= 41; ++h)
        {
            Trace.WriteLine("");
            Trace.WriteLine(h);
            Red[] gbs = MultiThread.MakeThreads<Red>(16);
            gbs[0].LoadState("basesaves/red/manip/rt3moon_slot2.gqs");
            // gbs[0].Show();
            rt3MoonIntro.ExecuteUntilIGT(gbs[0]);
            byte[] state = gbs[0].SaveState();
            Dictionary<string, int> results = new Dictionary<string, int>();
            MultiThread.For(60, gbs, (gb, i) => {
                gb.LoadState(state);
                // gb.CpuWrite("wPlayTimeSeconds", (byte)(i));
                // gb.CpuWrite("wPlayTimeFrames", (byte)(54));
                gb.CpuWrite("wPlayTimeSeconds", (byte)(0));
                gb.CpuWrite("wPlayTimeFrames", (byte)(i));
                rt3MoonIntro.ExecuteAfterIGT(gb);
                gb.CpuWriteBE<ushort>("wPartyMon1HP", h);
                gb.Execute(SpacePath(rt3Moon),
                    (gb.Maps[59][ 5, 31], gb.PickupItem),
                    (gb.Maps[59][34, 31], gb.PickupItem),
                    (gb.Maps[59][35, 23], gb.PickupItem),
                    (gb.Maps[61][28,  5], gb.PickupItem),
                    (gb.Maps[59][ 2,  3], gb.PickupItem),
                    (gb.Maps[59][ 3,  2], gb.PickupItem));
                // gb.SaveState("37hp.gqs");
                if(gb.EnemyMon.Species.Name != "PARAS") return;
                // gbs[0].Record("test");
                gb.RunUntil("WaitForTextScrollButtonPress");
                gb.AdvanceFrames(1); // missing textbox
                gb.Press(Joypad.B);
                gb.RunUntil("HandleMenuInput");
                gb.Press(Joypad.A | Joypad.Down, Joypad.A | Joypad.Down | Joypad.Right);
                // bool[] res = new bool[20];
                if(gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"]) return;
                gb.ClearText(Joypad.B);
                if(gb.BattleMon.HP == 0) return;
                gb.Press(Joypad.A);
                byte[] ballstate = gb.SaveState();
                for(int a = 0; a <= 4; ++a)
                {
                    for(int b = 0; b <= 4; ++b)
                    {
                        if(b == 0 && a != 0) continue;
                        for(int c = 8; c <= 9; ++c)
                        {
                            gb.LoadState(ballstate);
                            void Backout(int n)
                            {
                                if(n == 1) gb.Press(Joypad.B | Joypad.Right, Joypad.A);
                                else if(n == 2) gb.Press(Joypad.Select, Joypad.B, Joypad.A);
                                else if(n == 3) gb.Press(Joypad.A | Joypad.Up, Joypad.B);
                                else if(n == 4) gb.Press(Joypad.Select, Joypad.A | Joypad.Up, Joypad.B);
                            }
                            string trad(int n)
                            {
                                if(n == 1) return "i  ";
                                else if(n == 2) return "si ";
                                else if(n == 3) return "p  ";
                                else if(n == 4) return "sp ";
                                else if(n == 8) return "b  ";
                                else if(n == 9) return "sb ";
                                return "";
                            }
                            Backout(a);
                            Backout(b);
                            gb.RunUntil("HandleMenuInput_.getJoypadState");
                            if(c == 9) gb.Press(Joypad.Select);
                            gb.Press(Joypad.A | (gb.CpuRead("wCurrentMenuItem") == 0 ? Joypad.Down : Joypad.Left));
                            bool res = false;
                            if(gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"])
                                res = true;
                            lock(results) {
                                string str = trad(a) + trad(b) + trad(c);
                                results.TryAdd(str, 0);
                                if(res) results[str]++;
                            }
                            // Trace.WriteLine(a + " " + b + " " + c + ": " + res);
                            // gb.AdvanceFrames(60);
                        }
                    }
                }
            });
            foreach(var r in results)
            {
                Trace.WriteLine(r.Key + ": " + r.Value);
            }
        }
    }

    public static void EntrMoon()
    {
        string entrMoon = "UAUUUUULLLLLLLLALDD";
        entrMoon += "RUUUUURRRRURUURURRRRRRRRUUUUUUURRRRDDRDDDDDADDDDDDDDDDRRRRRRURRR";
        // entrMoon += "RUUUUURRRRURUURURRRRRRRRUUUUUUURRRRDRDDDDDDADDDDDDDDDDRRRRRRURRR"; // early ladder turn
        entrMoon += "UUUUUUUURUUUUUUUUUUULUUUUULLLUUUULLDDLLLLLALLLLLLLDDDDDD";
        // entrMoon += "UUUUUUUURUUUUUUUUUUULUUUUULLLUUUULLLLLLDDLALLLDDLLLLDDDD"; // original movement
        entrMoon += "LLALLDADLALLAL";
        entrMoon += "RRRUUULUR";
        entrMoon += "DDDDLLL";
        entrMoon += "URARRARRARRARU";
        entrMoon += "DDDADDALDLLLLALUULALUUUUUUUULLUAUULUULLL";
        entrMoon += "DADRARD";
        entrMoon += "DADDRRDDDDDDDDDRRRRRRRRRRRRRR";
        entrMoon += "RRUUURARRRDDRRRRRRUURRARDDDDDDDDLLLLDDDDDDDDDLLLLLLLLLLLLLLLLLLLLLLUUUUUUUUUUUAUUU";
        RbyIntroSequence entrMoonIntro = new RbyIntroSequence(RbyStrat.NoPalAB);
        CheckIGT("basesaves/red/manip/entrmoon.gqs", entrMoonIntro, entrMoon, "PARAS", 3600, false, true);
    }

    public static void YellowNido()
    {
        RbyIGTChecker<Yellow>.CheckIGT("basesaves/yellow/nido.gqs", new RbyIntroSequence(), "URARU", null, 3600, true);
    }

    public static void YellowMoon()
    {
        string path = "UAUUUUUUUUAUUURRRARRRURUUUUUURARRDDDDDDDDDDDDRDDDDDRRRRRRURRR"
        + "RAUUUAUUUUUUULUUAUUUUUUUUAUULLUUUUULULLLLLLLDDDLLLLDLLLDDLDDDADDDDDLLLLLLLALLLUULULLUUUUUUULAUUUU"
        + "DRRRD"
        + "DDDDDDDDRDDDDRRRARRRARRRARRRRRR"
        // + "DDDDDDDDRDDDDRRRARRRARRARRRRRRR" //3397
        // + "DDDDDDDRDDDDDRRRARRRARRRARRRRRR" //3377
        + "RRUUURRRDDARRRRARRUAURARRARDADDDADDDDADDLLDDADDDADDALLLLLALLLALLLLLLLLLLLLLLLUUUUUAUUUUAUUAUUUURUUAUUAUUU";
        // + "RRUUURRRDDARRRRARRUAURARRARDADDDADDDDADDLLDDADDDADDALLLLLALLALLLLLLLLLLLLLLLLUUUUUAUUUUAUUAUUUURUUAUUAUUU"; //3462
        RbyIGTChecker<Yellow>.CheckIGT("basesaves/yellow/moon.gqs", new RbyIntroSequence(), path, "", 3600, false, false, RbyIGTChecker<Yellow>.Verbosity.Full, false, -1, gb => true);
    }

    public static void PostNerd()
    {
        // string postnerd = "LLLLLLLLLLDDDADDRAR" + "RRRD"; // sf - 3300 & 3299
        // string postnerd = "LLLLLLLLLLDDDADRRAD" + "RRR"; // hw - 3359 & 3358
        // string postnerd = "LLLLLLLLLLDDDADRARD" + "RRR"; // 3359 3358
        string postnerd = "LLLLLLLLLLDDDDARRAD" + "RRR"; // 3359 3358
        RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.PalHold);
        CheckIGT("basesaves/red/manip/postnerd.gqs", intro, postnerd, "PARAS", 3600);
        CheckIGT("basesaves/red/manip/postnerd_redbar.gqs", intro, postnerd, "PARAS", 3600);
    }

    public static void NidoFrame33Backup()
    {
        RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.NoPal);
        Red gb = new Red();
        const Joypad PLD = Joypad.B, PLD_ball = Joypad.A;

        Dictionary<string, int> statsSuccess = new Dictionary<string, int>();

        gb.LoadState("basesaves/red/manip/nido.gqs");
        gb.HardReset();
        intro.ExecuteUntilIGT(gb);
        byte[] igtState = gb.SaveState();
        for(byte s = 0; s < 60; ++s)
        {
            gb.LoadState(igtState);

            gb.CpuWrite("wPlayTimeMinutes", 5);
            gb.CpuWrite("wPlayTimeSeconds", s);
            gb.CpuWrite("wPlayTimeFrames", 33);
            intro.ExecuteAfterIGT(gb);

            const string nidopath = "LLLULLUAULALDLDLLDADDADLALLALUUAU";
            int addr;
            addr = gb.Execute(SpacePath(nidopath));

            if(addr != gb.WildEncounterAddress)
            {
                Trace.WriteLine($"{s,2} 33,  No enc");
                continue;
            }
            if(gb.EnemyMon.Species.Name != "NIDORANM")
            {
                Trace.WriteLine($"{s,2} 33,  " + gb.EnemyMon.Species.Name);
                continue;
            }

            gb.AdvanceFrames(240);
            byte[] encounterState = gb.SaveState();

            // for(byte maxhp = 21; maxhp <= 23; ++maxhp)
            // for(byte hp = 10; hp <= maxhp; ++hp)
            // for(byte def = 12; def <= 14; ++def)
            // for(byte spd = 10; spd <= 12; ++spd)
            {
                gb.LoadState(encounterState);

                Trace.Write($"{s,2} 33,  ");

                // gb.CpuWriteBE<ushort>("wPartyMon1HP",      hp );
                // gb.CpuWriteBE<ushort>("wPartyMon1MaxHP",   maxhp );
                // gb.CpuWriteBE<ushort>("wPartyMon1Defense", def );
                // gb.CpuWriteBE<ushort>("wPartyMon1Speed",   spd );
                string stats = gb.CpuReadBE<ushort>("wPartyMon1HP") + "/" + gb.CpuReadBE<ushort>("wPartyMon1MaxHP") + " " + gb.CpuReadBE<ushort>("wPartyMon1Attack") + " " + gb.CpuReadBE<ushort>("wPartyMon1Defense") + " " + gb.CpuReadBE<ushort>("wPartyMon1Speed") + " " + gb.CpuReadBE<ushort>("wPartyMon1Special");
                // Trace.Write(stats + ", ");

                bool yoloball;

                gb.Hold(PLD, gb.SYM["ManualTextScroll"]); // nido appeared
                gb.Press(Joypad.A);

                gb.Hold(PLD, gb.SYM["PlayCry"]);
                gb.Press(Joypad.Down | Joypad.A, Joypad.A | Joypad.Left); // yoloball 1
                yoloball = gb.Hold(PLD_ball, gb.SYM["ItemUseBall.captured"], gb.SYM["ItemUseBall.failedToCapture"]) == gb.SYM["ItemUseBall.captured"];
                Trace.Write("Yoloball1: " + yoloball);
                if(yoloball)
                {
                    Trace.WriteLine("");
                    continue;
                }

                gb.Hold(PLD, gb.SYM["ManualTextScroll"]); // missed
                gb.Press(Joypad.A);
                gb.Hold(PLD, gb.SYM["EnemyCanExecuteMove"] + 0x7);

                addr = gb.RunUntil(gb.SYM["MoveMissed"], gb.SYM["ManualTextScroll"], gb.SYM["HandleMenuInput_"]); // get move info
                byte move = gb.CpuRead(gb.SYM["wEnemySelectedMove"]);
                if(move > 0)
                {
                    Trace.Write(", Move: " + gb.Moves[move].Name);
                }
                if(addr == gb.SYM["MoveMissed"])
                {
                    Trace.Write(" Miss");
                    addr = gb.RunUntil(gb.SYM["ManualTextScroll"], gb.SYM["HandleMenuInput_"]);
                }
                if(gb.CpuRead(gb.SYM["wCriticalHitOrOHKO"]) > 0)
                {
                    Trace.Write(" Crit");
                }

                if(addr == gb.SYM["ManualTextScroll"]) // crit / status
                {
                    gb.AdvanceFrames(9);
                    gb.Press(Joypad.B);
                }

                gb.Press(Joypad.A, Joypad.A | Joypad.Right); // yoloball 2
                // gb.Press(Joypad.A, Joypad.Select, Joypad.A); // select
                yoloball = gb.Hold(PLD_ball, gb.SYM["ItemUseBall.captured"], gb.SYM["ItemUseBall.failedToCapture"]) == gb.SYM["ItemUseBall.captured"];
                Trace.WriteLine(", Yoloball2: " + yoloball);
                statsSuccess.TryAdd(stats, 0);
                if(yoloball) statsSuccess[stats]++;
            }
        }

        foreach(var kv in statsSuccess)
        {
            Trace.WriteLine(kv.Key + " " + kv.Value);
        }
    }

    public static void ForestPath3()
    {
        RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.NoPal);
        Red gb = new Red();
        // gb.Record("test");

        gb.LoadState("basesaves/red/manip/nido.gqs");
        gb.HardReset();
        intro.ExecuteUntilIGT(gb);
        gb.CpuWrite("wPlayTimeMinutes", 0);
        gb.CpuWrite("wPlayTimeSeconds", 0);
        gb.CpuWrite("wPlayTimeFrames", 32);
        intro.ExecuteAfterIGT(gb);
        byte[] state = gb.SaveState();

        for(byte maxhp = 21; maxhp <= 23; ++maxhp)
            for(byte hp = 10; hp <= maxhp; ++hp)
            {
                gb.LoadState(state);
                gb.CpuWriteBE<ushort>("wPartyMon1HP", hp);
                gb.CpuWriteBE<ushort>("wPartyMon1MaxHP", maxhp);
                Trace.Write(gb.CpuReadBE<ushort>("wPartyMon1HP") + "/" + gb.CpuReadBE<ushort>("wPartyMon1MaxHP") + " ");

                gb.Execute(SpacePath("LLLULLUAULALDLDLLDADDADLALLALUUAU"));
                gb.Yoloball();

                gb.ClearText(Joypad.B);
                gb.Press(Joypad.A);
                gb.RunUntil("_Joypad");
                gb.AdvanceFrames(5); // 0 1 2 2 3

                gb.Press(Joypad.A, Joypad.Start);

                gb.Execute(SpacePath("DRRUUURRRRRRRRRRRRRRRRRRRRRURUUUUUURAUUUUUUUUUUUUUUUUUUUULUAUULLLUUUUUUUUUURRRARU"));
                gb.Yoloball();

                gb.ClearText(Joypad.A);
                gb.Press(Joypad.B);

                int adr = gb.Execute(SpacePath("UUUAULLLLLU" + "RUUUUUUU" + "UUURURRURRRRRUAUUUUUUUUUUUUUUUUUUAUUUUUUUUUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDDDDLLLLLUAUU"), (gb.Maps[51][25, 12], gb.PickupItem));
                if(adr == gb.WildEncounterAddress)
                    Trace.WriteLine(gb.EnemyMon.Species.Name + " " + gb.EnemyMon.Level);
                else
                    Trace.WriteLine("No encounter");
            }

        gb.AdvanceFrames(300);
        gb.Dispose();
    }

    public static void Cans(RbyIntroSequence intro = null, string path = null)
    {
        intro = new RbyIntroSequence(RbyStrat.NoPal);
        // intro = new RbyIntroSequence(RbyStrat.PalHold); // 60 igt (57)
        // intro = new RbyIntroSequence(RbyStrat.NoPalAB, RbyStrat.GfSkip, RbyStrat.Hop0, 1); // 60 igt (57)
        // intro = new RbyIntroSequence(RbyStrat.PalAB);
        // intro = new RbyIntroSequence(RbyStrat.Pal, RbyStrat.GfSkip, RbyStrat.Hop0, 1); // 59

        path = "SDALLLAURAUUUUUA"; // 60 cans - 3596/3600
        // path = "LLURUUUUUA"; // 59 cans - 3539/3600
        // path = "DALLLAURUUUUUA"; // 58 cans - 3477/3600
        // path = "DLLLURUUUUUA"; // 57 cans - 3420/3600
        // path = "DLLLU"+"RUUUUULUUUUUUURDA"; // xd
        // path = "DDLLLUURUUUUUA"; // fail 57 - 3419
        // path = "DDALLLUURUUUUUA"; // fail 58 - 3361
        // path = "DDLALLUURUUUUUA"; // fail 58 - 3361
        // path = "DLLLURRRRRUUUUUA"; // 60 igt (PalAB)

        int numFrames = 60*60;
        int numThreads = 16;

        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];

        gb.LoadState("basesaves/red/manip/cans.gqs");
        gb.HardReset();
        if(numThreads == 1)
            gb.Record("test");
        intro.ExecuteUntilIGT(gb);
        byte[] igtState = gb.SaveState();

        var full = new List<string>();
        var results = new Dictionary<(byte first, byte second), int>();

        MultiThread.For(numFrames, gbs, (gb, f) =>
        {
            gb.LoadState(igtState);
            gb.CpuWrite("wPlayTimeSeconds", (byte) (f / 60));
            gb.CpuWrite("wPlayTimeFrames", (byte) (f % 60));
            // gb.CpuWrite("wPlayTimeMinutes", (byte) (f % 60));
            // gb.CpuWrite("wPlayTimeSeconds", (byte) (54 + 3*(f / 2)));
            // gb.CpuWrite("wPlayTimeFrames", (byte) (36 + f % 2));

            intro.ExecuteAfterIGT(gb);
            gb.Execute(SpacePath(path));

            (byte first, byte second) cans = (gb.CpuRead("wFirstLockTrashCanIndex"), gb.CpuRead("wSecondLockTrashCanIndex"));
            lock(results)
            {
                full.Add($"{f / 60,2} {f % 60,2}: {cans.first},{cans.second}");
                if(!results.ContainsKey(cans))
                    results.Add(cans, 1);
                else
                    results[cans]++;
            }
        });
        full.Sort();
        foreach(string line in full)
            Trace.WriteLine(line);
        Trace.WriteLine("");
        foreach(var cans in results)
            Trace.WriteLine(cans.Key.first + "," + cans.Key.second + ": " + cans.Value);
    }
}
