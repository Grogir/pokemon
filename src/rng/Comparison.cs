using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class Comparison
{
    GameBoy Gb;

    public Comparison(GameBoy gb)
    {
        Gb = gb;
    }

    public delegate void Scenario();

    public void Compare(string name, byte[][] states, Scenario[] scenarios, bool video = true, bool record = true, int wait = 300, int ratio = 1)
    {
        Gb.SetSpeedupFlags(SpeedupFlags.None);

        TimerComponent[] timers = new TimerComponent[scenarios.Length];
        double[] times = new double[scenarios.Length];
        double longest = 0;

        for(int i = scenarios.Length - 1; i >= 0; --i)
        {
            if(states[i] != null)
                Gb.LoadState(states[i]);
            timers[i] = new TimerComponent(0, 144 * ratio, 2.0f * ratio);
            if(video)
            {
                new Scene(Gb, 160 * ratio, 160 * ratio);
                Gb.Scene.AddComponent(new VideoBufferComponent(0, 0, 160 * ratio, 144 * ratio));
                if(record) Gb.Scene.AddComponent(new RecordingComponent((i + 1).ToString()));
                Gb.Scene.AddComponent(timers[i]);
            }
            timers[i].OnInit(Gb);

            scenarios[i]();

            timers[i].Stop();
            times[i] = timers[i].Duration().TotalSeconds;
            Gb.AdvanceFrames(wait);
            if(times[i] > longest)
                longest = times[i];
            else
                Gb.AdvanceFrames((int) ((longest - times[i]) * 59.7));
            if(video) Gb.Scene.Dispose();
        }

        string movies = "";
        for(int i = 0; i < scenarios.Length; ++i)
        {
            Console.Write((i + 1) + ":  " + (video ? timers[i].Text : times[i].ToString("F3")) + "  ");
            for(int j = 0; j < scenarios.Length; ++j)
            {
                if(i != j)
                {
                    double delta = times[i] - times[j];
                    Console.Write(" " + (delta < 0 ? "" : "+") + delta.ToString("F3"));
                }
            }
            movies += "-i movies/" + (i + 1) + ".mp4 ";
            Console.WriteLine();
        }

        if(video && record) FFMPEG.RunFFMPEGCommand("-y " + movies + "-filter_complex hstack=inputs=" + scenarios.Length + " movies/" + name + ".mp4");
        // if(video && record) FFMPEG.RunFFMPEGCommand("-y " + movies + "-c:v libx265 -filter_complex hstack=inputs=" + scenarios.Length + " movies/" + name + ".mp4");
    }

    public void Compare(string name, byte[] state, Scenario[] scenarios, bool video = true, bool record = true, int wait = 300, int ratio = 1)
    {
        byte[][] states = new byte[scenarios.Length][];
        for(int i = 0; i < scenarios.Length; ++i) states[i] = state;
        Compare(name, states, scenarios, video, record, wait, ratio);
    }

    public void Compare(string name, string statepath, Scenario[] scenarios, bool video = true, bool record = true, int wait = 300, int ratio = 1)
    {
        Compare(name, File.ReadAllBytes(statepath), scenarios, video, record, wait, ratio);
    }

    public void Compare(string statepath, Scenario[] scenarios, bool video = true, bool record = true, int wait = 300, int ratio = 1)
    {
        string name = Regex.Match(statepath, @"([^/\\]+)\.gqs").Groups[1].Value;
        Compare(name, statepath, scenarios, video, record, wait, ratio);
    }

    public void Compare(string name, byte[] leftstate, byte[] rightstate, params Scenario[] scenarios)
    {
        Compare(name, new byte[][] { leftstate, rightstate }, scenarios);
    }

    public void Compare(string name, string leftpath, string rightpath, params Scenario[] scenarios)
    {
        Compare(name, File.ReadAllBytes(leftpath), File.ReadAllBytes(rightpath), scenarios);
    }

    public void Compare(string name, byte[] state, params Scenario[] scenarios)
    {
        Compare(name, state, scenarios, true);
    }

    public void Compare(string name, string statepath, params Scenario[] scenarios)
    {
        Compare(name, statepath, scenarios, true);
    }

    public void Compare(string statepath, params Scenario[] scenarios)
    {
        Compare(statepath, scenarios, true);
    }
}

