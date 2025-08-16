using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

public class RedTasTest : RedBlueForceComparisons
{
    void AgathaBug()
    {
        LoadState("basesaves/red/agathabug.gqs");
        ForceTurn(new RbyTurn("EARTHQUAKE"));

        // return;

        string GetLine()
        {
            int pc = PC;
            if(pc >= 0x4000) pc |= CpuRead("hLoadedROMBank") << 16;
            string pcs = SYM.Contains(pc) ? SYM[pc] : String.Format("{0:x4}", pc);

            int sp = CpuReadLE<ushort>(SP);
            if(sp >= 0x4000) sp |= CpuRead("hLoadedROMBank") << 16;
            string sps = String.Format("{0:x4}", sp) + (SYM.Contains(sp) ? " (" + SYM[sp] + ")" : "");

            return "instruction: " + pcs + "  stack: " + sps + "  ly:" + (int) CpuRead(0xff44);
        }


        LoadState("basesaves/red/agathabug_test.gqs");

        for(int k = 0; k < 30; ++k)
        {
            const int maxhist = 100;
            string[] hist = new string[maxhist];
            int j = 0;
            string ins = GetLine();
            while(!ins.Contains("NULL+0040"))
            {
                RunFor(1);
                ins = GetLine();
                hist[j] = ins;
                j = (j + 1) % maxhist;
            }

            for(int i = j; i < maxhist; ++i)
                Console.WriteLine(hist[i]);
            for(int i = 0; i < j; ++i)
                Console.WriteLine(hist[i]);

            // RunUntil(SYM["RandomizeDamage.loop"] + 0x0);

            for(int i = 0; i < 100; ++i)
            {
                RunFor(1);
                Console.WriteLine(GetLine());
            }
            Console.WriteLine();
        }
    }
    void Bide()
    {
        Record("bide");
        LoadState("basesaves/red/bide2.gqs");
        CpuWriteBE<ushort>("wBattleMonSpeed", 13);

        RbyTurn.DefaultRoll = 1;
        ForceTurn(new RbyTurn("BUBBLE", SideEffect), new RbyTurn("TACKLE", Miss));
        ForceTurn(new RbyTurn("BUBBLE", SideEffect), new RbyTurn("BIDE", 2 * Turns));
        ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("BIDE"), false);
        ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("BIDE"), false, false);

        // ForceTurn(new RbyTurn("BUBBLE", SideEffect), new RbyTurn("BIDE", 3 * Turns));
        // ForceTurn(new RbyTurn("BUBBLE", SideEffect), new RbyTurn("BIDE"));
        // ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("BIDE"), true);
        // ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("BIDE"), false, false);

        // ForceTurn(new RbyTurn("BUBBLE"));
        // Yes();
        // SendOut("NIDORANM");
        // BattleSwitch("SQUIRTLE", new RbyTurn("BIDE", 3 * Turns));
        // ForceTurn(new RbyTurn("POTION", "SQUIRTLE"), new RbyTurn("BIDE"));
        // ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("BIDE"));
        // ForceTurnAndSplit(new RbyTurn("BUBBLE"), new RbyTurn("BIDE"));

        AdvanceFrames(60);
        Dispose();
    }
    void Wrap()
    {
        Record("wrap");

        LoadState("basesaves/red/wrap.gqs");

        ForceTurn(new RbyTurn("LEER", Miss), new RbyTurn("WRAP", 20 | 5 * Turns));
        ForceTurn(new RbyTurn());
        ForceTurn(new RbyTurn());
        ForceTurn(new RbyTurn());
        ForceTurn(new RbyTurn());
        ForceTurn(new RbyTurn("LEER", Miss), new RbyTurn("LEER", Miss));

        // LoadState("basesaves/red/wrapy.gqs");

        // ForceTurn(new RbyTurn("GUST", Crit), new RbyTurn("TACKLE", Miss));
        // ForceTurn(new RbyTurn("GUST", Miss), new RbyTurn("WRAP", 20 | 4 * Turns));
        // ForceTurn(new RbyTurn());
        // ForceTurn(new RbyTurn());
        // ForceTurn(new RbyTurn());
        // // ForceTurn(new RbyTurn(""), new RbyTurn(""));
        // ForceTurn(new RbyTurn("GUST", Miss), new RbyTurn("LEER", Miss));

        AdvanceFrames(60);
        Dispose();
    }
    void TestRange()
    {
        Record("testrange");
        void HP()
        {
            Console.WriteLine(CpuReadBE<ushort>("wEnemyMonHP"));
        }

        void PSRange()
        {
            LoadState("basesaves/red/psrange2.gqs");
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("POISON STING", 1));
            LoadState("basesaves/red/psrange2.gqs");
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("POISON STING", 39));
            LoadState("basesaves/red/psrange2.gqs");
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("POISON STING", 1 | Crit));
            LoadState("basesaves/red/psrange2.gqs");
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("POISON STING", 39 | Crit));
        }

        void Vileplume()
        {
            LoadState("basesaves/red/vileplume.gqs");
            ForceTurn(new RbyTurn("EARTHQUAKE", Miss), new RbyTurn("PETAL DANCE", 1));
            LoadState("basesaves/red/vileplume.gqs");
            ForceTurn(new RbyTurn("EARTHQUAKE", Miss), new RbyTurn("PETAL DANCE", 39));
            LoadState("basesaves/red/vileplume.gqs");
            ForceTurn(new RbyTurn("EARTHQUAKE", Miss), new RbyTurn("PETAL DANCE", 1 | Crit));
            LoadState("basesaves/red/vileplume.gqs");
            ForceTurn(new RbyTurn("EARTHQUAKE", Miss), new RbyTurn("PETAL DANCE", 39 | Crit));
        }

        void Kadabar()
        {
            LoadState("basesaves/red/kadabar.gqs");
            ForceTurn(new RbyTurn("POISON STING"), new RbyTurn("CONFUSION", 1));
            LoadState("basesaves/red/kadabar.gqs");
            ForceTurn(new RbyTurn("POISON STING"), new RbyTurn("CONFUSION", 39));
            LoadState("basesaves/red/kadabar.gqs");
            ForceTurn(new RbyTurn("POISON STING"), new RbyTurn("CONFUSION", 1 | Crit));
            LoadState("basesaves/red/kadabar.gqs");
            ForceTurn(new RbyTurn("POISON STING"), new RbyTurn("CONFUSION", 39 | Crit));
        }

        void Champ()
        {
            LoadState("basesaves/red/champrange.gqs");
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("SKY ATTACK", 1));
            LoadState("basesaves/red/champrange.gqs");
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("SKY ATTACK", 39));
            LoadState("basesaves/red/champrange.gqs");
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("SKY ATTACK", 1 | Crit));
            LoadState("basesaves/red/champrange.gqs");
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("SKY ATTACK", 39 | Crit));
        }

        void Lance()
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", 1));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("HYDRO PUMP", 39));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", 1 | Crit));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("HYDRO PUMP", 39 | Crit));
        }

        // ForceTurn(new RbyTurn("LEER"), new RbyTurn("QUICK ATTACK", 39 | Crit));

        void Agatha()
        {
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("HAZE"));
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("CONFUSE RAY"));
            ForceTurn(new RbyTurn("THUNDERBOLT", Hitself), new RbyTurn("CONFUSE RAY"));
            ForceTurn(new RbyTurn("THUNDERBOLT", Hitself), new RbyTurn("CONFUSE RAY"));
            ForceTurn(new RbyTurn("THUNDERBOLT", Hitself), new RbyTurn("CONFUSE RAY"));
            ForceTurn(new RbyTurn("THUNDERBOLT", Crit));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("BITE", 1));
            ForceTurn(new RbyTurn("EARTHQUAKE", Miss), new RbyTurn("BITE", 39));
            ForceTurn(new RbyTurn("EARTHQUAKE", Miss), new RbyTurn("BITE", 1 | Crit));
            ForceTurn(new RbyTurn("EARTHQUAKE", Miss), new RbyTurn("BITE", 39 | Crit));
        }

        void Nerd()
        {
            LoadState("basesaves/red/nerd.gqs"); HP();
            ForceTurn(new RbyTurn("MEGA PUNCH", 1 + 7), new RbyTurn("POUND", Miss)); HP();
            ForceTurn(new RbyTurn("WATER GUN", 5), new RbyTurn("POUND", Miss)); HP();
            LoadState("basesaves/red/nerd.gqs"); HP();
            ForceTurn(new RbyTurn("MEGA PUNCH", 1 + 7 + 8), new RbyTurn("POUND", Miss)); HP();
            ForceTurn(new RbyTurn("WATER GUN", 5), new RbyTurn("POUND", Miss), true, false); HP();
            LoadState("basesaves/red/nerd.gqs"); HP();
            ForceTurn(new RbyTurn("MEGA PUNCH", 1 + 7 + 8 + 7), new RbyTurn("POUND", Miss)); HP();
            ForceTurn(new RbyTurn("WATER GUN", 1), new RbyTurn("POUND", Miss), true, false); HP();

            LoadState("basesaves/red/nerd.gqs");
            CpuWriteBE<ushort>("wEnemyMonHP", 13); HP();
            SaveState("basesaves/red/nerd13.gqs");
            ForceTurn(new RbyTurn("WATER GUN", 1), null, true, false); HP();
            LoadState("basesaves/red/nerd.gqs");
            CpuWriteBE<ushort>("wEnemyMonHP", 12); HP();
            ForceTurn(new RbyTurn("WATER GUN", 1), null, true, false); HP();
            LoadState("basesaves/red/nerd.gqs");
            CpuWriteBE<ushort>("wEnemyMonHP", 13); HP();
            ForceTurn(new RbyTurn("WATER GUN", 5), null, true, false); HP();

            LoadState("basesaves/red/nerdkoffing.gqs");
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("SMOG", 1));
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("SMOG", 39));
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("SMOG", 1 | Crit));
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("SMOG", 39 | Crit));
        }

        PSRange();
        Vileplume();
        Kadabar();
        Champ();
        Lance();
        Agatha();
        Nerd();

        AdvanceFrames(300);
        Dispose();
    }
    void MistyAI()
    {
        Record("mistyai");

        LoadState("basesaves/red/misty2.gqs");

        ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn(AiItem));
        ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("BUBBLEBEAM"));
        ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("BUBBLEBEAM"));

        AdvanceFrames(60);
        Dispose();
    }
    void Disable()
    {
        Record("disable");
        LoadState("basesaves/red/disable.gqs");

        ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("DISABLE", "EARTHQUAKE", 1 * Turns));
        ForceTurn(new RbyTurn("THUNDERBOLT", Miss), new RbyTurn("POISON GAS"));
        ForceTurn(new RbyTurn("EARTHQUAKE"));

        // RbyTurn.defaultRoll = 20;
        // ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("DISABLE", 8 * Turns, "EARTHQUAKE"));
        // ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("HEADBUTT", Crit));
        // ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("CONFUSION"));

        AdvanceFrames(60);
        Dispose();
    }
    void Potion()
    {
        Record("potion");
        LoadState("basesaves/red/potion.gqs");

        ForceTurn(new RbyTurn("POTION", "SQUIRTLE"), new RbyTurn("STRING SHOT"));
        ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
        ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));

        AdvanceFrames(60);
        Dispose();
    }
    void FuryAttack()
    {
        // LoadState("basesaves/red/furyattack.gqs");
        Record("furyattack");

        // ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FURY ATTACK", 30 | Crit | 4 * Turns));
        for(int i = 0; i < 10; ++i) {
        LoadState("basesaves/red/furyattack2.gqs");
        CpuWriteBE<ushort>("wPartyMon1HP", 50);
        ClearText();
        ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FURY ATTACK", 20 | new Random().Next(2,5+1) * Turns));
        AdvanceFrames(120);
        }

        AdvanceFrames(60);
        Dispose();
    }
    void MoveRng()
    {
        // Record("moverng");

        // for(int i = 0; i < 256; ++i)
        // {
        //     LoadState("basesaves/red/agathaai.gqs");
        //     LoadState("basesaves/red/blackbeltai.gqs");
        //     LoadState("basesaves/red/champai.gqs");
        //     LoadState("basesaves/red/loreleiai1.gqs");
        //     AdvanceFrames(7);



        //     Hold(Joypad.B, SYM["Random"]);
        //     int addr = CpuReadLE<ushort>(SP);
        //     if(addr >= 0x4000) addr |= CpuRead("hLoadedROMBank") << 16;
        //     string address = SYM[addr];
        //     Console.WriteLine(address);

        //     Hold(Joypad.B, SYM["TrainerAI.getpointer+0006"]);
        //     A = i;
        //     if(ClearText(Joypad.B, 1, SYM["ExecuteEnemyMove"], SYM["MainInBattleLoop.AIActionUsedPlayerFirst"]) == SYM["ExecuteEnemyMove"])
        //         Console.WriteLine(i + " skip");
        //     else
        //         Console.WriteLine(i + " AI");

        //     Hold(Joypad.B, SYM["SelectEnemyMove.chooseRandomMove+0004"]);
        //     A = i;
        //     int r;
        //     while((r = Hold(Joypad.B, SYM["SelectEnemyMove.chooseRandomMove+0004"], SYM["SelectEnemyMove.moveChosen"], SYM["SelectEnemyMove.done"])) != SYM["SelectEnemyMove.done"])
        //     {
        //         if(r == SYM["SelectEnemyMove.chooseRandomMove+0004"]) Console.WriteLine("chooseRandomMove " + A);
        //         if(r == SYM["SelectEnemyMove.moveChosen"]) Console.WriteLine("moveChosen " + B);
        //         RunFor(1);
        //     }
        //     Console.WriteLine("done " + Moves[A].Name + "\n");

        //     Hold(Joypad.B, SYM["SelectEnemyMove.chooseRandomMove+0004"]);
        //     A = i;
        //     Hold(Joypad.B, SYM["SelectEnemyMove.moveChosen"]);
        //     Console.WriteLine(B);
        //     Hold(Joypad.B, SYM["SelectEnemyMove.done"]);
        //     Console.WriteLine(Moves[A].Name);
        // }



        // move 1: 0-62    -> 63/256
        // move 2: 63-126  -> 64/256
        // move 3: 127-189 -> 63/256
        // move 4: 190-255 -> 66/256

        // Random rng = new Random();
        int chooseRandomMove = SYM["SelectEnemyMove.chooseRandomMove+0004"];
        int done = SYM["SelectEnemyMove.done"];
        int random = SYM["Random_"] + 2;
        const int max = 100;
        var moves = new int[256];
        var rolls = new int[max, 256];
        var stats = new int[max, 5];
        var rdivstats = new int[10];
        rolls.Initialize();
        LoadState("basesaves/red/loreleiai1.gqs");
        byte[] state = SaveState();
        const int it = 1000000;
        Console.WriteLine("iterations: " + it);
        for(int i = 0; i < it; ++i)
        {
            // CpuWriteBE<ushort>("hRandomAdd", (ushort) rng.Next());
            int roll = 0;
            int rdiv = -1;
            while(Hold(Joypad.B, chooseRandomMove, done) == chooseRandomMove)
            {
                rolls[roll, A]++;
                RunFor(1);
                stats[roll, 0]++;
                if(A < 0x3f) stats[roll, 1]++;
                else if(A < 0x7f) stats[roll, 2]++;
                else if(A < 0xbe) stats[roll, 3]++;
                else stats[roll, 4]++;
                roll++;
                if(A < 0x3f || A >= 0xbe)
                {
                    Hold(Joypad.B, random);
                    if(rdiv >= 0)
                    {
                        rdivstats[(byte) (A - rdiv)]++;
                        rdivstats[0]++;
                    }
                    rdiv = A;
                }
            }
            moves[A]++;
            LoadState(state);
            AdvanceFrame();
            state = SaveState();
            if(i % 1000 == 999) Console.WriteLine(".");
        }
        for(int r = 0; r < max; ++r)
        {
            if(stats[r, 0] != 0)
            {
                Console.WriteLine("ROLL " + (r + 1));
                for(int i = 0; i < 256; ++i)
                    if(rolls[r, i] != 0)
                        Console.WriteLine("  " + i + ": " + rolls[r, i]);
                Console.WriteLine("  AURORA BEAM: " + stats[r, 2] + " (" + 100.0f * stats[r, 2] / stats[r, 0] + "%)");
                Console.WriteLine("  REST: " + stats[r, 3] + " (" + 100.0f * stats[r, 3] / stats[r, 0] + "%)");
                Console.WriteLine("  REROLL: " + stats[r, 1] + "+" + stats[r, 4] + "=" + (stats[r, 1] + stats[r, 4]) + " (" + 100.0f * (stats[r, 1] + stats[r, 4]) / stats[r, 0] + "%)");
            }
        }
        int rdivsum = 0;
        for(int i = 1; i < 10; ++i)
        {
            rdivsum += rdivstats[i] * i;
            if(rdivstats[i] > 0)
                Console.WriteLine("RDIV DELTA=" + i + ": " + rdivstats[i] + " (" + 100.0f * rdivstats[i] / rdivstats[0] + "%)");
        }
        Console.WriteLine("RDIV DELTA AVG: " + 1.0f * rdivsum / rdivstats[0]);
        Console.WriteLine();
        for(int i = 0; i < 256; ++i)
            if(moves[i] != 0)
                Console.WriteLine(Moves[i].Name + ": " + moves[i] + " (" + 100.0f * moves[i] / it + "%)");

        // iterations: 1000000
        // sim1
        // AURORA BEAM: 532773 (53,2773%)
        // REST: 467227 (46,7227%)
        // sim2
        // AURORA BEAM: 532471 (53,2471%)
        // REST: 467529 (46,7529%)

        AdvanceFrames(60);
        Dispose();
    }
    void LoreleiSim()
    {
        int aurorabeam = 0;
        int rest = 0;
        for(int basehra = 0; basehra < 256; ++basehra)
        {
            for(int baserdiv = 0; baserdiv < 256; ++baserdiv)
            {
                float hra = basehra;
                float rdiv = baserdiv;
                while(hra < 0x3f || hra >= 0xbe) // reroll
                {
                    hra = (hra + rdiv) % 256;
                    rdiv = (rdiv + 2.5592f) % 256;
                }
                if(hra < 0x7f)
                    ++aurorabeam;
                else
                    ++rest;
            }
        }
        Console.WriteLine("Aurora Beam: " + aurorabeam + " (" + 100.0f * aurorabeam / 65536 + "%)");
        Console.WriteLine("Rest: " + rest + " (" + 100.0f * rest / 65536 + "%)");
        // Aurora Beam: 34897 (53,248596%)
        // Rest: 30639 (46,751404%)
    }
    void Agatha()
    {
        Record("agatha");

        LoadState("basesaves/red/agatha.gqs");

        ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("DREAM EATER", Switch));
        ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn(AiItem));
        ForceTurn(new RbyTurn("EARTHQUAKE"));

        AdvanceFrames(60);
        Dispose();
    }
    void GenSquirtle()
    {
        ushort dvs = 0x8777;
        // Record("test");

        new RbyIntroSequence(RbyStrat.NoPal, RbyStrat.GfSkip, RbyStrat.Hop0, RbyStrat.Title0).Execute(this);
        Press(Joypad.Down | Joypad.A, Joypad.Left, Joypad.Down, Joypad.Left, Joypad.B, Joypad.A); // Options

        ClearText(Joypad.A);
        Press(Joypad.A, Joypad.None, Joypad.A, Joypad.Start); // Name self

        ClearText(Joypad.A);
        Press(Joypad.A, Joypad.None, Joypad.A, Joypad.Start); // Name rival
        ClearText(); // Journey begins!

        // PC potion
        TalkTo(0, 1);
        WithdrawItems("POTION", 1);

        MoveTo("PalletTown", 10, 1); // Oak cutscene
        ClearText();

        TalkTo(7, 3);
        Yes();
        ClearText();
        Yes();
        Press(Joypad.None, Joypad.A, Joypad.Start); // Name Squirtle
        ForceGiftDVs(dvs);
        ClearText(); // Squirtle received

        MoveTo(5, 6);
        ClearText();

        // RIVAL1
        ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("TACKLE"));
        ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("GROWL", Miss));
        ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("GROWL", Miss));
        ForceTurn(new RbyTurn("TACKLE", 1), new RbyTurn("GROWL", Miss));
        ForceTurn(new RbyTurn("TACKLE", 1), new RbyTurn("GROWL", Miss));
        ClearText();

        MoveTo("Route1", 14, 14);
        ForceEncounter(Action.Up, 1, 0x8888);
        ClearText();
        ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TACKLE"));
        ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TACKLE"));
        ForceTurn(new RbyTurn("TACKLE"));

        MoveTo("ViridianCity", 29, 19);
        ClearText(); // Receive parcel

        TalkTo("OaksLab", 5, 2, Action.Right); // give parcel

        TalkTo("ViridianMart", 1, 5);
        Buy("POKE BALL", 4);
        MoveTo("ViridianCity", 27, 18);

        MoveTo("ViridianCity", 7, 18, Action.Left);
        Save();
        SaveState($"basesaves/red/manip/nido{dvs:X4}.gqs");
    }
    static void TestVariance()
    {
        Red gb = new Red();
        gb.LoadState("basesaves/red/nerdvoltorb19.gqs");
        // gb.LoadState("basesaves/red/nerdvoltorb19.gqs");
        Console.WriteLine("a=" + gb.CpuRead("hRandomAdd") + " s=" + gb.CpuRead("hRandomSub") + " d=" + gb.DividerState + " t=" + gb.TimeNow);
        gb.AdvanceFrame(Joypad.B);
        for(int i = 0; i < 180; ++i)
        {
            gb.AdvanceFrame();
            Console.WriteLine("a=" + gb.CpuRead("hRandomAdd") + " s=" + gb.CpuRead("hRandomSub") + " d=" + gb.DividerState + " t=" + gb.TimeNow);
        }

        // gb.RunUntil("Joypad");
        // gb.Inject(Joypad.B);
        // gb.RunUntil("HandleMenuInput_");
        // Console.WriteLine("hra="+gb.CpuRead("hRandomAdd"));
    }
    void TestXAcc()
    {
        LoadState("basesaves/red/xacc.gqs");
        RecordAndTime("test", true);
        ClearText();
        // ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("TACKLE", Miss));
        ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE", Miss));
        MoveSwap("THRASH", "BUBBLEBEAM");
        ForceTurn(new RbyTurn("HORN DRILL"));
        ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("CONFUSION", Miss));
        ForceTurn(new RbyTurn("HORN DRILL"));
        Timer.Stop();
        AdvanceFrames(60);
        // ClearText();
        Dispose();
    }
    void Metronome()
    {
        // LoadState("basesaves/red/metronome.gqs");
        LoadState("basesaves/red/metronome2.gqs");
        Record("test");
        ClearText();
        ForceTurn(new RbyTurn("METRONOME", "GUILLOTINE"), new RbyTurn("TACKLE", Miss));
        ForceTurn(new RbyTurn("METRONOME", "RAZOR LEAF"), new RbyTurn("TACKLE", Miss));
        ForceTurn(new RbyTurn("METRONOME", "HYPER BEAM", Crit), new RbyTurn("TACKLE", Miss));
        AdvanceFrames(60);
        Dispose();
    }
    void TestPress()
    {
        // LoadState("basesaves/red/moonmenu.gqs");
        // LoadState("basesaves/red/xacc.gqs");
        LoadState("basesaves/red/pickupitem.gqs");
        Record("test");

        // SetOptions(Fast | Off | Set);
        // OpenStartMenu();
        // ChooseMenuItem(4 + StartMenuOffset());
        // Press(Joypad.Down, Joypad.None, Joypad.Down, Joypad.Right);

        // MoveTo(12, 9);
        // UseItem("RARE CANDY", "NIDORANM");
        // RunUntil("Evolution_PartyMonLoop.done");
        // UseItem("TM12", "NIDORINO", "TACKLE");
        // UseItem("MOON STONE", "NIDORINO");
        // UseItem("TM01", "NIDOKING", "LEER");

        // MoveTo(12, 9);
        // ItemSwap("POTION", "RARE CANDY");
        // UseItem("RARE CANDY", "NIDORANM");
        // RunUntil("Evolution_PartyMonLoop.done");

        // ClearText();
        // MoveSwap("HORN DRILL", "THUNDERBOLT");
        // ForceTurn(new RbyTurn("HORN DRILL"));

        OpenStartMenu();
        PickupItem();

        AdvanceFrames(60);
        Dispose();
    }
    void Deposit()
    {
        LoadState("basesaves/red/pc.gqs");
        Record("test");
        TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
        Deposit("SQUIRTLE", "PIDGEY", "NIDOKING");
        DepositItems("X SPECIAL", 3, "HM01", 1, "SUPER POTION", 0);
        Withdraw("SQUIRTLE", "NIDOKING");
        WithdrawItems("X SPECIAL", 2, "SUPER POTION", -1, "POTION", 1, "HM01", 1);
        Deposit("PARAS", "SQUIRTLE");
        MoveTo(14, 8);

        // LoadState("basesaves/red/pcy.gqs");
        // Record("test");
        // TalkTo(13, 4);
        // Deposit("NIDORANM");
        // DepositItems("POTION", 1);
        // Withdraw("NIDORANM");
        // WithdrawItems("POTION", 0);
        // Deposit("PIKACHU");
        // MoveTo(12, 4);

        // LoadState("basesaves/red/pcpotion.gqs");
        // RecordAndTime("test", true);
        // ClearText();
        // TalkTo(0, 1);
        // WithdrawItems("POTION", 1);
        // DepositItems("POTION", 0);
        // MoveTo(7, 1);
        // Timer.Stop();

        // LoadState("basesaves/red/pcpotiony.gqs");
        // RecordAndTime("test", true);
        // TalkTo(0, 1);
        // WithdrawItems("POTION", 1);
        // DepositItems("POTION", 0);
        // MoveTo(7, 1);
        // Timer.Stop();

        AdvanceFrames(60);
        Dispose();
    }
    void Shopping()
    {
        LoadState("basesaves/red/shopping.gqs");
        RecordAndTime("test", true);

        Press(Joypad.A);
        ClearText();
        Buy("X ACCURACY", 12);
        CloseMenu();

        Timer.Stop();
        AdvanceFrames(60);
        Dispose();
    }
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
    void FadeOut()
    {
        LoadState("basesaves/red/fadeout.gqs");
        RecordAndTime("test", true);
        Execute("L");
        Save();

        AdvanceFrames(24);
        NoPal.Execute(this, true);

        // AdvanceFrames(29);
        // AdvanceFrames(105);
        // NoPal.Execute(this);

        Execute("L");
        Timer.Stop();
        AdvanceFrames(100);
        Dispose();
    }
    void NpcTroll()
    {
        Record("test");

        LoadState("basesaves/red/npctrollpal.gqs");
        MoveNpc(0, 3, 8, Action.Down);
        Fly("PalletTown");
        MoveTo(4, 17, Action.Right);

        LoadState("basesaves/red/npctrollpal.gqs");
        Fly("PalletTown");
        MoveNpc(12, 5, 24, Action.Right);
        MoveNpc(12, 15, 13, Action.Left);
        MoveTo(12, 14, 14);
        MoveTo(12, 14, 5);

        LoadState("basesaves/red/npctrollceru.gqs");
        MoveNpc(3, 15, 18, Action.Down);
        MoveNpc(3, 9, 21, Action.Up);
        MoveNpc(64, 3, 1, Action.Up);
        MoveNpc(64, 4, 3, Action.Left);
        MoveTo(3, 14, 18);
        TalkTo("CeruleanPokecenter", 3, 2);

        LoadState("basesaves/red/npctrollvermi.gqs");
        MoveNpc(5, 19, 7, Action.Right);
        MoveTo(5, 21, 4);
        MoveTo(5, 21, 5);
        MoveNpc(5, 19, 7, Action.Right);
        MoveTo(5, 21, 10);

        LoadState("basesaves/red/npctrollvermi.gqs");
        MoveNpc(5, 19, 7, Action.Right);
        MoveTo(5, 19, 6);
        MoveNpc(5, 19, 7, Action.Left);
        MoveTo(5, 19, 12);

        AdvanceFrames(100);
        Dispose();
    }
    void Cans()
    {
        RecordAndTime("test");

        bool FirstLockOpened() { return CheckEventFlag(0x161); }
        // bool SecondLockOpened() { return CheckEventFlag(0x160); }
        void CheckCan(byte id, Action dir = Action.None)
        {
            TalkTo(1 + (id / 3) * 2, 7 + (id % 3) * 2, dir);
        }
        void ForceSecondCan(byte id, Action dir = Action.None)
        {
            CpuWrite("wSecondLockTrashCanIndex", id);
            CheckCan(id, dir);
        }

        System.Action Cans123 = () => {
            CheckCan(2, Action.Up);
            if(FirstLockOpened()) { ForceSecondCan(5, Action.Up); return; }
            CheckCan(8, Action.Right);
            if(FirstLockOpened()) { ForceSecondCan(5); return; }
            CheckCan(4, Action.Left);
            if(FirstLockOpened()) { ForceSecondCan(7); return; }
            CheckCan(6, Action.Right);
            if(FirstLockOpened()) { ForceSecondCan(3); return; }
            CheckCan(0, Action.Down);
            if(FirstLockOpened()) { ForceSecondCan(0); return; }
            CheckCan(10, Action.Down);
            if(FirstLockOpened()) { ForceSecondCan(13, Action.Right); return; }
            CheckCan(12, Action.Up);
            if(FirstLockOpened()) { ForceSecondCan(13); return; }
            CheckCan(14, Action.Down);
            if(FirstLockOpened()) { ForceSecondCan(13); return; }
        };

        System.Action Cans128 = () => {
            CheckCan(2, Action.Up);
            if(FirstLockOpened()) { ForceSecondCan(5, Action.Up); return; }
            CheckCan(8, Action.Up);
            if(FirstLockOpened()) { ForceSecondCan(5, Action.Left); return; }
            CheckCan(14, Action.Right);
            if(FirstLockOpened()) { ForceSecondCan(13, Action.Right); return; }
            CheckCan(10, Action.Left);
            if(FirstLockOpened()) { ForceSecondCan(13); return; }
            CheckCan(12, Action.Up);
            if(FirstLockOpened()) { ForceSecondCan(13); return; }
            CheckCan(6, Action.Up);
            if(FirstLockOpened()) { ForceSecondCan(7); return; }
            CheckCan(4, Action.Left);
            if(FirstLockOpened()) { ForceSecondCan(7); return; }
            CheckCan(0, Action.Down);
            if(FirstLockOpened()) { ForceSecondCan(0); return; }
        };

        List<double> list = new List<double>();
        for(byte can1 = 0; can1 <= 14; can1 += 2)
        {
            LoadState("basesaves/red/cans.gqs");
            Timer.Start();
            MoveTo(12, 19);
            CpuWrite("wFirstLockTrashCanIndex", can1);
            // Cans123();
            Cans128();
            MoveTo(5, 2, Action.Up);
            Timer.Stop();
            Trace.WriteLine(can1 + ": " + Timer.Duration().TotalSeconds);
            list.Add(Timer.Duration().TotalSeconds);
        }
        SimulationUtils.PrintResults("", list);

        AdvanceFrames(100);
        Dispose();
    }
    bool CheckEventFlag(int flag)
    {
        int offs = flag / 8;
        int bit = flag % 8;
        return (CpuRead(SYM["wEventFlags"] + offs) & (1 << bit)) > 0;
    }
    public void Pathfinding()
    {
        Record("test");

        LoadState("basesaves/red/pathfinding3.gqs");
        MoveTo(4, 4);

        LoadState("basesaves/red/pathfinding4.gqs");
        PickupItemAt(234, 2, 12);

        LoadState("basesaves/red/postsilphgio.gqs");
        ClearText();
        TalkTo(236, 3, 0);

        LoadState("basesaves/red/champ.gqs");
    }
    void MirrorMove()
    {
        Record("mirrormove");
        LoadState("basesaves/red/champ.gqs");
        ClearText();
        ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
        ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("SKY ATTACK", Miss));
        ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE", Miss));
        ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
    }

    public RedTasTest() : base()
    {
        Bide();
        Environment.Exit(0);
    }
}
