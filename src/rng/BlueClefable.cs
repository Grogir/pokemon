public class BlueClefable : RedBlueForceComparisons
{
    public BlueClefable(bool blue = true) : base(blue ? "roms/pokeblue.gbc" : "roms/pokered.gbc", true)
    {
        // ulong lasttime = 0;
        // CallbackHandler.SetCallback(SYM["VBlank"], gb => {
        //     if(gb.EmulatedSamples - lasttime > 40000) System.Diagnostics.Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} " + (gb.EmulatedSamples - lasttime) + " +" + (float)(gb.EmulatedSamples - lasttime - SamplesPerFrame) / SamplesPerFrame);
        //     lasttime = gb.EmulatedSamples;
        //     // System.Diagnostics.Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} " + gb.EmulatedSamples);
        // });
        RecordAndTime(blue ? "blue-clefable" : "red-clefable");
        RbyTurn.DefaultRoll = 39;

        // Record("test");
        // LoadState("basesaves/blue/lass.gqs");
        // new RbyIntroSequence(RbyStrat.NoPal).Execute(this);
        // AdvanceFrames(600);
        // return;

        // ClearCache();
        CacheState("newgame", () => {
            new RbyIntroSequence(RbyStrat.NoPal, RbyStrat.GfSkip, RbyStrat.Hop0, RbyStrat.Title0).Execute(this);
            Press(Joypad.Down | Joypad.A, Joypad.Left, Joypad.Down, Joypad.Left, Joypad.Down, Joypad.Left, Joypad.B, Joypad.A); // Options
        });

        Timer.Start();

        // ClearCache();
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
            ForceGiftDVs(0xffff);
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
        CacheState("brock", () => {
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

            MoveTo(0, 10, 0);
            SaveAndQuit();

            if(blue)
            {
                PalHold.Execute(this, true);
                Execute(SpacePath("UUUUUUUUALLUUUURRRRUAUULLLUUUUUURRRRRUUUUUUUUUUUUULLLUUUUUUUULLUUUUUUUUUUUUUUUUUUUUUUUUUUUULLUUUUUUUUUUUUUUUUULLLUUUUURRRRUUUUUUUULLLLLURUUUUUUU"));
                Execute(SpacePath("UUURURRRRRUURRUUUUUUUUUUUUUUUUUUUUUUUUUUUUU"));
                PickupItem();
                Execute(SpacePath("UUULLLLLLLLADDDDDDDLLLLUUUAUUUUUUUUUULLLLLLDDDDDDDDDDDDDDLDDDDADLLLLUUU"));
            }
            else
            {
                new RbyIntroSequence(RbyStrat.PalAB).Execute(this, true);
                Execute(SpacePath("UAUUUULULUUUUUURRRRUUULULLUURRUURARUURUUUUUUUUUUULLLUUUUULUUUUUULUUUUUUUUUUUUUUUUUUUUUUUUUULUULUUUUUUUUUUUUUUULLLUUUUURRRRUUUUUULLLLLUUUURUUUUUU"));
                Execute(SpacePath("UUUURRRRRURRRUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU"));
                PickupItem();
                Execute(SpacePath("UUULALLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDDDDDDADLLLLLUUU"));
            }

            // WEEDLE GUY
            Press(Joypad.A);
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT"));

            // BROCK
            TalkTo("PewterGym", 4, 1);
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("DEFENSE CURL"));
            ForceTurn(new RbyTurn("BUBBLE"));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("BIDE"));
            ForceTurnAndSplit(new RbyTurn("BUBBLE"), new RbyTurn("BIDE"));
        });

        // ClearCache();
        CacheState("route3", () => {
            ClearText();

            TalkTo("PewterMart", 1, 5);
            Sell("TM34", 1);
            Buy("POKE BALL", 4, "ANTIDOTE", 4, "POTION", 10);

            // BUG CATCHER 1
            MoveTo("Route3", 11, 6);
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));

            // UseItem("POTION", "SQUIRTLE");

            // SHORTS GUY
            TalkTo(14, 4);
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("TACKLE"));

            // LASS
            MoveTo(19, 4);
            ClearText();
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("TAIL WHIP"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("LEER", Miss));
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("LEER", Miss));

            // BUG CATCHER 3
            MoveTo(2, 39, 17);
            TalkTo(14, 24, 6);
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("STRING SHOT", Miss));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("HARDEN"));
            ForceTurn(new RbyTurn("BUBBLE"), new RbyTurn("HARDEN"));

            MoveTo(27, 9);
            ForceEncounter(Action.Down, 0, 0x0000); // pidgey
            ClearText();
            ForceTurn(new RbyTurn("TACKLE"), new RbyTurn("SAND-ATTACK"));
            ForceYoloball("POKE BALL");
            ClearText();
            No();

            if(blue)
            {
                MoveTo(15, 18, 6);
            }
            else
            {
                MoveTo(27, 11);
                SaveAndQuit();
                PalHold.Execute(this, true);
                Execute(SpacePath("RRRRRRRRURRUUUUUARRRRRRRRRRRRDDDDDRRRRRRRARUURRUUUUUUUUUURRRRUUUUUUUUUURRRRR"));
            }
            MoveAndSplit(Joypad.Up);
        });

        // ClearCache();
        CacheState("mtmoon", () => {
            AfterMoveAndSplit();
            if(blue)
            {
                SaveAndQuit();
                NoPal.Execute(this, true);
                Execute(SpacePath("UUUUUUUUUUUUURARRRRRRUUUUUUURRRDDDDDDDDDDDRDDDDDDRRRRRRURRR"));
                PickupItem();
                Execute(SpacePath("UAUUUAUUUUR"));
                PickupItem();
                Execute(SpacePath("LUUUUUUUUUUUULUUUUUUUULLLLDLLLLLLLLLLLLDDDDDDD"));
                Execute(SpacePath("LALLALLALLALDD"));
                Execute(SpacePath("RRRAUUAULAUR"));
                PickupItem();
                Execute(SpacePath("DDADDLLL"));
                Execute(SpacePath("RARRARRARRARUU"));
                Execute(SpacePath("LDDDDDDLLLALLLLLLLLLLLUUAUUUUUUUAUUUUU"));
                PickupItem();
                Execute(SpacePath("RRDDR"));
                Execute(SpacePath("DDDDDDDDDDDDRRRRRRRRRRRRRRRAR"));
                Execute(SpacePath("UUURRRRRDDRRRRRRRUURRRDDDDDDDDLLLLDDDDDDDADDLLLLLLLLLLLLLLLLL"));
                Execute(SpacePath("S_BLLALLLLLLRRRRRLLLUAUUUUUUUUURUUALUS_BUA"));
                ForceEncounter(Action.Right, 9, 0xffef); // clefairy
                ForceYoloball("POKE BALL"); // ~
                ClearText();
                Yes();
                Press(Joypad.None, Joypad.A, Joypad.Start);

                ClearText();
                PartySwap("CLEFAIRY", "SQUIRTLE");
                // TossItem("POKE BALL");
                UseItem("RARE CANDY", "CLEFAIRY");
                UseItem("TM01", "CLEFAIRY");
                UseItem("MOON STONE", "CLEFAIRY");
            }
            else
            {
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
                Execute(SpacePath("RRUUURARRRDDRRRRRUARURARRDDDDDDDDALLLLDDDDDDDADDLLLALLLLLLLLLLLLALLLLLLUUUUAUUALUUUUUUD"));
                ForceEncounter(Action.Down, 5, 0x0000); // paras
                ForceYoloball("POKE BALL");
                ClearText();
                No();
                ClearText();

                Execute("L");
                PartySwap("SQUIRTLE", "PARAS");
                SaveAndQuit();
                NoPal.Execute(this, true);
                Execute(SpacePath("S_BS_BDDDDDS_BUUUS_BU"));
                ForceEncounter(Action.Up, 9, 0xffef); // clefairy
                ForceYoloball("POKE BALL");
                ClearText();
                Yes();
                Press(Joypad.None, Joypad.A, Joypad.Start);
                
                Execute("U");
                PartySwap("CLEFAIRY", "PARAS");
                TossItem("POKE BALL");
                UseItem("RARE CANDY", "CLEFAIRY");
                UseItem("TM01", "CLEFAIRY");
                UseItem("MOON STONE", "CLEFAIRY");
            }

            // MOON ROCKET
            MoveTo("MtMoonB2F", 11, 17);
            ClearText();
            MoveSwap("POUND", "MEGA PUNCH");
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TAIL WHIP", Miss));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("SUPERSONIC", Miss));
            ForceTurn(new RbyTurn("POUND"));

            // NERD
            TalkTo(12, 8);
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("DISABLE", "SING"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("SCREECH", Miss));
            ForceTurn(new RbyTurn("POUND"), new RbyTurn("SCREECH", Miss));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("SMOG", Miss));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            TalkTo(13, 6);
            Yes();
            ClearText(); // helix fossil picked up

            if(blue)
            {
                MoveTo(13, 2);
                SaveAndQuit();
                PalHold.Execute(this, true);
                Execute(SpacePath("LLLLLLLLLLDDDDDRAR" + "RRR"));
            }
            else
            {
                MoveTo(3, 7);
                MoveTo("MtMoonB1F", 26, 3);
            }
            MoveAndSplit(Joypad.Right);
        });

        // ClearCache();
        CacheState("misty", () => {
            AfterMoveAndSplit();
            if(blue)
            {
                Execute(SpacePath("RRRRRRRRRRRDDDDRRRRRRRRRRRRRRRRRRRRRRRRRRUURRRRRRRRRRRRD"));
                ForceEncounter(Action.Down, 8, 0x0000);
                ForceYoloball("POKE BALL");
                ClearText();
                No();
            }

            // MoveTo("BikeShop", 2, 6);
            // TalkTo(6, 3);
            // No();
            // ClearText(); // got instant text

            // GOLDEEN GIRL
            MoveTo("CeruleanGym", 5, 3);
            ClearText();
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("PECK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("PECK"));
            ForceTurn(new RbyTurn("POUND"), new RbyTurn("SUPERSONIC"));

            TossItem("POKE BALL");
            UseItem("POTION", "CLEFABLE");

            // MISTY
            TalkTo(4, 2);
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("SING", 1 * Turns), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("MEGA PUNCH", 1), new RbyTurn());
            ForceTurn(new RbyTurn("SING"), new RbyTurn("WATER GUN", 1));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn());
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn(AiItem));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn());
            ForceTurnAndSplit(new RbyTurn("POUND"), new RbyTurn());
        });

        // ClearCache();
        CacheState("bridge", () => {
            ClearText();
            TalkTo("CeruleanPokecenter", 3, 2);
            Yes();
            ClearText(); // healed at center

            MoveTo("BikeShop", 2, 6);
            UseItem("TM11", "CLEFABLE", "GROWL");
            TalkTo(6, 3);
            No();
            ClearText(); // got instant text

            MoveTo("CeruleanCity", 21, 6, Action.Up);

            // BRIDGE RIVAL
            ClearText();
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("QUICK ATTACK"));
            ForceTurn(new RbyTurn("POUND"), new RbyTurn("TELEPORT"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("VINE WHIP"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));

            // BUG CATCHER
            TalkTo("Route24", 11, 31);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));

            // LASS
            TalkTo(10, 28);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("SCRATCH"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));

            // YOUNGSTER
            TalkTo(11, 25);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            // LASS
            TalkTo(10, 22);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("SCRATCH"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));

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
            ForceTurn(new RbyTurn("BUBBLEBEAM"));

            // LASS
            TalkTo(18, 8, Action.Down);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            // HIKER
            MoveTo(24, 6);
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            // ODDISH LASS
            TalkTo(37, 4);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            TalkTo("BillsHouse", 6, 5, Action.Right);
            Yes();
            ClearText();
            TalkTo(1, 4);
            TalkTo(4, 4);

            // Bill menu
            UseItem("ESCAPE ROPE");
        });

        // ClearCache();
        CacheState("boat", () => {
            TalkTo("CeruleanPokecenter", 3, 2);
            Yes();
            ClearText(); // healed at center

            TalkTo("BikeShop", 6, 3);
            No();
            ClearText(); // got instant text

            // DIG ROCKET
            MoveTo("CeruleanCity", 30, 9);
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("KARATE CHOP"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            ClearText();
            PickupItemAt(119, 3, 4);
            MoveTo("Route6", 17, 25);
            MoveTo(15, 28);

            // FEMALE JR. TRAINER
            TalkTo(11, 30, Action.Down);
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            // MALE JR. TRAINER
            MoveTo(10, 31);
            ClearText();
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));

            MoveTo("VermilionCity", 18, 30);
            ClearText();

            MoveTo(102, 11, 12);
            ClearText();
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            PickupItemAt(12, 15);

            // BOAT RIVAL
            MoveTo("SSAnne2F", 36, 8, Action.Up);
            ClearText();
            ForceTurn(new RbyTurn("BUBBLEBEAM"), new RbyTurn("GUST"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"));
            ForceTurn(new RbyTurn("MEGA PUNCH"), new RbyTurn("LEECH SEED"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));

            TalkTo("SSAnneCaptainsRoom", 4, 2); // hm01 received

            MoveTo("VermilionDock", 14, 2);
            ClearText();
            ClearText(); // watch cutscene
        });

        // ClearCache();
        CacheState("surge", () => {
            TalkTo("VermilionMart", 1, 5);
            Sell("NUGGET", 1);
            Buy("REPEL", 3, "PARLYZ HEAL", 2);

            // Cut menu
            MoveTo("VermilionCity", 15, 17, Action.Down);
            ItemSwap("ANTIDOTE", "REPEL");
            UseItem("HM01", "SANDSHREW");
            UseItem("TM08", "CLEFABLE", "MEGA PUNCH");
            UseItem("TM28", "SANDSHREW");
            Cut();

            // Manip
            MoveTo(15, 19);
            SaveAndQuit();

            new RbyIntroSequence(RbyStrat.NoPal, RbyStrat.GfSkip, RbyStrat.Hop0, 1).Execute(this, true);
            Execute(SpacePath("DLLLURUUUUU"));
            ForceCan();
            MoveTo("VermilionGym", 4, 11);
            Press(Joypad.Left);
            ForceCan();

            // SURGE
            TalkTo(5, 1);
            ForceTurn(new RbyTurn("BODY SLAM"), new RbyTurn("SONICBOOM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"), new RbyTurn("THUNDERBOLT", Crit));
            ForceTurnAndSplit(new RbyTurn("BODY SLAM"), new RbyTurn("GROWL"));
        });

        // ClearCache();
        CacheState("route9", () => {
            ClearText();
            // CpuWriteBE<ushort>("wPartyMon1HP", 50);
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
            UseItem("TM24", "CLEFABLE", "SING");
            UseItem("BICYCLE");

            CutAt(19, 28);
            CutAt("Route9", 5, 8);

            // 4 TURN THRASH GIRL
            TalkTo(13, 10);
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));

            // BUG CATCHER
            TalkTo(40, 8);
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));

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
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));

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
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));

            MoveTo(15, 32);
            MoveAndSplit(Joypad.Down);
        });

        // ClearCache();
        CacheState("fly", () => {
            AfterMoveAndSplit();

            // GAMBLER
            TalkTo("Route8", 46, 13);
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BUBBLEBEAM"));

            MoveTo("UndergroundPathWestEast", 47, 2);

            UseItem("BICYCLE");
            PickupItemAt(21, 5, Action.Down); // elixer

            MoveTo("Route7", 5, 14);
            UseItem("BICYCLE");

            // Shopping
            TalkTo("CeladonMart2F", 5, 4);
            Buy("SUPER REPEL", 10, "SUPER POTION", 3);

            TalkTo("CeladonMart4F", 5, 6);
            Buy("POKE DOLL", 1);

            TalkTo("CeladonMartRoof", 12, 2);
            ChooseMenuItem(0);
            ClearText();
            Press(Joypad.A);
            ClearText();
            ChooseMenuItem(0);
            ClearText();

            TalkTo(5, 5);
            Yes();
            ChooseMenuItem(0); // trade
            ClearText();

            TalkTo("CeladonMart5F", 5, 4);
            Buy("X ACCURACY", 2, "X ATTACK", 5, "X SPECIAL", 22, "X SPEED", 8);

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
            ItemSwap("HELIX FOSSIL", "X SPECIAL");
            UseItem("HM02", "PIDGEY");
            ScrollTo("TM13");
            Fly("CeladonCity");

            UseItem("TM13", "CLEFABLE", "POUND");
            UseItem("BICYCLE");

            MoveTo("Route7Gate", 3, 4);
            ClearText();
            MoveTo("Route7", 18, 10);
            UseItem("BICYCLE");

            TalkTo(183, 5, 3);
            MoveTo(10, 29, 30);
            ItemSwap("S.S.TICKET", "SUPER REPEL");
            UseItem("TM29", "CLEFABLE", "BUBBLEBEAM");
            UseItem("SUPER REPEL");
            Fly("LavenderTown");

            // LAVENDER RIVAL
            MoveTo("PokemonTower2F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));

            // CHANNELER 1
            TalkTo("PokemonTower4F", 15, 7);
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC"));

            MoveTo("PokemonTower5F", 11, 9);
            ClearText(); // heal pad

            // CHANNELER 2
            MoveTo("PokemonTower6F", 15, 5);
            ClearText();
            ForceTurn(new RbyTurn("PSYCHIC"));

            // CHANNELER 3
            TalkTo("PokemonTower6F", 9, 5);
            ForceTurn(new RbyTurn("PSYCHIC"));

            PickupItemAt(6, 8); // rare candy

            MoveTo(10, 16);
            ClearText();
            ItemSwap("FULL RESTORE", "X SPEED");
            UseItem("POKE DOLL"); // escape ghost

            // ROCKET 1
            MoveTo("PokemonTower7F", 10, 11);
            ClearText();
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));

            // ROCKET 2
            MoveTo(10, 9);
            ClearText();
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("BODY SLAM"));

            // ROCKET 3
            MoveTo(10, 7);
            ClearText();
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));

            // Get Pokéflute
            TalkTo(10, 3);
            MoveTo(2, 1);
            Press(Joypad.Right);
            ReceiveItemAndSplit();
        });

        // ClearCache();
        CacheState("safari", () => {
            ClearText();
            MoveTo("LavenderTown", 7, 10);
            Fly("CeladonCity");
            TalkTo("CeladonPokecenter", 3, 2);
            Yes();
            ClearText(); // healed at center

            MoveTo("CeladonCity", 41, 10);
            UseItem("BICYCLE");

            // Snorlax menu
            MoveTo("Route16", 27, 10);
            UseItem("SUPER REPEL");
            ItemSwap("ANTIDOTE", "RARE CANDY");
            UseItem("POKE FLUTE");
            RunAway();

            PickupItemAt("Route17", 15, 14); // candy

            // Post cycling menu
            MoveTo("Route18", 13, 7);
            MoveTo("Route18", 40, 8);
            UseItem("SUPER REPEL");
            UseItem("RARE CANDY", "CLEFABLE");
            UseItem("RARE CANDY", "CLEFABLE");
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

            PickupItemAt(217, 21, 10, Action.Down);

            MoveTo(218, 7, 13);
            PickupItemAt("SafariZoneWest", 19, 7, Action.Down); // gold teeth

            TalkTo("SafariZoneSecretHouse", 3, 3);
            MoveTo("SafariZoneWest", 3, 4);
            Dig();
        });

        // ClearCache();
        CacheState("silph", () => {
            UseItem("BICYCLE");

            MoveTo("Route7Gate", 3, 4);
            ClearText();
            MoveTo("Route7", 18, 10);
            UseItem("BICYCLE");

            // ARBOK ROCKET
            TalkTo("SilphCo5F", 8, 16);
            ForceTurn(new RbyTurn("PSYCHIC"));

            PickupItemAt(21, 16);
            TalkTo(7, 13);
            TalkTo("SilphCo3F", 17, 9);

            // SILPH RIVAL
            MoveTo("SilphCo7F", 3, 2, Action.Left);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("BODY SLAM"), new RbyTurn("PSYBEAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));

            MoveTo(5, 7, Action.Right);

            // SILPH ROCKET
            TalkTo("SilphCo11F", 3, 16);
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));

            // SILPH GIOVANNI
            TalkTo(6, 13, Action.Up);
            MoveTo(6, 13);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("FOCUS ENERGY"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurnAndSplit(new RbyTurn("PSYCHIC"));
        });

        // ClearCache();
        CacheState("koga", () => {
            ClearText();
            Dig();
            Fly("FuchsiaCity");
            UseItem("BICYCLE");

            // JUGGLER 1
            TalkTo("FuchsiaGym", 7, 8);
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"), new RbyTurn("RECOVER"));
            ForceTurn(new RbyTurn("BODY SLAM"));

            // JUGGLER 2
            MoveTo(1, 7);
            ClearText();
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"), new RbyTurn("HEADBUTT"));
            ForceTurn(new RbyTurn("BODY SLAM"));

            // KOGA
            TalkTo(4, 10);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("TACKLE"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("PSYCHIC"));
        });

        // ClearCache();
        CacheState("sabrina", () => {
            ClearText();

            MoveTo("FuchsiaCity", 5, 28);
            UseItem("BICYCLE");

            TalkTo("WardensHouse", 2, 3);
            MoveTo("FuchsiaCity", 27, 28);
            Fly("SaffronCity");
            UseItem("BICYCLE");

            // SABRINA
            TalkTo("SaffronGym", 9, 8);
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("RECOVER"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("X ATTACK"), new RbyTurn("CONFUSION"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurnAndSplit(new RbyTurn("BODY SLAM"));
        });

        // ClearCache();
        CacheState("mansion", () => {
            ClearText();
            MoveTo(1, 5);
            Dig();
            MoveNpc("PalletTown", 3, 8, Action.Left); // good npc
            Fly("PalletTown");

            // Surf menu
            MoveTo(4, 13, Action.Down);
            UseItem("SUPER REPEL");
            ItemSwap("HM01", "X ATTACK");
            UseItem("HM03", "SQUIRTLE");
            Surf();

            MoveTo("CinnabarIsland", 4, 4);
            TalkTo("PokemonMansion3F", 6, 3);
            TossItem("ANTIDOTE", 0);
            UseItem("SUPER REPEL");
            UseItem("ELIXER", "CLEFABLE");

            TalkTo("PokemonMansion3F", 10, 5, Action.Up);
            ActivateMansionSwitch();
            MoveTo(16, 14);
            FallDown();

            TalkTo("PokemonMansionB1F", 18, 25, Action.Up);
            ActivateMansionSwitch();

            TalkTo(20, 3, Action.Up);
            ActivateMansionSwitch();
            PickupItemAt(10, 2); // candy
            PickupItemAt(1, 9); // candy
            PickupItemAt(5, 13); // secret key
            Dig();
        });

        // ClearCache();
        CacheState("blaine", () => {
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
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("X ATTACK"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("X ATTACK"), new RbyTurn("LEER"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"), new RbyTurn("FIRE BLAST"));
            ForceTurnAndSplit(new RbyTurn("BODY SLAM"));
        });

        // ClearCache();
        CacheState("erika", () => {
            ClearText();
            // CpuWriteBE<ushort>("wPartyMon1HP", 50);
            Dig();
            UseItem("BICYCLE");

            CutAt(35, 32);
            CutAt("CeladonGym", 2, 4);

            // BEAUTY
            MoveTo(3, 4);
            ClearText();
            ForceTurn(new RbyTurn("BODY SLAM"));

            // ERIKA
            TalkTo(4, 3);
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurnAndSplit(new RbyTurn("PSYCHIC"));
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
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC"));

            MoveTo("ViridianCity", 32, 8);

            // GIOVANNI
            TalkTo("ViridianGym", 2, 1);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("HORN DRILL"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("ICE BEAM"));
        });

        // ClearCache();
        CacheState("victoryroad", () => {
            ClearText();
            MoveTo("ViridianCity", 32, 8);
            UseItem("SUPER REPEL");
            ItemSwap("PARLYZ HEAL", "FULL RESTORE");
            UseItem("HM04", "SANDSHREW");
            UseItem("RARE CANDY", "CLEFABLE");
            UseItem("RARE CANDY", "CLEFABLE");
            UseItem("BICYCLE");

            // VIRIDIAN RIVAL
            MoveTo("Route22", 29, 5);
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("WHIRLWIND"));
            ForceTurn(new RbyTurn("X ATTACK"), new RbyTurn("WING ATTACK"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));

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
            Deposit("PIDGEY", "SQUIRTLE", "SANDSHREW");

            TalkTo(7, 6);
            Yes();
            ClearText();
            TalkTo(1, 5);
            Buy("FULL RESTORE", 3);

            MoveTo("IndigoPlateauLobby", 8, 0);

            // LORELEI
            TalkTo("LoreleisRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("REST"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("REST"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("REST"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("WATER GUN"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"), new RbyTurn("ICE PUNCH"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurnAndSplit(new RbyTurn("THUNDERBOLT"));
        });

        // ClearCache();
        CacheState("bruno", () => {
            ClearText();
            Execute("U U U");

            // BRUNO
            TalkTo("BrunosRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("RAGE"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurnAndSplit(new RbyTurn("PSYCHIC"));
        });

        // ClearCache();
        CacheState("agatha", () => {
            ClearText();
            Execute("U U U");

            // AGATHA
            TalkTo("AgathasRoom", 5, 2, Action.Right);
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("DREAM EATER"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC"));
            ForceTurn(new RbyTurn("PSYCHIC", 30), new RbyTurn("DREAM EATER"));
            ForceTurnAndSplit(new RbyTurn("PSYCHIC"));
        });

        ClearCache();
        CacheState("lance", () => {
            ClearText();
            Execute("U U U");

            // LANCE
            MoveTo("LancesRoom", 5, 1);
            ClearText();
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("DRAGON RAGE"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("DRAGON RAGE")); //hyperbeam
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurnAndSplit(new RbyTurn("ICE BEAM"));
        });

        // ClearCache();
        CacheState("champion", () => {
            ClearText();
            Execute("U U");

            // CHAMPION
            ClearText();
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("MIRROR MOVE"));
            ForceTurn(new RbyTurn("X SPEED"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("X SPECIAL"), new RbyTurn("SKY ATTACK"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurn(new RbyTurn("ICE BEAM"));
            ForceTurn(new RbyTurn("THUNDERBOLT"));
            ForceTurn(new RbyTurn("ICE BEAM"), new RbyTurn("ROAR"));
            ForceTurn(new RbyTurn("BODY SLAM"));
            ForceTurnAndSplit(new RbyTurn("ICE BEAM"));
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
