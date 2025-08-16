public class YellowGlitchless : YellowForceComparisons
{
    public YellowGlitchless()
    {
        // ulong lasttime = 0;
        // CallbackHandler.SetCallback(SYM["VBlank"], gb => {
        //     if(gb.EmulatedSamples - lasttime > 40000) System.Diagnostics.Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} " + (gb.EmulatedSamples - lasttime) + " +" + (float)(gb.EmulatedSamples - lasttime - SamplesPerFrame) / SamplesPerFrame);
        //     lasttime = gb.EmulatedSamples;
        // });
        // RecordAndTime("yellow-glitchless");
        RbyTurn.DefaultRoll = 20;

        // ClearCache();
        CacheState("newgame", () => {
            new RbyIntroSequence(RbyStrat.NoPal, RbyStrat.GfSkip, RbyStrat.Hop0, RbyStrat.Title0).Execute(this);
            Press(Joypad.Down, Joypad.A, Joypad.Left, Joypad.Down, Joypad.Left, Joypad.Down, Joypad.Left, Joypad.Start, Joypad.A); // Options
        });

        Timer.Start();

        ClearCache();
        CacheState("rival1", () => {
            ClearText();
            Press(Joypad.A, Joypad.None, Joypad.A, Joypad.Start); // Name self

            ClearText();
            Press(Joypad.A, Joypad.None, Joypad.A, Joypad.Start); // Name rival
            ClearText(); // Journey begins!

            MoveTo("PalletTown", 10, 0); // Oak cutscene
            ClearText();

            TalkTo(7, 3);
            No();
            ForceGiftDVs(0xbaaa);
            ClearText();

            MoveTo(5, 6);
            ClearText();

            // RIVAL1
            ForceTurn(new RbyTurn("THUNDERSHOCK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("THUNDERSHOCK", Crit), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("THUNDERSHOCK"), new RbyTurn("TAIL WHIP"));
            ForceTurn(new RbyTurn("THUNDERSHOCK", Crit));
            MoveTo(5, 10);
        });

        // ClearCache();
        CacheState("nidoran", () => {
            MoveTo("Route1", 11, 24);
            MoveTo("Route1", 13, 14);

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
            MoveTo("Route1", 11, 28);
            MoveTo("Route1", 11, 34);

            TalkTo("OaksLab", 5, 2, Action.Down); // give parcel

            MoveTo("Route1", 8, 29);
            ForceEncounter(Action.Up, 6, 0x8888);
            RunAway();
            MoveTo("Route1", 11, 24);
            MoveTo("Route1", 13, 14);
            MoveTo("ViridianCity", 21, 30);

            TalkTo("ViridianMart", 1, 5);
            Buy("POKE BALL", 2, "POTION", 5);

            MoveTo("ViridianCity", 19, 9);
            ClearText();
            MoveTo("Route2", 7, 51, Action.Up);
            SaveAndQuit();

            NoPal.Execute(this, true);
            Execute(SpacePath("URAR"));
            ForceEncounter(Action.Up, 6, 0xf9ed);
            ForceYoloball("POKE BALL");
            ClearText();
            Yes();
            Press(Joypad.None, Joypad.A, Joypad.Start); // nido nickname
            RunUntil("EnterMap");
        });

        // ClearCache();
        CacheState("forest", () => {
            MoveTo("ViridianForest", 25, 32);
            PartySwap("PIKACHU", "NIDORANM");

            TalkTo(30, 33, Action.Down);
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"), false);
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE", 1), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));

            TalkTo(13, 17, Action.Down);
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK", Crit), new RbyTurn("HARDEN"));

            MoveTo(13, 13);
            UseItem("POTION", "NIDORANM");
            SaveAndQuit();
            NoPal.Execute(this, true);
            Execute("U");
            MoveTo(1, 20, Action.Up);
            ForceEncounter(Action.Up, 5, 0x8888);
            ForceYoloball("POKE BALL");
            ClearText();
            No();

            RunUntil("JoypadOverworld");
            PickupItem();
            MoveTo(1, 18);
            ClearText();
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
        });

        // ClearCache();
        CacheState("brock", () => {
            // TalkTo(58, 13, 3, Action.Up);
            // Deposit("PIKACHU");
            // TalkTo(3, 2);
            // Yes();
            // ClearText();

            TalkTo("PewterGym", 3, 6, Action.Up);
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("SCRATCH"));
            MoveSwap("LEER", "HORN ATTACK");
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCRATCH"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("SCRATCH"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("SCRATCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCRATCH", 39));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            MoveTo(4, 4);
            UseItem("POTION", "NIDORANM");

            // BROCK
            TalkTo("PewterGym", 4, 1);
            ForceTurn(new RbyTurn("DOUBLE KICK", Crit), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("DOUBLE KICK", Crit));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("TACKLE", Miss));
            ForceTurn(new RbyTurn("DOUBLE KICK"), new RbyTurn("BIDE", 2 * Turns));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("BIDE"));
            ForceTurnAndSplit(new RbyTurn("DOUBLE KICK"), new RbyTurn("BIDE"));
        });

        // ClearCache();
        CacheState("route3", () => {
            ClearText();

            // BUG CATCHER 1
            MoveTo("Route3", 11, 6);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE", 39));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("TACKLE", Miss), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("TACKLE"));

            // SHORTS GUY
            TalkTo(14, 4);
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("HORN ATTACK", Crit), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("DOUBLE KICK"));

            // BUG CATCHER 2
            TalkTo(19, 5);
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("TACKLE", Crit));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            Evolve();

            // BUG CATCHER 3
            TalkTo(24, 6);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            TalkTo(68, 13, 3, Action.Up);
            Deposit("PIKACHU");
            TalkTo(3, 2);
            Yes();
            ClearText();

            MoveTo("Route4", 18, 6);
            MoveAndSplit(Joypad.Up);
        });

        // ClearCache();
        CacheState("mtmoon", () => {
            AfterMoveAndSplit();
            Execute("U");
            SaveAndQuit();
            NoPal.Execute(this, true);
            Execute(SpacePath("UAUUUUUUUUAUUURRRARRRURUUUUUURARRDDDDDDDDDDDDRDDDDDRRRRRRURRR"));
            PickupItem();
            Execute(SpacePath("RAUUUAUUUUUUULUUAUUUUUUUUAUULLUUUUULULLLLLLLDDDLLLLDLLLDDLDDDADDDDDLLLLLLLALLLUULULLUUUUUUULAUUUU"));
            PickupItem();
            Execute(SpacePath("DRRRD"));
            Execute(SpacePath("DDDDDDDDRDDDDRRRARRRARRRARRRRRR"));
            Execute(SpacePath("RRUUURRRDDARRRRARRUAURARRARDADDDADDDDADDLLDDADDDADDALLLLLALLLALLLLLLLLLLLLLLLUUUUUAUUUUAUUAUUUURUUAUUAUUU"));

            UseItem("MOON STONE", "NIDORINO");

            // NERD
            TalkTo(12, 8);
            ForceTurn(new RbyTurn("HORN ATTACK", Crit), new RbyTurn("DISABLE", "LEER", 1 * Turns));
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("HORN ATTACK", Crit), new RbyTurn("TACKLE", Crit));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            TalkTo(13, 6);
            Yes();
            ClearText(); // helix fossil picked up

            MoveTo(9, 4);
            ForceEncounter(Action.Left, 8, 0x0000);
            RunAway();

            MoveTo(3, 5);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("HORN ATTACK", Crit));
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("HORN ATTACK", Crit), new RbyTurn("SMOG", Miss));
            ForceTurn(new RbyTurn("HORN ATTACK", Crit));

            MoveTo(3, 7);
            MoveTo("MtMoonB1F", 26, 3);
            MoveAndSplit(Joypad.Right);
        });

        // ClearCache();
        CacheState("bridge", () => {
            AfterMoveAndSplit();
            // TalkTo("CeruleanPokecenter", 3, 2);
            // Yes();
            // ClearText();

            PickupItemAt("CeruleanCity", 15, 8);
            MoveTo("CeruleanCity", 21, 6, Action.Up);

            // BRIDGE RIVAL
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("PECK"));
            ForceTurn(new RbyTurn("HORN ATTACK", Crit));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("DOUBLE KICK", Miss), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("DOUBLE KICK", Miss), new RbyTurn("HYPER FANG"));
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("DOUBLE KICK", Miss), new RbyTurn("GROWL", Miss));
            ForceTurn(new RbyTurn("DOUBLE KICK", Miss), new RbyTurn("SAND-ATTACK"));
            ForceTurn(new RbyTurn("DOUBLE KICK", Crit));

            // BUG CATCHER
            TalkTo("Route24", 11, 31);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("HORN ATTACK", Crit));

            // LASS
            TalkTo(10, 28);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK", Crit));

            // YOUNGSTER
            TalkTo(11, 25);
            ForceTurn(new RbyTurn("DOUBLE KICK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            TeachLevelUpMove("TACKLE");

            // LASS
            TalkTo(10, 22);
            // MoveSwap("THRASH", "HORN ATTACK");
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("SCRATCH", Crit));
            ForceTurn(new RbyTurn("THRASH"));

            // MANKEY GUY
            TalkTo(11, 19);
            ForceTurn(new RbyTurn("THRASH"));

            // BRIDGE ROCKET
            MoveTo(10, 15);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
        });

        // ClearCache();
        CacheState("bill", () => {
            ClearText();

            TalkTo(6, 5);
            Yes();
            ClearText();
            No();
            ClearText();

            // HIKER
            MoveTo("Route25", 14, 7);
            ClearText();
            ForceTurn(new RbyTurn("DOUBLE KICK", Crit));

            // LASS
            TalkTo(18, 8, Action.Down);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // JR TRAINER
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
        });

        // ClearCache();
        CacheState("misty", () => {
            TalkTo("CeruleanPokecenter", 3, 2);
            Yes();
            ClearText();

            // GOLDEEN GIRL
            MoveTo("CeruleanGym", 5, 3);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("SUPERSONIC", Miss));
            ForceTurn(new RbyTurn("THRASH"));

            // MISTY
            TalkTo(4, 2);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("WATER GUN", 30));
            ForceTurnAndSplit(new RbyTurn("THRASH"));
        });

        // ClearCache();
        CacheState("boat", () => {
            ClearText();

            // DIG ROCKET
            MoveTo("CeruleanCity", 30, 9);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", Crit));

            PickupItemAt(119, 3, 4); // full restore
            MoveTo("Route6", 17, 25);
            MoveTo(15, 28);

            // FEMALE JR. TRAINER
            TalkTo(11, 30, Action.Down);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH", Crit), new RbyTurn("QUICK ATTACK"));

            // MALE JR. TRAINER
            MoveTo(10, 31);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("VermilionMart", 1, 5);
            Sell("TM34", 1, "NUGGET", 1);
            Buy("REPEL", 3);

            MoveTo("VermilionCity", 18, 30);
            ClearText();

            // Boat menu
            MoveTo(95, 27, 2);
            UseItem("TM11", "NIDOKING", "LEER");
            UseItem("TM28", "CHARMANDER");

            // BOAT RIVAL
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM", Crit));
            MoveSwap("BUBBLEBEAM", "DOUBLE KICK");
            ForceTurn(new RbyTurn("DOUBLE KICK"));

            TalkTo("SSAnneCaptainsRoom", 4, 2); // hm01 received

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText(); // watch cutscene
        });

        // ClearCache();
        CacheState("surge", () => {
            // Cut menu
            MoveTo("VermilionCity", 15, 17, Action.Down);
            ItemSwap("POTION", "REPEL");
            UseItem("HM01", "CHARMANDER", "SCRATCH");
            Cut();

            MoveTo("VermilionGym", 5, 12);
            ForceCan();
            MoveTo("VermilionGym", 1, 7, Action.Left);
            // Press(Joypad.Left);
            ForceCan();

            // SURGE
            TalkTo(5, 1);
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("MEGA PUNCH"));
            ForceTurnAndSplit(new RbyTurn("THRASH"), new RbyTurn("MEGA KICK", 39));
        });

        // ClearCache();
        CacheState("route9", () => {
            ClearText();
            CutAt("VermilionCity", 15, 18);
            TalkTo("PokemonFanClub", 3, 1);
            Yes();
            ClearText();
            MoveTo("Route11", 4, 5);
            Dig();
            ClearText();

            TalkTo("BikeShop", 6, 3);

            // Bike menu
            MoveTo("CeruleanCity", 13, 26);
            ItemSwap("HELIX FOSSIL", "BICYCLE");
            UseItem("TM24", "NIDOKING", "DOUBLE KICK");
            UseItem("BICYCLE");

            CutAt(19, 28);
            CutAt("Route9", 5, 8);

            // 4 TURN THRASH GIRL
            TalkTo(13, 10);
            ForceTurn(new RbyTurn("THRASH", Crit));
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
            ForceTurn(new RbyTurn("THUNDERBOLT", Crit));

            // ODDISH GIRL
            TalkTo(14, 28);
            ForceTurn(new RbyTurn("THRASH", Crit));
            ForceTurn(new RbyTurn("THRASH", 1), new RbyTurn("POISONPOWDER"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo(34, 19);
            UseItem("REPEL");
            MoveTo(82, 11, 14);
            UseItem("REPEL");

            // HIKER
            TalkTo(232, 6, 10);
            ForceTurn(new RbyTurn("BUBBLEBEAM", Crit));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));

            // PIDGEY GIRL
            TalkTo("RockTunnel1F", 22, 24);
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("QUICK ATTACK"));

            MoveTo(15, 32);
            MoveAndSplit(Joypad.Down);
        });

        // ClearCache();
        CacheState("fly", () => {
            AfterMoveAndSplit();

            // GAMBLER
            TalkTo("Route8", 46, 13);
            MoveSwap("THRASH", "BUBBLEBEAM");
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("THRASH"));

            MoveTo("UndergroundPathWestEast", 47, 2);

            UseItem("BICYCLE");
            PickupItemAt(21, 5, Action.Down); // elixer

            MoveTo("Route7", 5, 14);
            UseItem("BICYCLE");

            // Shopping
            TalkTo("CeladonMart2F", 7, 3);
            Buy("TM07", 1);
            TalkTo("CeladonMart2F", 5, 4);
            Buy("SUPER REPEL", 10, "SUPER POTION", 4);

            TalkTo("CeladonMart4F", 5, 6);
            Buy("POKE DOLL", 1);

            TalkTo("CeladonMartRoof", 12, 2);
            ChooseMenuItem(0); // get fresh water
            ClearText();
            TalkTo("CeladonMartRoof", 12, 2);
            ChooseMenuItem(1); // get soda pop
            ClearText();

            TalkTo(5, 5);
            Yes();
            ChooseMenuItem(0);
            ClearText();
            TalkTo(5, 5);
            Yes();
            ChooseMenuItem(0);
            ClearText();

            TalkTo(12, 2, Action.Up);
            ChooseMenuItem(0); // get fresh water
            ClearText();

            TalkTo("CeladonMart5F", 5, 4);
            Buy("X ACCURACY", 11, "X SPEED", 4, "X ATTACK", 3, "X SPECIAL", 4);

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
        CacheState("flute", () => {
            ClearText();

            // Fly menu
            MoveTo("Route16", 7, 6);
            ItemSwap("S.S.TICKET", "TM07");
            UseItem("SUPER REPEL");
            UseItem("TM48", "NIDOKING", "BUBBLEBEAM");
            UseItem("TM07", "NIDOKING", "HORN ATTACK");
            // UseItem("TM48", "NIDOKING", "HORN ATTACK");
            // UseItem("TM07", "NIDOKING", "BUBBLEBEAM");
            ItemSwap("POTION", "X ACCURACY");
            UseItem("HM02", "PIDGEY");
            Fly("LavenderTown");

            // LAVENDER RIVAL
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"), new RbyTurn("QUICK ATTACK", 1));

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
            ForceTurn(new RbyTurn("ROCK SLIDE", Miss), new RbyTurn("NIGHT SHADE"));
            ForceTurn(new RbyTurn("ROCK SLIDE"));

            PickupItemAt(6, 8); // rare candy

            MoveTo(10, 16);
            ClearText();
            ItemSwap("FULL RESTORE", "SUPER REPEL");
            UseItem("POKE DOLL"); // escape ghost

            // ROCKET
            MoveTo("PokemonTower7F", 10, 12);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("PAY DAY"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));

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

            MoveTo("Route7Gate", 3, 3);
            ClearText();
            MoveTo("Route7", 18, 10);
            UseItem("BICYCLE");

            TalkTo(236, 3, 0);
            ChooseListItem(9);
            Execute("L D D D"); // exit elevator

            // MACHOKE
            MoveTo(4, 9);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("HORN DRILL"));

            // Get candy and EQ
            PickupItemAt(2, 12);
            PickupItemAt(4, 14);
            PickupItemAt(5, 11);

            MoveTo(233, 14, 2);
            ItemSwap("HM01", "TM26");
            UseItem("CARBOS", "NIDOKING");
            UseItem("TM26", "NIDOKING", "ROCK SLIDE");

            // ARBOK ROCKET
            MoveTo(210, 9, 16);
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            PickupItemAt(21, 16);
            MoveTo(233, 17, 14);
            ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");

            TalkTo(210, 7, 13);
            TalkTo("SilphCo3F", 17, 9);

            // SILPH RIVAL
            MoveTo("SilphCo7F", 3, 2, Action.Left);
            ClearText();
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("SLASH", Crit));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE", Crit));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            TalkTo(1, 5);
            No();
            ClearText();

            MoveTo(5, 7, Action.Right);
            Execute("D");
            ClearText();

            // J&J
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            // SILPH GIOVANNI
            TalkTo(6, 13, Action.Up);
            MoveTo(6, 13);
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE", Crit));
            ForceTurn(new RbyTurn("EARTHQUAKE", 1), new RbyTurn("PAY DAY"));
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn(AiItem)); // Guard Spec
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("STOMP"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("safari", () => {
            ClearText();
            Dig();
            UseItem("BICYCLE");

            // Snorlax menu
            MoveTo("Route16", 27, 10);
            UseItem("SUPER REPEL");
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("POKE FLUTE");
            RunAway();

            PickupItemAt("Route17", 15, 14); // candy

            // Post cycling menu
            MoveTo("Route18", 13, 7);
            MoveTo("Route18", 40, 8);
            UseItem("BICYCLE");

            MoveTo("SafariZoneGate", 3, 2);
            ClearText();
            Yes();
            ClearText();
            ClearText(); // sneaky joypad call

            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            // MoveTo(217, 4, 23);
            // UseItem("SUPER REPEL");
            // CloseMenu(Joypad.Up); // direction close

            PickupItemAt(217, 21, 10, Action.Down); //todo
            MoveTo(218, 22, 31);
            UseItem("SUPER REPEL");

            MoveTo(218, 7, 13);
            PickupItemAt("SafariZoneWest", 19, 7, Action.Down); // gold teeth

            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();
        });

        // ClearCache();
        CacheState("sabrina", () => {
            UseItem("BICYCLE");

            MoveTo(18, 18, 10);
            UseItem("BICYCLE");

            // SABRINA
            TalkTo("SaffronGym", 9, 8);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FLASH", Miss));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("koga", () => {
            ClearText();
            MoveTo(1, 5);
            Dig();

            Fly("FuchsiaCity");
            UseItem("BICYCLE");

            // JUGGLER 1
            TalkTo("FuchsiaGym", 7, 8);
            ForceTurn(new RbyTurn("EARTHQUAKE", Crit));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

            // JUGGLER 2
            MoveTo(1, 7);
            ClearText();
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"), new RbyTurn("DISABLE", Miss));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            // KOGA
            TalkTo(4, 10);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("SLEEP POWDER"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("PSYCHIC", 35));
            ForceTurn(new RbyTurn("POKE FLUTE"), new RbyTurn("PSYCHIC", 35 | SideEffect));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("blaine", () => {
            ClearText();
            MoveTo("FuchsiaCity", 5, 28);
            UseItem("BICYCLE");
            TalkTo("WardensHouse", 2, 3);

            MoveTo("FuchsiaCity", 27, 28);
            UseItem("SUPER REPEL");
            UseItem("ELIXER", "NIDOKING");
            UseItem("FULL RESTORE", "NIDOKING");
            UseItem("TM13", "NIDOKING", "THRASH");
            ItemSwap("S.S.TICKET", "X ATTACK");
            UseItem("HM04", "LAPRAS");
            UseItem("HM03", "LAPRAS");
            UseItem("RARE CANDY", "NIDOKING");
            MoveNpc("PalletTown", 3, 8, Action.Up); // good npc
            Fly("PalletTown");

            MoveTo(4, 13, Action.Down);
            Surf();

            MoveTo("CinnabarIsland", 4, 4);
            MoveTo(165, 5, 26); // todo
            UseItem("SUPER REPEL");
            TalkTo("PokemonMansion3F", 10, 5, Action.Up);
            ActivateMansionSwitch();
            MoveTo(16, 14);
            FallDown();

            PickupItemAt(18, 21); // carbos

            TalkTo("PokemonMansionB1F", 18, 25, Action.Up);
            ActivateMansionSwitch();

            TalkTo(20, 3, Action.Up);
            ActivateMansionSwitch();
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
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("X ATTACK"), new RbyTurn("STOMP"));
            ForceTurn(new RbyTurn("EARTHQUAKE", Crit));
            ForceTurnAndSplit(new RbyTurn("EARTHQUAKE"));
        });

        // ClearCache();
        CacheState("erika", () => {
            ClearText();
            Dig();
            UseItem("BICYCLE");

            CutAt(35, 32);
            CutAt("CeladonGym", 2, 4);

            // BEAUTY
            MoveTo(3, 4);
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM", Crit));

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
            ForceTurn(new RbyTurn("X ATTACK"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("EARTHQUAKE", Crit));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

            MoveTo("ViridianCity", 32, 8);

            // GIOVANNI
            TalkTo("ViridianGym", 2, 1);
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("DIG"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("DIG"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("victoryroad", () => {
            ClearText();
            MoveTo("ViridianCity", 32, 8);
            UseItem("SUPER REPEL");
            UseItem("SUPER POTION", "NIDOKING");
            ItemSwap("X ATTACK", "X SPECIAL");
            UseItem("CARBOS", "NIDOKING");
            UseItem("BICYCLE");

            // VIRIDIAN RIVAL
            MoveTo("Route22", 29, 5);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SWIFT"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));

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

            // MoveTo(7, 90);
            // Press(Joypad.None, Joypad.Right, Joypad.None);
            // PickupItem(); // max ether

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
            ForceEncounter(Action.Up, 3, 0);
            RunAway();
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
            // UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            Execute("D R R U");
            PushBoulder(Joypad.Left, 14);

            PickupItemAt(198, 26, 8, Action.Left);

            MoveTo("VictoryRoad2F", 29, 7);
            MoveAndSplit(Joypad.Right);
            AdvanceFrames(20);
        });

        // ClearCache();
        CacheState("lorelei", () => {
            AfterMoveAndSplit();
            TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
            Deposit("PIDGEY", "CHARMANDER", "LAPRAS");

            TalkTo(7, 6);
            Yes();
            ClearText();

            MoveTo("IndigoPlateauLobby", 8, 0);

            // LORELEI
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("BUBBLEBEAM", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("bruno", () => {
            ClearText();
            Execute("U U U");
            UseItem("ELIXER", "NIDOKING");
            // UseItem("MAX ETHER", "NIDOKING", "HORN DRILL");

            // BRUNO
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("DOUBLE TEAM"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("ICE BEAM", Crit));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("agatha", () => {
            ClearText();
            Execute("U U U");

            // AGATHA
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("MEGA DRAIN"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM", Crit));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("EARTHQUAKE"));
        });

        // ClearCache();
        CacheState("lance", () => {
            ClearText();
            Execute("U U U");

            // LANCE
            MoveTo("LancesRoom", 5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("FULL RESTORE"), new RbyTurn("SLAM"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SLAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM", Crit));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("ICE BEAM"));
        });

        // ClearCache();
        CacheState("champion", () => {
            ClearText();
            UseItem("SUPER POTION", "NIDOKING");
            Execute("U U");

            // CHAMPION
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("HYPNOSIS"));
            ForceTurn(new RbyTurn("POKE FLUTE"), new RbyTurn("HYPNOSIS", Miss));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("EARTHQUAKE", Crit));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurnAndSplit(new RbyTurn("EARTHQUAKE"));
        });

        // ClearCache();
        CacheState("end", () => {
            ClearText();
            ClearText(26);
            AdvanceFrames(166);
        });

        Timer.Stop();
        AdvanceFrames(600);
        Dispose();
    }
}
