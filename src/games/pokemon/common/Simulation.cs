using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using static SimulationUtils;
using System.Text.RegularExpressions;

public class Simulation<Gb> where Gb : GameBoy
{
    public int Iterations;
    public int NumThreads;
    public byte[] State;
    public List<(string, Func<Gb, bool?, double?>)> Variables;

    public Simulation(int iterations = 1000, int numThreads = 20)
    {
        Iterations = iterations;
        NumThreads = numThreads;
        Variables = new List<(string, Func<Gb, bool?, double?>)>();
    }

    public Simulation(byte[] state, int iterations = 1000, int numThreads = 20) : this(iterations, numThreads)
    {
        State = state;
    }

    public Simulation(string statePath, int iterations = 1000, int numThreads = 20) : this(File.ReadAllBytes(statePath), iterations, numThreads)
    {
    }

    public List<double>[] Simulate(string title, byte[] state, Func<Gb, bool> scenario)
    {
        State = state;
        return Simulate(title, scenario);
    }

    public List<double>[] Simulate(string title, string statePath, Func<Gb, bool> scenario)
    {
        return Simulate(title, File.ReadAllBytes(statePath), scenario);
    }

    public Simulation<Gb> Track(params string[] variables)
    {
        foreach(string name in variables)
        {
            if(name == "Time")
                Track(name, (gb, s) => { return s != false ? (double?) gb.EmulatedSamples / 2097152.0 : null; });
            else if(name == "HP" && typeof(Gb).IsSubclassOf(typeof(Rby)))
                Track(name, (gb, s) => { return ((Rby) (object) gb).BattleMon.HP - ((Rby) (object) gb).BattleMon.MaxHP; });
            else if(name == "Success")
                Track(name, (gb, s) => { return s == true ? 1 : 0; });
        }
        return this;
    }

    public Simulation<Gb> Track(string name, Func<Gb, bool?, double?> get)
    {
        Variables.Add((name, get));
        return this;
    }

    public List<double>[] Simulate(string title, Func<Gb, bool> scenario)
    {
        Trace.WriteLine(title);
        List<double>[] data = new List<double>[Variables.Count];
        for(int i = 0; i < data.Length; ++i)
            data[i] = new List<double>(Iterations);

        Gb[] gbs = MultiThread.MakeThreads<Gb>(NumThreads);
        if(NumThreads == 1) gbs[0].Record("test");

        byte[] rngvalues = new byte[Iterations * 3];
        if(Iterations % 65536 == 0)
        {
            for(int i = 0; i < Iterations; ++i)
            {
                rngvalues[3 * i] = (byte) i;
                rngvalues[3 * i + 1] = (byte) (i / 256);
                rngvalues[3 * i + 2] = (byte) (i / 65536);
            }
        }
        else
        {
            new Random(123456789).NextBytes(rngvalues);
        }

        var w = Stopwatch.StartNew();
        MultiThread.For(NumThreads, gbs, (gb, thread) =>
        {
            gb.LoadState(State);
            byte[] state = gb.SaveState();

            for(int i = thread * Iterations / NumThreads; i < (thread + 1) * Iterations / NumThreads; ++i)
            // int i = 241;
            {
                // sf: 0x282 + 0x23c ; gsr: 0x1902 + 0x23c ; cpp: 0x3dd + 0x24b
                state[0x3dd + 0x24b] = rngvalues[3 * i]; // rDIV
                state[0x3dd + 0x1d3] = rngvalues[3 * i + 1]; // HRA
                state[0x3dd + 0x1d4] = rngvalues[3 * i + 2]; // HRS
                gb.LoadState(state);

                double[] start = new double[Variables.Count];
                for(int v = 0; v < Variables.Count; ++v)
                    start[v] = (double) Variables[v].Item2(gb, null);

                bool success = scenario(gb);

                lock(data)
                {
                    for(int v = 0; v < Variables.Count; ++v)
                    {
                        double? end = Variables[v].Item2(gb, success);
                        if(end != null)
                            data[v].Add((double) end - start[v]);
                    }
                }
            }
        });

        Console.WriteLine(w.Elapsed.TotalSeconds + "s");

        for(int i = 0; i < Variables.Count; ++i)
            PrintResults(Variables[i].Item1, data[i]);
        Trace.WriteLine("");
        return data;
    }
}

