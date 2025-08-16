using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;

using static SearchCommon;
using static RbyIGTChecker<Ao>;

class Cans
{
    public static void Check()
    {
        // Check<Red>(new RbyIntroSequence(RbyStrat.NoPal), "SDALLLAURAUUUUUA"); // 60 cans - 3596/3600
        // Check<Red>(new RbyIntroSequence(RbyStrat.Pal, RbyStrat.GfSkip, RbyStrat.Hop0, 1), "LLURUUUUUA"); // 59 cans - 3539/3600
        // Check<Red>(new RbyIntroSequence(RbyStrat.NoPal), "DALLLAURUUUUUA"); // 58 cans - 3477/3600
        // Check<Red>(new RbyIntroSequence(RbyStrat.NoPal), "DLLLURUUUUUA"); // 57 cans - 3420/3600

        // Check<Red>(new RbyIntroSequence(RbyStrat.NoPal), "DDLLLUURUUUUUA"); // extra step 57 - 3419
        // Check<Red>(new RbyIntroSequence(RbyStrat.NoPal), "DDALLLUURUUUUUA"); // extra step 58 - 3361
        // Check<Red>(new RbyIntroSequence(RbyStrat.NoPal), "DDLALLUURUUUUUA"); // extra step 58 - 3361
        // Check<Red>(new RbyIntroSequence(RbyStrat.NoPal), "DLLLU" + "RUUUUULUUUUUUURDA"); // xd

        // Check<Red>(new RbyIntroSequence(RbyStrat.PalHold), "DLLLURUUUUUA"); // first can igt
        // Check<Red>(new RbyIntroSequence(RbyStrat.NoPalAB, RbyStrat.GfSkip, RbyStrat.Hop0, 1), "DLLLURUUUUUA"); // 60 second can igt
        // Check<Red>(new RbyIntroSequence(RbyStrat.PalAB), "DLLLURRRRRUUUUUA"); // 60 second can igt

        Check<Blue>(new RbyIntroSequence(RbyStrat.NoPal, RbyStrat.GfSkip, RbyStrat.Hop0, 1)); // blue 57

        // for(RbyStrat pal = RbyStrat.NoPal; pal <= RbyStrat.PalRel; ++pal)
        // {
        //     for(int i = 1; i <= 6; ++i)
        //     {
        //         Trace.WriteLine(pal + " " + i);
        //         Check<Blue>(new RbyIntroSequence(pal), "DLLLURUUUUUA", i, false);
        //         Trace.WriteLine("");
        //     }
        // }
    }

    public static void Ao()
    {
        Check<Ao>(new RbyIntroSequence(RbyStrat.PalHold), "DLLLAURAUUUUUA", 1);
        Check<Ao>(new RbyIntroSequence(RbyStrat.PalHold), "DLLLAURAUUUUUA", 3);
        Check<Ao>(new RbyIntroSequence(RbyStrat.PalHold), "DLLLAURAUUUUUA", 5);

        Check<Ao>(new RbyIntroSequence(RbyStrat.PalAB), "DLLLURUUUUUA", 2);
        Check<Ao>(new RbyIntroSequence(RbyStrat.PalAB), "DLLLAURAUUUUUA", 2); // 58 --
        Check<Ao>(new RbyIntroSequence(RbyStrat.PalAB), "DALLLURAUUAUUUA", 2); // 58

        Check<Ao>(new RbyIntroSequence(RbyStrat.PalHold), "LLAURUUUUUA", 4); // 57

        Check<Ao>(new RbyIntroSequence(RbyStrat.PalHold), "URUUUUUA", 6); // 57
    }

    public static void Check<Gb>(RbyIntroSequence intro, string path = "DLLLURUUUUUA", int state = 2, bool fullreport = true) where Gb : Rby
    {
        int numFrames = 60;
        int numThreads = 16;

        Gb[] gbs = MultiThread.MakeThreads<Gb>(numThreads);
        Gb gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        gb.LoadState("basesaves/red/manip/cans/cans" + state + ".gqs");
        gb.HardReset();
        intro.ExecuteUntilIGT(gb);
        byte[] igtState = gb.SaveState();

        var full = new List<string>();
        var results = new Dictionary<(byte first, byte second), int>();

        MultiThread.For(numFrames, gbs, (gb, f) =>
        {
            gb.LoadState(igtState);
            gb.CpuWrite("wPlayTimeSeconds", (byte) (f / 60));
            gb.CpuWrite("wPlayTimeFrames", (byte) (f % 60));
            // gb.CpuWrite("wPlayTimeMinutes", (byte) (f % 60));
            // gb.CpuWrite("wPlayTimeSeconds", (byte) (54 + 3*(f / 2)));
            // gb.CpuWrite("wPlayTimeFrames", (byte) (36 + f % 2));

            intro.ExecuteAfterIGT(gb);
            gb.Execute(SpacePath(path));

            (byte first, byte second) cans = (gb.CpuRead("wFirstLockTrashCanIndex"), gb.CpuRead("wSecondLockTrashCanIndex"));
            lock(results)
            {
                full.Add($"{f / 60,2} {f % 60,2}: {cans.first},{cans.second}");
                if(!results.ContainsKey(cans))
                    results.Add(cans, 1);
                else
                    results[cans]++;
            }
        });
        if(fullreport)
        {
            full.Sort();
                foreach(string line in full)
                    Trace.WriteLine(line);
            Trace.WriteLine("");
        }
        foreach(var cans in results)
            Trace.WriteLine(cans.Key.first + "," + cans.Key.second + ": " + cans.Value);
    }
}
