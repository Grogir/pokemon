using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

class Program {
    static void IntroDelay()
    {
        // Red gb = new Red();
        Yellow gb = new Yellow();
        // byte[] state = System.IO.File.ReadAllBytes("basesaves/red/introdelay.gqs");
        byte[] state = System.IO.File.ReadAllBytes("basesaves/yellow/introdelay.gqs");
        int[] results = new int[300];
        for(int i = 0; i < 10000; ++i)
        {
            gb.LoadState(state);
            gb.AdvanceFrame();
            state = gb.SaveState();
            int frames = 0;
            do
            {
                gb.AdvanceFrame(Joypad.B);
                frames++;
            } while(gb.CpuRead("wPlayTimeFrames") == 0);
            // Trace.WriteLine(frames - 1 + " = 0");// (" + frames + " = 1)");
            results[frames - 1]++;
        }
        for(int i = 200; i < 300; ++i)
            if(results[i] != 0)
                Trace.WriteLine(i + ": " + results[i]);
    }
    static void Silphbar(int spc = 15, bool desperate = false, int maxhp = 999)
    {
        //               0    1    2    3    4    5    6    7    8   9  10  11  12  13  14  15  gl
        int[] plus1 = {110, 108, 108, 108, 104, 104, 104, 102, 102, 98, 98, 98, 98, 96, 96, 96, 104};
        int[] plus2 = { 84,  84,  80,  80,  80,  78,  78,  78,  78, 74, 74, 74, 74, 74, 74, 72, 78};
        int maxroll1 = plus1[spc];
        int maxroll2 = plus2[spc];
        float eval(int h)
        {
            // if(h >= 1 && h <= 18) return 1.0f;
            // if(h >= 19 && h <= 21) return 0.5f;
            // if(h >= 22 && h <= 25) return 0.25f;
            if(h >= 1 && h <= 15) return 1.0f;
            if(h >= 16 && h <= 18) return 0.5f;
            if(h >= 19 && h <= 21) return 0.25f;
            if(h <= 0 && !desperate) return -1.0f; // standard
            if(h <= 0 && desperate) return -0.25f; // desperate
            return 0.0f;
        }
        string prev = "";
        string current = "";
        int firsthp = 0;
        int hp;
        bool xspec2 = false;
        int xspec2hp = 0;
        string L36 = "";
        string L37 = "";
        for(hp = 40; hp <= 120; ++hp)
        {
            float score1 = 0, score2 = 0, scorepot = 0, scoresuper = 0, score2pot = 0;//, score2super = 0;
            for(int r = 217; r <= 255; ++r)
            {
                int roll1 = maxroll1 * r / 255;
                int roll2 = maxroll2 * r / 255;
                score1 += eval(hp - roll1);
                scorepot += eval(Math.Min(hp + 20, maxhp) - roll1);
                scoresuper += eval(Math.Min(hp + 50, maxhp) - roll1);
                score2 += eval(hp - roll2);
                score2pot += eval(Math.Min(hp + 20, maxhp) - roll2);
                // score2super += eval(Math.Min(hp + 50, maxhp) - roll2);
            }
            // if(score1 < 5 && score2 < 5 && scorepot < 5 && scoresuper < 5)
            //     current = "Drill";
            if(score1 >= score2 && score1 >= scorepot && score1 >= scoresuper)
                current = "Flute";
            else if(score2 >= score1 && score2 >= scorepot && score2 >= scoresuper)
                current = "X Spec";
            else if(scorepot >= score1 && scorepot >= score2 && scorepot >= scoresuper)
                current = "Potion";
            else if(scoresuper >= score1 && scoresuper >= score2 && scoresuper >= scorepot)
                current = "Super";
            if(current != prev && scoresuper >= 0)
            {
                if(prev != "") L37 += "\t" + firsthp + "-" + (hp - 1) + " " + prev + "\n";
                firsthp = hp;
                prev = current;
            }
            // Trace.WriteLine(hp + " ; flute:" + score1 + " ; xspec:" + score2 + " ; pot:" + scorepot + " ; super:" + scoresuper + " ; 2pot:" + score2pot/* + " ; 2super:" + score2super*/);
            if(score2pot >= score1 + 4 && score2pot >= score2 + 4 && score2pot >= scorepot + 4 && score2pot >= scoresuper + 4)
            {
                if(!xspec2) xspec2hp = hp;
                xspec2 = true;
            }
            else if(xspec2)
            {
                xspec2 = false;
                // L36 += "\t" + (xspec2hp - 3) + "-" + (hp - 4) + " X Spec x2\n"; // level before gyara
                L36 += "\t" + xspec2hp + "-" + (hp - 1) + " X Spec x2\n"; // level after gyara
            }
                // Trace.WriteLine((hp - 3) + " X Spec x2");
            // if(score1 < 20 && score2 < 20 && scorepot < 20 && scoresuper < 20)
            //     Trace.WriteLine((hp - 3) + " bad");
        }
        L37 += "\t" + firsthp + "-" + (hp - 1) + " " + prev + "\n";

        Trace.WriteLine(spc + " SPC");
        Trace.WriteLine("Pidgeot\n" + L36 + "Gyarados\n" + L37);
    }

    static void Main(string[] args)
    {
        Trace.Listeners.Add(new TextWriterTraceListener(File.CreateText("log.txt")));
        Trace.AutoFlush = true;

        // Tests.RunAllTests();

        new RedComparisons();
    }
}
