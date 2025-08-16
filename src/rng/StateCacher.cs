using System;
using System.IO;

public class StateCacher {

    private bool CacheCleared;
    private string CachedStatesDirectory;

    public StateCacher(string directoryName) {
        CachedStatesDirectory = "rng-cache/" + directoryName;
        if(!Directory.Exists(CachedStatesDirectory)) Directory.CreateDirectory(CachedStatesDirectory);
    }

    public void CacheState(GameBoy gb, string name, System.Action fn) {
        string state = CachedStatesDirectory + "/" + name + ".gqs";
        if(!CacheCleared && File.Exists(state)) {
            gb.LoadState(state);
        } else {
            fn();
            gb.SaveState(state);
        }

        ulong cc = gb.EmulatedSamples;
        TimeSpan time = TimeSpan.FromSeconds((double) cc / 2097152.0);
        // if(name=="end")
        // Console.WriteLine("{0}: {1} ({2:n0})", name, time.ToString(@"hh\:mm\:ss\.fff"), cc);
        System.Diagnostics.Trace.WriteLine(time.ToString(@"hh\:mm\:ss\.fff"));
        // System.Diagnostics.Trace.WriteLine(name.Substring(0, 3) + " " + time.ToString(@"hh\:mm\:ss\.fff"));
    }

    public void ClearCache() {
        CacheCleared = true;
    }
}