public static class SimulationUtils
{
    public static void PrintResults(string name, List<double> list)
    {
        if (list.Count > 0)
        {
            list.Sort();
            if (name == "Success")
                Trace.WriteLine(name +
                    $"\n\tAverage: {list.Average():G5}" +
                    $"\n\t50/24:   {Risk(list.Average()):G5}"
                );
            else
                Trace.WriteLine(name +
                    $"\n\tAverage: {list.Average():G5}" +
                    $"\n\tMedian:  {list[list.Count / 2]:G5}" +
                    $"\n\tStdev:   {Stdev(list):G5}" +
                    $"\n\tMin:     {list.Min():G5}" +
                    $"\n\tMax:     {list.Max():G5}"
                );
        }
    }

    public static double Stdev(List<double> list)
    {
        double avg = list.Average();
        return Math.Sqrt(list.Average(v => (v - avg) * (v - avg)));
    }

    public static double Risk(double success)
    {
        return Math.Log(success) / Math.Log(0.50) * 24.0;
    }

    public static void UseMove(this Rby gb, string move)
    {
        if (gb.PC != 0x019C) gb.ClearText();
        gb.BattleMenu(0, 0);
        gb.ChooseMenuItem(Array.IndexOf(gb.BattleMon.Moves, gb.Moves[move]));
        gb.ClearText(Joypad.None, int.MaxValue, 0x0f4696, 0x0f4700);
    }

    public static void UseItem(this Rby gb, string item, int target1 = -1, int target2 = -1)
    {
        if (gb.PC != 0x019C) gb.ClearText();
        gb.BattleMenu(0, 1);
        gb.ChooseListItem(gb.Bag.IndexOf(item));
        if (target1 != -1) gb.ChooseMenuItem(target1);
        if (target2 != -1)
        {
            gb.RunUntil("HandleMenuInput_.getJoypadState");
            gb.ChooseMenuItem(target2);
        }
        gb.ClearText(Joypad.None, int.MaxValue, 0x0f4696, 0x0f4700);
    }
    
    public static void CombineSimulations(string file)
    {
        List<List<double>> trainers = new List<List<double>>();
        foreach(string line in System.IO.File.ReadAllLines(file))
        {
            var m = Regex.Match(line, "([0-9]+): ([0-9.]+)%");
            if(m.Success)
            {
                int hp = int.Parse(m.Groups[1].Value);
                double pct = double.Parse(m.Groups[2].Value);
                if(hp == 0) trainers.Add(new List<double>());
                trainers.Last().Add(pct * 0.01);
            }
        }

        List<double> Combine(List<double> trainer1, List<double> trainer2)
        {
            // int max = Math.Min(trainer1.Count, trainer2.Count);
            int max = trainer1.Count;
            List<double> combined = new List<double>(new double[max]);
            for(int i = 0; i < trainer1.Count; ++i)
            {
                for(int j = 0; j < trainer2.Count; ++j)
                {
                    combined[Math.Min(i + j, max - 1)] += trainer1[i] * trainer2[j];
                }
            }
            return combined;
        }

        // var combined = trainers[2];
        // var combined = Combine(trainers[0], trainers[1]);
        // var combined = Combine(Combine(trainers[0], trainers[1]), trainers[2]);
        // var combined = Combine(trainers[3], trainers[4]);
        // var combined = Combine(Combine(trainers[3], trainers[4]), trainers[6]);
        var combined = Combine(Combine(Combine(Combine(Combine(trainers[0], trainers[1]), trainers[2]), trainers[3]), trainers[4]), trainers[6]);

        double sum = 0;
        double avg = 0;
        int med = -1;
        for(int i = 0; i < combined.Count; ++i)
        {
            Trace.WriteLine($"{i}: {100.0 * combined[i]:F3}%");
            avg += i * combined[i];
            sum += combined[i];
            if(sum >= 0.5 && med == -1) med = i;
        }
        Trace.WriteLine($"Average: {avg:F5} Median: {med}");
    }
}
