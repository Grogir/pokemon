public class RedClassic : RedBlueForceComparisons
{
    public enum XP { Hiker, Zubat, None };
    public enum Cutter { Paras, Oddish, Dux }
    public RedClassic(Cutter cutter = Cutter.Paras, XP xp = XP.Hiker, bool route6xp = false, bool silphbar = true, bool ppup = true, bool extracandy = false)
    {
        // RecordAndTime("red-classic");
        // LoadState("basesaves/red/classicfte_" + cutter + ".gqs");
        // CpuWriteBE<ushort>("wPartyMon1DVs", 0xffe0);
        RbyTurn.DefaultRoll = 39;

        // ClearCache();
        CacheState("newgame", () => {
            new RbyIntroSequence(RbyStrat.NoPal, RbyStrat.GfSkip, RbyStrat.Hop0, RbyStrat.Title0).Execute(this);
            Press(Joypad.Down | Joypad.A, Joypad.Left, Joypad.Down, Joypad.Left, Joypad.B, Joypad.A); // Options
        });

        Timer.Start();

        ClearCache();
        CacheState("rival1", () => {
            ClearText();
            Press(Joypad.A, Joypad.None, Joypad.A, Joypad.Start); // Name self
            ClearText();
            Press(Joypad.A, Joypad.None, Joypad.A, Joypad.Start); // Name rival
            ClearText(); // Journey begins!

            MoveTo("PalletTown", 10, 1); // Oak cutscene
            ClearText();

            TalkTo(7, 3);
            Yes();
            ClearText();
            Yes();
            Press(Joypad.None, Joypad.A, Joypad.Start); // Name Squirtle
            ForceGiftDVs(0xe0ee);
            ClearText(); // Squirtle received

            MoveTo(5, 6);
            ClearText();

            // RIVAL1
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("TACKLE"));
            ClearText();
            MoveTo(5, 10); // pathfinding doesnt like standing there (todo)
        });

        // ClearCache();
        CacheState("nidoran", () => {
            MoveTo("Route1", 11, 24);
            MoveTo("Route1", 13, 14);
            MoveTo("Route1", 14, 8);
            ForceEncounter(Action.Up, 3, 0x0000);
            ClearText();
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TAIL WHIP"));
            ForceTurn(new RbyTurn("TACKLE"));

            MoveTo("ViridianCity", 21, 30);
            MoveTo("ViridianCity", 29, 19);
            ClearText(); // Receive parcel

            MoveTo("Route1", 8, 6);
            MoveTo("Route1", 9, 14);
            MoveTo("Route1", 16, 22);
            MoveTo("Route1", 16, 24);
            MoveTo("Route1", 16, 26);
            MoveTo("Route1", 14, 26);
            MoveTo("Route1", 14, 28);
            MoveTo("Route1", 10, 28);

            TalkTo("OaksLab", 5, 2, Action.Right); // give parcel

            MoveTo("Route1", 11, 24);
            MoveTo("Route1", 13, 14);
            MoveTo("ViridianCity", 21, 30);

            TalkTo("ViridianMart", 1, 5);
            Buy("POKE BALL", 7);

            MoveTo("Route22", 33, 12);
            ForceEncounter(Action.Up, 1, 0x7fe0);
            ClearText();
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("LEER"));
            ForceYoloball("POKE BALL");
            ClearText();
            Yes();
            Press(Joypad.None, Joypad.A, Joypad.Start); // nido nickname
            RunUntil("EnterMap");
        });

        // ClearCache();
        CacheState("forest", () => {
            PickupItemAt("ViridianCity", 14, 4, Action.Left);
            MoveTo(13, 10, 52);
            MoveTo(10, 46);

            MoveTo("ViridianForest", 26, 42); // safe path
            MoveTo(26, 34);
            MoveTo(27, 32);
            MoveTo(27, 20);

            PickupItemAt("ViridianForest", 25, 11);
            MoveTo(17, 16);
            MoveTo(13, 3);
            MoveTo(7, 22);

            // WEEDLE GUY
            TalkTo("ViridianForest", 2, 18);
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
        });

        // ClearCache();
        CacheState("brock", () => {
            MoveTo("PewterCity", 18, 35);
            PartySwap("SQUIRTLE", "NIDORANM");

            // BROCK
            TalkTo("PewterGym", 4, 1);
            BattleSwitch("SQUIRTLE", new RbyTurn("DEFENSE CURL"));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("DEFENSE CURL"));
            ForceTurn(new RbyTurn("BUBBLE"));
            Yes();
            SendOut("NIDORANM");
            BattleSwitch("SQUIRTLE", new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("TACKLE"));
            ForceTurnAndSplit(new RbyTurn("BUBBLE"), new RbyTurn("TACKLE"));
        });

        // ClearCache();
        CacheState("route3", () => {
            ClearText();

            MoveTo("PewterMart", 3, 5);
            SetOptions(Fast | Off | Set);
            TalkTo("PewterMart", 1, 5);
            Buy("POTION", 8);

            // BUG CATCHER 1
            MoveTo("Route3", 11, 6);
            ClearText();
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));

            TossItem("ANTIDOTE");
            UseItem("POTION", "NIDORANM");

            // SHORTS GUY
            TalkTo(14, 4);
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("TACKLE"));
            MoveSwap("LEER", "HORN ATTACK");
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // BUG CATCHER 2
            TalkTo(19, 5);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("TACKLE"));

            // BUG CATCHER 3
            TalkTo(24, 6);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            MoveTo(36, 10);
            ForceEncounter(Action.Right, 3, 0x0000); // spearow
            ClearText();
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("GROWL"));
            ForceYoloball("POKE BALL");
            ClearText();
            No();

            TalkTo(68, 3, 2);
            Yes();
            ClearText(); // healed at center

            MoveTo("Route4", 18, 6);
            MoveAndSplit(Joypad.Up);
        });

        // ClearCache();
        CacheState("mtmoon", () => {
            AfterMoveAndSplit();
            if(xp == XP.Hiker) PickupItemAt(59, 5, 32);

            TalkTo(24, 31, Action.Left);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCREECH"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCREECH"));

            PickupItemAt(35, 31);
            PickupItemAt(36, 23);

            MoveTo(30, 7, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("ABSORB"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("ABSORB"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GROWTH"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            Evolve();

            MoveTo(11, 16);
            ForceEncounter(Action.Up, 2, 0);
            ClearText();
            if(xp == XP.Zubat) ForceTurn(new RbyTurn("HORN ATTACK")); else RunAway();
            MoveTo(11, 13);
            ForceEncounter(Action.Up, 2, 0);
            ClearText();
            if(xp == XP.Zubat) ForceTurn(new RbyTurn("HORN ATTACK")); else RunAway();
            MoveTo(11, 10);
            ForceEncounter(Action.Up, 2, 0);
            ClearText();
            if(xp == XP.Zubat) ForceTurn(new RbyTurn("HORN ATTACK")); else RunAway();
            MoveTo(11, 6);

            if(xp == XP.Hiker)
            {
                UseItem("TM12", "NIDORINO", "LEER");
                TalkTo(5, 6);
                ForceTurn(new RbyTurn("WATER GUN"));
                ForceTurn(new RbyTurn("WATER GUN"));
                ForceTurn(new RbyTurn("WATER GUN"));
            }

            PickupItemAt(2, 2, Action.Left);

            if(cutter == Cutter.Paras)
            {
                MoveTo(61, 10, 25);
                ForceEncounter(Action.Up, 5, 0xffff); // paras
                ClearText();
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCRATCH"));
                ForceYoloball("POKE BALL");
                ClearText();
                No();
                ClearText();
            }

            MoveTo(61, 10, 17);
            UseItem("MOON STONE", "NIDORINO");

            // MOON ROCKET
            Execute("R");
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SUPERSONIC"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // NERD
            TalkTo(12, 8);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("POUND"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCREECH"));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            TalkTo(13, 6);
            Yes();
            ClearText(); // helix fossil picked up

            MoveTo(3, 7);
            MoveTo("MtMoonB1F", 26, 3);
            MoveAndSplit(Joypad.Right);
        });

        // ClearCache();
        CacheState("bridge", () => {
            AfterMoveAndSplit();
            MoveNpc("CeruleanCity", 15, 18, Action.Down); // good npc
            MoveTo("CeruleanCity", 14, 18);
            TalkTo("CeruleanPokecenter", 3, 2);
            Yes();
            ClearText(); // healed at center

            PickupItemAt("CeruleanCity", 15, 8);

            MoveTo("CeruleanCity", 21, 6, Action.Up);

            // BRIDGE RIVAL
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TAIL WHIP"));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // BUG CATCHER
            TalkTo("Route24", 11, 31);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // LASS
            TalkTo(10, 28);
            if(xp == XP.Hiker)
            {
                ForceTurn(new RbyTurn("HORN ATTACK"));
            }
            else
            {
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
                ForceTurn(new RbyTurn("POISON STING"));
            }
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCRATCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // YOUNGSTER
            TalkTo(11, 25);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("QUICK ATTACK", xp == XP.Hiker ? 38 : 39));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            if(xp == XP.Hiker)
            {
                UseItem("RARE CANDY", "NIDOKING");
                UseItem("RARE CANDY", "NIDOKING");
                TeachLevelUpMove("TACKLE");

                // LASS
                TalkTo(10, 22);
                MoveSwap("HORN ATTACK", "THRASH");
                ForceTurn(new RbyTurn("THRASH"));
                ForceTurn(new RbyTurn("THRASH"));

                // MANKEY GUY
                TalkTo(11, 19);
                ForceTurn(new RbyTurn("THRASH"));

                // BRIDGE ROCKET
                MoveTo(10, 15);
                ClearText();
                ForceTurn(new RbyTurn("THRASH"));
                ForceTurnAndSplit(new RbyTurn("THRASH"));
            }
            else if(xp == XP.Zubat)
            {
                // LASS
                TalkTo(10, 22);
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
                ForceTurn(new RbyTurn("POISON STING"));
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("POISON STING"));
                ForceTurn(new RbyTurn("HORN ATTACK"));

                UseItem("RARE CANDY", "NIDOKING");
                UseItem("RARE CANDY", "NIDOKING");
                TeachLevelUpMove("TACKLE");

                // MANKEY GUY
                TalkTo(11, 19);
                MoveSwap("HORN ATTACK", "THRASH");
                ForceTurn(new RbyTurn("THRASH"));

                // BRIDGE ROCKET
                MoveTo(10, 15);
                ClearText();
                ForceTurn(new RbyTurn("THRASH"));
                ForceTurnAndSplit(new RbyTurn("THRASH"));
            }
            else
            {
                // LASS
                TalkTo(10, 22);
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
                ForceTurn(new RbyTurn("POISON STING"));
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("POISON STING"));
                ForceTurn(new RbyTurn("HORN ATTACK"));

                // MANKEY GUY
                TalkTo(11, 19);
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("LEER"));
                ForceTurn(new RbyTurn("POISON STING"));

                UseItem("RARE CANDY", "NIDOKING");
                UseItem("RARE CANDY", "NIDOKING");
                TeachLevelUpMove("TACKLE");

                // BRIDGE ROCKET
                MoveTo(10, 15);
                ClearText();
                MoveSwap("HORN ATTACK", "THRASH");
                ForceTurn(new RbyTurn("THRASH"));
                ForceTurnAndSplit(new RbyTurn("THRASH"));
            }
        });

        // ClearCache();
        CacheState("bill", () => {
            ClearText();

            if(xp == XP.Hiker)
            {
                // BOTTOM HIKER
                MoveTo("Route25", 14, 7);
                ClearText();
                ForceTurn(new RbyTurn("WATER GUN"));
            }
            else
            {
                // TOP HIKER
                TalkTo("Route25", 8, 4);
                ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("KARATE CHOP"));
                ForceTurn(new RbyTurn("POISON STING"));
                ForceTurn(new RbyTurn("THRASH"), new RbyTurn("TACKLE"));
                ForceTurn(new RbyTurn("THRASH"), new RbyTurn("DEFENSE CURL"));
                ForceTurn(new RbyTurn("THRASH"));
            }

            // LASS
            TalkTo(18, 8, Action.Down);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // JR. TRAINER
            MoveTo(24, 6);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // ODDISH LASS
            TalkTo(37, 4);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("BillsHouse", 6, 5, Action.Right);
            Yes();
            ClearText();
            TalkTo(1, 4);
            TalkTo(4, 4);

            // Bill menu
            UseItem("POTION", "NIDOKING");
            if(xp != XP.Hiker) UseItem("POTION", "NIDOKING");
            UseItem("ESCAPE ROPE");
        });

        // ClearCache();
        CacheState("misty", () => {
            // GOLDEEN GIRL
            MoveTo("CeruleanGym", 5, 3);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("PECK"));
            ForceTurn(new RbyTurn("THRASH"));

            // MISTY
            TalkTo(4, 2);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("BUBBLEBEAM"));
            ForceTurnAndSplit(new RbyTurn("THRASH"));
        });

        // ClearCache();
        CacheState("boat", () => {
            ClearText();

            // DIG ROCKET
            MoveTo("CeruleanCity", 30, 9);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo("Route6", 17, 18);
            ForceEncounter(Action.Down, 1, 0x0000);
            ClearText();
            if(route6xp)
                ForceTurn(new RbyTurn("HORN ATTACK"));
            else
                RunAway();

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

            // Mart (early)
            TalkTo("VermilionMart", 1, 5);
            Sell("POKE BALL", 0, "TM34", 1, "NUGGET", 1);
            Buy("REPEL", 3, "PARLYZ HEAL", 4);

            MoveTo("VermilionCity", 18, 30);
            ClearText();

            // BOAT RIVAL
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            // ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST", Miss));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            // ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE", Miss));
            ForceTurn(new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("THRASH"));
            // ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("VINE WHIP", Crit | 1));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("SSAnneCaptainsRoom", 4, 2); // hm01 received

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText(); // watch cutscene
        });

        // ClearCache();
        CacheState("surge", () => {
            // Cut menu
            MoveTo("VermilionCity", 15, 17, Action.Down);
            UseItem("TM11", "NIDOKING", "POISON STING");
            ItemSwap("POTION", "REPEL");
            UseItem("HM01", "PARAS");
            UseItem("TM28", "PARAS");
            Cut();

            MoveTo("VermilionGym", 1, 11, Action.Up);
            ForceCan();
            MoveTo("VermilionGym", 3, 12);
            Press(Joypad.Up);
            ForceCan();

            // SURGE
            TalkTo(5, 1);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("THRASH"));
        });

        // ClearCache();
        CacheState("route9", () => {
            ClearText();
            CutAt("VermilionCity", 15, 18);
            TalkTo("PokemonFanClub", 3, 1);
            Yes();
            ClearText();
            Dig();
            ClearText();

            TalkTo("BikeShop", 6, 3);

            // Bike menu
            MoveTo("CeruleanCity", 13, 26);
            ItemSwap("HELIX FOSSIL", "BICYCLE");
            UseItem("TM24", "NIDOKING", xp == XP.Hiker ? "WATER GUN" : "LEER");
            UseItem("BICYCLE");

            CutAt(19, 28);
            CutAt("Route9", 5, 8);

            // 4 TURN THRASH GIRL
            TalkTo(13, 10);
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
            MoveAndSplit(Joypad.Up);
        });

        // ClearCache();
        CacheState("rocktunnel", () => {
            AfterMoveAndSplit();
            MoveTo("RockTunnel1F", 15, 4);
            UseItem("REPEL");

            // POKEMANIAC 1
            TalkTo("RockTunnel1F", 23, 8);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

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
            MoveAndSplit(Joypad.Down);
        });

        // ClearCache();
        CacheState("fly", () => {
            AfterMoveAndSplit();
            PickupItemAt(21, 16, 53); // max ether

            // GAMBLER
            TalkTo("Route8", 46, 13);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo("UndergroundPathWestEast", 47, 2);

            UseItem("BICYCLE");
            PickupItemAt(21, 5, Action.Down); // elixer

            MoveTo("Route7", 5, 14);
            UseItem("BICYCLE");

            // Shopping
            TalkTo("CeladonMart2F", 5, 4);
            Buy("SUPER REPEL", 10, "SUPER POTION", 4);

            TalkTo("CeladonMartRoof", 12, 2);
            ChooseMenuItem(1); // get soda pop
            ClearText();
            Press(Joypad.A);
            ClearText();
            ChooseMenuItem(0); // get fresh water
            ClearText();
            Press(Joypad.A);
            ClearText();
            ChooseMenuItem(0); // get fresh water
            ClearText();

            TalkTo(5, 5);
            Yes();
            ChooseMenuItem(1); // trade soda pop
            ClearText();
            Press(Joypad.A);
            ClearText();
            Yes();
            ChooseMenuItem(0); // trade fresh water
            ClearText();

            TalkTo("CeladonMart5F", 5, 4);
            Buy("X ACCURACY", 13, "X SPECIAL", 3, "X SPEED", 5);

            TalkTo("CeladonMartElevator", 3, 0);
            ChooseListItem(0);

            MoveTo("CeladonCity", 8, 14);
            UseItem("BICYCLE");

            // Fly house
            CutAt("Route16", 34, 9);
            MoveTo("Route16", 17, 4);
            UseItem("BICYCLE");
            MoveTo("Route16FlyHouse", 2, 4);
            ReceiveItemAndSplit();
        });

        // ClearCache();
        CacheState("hideout", () => {
            ClearText();

            // Fly menu
            MoveTo("Route16", 7, 6);
            ItemSwap("S.S.TICKET", "X ACCURACY");
            UseItem("HM02", "SPEAROW");
            ScrollTo("TM13");
            Fly("CeladonCity");

            UseItem("TM13", "NIDOKING", "BUBBLEBEAM");
            UseItem("BICYCLE");

            // POSTER ROCKET
            TalkTo(135, 9, 5, Action.Up);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo(9, 4);

            MoveTo(201, 12, 11);
            AfterMoveAndSplit();
            MoveTo(10, 13);
            AfterMoveAndSplit();
            MoveTo(9, 16, Action.Left);
            MoveTo(11, 18);
            AfterMoveAndSplit();
            MoveTo(13, 25);

            // LIFT KEY ROCKET
            TalkTo(202, 11, 2);
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            TalkTo(11, 2);
            PickupItemAt(10, 2);

            MoveTo(201, 18, 16);
            AfterMoveAndSplit();
            PickupItemAt(20, 14);
            MoveTo(16, 13);
            AfterMoveAndSplit();

            MoveTo(200, 17, 11, Action.Left);
            AfterMoveAndSplit();
            MoveTo(4, 11, Action.Right);
            AfterMoveAndSplit();
            PickupItemAt(6, 12);
            MoveTo(11, 14);
            AfterMoveAndSplit();
            MoveTo(13, 18);
            AfterMoveAndSplit();
            MoveTo(13, 22);
            AfterMoveAndSplit();
            MoveTo(10, 25);
            AfterMoveAndSplit();

            MoveTo(24, 18);
            Execute("D D"); // bonk? todo check
            TalkTo(203, 1, 1, Action.Up);
            ChooseListItem(2);
            MoveTo(2, 1);

            MoveTo(25, 14);
            ItemSwap("POTION", "SUPER REPEL");
            UseItem("TM48", "NIDOKING", "HORN ATTACK");
            UseItem("SUPER REPEL");
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("TM07", "NIDOKING", "THRASH");

            // RIGHT GRUNT
            TalkTo(26, 12, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            // LEFT GRUNT
            TalkTo(23, 12);
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));

            // GIOVANNI
            AfterMoveAndSplit();
            Execute("U");
            TalkTo(25, 3);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("RAGE"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("flute", () => {
            ClearText();
            PickupItemAt(25, 2);
            Dig();
            Fly("LavenderTown");

            // LAVENDER RIVAL
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("EMBER"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

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

            // MAROWAK
            MoveTo(10, 16);
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM"));

            // ROCKET 1
            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));

            // ROCKET 2
            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            // ROCKET 3
            MoveTo(10, 7);
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));

            // Get Pokéflute
            TalkTo(10, 3);
            MoveTo(2, 1);
            Press(Joypad.Right);
            ReceiveItemAndSplit();
        });

        // ClearCache();
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
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));

                MoveTo(5, 7, Action.Right);
                UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");

                // SILPH ROCKET
                TalkTo("SilphCo11F", 3, 16);
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
                ForceTurn(new RbyTurn("HORN DRILL"));
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
            ItemSwap("PARLYZ HEAL", "RARE CANDY");
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
                ItemSwap("HM01", "TM26");
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
            ForceTurnAndSplit(new RbyTurn("ELIXER", "NIDOKING"), new RbyTurn("SELFDESTRUCT"));
        });

        ClearCache();
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
            ItemSwap("POTION", "X SPECIAL");
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
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn(ppup ? "THUNDERBOLT" : "ICE BEAM"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn(ppup ? "THUNDERBOLT" : "ICE BEAM"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn(ppup ? "HORN DRILL" : "EARTHQUAKE"));
            ForceTurn(new RbyTurn(ppup ? "HORN DRILL" : "EARTHQUAKE"));
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
            if(ppup) UseItem("ELIXER", "NIDOKING");
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
            if(cutter < Cutter.Dux) {
                TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
                Deposit("SQUIRTLE", cutter == Cutter.Paras ? "PARAS" : "ODDISH");
                MoveTo("IndigoPlateauLobby", 8, 0);
                PartySwap("NIDOKING", "SPEAROW");
            } else {
                MoveTo("IndigoPlateauLobby", 8, 0);
            }

            // LORELEI
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
            if(cutter >= Cutter.Dux)
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

        // RecordAndTime("red-classic");
        // ClearCache();
        CacheState("lance", () => {
            ClearText();
            Execute("U U U");
            // ItemSwap("BICYCLE", "X SPECIAL");
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
