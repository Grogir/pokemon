public class RedClassicFTEexa : RedBlueForceComparisons
{
    public RedClassicFTEexa(bool silphbar = true, bool ppup = true, bool extracandy = false)
    {
        // RecordAndTime("red-classic-fte-exa");
        // Show();
        // RbyTurn.DefaultRoll = 20;

        string cutter = "Paras";
        LoadState("basesaves/red/classicfte_" + cutter + ".gqs");
        CpuWrite(SYM["wBagItems"] + 2 * Bag.IndexOf("X ACCURACY") + 1, 10);
        CpuWrite(SYM["wBagItems"] + 2 * Bag.IndexOf("X SPEED") + 1, 7);
        CpuWrite(SYM["wBagItems"] + 2 * Bag.IndexOf("X SPECIAL") + 1, 5);
        CpuWrite("wNumBagItems", (byte) (CpuRead("wNumBagItems") - 1));
        for(int slot = 6 * 2; slot < 19 * 2; ++slot)
            CpuWrite(SYM["wBagItems"] + slot, CpuRead(SYM["wBagItems"] + slot + 2));
        // CpuWriteBE<ushort>("wPartyMon1DVs", 0xffe0);

        Timer.Start();

        ClearCache();
        CacheState("silph", () => {
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
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            PickupItemAt(21, 16);
            TalkTo(7, 13);
            TalkTo("SilphCo3F", 17, 9);

            if(silphbar) {
                // SILPH RIVAL
                MoveTo("SilphCo7F", 3, 2, Action.Left);
                ClearText();
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
                ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
                ForceTurn(new RbyTurn("THUNDERBOLT"));
                ForceTurn(new RbyTurn("ROCK SLIDE"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));

                MoveTo(5, 7, Action.Right);
                // UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");

                // SILPH ROCKET
                TalkTo("SilphCo11F", 3, 16);
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
                ForceTurn(new RbyTurn("ICE BEAM"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("ICE BEAM"));
            } else {
                // SILPH RIVAL
                MoveTo("SilphCo7F", 3, 2, Action.Left);
                ClearText();
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
                ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("ROCK SLIDE"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));

                MoveTo(5, 7, Action.Right);

                // SILPH ROCKET
                TalkTo("SilphCo11F", 3, 16);
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
                ForceTurn(new RbyTurn("ICE BEAM"));
                ForceTurn(new RbyTurn("MAX ETHER", "NIDOKING", "HORN DRILL"), new RbyTurn("CONFUSION"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
            }

            UseItem("ELIXER", "NIDOKING");
            // SILPH GIOVANNI
            TalkTo(6, 13, Action.Up);
            MoveTo(6, 13);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("safari", () => {
            ClearText();
            TalkTo(236, 3, 0);
            ChooseListItem(9);
            Execute("L D D D"); // exit elevator

            // PickupItemAt(234, 5, 11); // carbos
            PickupItemAt(234, 2, 12); // tm26
            PickupItemAt(234, 4, 14); // candy
            Dig();

            // UseItem("CARBOS", "NIDOKING");
            UseItem("BICYCLE");

            // Snorlax menu
            MoveTo("Route16", 27, 10);
            UseItem("SUPER REPEL");
            ItemSwap("POTION", "RARE CANDY");
            UseItem("POKE FLUTE");
            RunAway();

            MoveTo("Route17", 15, 5);
            PickupItemAt("Route17", 15, 13); // candy
            if(ppup) {
                MoveTo("Route17", 17, 59);
                PickupItemAt("Route17", 17, 71); // pp up
            }

            // Post cycling menu
            MoveTo("Route18", 13, 7);
            MoveTo("Route18", 40, 8);
            UseItem("SUPER REPEL");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            if(ppup) {
                ItemSwap("PARLYZ HEAL", "TM26");
                UseItem("PP UP", "NIDOKING", "HORN DRILL");
            }
            UseItem("TM26", "NIDOKING", "ROCK SLIDE");
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
        });

        // ClearCache();
        CacheState("koga", () => {
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
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            // KOGA
            TalkTo(4, 10);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("X ACCURACY"), new RbyTurn("SELFDESTRUCT")); // x acc
        });

        // ClearCache();
        CacheState("blaine", () => {
            ClearText();

            // Candy menu
            MoveTo("FuchsiaCity", 5, 28);
            // UseItem("RARE CANDY", "NIDOKING");
            UseItem("BICYCLE");

            TalkTo("WardensHouse", 2, 3);
            MoveTo("FuchsiaCity", 27, 28);
            MoveNpc("PalletTown", 3, 8, Action.Left); // good npc
            Fly("PalletTown");

            // Surf menu
            MoveTo(4, 13);
            UseItem("SUPER REPEL");
            UseItem("HM03", "SQUIRTLE", "TAIL WHIP");
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

            // PickupItemAt(18, 21); // carbos

            // HM04 menu
            // MoveTo(21, 21);
            // UseItem("HM04", "SQUIRTLE", "TACKLE");
            // UseItem("CARBOS", "NIDOKING");
            // UseItem("SUPER REPEL");

            TalkTo("PokemonMansionB1F", 18, 25, Action.Up);
            ActivateMansionSwitch();

            TalkTo(20, 3, Action.Up);
            ActivateMansionSwitch();

            PickupItemAt(10, 2); // candy
            if(extracandy)
                PickupItemAt(1, 9); // extra candy
            MoveTo(5, 9);
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            if(extracandy)
                UseItem("RARE CANDY", "NIDOKING");

            PickupItemAt(5, 13); // secret key
            Dig();
            Fly("CinnabarIsland");

            UseItem("BICYCLE");
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
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("sabrina", () => {
            ClearText();
            Dig();
            UseItem("ELIXER", "NIDOKING");
            UseItem("BICYCLE");

            MoveTo(18, 18, 10);
            UseItem("BICYCLE");

            // SABRINA
            TalkTo("SaffronGym", 9, 8);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("EARTHQUAKE"));
        });

        // ClearCache();
        CacheState("erika", () => {
            ClearText();
            MoveTo(1, 5);
            Dig();

            UseItem("BICYCLE");

            CutAt(35, 32);
            CutAt("CeladonGym", 2, 4);

            // BEAUTY
            MoveTo(3, 4);
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM"));

            // ERIKA
            TalkTo(4, 3);
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("ICE BEAM"));
        });

        // ClearCache();
        CacheState("giovanni", () => {
            ClearText();
            CutAt(5, 7);
            MoveTo("CeladonCity", 12, 28);

            Fly("ViridianCity");
            UseItem("BICYCLE");

            // COOLTRAINER
            MoveTo("ViridianGym", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM"));

            // BLACKBELT
            MoveTo(10, 4);
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            MoveTo("ViridianCity", 32, 8);

            // GIOVANNI
            TalkTo("ViridianGym", 2, 1);
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM"), new RbyTurn("FISSURE"));
            ForceTurnAndSplit(new RbyTurn("ICE BEAM"));
        });

        // ClearCache();
        CacheState("victoryroad", () => {
            ClearText();
            MoveTo("ViridianCity", 32, 8);
            UseItem("SUPER REPEL");
            if(!ppup) UseItem("ELIXER", "NIDOKING");
            UseItem("BICYCLE");

            // VIRIDIAN RIVAL
            MoveTo("Route22", 29, 5);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("AGILITY"));
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
            Execute("U"); // todo

            MoveTo(21, 15, Action.Right);
            PushBoulder(Joypad.Right);
            Execute("R R");
            FallDown();

            // VR menu
            Strength();
            // if(ppup) UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            Execute("D R R U");
            PushBoulder(Joypad.Left, 14);

            PickupItemAt("VictoryRoad2F", 26, 7);

            MoveTo("VictoryRoad2F", 29, 7);
            MoveAndSplit(Joypad.Right);
            AdvanceFrames(20);
        });

        // ClearCache();
        CacheState("lorelei", () => {
            AfterMoveAndSplit();
            if(!cutter.StartsWith("Dux")) {
                TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
                Deposit("SQUIRTLE", cutter.ToUpper());
                MoveTo("IndigoPlateauLobby", 8, 0);
                PartySwap("NIDOKING", "SPEAROW");
            } else {
                MoveTo("IndigoPlateauLobby", 8, 0);
            }

            // LORELEI
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
            if(cutter.StartsWith("Dux"))
                BattleSwitch("FARFETCH'D", new RbyTurn("AURORA BEAM"));
            else
                ForceTurn(new RbyTurn("PECK"), new RbyTurn("AURORA BEAM"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn(ppup ? "HORN DRILL" : "EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("bruno", () => {
            ClearText();
            Execute("U U U");
            UseItem("ELIXER", "NIDOKING");

            // BRUNO
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("RAGE"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("agatha", () => {
            ClearText();
            Execute("U U U");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("ELIXER", "NIDOKING");

            // AGATHA
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM"), new RbyTurn("WING ATTACK"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("EARTHQUAKE"));
        });

        // ClearCache();
        CacheState("lance", () => {
            ClearText();
            Execute("U U U");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");

            // LANCE
            MoveTo("LancesRoom", 5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("ICE BEAM"));
        });

        // ClearCache();
        CacheState("champion", () => {
            ClearText();
            // UseItem("POTION", "NIDOKING");
            Execute("U U");

            // CHAMPION
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn(ppup ? "HORN DRILL" : "EARTHQUAKE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("end", () => {
            ClearText();
            ClearText(26);
            AdvanceFrames(164);
        });

        Timer.Stop();
        AdvanceFrames(600);
        Dispose();
    }
}
