using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;

using static SearchCommon;
using static RbyIGTChecker<Blue>;

class Sandshrew
{
    const string State = "basesaves/blue/manip/sandshrew.gqs";

    public static void Check()
    {
        CheckIGT(new CheckIGTParameters() {
            StatePath = State,
            Intro = new RbyIntroSequence(RbyStrat.PalHold),
            // Path = "LLLLLLLLLLADDDDDARR" + "RRRR" + "RRRRRRRRRRRDDDDRRRRRRRRRRRRRRRRRRRRRRRRRRUURRRRRRRRRRRRDDDDDDUUD",
            Path = "LLLLLLLLLLDDDDDRARRRRRRRRRRRRRRRRDDDDRRRRRRRRRRRRRRRRRRRRRRRRRRUURRRRRRRRRRRRDD",
            // ForceRedBar = true,
            // MemeBall = gb => {
            //     gb.ClearText();
            //     gb.Press(Joypad.A | Joypad.Down, Joypad.Select, Joypad.A | Joypad.Down | Joypad.Right);
            //     return gb.Hold(Joypad.A, "ItemUseBall.captured", "ItemUseBall.failedToCapture") == gb.SYM["ItemUseBall.captured"];
            // },
            NumFrames = 60,
            TargetPoke = "SANDSHREW",
        });
    }

    public static void Search()
    {
        for(RbyStrat pal = RbyStrat.NoPal; pal <= RbyStrat.PalHold; ++pal)
            Search(new RbyIntroSequence(pal));
    }

    public static void CheckFile()
    {
        Paths paths = new Paths();
        RbyIntroSequence intro = new RbyIntroSequence();
        foreach(string line in System.IO.File.ReadAllLines("paths.txt"))
        {
            string path = System.Text.RegularExpressions.Regex.Match(line, @"/([LRUDSA_B]+) ").Groups[1].Value;
            if(path == "") { intro = new RbyIntroSequence(line); continue; }
            int cost = int.Parse(System.Text.RegularExpressions.Regex.Match(line, "cost: ([0-9]+)").Groups[1].Value);
            int s = CheckIGT(new CheckIGTParameters() {
                StatePath = State,
                Intro = intro,
                Path = path,
                NumFrames = 60,
                TargetPoke = "SANDSHREW",
            });
            Path p = new Path(path, s, cost, intro[0].LogString());
            paths.Add(p);
        }
        paths.CleanPrintAll("https://gunnermaniac.com/pokeworld?local=15#28/3/");
    }

    public static void Search(RbyIntroSequence intro, int numThreads = 16, int numFrames = 60, int success = 52, int cost = 6)
    {
        StartWatch();
        Trace.WriteLine(intro);

        Blue[] gbs = MultiThread.MakeThreads<Blue>(numThreads);
        Blue gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        gb.LoadState(State);
        IGTResults states = Blue.IGTCheckParallel(gbs, intro, numFrames, null, 0, 20);

        RbyMap moon3 = gb.Maps[61];
        RbyMap moon2 = gb.Maps[60];
        RbyMap route4 = gb.Maps[15];
        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A | Action.StartB;
        RbyTile startTile = gb.Tile;
        RbyTile[] blockedTiles = { moon3[7, 4], moon3[7, 3], moon3[8, 3], moon3[9, 3], moon3[10, 3], moon3[11, 3], moon3[12, 3], moon3[13, 3], moon3[14, 2],
            route4[64, 11], route4[65, 11], route4[66, 11], route4[67, 11], route4[68, 11], route4[69, 11], route4[70, 11], route4[71, 11], route4[72, 11],
        };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, route4[73, 14], actions, blockedTiles);

        var parameters = new DFParameters<Blue, RbyMap, RbyTile>()
        {
            MaxCost = cost,
            SuccessSS = success,
            EncounterCallback = gb => gb.EnemyMon.Species.Name == "SANDSHREW" && gb.EnemyMon.Level < 10 && gb.Tile.X >= 71 && gb.Yoloball(1),
            LogStart = "https://gunnermaniac.com/pokeworld?local=15#28/3/",
            FoundCallback = state =>
            {
                Trace.WriteLine(new Path(state.Log, state.IGT.TotalSuccesses, state.WastedFrames));
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states);
        Elapsed("search");
    }
}
