using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;

using static SearchCommon;
using static RbyIGTChecker<Red>;

class Clefairy
{
    public static void Check()
    {
        // string state = "basesaves/red/manip/clefairysq.gqs";
        string state = "basesaves/red/manip/clefairypa.gqs";

        // bool DoublePotionBackout(Red gb)
        // {
        //     gb.ClearText();
        //     gb.Press(Joypad.A | Joypad.Down, Joypad.A | Joypad.Down | Joypad.Right, Joypad.B, Joypad.A, Joypad.B, Joypad.A | Joypad.Up);
        //     return gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"];
        // }
        bool ItemBackoutSelect(Red gb)
        {
            gb.ClearText();
            gb.Press(Joypad.A | Joypad.Down, Joypad.B, Joypad.A, Joypad.Select, Joypad.A);
            return gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"];
        }
        bool PotionBackoutSelect(Red gb)
        {
            gb.ClearText();
            gb.Press(Joypad.A | Joypad.Down, Joypad.A | Joypad.Down | Joypad.Right, Joypad.B, Joypad.Select, Joypad.A | Joypad.Up);
            return gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"];
        }

        string path = "S_BS_BDDDDDS_BUUUS_BUU"; //3299 119
        // string path = "S_BS_BS_BDDDDDUUUS_BUU"; //3299 119
        // string path = "S_BDDDDDUUS_BS_BUS_BUU"; //3299 119
        // string path = "DDS_BDDDUUS_BS_BUS_BUU"; //3299 119
        // string path = "S_BLLDDDUUS_BS_BUS_BRR"; //3299 119
        // string path = "DDS_BDDUUUS_BS_BUS_BLR"; //3299 119
        // string path = "S_BS_BDDDDUS_BUUUS_BLR"; //3239 119 (62 00f0)
        // string path = "S_BS_BLS_BLRRDDDUUS_BU"; //3239 119 (60 feee)
        // string path = "S_BS_BLS_BLRRDDUULS_BR"; //3239 119 (60 feee)
        // CheckIGT(state, new RbyIntroSequence(RbyStrat.NoPal), path, "CLEFAIRY", 3600, true, false, Verbosity.Summary, false, -1, ItemBackoutSelect);
        // CheckIGT(state, new RbyIntroSequence(RbyStrat.NoPal), path, "CLEFAIRY", 3600, true, false, Verbosity.Summary, true, -1, PotionBackoutSelect);
        CheckIGT(state, new RbyIntroSequence(RbyStrat.NoPal), path, "CLEFAIRY", 60, true);

        // string path = "S_BS_BADDDDDDUAUS_BUAUUU"; //3360 60
        // string path = "S_BS_BADDDDDDUAUS_BAUUUU"; //3360 60
        // string path = "S_BS_BADDDDDDAUUS_BUAUUU"; //3360 60
        // string path = "S_BS_BADDDDDUUUS_BUAULAR"; //3360 60
        // string path = "S_BS_BADDDDDUUAUS_BUAULR"; //3360 60
        // string path = "S_BS_BADDDDDUUAUS_BAUULR"; //3360 60
        // string path = "DULLRRADDUS_BUALS_BS_BAR"; //3360 60
        // string path = "DULLRRADLUS_BRADS_BS_BAU"; //3360 60
        // string path = "DULLRRADLUS_BRALS_BS_BAR"; //3360 60
        // string path = "DULLRRADDDS_BUAUS_BS_BAU"; //3302 60 (59 geodude)
        // CheckIGT(state, new RbyIntroSequence(RbyStrat.NoPalAB), path, "CLEFAIRY", 3600, true, null, true,  0, 1, 16, Verbosity.Summary);

        // spearow 4,7,7s,9 redbar 1,2,2s
        // pidgey 6,8,9s redbar 1,1s
        // squirtle 4,7 redbar 2,2s,4,5s,7s
        // paras 2s,4s,7s
    }

    public static void Search(int numThreads = 16)
    {
        StartWatch();
        RbyIntroSequence intro = new RbyIntroSequence(RbyStrat.NoPal);

        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];

        gb.LoadState("basesaves/red/manip/clefairysq.gqs");
        IGTState state = gb.IGTCheck(intro, 1)[0];

        RbyMap moon = gb.Maps[61];
        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A | Action.StartB;
        RbyTile startTile = gb.Tile;
        RbyTile[] blockedTiles = { moon[7, 20], moon[8, 20], moon[9, 20], moon[10, 21], moon[10, 22], moon[10, 23], moon[10, 24], moon[10, 25], moon[10, 26], moon[11, 26] };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, moon[9, 21], actions, blockedTiles);
        // Pathfinding.DebugDrawEdges(gb, moon, 0);

        var parameters = new SFParameters<Red, RbyMap, RbyTile>()
        {
            MaxCost = 400,
            // SuccessSS = success,
            EncounterCallback = gb => gb.EnemyMon.Species.Name == "CLEFAIRY"
                && gb.EnemyMon.DVs.Attack >= 14 && gb.EnemyMon.DVs.Defense >= 14 && gb.EnemyMon.DVs.Speed >= 14 && gb.EnemyMon.DVs.Special >= 14,
            LogStart = startTile.PokeworldLink + "/",
            // FoundCallback = state =>
            // {
            //     Trace.WriteLine(state.Log + " Captured: " + state.IGT.TotalSuccesses + " Failed: " + (state.IGT.TotalFailures - state.IGT.TotalRunning) + " NoEnc: " + state.IGT.TotalRunning + " Cost: " + state.WastedFrames);
            // },
            FoundCallback = (state, gb) =>
            {
                Trace.WriteLine(state.Log + " " + gb.EnemyMon.Species.Name + " L" + gb.EnemyMon.Level + " dvs: " + gb.EnemyMon.DVs + " cost: " + state.WastedFrames);
            }
        };

        // DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states);
        SingleFrameSearch.StartSearch(gbs, parameters, startTile, 0, state);
        Elapsed("search");
    }
}
