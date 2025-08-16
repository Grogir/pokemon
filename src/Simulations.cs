using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class Simulations
{
    public static void NerdVoltorb()
    {
        var s = new Simulation<Red>("basesaves/red/nerdvoltorb.gqs").Track("Time", "HP", "Success");
        s.Simulate("WG + PS", gb =>
        {
            while(gb.EnemyMon.Species.Name == "VOLTORB" && gb.BattleMon.HP > 0)
            {
                // if(gb.EnemyMon.HP > 10) gb.UseMove("WATER GUN"); else gb.UseMove("POISON STING");
                if(gb.EnemyMon.HP == 33 || (gb.EnemyMon.HP >= 16 && gb.EnemyMon.HP <= 20)) gb.UseMove("WATER GUN"); else gb.UseMove("POISON STING");
            }
            return gb.BattleMon.HP > 0;
        });
        s.Simulate("Spam PS", gb =>
        {
            while(gb.EnemyMon.Species.Name == "VOLTORB" && gb.BattleMon.HP > 0)
                gb.UseMove("POISON STING");
            return gb.BattleMon.HP > 0;
        });
    }

    public static void BC2Caterpie()
    {
        Action<Red> Metapod = gb =>
        {
            while(gb.EnemyMon.Species.Name == "METAPOD" && gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
                gb.UseMove("HORN ATTACK");
        };
        Func<Red, bool> Leer_HA = gb =>
        {
            gb.ClearText();
            while(gb.EnemyMon.Species.Name == "CATERPIE" && gb.BattleMon.HP > 0)
            {
                if(gb.EnemyMon.DefenseModifider == 7)
                    gb.UseMove("LEER");
                else if(gb.EnemyMon.HP > 17)
                    gb.UseMove("HORN ATTACK");
                else
                    gb.UseMove("TACKLE");
            }
            Metapod(gb);
            return gb.BattleMon.HP > 0;
        };
        Func<Red, bool> HA_Tackle = gb =>
        {
            gb.ClearText();
            while(gb.EnemyMon.Species.Name == "CATERPIE" && gb.BattleMon.HP > 0)
            {
                if(gb.EnemyMon.HP > 10)
                    gb.UseMove("HORN ATTACK");
                else
                    gb.UseMove("TACKLE");
            }
            Metapod(gb);
            return gb.BattleMon.HP > 0;
        };
        Func<Red, bool> Tackle_HA = gb =>
        {
            gb.ClearText();
            while(gb.EnemyMon.Species.Name == "CATERPIE" && gb.BattleMon.HP > 0)
            {
                if(gb.EnemyMon.HP == 28)
                    gb.UseMove("TACKLE");
                else
                    gb.UseMove("HORN ATTACK");
            }
            Metapod(gb);
            return gb.BattleMon.HP > 0;
        };

        var s = new Simulation<Red>().Track("Time", "HP", "Success");
        s.Simulate("Leer + HA (cursor on HA)", "basesaves/red/bc2caterpieha30.gqs", Leer_HA);
        s.Simulate("HA + Tackle (cursor on HA)", "basesaves/red/bc2caterpieha30.gqs", HA_Tackle);
        s.Simulate("\n\n\nLeer + HA (cursor on Tackle)", "basesaves/red/bc2caterpieta30.gqs", Leer_HA);
        s.Simulate("HA + Tackle (cursor on Tackle)", "basesaves/red/bc2caterpieta30.gqs", HA_Tackle);
        s.Simulate("Tackle + HA (cursor on Tackle)", "basesaves/red/bc2caterpieta30.gqs", Tackle_HA);
    }

    public static void ClassicBlackbelt()
    {
        var s = new Simulation<Red>("basesaves/red/blackbeltclassic.gqs", 65536).Track("Time", "Success");

        for(int atk = 104; atk <= 121; ++atk)
        {
            s.Simulate("EQ (" + atk + " Atk)", gb =>
            {
                gb.CpuWriteBE<ushort>("wBattleMonAttack", (ushort)(atk * 9 / 8));
                while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
                {
                    if(gb.EnemyMon.HP < 50) gb.UseMove("THUNDERBOLT"); else gb.UseMove("EARTHQUAKE");
                }
                return gb.BattleMon.HP > 0;
            });
        }
        s.Simulate("X Acc + HD", gb =>
        {
            gb.UseItem("X ACCURACY");
            // while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            // {
            //     gb.UseMove("HORN DRILL");
            // }
            return gb.BattleMon.HP > 0;
        });
    }

    public static void Route1NPC(bool side)
    {
        const int it = 65536;
        var s = new Simulation<Red>("basesaves/red/route1npc.gqs", it);

        void RunAway(Rby gb) {
            gb.ClearText();
            gb.CpuWriteBE<ushort>("wBattleMonSpeed", 20);
            gb.BattleMenu(1, 1);
            gb.ClearText();
        }

        List<double> listencounters = new List<double>(it);
        List<double> liststeps = new List<double>(it);
        List<double> listtrolled = new List<double>(it);
        List<double> listcollision = new List<double>(it);

        Red r = new Red();
        int collisionaddress = r.SYM["CollisionCheckOnLand.collision"];
        int encounteraddress = r.SYM["CalcStats"];

        string path = side ? "URRRRUUUU" : "URRUUUURR";

        s.Simulate("https://gunnermaniac.com/pokeworld?local=12#9/19/" + path, gb =>
        {
            int encounters = 0;
            int steps = 0;
            int trolled = 0;
            int collision = 0;
            int addr, x;

            void Move(string dir)
            {
                addr = gb.Execute(dir);
                while(addr == collisionaddress)
                {
                    collision = 1;
                    addr = gb.Execute(dir);
                }
                steps++;
                if(addr == encounteraddress)
                {
                    encounters++;
                    RunAway(gb);
                }
            }
            void Decision1414()
            {
                x = gb.CpuRead("wSprite02StateData2MapX") - 4;
                if(x == 14)
                {
                    trolled++;
                    Move("R");
                    Decision1514();
                }
                else
                {
                    Move("U");
                    Move("U");
                    Move("U");
                }
            }
            void Decision1514()
            {
                x = gb.CpuRead("wSprite02StateData2MapX") - 4;
                if(x == 15)
                {
                    trolled++;
                    Move("L");
                    Decision1414();
                }
                else
                {
                    Move("U");
                    Move("U");
                    Move("U");
                    Move("L");
                }
            }

            gb.Execute(RbyForceComparisons.SpacePath(path));
            Move("R");
            Decision1414();

            lock(listencounters) {
                listencounters.Add(encounters);
                liststeps.Add(steps);
                listtrolled.Add(trolled);
                listcollision.Add(collision);
            }

            return true;
        });

        SimulationUtils.PrintResults("Encounters", listencounters);
        SimulationUtils.PrintResults("Steps", liststeps);
        SimulationUtils.PrintResults("Trolled", listtrolled);
        SimulationUtils.PrintResults("Bonks", listcollision);
    }

    public static void YoloLance()
    {
        var s = new Simulation<Red>("basesaves/red/lanceyolo.gqs", 65536).Track("Success", "Time");
        s.Simulate("TB", gb =>
        {
            while(gb.EnemyMon.Species.Name == "GYARADOS" && gb.BattleMon.HP > 0)
            {
                gb.UseMove("THUNDERBOLT");
            }
            return gb.BattleMon.HP > 0;
        });
        s.Simulate("HD", gb =>
        {
            while(gb.EnemyMon.Species.Name == "GYARADOS" && gb.BattleMon.HP > 0)
            {
                gb.UseMove("HORN DRILL");
            }
            return gb.BattleMon.HP > 0;
        });
        s.Simulate("X Spec", "basesaves/red/lance.gqs", gb =>
        {
            gb.UseItem("X SPECIAL");
            while(gb.BattleMon.HP > 100)
                gb.UseMove("EARTHQUAKE");
            while(gb.EnemyMon.Species.Name == "GYARADOS" && gb.BattleMon.HP > 0)
                gb.UseMove("THUNDERBOLT");
            return gb.BattleMon.HP > 0;
        });
    }

    public static void NpcIgt()
    {
        var s = new Simulation<Red>("basesaves/red/npcigt.gqs", 65536);
        int[] npcTimer = new int[256];
        s.Simulate("Npc Igt", gb =>
        {
            gb.CpuWrite("wGrassRate", 0);
            gb.Execute("L L L L L L U U U U U U U U");
            // gb.Execute("D D U U");
            gb.AdvanceFrames(1000);
            gb.Press(Joypad.Start);
            npcTimer[gb.CpuRead("wSprite04StateData2MovementDelay")]++;
            return true;
        });

        for(int i = 0; i < 256; ++i)
            Trace.WriteLine(i + " " + npcTimer[i]);
    }

    public static void SabrinaXSpeed()
    {
        // Red r = new Red();
        // r.LoadState("basesaves/red/sabrinazam.gqs");
        // r.CpuWrite(r.SYM["wBattleMonSpeed"] + 1, 115);
        // r.SaveState("basesaves/red/sabrinazam_speedtied.gqs");
        // r.CpuWrite(r.SYM["wBattleMonSpeed"] + 1, 114);
        // r.SaveState("basesaves/red/sabrinazam_slower.gqs");
        var s = new Simulation<Red>("basesaves/red/sabrinaxspeed.gqs", 65536).Track("Success", "Time");
        s.Simulate("X Speed", gb =>
        {
            gb.UseItem("X SPEED");
            Console.Write("xspd");
            while(gb.EnemyMon.Species.Name == "MR.MIME" && gb.BattleMon.HP > 0)
            {
                Console.Write(" " + gb.EnemyMon.HP + ":" + (gb.EnemyMon.HP == gb.EnemyMon.MaxHP ? "eq" : "tb"));
                if(gb.EnemyMon.HP == gb.EnemyMon.MaxHP)
                    gb.UseMove("EARTHQUAKE");
                else
                    gb.UseMove("THUNDERBOLT");
            }
            Console.WriteLine(gb.BattleMon.HP > 0 ? " win" : " dead");
            return gb.BattleMon.HP > 0;
        });
        s.Simulate("EQ", gb =>
        {
            while(gb.EnemyMon.Species.Name == "MR.MIME" && gb.BattleMon.HP > 0)
            {
                if(gb.EnemyMon.HP == gb.EnemyMon.MaxHP)
                    gb.UseMove("EARTHQUAKE");
                else
                    gb.UseMove("THUNDERBOLT");
            }
            return gb.BattleMon.HP > 0;
        });
        s.Simulate("Zam Faster", "basesaves/red/sabrinazam.gqs", gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
                gb.UseMove("EARTHQUAKE");
            return gb.BattleMon.HP > 0;
        });
        s.Simulate("Zam Speedtied", "basesaves/red/sabrinazam_speedtied.gqs", gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
                gb.UseMove("EARTHQUAKE");
            return gb.BattleMon.HP > 0;
        });
        s.Simulate("Zam Slower", "basesaves/red/sabrinazam_slower.gqs", gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
                gb.UseMove("EARTHQUAKE");
            return gb.BattleMon.HP > 0;
        });
    }

    public static void AgathaXSpeedSkip()
    {
        // Red r = new Red();
        // r.LoadState("basesaves/red/agathaclassic2.gqs");
        // r.CpuWriteBE<ushort>("wPartyMon2DVs", 0xffe7);
        // r.CpuWriteBE<ushort>("wPartyMon2Special", 105);
        // r.CpuWriteBE<ushort>("wPartyMon2HP", 130);
        // r.ClearText();
        Func<Red, bool> XSpeed = gb =>
        {
            gb.UseItem("X SPEED");
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.Asleep)
                    gb.UseItem("POKE FLUTE");
                else if(gb.BattleMon.Paralyzed && gb.Bag.IndexOf("PARLYZ HEAL") != -1)
                    gb.UseItem("PARLYZ HEAL", 1);
                else if(gb.EnemyMon.Species.Name == "GOLBAT")
                    gb.UseMove("ICE BEAM");
                else
                    gb.UseMove("EARTHQUAKE");
            }
            return gb.BattleMon.HP > 0;
        };

        Func<Red, bool> Skip = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.Asleep)
                    gb.UseItem("POKE FLUTE");
                else if(gb.BattleMon.Paralyzed && gb.Bag.IndexOf("PARLYZ HEAL") != -1)
                    gb.UseItem("PARLYZ HEAL", 1);
                else if(gb.EnemyMon.Frozen && gb.BattleMon.SpeedModifider == 7)
                    gb.UseItem("X SPEED");
                else if(gb.EnemyMon.Species.Name == "GOLBAT")
                    gb.UseMove("ICE BEAM");
                else
                    gb.UseMove("EARTHQUAKE");
            }
            return gb.BattleMon.HP > 0;
        };

        var s = new Simulation<Red>(10000).Track("Success", "Time");
        s.Simulate("X Speed (130 HP)", "basesaves/red/agathaclassic_130.gqs", XSpeed);
        s.Simulate("X Speed (80 HP)", "basesaves/red/agathaclassic_80.gqs", XSpeed);
        s.Simulate("X Speed (30 HP)", "basesaves/red/agathaclassic_30.gqs", XSpeed);
        s.Simulate("Skip (130 HP)", "basesaves/red/agathaclassic_130.gqs", Skip);
        s.Simulate("Skip (30 HP)", "basesaves/red/agathaclassic_30.gqs", Skip);
    }

    public static void AgathaCandy()
    {
        Func<Red, bool> Candy = gb =>
        {
            gb.UseItem("X SPECIAL");
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.Asleep)
                    gb.UseItem("POKE FLUTE");
                else if(gb.BattleMon.Paralyzed && gb.Bag.IndexOf("PARLYZ HEAL") != -1)
                    gb.UseItem("PARLYZ HEAL", 1);
                else if(gb.EnemyMon.Species.Name == "GOLBAT" && gb.BattleMon.PP[3] > 0 && gb.BattleMon.SpecialModifider > 7)
                    gb.UseMove("BLIZZARD");
                else if(gb.EnemyMon.Species.Name == "GOLBAT")
                    gb.UseMove("THUNDERBOLT");
                else
                    gb.UseMove("EARTHQUAKE");
            }
            return gb.BattleMon.HP > 0;
        };

        Func<Red, bool> Candyless = gb =>
        {
            gb.UseItem("X SPEED");
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.Asleep)
                    gb.UseItem("POKE FLUTE");
                else if(gb.BattleMon.Paralyzed && gb.Bag.IndexOf("PARLYZ HEAL") != -1)
                    gb.UseItem("PARLYZ HEAL", 1);
                else if(gb.EnemyMon.Species.Name == "GOLBAT" && gb.BattleMon.PP[3] > 0 && gb.EnemyMon.HP > 100)
                    gb.UseMove("BLIZZARD");
                else if(gb.EnemyMon.Species.Name == "GOLBAT")
                    gb.UseMove("THUNDERBOLT");
                else if(gb.BattleMon.PP[1] > 0)
                    gb.UseMove("EARTHQUAKE");
                else
                    gb.UseItem("ELIXER", 1);
            }
            return gb.BattleMon.HP > 0;
        };

        Func<Red, bool> CandylessTB = gb =>
        {
            gb.UseItem("X SPEED");
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.Asleep)
                    gb.UseItem("POKE FLUTE");
                else if(gb.BattleMon.Paralyzed && gb.Bag.IndexOf("PARLYZ HEAL") != -1)
                    gb.UseItem("PARLYZ HEAL", 1);
                else if(gb.EnemyMon.Species.Name == "GOLBAT")
                    gb.UseMove("THUNDERBOLT");
                else if(gb.BattleMon.PP[1] > 0)
                    gb.UseMove("EARTHQUAKE");
                else
                    gb.UseItem("ELIXER", 1);
            }
            return gb.BattleMon.HP > 0;
        };

        Func<Red, bool> CandylessXSpec = gb =>
        {
            gb.UseItem("X SPECIAL");
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.Asleep)
                    gb.UseItem("POKE FLUTE");
                else if(gb.BattleMon.Paralyzed && gb.Bag.IndexOf("PARLYZ HEAL") != -1)
                    gb.UseItem("PARLYZ HEAL", 1);
                else if(gb.EnemyMon.Species.Name == "GOLBAT" && gb.BattleMon.PP[3] > 0 && gb.BattleMon.SpecialModifider > 7)
                    gb.UseMove("BLIZZARD");
                else if(gb.EnemyMon.Species.Name == "GOLBAT")
                    gb.UseMove("THUNDERBOLT");
                else if(gb.BattleMon.PP[1] > 0)
                    gb.UseMove("EARTHQUAKE");
                else
                    gb.UseItem("ELIXER", 1);
            }
            return gb.BattleMon.HP > 0;
        };

        Func<Red, bool> HazedTB = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.Asleep)
                    gb.UseItem("POKE FLUTE");
                else if(gb.BattleMon.Paralyzed && gb.Bag.IndexOf("PARLYZ HEAL") != -1)
                    gb.UseItem("PARLYZ HEAL", 1);
                else if(gb.EnemyMon.Species.Name == "GOLBAT")
                    gb.UseMove("THUNDERBOLT");
                else
                    gb.UseMove("EARTHQUAKE");
            }
            return gb.BattleMon.HP > 0;
        };

        Func<Red, bool> HazedBlizz = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.Asleep)
                    gb.UseItem("POKE FLUTE");
                else if(gb.BattleMon.Paralyzed && gb.Bag.IndexOf("PARLYZ HEAL") != -1)
                    gb.UseItem("PARLYZ HEAL", 1);
                else if(gb.EnemyMon.Species.Name == "GOLBAT" && gb.BattleMon.PP[3] > 0 && gb.EnemyMon.HP > 100)
                    gb.UseMove("BLIZZARD");
                else if(gb.EnemyMon.Species.Name == "GOLBAT")
                    gb.UseMove("THUNDERBOLT");
                else
                    gb.UseMove("EARTHQUAKE");
            }
            return gb.BattleMon.HP > 0;
        };

        var s = new Simulation<Red>(10000).Track("Success", "Time");
        s.Simulate("Candy", "basesaves/red/agathacandy2.gqs", Candy);
        s.Simulate("Candyless", "basesaves/red/agathacandyskip2.gqs", Candyless);
        s.Simulate("CandylessTB", "basesaves/red/agathacandyskip2.gqs", CandylessTB);
        s.Simulate("CandylessXSpec", "basesaves/red/agathacandyskip2.gqs", CandylessXSpec);
        s.Simulate("HazedTB", "basesaves/red/agathahazed.gqs", HazedTB);
        s.Simulate("HazedBlizz", "basesaves/red/agathahazed.gqs", HazedBlizz);
    }

    public static void MoonRocket()
    {
        bool Fight(Red gb, byte startPP = 5, ushort startHP = 41, int ppToLeerRat = 2)
        {
            gb.CpuWrite(gb.SYM["wBattleMonPP"] + 2, startPP);
            gb.CpuWriteBE("wPartyMon1HP", startHP);

            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                bool leer = gb.EnemyMon.DefenseModifider == 6;
                bool bb = gb.BattleMon.Attack > 28;
                int pp = gb.BattleMon.PP[2];
                int hp = gb.EnemyMon.HP;

                if(gb.EnemyMon.Species.Name == "RATTATA") {
                    if(hp == gb.EnemyMon.MaxHP && !leer && pp <= ppToLeerRat)
                        gb.UseMove("LEER");
                    else if(hp <= 6 || bb && hp <= 7 || leer && hp <= 8 || bb && leer && hp <= 10)
                        gb.UseMove("POISON STING");
                    else if(hp <= 8 || bb && hp <= 9 || leer && hp <= 12)
                        gb.UseMove("TACKLE");
                    else if(pp >= 2)
                        gb.UseMove("HORN ATTACK");
                    else
                        gb.UseMove("TACKLE");
                }
                else if(gb.EnemyMon.Species.Name == "ZUBAT") {
                    if(hp == gb.EnemyMon.MaxHP) {
                        if(pp >= 3 || leer && pp == 2) // ha w/ badge boost
                            gb.UseMove("HORN ATTACK");
                        else if(leer)
                            gb.UseMove("TACKLE");
                        else
                            gb.UseMove("LEER");
                    }
                    else if(hp <= 2 || bb && hp <= 3 || leer && hp <= 4 || bb && leer && hp <= 5)
                        gb.UseMove("POISON STING");
                    else if(pp > 0)
                        gb.UseMove("HORN ATTACK");
                    else
                        gb.UseMove("TACKLE");
                }
            }
            return gb.BattleMon.HP > 0;
        }

        var s = new Simulation<Red>(10000).Track("Success", "Time");
        s.Simulate("HA"  , "basesaves/red/moonrocket.gqs", gb => Fight(gb, 2, 41, 1));
        s.Simulate("Leer", "basesaves/red/moonrocket.gqs", gb => Fight(gb, 2, 41, 2));
    }

    public static void BridgeRival()
    {
        Func<Red, bool> HAFirst = gb =>
        {
            while(gb.EnemyMon.Species.Name == "PIDGEOTTO" && gb.BattleMon.HP > 0)
            {
                if(gb.EnemyMon.HP <= 7)
                    gb.UseMove("POISON STING");
                else if(gb.EnemyMon.HP >= 18 && gb.EnemyMon.HP <= 22)
                    gb.UseMove("MEGA PUNCH");
                else
                    gb.UseMove("HORN ATTACK");
            }
            return gb.BattleMon.HP > 0;
        };
        Func<Red, bool> MPFirst = gb =>
        {
            while(gb.EnemyMon.Species.Name == "PIDGEOTTO" && gb.BattleMon.HP > 0)
            {
                if(gb.EnemyMon.HP == gb.EnemyMon.MaxHP && gb.BattleMon.PP[0] >= 18)
                    gb.UseMove("MEGA PUNCH");
                else if(gb.EnemyMon.HP <= 7)
                    gb.UseMove("POISON STING");
                else if(gb.EnemyMon.HP >= 18 && gb.EnemyMon.HP <= 22)
                    gb.UseMove("MEGA PUNCH");
                else
                    gb.UseMove("HORN ATTACK");
            }
            return gb.BattleMon.HP > 0;
        };

        var s = new Simulation<Red>(65536).Track("Success", "Time");
        s.Simulate("HAFirst", "basesaves/red/bridgerival.gqs", HAFirst);
        s.Simulate("MPFirst", "basesaves/red/bridgerival.gqs", MPFirst);
    }

    public static void LavenderRival()
    {
        Func<Red, bool> EarlyDrill = gb =>
        {
            gb.UseItem("X ACCURACY");
            if(gb.BattleMon.HP > 0) gb.UseMove("HORN DRILL");
            return gb.BattleMon.HP > 0;
        };
        Func<Red, bool> EarlyDrillPotion = gb =>
        {
            gb.UseItem("POTION", 0);
            if(gb.BattleMon.HP > 0) gb.UseItem("X ACCURACY");
            if(gb.BattleMon.HP > 0) gb.UseMove("HORN DRILL");
            return gb.BattleMon.HP > 0;
        };
        Func<Red, bool> EarlyDrillGrowlithe = gb =>
        {
            while(gb.BattleMon.HP > 0 && !gb.BattleMon.XAccuracyEffect)
            {
                if(gb.EnemyMon.Species.Name == "PIDGEOTTO")
                    gb.UseMove("THUNDERBOLT");
                else if(gb.EnemyMon.Species.Name == "GYARADOS")
                    gb.UseMove("THUNDERBOLT");
                else if(gb.EnemyMon.Species.Name == "GROWLITHE")
                    gb.UseItem("X ACCURACY");
            }
            return gb.BattleMon.HP > 0 && !gb.BattleMon.Burned;
        };
        Func<Red, bool> LateDrill = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.EnemyMon.Species.Name == "PIDGEOTTO")
                    gb.UseMove("THUNDERBOLT");
                else if(gb.EnemyMon.Species.Name == "GYARADOS")
                    gb.UseMove("THUNDERBOLT");
                else if(gb.EnemyMon.Species.Name == "GROWLITHE")
                    gb.UseMove("BUBBLEBEAM");
                else if(gb.EnemyMon.Species.Name == "KADABRA")
                    gb.UseMove("THRASH");
                else if(gb.EnemyMon.Species.Name == "IVYSAUR")
                    gb.UseMove("THRASH");
            }
            return gb.BattleMon.HP > 0 && !gb.BattleMon.Burned;
        };
        Func<Red, bool> LateDrillGrowlithe = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.EnemyMon.Species.Name == "PIDGEOTTO")
                    gb.UseMove("THUNDERBOLT");
                else if(gb.EnemyMon.Species.Name == "GYARADOS")
                    gb.UseMove("THUNDERBOLT");
                else if(gb.EnemyMon.Species.Name == "GROWLITHE" && gb.BattleMon.AttackModifider == 7)
                    gb.UseItem("X ATTACK");
                else if(gb.EnemyMon.Species.Name == "GROWLITHE")
                    gb.UseMove("THRASH");
                else if(gb.EnemyMon.Species.Name == "KADABRA")
                    gb.UseMove("THRASH");
                else if(gb.EnemyMon.Species.Name == "IVYSAUR")
                    gb.UseMove("THRASH");
            }
            return gb.BattleMon.HP > 0 && !gb.BattleMon.Burned;
        };

        var s = new Simulation<Red>(10000).Track("Success");
        // for(ushort hp = 5; hp <= 20; ++hp)
        for(ushort hp = 8; hp <= 8; ++hp)
        {
            // s.Simulate("EarlyDrillPotion " + hp, "basesaves/red/earlydrill.gqs", gb => { gb.CpuWriteBE<ushort>("wPartyMon1HP", hp); return EarlyDrillPotion(gb); });
            s.Simulate("EarlyDrillGrowlithe " + hp, "basesaves/red/earlydrill.gqs", gb => { gb.CpuWriteBE<ushort>("wPartyMon1HP", hp); return EarlyDrillGrowlithe(gb); });
            // s.Simulate("LateDrill " + hp, "basesaves/red/latedrill.gqs", gb => { gb.CpuWriteBE<ushort>("wBattleMonHP", hp); return LateDrill(gb); });
            // s.Simulate("EarlyDrill " + hp, "basesaves/red/earlydrill.gqs", gb => { gb.CpuWriteBE<ushort>("wPartyMon1HP", hp); return EarlyDrill(gb); });
            s.Simulate("LateDrillGrowlithe " + hp, "basesaves/red/latedrillxatk.gqs", gb => { gb.CpuWriteBE<ushort>("wPartyMon1HP", hp); return LateDrillGrowlithe(gb); });
            Trace.WriteLine("");
        }
    }

    public static void Mankey()
    {
        Func<Red, bool> MP = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.EnemyMon.HP == gb.EnemyMon.MaxHP) gb.UseMove("MEGA PUNCH");
                else if(gb.EnemyMon.HP > 12) gb.UseMove("HORN ATTACK");
                else gb.UseMove("POISON STING");
            }
            return gb.BattleMon.HP > 0;
        };
        Func<Red, bool> HA = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.EnemyMon.HP > 12) gb.UseMove("HORN ATTACK");
                else gb.UseMove("POISON STING");
            }
            return gb.BattleMon.HP > 0;
        };

        var s = new Simulation<Red>("basesaves/red/mankey.gqs", 10000).Track("Success");
        for(ushort hp = 1; hp <= 20; ++hp)
        {
            Trace.WriteLine(hp);
            s.Simulate("MP " + hp, gb => { gb.CpuWriteBE<ushort>("wPartyMon1HP", hp); return MP(gb); });
            s.Simulate("HA " + hp, gb => { gb.CpuWriteBE<ushort>("wPartyMon1HP", hp); return HA(gb); });
        }
    }

    public static void Weedle()
    {
        Func<Red, byte, bool> TW = (gb, tw) =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.EnemyMon.DefenseModifider > 7 - tw) gb.UseMove("TAIL WHIP");
                else gb.UseMove("TACKLE");
            }
            return gb.BattleMon.HP > 0;
        };

        var s = new Simulation<Red>("basesaves/red/weedle.gqs", 10000).Track("Success", "Time");
        for(ushort atk = 10; atk <= 12; ++atk)
        {
            Trace.WriteLine(atk);
            s.Simulate("TW x1 " + atk, gb => { gb.CpuWriteBE<ushort>("wPartyMon1Attack", atk); return TW(gb, 1); });
            s.Simulate("TW x2 " + atk, gb => { gb.CpuWriteBE<ushort>("wPartyMon1Attack", atk); return TW(gb, 2); });
            s.Simulate("TW x3 " + atk, gb => { gb.CpuWriteBE<ushort>("wPartyMon1Attack", atk); return TW(gb, 3); });
        }
    }

    public static void Misty()
    {
        Func<Red, bool> Misty = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                gb.UseMove("THRASH");
            }
            return gb.BattleMon.HP > 0;
        };

        var s = new Simulation<Red>(10000).Track("Success", "Time");
        for(ushort hp = 70; hp <= 81; ++hp)
        {
            s.Simulate("24 Misty " + hp, "basesaves/red/misty24.gqs", gb => { gb.CpuWriteBE<ushort>("wPartyMon1HP", hp); return Misty(gb); });
        }
        for(ushort hp = 40; hp <= 81; ++hp)
        {
            s.Simulate("25 Misty " + hp, "basesaves/red/misty25.gqs", gb => { gb.CpuWriteBE<ushort>("wPartyMon1HP", hp); return Misty(gb); });
        }
    }

    public static void Route1Encounter()
    {
        SortedDictionary<string, int> dict = new SortedDictionary<string, int>();
        var s = new Simulation<Red>(256*256*256);
        s.Simulate("Enc", "basesaves/red/route22encounter.gqs", gb => {
            gb.AdvanceFrames(2);
            string e = "L" + gb.EnemyMon.Level + " " + gb.EnemyMon.Species.Name + " " + gb.EnemyMon.HP + " " + gb.EnemyMon.Defense;
            // string e = gb.EnemyMon.Species != null ? gb.EnemyMon.ToString() : "No encounter";
            lock(dict) {
                dict.TryAdd(e, 0);
                dict[e]++;
            }
            return true;
        });
        foreach(var kv in dict)
            Trace.WriteLine(kv.Key + " " + kv.Value);
    }

    public static void L3Pidgey()
    {
        Func<Red, bool> Tackle = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                gb.UseMove("TACKLE");
            }
            return gb.BattleMon.HP > 0;
        };
        Func<Red, bool> TailWhip = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.EnemyMon.DefenseModifider > 6) gb.UseMove("TAIL WHIP");
                else gb.UseMove("TACKLE");
            }
            return gb.BattleMon.HP > 0;
        };

        var s = new Simulation<Red>("basesaves/red/pidgey3.gqs", 10000).Track("Time");
        for(ushort atk = 10;  atk <= 12; ++atk)
        {
            for(ushort hp = 15;  hp <= 16; ++hp)
            {
                for(ushort def = 7;  def <= 8; ++def)
                {
                    Trace.WriteLine(atk + " " + hp + "/" + def + "\n");
                    s.Simulate("Tackle", gb => {
                        gb.CpuWriteBE<ushort>("wBattleMonAttack", atk);
                        gb.CpuWriteBE<ushort>("wEnemyMonHP", hp);
                        gb.CpuWriteBE<ushort>("wEnemyMonMaxHP", hp);
                        gb.CpuWriteBE<ushort>("wEnemyMonDefense", def);
                        gb.CpuWriteBE<ushort>("wEnemyMonUnmodifiedDefense", def);
                        return Tackle(gb);
                    });
                    s.Simulate("TailWhip", gb => {
                        gb.CpuWriteBE<ushort>("wBattleMonAttack", atk);
                        gb.CpuWriteBE<ushort>("wEnemyMonHP", hp);
                        gb.CpuWriteBE<ushort>("wEnemyMonMaxHP", hp);
                        gb.CpuWriteBE<ushort>("wEnemyMonDefense", def);
                        gb.CpuWriteBE<ushort>("wEnemyMonUnmodifiedDefense", def);
                        return TailWhip(gb);
                    });
                }
            }
        }
    }

    public static void BillToSurge()
    {
        Func<Red, bool> Thrash = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                gb.UseMove("THRASH");
            }
            return gb.BattleMon.HP > 0;
        };
        Func<Red, bool> BoatRival = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.EnemyMon.Species.Name == "PIDGEOTTO")
                    gb.UseMove("HORN ATTACK");
                else if(gb.EnemyMon.Species.Name == "RATICATE" && gb.BattleMon.PP[1] >= 6)
                    gb.UseMove("MEGA PUNCH");
                else if(gb.BattleMon.AccuracyModifider == 7)
                    gb.UseMove("THRASH");
                else
                    gb.UseMove("HORN ATTACK");
            }
            return gb.BattleMon.HP > 0;
        };

        const int it = 65536;
        var s = new Simulation<Red>(it).Track("Success", "HP");
        // var data = s.Simulate("Dig Rocket", "basesaves/red/digrocket.gqs", Thrash)[1];
        // var data = s.Simulate("Goldeen", "basesaves/red/goldeen.gqs", Thrash)[1];
        var data = s.Simulate("Misty", "basesaves/red/misty25b.gqs", Thrash)[1];
        // var data = s.Simulate("Route6 Jr F", "basesaves/red/route6jrf.gqs", Thrash)[1];
        // var data = s.Simulate("Route6 Jr M", "basesaves/red/route6jrm.gqs", Thrash)[1];
        // var data = s.Simulate("Boat Rival", "basesaves/red/boatrivalfull.gqs", BoatRival)[1];

        int[] hplost = new int[100];
        foreach(int hp in data)
            hplost[-hp]++;
        for(int i = 0; i <= -data[0]; ++i)
            Trace.WriteLine($"{i}: {100.0 * hplost[i] / it:F3}%");
    }

    public static void DamageRolls(int w = 0)
    {
        int[] rolls = new int[256];
        for(w = 0; w < 64; ++w)
        {
            Red gb = new Red();
            gb.LoadState("basesaves/red/towergolbat.gqs");
            // gb.Hold(Joypad.A, gb.SYM["RandomizeDamage"]);
            gb.AdvanceFrames(w);
            gb.Hold(Joypad.A, gb.SYM["PlayerCalcMoveDamage"]);
            //towergolbat bridgerocketzubat oddishgirlthrash
            byte[] state = gb.SaveState();

            var s = new Simulation<Red>(state, 65536);
            s.Simulate("DamageRolls", gb =>
            {
                gb.Hold(Joypad.A, gb.SYM["RandomizeDamage.loop"] + 8);
                System.Threading.Interlocked.Increment(ref rolls[gb.A]);
                return true;
            });
        }

        Trace.WriteLine(w);
        for(int i = 217; i < 256; ++i)
            // Trace.WriteLine(i + " " + rolls[i]);
            Trace.WriteLine(rolls[i]);
    }

    public static void ClefSurge()
    {
        List<(int n1, int n2)> hplost = new List<(int, int)>();
        Func<Blue, bool> Surge = gb =>
        {
            int max = gb.BattleMon.MaxHP;
            int lostbeforelevel = -1;
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                gb.UseMove("BODY SLAM");
                if(gb.BattleMon.MaxHP != max)
                {
                    lostbeforelevel = gb.BattleMon.MaxHP - gb.BattleMon.HP;
                    max = gb.BattleMon.MaxHP;
                }
            }
            int losttotal = gb.BattleMon.MaxHP - gb.BattleMon.HP;
            if(lostbeforelevel < 0) lostbeforelevel = losttotal;
            lock(hplost) { hplost.Add((lostbeforelevel, losttotal)); }
            return gb.BattleMon.HP > 0;
        };

        // const int it = 20;
        var s = new Simulation<Blue>(1000).Track("Success", "HP");
        s.Simulate("Surge", "basesaves/blue/surge.gqs", Surge);
        // foreach(var x in hplost)
        // {
        //     int surv = Math.Max(x.n1 + 1, x.n2 - 4 + 1);
        //     Trace.WriteLine(x.n1 + " " + x.n2 + "   " + surv);
        // }
        // Trace.WriteLine(hplost.Count);
        // const double r = 100.0 / it;
        double Win(int hp, List<(int n1, int n2)> hplost)
        {
            return hplost.Count(x => x.n1 < hp && x.n2 < hp + 4) * 100.0 / hplost.Count;
        }
        double InRange(int hp, int min, int max, List<(int n1, int n2)> hplost)
        {
            return hplost.Count(x => x.n1 < hp && x.n2 <= hp + 4 - min && x.n2 >= hp + 4 - max) * 100.0 / hplost.Count;
        }
        Trace.WriteLine(" HP:     p(>0)   p(1-22)   p(1-27)");
        for(int hp = 1; hp <= 91; ++hp)
        {
            // double pwin = hplost.Count(x => x.n1 < hp && x.n2 < hp + 4) * r;
            // double p1_22 = hplost.Count(x => x.n1 < hp && x.n2 < hp + 4 && x.n2 >= hp + 4 - 22) * r;
            // Trace.WriteLine(hp + ": " + pwin + " " + p1_22);
            Trace.WriteLine($"{hp,3}:{Win(hp, hplost),9:F3}%{InRange(hp, 1, 22, hplost),9:F3}%{InRange(hp, 1, 27, hplost),9:F3}%");
        }

        // Func<Blue, bool> Surge = gb =>
        // {
        //     while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
        //     {
        //         gb.UseMove("BODY SLAM");
        //     }
        //     return gb.BattleMon.HP > 0;
        // };

        // const int it = 10;
        // var s = new Simulation<Blue>(it).Track("Success", "HP");
        // var data = s.Simulate("Surge", "basesaves/blue/surge.gqs", Surge)[1];

        // int[] hplost = new int[256];
        // int[] winrate = new int[256];
        // foreach(int hp in data)
        //     hplost[-hp]++;
        // const double r = 100.0 / it;
        // Trace.WriteLine("Damage   Chance   HP(L25):     p(>0)   p(1-22)   p(1-27)");
        // for(int i = 0; i <= -data[0]; ++i)
        // {
        //     int p1_22 = winrate[i] - (i > 22 ? winrate[i - 22] : 0);
        //     int p1_27 = winrate[i] - (i > 27 ? winrate[i - 27] : 0);
        //     Trace.WriteLine($"{i,6}{hplost[i] * r,8:F3}%{i - 4,10}:{winrate[i] * r,9:F3}%{p1_22 * r,9:F3}%{p1_27 * r,9:F3}%");
        //     winrate[i + 1] = winrate[i] + hplost[i];
        // }
    }

    public static void ClefSabrina()
    {
        Func<Blue, bool> Sabrina = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.SpeedModifider < 8)
                    gb.UseItem("X SPEED");
                else if(gb.EnemyMon.Species.Name == "KADABRA")
                    gb.UseMove("BODY SLAM");
                else if(gb.BattleMon.AttackModifider < 8)
                    gb.UseItem("X ATTACK");
                else
                    gb.UseMove("BODY SLAM");
            }
            return gb.BattleMon.HP > 0;
        };

        const int it = 10000;
        var s = new Simulation<Blue>(it).Track("Success", "HP");
        var data = s.Simulate("Sabrina", "basesaves/blue/sabrina1.gqs", Sabrina)[1];

        int[] hplost = new int[256];
        int[] winrate = new int[256];
        foreach(int hp in data)
            hplost[-hp]++;
        const double r = 100.0 / it;
        Trace.WriteLine("Damage   Chance        HP:     p(>0)   p(1-29)");
        for(int i = 0; i <= -data[0]; ++i)
        {
            int p1_29 = winrate[i] - (i > 29 ? winrate[i - 29] : 0);
            Trace.WriteLine($"{i,6}{hplost[i] * r,8:F3}%{i,10}:{winrate[i] * r,9:F3}%{p1_29 * r,9:F3}%");
            winrate[i + 1] = winrate[i] + hplost[i];
        }
    }

    public static void ClefBlaine()
    {
        Func<Blue, bool> Blaine = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.SpeedModifider < 8)
                    gb.UseItem("X SPEED");
                else if(gb.BattleMon.AttackModifider < 9 && gb.EnemyMon.Species.Name == "GROWLITHE")
                    gb.UseItem("X ATTACK");
                else if(gb.BattleMon.Burned && gb.Bag.Contains("FULL RESTORE"))
                    gb.UseItem("FULL RESTORE", 0);
                else
                    gb.UseMove("BODY SLAM");
            }
            return gb.BattleMon.HP > 0;
        };

        const int it = 10000;
        var s = new Simulation<Blue>(it).Track("Success", "HP");
        var data = s.Simulate("Blaine", "basesaves/blue/blaine2.gqs", Blaine)[1];

        int[] hplost = new int[256];
        int[] winrate = new int[256];
        foreach(int hp in data)
            hplost[-hp]++;
        const double r = 100.0 / it;
        Trace.WriteLine("Damage   Chance        HP:     p(>0)   p(1-29)");
        for(int i = 0; i <= -data[0]; ++i)
        {
            int p1_29 = winrate[i] - (i > 29 ? winrate[i - 29] : 0);
            Trace.WriteLine($"{i,6}{hplost[i] * r,8:F3}%{i,10}:{winrate[i] * r,9:F3}%{p1_29 * r,9:F3}%");
            winrate[i + 1] = winrate[i] + hplost[i];
        }
    }

    public static void ClefViridianRival()
    {
        Func<Blue, bool> ViridianRival = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.Species.Name == "PIDGEOT")
            {
                if(gb.BattleMon.SpecialModifider < 9)
                    gb.UseItem("X SPECIAL");
                else if(gb.BattleMon.AttackModifider < 8)
                    gb.UseItem("X ATTACK");
                else
                    gb.UseMove("ICE BEAM");
            }
            return gb.BattleMon.HP > 0;
        };

        const int it = 10000;
        var s = new Simulation<Blue>(it).Track("Success", "HP");
        var data = s.Simulate("ViridianRival", "basesaves/blue/viridianrival.gqs", ViridianRival)[1];

        int[] hplost = new int[256];
        int[] winrate = new int[256];
        foreach(int hp in data)
            hplost[-hp]++;
        const double r = 100.0 / it;
        Trace.WriteLine("Damage   Chance        HP:     p(>0)   p(1-37)");
        for(int i = 0; i <= -data[0]; ++i)
        {
            int p1_37 = winrate[i] - (i > 37 ? winrate[i - 37] : 0);
            Trace.WriteLine($"{i,6}{hplost[i] * r,8:F3}%{i,10}:{winrate[i] * r,9:F3}%{p1_37 * r,9:F3}%");
            winrate[i + 1] = winrate[i] + hplost[i];
        }
    }

    public static void ClefChamp()
    {
        Func<Blue, bool> Champ = gb =>
        {
            while(gb.BattleMon.HP > 0 && gb.EnemyMon.HP > 0)
            {
                if(gb.BattleMon.SpecialModifider < 10 && gb.EnemyMon.Species.Name == "PIDGEOT")
                    gb.UseItem("X SPECIAL");
                else if(gb.BattleMon.SpeedModifider < 8)
                    gb.UseItem("X SPEED");
                else if(gb.EnemyMon.Species.Name == "ALAKAZAM")
                    gb.UseMove("BODY SLAM");
                else if(gb.EnemyMon.Species.Name == "GYARADOS")
                    gb.UseMove("THUNDERBOLT");
                else
                    gb.UseMove("ICE BEAM");
            }
            return gb.BattleMon.HP > 0;
        };

        const int it = 10000;
        var s = new Simulation<Blue>(it).Track("Success", "HP");
        var data = s.Simulate("Champ", "basesaves/blue/champ.gqs", Champ)[1];

        int[] hplost = new int[256];
        int[] winrate = new int[256];
        foreach(int hp in data)
            hplost[-hp]++;
        const double r = 100.0 / it;
        Trace.WriteLine("Damage   Chance        HP:     p(>0)   p(1-42)");
        for(int i = 0; i <= -data[0]; ++i)
        {
            int p1_42 = winrate[i] - (i > 42 ? winrate[i - 42] : 0);
            Trace.WriteLine($"{i,6}{hplost[i] * r,8:F3}%{i,10}:{winrate[i] * r,9:F3}%{p1_42 * r,9:F3}%");
            winrate[i + 1] = winrate[i] + hplost[i];
        }
    }

    public static void Start()
    {
        ClefSurge();
        // ClefSabrina();
        // ClefBlaine();
        // ClefViridianRival();
        // ClefChamp();
    }
}
