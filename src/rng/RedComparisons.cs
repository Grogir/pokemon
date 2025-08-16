using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using static Comparison;

public class RedComparisons : RedBlueForceComparisons
{
    void BlackBeltSave()
    {
        Comparison.Compare("basesaves/red/blackbeltsave.gqs", () =>
        {
            MoveTo(10, 5);
            Save();
            MoveTo(10, 4);
        }, () =>
        {
            MoveTo(10, 4);
        });
    }
    void GioElixer()
    {
        Comparison.Compare("basesaves/red/gioelixer.gqs", () =>
        {
            ClearText();
            MoveTo(1, 32, 8);
            MoveTo(45, 16, 16);
            UseItem("ELIXER", "NIDOKING");
            MoveTo(45, 2, 2);
        }, () =>
        {
            ClearText();
            MoveTo(1, 32, 8);
            UseItem("ELIXER", "NIDOKING");
            MoveTo(45, 2, 2);
        }, () =>
        {
            ClearText();
            MoveTo(45, 16, 16);
            UseItem("ELIXER", "NIDOKING");
            MoveTo(1, 32, 8);
            MoveTo(45, 2, 2);
        });
    }
    void SilphMaxEther()
    {
        Comparison.Compare("basesaves/red/silphmaxether.gqs", () =>
        {
            ClearText();
            MoveTo(3, 7);
            MoveTo(5, 7);
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            MoveTo(235, 3, 5);
            MoveTo(235, 2, 16);
            Press(Joypad.Right);
        }, () =>
        {
            ClearText();
            MoveTo(5, 6);
            MoveTo(5, 7);
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            MoveTo(235, 3, 5);
            MoveTo(235, 2, 16);
            Press(Joypad.Right);
        });
        Comparison.Compare("basesaves/red/silphmaxether.gqs", () =>
        {
            ClearText();
            MoveTo(3, 7);
            MoveTo(5, 7);
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            MoveTo(235, 3, 5);
            MoveTo(235, 2, 16);
            Press(Joypad.Right);
        }, () =>
        {
            ClearText();
            MoveTo(3, 7);
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            MoveTo(5, 7);
            MoveTo(235, 3, 5);
            MoveTo(235, 2, 16);
            Press(Joypad.Right);
        });
    }
    void SilphMaxEtherCheckAll()
    {
        void UseAtCoords(int x, int y)
        {
            LoadState("basesaves/red/silphmaxether.gqs");
            Show();
            TimerComponent timer = new TimerComponent(0, 144, 2.0f);
            Scene.AddComponent(timer);

            ClearText();
            MoveTo(x, y);
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            MoveTo(235, 2, 16);
            Press(Joypad.Right);

            timer.Running = false;
            Console.WriteLine(x + " " + y + "  " + timer.Text);
            AdvanceFrames(50);
            Scene.Dispose();
        }
        int x = 3;
        for(int y = 3; y <= 7; ++y)
            UseAtCoords(x, y);
    }
    void SilphMovement()
    {
        for(int wait = 0; wait < 50; ++wait)
        {
            Console.WriteLine("\nwaiting " + wait);
            Comparison.Compare("basesaves/red/silphelixer.gqs", () =>
            {
                AdvanceFrames(wait);
                ClearText();
                MoveTo(3, 7);
                MoveTo(5, 7);
                MoveTo(3, 3);
            }, () =>
            {
                AdvanceFrames(wait);
                ClearText();
                MoveTo(5, 6);
                MoveTo(5, 7);
                MoveTo(3, 3);
            });
        }
        Comparison.Compare("silphmovement", "basesaves/blue/silph.gqs", () =>
        {
            ClearText();
            MoveTo(208, 3, 3);
            // PickupItemAt(210, 12, 3);
            MoveTo(210, 8, 15);
            Press(Joypad.A);
            ClearText();
            ForceTurn(new RbyTurn("PSYCHIC"));
            PickupItemAt(21, 16);
        }, () =>
        {
            ClearText();
            MoveTo(210, 14, 3);
            // PickupItemAt(210, 12, 3);
            MoveTo(210, 8, 15);
            Press(Joypad.A);
            ClearText();
            ForceTurn(new RbyTurn("PSYCHIC"));
            PickupItemAt(21, 16);
        }, () =>
        {
            ClearText();
            TalkTo(236, 3, 0);
            ChooseListItem(8);
            Execute("L D D D");
            MoveTo(210, 9, 16);
            Press(Joypad.A);
            ClearText();
            ForceTurn(new RbyTurn("PSYCHIC"));
            PickupItemAt(21, 16);
        });
    }
    void LanceStall()
    {
        Scenario LanceXAccStall = () =>
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", Miss));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("HYDRO PUMP", 20));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("BLIZZARD"));
        };
        Scenario LanceXSpeedStall = () =>
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", Miss));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("HYDRO PUMP", 39));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
        };
        Scenario LanceHydroHit = () =>
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", 20));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
        };
        Scenario LanceNoStall = () =>
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", Miss));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
        };
        Scenario LanceNoStallBlizzMiss = () =>
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", Miss));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
        };
        Scenario ChampTB = () =>
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        };
        Scenario ChampBlizz = () =>
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        };
        Scenario ChampPPUP = () =>
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        };
        Comparison.Compare("basesaves/red/lancestall.gqs", () =>
        {
            MoveTo(5, 1);
            ClearText();
            LanceHydroHit();
            Execute("U U");
            ClearText();
            ChampTB();
        }, () =>
        {
            MoveTo(5, 1);
            ClearText();
            LanceXAccStall();
            Execute("U U");
            ClearText();
            ChampTB();
        }, () =>
        {
            MoveTo(5, 1);
            ClearText();
            LanceXSpeedStall();
            Execute("U U");
            ClearText();
            ChampTB();
        }, () =>
        {
            MoveTo(5, 1);
            ClearText();
            LanceNoStall();
            Execute("U U");
            ClearText();
            ChampBlizz();
        });
        Comparison.Compare("lancestall_ppup", "basesaves/red/lancestall.gqs", () =>
        {
            MoveTo(5, 1);
            ClearText();
            LanceHydroHit();
            Execute("U U");
            ClearText();
            ChampPPUP();
        }, () =>
        {
            MoveTo(5, 1);
            ClearText();
            LanceXAccStall();
            Execute("U U");
            ClearText();
            ChampPPUP();
        }, () =>
        {
            MoveTo(5, 1);
            ClearText();
            LanceXSpeedStall();
            Execute("U U");
            ClearText();
            ChampPPUP();
        }, () =>
        {
            MoveTo(5, 1);
            ClearText();
            LanceNoStall();
            Execute("U U");
            ClearText();
            ChampPPUP();
        });
    }
    void BadgeCheck()
    {
        Comparison.Compare("basesaves/red/badgecheck.gqs", () =>
        {
            Surf();
            MoveTo(10, 96, Action.Up);
            ClearText();
            // MoveTo(8, 86, Action.Up);
            MoveTo(7, 85, Action.Up);
            ClearText();
            MoveTo(8, 71, Action.Up);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");
            MoveTo(12, 56, Action.Up);
            ClearText();
            MoveTo(5, 35, Action.Up);
            ClearText();
            MoveTo("VictoryRoad1F", 8, 17);
        }, () =>
        {
            Surf();
            MoveTo(10, 96, Action.Up);
            ClearText();

            TalkTo(8, 85, Action.Up); // talk to the girl
            // MoveTo(8, 86, Action.Up);
            // MoveTo(7, 85, Action.Up);
            // ClearText();

            MoveTo(8, 71, Action.Up);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            // TalkTo(10, 56, Action.Up); // talk to the guy
            MoveTo(12, 56, Action.Up);
            ClearText();

            MoveTo(5, 35, Action.Up);
            ClearText();
            MoveTo("VictoryRoad1F", 8, 17);
        });
    }
    void Options()
    {
        Comparison.Compare("optionstile", "basesaves/red/options.gqs", () =>
        {
            ClearText();
            MoveTo(4, 11);
            SetOptions(Fast | Off | Set);
            MoveTo(4, 12);
            Inject(Joypad.Down);
            Hold(Joypad.Down, "EnterMap");
        }, () =>
        {
            ClearText();
            MoveTo(4, 9);
            SetOptions(Fast | Off | Set);
            MoveTo(4, 12);
            Inject(Joypad.Down);
            Hold(Joypad.Down, "EnterMap");
        });
        Comparison.Compare("basesaves/red/options.gqs", () =>
        {
            ClearText();
            MoveTo(4, 10);
            SetOptions(Fast | Off | Set);
            MoveTo("Route3", 0, 8);
            MoveTo("Route3", 11, 6);
        }, () =>
        {
            ClearText();
            MoveTo(4, 10);
            // MoveTo(2, 39, 16);
            MoveTo("Route3", 0, 8);
            SetOptions(Fast | Off | Set);
            MoveTo("Route3", 11, 6);
        });
        Comparison.Compare("basesaves/red/optionslatemart.gqs", () =>
        {
            ClearText();
            MoveTo("PewterMart", 3, 5);
            SetOptions(Fast | Off | Set);
            TalkTo("PewterMart", 1, 5);
            Buy("POTION", 8);
            MoveTo(2, 24, 18);
        }, () =>
        {
            ClearText();
            TalkTo("PewterMart", 1, 5);
            Buy("POTION", 8);
            MoveTo("PewterMart", 3, 5);
            SetOptions(Fast | Off | Set);
            MoveTo(2, 24, 18);
        }, () =>
        {
            ClearText();
            MoveTo(4, 10);
            SetOptions(Fast | Off | Set);
            TalkTo("PewterMart", 1, 5);
            Buy("POTION", 8);
            MoveTo(2, 24, 18);
        });
    }
    void BirdSwap()
    {
        {
            LoadState("basesaves/red/birdswap.gqs");
            MenuPress(Joypad.B);
            MoveTo("IndigoPlateauLobby", 8, 1);
            Scene s;
            const int sca = 48;
            s = new Scene(this, 10 * sca, 9 * sca);
            s.AddComponent(new VideoBufferComponent(0, 0, 10 * sca, 9 * sca));
            s.AddComponent(new RecordingComponent("right"));
            Execute("U");
            PartySwap("NIDOKING", "PIDGEY");
            Execute("U U");
            AdvanceFrames(300);
            s.Dispose();
        }
        Comparison.Compare("basesaves/red/birdswap.gqs", () =>
        {
            MenuPress(Joypad.B);
            MoveTo("IndigoPlateauLobby", 8, 0);
            MoveTo("LoreleisRoom", 4, 4);
            PartySwap("NIDOKING", "PIDGEY");
            MoveTo("LoreleisRoom", 4, 2);
            Press(Joypad.Right);
        }, () =>
        {
            MenuPress(Joypad.B);
            MoveTo("IndigoPlateauLobby", 8, 1);
            PartySwap("NIDOKING", "PIDGEY");
            MoveTo("IndigoPlateauLobby", 8, 0);
            MoveTo("LoreleisRoom", 4, 2);
            Press(Joypad.Right);
        });
        Comparison.Compare("basesaves/red/birdswap3.gqs", () =>
        {
            MoveTo("IndigoPlateauLobby", 7, 10);
            PartySwap("NIDOKING", "PARAS");
            TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
            Deposit("PARAS", "SQUIRTLE");
            MoveTo("IndigoPlateauLobby", 8, 0);
            MoveTo("LoreleisRoom", 4, 2);
            Press(Joypad.Right, Joypad.None, Joypad.A);
            AdvanceFrames(3);
        }, () =>
        {
            TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
            Deposit("SQUIRTLE", "PARAS");
            MoveTo("IndigoPlateauLobby", 8, 0);
            PartySwap("NIDOKING", "SPEAROW");
            MoveTo("LoreleisRoom", 4, 2);
            Press(Joypad.Right, Joypad.A);
            AdvanceFrames(3);
        });
        Comparison.Compare("basesaves/red/birdswap4.gqs", () => // turnframe
        {
            AdvanceFrames(214, Joypad.Up);
            AdvanceFrames(2, Joypad.Right);
            AdvanceFrames(2);
            AdvanceFrames(2, Joypad.A);
            ClearText();
        }, () =>
        {
            MoveTo("IndigoPlateauLobby", 8, 0);
            MoveTo("LoreleisRoom", 4, 2);
            Press(Joypad.Right, Joypad.None, Joypad.A);
            ClearText();
        }, () =>
        {
            MoveTo("IndigoPlateauLobby", 8, 0);
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
        });
        Comparison.Compare("basesaves/red/birdswap5.gqs", () => // no turnframe
        {
            CurrentMenuType = MenuType.StartMenu;
            MoveTo("LoreleisRoom", 4, 2);
            Press(Joypad.Right, Joypad.A);
            ClearText();
        }, () =>
        {
            CurrentMenuType = MenuType.StartMenu;
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
        });
    }
    void DrillBruno()
    {
        Comparison.Compare("basesaves/red/drillbruno.gqs", () =>
        {
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("RAGE", 20));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("RAGE", 20));
            ForceTurn(new RbyTurn("BLIZZARD", Crit));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("RAGE", 20));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD", Crit));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("RAGE", 20));
            ForceTurn(new RbyTurn("BLIZZARD", Crit));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD", Crit));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("RAGE", 20));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
    }
    void RhydonRange()
    {
        Comparison.Compare("basesaves/red/rhydonrange.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", 20), new RbyTurn("FISSURE"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("FISSURE"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", 20 | SideEffect), new RbyTurn("FISSURE"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        }, () =>
        {
            ClearText();
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        });
    }
    void PidgeotRange()
    {
        Comparison.Compare("basesaves/red/pidgeotrange.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD", 10), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD", 10), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn(AiItem));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD", 10 + SideEffect), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
        });
    }
    void BlaineSuper()
    {
        Comparison.Compare("basesaves/red/blainesuper.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
    }
    void FlyPallet()
    {
        Comparison.Compare("basesaves/red/flypallet.gqs", () =>
        {
            Fly("ViridianCity");
        }, () =>
        {
            Fly("PalletTown");
            Fly("ViridianCity");
        });
    }
    void BikePallet()
    {
        Comparison.Compare("basesaves/red/bikepallet.gqs", () =>
        {
            MoveNpc("PalletTown", 3, 8, Action.Down);
            CpuWrite(SYM["wSpritePlayerStateData1MovementStatus"] + 32, 0);
            RunUntil("TryWalking");
            HandleNpcMovement();

            UseItem("BICYCLE");

            MoveTo(4, 17, Action.Right);
            UseItem("SUPER REPEL");
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("HM03", "SQUIRTLE");
            Surf();
        }, () =>
        {
            MoveNpc("PalletTown", 3, 8, Action.Down);
            CpuWrite(SYM["wSpritePlayerStateData1MovementStatus"] + 32, 0);
            RunUntil("TryWalking");
            HandleNpcMovement();

            MoveTo(4, 17, Action.Right);
            UseItem("SUPER REPEL");
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("HM03", "SQUIRTLE");
            Surf();
        });
        Comparison.Compare("basesaves/red/bikepallet.gqs", () =>
        {
            MoveNpc("PalletTown", 3, 8, Action.Up);
            CpuWrite(SYM["wSpritePlayerStateData1MovementStatus"] + 32, 0);
            RunUntil("TryWalking");
            HandleNpcMovement();

            UseItem("BICYCLE");
            MoveTo(4, 8, Action.Left);
            Inject(Joypad.Left);
            AdvanceFrames(5, Joypad.Left);
            Execute("D");

            MoveTo(4, 17, Action.Right);
            UseItem("SUPER REPEL");
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("HM03", "SQUIRTLE");
            Surf();
        }, () =>
        {
            MoveNpc("PalletTown", 3, 8, Action.Up);
            CpuWrite(SYM["wSpritePlayerStateData1MovementStatus"] + 32, 0);
            RunUntil("TryWalking");
            HandleNpcMovement();

            MoveTo(4, 8);

            MoveTo(4, 17, Action.Right);
            UseItem("SUPER REPEL");
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("HM03", "SQUIRTLE");
            Surf();
        });
    }
    void Gentleman()
    {
        Comparison.Compare("basesaves/red/gentlemanit.gqs", () =>
        // Comparison.Compare("basesaves/red/gentlemannoit.gqs", () =>
        {
            TalkTo(103, 0, 14);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            PickupItemAt(0, 12);
            MoveTo(96, 2, 4);
        }, () =>
        {
            MoveTo(96, 2, 4);
        });
    }
    void Deposit()
    {
        Comparison.Compare("basesaves/red/deposit.gqs", () =>
        {
            TalkTo("IndigoPlateauLobby", 15, 7, Action.Up);
            Deposit("SQUIRTLE", "PARAS");
            MoveTo("IndigoPlateauLobby", 8, 1, Action.Up);
        }, () =>
        {
            TalkTo("IndigoPlateauLobby", 15, 7, Action.Up);
            Deposit("SQUIRTLE", "PIDGEY", "PARAS");
            MoveTo("IndigoPlateauLobby", 8, 1, Action.Up);
        }, () =>
        {
            MoveTo("IndigoPlateauLobby", 8, 1, Action.Up);
        });
    }
    void Hof()
    {
        byte[][] states = new byte[3][];
        LoadState("basesaves/red/hof1.gqs");
        RunUntil("EnterMap");
        states[0] = SaveState();
        LoadState("basesaves/red/hof2.gqs");
        RunUntil("EnterMap");
        states[1] = SaveState();
        LoadState("basesaves/red/hof4.gqs");
        RunUntil("EnterMap");
        states[2] = SaveState();

        Scenario hof = () =>
        {
            ClearText();
            ClearText(26);
            AdvanceFrames(164);
        };

        Comparison.Compare("hof", states, new Scenario[] { hof, hof, hof });
    }
    void SilphBar()
    {
        Scenario StartToArbok = () =>
        {
            ClearText();
            MoveTo("LavenderTown", 7, 10);
            Fly("CeladonCity");
            TalkTo("CeladonPokecenter", 3, 2);
            Yes();
            ClearText(); // healed at center

            MoveTo("CeladonCity", 41, 10);
            UseItem("BICYCLE");

            MoveTo(44, 10);
            Press(Joypad.Right);
            ClearText(); // repel

            MoveTo("Route7Gate", 3, 4);
            ClearText();
            MoveTo("Route7", 18, 10);
            UseItem("BICYCLE");

            PickupItemAt("SilphCo5F", 12, 3);
        };
        Scenario GioToJuggler = () =>
        {
            // SILPH GIOVANNI
            TalkTo(6, 13, Action.Up);
            MoveTo(6, 13);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            Execute("D"); // after gio (?)

            TalkTo(236, 3, 0);
            ChooseListItem(9);
            MoveTo(2, 3);
            Execute("D"); // exit elevator

            MoveTo(3, 9);
            MoveTo(2, 9); // thinks trainer is still there (?)
            PickupItemAt(234, 2, 12);
            PickupItemAt(234, 4, 14);
            Dig();

            UseItem("BICYCLE");

            // Snorlax menu
            MoveTo("Route16", 27, 10);
            UseItem("REPEL");
            ItemSwap("HELIX FOSSIL", "X SPECIAL");
            UseItem("POKE FLUTE");
            RunAway();

            MoveTo("Route17", 15, 5);
            PickupItemAt("Route17", 15, 13); // candy

            // MoveTo(18,72);
            MoveTo("Route17", 17, 59);
            PickupItemAt("Route17", 17, 71); // pp up
            Press(Joypad.Down);
            ClearText(); // repel

            // Post cycling
            MoveTo("Route18", 40, 8);
            UseItem("REPEL");
            ItemSwap("PARLYZ HEAL", "TM26");
            UseItem("PP UP", "NIDOKING", "HORN DRILL");
            UseItem("TM26", "NIDOKING", "THRASH");
            UseItem("BICYCLE");

            CutAt("FuchsiaCity", 18, 19);
            CutAt(16, 11);
            MoveTo("SafariZoneGate", 3, 2);
            ClearText();
            Yes();
            ClearText();
            ClearText(); // sneaky joypad call

            UseItem("BICYCLE");

            MoveTo(217, 4, 23);
            UseItem("SUPER REPEL");

            MoveTo(218, 6, 9);
            Press(Joypad.Down);
            ClearText(); // repel

            PickupItemAt("SafariZoneWest", 19, 7, Action.Down); // gold teeth

            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();

            Fly("FuchsiaCity");
            UseItem("BICYCLE");

            // JUGGLER 1
            TalkTo("FuchsiaGym", 7, 8);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
        };
        Scenario Koga = () =>
        {
            // KOGA
            TalkTo(4, 10);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ELIXER", "NIDOKING"), new RbyTurn("SELFDESTRUCT"), true, false);
            ClearText(7);
            AdvanceFrames(2);
        };
        Scenario Juggler2Lives = () =>
        {
            // JUGGLER 2
            MoveTo(1, 7);
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("POISON GAS"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
        };
        Scenario Juggler2Dies = () =>
        {
            // JUGGLER 2
            MoveTo(1, 7);
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("CONFUSION"));
            SendOut("SQUIRTLE");
            ForceTurn(new RbyTurn("REVIVE", "NIDOKING"), new RbyTurn("CONFUSION"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("THUNDERBOLT"));
        };

        void FullSilphBar(bool livehypno)
        {
            StartToArbok();

            // ARBOK TRAINER
            TalkTo(8, 16);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));

            PickupItemAt(21, 16);
            TalkTo(7, 13);
            TalkTo("SilphCo3F", 17, 9);

            // SILPH RIVAL
            MoveTo("SilphCo7F", 3, 2, Action.Left);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ItemSwap("POTION", "RARE CANDY");
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo("SilphCo7F", 5, 7, Action.Right);
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");

            // SILPH ROCKET
            TalkTo("SilphCo11F", 3, 16);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            GioToJuggler();

            if(livehypno)
                Juggler2Lives();
            else
                Juggler2Dies();

            Koga();
        };
        void NoSilphBar()
        {
            StartToArbok();

            // ARBOK TRAINER
            TalkTo(8, 16);
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("THRASH"));

            PickupItemAt(21, 16);
            TalkTo(7, 13);
            TalkTo("SilphCo3F", 17, 9);

            // SILPH RIVAL
            MoveTo("SilphCo7F", 3, 2, Action.Left);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ItemSwap("POTION", "RARE CANDY");
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo("SilphCo7F", 5, 7, Action.Right);

            // SILPH ROCKET
            TalkTo("SilphCo11F", 3, 16);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("MAX ETHER", "NIDOKING", "HORN DRILL"), new RbyTurn("CONFUSION"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            GioToJuggler();

            Juggler2Lives();

            Koga();
        };
        void DrowzeeBar(bool livehypno)
        {
            StartToArbok();

            // ARBOK TRAINER
            TalkTo(8, 16);
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("THRASH"));

            PickupItemAt(21, 16);
            TalkTo(7, 13);
            TalkTo("SilphCo3F", 17, 9);

            // SILPH RIVAL
            MoveTo("SilphCo7F", 3, 2, Action.Left);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WING ATTACK"));
            ItemSwap("POTION", "RARE CANDY");
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WING ATTACK"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo("SilphCo7F", 5, 7, Action.Right);

            // SILPH ROCKET
            TalkTo("SilphCo11F", 3, 16);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("MAX ETHER", "NIDOKING", "HORN DRILL"), new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            GioToJuggler();

            if(livehypno)
                Juggler2Lives();
            else
                Juggler2Dies();

            Koga();
        };

        Comparison.Compare("basesaves/red/silphbar.gqs", () =>
        {
            FullSilphBar(true);
        }, () =>
        {
            FullSilphBar(false);
        }, () =>
        {
            DrowzeeBar(true);
        }, () =>
        {
            DrowzeeBar(false);
        }, () =>
        {
            NoSilphBar();
        });
    }
    void Bill()
    {
        Comparison.Compare("basesaves/red/bill.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 45);
            ClearText();
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 25);
            ClearText();
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            ClearText();
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            ClearText();
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");
        }, () =>
        {
            ClearText();
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");
            TalkTo("CeruleanPokecenter", 3, 2);
            Yes();
            ClearText();
            MoveTo(3, 19, 18);
        });
    }
    void BillEther()
    {
        Comparison.Compare("basesaves/red/billether.gqs", () =>
        {
            ClearText();
            CpuWriteBE<ushort>("wPartyMon1HP", 45);
            PickupItemAt(38, 3);
            TalkTo("BillsHouse", 6, 5, Action.Right);
            Yes();
            ClearText();
            TalkTo(1, 4);
            TalkTo(4, 4);
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ETHER", "NIDOKING", "MEGA PUNCH");
            UseItem("ESCAPE ROPE");
            MoveTo(3, 18, 18);
        }, () =>
        {
            ClearText();
            CpuWriteBE<ushort>("wPartyMon1HP", 25);
            PickupItemAt(38, 3);
            TalkTo("BillsHouse", 6, 5, Action.Right);
            Yes();
            ClearText();
            TalkTo(1, 4);
            TalkTo(4, 4);
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ETHER", "NIDOKING", "MEGA PUNCH");
            UseItem("ESCAPE ROPE");
            MoveTo(3, 18, 18);
        }, () =>
        {
            ClearText();
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            PickupItemAt(38, 3);
            TalkTo("BillsHouse", 6, 5, Action.Right);
            Yes();
            ClearText();
            TalkTo(1, 4);
            TalkTo(4, 4);
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ETHER", "NIDOKING", "MEGA PUNCH");
            UseItem("ESCAPE ROPE");
            MoveTo(3, 18, 18);
        }, () =>
        {
            ClearText();
            TalkTo("BillsHouse", 6, 5, Action.Right);
            Yes();
            ClearText();
            TalkTo(1, 4);
            TalkTo(4, 4);
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");
            TalkTo("CeruleanPokecenter", 3, 2);
            Yes();
            ClearText();
            MoveTo(3, 18, 18);
        });
    }
    void AgathaMenu()
    {
        // todo fix this
        Comparison.Compare("basesaves/red/agathamenu.gqs", () =>
        {
            ClearText();
            Execute("U U U");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            MoveTo("AgathasRoom", 4, 2);
            Press(Joypad.Right);
        }, () =>
        {
            ClearText();
            Execute("U U U");
            MoveTo("AgathasRoom", 4, 3);
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            MoveTo("AgathasRoom", 4, 2);
            Press(Joypad.Right);
        });
    }
    void SabrinaMovement()
    {
        Comparison.Compare("basesaves/red/sabrina.gqs", () =>
        {
            Execute("R");
            UseItem("BICYCLE");
            // MoveTo(178, 10, 8, Action.Left);
            TalkTo("SaffronGym", 9, 8, Action.Left);
        }, () =>
        {
            Execute("R");
            UseItem("BICYCLE");
            MoveTo(10, 1, 6);
            MoveTo(10, 20, 4);
            MoveTo(178, 8, 16);
            MoveTo(178, 16, 15);
            MoveTo(178, 19, 4);
            MoveTo(178, 11, 10);
            // MoveTo(178, 9, 9, Action.Up);
            TalkTo("SaffronGym", 9, 8, Action.Up);
        });
    }
    void ErikaMovement()
    {
        Comparison.Compare("basesaves/red/erika.gqs", () =>
        {
            // AdvanceFrames(10);
            PickupItem();
            Dig();
            UseItem("BICYCLE");
            Execute("L");
            Press(Joypad.Left);
            ClearText(); // repel
            MoveTo(36, 22);
            // MoveTo(35, 31, Action.Down);
            CutAt(35, 32);
            CutAt(134, 2, 4);
            MoveTo(3, 4);
        }, () =>
        {
            // AdvanceFrames(10);
            PickupItem();
            Dig();
            UseItem("BICYCLE");
            Execute("D");
            Press(Joypad.Down);
            ClearText(); // repel
            MoveTo(41, 14);
            MoveTo(37, 23);
            MoveTo(25, 30);
            // MoveTo(35, 31, Action.Down);
            CutAt(35, 32);
            MoveTo(33, 33);
            MoveTo(21, 32);
            MoveTo(5, 29);
            MoveTo(134, 4, 16);
            MoveTo(134, 1, 9);
            CutAt(134, 2, 4);
            MoveTo(3, 4);
        });
    }
    void PostSilphGio()
    {
        Comparison.Compare("basesaves/red/postsilphgio.gqs", () =>
        {
            ClearText();
            Execute("D"); // after gio (?)

            TalkTo(236, 3, 0);
            ChooseListItem(9);
            MoveTo(2, 3);
            Execute("D"); // exit elevator

            MoveTo(3, 9);
        }, () =>
        {
            ClearText();
            Execute("D"); // after gio (?)

            MoveTo(234, 3, 9);
        });
        // return;
        Comparison.Compare("basesaves/red/postsilphgio.gqs", () =>
        {
            ClearText();
            Execute("D"); // after gio (?)

            TalkTo(236, 3, 0);
            ChooseListItem(9);
            MoveTo(2, 3);
            Execute("D"); // exit elevator

            MoveTo(3, 9);
            MoveTo(2, 9); // thinks trainer is still there (?)
            PickupItemAt(234, 2, 12);
            PickupItemAt(234, 4, 14);
            Dig();

            UseItem("BICYCLE");
            MoveTo("Route16", 27, 10);
        }, () =>
        {
            ClearText();
            Execute("D"); // after gio (?)

            MoveTo(212, 5, 6);
            MoveTo(208, 14, 8);
            MoveTo(208, 18, 7);
            MoveTo(208, 18, 1);
            MoveTo(236, 1, 2);

            TalkTo(236, 3, 0);
            ChooseListItem(9);
            MoveTo(3, 2);
            MoveTo(2, 3);
            Execute("D"); // exit elevator

            MoveTo(12, 3);
            MoveTo(6, 7);
            MoveTo(4, 9);

            MoveTo(3, 9);
            MoveTo(2, 9); // thinks trainer is still there (?)
            PickupItemAt(234, 2, 12);
            PickupItemAt(234, 4, 14);
            Dig();

            UseItem("BICYCLE");
            // MoveTo(41, 13);
            MoveTo(31, 13);
            MoveTo(19, 14);
            MoveTo(6, 18);
            MoveTo("Route16", 27, 10);
        });
    }
    void Elevator()
    {
        Comparison.Compare("basesaves/red/elevator.gqs", () =>
        {
            TalkTo(236, 3, 0);
            ChooseListItem(8);
            MoveTo(2, 3);
            Execute("D");
            MoveTo(234, 7, 3, Action.Left);
        }, () =>
        {
            TalkTo(236, 3, 0);
            ChooseListItem(9);
            MoveTo(2, 3);
            Execute("D");
            MoveTo(234, 7, 3, Action.Left);
        }, () =>
        {
            TalkTo(236, 3, 0);
            ChooseListItem(10);
            MoveTo(2, 3);
            Execute("D");
            MoveTo(234, 7, 3, Action.Left);
        }, () =>
        {
            MoveTo(208, 20, 1);
            MoveTo(234, 7, 3, Action.Left);
        });
        Comparison.Compare("basesaves/red/elevator.gqs", () =>
        {
            TalkTo(236, 3, 0);
            ChooseListItem(9);
            MoveTo(2, 3);
            Execute("D");
            MoveTo(234, 7, 3, Action.Left);
        }, () =>
        {
            MoveTo(236, 3, 1, Action.Up);
            Execute("A");
            // AdvanceFrames(92, Joypad.Down); // 9f
            // AdvanceFrames(99, Joypad.Down); // 9f
            AdvanceFrames(100, Joypad.Down); // 10f
            // AdvanceFrames(108, Joypad.Down); // 11f
            // AdvanceFrames(114, Joypad.Down); // 11f
            Press(Joypad.A);
            MoveTo(2, 3);
            Execute("D");
            MoveTo(234, 7, 3, Action.Left);
        });
    }
    void CeruleanLedge()
    {
        Comparison.Compare("basesaves/red/ceruleanledge.gqs", () =>
        {
            MoveTo(15, 76, 8);
            MoveTo(15, 76, 10);
            MoveTo(3, 19, 17);
        }, () =>
        {
            MoveTo(15, 77, 8);
            MoveTo(15, 77, 10);
            MoveTo(3, 19, 17);
        }, () =>
        {
            MoveTo(15, 78, 8);
            MoveTo(15, 78, 10);
            MoveTo(3, 19, 17);
        }, () =>
        {
            MoveTo(15, 79, 8);
            MoveTo(15, 79, 10);
            MoveTo(3, 19, 17);
        });
    }
    void ThrashvsHA()
    {
        byte[] state1, state2, state3, state4;

        LoadState("basesaves/red/oddishlass_noredbar.gqs");
        Press(Joypad.A);
        ClearText();
        ForceTurn(new RbyTurn("MEGA PUNCH"));
        ForceTurn(new RbyTurn("HORN ATTACK"));
        state1 = SaveState();
        LoadState("basesaves/red/oddishlass_redbar.gqs");
        Press(Joypad.A);
        ClearText();
        ForceTurn(new RbyTurn("MEGA PUNCH"));
        ForceTurn(new RbyTurn("HORN ATTACK"));
        state2 = SaveState();
        LoadState("basesaves/red/oddishlass_noredbar_s2.gqs");
        Press(Joypad.A);
        ClearText();
        ForceTurn(new RbyTurn("MEGA PUNCH"));
        ForceTurn(new RbyTurn("HORN ATTACK"));
        state3 = SaveState();
        LoadState("basesaves/red/oddishlass_redbar_s2.gqs");
        Press(Joypad.A);
        ClearText();
        ForceTurn(new RbyTurn("MEGA PUNCH"));
        ForceTurn(new RbyTurn("HORN ATTACK"));
        state4 = SaveState();

        Comparison.Compare("oddishthrash", state1, () =>
        {
            LoadState(state1);
            TeachLevelUpMove("WATER GUN");
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state1);
            TeachLevelUpMove("WATER GUN");
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        }, () =>
        {
            LoadState(state2);
            TeachLevelUpMove("WATER GUN");
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state2);
            TeachLevelUpMove("WATER GUN");
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        }, () =>
        {
            LoadState(state3);
            TeachLevelUpMove("WATER GUN");
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state3);
            TeachLevelUpMove("WATER GUN");
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        }, () =>
        {
            LoadState(state4);
            TeachLevelUpMove("WATER GUN");
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state4);
            TeachLevelUpMove("WATER GUN");
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        });

        LoadState("basesaves/red/4ttg_noredbar.gqs");
        Press(Joypad.A);
        ClearText();
        ForceTurn(new RbyTurn("THRASH", ThreeTurn));
        ForceTurn(new RbyTurn("THRASH"));
        state1 = SaveState();
        LoadState("basesaves/red/4ttg_redbar.gqs");
        Press(Joypad.A);
        ClearText();
        ForceTurn(new RbyTurn("THRASH", ThreeTurn));
        ForceTurn(new RbyTurn("THRASH"));
        state2 = SaveState();

        Comparison.Compare("4ttgthrash", state1, () =>
        {
            LoadState(state1);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state1);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        }, () =>
        {
            LoadState(state2);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state2);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        });

        LoadState("basesaves/red/surge_noredbar.gqs");
        ClearText();
        ForceTurn(new RbyTurn("THRASH", ThreeTurn));
        ForceTurn(new RbyTurn("THRASH"));
        ForceTurn(new RbyTurn("THRASH"), new RbyTurn("GROWL", Miss));
        state1 = SaveState();
        LoadState("basesaves/red/surge_redbar.gqs");
        ClearText();
        ForceTurn(new RbyTurn("THRASH", ThreeTurn));
        ForceTurn(new RbyTurn("THRASH"));
        ForceTurn(new RbyTurn("THRASH"), new RbyTurn("GROWL", Miss));
        BattleSwitch("PIDGEY", new RbyTurn("THUNDERBOLT"));
        state2 = SaveState();
        LoadState("basesaves/red/surge_noredbar_s2.gqs");
        ClearText();
        ForceTurn(new RbyTurn("THRASH", ThreeTurn));
        ForceTurn(new RbyTurn("THRASH"));
        ForceTurn(new RbyTurn("THRASH"), new RbyTurn("GROWL", Miss));
        state3 = SaveState();
        LoadState("basesaves/red/surge_redbar_s2.gqs");
        ClearText();
        ForceTurn(new RbyTurn("THRASH", ThreeTurn));
        ForceTurn(new RbyTurn("THRASH"));
        ForceTurn(new RbyTurn("THRASH"), new RbyTurn("GROWL", Miss));
        BattleSwitch("PIDGEY", new RbyTurn("THUNDERBOLT"));
        state4 = SaveState();

        Comparison.Compare("surgethrash", state1, () =>
        {
            LoadState(state1);
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state1);
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        }, () =>
        {
            LoadState(state2);
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state2);
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        }, () =>
        {
            LoadState(state3);
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state3);
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        }, () =>
        {
            LoadState(state4);
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            LoadState(state4);
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        });
    }
    void Boulder()
    {
        Comparison.Compare("basesaves/red/boulder.gqs", () =>
        {
            MoveTo("VictoryRoad3F", 23, 6);
            Strength();
            MoveTo(22, 4);

            for(int i = 0; i < 2; i++) { PushBoulder(Joypad.Up); Execute("U"); }
            Execute("R U");
            for(int i = 0; i < 16; i++) { PushBoulder(Joypad.Left); Execute("L"); }

            Execute("U L");
            PushBoulder(Joypad.Down);
            Execute("R D D");
            for(int i = 0; i < 4; i++) { PushBoulder(Joypad.Left); Execute("L"); }
            Execute("U L");
            for(int i = 0; i < 3; i++) { PushBoulder(Joypad.Down); Execute("D"); }
            Execute("L D");
            PushBoulder(Joypad.Right); Execute("U");
            MoveTo(21, 15, Action.Right);
            PushBoulder(Joypad.Right);
            Execute("R R");
            FallDown();
            AdvanceFrames(300);
        }, () =>
        {
            MoveTo("VictoryRoad3F", 23, 6);
            // return;
            Strength();
            MoveTo(22, 5);

            AdvanceFrames(8, Joypad.Up);
            // AdvanceFrames(140, Joypad.Up);

            AdvanceFrames(165, Joypad.Up);
            AdvanceFrames(10, Joypad.Right);
            AdvanceFrames(10, Joypad.Up);
            AdvanceFrames(1330, Joypad.Left);
            AdvanceFrames(300);
        });
    }
    void MoonXP()
    {
        Comparison.Compare("basesaves/red/moonxp.gqs", () =>
        {
            AfterMoveAndSplit();
            MoveTo(24, 3);
            ForceEncounter(Action.Right, 5, 0x9888);
            ClearText();
            ForceTurn(new RbyTurn("POISON STING"));
            MoveTo(26, 3);
            MoveAndSplit(Joypad.Right);
        }, () =>
        {
            AfterMoveAndSplit();
            MoveTo(24, 3);
            ForceEncounter(Action.Right, 5, 0x9888);
            RunAway();
            MoveTo(26, 3);
            MoveAndSplit(Joypad.Right);
        }, () =>
        {
            AfterMoveAndSplit();
            MoveTo(24, 3);
            ForceEncounter(Action.Right, 9, 0x9888);
            ClearText();
            ForceTurn(new RbyTurn("WATER GUN"));
            MoveTo(26, 3);
            MoveAndSplit(Joypad.Right);
        }, () =>
        {
            AfterMoveAndSplit();
            MoveTo(24, 3);
            ForceEncounter(Action.Right, 9, 0x9888);
            RunAway();
            MoveTo(26, 3);
            MoveAndSplit(Joypad.Right);
        });
    }
    void EarlyMisty()
    {
        RbyTurn.DefaultRoll = 20;
        Scenario L24Misty = () =>
        {
            // Bill menu
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");

            TalkTo("BikeShop", 6, 3);
            No();
            ClearText(); // got instant text

            // MISTY MINION
            MoveTo("CeruleanGym", 5, 3);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("TAIL WHIP"));
            ForceTurn(new RbyTurn("THRASH"));

            // MISTY
            TalkTo(4, 2);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn(AiItem));

            // DIG ROCKET
            MoveTo("CeruleanCity", 30, 9);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            MoveTo("CeruleanCity", 36, 19);
        };
        Scenario RedBarMisty = () =>
        {
            // Bill menu
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");

            TalkTo("BikeShop", 6, 3);
            No();
            ClearText(); // got instant text

            // DIG ROCKET
            MoveTo("CeruleanCity", 30, 9);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("KARATE CHOP", Crit));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // MISTY MINION
            MoveTo("CeruleanGym", 5, 3);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("TAIL WHIP"));
            ForceTurn(new RbyTurn("THRASH"));

            // MISTY
            TalkTo(4, 2);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", Crit), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("THRASH"));
            MoveTo("CeruleanCity", 36, 19);
        };
        Scenario L25Misty = () =>
        {
            // Bill menu
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");

            TalkTo("BikeShop", 6, 3);
            No();
            ClearText(); // got instant text

            // DIG ROCKET
            MoveTo("CeruleanCity", 30, 9);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("KARATE CHOP", Crit));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // MISTY MINION
            MoveTo("CeruleanGym", 5, 3);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("TAIL WHIP"));
            ForceTurn(new RbyTurn("THRASH"));

            // MISTY
            TalkTo(4, 2);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", Crit), new RbyTurn("BUBBLEBEAM", AiItem));
            ForceTurn(new RbyTurn("THRASH"));
            MoveTo("CeruleanCity", 36, 19);
        };
        Comparison.Compare("earlymisty24", "basesaves/red/earlymisty.gqs", L24Misty, L25Misty);
        Comparison.Compare("earlymistyredbar", "basesaves/red/earlymisty.gqs", RedBarMisty, L25Misty);
        void Prepare()
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 18);
            CpuWriteBE<ushort>("wPartyMon1MaxHP", 78);
            CpuWrite("wPartyMon1Level", 23);
            CpuWrite(SYM["wBagItems"] + 1, 6);
            CpuWrite(SYM["wBagItems"] + 4, Items["ESCAPE ROPE"].Id);
            CpuWrite(SYM["wBagItems"] + 8, Items["RARE CANDY"].Id);
        }
        Comparison.Compare("basesaves/red/earlymistymvt.gqs",  () =>
        {
            Prepare();
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");

            TalkTo("BikeShop", 6, 3);
            No();
            ClearText();
            MoveTo("CeruleanGym", 2, 7);
            MoveTo("CeruleanGym", 5, 2);
            MoveTo("CeruleanCity", 30, 9);
            MoveTo("CeruleanCity", 36, 19);
        }, () =>
        {
            Prepare();
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");
            TalkTo("CeruleanPokecenter", 3, 2);
            Yes();
            ClearText();

            TalkTo("BikeShop", 6, 3);
            No();
            ClearText();
            MoveTo("CeruleanGym", 2, 7);
            MoveTo("CeruleanGym", 5, 2);
            MoveTo("CeruleanCity", 30, 9);
            MoveTo("CeruleanCity", 36, 19);
        }, () =>
        {
            Prepare();
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");

            TalkTo("BikeShop", 6, 3);
            No();
            ClearText();
            MoveTo("CeruleanCity", 30, 9);
            MoveTo("CeruleanGym", 2, 7);
            MoveTo("CeruleanGym", 5, 2);
            MoveTo("CeruleanCity", 36, 19);
        });
    }
    void PostMisty()
    {
        Comparison.Compare("basesaves/red/postmisty.gqs", () =>
        {
            ClearText();
            MoveTo(3, 36, 31);
            MoveTo(16, 18, 23);
            MoveTo(16, 17, 27);
        }, () =>
        {
            ClearText();
            MoveTo(3, 22, 18);
            MoveTo(3, 17, 16);
            MoveTo(3, 8, 12);
            MoveTo(3, 32, 11);
            MoveTo(3, 33, 18);

            MoveTo(16, 17, 27);
        });
    }
    void BlaineDig()
    {
        Comparison.Compare("basesaves/red/blainedig.gqs", () =>
        {
            ClearText(6);
            AdvanceFrames(53);
            Press(Joypad.B);
            AdvanceFrames(10);
            OpenStartMenu();
            Dig();
        }, () =>
        {
            ClearText(6);
            AdvanceFrames(53);
            Press(Joypad.B);
            AdvanceFrames(20);
            OpenStartMenu();
            Dig();
        });
    }
    void BlaineFirst()
    {
        Comparison.Compare("basesaves/red/blainefirst.gqs", () =>
        {
            PickupItem();
            Dig();

            Fly("CinnabarIsland");
            UseItem("BICYCLE");
            MoveTo("CinnabarGym", 16, 16);//simplified
            {
                TalkTo("CinnabarGym", 15, 7, Action.Up);
                BlaineQuiz(Joypad.A);
                TalkTo(10, 1, Action.Up);
                BlaineQuiz(Joypad.B);
                TalkTo(9, 7, Action.Up);
                BlaineQuiz(Joypad.B);
                TalkTo(9, 13, Action.Up);
                BlaineQuiz(Joypad.B);
                TalkTo(1, 13, Action.Up);
                BlaineQuiz(Joypad.A);
                TalkTo(1, 7, Action.Up);
                BlaineQuiz(Joypad.B);
                // BLAINE
                TalkTo(3, 3);
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
                ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("AGILITY"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
            }
            Dig();

            UseItem("BICYCLE");
            CutAt(35, 32);
            MoveTo("CeladonGym", 4, 16);//simplified
            {
                CutAt("CeladonGym", 2, 4);
                // BEAUTY
                MoveTo(3, 4);
                ClearText();
                ForceTurn(new RbyTurn("BLIZZARD"));
                // ERIKA
                TalkTo(4, 3);
                ForceTurn(new RbyTurn("EARTHQUAKE"));
                ForceTurn(new RbyTurn("BLIZZARD"));
                ForceTurn(new RbyTurn("EARTHQUAKE"));
                CutAt(5, 7);
            }
            MoveTo("CeladonCity", 12, 28);

            Fly("SaffronCity");
            UseItem("BICYCLE");
            MoveTo("SaffronGym", 8, 16);
        }, () =>
        {
            PickupItem();
            Dig();

            UseItem("BICYCLE");
            CutAt(35, 32);
            MoveTo("CeladonGym", 4, 16);//simplified
            {
                CutAt("CeladonGym", 2, 4);
                // BEAUTY
                MoveTo(3, 4);
                ClearText();
                ForceTurn(new RbyTurn("BLIZZARD"));
                // ERIKA
                TalkTo(4, 3);
                ForceTurn(new RbyTurn("EARTHQUAKE"));
                ForceTurn(new RbyTurn("BLIZZARD"));
                ForceTurn(new RbyTurn("EARTHQUAKE"));
                CutAt(5, 7);
            }
            MoveTo("CeladonCity", 12, 28);

            Fly("CinnabarIsland");
            UseItem("BICYCLE");
            MoveTo("CinnabarGym", 16, 16);//simplified
            {
                TalkTo("CinnabarGym", 15, 7, Action.Up);
                BlaineQuiz(Joypad.A);
                TalkTo(10, 1, Action.Up);
                BlaineQuiz(Joypad.B);
                TalkTo(9, 7, Action.Up);
                BlaineQuiz(Joypad.B);
                TalkTo(9, 13, Action.Up);
                BlaineQuiz(Joypad.B);
                TalkTo(1, 13, Action.Up);
                BlaineQuiz(Joypad.A);
                TalkTo(1, 7, Action.Up);
                BlaineQuiz(Joypad.B);
                // BLAINE
                TalkTo(3, 3);
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
                ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("AGILITY"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
            }
            Dig();

            UseItem("BICYCLE");
            MoveTo(18, 18, 10);
            UseItem("BICYCLE");
            MoveTo("SaffronGym", 8, 16);
        });
    }
    void FlySaffron()
    {
        Comparison.Compare("basesaves/red/flysaffron.gqs", () =>
        {
            ClearText();
            Dig();
            UseItem("BICYCLE");
            MoveTo(18, 18, 10);
            UseItem("BICYCLE");
            MoveTo("SaffronGym", 8, 17);
        }, () =>
        {
            ClearText();
            Dig();
            Fly("SaffronCity");
            UseItem("BICYCLE");
            MoveTo("SaffronGym", 8, 17);
        });
    }
    void BikevsWalk()
    {
        Comparison.Compare("basesaves/red/biketower.gqs", () =>
        {
            UseItem("BICYCLE");
            MoveTo(14, 5);
        }, () =>
        {
            MoveTo(14, 5);
        });
        Comparison.Compare("basesaves/red/bikekoga.gqs", () =>
        {
            UseItem("BICYCLE");
            // Press(Joypad.Start, Joypad.Down, Joypad.A, Joypad.Up | Joypad.A); // slot2
            // Press(Joypad.Start, Joypad.Down, Joypad.A, Joypad.Up, Joypad.Up | Joypad.A); // slot3
            // ClearText();
            MoveTo(5, 28);
            Inject(Joypad.Up);
            RunUntil("EnterMap");
        }, () =>
        {
            MoveTo(5, 28);
            Inject(Joypad.Up);
            RunUntil("EnterMap");
        });
        Comparison.Compare("basesaves/red/bikeflyhouse.gqs", () =>
        {
            MoveTo(27, 17, 4);
            UseItem("BICYCLE");
            MoveTo(7, 6);
            Inject(Joypad.Up);
            RunUntil("EnterMap");
        }, () =>
        {
            MoveTo(27, 7, 6);
            Inject(Joypad.Up);
            RunUntil("EnterMap");
        });
        Comparison.Compare("basesaves/red/bikeflyhouse.gqs", () =>
        {
            CurrentMenuType = MenuType.None;
            MoveTo(27, 17, 4);
            UseItem("BICYCLE");
            TalkTo("Route16FlyHouse", 2, 3);
            MoveTo("Route16", 7, 6);
            ItemSwap("HELIX FOSSIL", "TM07");
        }, () =>
        {
            CurrentMenuType = MenuType.None;
            TalkTo("Route16FlyHouse", 2, 3);
            MoveTo("Route16", 7, 6);
            ItemSwap("HELIX FOSSIL", "TM07");
        });
        Comparison.Compare("basesaves/red/biketower2.gqs", () =>
        {
            MoveTo("Route16", 7, 6);
            ItemSwap("S.S.TICKET", "TM07");
            UseItem("SUPER REPEL");
            UseItem("HM02", "PIDGEY");
            Press(Joypad.Up | Joypad.Left, Joypad.Up | Joypad.Right, Joypad.Up | Joypad.Left, Joypad.Up | Joypad.Right);
            Fly("LavenderTown");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            UseItem("BICYCLE");
            MoveTo(14, 5);
        }, () =>
        {
            MoveTo("Route16", 7, 6);
            ItemSwap("S.S.TICKET", "TM07");
            UseItem("SUPER REPEL");
            UseItem("HM02", "PIDGEY");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            Fly("LavenderTown");
            MoveTo(14, 5);
        });
    }
    void Champ()
    {
        void Setup()
        {
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
        }
        void BlizzPidgeot()
        {
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }
        void EQAlakazam()
        {
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }
        void BlizzRhydon()
        {
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }
        void TBGyarados()
        {
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }
        void DrillAll()
        {
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }

        Comparison.Compare("champanims", "basesaves/red/champ.gqs", () =>
        {
            Setup();
            BlizzPidgeot();
        }, () =>
        {
            Setup();
            EQAlakazam();
        }, () =>
        {
            Setup();
            BlizzRhydon();
        }, () =>
        {
            Setup();
            TBGyarados();
        }, () =>
        {
            CpuWrite("wPartyMon2PP", 6);
            Setup();
            DrillAll();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 35);
            Setup();
            BlizzPidgeot();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 35);
            Setup();
            EQAlakazam();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 35);
            Setup();
            BlizzRhydon();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 35);
            Setup();
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 35);
            CpuWrite("wPartyMon2PP", 6);
            Setup();
            DrillAll();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            Setup();
            BlizzPidgeot();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            Setup();
            EQAlakazam();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            Setup();
            BlizzRhydon();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            Setup();
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            CpuWrite("wPartyMon2PP", 6);
            Setup();
            DrillAll();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WING ATTACK", 20));
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            CpuWrite("wPartyMon2PP", 6);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WING ATTACK", 20));
            DrillAll();
        });
        Comparison.Compare("champstall", "basesaves/red/champ.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            CpuWrite(SYM["wBagItems"] + 3, 1);
            Setup();
            ForceTurn(new RbyTurn("POKE FLUTE"), new RbyTurn("WING ATTACK", 20));
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            CpuWrite(SYM["wBagItems"] + 3, 1);
            Setup();
            ForceTurn(new RbyTurn("POKE FLUTE"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("POKE FLUTE"), new RbyTurn("WING ATTACK", 20));
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            CpuWrite(SYM["wBagItems"] + 3, 1);
            Setup();
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("WING ATTACK", 20));
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            CpuWrite(SYM["wBagItems"] + 3, 1);
            Setup();
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("WING ATTACK", 20));
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 40);
            CpuWrite(SYM["wBagItems"] + 3, 1);
            Setup();
            BlizzRhydon();
        });
        Comparison.Compare("champwwmm", "basesaves/red/champ.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
        Comparison.Compare("champskyattack", "basesaves/red/champ.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 34);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 34);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 34);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TAIL WHIP"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 34);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FURY ATTACK", 2 * Turns | 20));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 34);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("REFLECT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 34);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("SUPER POTION", "NIDOKING"), new RbyTurn("SKY ATTACK", 20));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 34);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK", Miss));
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 34);
            Setup();
            TBGyarados();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 34);
            CpuWrite("wPartyMon2PP", 6);
            Setup();
            DrillAll();
        });
    }
    void PalletSurf()
    {
        Comparison.Compare("basesaves/red/palletsurf.gqs", () =>
        {
            MoveTo(4, 8); // no troll

            MoveTo(4, 13, Action.Down);
            UseItem("SUPER REPEL");
            UseItem("HM03", "SQUIRTLE");
            Surf();
            MoveTo(32, 4, 0);
        }, () =>
        {
            MoveTo(4, 8); // no troll

            MoveTo(4, 17, Action.Right);
            UseItem("SUPER REPEL");
            UseItem("HM03", "SQUIRTLE");
            Surf();
            MoveTo(32, 4, 0);
        });
        Comparison.Compare("basesaves/red/palletsurf.gqs", () =>
        {
            UseItem("SUPER REPEL");
            UseItem("HM03", "SQUIRTLE");
            TossItem("PARLYZ HEAL", 0);
            UseItem("BICYCLE");
            MoveTo(4, 8, Action.Left); // no troll
            Inject(Joypad.Left);
            AdvanceFrames(5, Joypad.Left);
            Execute("D");

            MoveTo(3, 17, Action.Right);
            Surf();
            MoveTo(32, 4, 0);
        }, () =>
        {
            MoveTo(4, 8); // no troll

            MoveTo(4, 13, Action.Down);
            UseItem("SUPER REPEL");
            UseItem("HM03", "SQUIRTLE");
            TossItem("PARLYZ HEAL", 0);
            Surf();
            MoveTo(32, 4, 0);
        });
    }
    void EarlyPotions()
    {
        Comparison.Compare("basesaves/red/pcpotion.gqs", () =>
        {
            ClearText();
            TalkTo(0, 1);
            WithdrawItems("POTION", 1);
            MoveTo(7, 1);
        }, () =>
        {
            ClearText();
            MoveTo(7, 1);
        });
        Comparison.Compare("basesaves/red/martguypotion.gqs", () =>
        {
            MoveTo(8, 27);
            TalkTo(5, 24);
            MoveTo(11, 24);
            MoveTo(12, 23);
        }, () =>
        {
            MoveTo(8, 27);
            MoveTo(11, 24);
            MoveTo(12, 23);
        });
        Comparison.Compare("basesaves/red/treepotion.gqs", () =>
        {
            PickupItemAt(14, 4);
            MoveTo(13, 7, 71);
        }, () =>
        {
            MoveTo(13, 7, 71);
        });
        Comparison.Compare("basesaves/red/extrapotion.gqs", () =>
        {
            MoveTo(7, 24);
            PickupItemAt(12, 29);
            MoveTo(1, 21, Action.Up);
        }, () =>
        {
            MoveTo(7, 22);
            MoveTo(1, 21, Action.Up);
        });
        Scenario WeedleFight = () =>
        {
            // ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            // ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            // ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            // ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            // ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            CpuWriteBE<ushort>("wEnemyMonHP", 0);
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
        };
        Comparison.Compare("basesaves/red/weedlepotion.gqs", () =>
        {
            PickupItemAt(1, 18);
            MoveTo(1, 18);
            ClearText();
            WeedleFight();
            MoveTo(1, 17);
        }, () =>
        {
            TalkTo(2, 18);
            WeedleFight();
            MoveTo(1, 17);
        });
    }
    void BrunoMenu()
    {
        Comparison.Compare("basesaves/red/brunomenu.gqs", () =>
        {
            ClearText();
            Execute("U U U U U");
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            MoveTo(4, 2);
            Press(Joypad.Right, Joypad.A);
        }, () =>
        {
            ClearText();
            Execute("U");
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            Execute("U U");
            MoveTo(4, 2);
            Press(Joypad.Right, Joypad.None, Joypad.A);
            // Press(Joypad.Right, Joypad.A);
        });
    }
    void Missvs2Shot()
    {
        Comparison.Compare("basesaves/red/nerd13.gqs", () =>
        {
            ForceTurn(new RbyTurn("WATER GUN", 1), new RbyTurn("POUND", 20));
            ForceTurn(new RbyTurn("POISON STING"), null, true, false);
        }, () =>
        {
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("POUND", 20));
            ForceTurn(new RbyTurn("MEGA PUNCH"), null, true, false);
        });
    }
    void FuchsiaCut()
    {
        Comparison.Compare("basesaves/red/fuchsiacut.gqs", () =>
        {
            CutAt(18, 19);
            CutAt(16, 12);
            MoveTo(18, 4);
            Inject(Joypad.Up);
            RunUntil("EnterMap");
        }, () =>
        {
            MoveTo(18, 4);
            Inject(Joypad.Up);
            RunUntil("EnterMap");
        });
    }
    void WildEncounter()
    {
        Comparison.Compare("basesaves/red/wildrt1.gqs", () =>
        {
            MoveTo(14, 8);
            ForceEncounter(Action.Up, 0, 0x8888); // pidgey
            RunAway();
            MoveTo(14, 5);
        }, () =>
        {
            MoveTo(14, 8);
            ForceEncounter(Action.Up, 1, 0x8888); // rat
            RunAway();
            MoveTo(14, 5);
        }, () =>
        {
            MoveTo(14, 5);
        });
        Comparison.Compare("basesaves/red/wildforest_squirtle.gqs", () =>
        {
            MoveTo(1, 8);
            ForceEncounter(Action.Up, 0, 0x8888); // weedle
            RunAway();
            MoveTo(1, 5);
        }, () =>
        {
            MoveTo(1, 8);
            ForceEncounter(Action.Up, 1, 0x8888); // kakuna
            RunAway();
            MoveTo(1, 5);
        }, () =>
        {
            MoveTo(1, 5);
        });
        Comparison.Compare("basesaves/red/wildforest_nidoran.gqs", () =>
        {
            MoveTo(1, 8);
            ForceEncounter(Action.Up, 0, 0x8888); // weedle
            RunAway();
            MoveTo(1, 5);
        }, () =>
        {
            MoveTo(1, 8);
            ForceEncounter(Action.Up, 1, 0x8888); // kakuna
            RunAway();
            MoveTo(1, 5);
        }, () =>
        {
            MoveTo(1, 5);
        });
        Comparison.Compare("basesaves/red/wildrt3.gqs", () =>
        {
            MoveTo(27, 9);
            ForceEncounter(Action.Down, 0, 0x8888); // pidgey
            RunAway();
            MoveTo(27, 11);
        }, () =>
        {
            MoveTo(27, 9);
            ForceEncounter(Action.Down, 1, 0x8888); // spearow
            RunAway();
            MoveTo(27, 11);
        }, () =>
        {
            MoveTo(27, 9);
            ForceEncounter(Action.Down, 9, 0x8888); // puff
            RunAway();
            MoveTo(27, 11);
        }, () =>
        {
            MoveTo(27, 11);
        });
        Comparison.Compare("basesaves/red/wildmoon_nidoran.gqs", () =>
        {
            MoveTo(12, 14);
            ForceEncounter(Action.Up, 0, 0x8888); // zubat
            RunAway();
            MoveTo(12, 9);
        }, () =>
        {
            MoveTo(12, 14);
            ForceEncounter(Action.Up, 1, 0x8888); // geo
            RunAway();
            MoveTo(12, 9);
        }, () =>
        {
            MoveTo(12, 14);
            ForceEncounter(Action.Up, 5, 0x8888); // paras
            RunAway();
            MoveTo(12, 9);
        }, () =>
        {
            MoveTo(12, 9);
        });
        Comparison.Compare("basesaves/red/wildmoon_nidoking.gqs", () =>
        {
            AfterMoveAndSplit();
            MoveTo(24, 3);
            ForceEncounter(Action.Right, 0, 0x8888); // zubat
            RunAway();
            MoveTo(26, 3);
            MoveAndSplit(Joypad.Right);
        }, () =>
        {
            AfterMoveAndSplit();
            MoveTo(24, 3);
            ForceEncounter(Action.Right, 2, 0x8888); // geo
            RunAway();
            MoveTo(26, 3);
            MoveAndSplit(Joypad.Right);
        }, () =>
        {
            AfterMoveAndSplit();
            MoveTo(24, 3);
            ForceEncounter(Action.Right, 5, 0x8888); // paras
            RunAway();
            MoveTo(26, 3);
            MoveAndSplit(Joypad.Right);
        }, () =>
        {
            AfterMoveAndSplit();
            MoveTo(26, 3);
            MoveAndSplit(Joypad.Right);
        });
        Comparison.Compare("basesaves/red/wildrt6.gqs", () =>
        {
            MoveTo(17, 17);
            ForceEncounter(Action.Down, 1, 0x8888); // pidgey
            RunAway();
            MoveTo(17, 20);
        }, () =>
        {
            MoveTo(17, 17);
            ForceEncounter(Action.Down, 0, 0x8888); // oddish
            RunAway();
            MoveTo(17, 20);
        }, () =>
        {
            MoveTo(17, 17);
            ForceEncounter(Action.Down, 3, 0x8888); // mankey
            RunAway();
            MoveTo(17, 20);
        }, () =>
        {
            MoveTo(17, 20);
        });
        Comparison.Compare("wildrt6rb", "basesaves/red/wildrt6.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            MoveTo(17, 17);
            ForceEncounter(Action.Down, 1, 0x8888);
            RunAway();
            MoveTo(17, 20);
        }, () =>
        {
            MoveTo(17, 17);
            ForceEncounter(Action.Down, 1, 0x8888);
            RunAway();
            MoveTo(17, 20);
        }, () =>
        {
            CpuWrite("wd730", 0);
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            MoveTo(17, 17);
            ForceEncounter(Action.Down, 1, 0x8888);
            RunAway();
            MoveTo(17, 20);
        }, () =>
        {
            CpuWrite("wd730", 0);
            MoveTo(17, 17);
            ForceEncounter(Action.Down, 1, 0x8888);
            RunAway();
            MoveTo(17, 20);
        });
    }
    void BirdCatch()
    {
        // Comparison.Compare("basesaves/red/birdcatchrb.gqs", ()=>{
        Comparison.Compare("basesaves/red/birdcatch.gqs", () =>
        {
            // CpuWriteBE<ushort>("wPartyMon1HP", 8);
            MoveTo(27, 9);
            ForceEncounter(Action.Down, 0, 0x8888);
            ClearText();
            ForceTurn(new RbyTurn("TACKLE", 20), new RbyTurn("GUST", 20));
            ForceYoloball("POKE BALL");
            ClearText();
            No();
            MoveTo(27, 11);
        }, () =>
        {
            MoveTo(27, 11);
        });
        Comparison.Compare("basesaves/red/birdcatchext.gqs", () =>
        {
            Execute(SpacePath("UAUUAURARRRAU"));
            ForceEncounter(Action.Up, 4, 0x8888);
            ForceYoloball("POKE BALL");
            ClearText();
            No();
            MoveTo(3, 44);
        }, () =>
        {
            MoveTo(10, 52);
            MoveTo(10, 47);
            MoveTo(3, 44);
        });
    }
    void SaveAndQuitCompare()
    {
        Comparison.Compare("basesaves/red/saveandquit.gqs", () =>
        {
            MoveTo(59, 5, 5);
            Save();
            AdvanceFrames(32);
            AdvanceFrames(105);
            HardReset();
            NoPal.Execute(this);
            Execute("D");
        }, () =>
        {
            MoveTo(60, 5, 6);
        });
    }
    void LanceMenu()
    {
        Comparison.Compare("basesaves/red/lancemenu.gqs", () =>
        {
            MoveTo(6, 8);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            MoveTo(6, 7);
        }, () =>
        {
            MoveTo(6, 8);
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("ELIXER", "NIDOKING");
            MoveTo(6, 7);
        });
    }
    void Jingles()
    {
        Comparison.Compare("basesaves/red/itemhidden_int.gqs", () =>
        {
            PickupItemAt(1, 18);
            MoveTo(1, 18);
        }, () =>
        {
            MoveTo(1, 18);
        });
        Comparison.Compare("basesaves/red/itemhidden_ext.gqs", () =>
        {
            Execute("L L");
            PickupItem();
            Execute("R R");
        }, () =>
        {
            Execute("L L");
            Execute("R R");
        });
        Comparison.Compare("itemhiddenzone", "basesaves/red/itemhidden_int.gqs", "basesaves/red/itemhidden_ext.gqs", () =>
        {
            Execute("U U");
            PickupItem();
            Execute("D D");
        }, () =>
        {
            Execute("L L");
            PickupItem();
            Execute("R R");
        });
        Comparison.Compare("basesaves/red/itemball_int.gqs", () =>
        {
            PickupItemAt(12, 29);
            MoveTo(12, 26);
        }, () =>
        {
            MoveTo(12, 28);
            MoveTo(12, 26);
        });
        Comparison.Compare("basesaves/red/itemball_ext.gqs", () =>
        {
            PickupItemAt(10, 5);
            MoveTo(7, 5);
        }, () =>
        {
            MoveTo(9, 5);
            MoveTo(7, 5);
        });
        Comparison.Compare("basesaves/red/exclamation.gqs", () =>
        {
            MoveTo(1, 18);
            ClearText(2);
        }, () =>
        {
            MoveTo(2, 19, Action.Up);
            Press(Joypad.A);
            ClearText(1);
        });
    }
    void FluteRepel()
    {
        Scenario FluteSplit = () =>
        {
            // RIVAL
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            // CHANNELER 1
            TalkTo("PokemonTower4F", 15, 7);
            ForceTurn(new RbyTurn("ROCK SLIDE"));
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(12, 10); // elixer
            PickupItemAt("PokemonTower5F", 4, 12); // elixer

            MoveTo("PokemonTower5F", 11, 9);
            ClearText(); // heal pad

            // CHANNELER 2
            MoveTo("PokemonTower6F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            // CHANNELER 3
            TalkTo("PokemonTower6F", 9, 5);
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(6, 8); // rare candy

            MoveTo(10, 15);
            UseItem("TM07", "NIDOKING", "ROCK SLIDE");

            MoveTo(10, 16);
            ClearText();
            ItemSwap("HM01", "SUPER REPEL");
            UseItem("POKE DOLL"); // escape ghost

            // ROCKET 1
            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            // ROCKET 2
            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            // ROCKET 3
            MoveTo(10, 7);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // Get Pokéflute
            TalkTo(10, 3);
            TalkTo(3, 1, Action.Right);

            MoveTo("LavenderTown", 7, 10);
            Fly("CeladonCity");
            TalkTo("CeladonPokecenter", 3, 2);
            Yes();
            ClearText(); // healed at center

            MoveTo("CeladonCity", 41, 10);
            UseItem("BICYCLE");

            MoveTo("Route7Gate", 3, 4);
        };
        Comparison.Compare("basesaves/red/fluterepel.gqs", () =>
        {
            AfterMoveAndSplit();
            UseItem("BICYCLE");

            // Fly house
            CutAt("Route16", 34, 9);
            MoveTo("Route16", 17, 4);
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            UseItem("BICYCLE");

            TalkTo("Route16FlyHouse", 2, 3);

            // Fly menu
            MoveTo("Route16", 7, 6);
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("HM02", "PIDGEY");
            Fly("LavenderTown");

            FluteSplit();
        }, () =>
        {
            AfterMoveAndSplit();
            UseItem("BICYCLE");

            // Fly house
            CutAt("Route16", 34, 9);
            MoveTo("Route16", 17, 4);
            UseItem("BICYCLE");

            TalkTo("Route16FlyHouse", 2, 3);

            // Fly menu
            MoveTo("Route16", 7, 6);
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("HM02", "PIDGEY");
            Fly("LavenderTown");

            FluteSplit();
        });
        Comparison.Compare("basesaves/red/fluterepel2.gqs", () =>
        {
            AfterMoveAndSplit();
            MoveTo(142, 10, 16);
            MoveTo(142, 18, 10);
            MoveTo(143, 18, 8);

            // RIVAL
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(143, 3, 8);
            MoveTo(144, 3, 10);

            // CHANNELER 1
            TalkTo("PokemonTower4F", 15, 7);
            ForceTurn(new RbyTurn("ROCK SLIDE"));
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(12, 10); // elixer
            PickupItemAt("PokemonTower5F", 4, 12); // elixer

            MoveTo("PokemonTower5F", 11, 9);
            ClearText(); // heal pad

            MoveTo(146, 11, 10); //?
            MoveTo(146, 18, 10);
            MoveTo(147, 18, 8);

            // CHANNELER 2
            MoveTo("PokemonTower6F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            // CHANNELER 3
            TalkTo("PokemonTower6F", 9, 5);
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(6, 8); // rare candy

            MoveTo(10, 15);
            UseItem("TM07", "NIDOKING", "ROCK SLIDE");

            MoveTo(10, 16);
            ClearText();
            ItemSwap("HM01", "SUPER REPEL");
            UseItem("POKE DOLL"); // escape ghost

            // ROCKET 1
            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            // ROCKET 2
            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            // ROCKET 3
            MoveTo(10, 7);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // Get Pokéflute
            TalkTo(10, 3);
            MoveTo(3, 6);
            TalkTo(3, 1, Action.Right);

            MoveTo("LavenderTown", 7, 10);
            Fly("CeladonCity");
            TalkTo("CeladonPokecenter", 3, 2);
            Yes();
            // ClearText();
            // MoveTo("CeladonCity", 41, 10);
            ClearText(Joypad.Down);
            Hold(Joypad.Down, SYM["DisplayRepelWoreOffText"]);
            ClearText(Joypad.Down);
            AfterMoveAndSplit();
            UseItem("BICYCLE");

            MoveTo("Route7Gate", 3, 4);
        }, () =>
        {
            AfterMoveAndSplit();
            FluteSplit();
        });
        {
            // 3.557 3.522
            RecordAndTime("fluterepel3", true);
            LoadState("basesaves/red/fluterepel3.gqs");
            ClearText(Joypad.Down);
            Hold(Joypad.Down, SYM["DisplayRepelWoreOffText"]);
            ClearText(Joypad.Down);
            AfterMoveAndSplit();
            Timer.Stop();
            AdvanceFrames(60);
            Dispose();
        }
        Comparison.Compare("basesaves/red/flymenu2.gqs", () =>
        {
            CurrentMenuType = MenuType.None;
            MoveTo("Route16", 17, 4);
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            UseItem("BICYCLE");

            TalkTo("Route16FlyHouse", 2, 3);

            MoveTo("Route16", 7, 6);
            OpenStartMenu();
            ChooseMenuItem(1 + StartMenuOffset());
            SelectMenuItem(2);
            // ItemSwap("S.S.TICKET", "X ACCURACY");
            // UseItem("HM02", "PIDGEY");
        }, () =>
        {
            CurrentMenuType = MenuType.None;
            MoveTo("Route16", 17, 4);
            UseItem("BICYCLE");

            TalkTo("Route16FlyHouse", 2, 3);

            MoveTo("Route16", 7, 6);
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            SelectMenuItem(2);
            // ItemSwap("S.S.TICKET", "X ACCURACY");
            // UseItem("HM02", "PIDGEY");
        });
    }
    void TowerTop()
    {
        Comparison.Compare("basesaves/red/towertop.gqs", () =>
        {
            MoveTo(10, 11, Action.Up);
            ClearText();
            // TalkTo(9, 11);
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            MoveTo(11, 9, Action.Up);
            ClearText();
            // TalkTo(12, 9);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo(10, 7, Action.Up);
            ClearText();
            // TalkTo(9, 7);
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(10, 4, Action.Up);
        }, () =>
        {
            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo(10, 7);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(10, 4);
        });
    }
    void RockTunnelRepel()
    {
        Comparison.Compare("basesaves/red/rocktunnelrepel.gqs", () =>
        {
            ClearText();
            MoveTo(34, 19);
            UseItem("REPEL");
            MoveTo(82, 12, 14);
            // MoveTo(82, 13, 14);
            UseItem("REPEL");

            TalkTo(232, 6, 10);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            MoveTo(3, 3, Action.Left);

            TalkTo("RockTunnel1F", 22, 24);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            PickupItemAt(21, 16, 53);
            MoveTo("Route8", 46, 13);
        }, () =>
        {
            ClearText();
            MoveTo(34, 19);
            UseItem("REPEL");
            MoveTo(8, 10);
            UseItem("REPEL");

            TalkTo(6, 10);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            MoveTo(3, 3, Action.Left);

            TalkTo("RockTunnel1F", 22, 24);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            PickupItemAt(21, 16, 53);
            MoveTo("Route8", 46, 13);
        });
    }
    void BikeClick()
    {
        Comparison.Compare("basesaves/red/bikeclick.gqs", () =>
        {
            Execute("U");
            AdvanceFrames(15);
            UseItem("BICYCLE");
            Execute("L");
        }, () =>
        {
            AdvanceFrames(1);
            Execute("U");
            AdvanceFrames(14);
            UseItem("BICYCLE");
            Execute("L");
        });
        Comparison.Compare("basesaves/red/bikeclick.gqs", () =>
        {
            AdvanceFrames(80);
            Execute("U");
            AdvanceFrames(20);
            UseItem("BICYCLE");
            Execute("L");
        }, () =>
        {
            AdvanceFrames(20);
            Execute("U");
            AdvanceFrames(80);
            UseItem("BICYCLE");
            Execute("L");
        });
    }
    void BlizzRhyhorn()
    {
        Comparison.Compare("basesaves/red/blizzrhyhorn.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD", 1), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD", 1), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD", 1), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
    }
    void SwagMaxEther()
    {
        Comparison.Compare("basesaves/red/swagmaxether.gqs", () =>
        {
            MoveTo(8, 89);
            Press(Joypad.None, Joypad.Down, Joypad.None);
            PickupItem();
            MoveTo(7, 85);
        }, () =>
        {
            MoveTo(8, 89, Action.Down);
            PickupItem();
            MoveTo(7, 85);
        });
    }
    void RepelWearOff()
    {
        Comparison.Compare("basesaves/red/repelwearoff.gqs", () =>
        {
            MoveTo(21, 15, Action.Right);
            PushBoulder(Joypad.Right);
            // UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            UseItem("SUPER REPEL");
            Execute("R R");
            FallDown();

            UseItem("BICYCLE");
            ClearText();
            Strength();
            Execute("D R R U");
            PushBoulder(Joypad.Left);
        }, () =>
        {
            MoveTo(21, 15, Action.Right);
            PushBoulder(Joypad.Right);
            Execute("R R");
            FallDown();

            Strength();
            // UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");
            Execute("D R R U");
            PushBoulder(Joypad.Left);
        });
    }
    void LanceTurnFrame()
    {
        Comparison.Compare("basesaves/red/lanceturnframe.gqs", () =>
        {
            Execute("U");
            MoveTo(6, 3);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            Save();
            MoveTo(6, 2);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            MoveTo(5, 0);
        }, () =>
        {
            Execute("U");
            MoveTo(5, 2);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            Save();
            MoveTo(5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            MoveTo(5, 0);
        });
    }
    void LavenderRival()
    {
        Comparison.Compare("basesaves/red/lavenderrival.gqs", () =>
        {
            MoveTo("PokemonTower2F", 16, 9);
            MoveTo("PokemonTower2F", 16, 5);
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            MoveTo("PokemonTower2F", 5, 5);
            MoveTo("PokemonTower2F", 5, 9);
            MoveTo("PokemonTower2F", 3, 9);
        }, () =>
        {
            MoveTo("PokemonTower2F", 18, 7);
            MoveTo("PokemonTower2F", 14, 7);
            MoveTo("PokemonTower2F", 14, 6);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            MoveTo("PokemonTower2F", 5, 5);
            MoveTo("PokemonTower2F", 5, 9);
            MoveTo("PokemonTower2F", 3, 9);
        });
    }
    void KogaStall()
    {
        Comparison.Compare("basesaves/red/kogastall.gqs", () =>
        {
            ClearText();
            // ForceTurnAndSplit(new RbyTurn("ELIXER", "NIDOKING"), new RbyTurn("SELFDESTRUCT"));
            ForceTurn(new RbyTurn("ELIXER", "NIDOKING"), new RbyTurn(AiItem));
            // ForceTurnAndSplit(new RbyTurn("POKE FLUTE"), new RbyTurn(AiItem));
            // ForceTurnAndSplit(new RbyTurn("POKE FLUTE"), new RbyTurn("SELFDESTRUCT"));
            // ForceTurnAndSplit(new RbyTurn("BUBBLEBEAM"), new RbyTurn(AiItem));
            ForceTurnAndSplit(new RbyTurn("BUBBLEBEAM"), new RbyTurn("SELFDESTRUCT"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("ELIXER", "NIDOKING"), new RbyTurn(AiItem));
            // ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn(AiItem));
            ForceTurnAndSplit(new RbyTurn("X SPECIAL"), new RbyTurn("SELFDESTRUCT"));
        });
    }
    void Yolorelei()
    {
        Comparison.Compare("basesaves/red/yolorelei.gqs", () =>
        {
            TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
            Deposit("SQUIRTLE", "PIDGEY", "PARAS");

            MoveTo("IndigoPlateauLobby", 8, 0);

            // LORELEI
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
            Deposit("SQUIRTLE", "PARAS");

            MoveTo("IndigoPlateauLobby", 8, 0);
            PartySwap("NIDOKING", "PIDGEY");

            // LORELEI
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
            // BattleSwitch("PIDGEY", new RbyTurn("AURORA BEAM"));
            ForceTurn(new RbyTurn("GUST"), new RbyTurn("AURORA BEAM"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
    }
    void ShakeMove()
    {
        Comparison.Compare("basesaves/red/shakemove_it.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("MEGA PUNCH", 10), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("POISON STING"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("MEGA PUNCH", 10), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("HORN ATTACK"));
        });
        Comparison.Compare("basesaves/red/shakeflashmove.gqs", () =>
        {
            ForceTurn(new RbyTurn("POISON STING"));
        }, () =>
        {
            ForceTurn(new RbyTurn("WATER GUN"));
        });
    }
    void NerdVoltorb()
    {
        Comparison.Compare("basesaves/red/nerdvoltorb.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("POISON STING", 1 | SideEffect), new RbyTurn("SCREECH"));
            ForceTurn(new RbyTurn("POISON STING", 1), new RbyTurn("SCREECH"));
            ForceTurn(new RbyTurn("POISON STING", 1), new RbyTurn("SCREECH"));
        }, () =>
        {
            ClearText();
            // ForceTurn(new RbyTurn("WATER GUN", 1 | Crit), new RbyTurn("SCREECH", Miss));
            // ForceTurn(new RbyTurn("WATER GUN", 1 | Crit), new RbyTurn("SCREECH", Miss));
            ForceTurn(new RbyTurn("POISON STING", 1), new RbyTurn("SCREECH"));
            ForceTurn(new RbyTurn("POISON STING", 1), new RbyTurn("SCREECH"));
            ForceTurn(new RbyTurn("POISON STING", 1), new RbyTurn("SCREECH"));
            ForceTurn(new RbyTurn("POISON STING", 1), new RbyTurn("SCREECH"));
        });
    }
    void BC2Caterpie()
    {
        RbyTurn.DefaultRoll = 20;
        Comparison.Compare("basesaves/red/bc2caterpieta.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT", Miss));
            // ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            BattleMenu(0, 0);
            ChooseMenuItem(2); //ha
            Hold(Joypad.A, SYM["MainInBattleLoop.speedEqual"] + 9);
            A = 0;
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            // ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            BattleMenu(0, 0);
            ChooseMenuItem(2); //ha
            Hold(Joypad.A, SYM["MainInBattleLoop.speedEqual"] + 9);
            A = 0;
        });
    }
    void SurfVsWalk()
    {
        Comparison.Compare("surfvswalk", "basesaves/red/surfvswalkw.gqs", "basesaves/red/surfvswalks.gqs", () =>
        {
            MoveTo(4, 17);
            MoveTo(4, 14);
            MoveTo(4, 17);
            MoveTo(4, 14);
        }, () =>
        {
            MoveTo(3, 17);
            MoveTo(3, 14);
            MoveTo(3, 17);
            MoveTo(3, 14);
        });
    }
    void SixtyCans()
    {
        Comparison.Compare("basesaves/red/manip/cans.gqs", () =>
        {
            Execute(SpacePath("DALLLAURUUUUUA"));
            ClearText();
            TalkTo(3, 11, Action.Left);
            MoveTo(4, 9);
        }, () =>
        {
            Execute(SpacePath("SDALLLAURAUUUUUA"));
            ClearText();
            TalkTo(7, 11, Action.Right);
            MoveTo(4, 9);
        });
        Scenario Yolo123 = () =>
        {
            MoveTo(12, 19);
            CpuWrite("wFirstLockTrashCanIndex", 14);
            TalkTo(1, 11, Action.Up);
            TalkTo(5, 11, Action.Right);
            TalkTo(3, 9, Action.Left);
            TalkTo(5, 7, Action.Right);
            TalkTo(1, 7, Action.Down);
            TalkTo(7, 9, Action.Down);
            TalkTo(9, 7, Action.Up);
            TalkTo(9, 11, Action.Down);
            CpuWrite("wSecondLockTrashCanIndex", 13);
            TalkTo(9, 9);
            MoveTo(5, 2, Action.Up);
        };
        Scenario Yolo128 = () =>
        {
            MoveTo(12, 19);
            CpuWrite("wFirstLockTrashCanIndex", 0);
            TalkTo(1, 11, Action.Up);
            TalkTo(5, 11, Action.Up);
            TalkTo(9, 11, Action.Right);
            TalkTo(7, 9, Action.Left);
            TalkTo(9, 7, Action.Up);
            TalkTo(5, 7, Action.Up);
            TalkTo(3, 9, Action.Left);
            TalkTo(1, 7, Action.Down);
            CpuWrite("wSecondLockTrashCanIndex", 0);
            TalkTo(1, 6);
            MoveTo(5, 2, Action.Up);
        };
        Scenario Manip58Cans = () =>
        {
            MoveTo(15, 19);
            SaveAndQuit();
            NoPal.Execute(this, true);
            Execute(SpacePath("DALLLAURUUUUUA"));
            ClearText();
            TalkTo(3, 11, Action.Left);
            MoveTo(5, 2, Action.Up);
        };
        Comparison.Compare("basesaves/red/cans.gqs", () =>
        {
            Yolo128();
        }, () =>
        {
            Yolo123();
        });
    }
    void LateMart()
    {
        Comparison.Compare("basesaves/red/latemart.gqs", () =>
        {
            MoveTo(54, 4, 12);
            TalkTo("PewterMart", 1, 5);
            Buy("POTION", 8);
            MoveTo(2, 34, 17, Action.Right);
        }, () =>
        {
            TalkTo("PewterMart", 1, 5);
            Buy("POTION", 8);
            MoveTo(54, 4, 12);
            MoveTo(2, 34, 17, Action.Right);
        });
    }
    void SideOak()
    {
        Comparison.Compare("basesaves/red/sideoak.gqs", () =>
        {
            MoveTo(40, 5, 3);
            TalkTo(40, 5, 2, Action.Right);
            MoveTo(0, 12, 12);
        }, () =>
        {
            TalkTo(40, 5, 2);
            MoveTo(0, 12, 12);
            // MoveTo(40, 5, 3);
            // TalkTo(40, 5, 2, Action.Down);
            // MoveTo(0, 12, 12);
        });
    }
    void BadLevelUp()
    {
        Comparison.Compare("basesaves/red/badlevelup.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 31);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 32);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });
    }
    void TeachDrill()
    {
        Comparison.Compare("basesaves/red/teachdrill.gqs", () =>
        {
            ClearText();
            PickupItemAt(6, 8);
            MoveTo(10, 14);
            UseItem("TM07", "NIDOKING", "ROCK SLIDE");
            MoveTo(10, 16);
        }, () =>
        {
            ClearText();
            PickupItemAt(6, 8);
            MoveTo(10, 15);
            UseItem("TM07", "NIDOKING", "ROCK SLIDE");
            MoveTo(10, 16);
        });
    }
    void TeachDrillCheckAll()
    {
        void TeachAtCoords(int x, int y, Action dir = Action.None, bool aftercandy = true)
        {
            if(y < 7) aftercandy = false;
            LoadState("basesaves/red/teachdrill.gqs");
            Show();
            TimerComponent timer = new TimerComponent(0, 144, 2.0f);
            Scene.AddComponent(timer);

            AdvanceFrames(3);
            ClearText();
            if(aftercandy) PickupItemAt(6, 8);
            MoveTo(x, y, dir);
            UseItem("TM07", "NIDOKING", "ROCK SLIDE");
            if(!aftercandy) PickupItemAt(6, 8);
            MoveTo(10, 16);

            timer.Running = false;
            Console.WriteLine(x + " " + y + "  " + timer.Text);
            AdvanceFrames(50);
            Scene.Dispose();
        }
        TeachAtCoords(10, 5);
        for(int x = 10; x >= 6; --x) TeachAtCoords(x, 6);
        TeachAtCoords(6, 7, Action.None, false);
        TeachAtCoords(6, 7, Action.None, true);
        for(int y = 8; y <= 14; ++y) TeachAtCoords(6, y);
        TeachAtCoords(7, 10);
        for(int y = 11; y <= 14; ++y) { TeachAtCoords(7, y, Action.Down); TeachAtCoords(7, y, Action.Right); }
        TeachAtCoords(8, 11);
        TeachAtCoords(9, 11);
        TeachAtCoords(10, 11);
        TeachAtCoords(8, 12, Action.Down); TeachAtCoords(8, 12, Action.Right);
        TeachAtCoords(9, 12, Action.Down); TeachAtCoords(9, 12, Action.Right);
        TeachAtCoords(10, 12, Action.Down); TeachAtCoords(10, 12, Action.Right);
        TeachAtCoords(10, 13);
        TeachAtCoords(8, 14);
        TeachAtCoords(9, 14);
        TeachAtCoords(10, 14, Action.Down); TeachAtCoords(10, 14, Action.Right);
        TeachAtCoords(10, 15);
    }
    void FirstOddishHA()
    {
        RbyTurn.DefaultRoll = 10;
        void NormalExp(ushort hp, bool qa = false)
        {
            Comparison.Compare("basesaves/red/oddishha.gqs", () =>
            {
                CpuWriteBE<ushort>("wPartyMon1HP", hp);
                Press(Joypad.A);
                ClearText();
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("ABSORB"));
                ForceTurn(new RbyTurn("POISON STING"));
                ForceTurn(new RbyTurn("HORN ATTACK"), (qa ? new RbyTurn("QUICK ATTACK") : null));
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                TeachLevelUpMove("WATER GUN");
            }, () =>
            {
                CpuWriteBE<ushort>("wPartyMon1HP", hp);
                Press(Joypad.A);
                ClearText();
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("HORN ATTACK"), (qa ? new RbyTurn("QUICK ATTACK") : null));
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                TeachLevelUpMove("WATER GUN");
            });
        }
        void MoonExp(ushort hp, bool qa = false)
        {
            Comparison.Compare("basesaves/red/oddishha.gqs", () =>
            {
                CpuWriteBE<ushort>(SYM["wPartyMon1Exp"] + 1, 8460);
                CpuWriteBE<ushort>("wPartyMon1HP", hp);
                Press(Joypad.A);
                ClearText();
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("ABSORB"));
                ForceTurn(new RbyTurn("POISON STING"));
                ForceTurn(new RbyTurn("HORN ATTACK"), (qa ? new RbyTurn("QUICK ATTACK") : null));
                TeachLevelUpMove("WATER GUN");
                ForceTurn(new RbyTurn("HORN ATTACK"));
            }, () =>
            {
                CpuWriteBE<ushort>(SYM["wPartyMon1Exp"] + 1, 8460);
                CpuWriteBE<ushort>("wPartyMon1HP", hp);
                Press(Joypad.A);
                ClearText();
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("HORN ATTACK"), (qa ? new RbyTurn("QUICK ATTACK") : null));
                TeachLevelUpMove("WATER GUN");
                ForceTurn(new RbyTurn("HORN ATTACK"));
            });
        }
        Console.WriteLine("Normal Exp");
        for(ushort hp = 10; hp <= 25; ++hp)
        {
            Console.Write("HP=" + hp + " ");
            NormalExp(hp);
        }
        Console.WriteLine("Moon Exp");
        for(ushort hp = 10; hp <= 25; ++hp)
        {
            Console.Write("HP=" + hp + " ");
            MoonExp(hp);
        }
        MoonExp(16, false);
    }
    void SecondOddishHA()
    {
        RbyTurn.DefaultRoll = 10;
        void Stall(ushort hp)
        {
            Comparison.Compare("basesaves/red/oddishha.gqs", () =>
            {
                CpuWriteBE<ushort>("wPartyMon1HP", (ushort) (hp + 5));
                Press(Joypad.A);
                ClearText();
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("QUICK ATTACK"));
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("ABSORB"));
                ForceTurn(new RbyTurn("POISON STING"));
                TeachLevelUpMove("WATER GUN");
            }, () =>
            {
                CpuWriteBE<ushort>("wPartyMon1HP", (ushort) (hp + 5));
                Press(Joypad.A);
                ClearText();
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("QUICK ATTACK"));
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                TeachLevelUpMove("WATER GUN");
            });
        }
        for(ushort hp = 10; hp <= 25; ++hp)
        {
            Console.Write("\nHP=" + hp + " ");
            Stall(hp);
        }
    }
    void BridgeRivalWalk()
    {
        for(int wait = 0; wait < 20; ++wait)
            Comparison.Compare("basesaves/red/bridgerivalwalk.gqs", () =>
            {
                AdvanceFrames(wait);
                MoveTo("CeruleanCity", 20, 6, Action.Up);
                ClearText();
                ForceTurn(new RbyTurn("HORN ATTACK", Crit), new RbyTurn("SAND-ATTACK", Miss));
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("HORN ATTACK"));
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("MEGA PUNCH", Crit));
                TalkTo("Route24", 11, 31);
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                MoveTo(10, 29);
                // MoveTo("Route24", 11, 32);
                // MoveTo(10, 32);
            }, () =>
            {
                AdvanceFrames(wait);
                MoveTo("CeruleanCity", 21, 6, Action.Up);
                ClearText();
                ForceTurn(new RbyTurn("HORN ATTACK", Crit), new RbyTurn("SAND-ATTACK", Miss));
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("HORN ATTACK"));
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("MEGA PUNCH", Crit));
                TalkTo("Route24", 11, 31);
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                ForceTurn(new RbyTurn("MEGA PUNCH"));
                MoveTo(10, 29);
                // MoveTo("Route24", 11, 32);
                // MoveTo(10, 32);
            });
    }
    void LanceMiss()
    {
        // miss drago 1
        Comparison.Compare("basesaves/red/lancemiss.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"), null, true, false);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"), null, true, false);
        });
        // miss dragonite
        Comparison.Compare("basesaves/red/lancemiss.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("BARRIER"));
            ForceTurn(new RbyTurn("BLIZZARD"), null, true, false);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"), null, true, false);
        });
        // x spec vs x speed
        // no extra miss
        Comparison.Compare("basesaves/red/lancemiss.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("BLIZZARD"), null, true, false);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), null, true, false);
        });
        // miss 1
        Comparison.Compare("basesaves/red/lancemiss.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("BLIZZARD"), null, true, false);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));
            // ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), null, true, false);
        });
        // miss 2
        Comparison.Compare("basesaves/red/lancemiss.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"), true, false);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));

            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD", Miss), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"), true, false);
        });
    }
    void EarlyElixer()
    {
        bool range = true;
        Comparison.Compare("basesaves/red/earlyelixer.gqs", () =>
        {
            LoadState("basesaves/red/blainesuper.gqs");
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));

            LoadState("basesaves/red/earlyelixer.gqs");
            UseItem("ELIXER", "NIDOKING");
            UseItem("BICYCLE");

            // RHYHORN
            MoveTo("ViridianGym", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            // BLACKBELT
            MoveTo(10, 4);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo("ViridianCity", 32, 8);
            MoveTo("ViridianGym", 16, 16);

            // GIOVANNI
            TalkTo("ViridianGym", 2, 1);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            if(range)
            {
                ForceTurn(new RbyTurn("BLIZZARD"));
            }
            else
            {
                ForceTurn(new RbyTurn("BLIZZARD", 1), new RbyTurn("FISSURE"));
                ForceTurn(new RbyTurn("EARTHQUAKE"));
            }

            MoveTo("ViridianCity", 32, 8);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            // VIRIDIAN RIVAL
            MoveTo("Route22", 29, 5);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            LoadState("basesaves/red/blainesuper.gqs");
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));

            LoadState("basesaves/red/earlyelixer.gqs");
            UseItem("BICYCLE");

            // RHYHORN
            MoveTo("ViridianGym", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            // BLACKBELT
            MoveTo(10, 4);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo("ViridianCity", 32, 8);
            MoveTo("ViridianGym", 16, 16);
            UseItem("ELIXER", "NIDOKING");

            // GIOVANNI
            TalkTo("ViridianGym", 2, 1);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            if(range)
            {
                ForceTurn(new RbyTurn("BLIZZARD"));
            }
            else
            {
                ForceTurn(new RbyTurn("BLIZZARD", 1), new RbyTurn("FISSURE"));
                ForceTurn(new RbyTurn("BLIZZARD", 1));
            }

            MoveTo("ViridianCity", 32, 8);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            // VIRIDIAN RIVAL
            MoveTo("Route22", 29, 5);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
    }
    void LanceElixer()
    {
        Comparison.Compare("basesaves/red/lanceelixer.gqs", () =>
        {
            Execute("U");
            MoveTo(5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("ELIXER", "NIDOKING"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        }, () =>
        {
            Execute("U");
            UseItem("ELIXER", "NIDOKING");
            MoveTo(5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        });
        Comparison.Compare("basesaves/red/lanceelixer.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 20);
            Execute("U U");
            MoveTo(5, 1);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT", Crit));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("ELIXER", "NIDOKING"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 20);
            Execute("U U");
            UseItem("ELIXER", "NIDOKING");
            MoveTo(5, 1);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT", Crit));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        });
    }
    void HealPadSkip()
    {
        Scenario Flute = () =>
        {
            MoveTo("PokemonTower6F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            TalkTo("PokemonTower6F", 9, 5);
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(6, 8);

            MoveTo(10, 15);
            UseItem("TM07", "NIDOKING", "ROCK SLIDE");

            MoveTo(10, 16);
            ClearText();
            ItemSwap("HM01", "SUPER REPEL");
            UseItem("POKE DOLL");

            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo(10, 7);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // Get Pokéflute
            TalkTo(10, 3);
            MoveTo(2, 1);
            Press(Joypad.Right);
            ReceiveItemAndSplit();
        };
        Scenario Silph = () =>
        {
            ClearText();
            MoveTo("LavenderTown", 7, 10);
            Fly("CeladonCity");
            TalkTo("CeladonPokecenter", 3, 2);
            Yes();
            ClearText(); // healed at center

            MoveTo("CeladonCity", 41, 10);
            UseItem("BICYCLE");

            MoveTo("Route7Gate", 3, 4);
            ClearText();
            MoveTo("Route7", 18, 10);
            UseItem("BICYCLE");

            PickupItemAt("SilphCo5F", 12, 3);

            // ARBOK TRAINER
            TalkTo(8, 16);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            PickupItemAt(21, 16);
            TalkTo(7, 13);
            TalkTo("SilphCo3F", 17, 9);

            // SILPH RIVAL
            MoveTo("SilphCo7F", 3, 2, Action.Left);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo(5, 7, Action.Right);
            UseItem("ELIXER", "NIDOKING");

            // SILPH ROCKET
            TalkTo("SilphCo11F", 3, 16);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            // SILPH GIOVANNI
            TalkTo(6, 13, Action.Up);
            MoveTo(6, 13);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        };
        Scenario Safari = () =>
        {
            ClearText();
            MoveTo(6, 14); // blocked by door?

            TalkTo(236, 3, 0);
            ChooseListItem(9);
            MoveTo(2, 3);
            Execute("D"); // exit elevator

            // Get candy and EQ
            MoveTo(3, 9);
            MoveTo(2, 9); // thinks trainer is still there
            PickupItemAt(234, 2, 12);
            PickupItemAt(234, 4, 14);
            Dig();

            UseItem("BICYCLE");

            // Snorlax menu
            MoveTo("Route16", 27, 10);
            UseItem("REPEL");
            ItemSwap("PARLYZ HEAL", "RARE CANDY");
            UseItem("POKE FLUTE");
            RunAway();

            PickupItemAt("Route17", 15, 14); // candy

            // Post cycling menu
            MoveTo("Route18", 40, 8);
            UseItem("REPEL");
            ItemSwap("POTION", "X SPECIAL");
            UseItem("TM26", "NIDOKING", "THRASH");
            UseItem("BICYCLE");

            CutAt("FuchsiaCity", 18, 19);
            CutAt(16, 11);
            MoveTo("SafariZoneGate", 3, 2);
            ClearText();
            Yes();
            ClearText();
            ClearText(); // sneaky joypad call

            UseItem("BICYCLE");

            MoveTo(217, 4, 23);
            UseItem("SUPER REPEL");
            CloseMenu(Joypad.Up); // direction close

            MoveTo(218, 7, 13);
            PickupItemAt("SafariZoneWest", 19, 7, Action.Down); // gold teeth

            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();
        };
        Scenario Koga = () =>
        {
            Fly("FuchsiaCity");
            UseItem("BICYCLE");

            // JUGGLER 1
            TalkTo("FuchsiaGym", 7, 8);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            // JUGGLER 2
            MoveTo(1, 7);
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("POISON GAS"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            // KOGA
            TalkTo(4, 10);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("ELIXER", "NIDOKING"), new RbyTurn("SELFDESTRUCT"));
        };
        Comparison.Compare("basesaves/red/healpadskip.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 10);
            MoveTo("PokemonTower5F", 15, 7);
            ClearText();
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            Flute();

            Silph();
            Safari();
            Koga();
        }, () =>
        {
            MoveTo("PokemonTower5F", 11, 9);
            ClearText(); // heal pad

            Flute();

            Silph();
            Safari();
            Koga();
        });
    }
    void WrapIT()
    {
        Comparison.Compare("basesaves/red/bridge3wrapnoit.gqs", () =>
        {
            // Comparison.Compare("basesaves/red/bridge3wrapit.gqs", () => {
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("WRAP", 20 | 5 * Turns));
            ForceTurn(new RbyTurn());
            ForceTurn(new RbyTurn());
            ForceTurn(new RbyTurn());
            ForceTurn(new RbyTurn());
            ForceTurn(new RbyTurn("MEGA PUNCH"));
        }, () =>
        {
            ForceTurn(new RbyTurn("MEGA PUNCH"));
        });
    }
    void Agatha()
    {
        void Lance()
        {
            MoveTo("LancesRoom", 5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", 1));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
        }
        void Champ()
        {
            Execute("U U");
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }
        void YoloDelayCandy()
        {
            ClearText();
            Execute("U U U");

            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("TOXIC"));

            Execute("U U U");
            MoveTo("LancesRoom", 6, 7);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");

            Lance();
            UseItem("RARE CANDY", "NIDOKING");
            Champ();
        }
        void YoloCandy()
        {
            ClearText();
            Execute("U U U");
            UseItem("RARE CANDY", "NIDOKING");

            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("TOXIC"));

            Execute("U U U");
            MoveTo("LancesRoom", 6, 7);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");

            Lance();
            Champ();
        }
        void NightShade()
        {
            ClearText();
            Execute("U U U");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");

            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("NIGHT SHADE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            Execute("U U U");
            MoveTo("LancesRoom", 6, 7);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");

            Lance();
            Champ();
        }
        void DreamEater()
        {
            ClearText();
            Execute("U U U");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");

            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            Execute("U U U");
            MoveTo("LancesRoom", 6, 7);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");

            Lance();
            Champ();
        }
        void Hypnosis()
        {
            ClearText();
            Execute("U U U");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");

            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYPNOSIS"));
            ForceTurn(new RbyTurn("POKE FLUTE"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            Execute("U U U");
            MoveTo("LancesRoom", 6, 7);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");

            Lance();
            Champ();
        }
        Comparison.Compare("agatha", "basesaves/red/agathamenu.gqs", YoloDelayCandy, YoloCandy, NightShade, Hypnosis, DreamEater);
    }
    void SoftReset()
    {
        Comparison.Compare("softreset", "basesaves/red/blackbelt.gqs", () =>
        {
            AdvanceFrames(6);
            BattleMenu(0, 1);
            ChooseListItem(1);
            ClearText(1);
            AdvanceFrames(170);
            RbyStrat.GfReset.Execute(this);
            RbyStrat.GfSkip.Execute(this);
            RbyStrat.Hop0.Execute(this);
            RbyStrat.Title0.Execute(this);
            RbyStrat.Continue.Execute(this);
            RbyStrat.Continue.Execute(this);
        }, () =>
        {
            AdvanceFrames(6);
            BattleMenu(0, 1);
            ChooseListItem(1);
            ClearText(1);
            AdvanceFrames(170);
            NoPal.Execute(this, true);
        });
    }
    void SafariEntrance()
    {
        Comparison.Compare("basesaves/red/safarientrance.gqs", () =>
        {
            MoveTo("SafariZoneGate", 4, 2, Action.Up);
            ClearText();
            Yes();
            ClearText();
            ClearText();
            UseItem("BICYCLE");
        }, () =>
        {
            MoveTo("SafariZoneGate", 3, 2);
            ClearText();
            Yes();
            ClearText();
            ClearText();
            UseItem("BICYCLE");
        });
    }
    void VenonatThrash()
    {
        Comparison.Compare("basesaves/red/venonatthrash.gqs",  () => {
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("DISABLE", Miss));
            ForceTurnAndSplit(new RbyTurn("THRASH"));
        }, () => {
            ClearText();
            ForceTurn(new RbyTurn("THRASH", ThreeTurn));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("DISABLE", Miss));
            ForceTurnAndSplit(new RbyTurn("BUBBLEBEAM"));
        }, () => {
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("DISABLE", Miss));
            ForceTurnAndSplit(new RbyTurn("THRASH"));
        });
    }
    void TowerRocket3()
    {
        Comparison.Compare("basesaves/red/towerrocket3.gqs",  () => {
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("TACKLE", Miss));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurnAndSplit(new RbyTurn("THRASH"));
        }, () => {
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH", ThreeTurn));
            ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("TACKLE", Miss));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurnAndSplit(new RbyTurn("THUNDERBOLT"));
        }, () => {
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("TACKLE", Miss));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurnAndSplit(new RbyTurn("THRASH"));
        });
    }
    void PotionInFight()
    {
        Comparison.Compare("basesaves/red/potioninfight.gqs", () =>
        {
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("POTION", "NIDORANM"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("TACKLE"));
            MoveTo(23, 4, Action.Up);
        }, () =>
        {
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("TACKLE"));
            UseItem("POTION", "NIDORANM");
            MoveTo(23, 4, Action.Up);
        });
    }
    void SurgeSelfHit()
    {
        RbyTurn.DefaultRoll = 20;
        void Route9()
        {
            ushort hp = CpuReadBE<ushort>("wPartyMon1HP");
            LoadState("basesaves/red/route9.gqs");
            CpuWriteBE<ushort>("wPartyMon1HP", hp);
            // ClearText();
            // CutAt("VermilionCity", 15, 18);
            // TalkTo("PokemonFanClub", 3, 1);
            // Yes();
            // ClearText();
            // Dig();
            // ClearText();

            // TalkTo("BikeShop", 6, 3);

            // // Bike menu
            // MoveTo("CeruleanCity", 13, 26);
            // ItemSwap("POTION", "BICYCLE");
            // UseItem("TM24", "NIDOKING", "HORN ATTACK");
            // UseItem("BICYCLE");

            // CutAt(19, 28);
            // CutAt("Route9", 5, 8);

            // 4 TURN THRASH
            TalkTo(13, 10);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // BUG CATCHER
            TalkTo(40, 8);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(21, 3, 10);
            MoveTo("RockTunnel1F", 15, 4);
            UseItem("REPEL");
        }
        Comparison.Compare("surgeselfhit", "basesaves/red/surge.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 26);
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("SONICBOOM", Miss));
            ForceTurn(new RbyTurn("THRASH", ThreeTurn));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("GROWL", Miss));
            ForceTurn(new RbyTurn("THRASH", Hitself), new RbyTurn("THUNDERBOLT"));
            BattleSwitch("PIDGEY", new RbyTurn("THUNDERBOLT"));
            SendOut("NIDOKING");
            ForceTurnAndSplit(new RbyTurn("THRASH"));

            Route9();

            TalkTo("RockTunnel1F", 23, 8);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 26);
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("SONICBOOM", Miss));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("GROWL", Miss));
            ForceTurnAndSplit(new RbyTurn("THRASH"));

            Route9();

            TalkTo("RockTunnel1F", 23, 8);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("CONFUSION"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            // ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("BONE CLUB"));
            // ForceTurn(new RbyTurn("BUBBLEBEAM"));
            // ForceTurn(new RbyTurn("THUNDERBOLT"));
        });

        LoadState("basesaves/red/surge_redbar.gqs");
        ClearText();
        ForceTurn(new RbyTurn("THRASH", ThreeTurn));
        ForceTurn(new RbyTurn("THRASH"));
        byte[] state = SaveState();

        Comparison.Compare("surgeyolo", state, () =>
        {
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("GROWL", Miss));
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("GROWL", Miss));
            BattleSwitch("PIDGEY", new RbyTurn("THUNDERBOLT"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        });
    }
    void TM07()
    {
        Comparison.Compare("basesaves/red/tm07buy.gqs", () =>
        {
            TalkTo(7, 3);
            Buy("TM07", 1);
            MoveTo(5, 5);
        }, () =>
        {
            MoveTo(5, 5);
        });
        Comparison.Compare("basesaves/red/tm07pick.gqs", () =>
        {
            MoveTo(4, 11, Action.Right);
            Execute("L");
            PickupItemAt(6, 12);
            MoveTo(10, 12);
        }, () =>
        {
            MoveTo(4, 11, Action.Right);
            Execute("R");
            MoveTo(10, 12);
        });
        Comparison.Compare("tm07", "basesaves/red/tm07buy.gqs", () =>
        {
            TalkTo(7, 3);
            Buy("TM07", 1);
            MoveTo(5, 5);
            LoadState("basesaves/red/tm07pick.gqs");
            MoveTo(4, 11, Action.Right);
            Execute("R");
            MoveTo(10, 12);
        }, () =>
        {
            MoveTo(5, 5);
            LoadState("basesaves/red/tm07pick.gqs");
            MoveTo(4, 11, Action.Right);
            Execute("L");
            PickupItemAt(6, 12);
            MoveTo(10, 12);
        });
    }
    void LiftKey()
    {
        Comparison.Compare("basesaves/red/liftkey.gqs", () =>
        {
            TalkTo(11, 2);
            ForceTurn(new RbyTurn("THRASH", Crit));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo(11, 2);
            PickupItemAt(10, 2);
            MoveTo(14, 3);
        }, () =>
        {
            MoveTo(11, 3);
            ClearText();
            ForceTurn(new RbyTurn("THRASH", Crit));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo(11, 2);
            PickupItemAt(10, 2);
            MoveTo(14, 3);
        });
        Comparison.Compare("basesaves/red/liftkeyrocket.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM", 1), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("ICE BEAM"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM", 1), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("THUNDERBOLT"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM", 1), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurnAndSplit(new RbyTurn("THRASH"));
        });
    }
    void FallDownTurnFrame()
    {
        Comparison.Compare("basesaves/red/falldown.gqs", () =>
        {
            ClearText();
            ActivateMansionSwitch();
            MoveTo(16, 14);
            FallDown();
            MoveTo(16, 16);
            MoveTo(15, 16);
            OpenStartMenu();
            MoveTo(13, 18);
            // MoveTo(21, 23);
            // OpenStartMenu();
            // MoveTo(20, 15);
        }, () =>
        {
            ClearText();
            ActivateMansionSwitch();
            MoveTo(16, 14);
            FallDown();
            MoveTo(16, 15);
            OpenStartMenu();
            MoveTo(16, 16);
            MoveTo(15, 16);
            MoveTo(13, 18);
            // MoveTo(21, 23);
            // MoveTo(20, 15);
        });
    }
    void TopGuy()
    {
        Comparison.Compare("basesaves/red/topguy.gqs", () =>
        {
            ClearText();
            MoveTo(24, 5, Action.Right);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            MoveTo(25, 6);
        }, () =>
        {
            ClearText();
            MoveTo(24, 6, Action.Right);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            MoveTo(25, 6);
        });
    }
    void MoonLag()
    {
        void Pick()
        {
            Inject(Joypad.A);
            Hold(Joypad.A, SYM["PlaySound"]);
        }
        Comparison.Compare("basesaves/red/moonlag1.gqs", () =>
        {
            MoveTo(21, 22);
            MoveTo(24, 26); // 26 or 27
            MoveTo(24, 31, Action.Left);
        }, () =>
        {
            MoveTo(5, 26);
            MoveTo(20, 15);
            MoveTo(25, 16);
            MoveTo(24, 31, Action.Left);
        });
        Comparison.Compare("basesaves/red/moonlag2.gqs", () =>
        {
            ClearText();
            MoveTo(30, 31);
            PickupItemAt(35, 31);
            MoveTo(35, 31);
            MoveTo(36, 23, Action.Right);
            Pick();
        }, () =>
        {
            ClearText();
            MoveTo(30, 31);
            PickupItemAt(35, 31);
            MoveTo(36, 23, Action.Right);
            Pick();
        });
        Comparison.Compare("moonlag2full", "basesaves/red/moonlag2b.gqs", () =>
        {
            AdvanceFrames(116, Joypad.Right);
            AdvanceFrames(140, Joypad.Up);
            AdvanceFrames(2, Joypad.Right);
            AdvanceFrames(2, Joypad.A);
            Pick();
        }, () =>
        {
            AdvanceFrames(116, Joypad.Right);
            AdvanceFrames(140, Joypad.Up);
            AdvanceFrames(4, Joypad.Right);
            AdvanceFrames(2, Joypad.A);
            Pick();
        }, () =>
        {
            AdvanceFrames(116, Joypad.Right);
            AdvanceFrames(140, Joypad.Up);
            AdvanceFrames(6, Joypad.Right);
            AdvanceFrames(2, Joypad.A);
            Pick();
        }, () =>
        {
            AdvanceFrames(116, Joypad.Right);
            AdvanceFrames(140, Joypad.Up);
            AdvanceFrames(8, Joypad.Right);
            AdvanceFrames(2, Joypad.A);
            Pick();
        }, () =>
        {
            AdvanceFrames(240, Joypad.Up);
            AdvanceFrames(15, Joypad.Right);
            AdvanceFrames(10, Joypad.A);
            Pick();
        });
        Comparison.Compare("basesaves/red/moonlag3.gqs", () =>
        {
            MoveTo(35, 22);
            MoveTo(30, 11);
            MoveTo(30, 7);
        }, () =>
        {
            MoveTo(35, 22);
            MoveTo(34, 7);
            MoveTo(30, 7);
        });
        Comparison.Compare("basesaves/red/moonlag4.gqs", () =>
        {
            // MoveTo(11, 6);
            MoveTo(11, 11);
            MoveTo(10, 11);
            MoveTo(10, 6);
            MoveTo(6, 6);
        }, () =>
        {
            MoveTo(8, 6);
            MoveTo(6, 6);
        });
        Comparison.Compare("moonlag4b", "basesaves/red/moonlag4.gqs", () =>
        {
            MoveTo(11, 2);
            MoveTo(3, 2);
            Pick();
        }, () =>
        {
            MoveTo(8, 2);
            MoveTo(3, 2);
            Pick();
        });
        Comparison.Compare("basesaves/red/moonlag5.gqs", () =>
        {
            MoveTo(3, 5);
            MoveTo(5, 5);
            // MoveTo(5, 17);
            MoveTo(5, 16);
            MoveTo(21, 16);
            MoveTo(21, 17);
        }, () =>
        {
            MoveTo(5, 2);
            MoveTo(5, 5);
            MoveTo(7, 5);
            // MoveTo(7, 17);
            MoveTo(7, 16);
            MoveTo(21, 16);
            MoveTo(21, 17);
        });
        Comparison.Compare("basesaves/red/moonlag6.gqs", () =>
        {
            AfterMoveAndSplit();
            MoveTo(21, 14);
            MoveTo(27, 14);
            MoveTo(32, 14);
            MoveTo(32, 22);
            MoveTo(11, 21);
            MoveTo(10, 17);
            Execute("R");
        }, () =>
        {
            AfterMoveAndSplit();
            MoveTo(23, 17);
            MoveTo(26, 16);
            MoveTo(33, 16);
            MoveTo(36, 24);
            MoveTo(34, 31);
            MoveTo(10, 31);
            MoveTo(10, 17);
            Execute("R");
        });
    }
    void SafariOneRepel()
    {
        void OneRepel(int enc = 0)
        {
            ClearText();
            ItemSwap("PARLYZ HEAL", "TM26");
            UseItem("PP UP", "NIDOKING", "HORN DRILL");
            UseItem("TM26", "NIDOKING", "THRASH");
            UseItem("BICYCLE");

            CutAt("FuchsiaCity", 18, 19);
            CutAt(16, 11);
            MoveTo("SafariZoneGate", 3, 2);
            ClearText();
            Yes();
            ClearText();
            ClearText();

            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            MoveTo(217, 0, 24);
            MoveTo(218, 39, 30);

            MoveTo(218, 11, 1);
            MoveTo(218, 5, 2, Action.Down);
            if(enc >= 1) { ForceEncounter(Action.Down, 9, 0); RunAway(); }
            MoveTo(218, 5, 7);
            if(enc == 2) { ForceEncounter(Action.Down, 9, 0); RunAway(); }
            MoveTo(218, 5, 13);
            MoveTo(218, 3, 14);
        }
        Scenario NormalRepeling = () =>
        {
            ClearText();
            UseItem("SUPER REPEL");
            ItemSwap("PARLYZ HEAL", "TM26");
            UseItem("PP UP", "NIDOKING", "HORN DRILL");
            UseItem("TM26", "NIDOKING", "THRASH");
            UseItem("BICYCLE");

            CutAt("FuchsiaCity", 18, 19);
            CutAt(16, 11);
            MoveTo("SafariZoneGate", 3, 2);
            ClearText();
            Yes();
            ClearText();
            ClearText();

            UseItem("BICYCLE");

            MoveTo(217, 4, 23);
            UseItem("SUPER REPEL");
            CloseMenu(Joypad.Up);

            MoveTo(218, 7, 13);
            MoveTo(218, 3, 14);
        };
        Scenario BadOneRepel = () =>
        {
            ClearText();
            ItemSwap("PARLYZ HEAL", "TM26");
            UseItem("PP UP", "NIDOKING", "HORN DRILL");
            UseItem("TM26", "NIDOKING", "THRASH");
            UseItem("BICYCLE");

            CutAt("FuchsiaCity", 18, 19);
            CutAt(16, 11);
            MoveTo("SafariZoneGate", 3, 2);
            ClearText();
            Yes();
            ClearText();
            ClearText();

            UseItem("BICYCLE");
            MoveTo(20, 14);
            MoveTo(23, 14);
            MoveTo(23, 11);

            MoveTo(217, 4, 23);
            UseItem("SUPER REPEL");
            CloseMenu(Joypad.Up);

            MoveTo(218, 7, 13);
            MoveTo(218, 3, 14);
        };

        Comparison.Compare("basesaves/red/safarionerepel.gqs", ()=>
        {
            OneRepel(0);
        }, () =>
        {
            OneRepel(1);
        }, () =>
        {
            OneRepel(2);
        }, () =>
        {
            NormalRepeling();
        });
    }
    void BlackBeltRevive()
    {
        void ToNidoking()
        {
            MoveTo("ViridianCity", 32, 8);
            MoveTo("ViridianGym", 16, 16);
            UseItem("ELIXER", "NIDOKING");

            // GIOVANNI
            TalkTo("ViridianGym", 2, 1);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
        }
        void ToLorelei()
        {
            ForceTurnAndSplit(new RbyTurn("BLIZZARD", 30));

            ClearText();
            MoveTo("ViridianCity", 32, 8);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            // VIRIDIAN RIVAL
            MoveTo("Route22", 29, 5);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo("Route22Gate", 4, 2, Action.Up);
            ClearText();
            MoveTo("Route23", 7, 139);
            UseItem("BICYCLE");
            MoveTo(7, 136, Action.Up);
            ClearText();
            MoveTo(9, 119, Action.Up);
            ClearText();
            MoveTo(10, 105, Action.Up);
            ClearText();
            MoveTo(10, 104, Action.Up);
            Surf();
            MoveTo(10, 96, Action.Up);
            ClearText();

            MoveTo(7, 85, Action.Up);
            ClearText();
            MoveTo(8, 71, Action.Up);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");
            MoveTo(12, 56, Action.Up);
            ClearText();
            MoveTo(5, 35, Action.Up);
            ClearText();

            MoveTo("VictoryRoad1F", 8, 16);
            Strength();
            MoveTo(5, 14);
            PushBoulder(Joypad.Down);
            Execute("L D D");
            PushBoulder(Joypad.Right, 4);
            Execute("D R R");
            PushBoulder(Joypad.Up, 2);
            Execute("L U U");
            PushBoulder(Joypad.Right, 7);
            Execute("D R R");
            PushBoulder(Joypad.Up, 2);
            Execute("L L U U R");
            PushBoulder(Joypad.Right);
            Execute("U R R");
            PushBoulder(Joypad.Down);
            MoveTo("VictoryRoad2F", 5, 14);

            UseItem("SUPER REPEL");
            Strength();

            PushBoulder(Joypad.Left);
            Execute("U L L");
            PushBoulder(Joypad.Down, 2);
            Execute("R D D");
            PushBoulder(Joypad.Left, 2);

            MoveTo("VictoryRoad3F", 23, 6);
            Strength();
            MoveTo(22, 4);
            PushBoulder(Joypad.Up, 2);
            Execute("R U U");
            PushBoulder(Joypad.Left, 16);
            Execute("U L L");
            PushBoulder(Joypad.Down);
            Execute("R D D");
            PushBoulder(Joypad.Left, 4);
            Execute("L U L");
            PushBoulder(Joypad.Down, 3);
            Execute("L D D");
            PushBoulder(Joypad.Right);
            Execute("U");

            MoveTo(21, 15, Action.Right);
            PushBoulder(Joypad.Right);
            Execute("R R");
            FallDown();

            // VR menu
            Strength();
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            Execute("D R R U");
            PushBoulder(Joypad.Left, 14);

            MoveTo("VictoryRoad2F", 29, 7);
            MoveAndSplit(Joypad.Right);
            AdvanceFrames(20);
            AfterMoveAndSplit();
            MoveTo("IndigoPlateauLobby", 8, 0);

            // LORELEI
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
        }
        void ToAgatha()
        {
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));

            ClearText();
            Execute("U U U");
            UseItem("ELIXER", "NIDOKING");

            // BRUNO
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
            ClearText();
            Execute("U U U");
        }
        Comparison.Compare("basesaves/red/blackbeltrevive.gqs", () =>
        {
            ClearText();
            MoveTo(10, 5);
            Save();
            MoveTo(10, 4);
            ClearText();
            AdvanceFrames(1);
            BattleMenu(0, 1);
            ChooseListItem(1);
            ClearText(1);
            AdvanceFrames(170);
            RbyStrat.GfReset.Execute(this);
            RbyStrat.GfSkip.Execute(this);
            RbyStrat.Hop0.Execute(this);
            RbyStrat.Title0.Execute(this);
            RbyStrat.Continue.Execute(this);
            RbyStrat.Continue.Execute(this);
            MoveTo(10, 4);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            MoveTo(10, 5);
            Save();
            MoveTo(10, 4);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
        Comparison.Compare("basesaves/red/blackbeltrevive.gqs", () =>
        {
            ClearText();
            MoveTo(10, 4);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("KARATE CHOP", Crit));
            SendOut("SQUIRTLE");
            ForceTurn(new RbyTurn("REVIVE", "NIDOKING"), new RbyTurn("LOW KICK", Crit));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            ToNidoking();
            // ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("THRASH", 20));
            // ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("THRASH", 20));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            ToLorelei();
            // ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AURORA BEAM", 20));
            BattleSwitch("PIDGEY", new RbyTurn("AURORA BEAM"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));

            ToAgatha();
            // UseItem("SUPER POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            MoveTo("AgathasRoom", 5, 2, Action.Right);
        }, () =>
        {
            ClearText();
            MoveTo(10, 5);
            Save();
            MoveTo(10, 4);
            ClearText();
            AdvanceFrames(1);
            BattleMenu(0, 1);
            ChooseListItem(1);
            ClearText(1);
            AdvanceFrames(170);
            RbyStrat.GfReset.Execute(this);
            RbyStrat.GfSkip.Execute(this);
            RbyStrat.Hop0.Execute(this);
            RbyStrat.Title0.Execute(this);
            RbyStrat.Continue.Execute(this);
            RbyStrat.Continue.Execute(this);
            MoveTo(10, 4);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            ToNidoking();
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            ToLorelei();
            BattleSwitch("PIDGEY", new RbyTurn("AURORA BEAM"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));

            ToAgatha();
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            MoveTo("AgathasRoom", 5, 2, Action.Right);
        });
    }
    void PostCycling()
    {
        Comparison.Compare("basesaves/red/postcycling.gqs", () =>
        {
            ClearText();
            UseItem("BICYCLE");
            MoveTo("FuchsiaCity", 18, 19);
            UseItem("REPEL");
            ItemSwap("POTION", "X SPECIAL");
            UseItem("TM26", "NIDOKING", "THRASH");
            Cut();

            // CutAt(18, 19);
            CutAt(16, 11);
            Execute("U");
        }, () =>
        {
            ClearText();
            UseItem("REPEL");
            ItemSwap("POTION", "X SPECIAL");
            UseItem("TM26", "NIDOKING", "THRASH");
            UseItem("BICYCLE");

            CutAt("FuchsiaCity", 18, 19);
            CutAt(16, 11);
            Execute("U");
        });
        Comparison.Compare("basesaves/red/postcycling.gqs", () =>
        {
            ClearText();
            UseItem("BICYCLE");

            CutAt("FuchsiaCity", 18, 19);
            CutAt(16, 11);
            MoveTo("SafariZoneGate", 3, 2);
            ClearText();
            Yes();
            ClearText();
            ClearText();

            UseItem("REPEL");
            ItemSwap("POTION", "X SPECIAL");
            UseItem("TM26", "NIDOKING", "THRASH");
            UseItem("BICYCLE");

            MoveTo(217, 4, 23);
            UseItem("SUPER REPEL");
            CloseMenu(Joypad.Up);

            MoveTo(218, 7, 13);
            PickupItemAt("SafariZoneWest", 19, 7, Action.Down);

            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();
        }, () =>
        {
            ClearText();
            UseItem("REPEL");
            ItemSwap("POTION", "X SPECIAL");
            UseItem("TM26", "NIDOKING", "THRASH");
            UseItem("BICYCLE");

            CutAt("FuchsiaCity", 18, 19);
            CutAt(16, 11);
            MoveTo("SafariZoneGate", 3, 2);
            ClearText();
            Yes();
            ClearText();
            ClearText();

            UseItem("BICYCLE");

            MoveTo(217, 4, 23);
            UseItem("SUPER REPEL");
            CloseMenu(Joypad.Up);

            MoveTo(218, 7, 13);
            PickupItemAt("SafariZoneWest", 19, 7, Action.Down);

            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();
        });
    }
    void YoloLance()
    {
        Comparison.Compare("yololance", "basesaves/red/lancemenu.gqs", () =>
        {
            CpuWrite("wPartyMon2PP", 1);
            CpuWriteBE<ushort>("wPartyMon2HP", 30);
            MoveTo(5, 1);
            ClearText();
            // ForceTurn(new RbyTurn("THUNDERBOLT", Crit));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("ELIXER", "NIDOKING"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
            // ClearText();
            // Execute("U U");
            // ClearText();
            // ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WING ATTACK"));
            // ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            // ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 30);
            MoveTo(6, 8);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            MoveTo(5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", 20));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
            // ClearText();
            // Execute("U U");
            // ClearText();
            // ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            // ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            // ForceTurn(new RbyTurn("HORN DRILL"));
        });
    }
    void CandiesMenu()
    {
        Comparison.Compare("basesaves/red/candiesmenu.gqs", () =>
        {
            RunUntil("JoypadOverworld");
            UseItem("BICYCLE");
            TalkTo("WardensHouse", 2, 3);
            MoveTo("FuchsiaCity", 27, 28);
            MoveNpc("PalletTown", 3, 8, Action.Up); // good npc
            Fly("PalletTown");

            // Surf menu
            MoveTo(4, 8, Action.Down);
            MoveTo(4, 17, Action.Right);
            UseItem("SUPER REPEL");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("HM03", "SQUIRTLE");
            Surf();

            MoveTo(32, 4, 0);
        }, () =>
        {
            RunUntil("JoypadOverworld");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("BICYCLE");

            TalkTo("WardensHouse", 2, 3);
            MoveTo("FuchsiaCity", 27, 28);
            MoveNpc("PalletTown", 3, 8, Action.Up); // good npc
            Fly("PalletTown");

            // Surf menu
            MoveTo(4, 8, Action.Down);
            MoveTo(4, 17, Action.Right);
            UseItem("SUPER REPEL");
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("HM03", "SQUIRTLE");
            Surf();

            MoveTo(32, 4, 0);
        });
    }
    void Gambler()
    {
        Comparison.Compare("basesaves/red/gambler.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM", 1), new RbyTurn("ROAR"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM", 1), new RbyTurn("ROAR"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
        });
        Comparison.Compare("gamblerbb", "basesaves/red/gambler.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
        });
    }
    void MansionCandy()
    {
        Comparison.Compare("basesaves/red/mansioncandy.gqs", () =>
        {
            PickupItemAt(10, 2);
            MoveTo(9, 6);
        }, () =>
        {
            MoveTo(9, 6);
        });
        Comparison.Compare("basesaves/red/mansioncandy.gqs", () =>
        {
            PickupItemAt(1, 9);
            MoveTo(5, 12);
        }, () =>
        {
            MoveTo(5, 12);
        });
    }
    void BrockRedbar()
    {
        Comparison.Compare("basesaves/red/brockredbar.gqs", () =>
        {
            CpuWriteBE<ushort>("wBattleMonHP", 12);
            ForceTurnAndSplit(new RbyTurn("BUBBLE"), new RbyTurn("TACKLE"));
        }, () =>
        {
            CpuWriteBE<ushort>("wBattleMonHP", 13);
            ForceTurnAndSplit(new RbyTurn("BUBBLE"), new RbyTurn("TACKLE"));
        });
    }
    void ChampLevelUp()
    {
        Comparison.Compare("basesaves/red/champlevelup.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1Exp", 633);
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1Exp", 655);
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });
    }
    void CeruleanMart()
    {
        Comparison.Compare("basesaves/red/ceruleanmart.gqs", () =>
        {
            TalkTo("CeruleanMart", 1, 5);
            Buy("REPEL", 7, "PARLYZ HEAL", 4);
            MoveTo(3, 14, 21);
            // MoveTo(71, 3, 6);//06:34.536
            // MoveTo(119, 2, 23);//06:34.430
            // MoveTo(119, 2, 24);//06:34.146
            MoveTo(119, 2, 25);//06:34.162
            // MoveTo(119, 2, 30);//06:34.215
            // MoveTo(119, 2, 40);//06:34.313
            // MoveTo(74, 4, 6);//06:34.319
            UseItem("REPEL");
            UseItem("TM11", "NIDOKING", "POISON STING");
            MoveTo("Route6", 17, 25);
            MoveTo(15, 28);

            // FEMALE JR. TRAINER
            TalkTo(11, 30, Action.Down);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // MALE JR. TRAINER
            MoveTo(10, 31);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo("VermilionCity", 18, 30);
            ClearText();

            // BOAT RIVAL
            MoveTo(95, 27, 4);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            // ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("TAIL WHIP"));
            // ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("SSAnneCaptainsRoom", 4, 2);

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText(); // watch cutscene

            // Cut menu
            MoveTo("VermilionCity", 15, 17, Action.Down);
            UseItem("HM01", "PARAS");
            UseItem("TM28", "PARAS");
            Cut();
        }, () =>
        {
            // ClearText(); return;
            MoveTo("Route6", 17, 25);
            MoveTo(15, 25);
            ForceEncounter(Action.Down, 9, 0x0000);
            RunAway();
            MoveTo(15, 28);

            // FEMALE JR. TRAINER
            TalkTo(11, 30, Action.Down);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // MALE JR. TRAINER
            MoveTo(10, 31);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo("VermilionCity", 18, 30);
            ClearText();

            // BOAT RIVAL
            MoveTo(95, 27, 4);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            // ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TAIL WHIP"));
            // ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("SSAnneCaptainsRoom", 4, 2);

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText(); // watch cutscene

            TalkTo("VermilionMart", 1, 5);
            Buy("REPEL", 6, "PARLYZ HEAL", 4);

            // Cut menu
            MoveTo("VermilionCity", 15, 17, Action.Down);
            UseItem("TM11", "NIDOKING", "POISON STING");
            UseItem("HM01", "PARAS");
            UseItem("TM28", "PARAS");
            Cut();
        });
        Comparison.Compare("basesaves/red/digrocketdeath.gqs", () =>
        {
            ClearText();
            MoveTo(3, 19, 17);
        }, () =>
        {
            ClearText();
            MoveTo(3, 27, 13);
            MoveTo(3, 19, 17);
        });
    }
    void GioRevive()
    {
        Comparison.Compare("basesaves/red/gioreviveup.gqs", () =>
        {
            PickupItemAt(16, 9);
            MoveTo(15, 5, Action.Up);
        }, () =>
        {
            MoveTo(15, 5, Action.Up);
        });
        Comparison.Compare("basesaves/red/giorevivedown.gqs", () =>
        {
            PickupItemAt(16, 9);
            Execute("D D R");
            MoveTo(1, 32, 8);
        }, () =>
        {
            PickupItemAt(16, 9);
            MoveTo(1, 32, 8);
        }, () =>
        {
            MoveTo(1, 32, 8);
        });
        Comparison.Compare("basesaves/red/giorevivedown2.gqs", () =>
        {
            MoveTo(16, 9);
            Execute("D R");
            MoveTo(1, 32, 8);
        }, () =>
        {
            MoveTo(1, 32, 8);
        });
    }
    void BlackbeltBlizz()
    {
        Comparison.Compare("blackbeltblizz", "basesaves/red/blackbelt.gqs", () =>
        {
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD", Crit));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });
    }
    void LanceDeath()
    {
        Comparison.Compare("lancedeath", "basesaves/red/lancemenu.gqs", () =>
        {
            MoveTo(5, 2);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            Save();
            MoveTo(5, 1);
            ClearText();
            CpuWrite("hRandomAdd", 220);
            BattleMenu(0, 1);
            ChooseListItem(2);
            ClearText(3);
            RbyStrat.GfReset.Execute(this);
            RbyStrat.GfSkip.Execute(this);
            RbyStrat.Hop0.Execute(this);
            RbyStrat.Title0.Execute(this);
            RbyStrat.Continue.Execute(this);
            RbyStrat.Continue.Execute(this);
            MoveTo(5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
        }, () =>
        {
            MoveTo(5, 2);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            Save();
            MoveTo(5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
        });
    }
    void LeftBoatRival()
    {
        Comparison.Compare("boatrival", "basesaves/red/boatrivalit.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 1);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK", Crit));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", Crit));
            Inject(Joypad.Up);
            Hold(Joypad.Up, "EnterMap");
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 1);
            MoveTo("SSAnne2F", 37, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK", Crit));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", Crit));
            MoveTo(36, 5);
            Inject(Joypad.Up);
            Hold(Joypad.Up, "EnterMap");
        });
    }
    void BlizzMenuPPUP()
    {
        Comparison.Compare("blizzmenuppup", "basesaves/red/blizzmenu3repel.gqs", "basesaves/red/blizzmenu6repel.gqs", () =>
        {
            PickupItemAt("PokemonMansionB1F", 19, 25);
            UseItem("HM04", "SQUIRTLE", "TACKLE");
            UseItem("TM14", "NIDOKING", "BUBBLEBEAM");
            UseItem("SUPER REPEL");
            UseItem("PP UP", "NIDOKING", "HORN DRILL");
            Execute("D");
        }, () =>
        {
            PickupItemAt("PokemonMansionB1F", 19, 25);
            UseItem("HM04", "SQUIRTLE", "TACKLE");
            UseItem("TM14", "NIDOKING", "BUBBLEBEAM");
            UseItem("REPEL");
            Execute("D");
        });
    }
    void JugglerXAccStall()
    {
        Comparison.Compare("jugglerxaccstall", "basesaves/red/jugglerxaccstall.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("POISON GAS"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            // ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn(AiItem));
            // ForceTurn(new RbyTurn("HORN DRILL"));
            // ForceTurn(new RbyTurn("EARTHQUAKE"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("POISON GAS"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            // ForceTurn(new RbyTurn("EARTHQUAKE", Crit));
        });
    }
    void Double1()
    {
        Comparison.Compare("basesaves/red/double1.gqs", () =>
        {
            MoveTo("ViridianCity", 32, 10);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");
            MoveTo("ViridianCity", 30, 10);
        }, () =>
        {
            MoveTo("ViridianCity", 32, 8);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");
            MoveTo("ViridianCity", 32, 10);
            MoveTo("ViridianCity", 30, 10);
        });
    }
    void LoreleiPPUPvsMaxEther()
    {
        void LoreleiSplit(bool maxether)
        {
            ClearText();
            MoveTo("ViridianCity", 32, 8);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            // VIRIDIAN RIVAL
            MoveTo("Route22", 29, 5);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            if(maxether)
            {
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
            }
            else
            {
                ForceTurn(new RbyTurn("THUNDERBOLT"));
                ForceTurn(new RbyTurn("EARTHQUAKE"));
                ForceTurn(new RbyTurn("EARTHQUAKE"));
            }
            ForceTurn(new RbyTurn("HORN DRILL"));

            MoveTo("Route22Gate", 4, 2, Action.Up);
            ClearText();
            MoveTo("Route23", 7, 139);
            UseItem("BICYCLE");
            MoveTo(7, 136, Action.Up);
            ClearText();
            MoveTo(9, 119, Action.Up);
            ClearText();
            MoveTo(10, 105, Action.Up);
            ClearText();
            MoveTo(10, 104, Action.Up);
            Surf();
            MoveTo(10, 96, Action.Up);
            ClearText();

            if(maxether)
            {
                // MoveTo(7, 90);
                // Press(Joypad.None, Joypad.Right, Joypad.None);
                // PickupItem(); // max ether
                PickupItemAt(7, 90, Action.Right);
            }

            MoveTo(7, 85, Action.Up);
            ClearText();
            MoveTo(8, 71, Action.Up);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");
            MoveTo(12, 56, Action.Up);
            ClearText();
            MoveTo(5, 35, Action.Up);
            ClearText();

            MoveTo("VictoryRoad1F", 8, 16);
            Strength();
            MoveTo(5, 14);
            PushBoulder(Joypad.Down);
            Execute("L D D");
            PushBoulder(Joypad.Right, 4);
            Execute("D R R");
            PushBoulder(Joypad.Up, 2);
            Execute("L U U");
            PushBoulder(Joypad.Right, 7);
            Execute("D R R");
            PushBoulder(Joypad.Up, 2);
            Execute("L L U U R");
            PushBoulder(Joypad.Right);
            Execute("U R R");
            PushBoulder(Joypad.Down);
            MoveTo("VictoryRoad2F", 5, 14);

            UseItem("SUPER REPEL");
            Strength();

            PushBoulder(Joypad.Left);
            Execute("U L L");
            PushBoulder(Joypad.Down, 2);
            Execute("R D D");
            PushBoulder(Joypad.Left, 2);

            MoveTo("VictoryRoad3F", 23, 6);
            Strength();
            MoveTo(22, 4);
            PushBoulder(Joypad.Up, 2);
            Execute("R U U");
            PushBoulder(Joypad.Left, 16);
            Execute("U L L");
            PushBoulder(Joypad.Down);
            Execute("R D D");
            PushBoulder(Joypad.Left, 4);
            Execute("L U L");
            PushBoulder(Joypad.Down, 3);
            Execute("L D D");
            PushBoulder(Joypad.Right);
            Execute("U");

            MoveTo(21, 15, Action.Right);
            PushBoulder(Joypad.Right);
            Execute("R R");
            FallDown();

            // VR menu
            Strength();
            if(maxether) UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            Execute("D R R U");
            PushBoulder(Joypad.Left, 14);

            MoveTo("VictoryRoad2F", 29, 7);

            // TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
            // Deposit("SQUIRTLE", "PARAS");
            MoveTo("IndigoPlateauLobby", 8, 0);
            // PartySwap("NIDOKING", "PIDGEY");

            // LORELEI
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
            BattleSwitch("PIDGEY", new RbyTurn("AURORA BEAM"));
            // ForceTurn(new RbyTurn("GUST"), new RbyTurn("AURORA BEAM"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }
        Comparison.Compare("loreleippupvsmaxether", "basesaves/red/loreleisplitppup.gqs", "basesaves/red/loreleisplitmaxether.gqs", () =>
        {
            LoreleiSplit(false);
        }, () =>
        {
            LoreleiSplit(true);
        });
    }
    void CryLength()
    {
        Comparison.Compare("basesaves/red/crylength.gqs", () =>
        {
            Inject(Joypad.Down);
            Hold(Joypad.Down, SYM["TryDoWildEncounter.gotWildEncounterType"] + 8);
            A = 165;
            RunUntil("ManualTextScroll");
        }, () =>
        {
            Inject(Joypad.Down);
            Hold(Joypad.Down, SYM["TryDoWildEncounter.gotWildEncounterType"] + 8);
            A = 132;
            RunUntil("ManualTextScroll");
        });
        Comparison.Compare("crylength", "basesaves/red/crylength2.gqs", () =>
        {
            CpuWrite("wEnemyMonSpecies2", Species["SNORLAX"].Id);
            RunUntil("ManualTextScroll");
        }, () =>
        {
            CpuWrite("wEnemyMonSpecies2", Species["PIDGEY"].Id);
            RunUntil("ManualTextScroll");
        });
    }
    void Route1Manip()
    {
        Comparison.Compare("route1manip", "basesaves/red/route1.gqs", () =>
        {
            ClearText();
            MoveTo(10, 0);
            SaveAndQuit();
            new RbyIntroSequence(RbyStrat.PalAB).Execute(this, true);
            Execute(SpacePath("UAUUUULLUUUUUUURRRRUUUULLLUURRUURARUURUUUUUUUUUUUULLLLUUUUUUUUUULUUUUUUUURRRRRRRRRRU"));
            ClearText();
            MoveTo("ViridianCity", 29, 20);
            Execute(SpacePath("DDDDDDDDDLLLLLLLLDDDDDD"));
            Execute(SpacePath("DDDDRRRDDDDDADDADDDLLDDDDDDDDDDDLLDDDDDDDD"));
            MoveTo(0, 10, 2);
        }, () =>
        {
            ClearText();
            MoveTo("Route1", 11, 24);
            MoveTo("Route1", 11, 14);

            MoveTo("Route1", 14, 10);
            ForceEncounter(Action.Up, 0, 0x0000);
            RunAway();
            MoveTo("Route1", 14, 7);
            ForceEncounter(Action.Up, 1, 0x0000);
            RunAway();

            MoveTo("ViridianCity", 21, 30);
            MoveTo("ViridianCity", 20, 20);
            MoveTo("ViridianCity", 29, 19);
            ClearText();
            MoveTo("Route1", 8, 6);
            MoveTo("Route1", 9, 14);
            MoveTo("Route1", 16, 22);
            MoveTo("Route1", 16, 24);
            MoveTo("Route1", 16, 26);
            MoveTo("Route1", 14, 26);
            MoveTo("Route1", 14, 28);
            MoveTo("Route1", 10, 28);
            MoveTo("Route1", 10, 35);
            MoveTo(0, 10, 2);
        });
    }
    void FlyMenuPg()
    {
        Comparison.Compare("flymenupg", "basesaves/red/flymenu_slot1.gqs", () =>
        {
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("HM02", "PIDGEY");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            Fly("LavenderTown");

            // LAVENDER RIVAL
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", Crit));

            // CHANNELER 1
            TalkTo("PokemonTower4F", 15, 7);
            MoveSwap("THRASH", "ROCK SLIDE");
            ForceTurn(new RbyTurn("ROCK SLIDE"));
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(12, 10); // elixer
            PickupItemAt("PokemonTower5F", 4, 12); // elixer

            MoveTo("PokemonTower5F", 11, 9);
            ClearText(); // heal pad

            // CHANNELER 2
            MoveTo("PokemonTower6F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            // CHANNELER 3
            TalkTo("PokemonTower6F", 9, 5);
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(6, 8); // rare candy

            MoveTo(10, 15);
            UseItem("TM07", "NIDOKING", "ROCK SLIDE");

            MoveTo(10, 16);
            SaveState("basesaves/red/ghostmenu_alt.gqs");
            ClearText();
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("POKE DOLL"); // escape ghost

            // ROCKET 1
            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            // ROCKET 2
            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("SMOG", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            // ROCKET 3
            MoveTo(10, 7);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // Get Pokéflute
            TalkTo(10, 3);
            TalkTo(3, 1);
            ClearText();

            MoveTo("LavenderTown", 7, 10);
            Fly("CeladonCity");
            TalkTo("CeladonPokecenter", 3, 2);
            Yes();
            ClearText(); // healed at center

            MoveTo("CeladonCity", 41, 10);
            UseItem("BICYCLE");

            MoveTo("Route7Gate", 3, 4);
            ClearText();
            MoveTo("Route7", 18, 10);
            UseItem("BICYCLE");

            PickupItemAt("SilphCo5F", 12, 3);

            // ARBOK ROCKET
            TalkTo(8, 16);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));

            PickupItemAt(21, 16);
            TalkTo(7, 13);
            TalkTo("SilphCo3F", 17, 9);

            // SILPH RIVAL
            MoveTo("SilphCo7F", 3, 2, Action.Left);
            SaveState("basesaves/red/silphrival_alt.gqs");
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ItemSwap("HM01", "SUPER REPEL");
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("HM02", "PIDGEY");
            Fly("LavenderTown");

            // LAVENDER RIVAL
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", Crit));

            // CHANNELER 1
            TalkTo("PokemonTower4F", 15, 7);
            MoveSwap("THRASH", "ROCK SLIDE");
            ForceTurn(new RbyTurn("ROCK SLIDE"));
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(12, 10); // elixer
            PickupItemAt("PokemonTower5F", 4, 12); // elixer

            MoveTo("PokemonTower5F", 11, 9);
            ClearText(); // heal pad

            // CHANNELER 2
            MoveTo("PokemonTower6F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            // CHANNELER 3
            TalkTo("PokemonTower6F", 9, 5);
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(6, 8); // rare candy

            MoveTo(10, 15);
            UseItem("TM07", "NIDOKING", "ROCK SLIDE");

            MoveTo(10, 16);
            SaveState("basesaves/red/ghostmenu.gqs");
            ClearText();
            ItemSwap("HM01", "SUPER REPEL");
            UseItem("POKE DOLL"); // escape ghost

            // ROCKET 1
            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            // ROCKET 2
            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("SMOG", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            // ROCKET 3
            MoveTo(10, 7);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // Get Pokéflute
            TalkTo(10, 3);
            TalkTo(3, 1);
            ClearText();

            MoveTo("LavenderTown", 7, 10);
            Fly("CeladonCity");
            TalkTo("CeladonPokecenter", 3, 2);
            Yes();
            ClearText(); // healed at center

            MoveTo("CeladonCity", 41, 10);
            UseItem("BICYCLE");

            MoveTo("Route7Gate", 3, 4);
            ClearText();
            MoveTo("Route7", 18, 10);
            UseItem("BICYCLE");

            PickupItemAt("SilphCo5F", 12, 3);

            // ARBOK ROCKET
            TalkTo(8, 16);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));

            PickupItemAt(21, 16);
            TalkTo(7, 13);
            TalkTo("SilphCo3F", 17, 9);

            // SILPH RIVAL
            MoveTo("SilphCo7F", 3, 2, Action.Left);
            SaveState("basesaves/red/silphrival.gqs");
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
        Comparison.Compare("flymenupg1", "basesaves/red/flymenu_slot1.gqs", () =>
        {
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("HM02", "PIDGEY");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            Fly("LavenderTown");
        }, () =>
        {
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("HM02", "PIDGEY");
            Fly("LavenderTown");
        });
        Comparison.Compare("flymenupg2", "basesaves/red/ghostmenu_alt.gqs", "basesaves/red/ghostmenu.gqs", () =>
        {
            ClearText();
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("POKE DOLL");
        }, () =>
        {
            ClearText();
            ItemSwap("HM01", "SUPER REPEL");
            UseItem("POKE DOLL");
        });
        Comparison.Compare("flymenupg3", "basesaves/red/silphrival_alt.gqs", "basesaves/red/silphrival.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ItemSwap("HM01", "SUPER REPEL");
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
        Comparison.Compare("flymenupgshort", "basesaves/red/flymenu_slot1.gqs", () =>
        {
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("HM02", "PIDGEY");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            Fly("LavenderTown");
            LoadState("basesaves/red/ghostmenu_alt.gqs");
            ClearText();
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("POKE DOLL");
            LoadState("basesaves/red/silphrival_alt.gqs");
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ItemSwap("HM01", "SUPER REPEL");
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("HM02", "PIDGEY");
            Fly("LavenderTown");
            LoadState("basesaves/red/ghostmenu.gqs");
            ClearText();
            ItemSwap("HM01", "SUPER REPEL");
            UseItem("POKE DOLL");
            LoadState("basesaves/red/silphrival.gqs");
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
    }
    void AgathaMenuClassic()
    {
        Comparison.Compare("basesaves/red/agathamenuclassic.gqs", () =>
        {
            ClearText();
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("ELIXER", "NIDOKING");
            // CloseMenu();
            Execute("U");
        }, () =>
        {
            ClearText();
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            // CloseMenu();
            Execute("U");
        });
    }
    void SilphCarbos()
    {
        Comparison.Compare("basesaves/red/silphcarbos.gqs", () =>
        {
            MoveTo(234, 4, 9);
            PickupItemAt(234, 5, 11); // carbos
            PickupItemAt(234, 2, 12); // tm26
            PickupItemAt(234, 4, 14); // candy
            Dig();
            UseItem("CARBOS", "NIDOKING");
            UseItem("BICYCLE");
            Execute("L");
        }, () =>
        {
            MoveTo(234, 4, 9);
            PickupItemAt(234, 2, 12); // tm26
            PickupItemAt(234, 4, 14); // candy
            Dig();
            UseItem("BICYCLE");
            Execute("L");
        });
    }
    void MansionCarbos()
    {
        Comparison.Compare("basesaves/red/mansioncarbos.gqs", () =>
        {
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("BICYCLE");

            TalkTo("WardensHouse", 2, 3);
            MoveTo("FuchsiaCity", 27, 28);
            MoveNpc("PalletTown", 3, 8, Action.Left); // good npc
            Fly("PalletTown");

            // Surf menu
            MoveTo(4, 13);
            UseItem("SUPER REPEL");
            UseItem("HM03", "SQUIRTLE");
            Surf();

            MoveTo("CinnabarIsland", 4, 4);

            TalkTo("PokemonMansion3F", 10, 5, Action.Up);
            ActivateMansionSwitch();
            MoveTo(16, 14);
            FallDown();

            PickupItemAt(18, 21); // carbos

            // HM04 menu
            MoveTo(21, 21);
            UseItem("HM04", "SQUIRTLE", "TACKLE");
            UseItem("CARBOS", "NIDOKING");
            UseItem("SUPER REPEL");
            ScrollTo(Bag.NumItems);
            // ListScroll(19, Joypad.B, false);
            // CurrentMenuType = MenuType.StartMenu;

            TalkTo("PokemonMansionB1F", 18, 25, Action.Up);
            ActivateMansionSwitch();

            TalkTo(20, 3, Action.Up);
            ActivateMansionSwitch();

            PickupItemAt(10, 2); // candy
            MoveTo(5, 9);
            UseItem("RARE CANDY", "NIDOKING");

            PickupItemAt(5, 13); // secret key
        }, () =>
        {
            UseItem("BICYCLE");

            TalkTo("WardensHouse", 2, 3);
            MoveTo("FuchsiaCity", 27, 28);
            MoveNpc("PalletTown", 3, 8, Action.Left); // good npc
            Fly("PalletTown");

            // Surf menu
            MoveTo(4, 13);
            UseItem("SUPER REPEL");
            UseItem("HM03", "SQUIRTLE");
            Surf();

            MoveTo("CinnabarIsland", 4, 4);

            // HM04 menu
            MoveTo("PokemonMansion3F", 6, 3);
            UseItem("HM04", "SQUIRTLE", "TACKLE");
            UseItem("SUPER REPEL");
            CloseMenu(Joypad.Down); // direction close

            TalkTo("PokemonMansion3F", 10, 5, Action.Up);
            ActivateMansionSwitch();
            MoveTo(16, 14);
            FallDown();

            TalkTo("PokemonMansionB1F", 18, 25, Action.Up);
            ActivateMansionSwitch();

            TalkTo(20, 3, Action.Up);
            ActivateMansionSwitch();

            PickupItemAt(10, 2); // candy
            MoveTo(5, 9);
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");

            PickupItemAt(5, 13); // secret key
        });
    }
    void SafariCarbos()
    {
        Comparison.Compare("basesaves/red/safaricarbos.gqs", () =>
        {
            AfterMoveAndSplit();
            UseItem("BICYCLE");

            PickupItemAt(217, 20, 13);

            MoveTo(218, 33, 31);
            UseItem("SUPER REPEL");
            UseItem("CARBOS", "NIDOKING");

            MoveTo(218, 7, 13);
            PickupItemAt("SafariZoneWest", 19, 7, Action.Down); // gold teeth

            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();
        }, () =>
        {
            AfterMoveAndSplit();
            UseItem("BICYCLE");

            // MoveTo(217, 4, 23);
            MoveTo(218, 22, 31);
            UseItem("SUPER REPEL");
            CloseMenu(Joypad.Up); // direction close

            MoveTo(218, 7, 13);
            PickupItemAt("SafariZoneWest", 19, 7, Action.Down); // gold teeth

            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();
        });
    }
    void ExtraCandy()
    {
        Comparison.Compare("basesaves/red/extracandy.gqs", () =>
        {
            PickupItemAt(1, 9); // extra candy
            MoveTo(5, 9);
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            PickupItemAt(5, 13); // secret key
        }, () =>
        {
            MoveTo(5, 9);
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            PickupItemAt(5, 13); // secret key
        });
    }
    void ElixerSkip()
    {
        Comparison.Compare("basesaves/red/towerelixer.gqs", () =>
        {
            ClearText();
            PickupItemAt(12, 10);
            MoveTo("PokemonTower5F", 5, 7);
        }, () =>
        {
            ClearText();
            PickupItemAt("PokemonTower5F", 4, 12);
            MoveTo("PokemonTower5F", 5, 7);
        }, () =>
        {
            ClearText();
            PickupItemAt(12, 10);
            PickupItemAt("PokemonTower5F", 4, 12);
            MoveTo("PokemonTower5F", 5, 7);
        });
        Comparison.Compare("basesaves/red/maxetherpickup.gqs", () =>
        {
            AfterMoveAndSplit();
            MoveTo(21, 15, 57);
        }, () =>
        {
            AfterMoveAndSplit();
            PickupItemAt(21, 16, 53);
            MoveTo(21, 15, 57);
        });
        Comparison.Compare("basesaves/red/silphmaxetherclassic.gqs", () =>
        {
            CpuWrite("wNumBagItems", (byte) (CpuRead("wNumBagItems") - 1));
            for(int slot = 6 * 2; slot < 19 * 2; ++slot)
                CpuWrite(SYM["wBagItems"] + slot, CpuRead(SYM["wBagItems"] + slot + 2));
            AfterMoveAndSplit();
            UseItem("ELIXER", "NIDOKING", "HORN DRILL");
            MoveTo(3, 3);
        }, () =>
        {
            AfterMoveAndSplit();
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            MoveTo(3, 3);
        });
        RbyTurn onix = new RbyTurn("ROCK THROW");
        Comparison.Compare("basesaves/red/brunoelixerskip.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            Execute("U U U");
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), onix);
            ForceTurn(new RbyTurn("X SPECIAL"), onix);
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            Execute("U U U");
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), onix);
            ForceTurn(new RbyTurn("X SPECIAL"), onix);
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            Execute("U U U");
            UseItem("ELIXER", "NIDOKING");
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), onix);
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });
    }
    void EarlyBB()
    {
        RbyTurn.DefaultRoll = 20;
        Comparison.Compare("basesaves/red/earlybb.gqs", () =>
        {
            MoveTo(95, 27, 2);
            UseItem("TM11", "NIDOKING", "POISON STING");

            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            // ForceTurn(new RbyTurn("HORN ATTACK", Crit));
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            // ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("SSAnneCaptainsRoom", 4, 2);

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText();

            MoveTo("VermilionCity", 15, 17, Action.Down);
            ItemSwap("POTION", "REPEL");
            UseItem("HM01", "PARAS");
            UseItem("TM28", "PARAS");
            Cut();
        }, () =>
        {
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            // ForceTurn(new RbyTurn("HORN ATTACK", Crit));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("POISON STING"));
            // ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("SSAnneCaptainsRoom", 4, 2);

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText();

            MoveTo("VermilionCity", 15, 17, Action.Down);
            UseItem("TM11", "NIDOKING", "POISON STING");
            ItemSwap("POTION", "REPEL");
            UseItem("HM01", "PARAS");
            UseItem("TM28", "PARAS");
            Cut();
        });
    }
    void BridgeHikerStrats()
    {
        RbyTurn.DefaultRoll = 20;
        Comparison.Compare("basesaves/red/bridgehikerstrats.gqs", () =>
        {
            CpuWriteBE<ushort>(SYM["wPartyMon1Exp"] + 1, 6350);
            ForceTurn(new RbyTurn("HORN ATTACK"));

            Execute("L");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            TeachLevelUpMove("TACKLE");

            TalkTo(10, 22);
            MoveSwap("HORN ATTACK", "THRASH");
            ForceTurn(new RbyTurn("THRASH"));
            // ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo(11, 19);
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(10, 15);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo("Route25", 14, 7);
            ClearText();
            ForceTurn(new RbyTurn("WATER GUN"));

            TalkTo(18, 8, Action.Down);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
        }, () =>
        {
            ForceTurn(new RbyTurn("HORN ATTACK"));

            TalkTo(10, 22);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            Execute("R");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            TeachLevelUpMove("TACKLE");

            TalkTo(11, 19);
            MoveSwap("HORN ATTACK", "THRASH");
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(10, 15);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("Route25", 8, 4);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("KARATE CHOP", Crit));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("DEFENSE CURL"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo(18, 8, Action.Down);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
        });
    }
    void MoonHiker()
    {
        Comparison.Compare("basesaves/red/moonhiker.gqs", () =>
        {
            MoveTo(11, 16);
            ForceEncounter(Action.Up, 2, 0);
            RunAway();
            MoveTo(11, 13);
            ForceEncounter(Action.Up, 2, 0);
            RunAway();
            MoveTo(11, 10);
            ForceEncounter(Action.Up, 2, 0);
            RunAway();
            MoveTo(11, 6);
            UseItem("TM12", "NIDORINO", "LEER");
            TalkTo(5, 6);
            ForceTurn(new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("WATER GUN"));
            PickupItemAt(2, 2, Action.Left);
        }, () =>
        {
            MoveTo(11, 16);
            ForceEncounter(Action.Up, 2, 0);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"));
            MoveTo(11, 13);
            ForceEncounter(Action.Up, 2, 0);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"));
            MoveTo(11, 10);
            ForceEncounter(Action.Up, 2, 0);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"));
            MoveTo(11, 2);
            PickupItemAt(2, 2);
        }, () =>
        {
            MoveTo(11, 16);
            ForceEncounter(Action.Up, 2, 0);
            RunAway();
            MoveTo(11, 13);
            ForceEncounter(Action.Up, 2, 0);
            RunAway();
            MoveTo(11, 10);
            ForceEncounter(Action.Up, 2, 0);
            RunAway();
            MoveTo(11, 2);
            PickupItemAt(2, 2);
        });
        Comparison.Compare("basesaves/red/moonwatergun.gqs", () =>
        {
            PickupItemAt(5, 32);
            MoveTo(14, 22);
        }, () =>
        {
            MoveTo(14, 22);
        });
    }
    void TopHiker()
    {
        Comparison.Compare("basesaves/red/tophiker.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 50);
            ClearText();
            ForceTurn(new RbyTurn("THRASH", ThreeTurn));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("DEFENSE CURL"));
            BattleSwitch("SQUIRTLE", new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("BUBBLE"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 50);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("KARATE CHOP", Crit));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("DEFENSE CURL"));
            ForceTurn(new RbyTurn("THRASH"));
        });
    }
    void ChampClassic()
    {
        Comparison.Compare("basesaves/red/champclassicpotion.gqs", () =>
        {
            ClearText();
            UseItem("POTION", "NIDOKING");
            Execute("U");
        }, () =>
        {
            ClearText();
            Execute("U");
        });
        Comparison.Compare("basesaves/red/champclassic.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 33);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 35);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 37);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 38);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("FULL RESTORE"), new RbyTurn("SKY ATTACK", 20));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("SUPER POTION"), new RbyTurn("SKY ATTACK", 20));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("REFLECT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("SKY ATTACK", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });
    }
    void LanceClassic()
    {
        Comparison.Compare("basesaves/red/lanceclassic.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            Execute("U U");
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", Miss));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            Execute("U U");
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP", Miss));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            Execute("U U");
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });
    }
    void AgathaClassic()
    {
        Comparison.Compare("basesaves/red/agathaclassic.gqs", () =>
        {
            ClearText();
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("ELIXER", "NIDOKING");
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM"), new RbyTurn("SUPERSONIC", Miss));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            UseItem("SUPER POTION", "NIDOKING");
            Execute("U");
        }, () =>
        {
            ClearText();
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM"), new RbyTurn("SUPERSONIC", Miss));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("ELIXER", "NIDOKING");
            Execute("U");
        }, () =>
        {
            ClearText();
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("ELIXER", "NIDOKING");
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM"), new RbyTurn("SUPERSONIC", Miss));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            Execute("U");
        }, () =>
        {
            ClearText();
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("ELIXER", "NIDOKING");
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("NIGHT SHADE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM"), new RbyTurn("SUPERSONIC", Miss));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            Execute("U");
        }, () =>
        {
            ClearText();
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("ELIXER", "NIDOKING");
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("NIGHT SHADE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM"), new RbyTurn("SUPERSONIC", Miss));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            Execute("U");
        });
    }
    void DuxTrade()
    {
        Comparison.Compare("basesaves/red/duxtrade.gqs", () =>
        {
            TalkTo(196, 3, 5);
            Yes();
            ChooseMenuItem(2);
            ClearText();
            MoveTo("VermilionCity", 15, 17, Action.Down);
        }, () =>
        {
            MoveTo("VermilionCity", 15, 17, Action.Down);
        });
    }
    void Route6XP()
    {
        Comparison.Compare("route6xp", "basesaves/red/route6.gqs", () =>
        {
            MoveTo("Route6", 17, 17);
            ForceEncounter(Action.Down, 1, 0x9888);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"));
            MoveTo("Route6", 17, 25);
        }, () =>
        {
            MoveTo("Route6", 17, 17);
            ForceEncounter(Action.Down, 1, 0x9888);
            RunAway();
            MoveTo("Route6", 17, 25);
        });
    }
    void BeatMistyIT()
    {
        RbyTurn.DefaultRoll = 30;
        Scenario Araya = () =>
        {
            MoveTo("CeruleanGym", 5, 3);
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("PECK"), false);
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("SUPERSONIC", Miss), false);
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TAIL WHIP", Miss), false);
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("PECK"), false);

            TalkTo(4, 2);
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("TACKLE", Crit));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("POTION", "WARTORTLE"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("POTION", "WARTORTLE"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TACKLE"));
            //
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("TACKLE", Crit));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH", Crit), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("POTION", "WARTORTLE"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH", Crit), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TACKLE"), false, false);
            ClearText(4);
        };
        Scenario Hwang = () =>
        {
            MoveTo("CeruleanGym", 5, 3);
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("SUPERSONIC"));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("PECK"), false);
            ForceTurn(new RbyTurn("MEGA PUNCH", Miss), new RbyTurn("TAIL WHIP"), false);
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("SUPERSONIC", Miss), false);
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            TalkTo(4, 2);
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH", Crit), new RbyTurn("TACKLE"));
            //
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("TACKLE", Crit));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("POTION", "WARTORTLE"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TACKLE", Crit), false, false);
            ClearText(4);
        };
        Comparison.Compare("beatmistyit", "basesaves/red/beatmistyit.gqs", () =>
        {
            MoveTo("BikeShop", 2, 6);
            UseItem("POTION", "WARTORTLE");
            UseItem("POTION", "WARTORTLE");
            TalkTo(6, 3);
            No();
            ClearText();

            Araya();
        }, () =>
        {
            MoveTo("CeruleanGym", 4, 12);
            UseItem("POTION", "WARTORTLE");
            UseItem("POTION", "WARTORTLE");

            Araya();
        });
    }
    void AgathaCandy()
    {
        Comparison.Compare("agathacandyuseyolo", "basesaves/red/agathacandyskip.gqs", "basesaves/red/agathacandy.gqs", () =>
        {
            ClearText();
            Execute("U U U");
            // UseItem("SUPER POTION", "NIDOKING");
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            SaveState("basesaves/red/agathacandyskip2yolo.gqs");
        }, () =>
        {
            ClearText();
            Execute("U U U");
            // UseItem("SUPER POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            SaveState("basesaves/red/agathacandy2yolo.gqs");
        });
        Comparison.Compare("agathacandy", "basesaves/red/agathacandyskip2.gqs", "basesaves/red/agathacandy2.gqs", () =>
        {
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("DREAM EATER"));
            // ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("NIGHT SHADE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            // ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn(AiItem | Turns * 20));
            // ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("SUPERSONIC", Miss));
            ForceTurn(new RbyTurn("BLIZZARD"), new RbyTurn("WING ATTACK"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("EARTHQUAKE"));
        }, () =>
        {
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("DREAM EATER"));
            // ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("NIGHT SHADE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("EARTHQUAKE"));
        });
    }
    void ChampCandySkip()
    {
        Comparison.Compare("basesaves/red/champcandyskip.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 31);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 32);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 35);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 37);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon2HP", 31);
            ClearText();
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            LoadState("basesaves/red/champ.gqs");
            CpuWriteBE<ushort>("wPartyMon2HP", 31);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });
    }
    void BoatRivalIT()
    {
        void Mart()
        {
            byte b = CpuRead("wd730");
            LoadState("basesaves/red/vermilionmart.gqs");
            CpuWrite("wd730", b);
            TalkTo(1, 5);
        }
        Comparison.Compare("boatrivalit", "basesaves/red/boatrivalit.gqs", () =>
        {
            CpuWrite("wd730", 0);
            CpuWriteBE<ushort>("wPartyMon1HP", 10);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo("SSAnneCaptainsRoom", 4, 2);
            Mart();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 10);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            CpuWrite("wd730", 0);
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo("SSAnneCaptainsRoom", 4, 2);
            Mart();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 10);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            CpuWrite("wd730", 0);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo("SSAnneCaptainsRoom", 4, 2);
            Mart();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 10);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            CpuWrite("wd730", 0);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo("SSAnneCaptainsRoom", 4, 2);
            Mart();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 10);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            CpuWrite("wd730", 0);
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo("SSAnneCaptainsRoom", 4, 2);
            Mart();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 10);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            BattleSwitch("PIDGEY", new RbyTurn("GUST"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo("SSAnneCaptainsRoom", 4, 2);
            Mart();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 10);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            BattleSwitch("PIDGEY", new RbyTurn("QUICK ATTACK"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo("SSAnneCaptainsRoom", 4, 2);
            Mart();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 10);
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo("SSAnneCaptainsRoom", 4, 2);
            Mart();
        });
    }
    void RockSlideSkip()
    {
        RbyTurn.DefaultRoll = 20;
        Comparison.Compare("rockslideskip", "basesaves/red/rockslideskip.gqs", () =>
        {
            TalkTo("CeladonMartRoof", 12, 2);
            ChooseMenuItem(0);
            ClearText();

            TalkTo("CeladonMart5F", 5, 4);
            Buy("X ACCURACY", 11, "X SPECIAL", 6, "X SPEED", 3);

            TalkTo("CeladonMartElevator", 3, 0);
            ChooseListItem(0);

            MoveTo("CeladonCity", 8, 14);
            UseItem("BICYCLE");

            CutAt("Route16", 34, 9);
            MoveTo("Route16", 17, 4);
            UseItem("BICYCLE");
            TalkTo("Route16FlyHouse", 2, 3);

            MoveTo("Route16", 7, 6);
            ItemSwap("HELIX FOSSIL", "X ACCURACY");
            UseItem("HM02", "PIDGEY");
            UseItem("SUPER REPEL");
            UseItem("TM07", "NIDOKING", "MEGA PUNCH");
            Fly("LavenderTown");

            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("PokemonTower4F", 15, 7);
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("LICK"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("LICK"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            PickupItemAt(12, 10);
            PickupItemAt("PokemonTower5F", 4, 12);

            MoveTo("PokemonTower5F", 11, 9);
            ClearText();

            MoveTo("PokemonTower6F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("LICK"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            TalkTo("PokemonTower6F", 9, 5);
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("LICK"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            PickupItemAt(6, 8);

            MoveTo(10, 16);
            ClearText();
            ItemSwap("HM01", "SUPER REPEL");
            UseItem("POKE DOLL");

            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE"));
            MoveSwap("THRASH", "HORN DRILL");
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            TalkTo("CeladonMartRoof", 12, 2);
            ChooseMenuItem(1);
            ClearText();

            TalkTo(5, 5);
            Yes();
            ChooseMenuItem(0);
            ClearText();

            TalkTo(12, 2, Action.Up);
            ChooseMenuItem(0);
            ClearText();

            TalkTo("CeladonMart5F", 5, 4);
            Buy("X ACCURACY", 11, "X SPECIAL", 6, "X SPEED", 3);

            TalkTo("CeladonMartElevator", 3, 0);
            ChooseListItem(0);

            MoveTo("CeladonCity", 8, 14);
            UseItem("BICYCLE");

            CutAt("Route16", 34, 9);
            MoveTo("Route16", 17, 4);
            UseItem("BICYCLE");
            TalkTo("Route16FlyHouse", 2, 3);

            MoveTo("Route16", 7, 6);
            ItemSwap("HELIX FOSSIL", "TM07");
            UseItem("SUPER REPEL");
            UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("HM02", "PIDGEY");
            Fly("LavenderTown");

            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("PokemonTower4F", 15, 7);
            MoveSwap("THRASH", "ROCK SLIDE");
            ForceTurn(new RbyTurn("ROCK SLIDE"));
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(12, 10);
            PickupItemAt("PokemonTower5F", 4, 12);

            MoveTo("PokemonTower5F", 11, 9);
            ClearText();

            MoveTo("PokemonTower6F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            TalkTo("PokemonTower6F", 9, 5);
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(6, 8);

            MoveTo(10, 15);
            UseItem("TM07", "NIDOKING", "ROCK SLIDE");

            MoveTo(10, 16);
            ClearText();
            ItemSwap("HM01", "SUPER REPEL");
            UseItem("POKE DOLL");

            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
        });
    }
    void HikerMachop()
    {
        Comparison.Compare("hikermachop", "basesaves/red/hikermachop.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 15);
            ClearText();
            ForceTurn(new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("KARATE CHOP", Crit));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("WATER GUN"));
            TalkTo(37, 4);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            TeachLevelUpMove("WATER GUN");
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 15);
            ClearText();
            ForceTurn(new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("WATER GUN"));
            TalkTo(37, 4);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            TeachLevelUpMove("WATER GUN");
        });
    }
    void CyclingCandy()
    {
        Comparison.Compare("cyclingcandy", "basesaves/red/snorlaxclassic.gqs", () =>
        {
            MoveTo("Route16", 27, 10);
            UseItem("SUPER REPEL");
            ItemSwap("PARLYZ HEAL", "RARE CANDY");
            UseItem("POKE FLUTE");
            RunAway();

            MoveTo("Route17", 15, 5);
            PickupItemAt("Route17", 15, 13);
            MoveTo("Route17", 17, 59);
            PickupItemAt("Route17", 17, 71);

            MoveTo("Route18", 13, 7);
            MoveTo("Route18", 40, 8);
        }, () =>
        {
            MoveTo("Route16", 27, 10);
            // UseItem("SUPER REPEL");
            ItemSwap("PARLYZ HEAL", "RARE CANDY");
            UseItem("POKE FLUTE");
            RunAway();

            // MoveTo("Route17", 15, 5);
            // PickupItemAt("Route17", 15, 13);
            MoveTo("Route17", 17, 59);
            PickupItemAt("Route17", 17, 71);

            MoveTo("Route18", 13, 7);
            MoveTo("Route18", 40, 8);
        });
    }
    void AntidotePotion()
    {
        Comparison.Compare("basesaves/red/antidotepotion.gqs", () =>
        {
            Execute("L");
            Press(Joypad.None, Joypad.Up, Joypad.None);
            PickupItem();
            Execute("U");
            PartySwap("SQUIRTLE", "NIDORANM");
            UseItem("ANTIDOTE", "SQUIRTLE");
            UseItem("POTION", "SQUIRTLE");
            MoveTo("PewterGym", 4, 2);
            TalkTo("PewterMart", 1, 5);
            Buy("POTION", 8);
            MoveTo(2, 33, 17, Action.Right);
        }, () =>
        {
            Execute("L U");
            PartySwap("SQUIRTLE", "NIDORANM");
            UseItem("ANTIDOTE", "SQUIRTLE");
            TalkTo("PewterMart", 1, 5);
            Buy("POTION", 8);
            MoveTo("PewterGym", 4, 12);
            UseItem("POTION", "SQUIRTLE");
            MoveTo("PewterGym", 4, 2);
            MoveTo(2, 33, 17, Action.Right);
        });
    }
    void MistyRetakeIT()
    {
        Comparison.Compare("mistyretakeit", "basesaves/red/postmisty.gqs", () =>
        {
            ClearText();
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            MoveTo(66, 2, 6);
            UseItem("POTION", "NIDOKING");
            UseItem("TM11", "NIDOKING", "POISON STING");
            TalkTo(6, 3);
            No();
            ClearText();
            MoveTo("Route6", 17, 25);
            MoveTo(15, 28);

            TalkTo(11, 30, Action.Down);
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(10, 31);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo("VermilionCity", 18, 30);
            ClearText();

            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("SSAnneCaptainsRoom", 4, 2);

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText();
            TalkTo("VermilionMart", 1, 5);
            Sell("TM34", 1, "NUGGET", 1);
            Buy("REPEL", 6, "PARLYZ HEAL", 2);

            MoveTo("VermilionCity", 15, 17, Action.Down);
            UseItem("HM01", "PARAS");
            UseItem("TM28", "PARAS");
            Cut();
        }, () =>
        {
            ClearText();
            CpuWriteBE<ushort>("wPartyMon1HP", 25);
            MoveTo("Route6", 17, 25);
            MoveTo(15, 28);

            TalkTo(11, 30, Action.Down);
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(10, 31);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo("VermilionCity", 18, 30);
            ClearText();

            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("SSAnneCaptainsRoom", 4, 2);

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText();
            TalkTo("VermilionMart", 1, 5);
            Sell("TM34", 1, "NUGGET", 1);
            Buy("REPEL", 6, "PARLYZ HEAL", 2);

            MoveTo("VermilionCity", 15, 17, Action.Down);
            UseItem("TM11", "NIDOKING", "POISON STING");
            UseItem("HM01", "PARAS");
            UseItem("TM28", "PARAS");
            Cut();
        });
    }
    void SilphBarZam()
    {
        Comparison.Compare("silphbarzam", "basesaves/red/silphrivalqa.gqs", () =>
        {
            ForceTurn(new RbyTurn("HORN DRILL"), new RbyTurn("QUICK ATTACK", 1));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("PSYBEAM", 20));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ForceTurn(new RbyTurn("HORN DRILL"), new RbyTurn("QUICK ATTACK", 1));
            ForceTurn(new RbyTurn("SUPER POTION", "NIDOKING"), new RbyTurn("HYDRO PUMP", 16));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        }, () =>
        {
            ForceTurn(new RbyTurn("HORN DRILL"), new RbyTurn("QUICK ATTACK", 1));
            ForceTurn(new RbyTurn("SUPER POTION", "NIDOKING"), new RbyTurn("HYDRO PUMP", Miss));
            ForceTurn(new RbyTurn("POKE FLUTE"), new RbyTurn("HYDRO PUMP", 16));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });
    }
    void Flybar()
    {
        void Flybar(bool cubone = false, bool slowpoke = false)
        {
            Press(Joypad.A);
            ClearText();
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // BUG CATCHER
            TalkTo(40, 8);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(21, 3, 10);
            MoveTo(8, 18);
            MoveTo("RockTunnel1F", 15, 4);
            UseItem("REPEL");

            // POKEMANIAC 1
            TalkTo("RockTunnel1F", 23, 8);
            if(cubone) ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("BONE CLUB"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            if(slowpoke) {
                ForceTurn(new RbyTurn("THRASH"), new RbyTurn("CONFUSION"));
                ForceTurn(new RbyTurn("THRASH"));
            }
            else ForceTurn(new RbyTurn("THUNDERBOLT"));

            // POKEMANIAC 2
            TalkTo("RockTunnelB1F", 26, 30);
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            // ODDISH GIRL
            TalkTo(14, 28);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(34, 19);
            UseItem("REPEL");
            MoveTo(82, 11, 14);
            UseItem("REPEL");

            // HIKER
            TalkTo(232, 6, 10);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));

            // PIDGEY GIRL
            TalkTo("RockTunnel1F", 22, 24);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(15, 32);
            PickupItemAt(21, 16, 53); // max ether

            // GAMBLER
            TalkTo("Route8", 46, 13);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));

            ushort hp = CpuReadBE<ushort>("wPartyMon1HP");
            LoadState("basesaves/red/lavenderrival.gqs");
            CpuWriteBE<ushort>("wPartyMon1HP", hp);

            // LAVENDER RIVAL
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            // CHANNELER 1
            TalkTo("PokemonTower4F", 15, 7);
            MoveSwap("THRASH", "ROCK SLIDE");
            ForceTurn(new RbyTurn("ROCK SLIDE"));
            ForceTurn(new RbyTurn("ROCK SLIDE"));
        }
        Comparison.Compare("flybar", "basesaves/red/4ttg.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            Flybar();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 29);
            Flybar(true, false);
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 23);
            Flybar(false, true);
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 23);
            Flybar();
        });
    }
    void Fastswap()
    {
        Comparison.Compare("fastswap", "basesaves/red/wildforest_squirtle.gqs", () =>
        {
            MoveTo("PewterCity", 18, 35);
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            MoveTo(1, 8);
            ForceEncounter(Action.Up, 0, 0x8888);
            RunAway();
            MoveTo("PewterCity", 18, 35);
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo(1, 8);
            ForceEncounter(Action.Up, 0, 0x8888);
            RunAway();
            MoveTo("PewterCity", 18, 32);
        });
    }
    void AntidoteInFight()
    {
        RbyTurn.DefaultRoll = 20;
        Comparison.Compare("antidoteinfight", "basesaves/red/weedle.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("ANTIDOTE", "SQUIRTLE"), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            MoveTo("PewterCity", 18, 35);
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            PartySwap("SQUIRTLE", "NIDORANM");
            UseItem("ANTIDOTE", "SQUIRTLE");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("ANTIDOTE", "SQUIRTLE"), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            MoveTo("PewterCity", 18, 35);
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            PartySwap("SQUIRTLE", "NIDORANM");
            UseItem("ANTIDOTE", "SQUIRTLE");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("ANTIDOTE", "SQUIRTLE"), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            MoveTo("PewterCity", 18, 35);
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            PartySwap("SQUIRTLE", "NIDORANM");
            UseItem("ANTIDOTE", "SQUIRTLE");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("ANTIDOTE", "SQUIRTLE"), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            MoveTo("PewterCity", 18, 35);
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            PartySwap("SQUIRTLE", "NIDORANM");
            UseItem("ANTIDOTE", "SQUIRTLE");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("ANTIDOTE", "SQUIRTLE"), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            MoveTo("PewterCity", 18, 35);
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("POISON STING", SideEffect));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            PartySwap("SQUIRTLE", "NIDORANM");
            UseItem("ANTIDOTE", "SQUIRTLE");
            MoveTo("PewterCity", 18, 32);
        });
    }
    void AntidoteSkip()
    {
        Comparison.Compare("antidotepick", "basesaves/red/antidote.gqs", () =>
        {
            PickupItemAt(25, 11);
            MoveTo(24, 9);
        }, () =>
        {
            MoveTo(24, 9);
        });
        Comparison.Compare("antidoteuse", "basesaves/red/antidotepotion.gqs", () =>
        {
            Execute("L U");
            PartySwap("SQUIRTLE", "NIDORANM");
            UseItem("ANTIDOTE", "SQUIRTLE");
            MoveTo("PewterCity", 18, 32);
        }, () =>
        {
            CpuWrite("wPartyMon1Status", 0);
            Execute("L");
            MoveTo("PewterCity", 18, 35);
            PartySwap("SQUIRTLE", "NIDORANM");
            MoveTo("PewterCity", 18, 32);
        });
        Comparison.Compare("basesaves/red/antidotesell.gqs", () =>
        {
            ClearText();
            TalkTo(1, 5);
            Sell("ANTIDOTE", 1);
            Buy("POTION", 8);
            MoveTo(3, 6);
        }, () =>
        {
            ClearText();
            TalkTo(1, 5);
            Buy("POTION", 8);
            MoveTo(3, 6);
        });
        Comparison.Compare("basesaves/red/antidotetoss.gqs", () =>
        {
            ClearText();
            MoveTo(11, 4);
            TossItem("ANTIDOTE");
            UseItem("POTION", "NIDORANM");
            MoveTo(13, 4);
        }, () =>
        {
            CpuWrite("wNumBagItems", 3);
            CpuWrite(SYM["wBagItems"] + 2, CpuRead(SYM["wBagItems"] + 4, 6));
            ClearText();
            MoveTo(11, 4);
            UseItem("POTION", "NIDORANM");
            MoveTo(13, 4);
        });
    }
    void Yolograss()
    {
        Comparison.Compare("basesaves/red/route1yolograss.gqs", () =>
        {
            ClearText();
            MoveTo(12, 21);
            MoveTo(11, 24);
            MoveTo(11, 31);
        }, () =>
        {
            ClearText();
            MoveTo(12, 22);
            ForceEncounter(Action.Down, 0, 0);
            RunAway();
            MoveTo(11, 24);
            MoveTo(11, 31);
        }, () =>
        {
            ClearText();
            MoveTo(16, 20);
            MoveTo(16, 26);
            MoveTo(14, 26);
            MoveTo(14, 28);
            MoveTo(11, 28);
            MoveTo(11, 31);
        });
        Comparison.Compare("route1ledge", "basesaves/red/route1yolograss.gqs", () =>
        {
            ClearText();
            MoveTo(12, 23);
            ForceEncounter(Action.Down, 0, 0);
            RunAway();
            MoveTo(10, 24);
            MoveTo(10, 28);
            MoveTo(0, 10, 3, Action.Down);
        }, () =>
        {
            ClearText();
            MoveTo(12, 23);
            ForceEncounter(Action.Down, 0, 0);
            RunAway();
            MoveTo(11, 24);
            MoveTo(11, 28);
            MoveTo(0, 10, 3, Action.Down);
        }, () =>
        {
            ClearText();
            MoveTo(12, 23);
            ForceEncounter(Action.Down, 0, 0);
            RunAway();
            MoveTo(12, 28);
            MoveTo(11, 28);
            MoveTo(0, 10, 3, Action.Down);
        });
        Comparison.Compare("basesaves/red/route1down.gqs", () =>
        {
            MoveTo("Route1", 10, 1);
            MoveTo(8, 6);
            MoveTo(9, 12);
            MoveTo(9, 14);
            MoveTo(16, 22);
            MoveTo(16, 24);
            MoveTo(16, 26);
            MoveTo(14, 26);
            MoveTo(14, 28);
            MoveTo(10, 28);
            MoveTo(0, 10, 3, Action.Down);
        }, () =>
        {
            MoveTo(1, 29, 26);
            MoveTo(1, 29, 28);
            MoveTo("Route1", 10, 1);
            MoveTo(10, 3);
            MoveTo(8, 6);
            MoveTo(8, 12);
            MoveTo(9, 12);
            MoveTo(9, 14);
            MoveTo(9, 21);
            MoveTo(16, 21);
            MoveTo(16, 24);
            MoveTo(16, 26);
            MoveTo(10, 26);
            MoveTo(10, 28);
            MoveTo(0, 10, 3, Action.Down);
        });
    }
    void PokeManiac()
    {
        Comparison.Compare("pokemaniac1r", "basesaves/red/pokemaniac1.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 23);
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("CONFUSION"));
            ForceTurn(new RbyTurn("THRASH"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 23);
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("CONFUSION"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
        });
        Comparison.Compare("pokemaniac1rcrit", "basesaves/red/pokemaniac1.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 23);
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH", Crit));
            TalkTo("RockTunnelB1F", 26, 30);
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("CONFUSION"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 23);
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM", Crit), new RbyTurn("CONFUSION"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            TalkTo("RockTunnelB1F", 26, 30);
            ForceTurn(new RbyTurn("THUNDERBOLT"));
        });
    }
    void LedgeHop()
    {
        Comparison.Compare("basesaves/red/ledgehop.gqs", () =>
        {
            Execute("R D D D R R R");
        }, () =>
        {
            Execute("R R R D D D R");
        }, () =>
        {
            Execute("R R D D D D R R");
        });
        Comparison.Compare("basesaves/red/ledgehopbike.gqs", () =>
        {
            Execute("R R R R D D D D R R R");
        }, () =>
        {
            Execute("R R R R R R D D D D R");
        }, () =>
        {
            Execute("R R R R R D D D D D R R");
        });
    }
    void FlashVsStatus()
    {
        RbyTurn.DefaultRoll = 20;
        Comparison.Compare("flashvsstatus", "basesaves/red/shortsguy.gqs", () =>
        {
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TAIL WHIP", Miss), true, false);
        }, () =>
        {
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TAIL WHIP", Miss), true, false);
        });
    }
    void ParasRedbar()
    {
        Comparison.Compare("basesaves/red/parasredbar.gqs", () =>
        {
            Execute("D D");
            SaveAndQuit();
            LoadState("basesaves/red/parasredbar2.gqs");
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            Execute("U U");
            ForceEncounter(Action.Up, 5, 0x0000);
            ForceYoloball("POKE BALL");
            ClearText();
            No();
            ClearText();
            UseItem("POTION", "NIDORANM");
            UseItem("POTION", "NIDORANM");
            MoveTo("MtMoonB2F", 11, 17);
        }, () =>
        {
            Execute("D D");
            UseItem("POTION", "NIDORANM");
            UseItem("POTION", "NIDORANM");
            SaveAndQuit();
            LoadState("basesaves/red/parasredbar2.gqs");
            CpuWriteBE<ushort>("wPartyMon1HP", 41);
            Execute("U U");
            ForceEncounter(Action.Up, 5, 0x0000);
            ForceYoloball("POKE BALL");
            ClearText();
            No();
            ClearText();
            MoveTo("MtMoonB2F", 11, 17);
        });
        Comparison.Compare("parasredbarcatch", "basesaves/red/parasredbar2.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            Execute("U U");
            ForceEncounter(Action.Up, 5, 0x0000);
            ForceYoloball("POKE BALL");
            ClearText();
            No();
            ClearText();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 41);
            Execute("U U");
            ForceEncounter(Action.Up, 5, 0x0000);
            ForceYoloball("POKE BALL");
            ClearText();
            No();
            ClearText();
        });
    }
    void Route22Ledge()
    {
        Comparison.Compare("basesaves/red/route22ledge.gqs", () =>
        {
            ClearText();
            MoveTo(16, 5);
            MoveTo(16, 12);
            MoveTo(14, 12);
        }, () =>
        {
            ClearText();
            MoveTo(17, 5);
            MoveTo(17, 12);
            MoveTo(14, 12);
        }, () =>
        {
            ClearText();
            MoveTo(18, 5);
            MoveTo(18, 12);
            MoveTo(14, 12);
        }, () =>
        {
            ClearText();
            MoveTo(19, 5);
            MoveTo(19, 12);
            MoveTo(14, 12);
        }, () =>
        {
            ClearText();
            MoveTo(20, 5);
            MoveTo(20, 12);
            MoveTo(14, 12);
        }, () =>
        {
            ClearText();
            MoveTo(21, 5);
            MoveTo(21, 12);
            MoveTo(14, 12);
        });
    }

    Comparison Comparison;
    public RedComparisons() : base()
    {
        Comparison = new Comparison(this);
        LedgeHop();
        // for(int w = 0; w < 100; ++w)
        // int w = 0;
        // {
        //     Console.WriteLine(w);
        // Comparison.Compare("loreleippup", "basesaves/red/lorelei2.gqs", new Scenario[] {() =>
        // {
        //     // AfterMoveAndSplit();
        //     // TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
        //     // Deposit("SQUIRTLE", "PARAS");
        //     // MoveTo("IndigoPlateauLobby", 8, 0);
        //     // PartySwap("NIDOKING", "PIDGEY");

        //     AdvanceFrames(w);
        //     TalkTo("LoreleisRoom", 5, 2, Action.Right);
        //     ForceTurn(new RbyTurn("GUST"), new RbyTurn("AURORA BEAM"));
        //     SendOut("NIDOKING");
        //     ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));
        //     ForceTurn(new RbyTurn("HORN DRILL"));
        //     ForceTurn(new RbyTurn("HORN DRILL"));
        //     ForceTurn(new RbyTurn("HORN DRILL"));
        //     ForceTurn(new RbyTurn("HORN DRILL"));
        //     ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        // }, () =>
        // {
        //     // AfterMoveAndSplit();
        //     // TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
        //     // Deposit("SQUIRTLE", "PARAS");
        //     // MoveTo("IndigoPlateauLobby", 8, 0);
        //     // PartySwap("NIDOKING", "PIDGEY");

        //     CpuWrite("wPartyMon2PP", 5 + 64);
        //     AdvanceFrames(w);
        //     TalkTo("LoreleisRoom", 5, 2, Action.Right);
        //     ForceTurn(new RbyTurn("GUST"), new RbyTurn("AURORA BEAM"));
        //     SendOut("NIDOKING");
        //     ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));
        //     ForceTurn(new RbyTurn("HORN DRILL"));
        //     ForceTurn(new RbyTurn("HORN DRILL"));
        //     ForceTurn(new RbyTurn("HORN DRILL"));
        //     ForceTurn(new RbyTurn("HORN DRILL"));
        //     ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        // }}, true);
        // }
        // LoadState("rng-cache/RedGlitchlessPPUPFlexCandy/victoryroad.gqs");
        // byte[] data = CpuRead("wPartyDataStart", SYM["wRivalName"] - SYM["wPartyDataStart"]);
        // LoadState("rng-cache/checkpoints/victoryroad.gqs");
        //     AfterMoveAndSplit();
        //     TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
        //     Deposit("SQUIRTLE", "PARAS");
        //     MoveTo("IndigoPlateauLobby", 8, 0);
        //     PartySwap("NIDOKING", "PIDGEY");
        //     MoveTo("LoreleisRoom", 4, 3);
        // SaveState("basesaves/red/lorelei2.gqs");
        // CpuWrite("wPartyDataStart", data);
        // SaveState("basesaves/red/loreleisplitflex.gqs");
    }
}
