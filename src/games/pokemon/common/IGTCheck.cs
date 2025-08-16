using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

public class IGTState {

    public int IGTStamp;
    public bool Success;
    public bool Running = true;
    public byte[] State;
    public int HRA;
    public int HRS;
    public int Divider;

    public int IGTFrame {
        get { return IGTStamp % 60; }
    }

    public int IGTSecond {
        get { return IGTStamp / 60; }
    }

    public int Dsum {
        get { return (HRA + HRS) & 0xff; }
    }

    public IGTState(GameBoy gb, bool success, int igtStamp) {
        IGTStamp = igtStamp;
        Success = success;
        State = gb.SaveState();
        HRA = gb.CpuRead("hRandomAdd");
        HRS = gb.CpuRead("hRandomSub");
        Divider = gb.DividerState;
    }
}

public class IGTResults {

    public IGTState[] IGTs;

    public IGTResults(int num) {
        IGTs = new IGTState[num];
    }

    public IGTState this[int index] {
        get { return IGTs[index]; }
        set { IGTs[index] = value; }
    }

    public int Length {
        get { return IGTs.Length; }
    }

    public int TotalSuccesses {
        get { return IGTs.Where(x => x != null && x.Success).Count(); }
    }

    public int TotalRunning {
        get { return IGTs.Where(x => x != null && x.Running).Count(); }
    }

    public int TotalFailures {
        get { return IGTs.Where(x => x == null || !x.Success).Count(); }
    }

    public int MostCommonHRA {
        get { return IGTs.Where(x => x != null && x.Running).GroupBy(x => x.HRA).OrderByDescending(g => g.Count()).First().Key; }
    }

    public int MostCommonHRS {
        get { return IGTs.Where(x => x != null && x.Running).GroupBy(x => x.HRS).OrderByDescending(g => g.Count()).First().Key; }
    }

    public int MostCommonDivider {
        get { return IGTs.Where(x => x != null && x.Running).GroupBy(x => x.Divider).OrderByDescending(g => g.Count()).First().Key; }
    }

    public byte[] FirstState {
        get { return IGTs.Where(x => x != null && x.Success).First().State; }
    }

    public byte[][] States {
        get { return IGTs.Where(x => x != null).Select(x => x.State).ToArray(); }
    }

    // Returns the number of frames that fall into the most commonly hit RNG band.
    public int RNGSuccesses(int range) {
        return TotalRunning > 0 ? RNGBands(range).Values.Max() : 0;
    }

    // TODO: Use rDiv too???
    public Dictionary<(int, int), int> RNGBands(int range) {
        Dictionary<(int hra, int hrs), int> ret = new Dictionary<(int, int), int>();

        foreach(IGTState i in IGTs) {
            if(i.Running && !ret.ContainsKey((i.HRA, i.HRS))) {
                int count = 0;
                foreach(IGTState j in IGTs)
                    if(j.Running && MathHelper.RangeTest(i.HRA, j.HRA, range) && MathHelper.RangeTest(i.HRS, j.HRS, range))
                        count++;
                ret[(i.HRA, i.HRS)] = count;
            }
        }

        return ret;
    }

    public int RDivSuccesses() {
        if(TotalRunning == 0) return 0;
        Dictionary<int, int> ret = new Dictionary<int, int>();
        foreach(IGTState i in IGTs) {
            if(i.Running) {
                ret.TryAdd(i.Divider, 0);
                ret[i.Divider]++;
            }
        }
        return ret.Values.Max();
    }

    public IGTResults Purge(bool success = false)
    {
        IGTResults ret = new IGTResults(success ? TotalFailures : TotalSuccesses);
        for(int i = 0, j = 0; i < Length; ++i) if(IGTs[i].Success != success) ret[j++] = IGTs[i];
        return ret;
    }
}

public partial class PokemonGame {

    public int OverworldLoopAddress;
    public int WildEncounterAddress;

    public IGTState IGTCheckFrame(IGTState state, Func<bool> fn) {
        return IGTCheckFrame(state, _ => fn == null || fn());
    }

    public IGTState IGTCheckFrame(IGTState initialState, Func<PokemonGame, bool> fn) {
        if(!initialState.Success) return initialState;

        LoadState(initialState.State);
        bool success = fn == null || fn(this);
        return new IGTState(this, success, initialState.IGTStamp);
    }

    public IGTResults IGTCheck(IGTResults initialStates, Func<bool> fn, int ss = 0) {
        IGTResults results = new IGTResults(initialStates.Length);
        int successes = initialStates.Length;
        for(int i = 0; i < initialStates.Length && successes >= ss; i++) {
            results[i] = IGTCheckFrame(initialStates[i], fn);
            if(!results[i].Success) {
                successes--;
            }
        }
        return results;
    }

    public static IGTResults IGTCheckParallel<Gb>(Gb[] gbs, IGTResults initialStates, Func<PokemonGame, bool> fn, int ss = 0) where Gb : PokemonGame {
        IGTResults results = new IGTResults(initialStates.Length);
        int successes = initialStates.Length;
        MultiThread.For(initialStates.Length, gbs, (gb, i) => {
            if(successes < ss) return;
            results[i] = gb.IGTCheckFrame(initialStates[i], fn);
            if(!results[i].Success) {
                Interlocked.Decrement(ref successes);
            }
        });

        return results;
    }

    public static IGTResults IGTCheckParallel<Gb>(int numThreads, IGTResults initialStates, Func<PokemonGame, bool> fn, int ss = 0) where Gb : PokemonGame {
        return IGTCheckParallel(MultiThread.MakeThreads<Gb>(numThreads), initialStates, fn, ss);
    }
}