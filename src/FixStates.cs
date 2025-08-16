public static class FixStates
{
    public static void Fix(string folder)
    {
        string[] states = Directory.GetFiles(folder);
        Blue r = new Blue();
        foreach (string state in states)
        {
            r.LoadState(state);

            // swap move 1 & 2
            /*
            ushort moves = r.CpuReadBE<ushort>("wPartyMon1Moves");
            ushort pp = r.CpuReadBE<ushort>("wPartyMon1PP");
            r.CpuWriteLE("wPartyMon1Moves", moves);
            r.CpuWriteLE("wPartyMon1PP", pp);
            moves = r.CpuReadBE<ushort>("wBattleMonMoves");
            pp = r.CpuReadBE<ushort>("wBattleMonPP");
            r.CpuWriteLE("wBattleMonMoves", moves);
            r.CpuWriteLE("wBattleMonPP", pp);
            */

            // parlyz heal & money
            /*
            int slot = r.Bag.IndexOf("PARLYZ HEAL");
            if(slot != -1)
            {
                // r.CpuWrite(r.SYM["wBagItems"] + 2 * slot + 1, 2);
                byte[] t = r.CpuRead("wPlayerMoney", 3);
                int bcd = 10000 * (t[0] / 16 * 10 + (t[0] & 0xf)) + 100 * (t[1] / 16 * 10 + (t[1] & 0xf)) + (t[2] / 16 * 10 + (t[2] & 0xf));
                Console.WriteLine(bcd);
                bcd -= 50;
                int n0 = (bcd / 10000) % 100, n1 = (bcd / 100) % 100, n2 = bcd % 100;
                t[0] = (byte) ((n0 / 10 * 16) | (n0 % 10));
                t[1] = (byte) ((n1 / 10 * 16) | (n1 % 10));
                t[2] = (byte) ((n2 / 10 * 16) | (n2 % 10));
                r.CpuWrite("wPlayerMoney", t);
            }
            */

            // race x speed swap
            /*
            int xspeedslot = r.Bag.IndexOf("X SPEED");
            int hm01slot = r.Bag.IndexOf("HM01");
            ushort item1 = r.CpuReadLE<ushort>(r.SYM["wBagItems"] + 0);
            ushort item2 = r.CpuReadLE<ushort>(r.SYM["wBagItems"] + 2);
            ushort item3 = r.CpuReadLE<ushort>(r.SYM["wBagItems"] + 4);
            r.CpuWriteLE<ushort>(r.SYM["wBagItems"] + 0, item2);
            r.CpuWriteLE<ushort>(r.SYM["wBagItems"] + 2, item3);
            r.CpuWriteLE<ushort>(r.SYM["wBagItems"] + 4, item1);
            r.CpuWrite(r.SYM["wBagItems"] + 5, 1);
            */

            // bird hp
            /*
            r.CpuWrite("wPartyMon3Level", 5);
            if(r.CpuReadBE<ushort>("wPartyMon3HP") > 19) r.CpuWriteBE<ushort>("wPartyMon3HP", 19);
            r.CpuWriteBE<ushort>("wPartyMon3MaxHP", 19);
            r.CpuWrite("wPartyMon1Level", 5);
            if(r.CpuReadBE<ushort>("wPartyMon1HP") > 19) r.CpuWriteBE<ushort>("wPartyMon1HP", 19);
            r.CpuWriteBE<ushort>("wPartyMon1MaxHP", 19);
            */

            // classic early vermilion mart
            /*
            int hm01slot = r.SYM["wBagItems"] + r.Bag.IndexOf("HM01") * 2;
            int potionslot = r.SYM["wBagItems"] + r.Bag.IndexOf("POTION") * 2;
            int parlyzslot = r.SYM["wBagItems"] + r.Bag.IndexOf("PARLYZ HEAL") * 2;
            ushort hm01 = r.CpuReadLE<ushort>(hm01slot);
            ushort potion = r.CpuReadLE<ushort>(potionslot);
            ushort parlyz = r.CpuReadLE<ushort>(parlyzslot);
            r.CpuWriteLE<ushort>(hm01slot, potion);
            r.CpuWriteLE<ushort>(potionslot, parlyz);
            r.CpuWriteLE<ushort>(parlyzslot, hm01);
            int ballslot = r.SYM["wBagItems"] + r.Bag.IndexOf("POKE BALL") * 2;
            ushort ball = r.CpuReadLE<ushort>(ballslot);
            potion = (ushort)(potion - 256);
            r.CpuWriteLE<ushort>(potionslot, ball);
            r.CpuWriteLE<ushort>(ballslot, potion);
            */

            // late/early mart
            /*
            r.CpuWrite("wNumBagItems", 4);
            r.CpuWriteLE<ushort>(r.SYM["wBagItems"] + 4 * 2, r.CpuReadLE<ushort>(r.SYM["wBagItems"] + 3 * 2));
            r.CpuWriteLE<ushort>(r.SYM["wBagItems"] + 3 * 2, r.CpuReadLE<ushort>(r.SYM["wBagItems"] + 2 * 2));
            r.CpuWrite(r.SYM["wBagItems"] + 2 * 2, r.Items["ANTIDOTE"].Id);
            r.CpuWrite(r.SYM["wBagItems"] + 1*2, r.Items["ANTIDOTE"].Id);
            r.CpuWrite(r.SYM["wBagItems"] + 1*2 + 1, 1);
            r.CpuWrite(r.SYM["wBagItems"] + 2*2, r.Items["TM34"].Id);
            r.CpuWrite(r.SYM["wBagItems"] + 2*2 + 1, 1);
            r.CpuWrite(r.SYM["wBagItems"] + 3*2, r.Items["POTION"].Id);
            r.CpuWrite(r.SYM["wBagItems"] + 3*2 + 1, 8);
            r.CpuWrite(r.SYM["wBagItems"] + 1*2, r.Items["TM34"].Id);
            r.CpuWrite(r.SYM["wBagItems"] + 1*2 + 1, 1);
            r.CpuWrite(r.SYM["wBagItems"] + 2*2, r.Items["POTION"].Id);
            r.CpuWrite(r.SYM["wBagItems"] + 2*2 + 1, 6);
            */

            // classic stats
            /*
            r.CpuWriteBE<ushort>("wBattleMonHP", 5);
            r.CpuWriteBE<ushort>("wPartyMon1HP", 5);
            y.CpuWrite("wPlayerMoveListIndex", 0);
            r.CpuWrite("wRepelRemainingSteps", 0x7);
            r.CpuWrite("wRepelRemainingSteps", 0xd);
            r.CpuWriteBE<ushort>("wPartyMon1DVs", 0xffe0);
            r.CpuWriteBE<ushort>("wPartyMon1HP", 119);
            r.CpuWriteBE<ushort>("wPartyMon1MaxHP", 119);
            r.CpuWriteBE<ushort>("wPartyMon1Special", (ushort)(r.CpuReadBE<ushort>("wPartyMon1Special") - 9));
            */

            // classic silph bar
            /*
            ushort[] hps  = {110, 111, 112, 112, 113, 114, 115, 115, 116, 117, 118, 118, 119, 120, 120, 121};
            ushort[] spcs = { 65,  66,  66,  67,  68,  69,  69,  70,  71,  71,  72,  73,  74,  74,  75,  76};
            for(int i = 0; i <= 3; ++i)
            {
                for(int j = 0; j <= 3; ++j)
                {
                    for(int k = 0; k <= 1; ++k)
                    {
                        int hp = i * 5 - (i * 5) % 2;
                        int spc = j * 5 - (j * 5) % 2;
                        int dmg = k * 15;
                        ushort dvs = 0;
                        if(hp == 0) dvs = 0x8880;
                        if(hp == 4) dvs = 0x8780;
                        if(hp == 10) dvs = 0x7870;
                        if(hp == 14) dvs = 0x7770;
                        dvs |= (ushort)spc;
                        r.CpuWriteBE<ushort>("wPartyMon1DVs", dvs);
                        r.CpuWriteBE<ushort>("wPartyMon1MaxHP", hps[hp]);
                        r.CpuWriteBE<ushort>("wPartyMon1Special", spcs[spc]);
                        r.CpuWriteBE<ushort>("wPartyMon1HP", (ushort)(hps[hp] - dmg));
                        r.SaveState(@"C:\Users\PY\AppData\Roaming\gambatte\saves\Classic\bot\silphbar_" + hp + "_" + spc + "_" + dmg + ".gqs");
                    }
                }
            }
            */

            // swap 2 items
            /*
            void SwapItems(string item1name, string item2name)
            {
                int item1slot = r.SYM["wBagItems"] + r.Bag.IndexOf(item1name) * 2;
                int item2slot = r.SYM["wBagItems"] + r.Bag.IndexOf(item2name) * 2;
                ushort item1 = r.CpuReadLE<ushort>(item1slot);
                ushort item2 = r.CpuReadLE<ushort>(item2slot);
                r.CpuWriteLE(item1slot, item2);
                r.CpuWriteLE(item2slot, item1);
            }
            SwapItems("POTION", "TM34");
            */

            // add item
            /*
            void AddItem(string name, byte qty, int slot)
            {
                int numitems = r.CpuRead("wNumBagItems");
                r.CpuWrite("wNumBagItems", (byte)(numitems + 1));
                int newaddr = r.SYM["wBagItems"] + slot * 2;
                r.CpuWrite(newaddr + 2, r.CpuRead(newaddr, (numitems - slot + 1) * 2));
                r.CpuWrite(newaddr, r.Items[name].Id);
                r.CpuWrite(newaddr + 1, qty);
            }
            // int index1 = r.Bag.IndexOf("S.S.TICKET"); int index2 = r.Bag.IndexOf("X ACCURACY"); int newindex = index2 != -1 && index2 < index1 ? index2 : index1;
            AddItem("TM06", 1, r.Bag.IndexOf("CARD KEY") + 1);
            */

            // change quantity
            /*
            int addr = r.SYM["wBagItems"] + r.Bag.IndexOf("ELIXER") * 2 + 1;
            r.CpuWrite(addr, (byte)(r.CpuRead(addr) - 1));
            */

            // delete item
            /*
            void DeleteItem(string name)
            {
                int numitems = r.CpuRead("wNumBagItems");
                r.CpuWrite("wNumBagItems", (byte)(numitems - 1));
                int delindex = r.Bag.IndexOf(name);
                int deladdr = r.SYM["wBagItems"] + delindex * 2;
                r.CpuWrite(deladdr, r.CpuRead(deladdr + 2, (numitems - delindex) * 2));
            }
            DeleteItem("X ACCURACY");
            */

            // r.CpuWrite("wPartyAndBillsPCSavedMenuItem", 0);
            // r.CpuWriteBE<ushort>("wPartyMon1HP", 7);
            // r.CpuWrite(r.SYM["wBagItems"] + r.Bag.IndexOf("ELIXER") * 2 + 1, 2);
            // r.CpuWrite(r.SYM["wBagItems"] + r.Bag.IndexOf("ELIXER") * 2, r.Items["PARLYZ HEAL"]);

            // change party mon
            // r.LoadState("basesaves/blue/sandshrewslot4.gqs");
            // byte[] species = r.CpuRead("wPartyDataStart", r.SYM["wPartyMon1"] - r.SYM["wPartyDataStart"]);
            // byte[] sandshrew = r.CpuRead("wPartyMon4", r.SYM["wPartyDataEnd"] - r.SYM["wPartyMon4"]);
            // r.LoadState(state);
            // byte[] parasmoves = r.CpuRead("wPartyMon4Moves", r.SYM["wPartyMon4OTID"] - r.SYM["wPartyMon4Moves"]);
            // byte[] paraspp = r.CpuRead("wPartyMon4PP", r.SYM["wPartyMon4Level"] - r.SYM["wPartyMon4PP"]);
            // r.CpuWrite("wPartyDataStart", species);
            // r.CpuWrite("wPartyMon4", sandshrew);
            // r.CpuWrite("wPartyMon4Moves", parasmoves);
            // r.CpuWrite("wPartyMon4PP", paraspp);
            // r.CpuWrite(r.SYM["wPartyMon3Moves"], r.Moves["BUBBLE"].Id);
            // r.CpuWrite(r.SYM["wPartyMon3PP"], 30);
            // r.CpuWrite(r.SYM["wPartyMon4Moves"] + 3, r.Moves["STRENGTH"].Id);
            // r.CpuWrite(r.SYM["wPartyMon4PP"] + 3, 15);

            // byte[] items = r.CpuRead("wNumBagItems", r.SYM["wRivalName"] - r.SYM["wNumBagItems"]);
            // byte[] party = r.CpuRead("wPartyDataStart", r.SYM["wPartyDataEnd"] - r.SYM["wPartyDataStart"]);

            r.SaveState(state);
        }
    }

    static void ClassicState(string state, string party, int cursor, bool cutdig = false, bool fly = false, bool surf = false, bool str = false)
    {
        const string path = @"C:\Users\PY\AppData\Roaming\gambatte\saves\Classic\bot\";
        Red r = new Red();
        r.LoadState(path + state + ".gqs");

        // party
        Red r2 = new Red();
        r2.LoadState(path + "_" + party + ".gqs");
        r.CpuWrite("wPartyDataStart", r2.CpuRead("wPartyDataStart", r2.SYM["wPartyMon1"] - r2.SYM["wPartyDataStart"]));
        r.CpuWrite("wPartyMon2", r2.CpuRead("wPartyMon2", r2.SYM["wPartyDataEnd"] - r2.SYM["wPartyMon2"]));

        // cursor
        if (cursor > 0) r.CpuWrite("wPartyAndBillsPCSavedMenuItem", (byte)(cursor - 1));

        // hms
        int squirtle = r.SYM["wPartyMon2Moves"] - 1;
        int spearow = r.SYM["wPartyMon3Moves"] - 1;
        int dux = r.SYM["wPartyMon3Moves"] - 1;
        int oddish = r.SYM["wPartyMon4Moves"] - 1;
        if (cutdig)
        {
            if (party == "Oddish")
                r.CpuWrite(oddish + 2, r.Moves["CUT"].Id);
            else if (party == "Dux6" || party == "Dux")
                r.CpuWrite(dux + 3, r.Moves["CUT"].Id);
            else if (party == "Dux7")
                r.CpuWrite(dux + 4, r.Moves["CUT"].Id);

            r.CpuWrite(squirtle + 4, r.Moves["DIG"].Id);
        }
        if (fly)
        {
            if (party == "Oddish")
            {
                r.CpuWrite(spearow + 2, r.Moves["FLY"].Id);
                r.CpuWrite(r.SYM["wPartyMon4HP"] + 1, r.CpuRead(r.SYM["wPartyMon4MaxHP"] + 1));
            }
            else if (party == "Dux6" || party == "Dux")
                r.CpuWrite(dux + 4, r.Moves["FLY"].Id);
            else if (party == "Dux7")
                r.CpuWrite(dux + 1, r.Moves["FLY"].Id);
        }
        if (surf)
        {
            r.CpuWrite(squirtle + 2, r.Moves["SURF"].Id);
        }
        if (str)
        {
            r.CpuWrite(squirtle + 1, r.Moves["STRENGTH"].Id);
        }

        int space = state.IndexOf(' ');
        r.SaveState(path + state.Substring(0, space) + (party == "Oddish" ? "b" : "c") + state.Substring(space) + party + ".gqs");
    }

    static void FixClassicStates()
    {
        ClassicState("22 Cut1", "Oddish", -1);
        ClassicState("22 Cut1", "Dux", 3);
        ClassicState("24 Voucher", "Oddish", -1, true);
        ClassicState("24 Voucher", "Dux", 3, true);
        ClassicState("25 Bike", "Oddish", 2, true);
        ClassicState("40 Fly1", "Dux6", 3, true);
        ClassicState("40 Fly1", "Dux7", 3, true);
        ClassicState("40d Fly2", "Dux", 3, true, true);
        ClassicState("43 DigFly", "Oddish", -1, true, true);
        ClassicState("43 DigFly", "Dux6", -1, true, true);
        ClassicState("45 FlyCeladon", "Dux6", -1, true, true);
        ClassicState("45 FlyCeladon", "Dux7", -1, true, true);
        ClassicState("48b SilphDig", "Oddish", -1, true, true);
        ClassicState("49 SilphCarbos", "Oddish", 2, true, true);
        ClassicState("54 FuchsiaCut", "Dux7", -1, true, true);
        ClassicState("57 SafariDig", "Oddish", -1, true, true);
        ClassicState("57 SafariDig", "Dux6", 3, true, true);
        ClassicState("57 SafariDig", "Dux7", 3, true, true);
        ClassicState("59 FlyPallet", "Dux6", -1, true, true);
        ClassicState("59 FlyPallet", "Dux7", -1, true, true);
        ClassicState("60 Pallet", "Oddish", -1, true, true);
        ClassicState("63x MansionDig", "Oddish", -1, true, true, true, true);
        ClassicState("64 BlaineDig", "Oddish", -1, true, true, true, true);
        ClassicState("66 SabrinaDig", "Oddish", -1, true, true, true, true);
        ClassicState("67 CutFly", "Dux6", -1, true, true, true, true);
        ClassicState("67 CutFly", "Dux7", -1, true, true, true, true);

        ClassicState("s8 Fly", "Oddish", -1, true);
        ClassicState("s8 Fly", "Dux", -1, true);
        // ClassicState("s9 Hideout", "Oddish", 4, true);
        ClassicState("s9 Hideout", "Dux6", 3, true);
        ClassicState("s9 Hideout", "Dux7", 3, true);
        ClassicState("s10 Flute", "Oddish", -1, true, true);
        ClassicState("s10 Flute", "Dux6", -1, true, true);
        // ClassicState("s10 Flute", "Dux7", -1, true, true);
        ClassicState("s11 Silph", "Oddish", -1, true, true);
        ClassicState("s11 Silph", "Dux6", -1, true, true);
        ClassicState("s11 Silph", "Dux7", -1, true, true);
        ClassicState("s12 Koga", "Oddish", -1, true, true);
        ClassicState("s12 Koga", "Dux6", -1, true, true);
        ClassicState("s12 Koga", "Dux7", -1, true, true);
        ClassicState("s13 Blaine", "Oddish", -1, true, true);
        ClassicState("s13 Blaine", "Dux6", -1, true, true);
        ClassicState("s13 Blaine", "Dux7", -1, true, true);
        ClassicState("s14 Sabrina", "Oddish", -1, true, true, true, true);
        ClassicState("s14 Sabrina", "Dux6", -1, true, true, true, true);
        ClassicState("s14 Sabrina", "Dux7", -1, true, true, true, true);
        ClassicState("s15 Erika", "Oddish", -1, true, true, true, true);
        ClassicState("s15 Erika", "Dux6", -1, true, true, true, true);
        ClassicState("s15 Erika", "Dux7", -1, true, true, true, true);
        ClassicState("s16 Giovanni", "Oddish", -1, true, true, true, true);
        ClassicState("s16 Giovanni", "Dux6", -1, true, true, true, true);
        ClassicState("s16 Giovanni", "Dux7", -1, true, true, true, true);

        ClassicState("1 classicfte", "Oddish", -1, true, true);
        ClassicState("1 classicfte", "Dux6", -1, true, true);
        ClassicState("1 classicfte", "Dux7", -1, true, true);

        ClassicState("72 Pallet", "Oddish", -1, true, true);
        ClassicState("73 Pallet15", "Oddish", -1, true, true);
    }
}
