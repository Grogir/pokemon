﻿using System.IO;
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

    static void Main(string[] args) {
        Trace.Listeners.Add(new TextWriterTraceListener(File.CreateText("log.txt")));
        Trace.AutoFlush = true;

        // Tests.RunAllTests();
        // new BlueNidoTas();
        // new SCTTas();
        // new YellowTASPidgeFable();

        new RedTasTest();
        new RedComparisons();
        new RedGlitchlessWR();
    }
}
