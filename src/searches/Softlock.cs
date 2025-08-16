using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using static SearchCommon;
using static RbyIGTChecker<Red>;

class Softlock
{
    public static void Any()
    {
        RedCb gb = new RedCb();
        gb.Record("test");
        gb.CallbackHandler.SetCallback(gb.SYM["VBlank"], gb => {
            Trace.WriteLine($"{gb.EmulatedSamples} {gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} {gb.CpuRead("hRandomAdd"):x2}{gb.CpuRead("hRandomSub"):x2}");
        });
        new RbyIntroSequence(RbyStrat.NoPal, RbyStrat.GfSkip, RbyStrat.Hop2, RbyStrat.Title0, RbyStrat.NewGame, RbyStrat.NewGameReset, RbyStrat.GfReset, RbyStrat.GfSkip, RbyStrat.Hop0, RbyStrat.Title0, RbyStrat.NewGame).Execute(gb);
        // Trace.WriteLine(gb.EmulatedSamples);
        Trace.WriteLine($"{gb.EmulatedSamples} {gb.CpuRead("hRandomAdd"):x2}{gb.CpuRead("hRandomSub"):x2}");
            // if(gb.EmulatedSamples - lasttime > 40000) System.Diagnostics.Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} " + (gb.EmulatedSamples - lasttime) + " +" + (float)(gb.EmulatedSamples - lasttime - SamplesPerFrame) / SamplesPerFrame);
            // lasttime = gb.EmulatedSamples;
        gb.ClearText(Joypad.A);
        gb.Press(Joypad.Down | Joypad.A);
        gb.ClearText(Joypad.A);
        Trace.WriteLine(gb.CpuReadBE<ushort>("wPlayerID"));
        gb.Dispose();
    }

    public static void Check()
    {
        RedCb gb = new RedCb();
        gb.Record("test");
        new RbyIntroSequence(RbyStrat.PalHold, RbyStrat.GfSkip, RbyStrat.Hop0, RbyStrat.Title0, RbyStrat.NewGame).Execute(gb);
        // gb.CallbackHandler.SetCallback(gb.SYM["VBlank"], gb => {
        //     Trace.WriteLine($"{gb.CpuRead("wPlayTimeMinutes"):d2}:{gb.CpuRead("wPlayTimeSeconds"):d2}.{gb.CpuRead("wPlayTimeFrames"):d2} {gb.CpuRead("hRandomAdd"):x2}{gb.CpuRead("hRandomSub"):x2}");
        // });
        Trace.WriteLine(gb.EmulatedSamples);
        gb.ClearText(Joypad.A);
        gb.Press(Joypad.Down | Joypad.A);
        gb.ClearText(Joypad.A);
        gb.Press(Joypad.Down | Joypad.A);
        gb.ClearText(Joypad.A);
        gb.Inject(Joypad.Right);
        gb.Execute(SpacePath("RUUUUURRDDDDDLLLLD"));
        gb.RunUntil("TryWalking");
        gb.RunUntil("JoypadOverworld");
        gb.Execute(SpacePath("RRDDRADDUUUUAUUUDA" + "DRARUADDDLLALLAULLL" + "RDDRRRRRURRA" + "ULLLLLRRUAUUURRRU"));
        gb.AdvanceFrames(400, Joypad.B);
        gb.ClearText(Joypad.A, 6);
        int x = gb.CpuRead("wSprite02StateData2MapX") - 4;
        int y = gb.CpuRead("wSprite02StateData2MapY") - 4;
        Trace.WriteLine(x + " " + y);
        gb.AdvanceFrames(600);
        gb.Dispose();
    }

    public static void Search(int numThreads = 16)
    {
        StartWatch();

        Red[] gbs = MultiThread.MakeThreads<Red>(numThreads);
        Red gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        new RbyIntroSequence(RbyStrat.PalHold, RbyStrat.GfSkip, RbyStrat.Hop0, RbyStrat.Title0, RbyStrat.NewGame).Execute(gb);
        gb.ClearText(Joypad.A);
        gb.Press(Joypad.Down | Joypad.A);
        gb.ClearText(Joypad.A);
        gb.Press(Joypad.Down | Joypad.A);
        gb.ClearText(Joypad.A);
        gb.Inject(Joypad.Right);
        gb.Execute(SpacePath("RUUUUURRDDDDDLLLLD"));
        gb.RunUntil("TryWalking");
        gb.RunUntil("JoypadOverworld");
        gb.Execute(SpacePath("RRDDRADDUUUUAUUUDA" + "DRARUADDDLLALLAULLL" + "RDDRRRRRURRA" + "ULLLLLRRUAUUURRRU"));

        IGTState state = new IGTState(gb, true, 0);

        RbyMap pallet = gb.Maps[0];
        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A;
        RbyTile startTile = gb.Tile;
        RbyTile[] endTiles = { pallet[11, 1] };
        RbyTile[] blockedTiles = {
            pallet[1, 5], pallet[2, 5], pallet[7, 2],
            pallet[15, 2], pallet[15, 6], pallet[15, 7],
            pallet[5, 5], pallet[13, 5],
            pallet[10, 1], //pallet[11, 1],
            pallet[1, 11], pallet[2, 11], pallet[3, 11], pallet[4, 11], pallet[5, 11],
            pallet[6, 11], pallet[7, 11], pallet[8, 11], pallet[9, 11],
        };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, pallet[11, 1], actions, blockedTiles);
        // Edge<RbyMap, RbyTile> edge;
        // foreach(RbyTile tile in pallet.Tiles)
        // {
        //     if((edge = tile.GetEdge(0, Action.Left)) != null) edge.Cost = 17;
        //     if((edge = tile.GetEdge(0, Action.Right)) != null) edge.Cost = 17;
        //     if((edge = tile.GetEdge(0, Action.Up)) != null) edge.Cost = 17;
        //     if((edge = tile.GetEdge(0, Action.Down)) != null) edge.Cost = 17;
        // }

        int threshold = 0;
        Paths results = new Paths();
        var parameters = new SFParameters<Red,RbyMap,RbyTile>()
        {
            MaxCost = 140,
            EndTiles = endTiles,
            LogStart = startTile.PokeworldLink + "/",
            FoundCallback = (state, gb) =>
            {
                gb.LoadState(state.IGT.State);
                gb.Hold(Joypad.B, "ManualTextScroll");
                int x = gb.CpuRead("wSprite02StateData2MapX") - 4;
                int y = gb.CpuRead("wSprite02StateData2MapY") - 4;
                // int score = Math.Abs(10 - x) + Math.Abs(6 - y);
                // int score = Math.Abs(10 - x) + Math.Abs(4 - y);
                int score = Math.Abs(10 - x) + Math.Abs(1 - y);
                if(score <= threshold) {
                    if(score < threshold) threshold = score;
                    Path p = new Path(state.Log, 0, state.WastedFrames, x + ";" + y + " -> " + score);
                    Trace.WriteLine(p);
                    results.Add(p);
                }
            }
        };

        SingleFrameSearch.StartSearch(gbs, parameters, startTile, 0, state, 0);
        results.CleanPrintAll();
        Elapsed("search");
    }
}
