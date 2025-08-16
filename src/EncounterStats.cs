using System;
using System.Collections.Generic;
using System.Diagnostics;

public class EncounterStats
{
    // static double[] stats = new double[100];
    // static int[] path = { 68 };
    // static int totaltiles = path.Sum();
    // static void Tile(int t, double p, int encounters = 0, int immunity = 0)
    // {
    //     if(t == totaltiles)
    //     {
    //         stats[encounters] += p;
    //         return;
    //     }

    //     int total = 0;
    //     for(int i = 0; i < path.Length && total < t; ++i)
    //         total += path[i];
    //     if(t == total)
    //         immunity = 0;

    //     if(immunity > 0)
    //     {
    //         Tile(t + 1, p, encounters, immunity - 1);
    //     }
    //     else
    //     {
    //         Tile(t + 1, p * rate, encounters + 1, 2);
    //         Tile(t + 1, p - p * rate, encounters, 0);
    //     }
    // }
    // static void Route1old()
    // {
    //     Tile(0, 1);
    //     double avg = 0;
    //     for(int i = 0; i < 20; ++i)
    //     {
    //         Trace.WriteLine($"{i}: {100 * stats[i]:F3}%");
    //         avg += stats[i] * i;
    //         if(stats[i] == 0) break;
    //     }
    //     Trace.WriteLine("Avg: " + avg);
    // }

    const int size = 128;
    double rate;
    Dictionary<int, double[]> Cache;
    int[] ladders;

    static double[] Combine(double[] patch1, double[] patch2)
    {
        double[] result = new double[size];
        for(int i = 0; patch1[i] != 0 || i == 0; ++i)
            for(int j = 0; patch2[j] != 0; ++j)
                result[i + j] += patch1[i] * patch2[j];
        return result;
    }

    static double[] Multiply(double p, double[] branch)
    {
        double[] result = new double[size];
        for(int i = 0; branch[i] != 0 || i == 0; ++i)
            result[i] = p * branch[i];
        return result;
    }

    static double[] Add(double[] branch1, double[] branch2)
    {
        double[] result = new double[size];
        for(int i = 0; branch1[i] != 0 || branch2[i] != 0 || i == 0; ++i)
            result[i] = branch1[i] + branch2[i];
        return result;
    }

    static double[] PlusOne(double[] branch)
    {
        double[] result = new double[size];
        for(int i = 0; branch[i] != 0 || i == 0; ++i)
            result[i + 1] = branch[i];
        return result;
    }

    double[] Patch(int tiles)
    {
        if(tiles < 0) tiles = 0;
        if(Cache.ContainsKey(tiles)) return Cache[tiles];

        int immune = 3;
        foreach(int l in ladders)
            if(tiles == l + 2) immune = 4;

        return Cache[tiles] = Add(
            Multiply(rate, PlusOne(Patch(tiles - immune))),
            Multiply(1.0 - rate, Patch(tiles - 1))
        );
    }

    public EncounterStats(int[] path, int rate, int[][] ladders = null)
    {
        this.rate = rate / 256.0;
        if(ladders == null) ladders = new int[path.Length][]; for(int i = 0; i < path.Length; ++i) if(ladders[i] == null) ladders[i] = new int[]{};

        double[] empty = new double[size];
        empty[0] = 1.0;

        double[] stats = empty;
        int p = 0;
        do {
            Cache = new Dictionary<int, double[]>() { {0, empty} };
            this.ladders = ladders[p];
            stats = Combine(stats, Patch(path[p]));
        } while(++p < path.Length);

        double avg = 0;
        for(int i = 0; stats[i] != 0; ++i)
        {
            Trace.WriteLine($"{i}: {100 * stats[i]:F3}%");
            avg += stats[i] * i;
        }
        Trace.WriteLine("Avg: " + avg);
    }

    static public void Start()
    {
        const int bush1_up = 3, bush1_down = 4, bush2 = 2, bush3 = 3, bush4 = 3, bush4_troll = 4, bush5 = 4;
        int[] route1_full = { bush1_up, bush2, bush3, bush4, bush5, bush1_down, bush1_up, bush2, bush3, bush4_troll, bush5 };
        int[] route1_yolo = { bush1_up, bush2, bush3, bush4, bush5, bush3, bush1_down, bush1_up, bush2, bush3, bush4_troll, bush5 };
        int[] route1_up = { bush1_up, bush2, bush3, bush4, bush5 };
        int[] route1_up_troll = { bush1_up, bush2, bush3, bush4_troll, bush5 };

        const int tonerd = 46, wg = 22, tolass = 39, tohiker = 43, lasstoladder = 54, hikertoladder = 9, b1f = 27, b2f = 80, rockettonerd = 7, fossiltoladder = 13, toend = 3;
        int[] rockettoend = { rockettonerd, fossiltoladder + toend };
        int[][] rockettoend_ladders = { null, new int[]{ toend } };
        int[] torocket = { tonerd, tolass, lasstoladder + b1f + b2f };
        int[][] torocket_ladders = { null, null, new int[]{ b1f + b2f, b2f } };
        int[] torocketwg = { tonerd + wg, tolass, tohiker, hikertoladder + b1f + b2f };
        int[][] torocketwg_ladders = { null, null, null, new int[]{ b1f + b2f, b2f } };
        int[] fullmoon = { tonerd, tolass, lasstoladder + b1f + b2f, rockettonerd, fossiltoladder + toend };
        int[][] fullmoon_ladders = { null, null, new int[]{ b1f + b2f, b2f }, null, new int[]{ toend } };
        int[] fullmoonwg = { tonerd + wg, tolass, tohiker, hikertoladder + b1f + b2f, rockettonerd, fossiltoladder + toend };
        int[][] fullmoonwg_ladders = { null, null, null, new int[]{ b1f + b2f, b2f }, null, new int[]{ toend } };

        int[] route6 = { 4, 3 };
        int[] forest = { 3, 2, 3, 1, 1, 4, 12}; int[][] forest_immune = { null, null, new int[]{ 1, 0 }, null, null, null, null }; // immune = 2
        int[] safari = { 1, 1 };

        var w = Stopwatch.StartNew();

        // new EncounterStats(fullmoonwg, 10, fullmoonwg_ladders);
        // new EncounterStats(route1_full, 25);
        // new EncounterStats(safari, 30);
        // new EncounterStats(forest, 8, forest_immune);
        // new EncounterStats(new int[]{ 10 }, 8);
        new EncounterStats(new int[]{ 2 }, 20);
        new EncounterStats(new int[]{ 5 }, 20);
        new EncounterStats(new int[]{ 10 }, 20);

        Console.WriteLine(w.Elapsed.TotalSeconds + "s");
    }
}
