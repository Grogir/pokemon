public class RedGlitchlessBlizzVenu : RedBlueForceComparisons
{
    public RedGlitchlessBlizzVenu(bool silphbar = true, bool latedrill = true)
    {
        // RecordAndTime("red-glitchless-doublemaxether");
        // RecordAndTime("red-glitchless-ether");
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
            ForceGiftDVs(0xe0ff);
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
            Buy("POKE BALL", 3);
            MoveTo("ViridianCity", 27, 18);

            MoveTo("ViridianCity", 7, 18, Action.Left);
            SaveAndQuit();

            NoPal.Execute(this, true);
            Execute(SpacePath("LLLULLUAULALDLDLLDADDADLALLALUUA"));
            ForceEncounter(Action.Up, 3, 0xffef);
            ForceYoloball("POKE BALL");
            ClearText();
            Yes();
            Press(Joypad.None, Joypad.A, Joypad.Start); // nido nickname
            RunUntil("EnterMap");
        });

        // ClearCache();
        CacheState("forest", () => {
            MoveNpc("ViridianCity", 13, 20, Action.Right);
            MoveNpc("ViridianCity", 17, 5, Action.Right);
            Execute(SpacePath("DRRUUURRRRRRRRRRRRRRRRRRRRRURUUUUUURUUUULUUUUUUAUUUUUUUUUUUUULLLUUUUUUUUUUURR"));
            ForceEncounter(Action.Right, 4, 0x8cfa);
            ForceYoloball("POKE BALL");
            ClearText();
            No(); // pidgey caught

            Execute(SpacePath("RUULLLLLUUURUUUUUUUUUUURRRRRURRRUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU"));
            PickupItem();
            Execute(SpacePath("UUUALLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDDDLDLLLLUU"));

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

            TalkTo("PewterMart", 1, 5);
            Buy("POTION", 7);

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
            MoveTo(4, 10);
            SetOptions(Fast | Off | Set);

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
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("LEER"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // BUG CATCHER 2
            TalkTo(19, 5);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("POISON STING"));
            ForceTurn(new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // BUG CATCHER 3
            TalkTo(24, 6);
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            MoveTo(27, 11);
            SaveAndQuit();

            PalHold.Execute(this, true);
            Execute(SpacePath("RRRRRRRRURRUUUUUARRRRRRRRRRRRDDDDDRRRRRRRARUURRUUUUUUUUUURRRRUUUUUUUUUURRRRR"));
            MoveAndSplit(Joypad.Up);
        });

        // ClearCache();
        CacheState("mtmoon", () => {
            AfterMoveAndSplit();
            Execute(SpacePath("UUUUUULLLLLALLLLDD"));
            PickupItem();
            Execute(SpacePath("RRRRUURRRARRUUUUUUURRRRRRRAUUUUUUURRRDRDDDDDDDADDDDDDDDADRRRRRURRRR"));
            PickupItem();
            Execute(SpacePath("UUUUUUUUR"));
            PickupItem();
            Execute(SpacePath("ULUUUUUAUUUUUULLLUUUUUUUULLLLLLDDLALLLLLLLDDDDDD"));
            Execute(SpacePath("LALLALLALLALDD"));
            Execute(SpacePath("RRRUUULAUR"));
            PickupItem();
            Execute(SpacePath("DDADLALLAD"));
            Execute(SpacePath("RARRARRARRARUU"));
            Execute(SpacePath("DDLDDDDLLLLLLLULUUUUULUUUUUUUULLLUL"));
            PickupItem();
            Execute(SpacePath("DADDRAR"));
            Execute(SpacePath("DRRDDDDDDDDDDRRRARRRRRRRRRRDR"));
            Execute(SpacePath("RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUU"));

            ForceEncounter(Action.Up, 5, 0x0000); // paras
            ForceYoloball("POKE BALL");
            ClearText();
            No();
            ClearText();

            UseItem("POTION", "NIDORANM");
            UseItem("POTION", "NIDORANM");

            // MOON ROCKET
            MoveTo("MtMoonB2F", 11, 17);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TAIL WHIP"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TAIL WHIP"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SUPERSONIC"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // Moon menu
            MoveTo(12, 9);
            UseItem("RARE CANDY", "NIDORANM");
            RunUntil("Evolution_PartyMonLoop.done");
            UseItem("TM12", "NIDORINO", "TACKLE");
            UseItem("MOON STONE", "NIDORINO");
            UseItem("TM01", "NIDOKING", "LEER");

            // NERD
            TalkTo(12, 8);
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("POUND"));
            ForceTurn(new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

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

            MoveTo("BikeShop", 2, 6);
            TalkTo(6, 3);
            No();
            ClearText(); // got instant text

            PickupItemAt("CeruleanCity", 15, 8);

            MoveTo("CeruleanCity", 21, 6, Action.Up);

            // BRIDGE RIVAL
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("VINE WHIP"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // BUG CATCHER
            TalkTo("Route24", 11, 31);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            // LASS
            TalkTo(10, 28);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCRATCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // YOUNGSTER
            TalkTo(11, 25);
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            // LASS
            TalkTo(10, 22);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCRATCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // MANKEY GUY
            TalkTo(11, 19);
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            // BRIDGE ROCKET
            MoveTo(10, 15);
            ClearText();
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurnAndSplit(new RbyTurn("MEGA PUNCH"));
        });

        // ClearCache();
        CacheState("bill", () => {
            ClearText();

            // HIKER
            MoveTo("Route25", 14, 7);
            ClearText();
            MoveSwap("MEGA PUNCH", "WATER GUN");
            ForceTurn(new RbyTurn("WATER GUN"));

            // LASS
            TalkTo(18, 8, Action.Down);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("SCRATCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"));

            // HIKER
            TalkTo(23, 9);
            ForceTurn(new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("WATER GUN"));

            // ODDISH LASS
            TalkTo(37, 4);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            TeachLevelUpMove("WATER GUN");

            PickupItemAt(38, 3); // ether

            TalkTo("BillsHouse", 6, 5, Action.Right);
            Yes();
            ClearText();
            TalkTo(1, 4);
            TalkTo(4, 4);

            // Bill menu
            UseItem("POTION", "NIDOKING");
            UseItem("POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("ESCAPE ROPE");
        });

        // ClearCache();
        CacheState("misty", () => {
            TalkTo("BikeShop", 6, 3);
            No();
            ClearText(); // got instant text

            // DIG ROCKET
            MoveTo("CeruleanCity", 30, 9);
            ClearText();
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"));

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
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("HORN ATTACK"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("HORN ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("THRASH"));
            ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("THRASH"));

            TalkTo("SSAnneCaptainsRoom", 4, 2); // hm01 received

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText(); // watch cutscene
        });

        // ClearCache();
        CacheState("surge", () => {
            TalkTo("VermilionMart", 1, 5);
            Sell("TM34", 1, "NUGGET", 1);
            Buy("REPEL", 4, "PARLYZ HEAL", 2);

            // Cut menu
            MoveTo("VermilionCity", 15, 17, Action.Down);
            UseItem("TM11", "NIDOKING", "POISON STING");
            UseItem("HM01", "PARAS");
            UseItem("TM28", "PARAS");
            Cut();

            // Manip
            MoveTo(15, 19);
            SaveAndQuit();

            NoPal.Execute(this, true);
            Execute(SpacePath("DLALLAURUUUUU"));
            ForceCan();
            MoveTo("VermilionGym", 4, 11);
            Press(Joypad.Left);
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
            ItemSwap("POTION", "BICYCLE");
            UseItem("TM24", "NIDOKING", "HORN ATTACK");
            UseItem("BICYCLE");

            CutAt(19, 28);
            CutAt("Route9", 5, 8);

            // 4 TURN THRASH GIRL
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
            // PickupItemAt(21, 16, 53); // max ether

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
            TalkTo("CeladonMart2F", 7, 3);
            Buy("TM07", 1);
            TalkTo("CeladonMart2F", 5, 4);
            Buy("SUPER REPEL", 9, "SUPER POTION", 3);

            TalkTo("CeladonMart4F", 5, 6);
            Buy("POKE DOLL", 2);

            TalkTo("CeladonMartRoof", 12, 2);
            ChooseMenuItem(1); // get soda pop
            ClearText();
            if(latedrill) {
                Press(Joypad.A);
                ClearText();
                ChooseMenuItem(0); // get fresh water
                ClearText();
            }

            TalkTo(5, 5);
            Yes();
            ChooseMenuItem(latedrill ? 1 : 0); // trade soda pop
            ClearText();

            if(!latedrill) {
                TalkTo(12, 2, Action.Up);
                ChooseMenuItem(0); // get fresh water
                ClearText();
            }

            TalkTo("CeladonMart5F", 5, 4);
            Buy("X ACCURACY", 11, "X SPECIAL", 6, "X SPEED", 2);

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
            if(latedrill) {
                ItemSwap("HELIX FOSSIL", "TM07");
                UseItem("SUPER REPEL");
                UseItem("HM02", "PIDGEY");
                UseItem("TM48", "NIDOKING", "MEGA PUNCH");
            } else {
                ItemSwap("HELIX FOSSIL", "TM07");
                UseItem("SUPER REPEL");
                UseItem("TM48", "NIDOKING", "MEGA PUNCH");
                ItemSwap("S.S.TICKET", "X ACCURACY");
                UseItem("HM02", "PIDGEY");
            }
            Fly("LavenderTown");

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
            if(latedrill)
                ItemSwap("S.S.TICKET", "X ACCURACY");
            else
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
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("TACKLE"));
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

            if(silphbar) {
                // ARBOK ROCKET
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
                if(latedrill) ItemSwap("HM01", "SUPER REPEL");
                ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("BUBBLEBEAM"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));

                MoveTo(5, 7, Action.Right);
                UseItem("ETHER", "NIDOKING", "HORN DRILL");

                // SILPH ROCKET
                TalkTo("SilphCo11F", 3, 16);
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
                ForceTurn(new RbyTurn("BUBBLEBEAM"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
            } else {
                // ARBOK ROCKET
                TalkTo(8, 16);
                ForceTurn(new RbyTurn("THRASH"), new RbyTurn("LEER"));
                ForceTurn(new RbyTurn("THRASH"));

                PickupItemAt(21, 16);
                TalkTo(7, 13);
                TalkTo("SilphCo3F", 17, 9);

                // SILPH RIVAL
                MoveTo("SilphCo7F", 3, 2, Action.Left);
                ClearText();
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("WHIRLWIND"));
                if(latedrill) ItemSwap("HM01", "SUPER REPEL");
                ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));
                ForceTurn(new RbyTurn("HORN DRILL"));

                MoveTo(5, 7, Action.Right);

                // SILPH ROCKET
                TalkTo("SilphCo11F", 3, 16);
                ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("FOCUS ENERGY"));
                ForceTurn(new RbyTurn("BUBBLEBEAM"));
                ForceTurn(new RbyTurn("ETHER", "NIDOKING", "HORN DRILL"), new RbyTurn("CONFUSION"));
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
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("safari", () => {
            ClearText();
            TalkTo(236, 3, 0);
            ChooseListItem(9);
            Execute("L D D D"); // exit elevator

            // Get candy and EQ
            PickupItemAt(234, 2, 12);
            PickupItemAt(234, 4, 14);
            Dig();

            UseItem("BICYCLE");

            // Snorlax menu
            MoveTo("Route16", 27, 10);
            UseItem("SUPER REPEL"); // REPEL
            ItemSwap("PARLYZ HEAL", "RARE CANDY");
            // ItemSwap("POTION", "X SPECIAL");
            UseItem("POKE FLUTE");
            RunAway();

            // PickupItemAt("Route17", 15, 14, Action.Down); // bugged (todo)
            PickupItemAt("Route17", 15, 14); // candy

            // Post cycling menu
            MoveTo("Route18", 13, 7);
            MoveTo("Route18", 40, 8);
            UseItem("SUPER REPEL"); // REPEL
            ItemSwap("POTION", "X SPECIAL");
            // ItemSwap("PARLYZ HEAL", "RARE CANDY");
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
        });

        // ClearCache();
        CacheState("koga", () => {
            Fly("FuchsiaCity");
            UseItem("BICYCLE");

            // JUGGLER 1
            TalkTo("FuchsiaGym", 7, 8);
            MoveSwap("HORN DRILL", "EARTHQUAKE");
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
        });

        // ClearCache();
        CacheState("erika", () => {
            ClearText();

            MoveTo("FuchsiaCity", 5, 28);
            UseItem("BICYCLE");

            TalkTo("WardensHouse", 2, 3);
            MoveTo("FuchsiaCity", 27, 28);
            MoveNpc("PalletTown", 3, 8, Action.Left); // good npc
            Fly("PalletTown");

            // Surf menu
            MoveTo(4, 17, Action.Right);
            UseItem("SUPER REPEL");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");
            // ItemSwap("HELIX FOSSIL", "X SPEED");
            UseItem("HM03", "SQUIRTLE");
            Surf();

            MoveTo("CinnabarIsland", 4, 4);
            TalkTo("PokemonMansion3F", 10, 5, Action.Up);
            ActivateMansionSwitch();
            MoveTo(16, 14);
            FallDown();

            // Blizzard menu
            PickupItemAt("PokemonMansionB1F", 19, 25);
            UseItem("HM04", "SQUIRTLE", "TACKLE");
            UseItem("TM14", "NIDOKING", "BUBBLEBEAM");
            UseItem("REPEL");

            TalkTo("PokemonMansionB1F", 18, 25, Action.Up);
            ActivateMansionSwitch();

            TalkTo(20, 3, Action.Up);
            ActivateMansionSwitch();
            PickupItemAt(10, 2); // candy
            PickupItemAt(5, 13); // secret key
            Dig();

            UseItem("BICYCLE");

            CutAt(35, 32);
            CutAt("CeladonGym", 2, 4);

            // BEAUTY
            MoveTo(3, 4);
            ClearText();
            ForceTurn(new RbyTurn("BLIZZARD"));

            // ERIKA
            TalkTo(4, 3);
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurnAndSplit(new RbyTurn("EARTHQUAKE"));
        });

        // ClearCache();
        CacheState("blaine", () => {
            ClearText();
            CutAt(5, 7);
            MoveTo("CeladonCity", 12, 28);

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
        CacheState("giovanni", () => {
            ClearText();
            MoveTo(1, 5);
            Dig();

            Fly("ViridianCity");
            UseItem("BICYCLE");

            // COOLTRAINER
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
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        });

        // ClearCache();
        CacheState("victoryroad", () => {
            ClearText();
            MoveTo("ViridianCity", 32, 8);
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            // VIRIDIAN RIVAL
            MoveTo("Route22", 29, 5);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("BLIZZARD"));

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
            UseItem("SUPER REPEL");
            UseItem("BICYCLE");

            Execute("D R R U");
            PushBoulder(Joypad.Left, 14);

            MoveTo("VictoryRoad2F", 29, 7);
            MoveAndSplit(Joypad.Right);
            AdvanceFrames(20);
        });

        // ClearCache();
        CacheState("lorelei", () => {
            AfterMoveAndSplit();
            TalkTo("IndigoPlateauLobby", 15, 8, Action.Up);
            Deposit("SQUIRTLE", "PARAS");

            MoveTo("IndigoPlateauLobby", 8, 0);
            PartySwap("NIDOKING", "PIDGEY");

            // LORELEI
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("GUST"), new RbyTurn("AURORA BEAM"));
            SendOut("NIDOKING");
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("REST"));
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

            // BRUNO
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X ACCURACY"), new RbyTurn("RAGE"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurnAndSplit(new RbyTurn("HORN DRILL"));
        });

        // ClearCache();
        CacheState("agatha", () => {
            ClearText();
            Execute("U U U");
            UseItem("SUPER POTION", "NIDOKING");
            UseItem("RARE CANDY", "NIDOKING");

            // AGATHA
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurn(new RbyTurn("EARTHQUAKE"));
            ForceTurnAndSplit(new RbyTurn("EARTHQUAKE"));
        });

        // ClearCache();
        CacheState("lance", () => {
            ClearText();
            Execute("U U U");
            MoveTo("LancesRoom", 6, 7);
            UseItem("ELIXER", "NIDOKING");
            UseItem("SUPER POTION", "NIDOKING");

            // LANCE
            MoveTo("LancesRoom", 5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HYDRO PUMP"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("AGILITY"));
            ForceTurn(new RbyTurn("BLIZZARD"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("BLIZZARD"));
        });

        // ClearCache();
        CacheState("champion", () => {
            ClearText();
            Execute("U U");

            // CHAMPION
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