public class RbyForceComparisons : RbyForce
{
    public static RbyIntroSequence NoPal = new RbyIntroSequence(RbyStrat.NoPal);
    public static RbyIntroSequence PalHold = new RbyIntroSequence(RbyStrat.PalHold);
    public TimerComponent Timer = new TimerComponent(0, 144, 2.0f);

    public static string SpacePath(string path)
    {
        string output = "";
        string[] validActions = new string[] { "A", "U", "D", "L", "R", "S", "S_B" };
        while(path.Length > 0)
        {
            if(validActions.Any(path.StartsWith))
            {
                if(path.StartsWith("S_B"))
                {
                    output += "S_B";
                    path = path.Remove(0, 3);
                }
                else if(path.StartsWith("S"))
                {
                    output += "S_B";
                    path = path.Remove(0, 1);
                }
                else
                {
                    output += path[0];
                    path = path.Remove(0, 1);
                }

                output += " ";
            }
            else
            {
                throw new Exception(String.Format("Invalid Path Action Recieved: {0}", path));
            }
        }
        return output.Trim();
    }

    public void MoveAndSplit(Joypad j)
    {
        Inject(j);
        AdvanceFrames(16, j);
    }

    public void AfterMoveAndSplit()
    {
        do
        {
            RunFor(1);
            RunUntil("JoypadOverworld");
        } while((CpuRead("wd730") & 0xa0) > 0);
    }

    public void ReceiveItemAndSplit()
    {
        Press(Joypad.A);
        ClearTextUntil(Joypad.None, SYM["GiveItem"]);
        RunUntil("PlaySound");
    }

    public void ForceTurnAndSplit(RbyTurn playerTurn, RbyTurn enemyTurn = null, bool speedTieWin = true)
    {
        ForceTurn(playerTurn, enemyTurn, speedTieWin, false);
        ClearTextUntil(Joypad.None, SYM["EnterMap"]);
    }

    public void SaveAndQuit()
    {
        Save();
        RunUntil(SYM["SaveSAV.save"] + 0x3);
        HardReset(true);
    }

    public void RecordAndTime(string movie, bool start = false)
    {
        SetSpeedupFlags(SpeedupFlags.None);
        Scene s = new Scene(this, 160, 160);
        s.AddComponent(new VideoBufferComponent(0, 0, 160, 144));
        s.AddComponent(new RecordingComponent(movie));
        s.AddComponent(Timer);
        Timer.Running = start;
    }

    public void ScrollTo(string target)
    {
        ScrollTo(FindItem(target));
    }

    public void ScrollTo(int target)
    {
        OpenBag();
        ListScroll(target, Joypad.B, true);
        CurrentMenuType = MenuType.StartMenu;
    }

    public void Inventory()
    {
        for(int i = 0; i < Bag.NumItems; ++i)
            System.Console.WriteLine((i + 1) + " " + Bag[i].Item + " x" + Bag[i].Quantity);
    }

    public RbyForceComparisons(string rom, SpeedupFlags speedupFlags) : base(rom, speedupFlags)
    {
    }
}

public class RedBlueForceComparisons : RbyForceComparisons
{
    public RedBlueForceComparisons(string rom = "roms/pokered.gbc", bool speedup = true) : base(rom, speedup ? SpeedupFlags.All : SpeedupFlags.None)
    {
    }

    public RedBlueForceComparisons(bool speedup) : this("roms/pokered.gbc", speedup)
    {
    }
}

public class YellowForceComparisons : RbyForceComparisons
{
    public YellowForceComparisons(bool speedup = true) : base("roms/pokeyellow.gbc", speedup ? SpeedupFlags.NoVideo | SpeedupFlags.NoSound : SpeedupFlags.None)
    {
    }
}
