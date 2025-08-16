using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using static Comparison;

public class YellowComparisons : YellowForceComparisons
{
    void FuchsiaCut()
    {
        Comparison.Compare("basesaves/yellow/fuchsiacuty.gqs", () =>
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
    void SafariRepels()
    {
        Comparison.Compare("basesaves/yellow/safarirepels.gqs", () =>
        {
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");
            MoveTo(217, 4, 23);
            UseItem("SUPER REPEL");
            CloseMenu(Joypad.Up); // direction close
            PickupItemAt(217, 21, 10, Action.Down);
            MoveTo(218, 39, 30);
            MoveTo(218, 7, 13);
            PickupItemAt("SafariZoneWest", 19, 7, Action.Down);
            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();
            UseItem("BICYCLE");
            MoveTo(18, 18, 10);
            UseItem("BICYCLE");
            MoveTo(10, 34, 4);
            Inject(Joypad.Up);
            RunUntil("EnterMap");
        }, () =>
        {
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");
            PickupItemAt(217, 21, 10, Action.Down);
            MoveTo(218, 22, 31);
            UseItem("SUPER REPEL");
            MoveTo(218, 7, 13);
            PickupItemAt("SafariZoneWest", 19, 7, Action.Down);
            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();
            UseItem("BICYCLE");
            MoveTo(18, 18, 10);
            UseItem("BICYCLE");
            MoveTo(10, 34, 4);
            Inject(Joypad.Up);
            RunUntil("EnterMap");
        });
    }
    void MansionRepels()
    {
        Comparison.Compare("basesaves/yellow/mansionrepels.gqs", () =>
        {
            MoveTo("CinnabarIsland", 4, 4);
            MoveTo(165, 5, 15);
            UseItem("SUPER REPEL");
            TalkTo("PokemonMansion3F", 10, 5, Action.Up);
            ActivateMansionSwitch();
            MoveTo(16, 14);
            FallDown();
            PickupItemAt(18, 21);
            TalkTo("PokemonMansionB1F", 18, 25, Action.Up);
            ActivateMansionSwitch();
            TalkTo(20, 3, Action.Up);
            ActivateMansionSwitch();
            PickupItemAt(5, 13);
            Dig();
            Fly("CinnabarIsland");
            UseItem("BICYCLE");
            MoveTo("CinnabarGym", 15, 7, Action.Up);
        }, () =>
        {
            MoveTo("CinnabarIsland", 4, 4);
            MoveTo(165, 5, 26);
            UseItem("SUPER REPEL");
            TalkTo("PokemonMansion3F", 10, 5, Action.Up);
            ActivateMansionSwitch();
            MoveTo(16, 14);
            FallDown();
            PickupItemAt(18, 21);
            TalkTo("PokemonMansionB1F", 18, 25, Action.Up);
            ActivateMansionSwitch();
            TalkTo(20, 3, Action.Up);
            ActivateMansionSwitch();
            PickupItemAt(5, 13);
            Dig();
            Fly("CinnabarIsland");
            UseItem("BICYCLE");
            MoveTo("CinnabarGym", 15, 7, Action.Up);
        });
    }
    void ThrashvsHA()
    {
        LoadState("basesaves/yellow/4ttg.gqs");
        Press(Joypad.A);
        ClearText();
        ForceTurn(new RbyTurn("THRASH", ThreeTurn));
        ForceTurn(new RbyTurn("THRASH"));
        byte[] state = SaveState();

        Comparison.Compare("4ttgthrashy", state, () =>
        {
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), null, true, false);
        }, () =>
        {
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("HORN ATTACK"), null, true, false);
        });
    }
    void VRElixer()
    {
        void ToMaxEther()
        {
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
        }
        void ToVRMenu()
        {
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
            MoveTo("VictoryRoad3F", 23, 7);
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
        }
        void ToCenter()
        {
            Execute("D R R U");
            PushBoulder(Joypad.Left, 14);
            PickupItemAt(194, 26, 7);
            MoveTo("VictoryRoad2F", 29, 7);
            TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
            Deposit("PIDGEY", "CHARMANDER", "LAPRAS");
        }
        void ToBrunoMenu()
        {
            MoveTo("IndigoPlateauLobby", 8, 0);
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
            ClearText();
            Execute("U U U");
        }

        Comparison.Compare("basesaves/yellow/vrelixer.gqs", () =>
        {
            RunUntil("JoypadOverworld");
            UseItem("BICYCLE");

            ToMaxEther();
            MoveTo(7, 90);
            // Press(Joypad.None, Joypad.Right, Joypad.None);
            Execute("L R");
            PickupItem();

            ToVRMenu();
            Strength();
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            ToCenter();
            ToBrunoMenu();
            UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");
            MoveTo("BrunosRoom", 5, 2, Action.Right);
        }, () =>
        {
            RunUntil("JoypadOverworld");
            UseItem("BICYCLE");

            ToMaxEther();
            ToVRMenu();
            Strength();
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            ToCenter();
            TalkTo(7, 6);
            Yes();
            ClearText();

            ToBrunoMenu();
            UseItem("ELIXER", "NIDOKING");
            MoveTo("BrunosRoom", 5, 2, Action.Right);
        });
        Comparison.Compare("vrelixeronbike", "basesaves/yellow/vrelixer.gqs", () =>
        {
            RunUntil("JoypadOverworld");
            UseItem("BICYCLE");

            ToMaxEther();
            MoveTo(7, 90);
            Execute("L R");
            PickupItem();

            ToVRMenu();
            Strength();
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            Execute("D R R U");
            PushBoulder(Joypad.Left);
        }, () =>
        {
            RunUntil("JoypadOverworld");
            UseItem("ELIXER", "NIDOKING");
            UseItem("BICYCLE");

            ToMaxEther();
            MoveTo(7, 90);
            Execute("L R");
            PickupItem();

            ToVRMenu();
            Strength();
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            Execute("D R R U");
            PushBoulder(Joypad.Left);
        });
    }
    void DKvsTackle()
    {
        Comparison.Compare("basesaves/yellow/dkvstackle.gqs", () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK", 30 | Crit), new RbyTurn("POISON STING", 20));
            ForceTurn(new RbyTurn("DOUBLE KICK", 20), null, false, false);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK", 20 | Crit), new RbyTurn("POISON STING", 20));
            ForceTurn(new RbyTurn("DOUBLE KICK", 20), null, false, false);
        }, () =>
        {
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK", 20 | Crit), new RbyTurn("POISON STING", 20));
            ForceTurn(new RbyTurn("TACKLE"), null, false, false);
        });
    }
    void FuchsiaMenu()
    {
        Comparison.Compare("basesaves/yellow/fuchsiamenu.gqs", () =>
        {
            RunUntil("JoypadOverworld");
            MoveNpc("PalletTown", 3, 8, Action.Up);
            Fly("PalletTown");
            UseItem("SUPER REPEL");
            UseItem("ELIXER", "NIDOKING");
            // UseItem("FULL RESTORE", "NIDOKING");
            UseItem("TM13", "NIDOKING", "THRASH");
            ItemSwap("S.S.TICKET", "X ATTACK");
            UseItem("HM04", "LAPRAS");
            UseItem("HM03", "LAPRAS");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("BICYCLE");
            MoveTo(4, 8, Action.Left);
            Inject(Joypad.Left);
            AdvanceFrames(5, Joypad.Left);
            Execute("D");
            MoveTo(4, 17, Action.Right);
            Surf();
            MoveTo(32, 4, 0);
        }, () =>
        {
            RunUntil("JoypadOverworld");
            UseItem("SUPER REPEL");
            UseItem("ELIXER", "NIDOKING");
            // UseItem("FULL RESTORE", "NIDOKING");
            UseItem("TM13", "NIDOKING", "THRASH");
            ItemSwap("S.S.TICKET", "X ATTACK");
            UseItem("HM04", "LAPRAS");
            UseItem("HM03", "LAPRAS");
            UseItem("RARE CANDY", "NIDOKING");
            MoveNpc("PalletTown", 3, 8, Action.Up);
            Fly("PalletTown");
            MoveTo(4, 13, Action.Down);
            Surf();
            MoveTo(32, 4, 0);
        });
        Comparison.Compare("fuchsiamenutroll", "basesaves/yellow/fuchsiamenu.gqs", () =>
        {
            RunUntil("JoypadOverworld");
            MoveNpc("PalletTown", 3, 8, Action.Down);
            Fly("PalletTown");
            UseItem("SUPER REPEL");
            UseItem("ELIXER", "NIDOKING");
            // UseItem("FULL RESTORE", "NIDOKING");
            UseItem("TM13", "NIDOKING", "THRASH");
            ItemSwap("S.S.TICKET", "X ATTACK");
            UseItem("HM04", "LAPRAS");
            UseItem("HM03", "LAPRAS");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("BICYCLE");
            Execute("D");
            MoveTo(4, 17, Action.Right);
            Surf();
            MoveTo(32, 4, 0);
        }, () =>
        {
            RunUntil("JoypadOverworld");
            UseItem("SUPER REPEL");
            UseItem("ELIXER", "NIDOKING");
            // UseItem("FULL RESTORE", "NIDOKING");
            UseItem("TM13", "NIDOKING", "THRASH");
            ItemSwap("S.S.TICKET", "X ATTACK");
            UseItem("HM04", "LAPRAS");
            UseItem("HM03", "LAPRAS");
            UseItem("RARE CANDY", "NIDOKING");
            MoveNpc("PalletTown", 3, 8, Action.Down);
            Fly("PalletTown");
            MoveTo(4, 7);
            MoveTo(4, 13, Action.Down);
            Surf();
            MoveTo(32, 4, 0);
        });
        Comparison.Compare("momheal", "basesaves/yellow/fuchsiamenulow.gqs", () =>
        {
            RunUntil("JoypadOverworld");
            Fly("PalletTown");
            TalkTo(37, 5, 4);
            MoveNpc("PalletTown", 3, 8, Action.Up);
            MoveTo(0, 5, 6);
            UseItem("SUPER REPEL");
            // UseItem("ELIXER", "NIDOKING");
            // UseItem("FULL RESTORE", "NIDOKING");
            UseItem("TM13", "NIDOKING", "THRASH");
            ItemSwap("S.S.TICKET", "X ATTACK");
            UseItem("HM04", "LAPRAS");
            UseItem("HM03", "LAPRAS");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("BICYCLE");
            MoveTo(4, 8, Action.Left);
            // Inject(Joypad.Left);
            // AdvanceFrames(5, Joypad.Left);
            // Execute("D");
            MoveTo(4, 17, Action.Right);
            Surf();
            MoveTo(32, 4, 0);
        }, () =>
        {
            RunUntil("JoypadOverworld");
            MoveNpc("PalletTown", 3, 8, Action.Up);
            Fly("PalletTown");
            UseItem("SUPER REPEL");
            UseItem("ELIXER", "NIDOKING");
            UseItem("FULL RESTORE", "NIDOKING");
            UseItem("TM13", "NIDOKING", "THRASH");
            ItemSwap("S.S.TICKET", "X ATTACK");
            UseItem("HM04", "LAPRAS");
            UseItem("HM03", "LAPRAS");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("BICYCLE");
            MoveTo(4, 8, Action.Left);
            Inject(Joypad.Left);
            AdvanceFrames(5, Joypad.Left);
            Execute("D");
            MoveTo(4, 17, Action.Right);
            Surf();
            MoveTo(32, 4, 0);
        });
    }
    void FullRestore()
    {
        Comparison.Compare("basesaves/yellow/fullrestore.gqs", () =>
        {
            MoveTo(198, 26, 8, Action.Left);
            TalkTo(174, 1, 5);
            Buy("FULL RESTORE", 2);
            MoveTo("IndigoPlateauLobby", 8, 0);
            MoveTo(245, 5, 2, Action.Right);
        }, () =>
        {
            PickupItemAt(198, 26, 8, Action.Left);
            MoveTo("IndigoPlateauLobby", 8, 0);
            MoveTo(245, 5, 2, Action.Right);
        });
    }
    void XSpeedMenu()
    {
        Comparison.Compare("basesaves/yellow/xspeedmenu.gqs", () =>
        {
            ClearText();
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            PickupItemAt(21, 16);
            TalkTo(210, 8, 13, Action.Left);
            TalkTo(208, 17, 9);
            MoveTo(212, 4, 2, Action.Left);
        }, () =>
        {
            ClearText();
            PickupItemAt(21, 16);
            MoveTo(233, 17, 14);
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            TalkTo(210, 8, 13, Action.Left);
            TalkTo(208, 17, 9);
            MoveTo(212, 4, 2, Action.Left);
        });
    }
    void Gambler()
    {
        Comparison.Compare("basesaves/yellow/gambler.gqs", () =>
        {
            ClearText();
            MoveSwap("THRASH", "BUBBLEBEAM");
            ForceTurn(new RbyTurn("BUBBLEBEAM", 1), new RbyTurn("ROAR"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
        }, () =>
        {
            ClearText();
            MoveSwap("THRASH", "BUBBLEBEAM");
            ForceTurn(new RbyTurn("BUBBLEBEAM", 1), new RbyTurn("ROAR"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THRASH"));
        });
    }
    void Flybar()
    {
        void Flybar(bool cubone = false, bool slowpoke = false)
        {
            Press(Joypad.A);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
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

            // GAMBLER
            TalkTo("Route8", 46, 13);
            MoveSwap("THRASH", "BUBBLEBEAM");
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));

            ushort hp = CpuReadBE<ushort>("wPartyMon1HP");
            LoadState("basesaves/yellow/lavenderrival.gqs");
            CpuWriteBE<ushort>("wPartyMon1HP", hp);

            // LAVENDER RIVAL
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            // CHANNELER 1
            TalkTo("PokemonTower4F", 15, 7);
            ForceTurn(new RbyTurn("ROCK SLIDE"));
            ForceTurn(new RbyTurn("ROCK SLIDE"));
        }
        Comparison.Compare("flybary", "basesaves/yellow/4ttg.gqs", () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 5);
            Flybar();
        }, () =>
        {
            CpuWriteBE<ushort>("wPartyMon1HP", 31);
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
    void PokeManiac()
    {
        Comparison.Compare("pokemaniac1y", "basesaves/yellow/pokemaniac1.gqs", () =>
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
        Comparison.Compare("pokemaniac1ycrit", "basesaves/yellow/pokemaniac1.gqs", () =>
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

    Comparison Comparison;
    public YellowComparisons() : base()
    {
        Comparison = new Comparison(this);
        DKvsTackle();
        Environment.Exit(0);
    }
}
