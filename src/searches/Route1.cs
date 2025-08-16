using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;

using static SearchCommon;
using static RbyIGTChecker<Blue>;
using System.Threading;

class Route1
{
    const string State = "basesaves/blue/manip/route1.gqs";
    const string CurrentPath = "UUUUUUUUALLUUUURRRRUAUULLLUUUUUURRRRRUUUUUUUUUUUUULLLUUUUUUUULLUUUUUUUUUUUUUUUUUUUUUUUUUUUULLUUUUUUUUUUUUUUUUULLLUUUUURRRRUUUUU"
    // + "UUULLLLLU" + "RUUUUUUU" + "U"
    ;

    public static void Check()
    {
        CheckIGT(new CheckIGTParameters() {
            StatePath = State,
            Intro = new RbyIntroSequence(RbyStrat.PalHold),
            Path = "",
            NumFrames = 60,
        });
    }

    public static void Search()
    {
        Search(new RbyIntroSequence(RbyStrat.PalHold));
        SearchForest(new RbyIntroSequence(RbyStrat.PalHold));

        foreach(string line in System.IO.File.ReadAllLines(@"E:\PY\Documents\Poké\clef\weedlepaths.txt"))
        {
            var r = System.Text.RegularExpressions.Regex.Match(line, @"([0-9]+) ([0-9]+) ([0-9]+)/([0-9]+) [^LRUDSA_B]*/([LRUDSA_B]+) [0-9]+ a:[0-9]+ t:[0-9]+ 1:[0-9]+ 2:[0-9]+ 6:([0-9]+)\-([0-9]+)");
            if(r.Success)
            {
                Trace.WriteLine($"[{int.Parse(r.Groups[1].Value)}, {int.Parse(r.Groups[2].Value)}, {int.Parse(r.Groups[3].Value)}, {int.Parse(r.Groups[4].Value)}, 9, {int.Parse(r.Groups[6].Value)}, {int.Parse(r.Groups[7].Value)}, '', 'https://gunnermaniac.com/pokeworld?local=51#21/59/{r.Groups[5].Value}'],");
            }
        }
    }

    public static void Search(RbyIntroSequence intro, int numThreads = 16, int numFrames = 60, int success = 60, int cost = 4)
    {
        StartWatch();
        Trace.WriteLine(intro);

        Blue[] gbs = MultiThread.MakeThreads<Blue>(numThreads);
        Blue gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        gb.LoadState(State);
        // IGTResults states = Blue.IGTCheckParallel(gbs, intro, numFrames);
        // IGTResults states = Blue.IGTCheckParallel(gbs, intro, numFrames, gb => gb.Execute(SpacePath(CurrentPath)) == gb.OverworldLoopAddress);
        IGTResults states = Blue.IGTCheckParallel(gbs, intro, numFrames, gb => {
            if(gb.Execute(SpacePath(CurrentPath
            + "UUURUURRRRRRRUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUULLLLLLLLDDDDDDDLLLLUUUUUUUUUUUUULLLLLLDDDDDDDDDDDDDADDDDADLADLLLLUUU"),
            (gb.Maps[51][25, 12], gb.PickupItem)) == gb.OverworldLoopAddress)
            { 
                    gb.Press(Joypad.A);
                    gb.ClearText(1);
                    gb.Inject(Joypad.None);
                    gb.AdvanceFrames(1);
                    gb.Inject(Joypad.B);
                    gb.AdvanceFrame(Joypad.B);
                    gb.ClearText(Joypad.B);
                    gb.Press(Joypad.A, Joypad.Down, Joypad.A);
                 return true; } else return false;
        });
        // RNGDebug(states); return;

        RbyMap route2 = gb.Maps[13];
        Action actions = Action.Right | Action.Down | Action.Up | Action.Left | Action.A;
        RbyTile startTile = gb.Tile;
        RbyTile[] blockedTiles = { route2[4, 51] };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, route2[8, 47], actions, blockedTiles);

        Paths results = new Paths();
        var parameters = new DFParameters<Blue, RbyMap, RbyTile>()
        {
            MaxCost = cost,
            SuccessSS = success,
            EndTiles = new RbyTile[] { route2[8, 47] },
            MaxTurns = 22,
            LogStart = "https://gunnermaniac.com/pokeworld?map=1#60/234/",
            FoundCallback = state =>
            {
                Path p = new Path(state.Log, state.IGT.TotalRunning, state.WastedFrames);
                Trace.WriteLine(p);
                results.Add(p);
            }
        };

        DepthFirstSearch.StartSearch(gbs, parameters, startTile, 0, states);
        results.CleanPrintAll();
        Elapsed("search");
    }

    public static void SearchForest(RbyIntroSequence intro, int numThreads = 16, int igtframe = 15, int cost = 8)
    {
        StartWatch();
        Blue[] gbs = MultiThread.MakeThreads<Blue>(numThreads);
        Blue gb = gbs[0];
        if(numThreads == 1) gb.Record("test");

        gb.LoadState(State);
        IGTResults states = Blue.IGTCheckParallel(gbs, intro, 60, gb => gb.Execute(SpacePath(CurrentPath)) == gb.OverworldLoopAddress);

        RbyMap route2 = gb.Maps[13];
        RbyMap gate = gb.Maps[50];
        RbyMap forest = gb.Maps[51];
        forest.Sprites.Remove(25, 11);
        Action actions = Action.Right | Action.Left | Action.Up | Action.Down | Action.A;
        RbyTile startTile = gb.Tile;
        RbyTile[] endTiles = { forest[2, 19] };
        RbyTile[] blockedTiles = {
            forest[26, 12],
            forest[16, 10], forest[18, 10],
            forest[16, 15], forest[18, 15],
            forest[11, 15], forest[12, 15],
            forest[11, 4], forest[12, 4],
            forest[6, 4], forest[8, 4],
            forest[6, 15], forest[8, 15],
        };
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, endTiles[0], actions, blockedTiles);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, gate[5, 1], actions, blockedTiles);
        Pathfinding.GenerateEdges<RbyMap, RbyTile>(gb, 0, route2[3, 44], actions, blockedTiles);
        for(int x = 4; x <= 7; ++x) for(int y = 45; y <= 46; ++y) route2[x, y].RemoveEdge(0, Action.Up);
        route2[3, 44].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = gate[4, 7], NextEdgeset = 0, Cost = 0 });
        gate[4, 7].GetEdge(0, Action.Right).Cost = 0;
        gate[4, 7].RemoveEdge(0, Action.A);
        gate[4, 7].RemoveEdge(0, Action.StartB);
        gate[5, 1].AddEdge(0, new Edge<RbyMap, RbyTile>() { Action = Action.Up, NextTile = forest[17, 47], NextEdgeset = 0, Cost = 0 });
        forest[25, 12].RemoveEdge(0, Action.A);
        forest[25, 13].RemoveEdge(0, Action.A);
        forest[2, 19].RemoveEdge(0, Action.A);
        forest[2, 20].RemoveEdge(0, Action.A);

        const int weedleframes = 3;
        var results = new List<SFState<RbyMap, RbyTile>>();

        var stats = new List<(int atk, int def, int hp, int maxhp)>();
        for(int maxhp = 21; maxhp <= 23; ++maxhp)
            for(int hp = maxhp - 9; hp <= maxhp; ++hp)
                if(hp != maxhp - 1)
                    for(int atk = 11; atk <= 12; ++atk)
                        for(int def = 12; def <= 14; ++def)
                            stats.Add((atk, def, hp, maxhp));

        Paths[] paths = new Paths[stats.Count];
        for(int i = 0; i < stats.Count; ++i) paths[i] = new Paths();
        string link = "https://gunnermaniac.com/pokeworld?map=51#17/46/";
        var parameters = new SFParameters<Blue, RbyMap, RbyTile>()
        {
            MaxCost = cost,
            EndTiles = endTiles,
            TileCallback = (forest[25, 12], gb => gb.PickupItem()),
            FoundCallback = (state, gb) =>
            {
                gb.LoadState(state.IGT.State);
                gb.Press(Joypad.A);
                gb.ClearText(1);
                gb.Inject(Joypad.None);
                byte[] textboxstate = gb.SaveState();
                byte[][] battlestates = new byte[3][];
                for(int weedleframe = 0; weedleframe < weedleframes; ++weedleframe)
                {
                    gb.LoadState(textboxstate);
                    gb.AdvanceFrames(weedleframe);
                    gb.Inject(Joypad.B);
                    gb.AdvanceFrame(Joypad.B);
                    gb.ClearText(Joypad.B, 1);
                    battlestates[weedleframe] = gb.SaveState();
                }
                for(int i = 0; i < stats.Count; ++i)
                {
                    int hp = stats[i].hp;
                    int weedleframe;
                    for(weedleframe = 0; weedleframe < weedleframes; ++weedleframe)
                    {
                        gb.LoadState(battlestates[weedleframe]);
                        gb.CpuWriteBE("wPartyMon1HP", (ushort) hp);
                        gb.CpuWriteBE("wPartyMon1MaxHP", (ushort) stats[i].maxhp);
                        gb.CpuWriteBE("wPartyMon1Attack", (ushort) stats[i].atk);
                        gb.CpuWriteBE("wPartyMon1Defense", (ushort) stats[i].def);
                        gb.ClearText(Joypad.B);
                        gb.Press(Joypad.A, Joypad.Down, Joypad.A); // t1
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp) break;
                        int lasthp = hp;
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t2
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp - 3 || gb.BattleMon.Poisoned) break;
                        lasthp = gb.BattleMon.HP;
                        gb.Press(Joypad.A, Joypad.Up, Joypad.A); // t3
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp - 6 || gb.BattleMon.HP <= lasthp - 5 || gb.BattleMon.Poisoned || gb.EnemyMon.HP > 21) break;
                        lasthp = gb.BattleMon.HP;
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t4
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp - 6 || gb.BattleMon.HP <= lasthp - 5 || gb.BattleMon.Poisoned || gb.EnemyMon.HP > 15) break;
                        lasthp = gb.BattleMon.HP;
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t5
                        DoTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP < hp - 6 || gb.BattleMon.HP <= lasthp - 5 || gb.BattleMon.Poisoned || gb.EnemyMon.HP > 8) break;
                        lasthp = gb.BattleMon.HP;
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t6
                        DoTurn(gb);
                        if(gb.BattleMon.HP < hp - 6 || gb.BattleMon.HP <= lasthp - 5 || gb.BattleMon.Poisoned || gb.EnemyMon.HP != 0) break;
                    }
                    if(weedleframe == weedleframes)
                    {
                        Path p = new Path(state.Log);
                        Console.WriteLine(link + p);
                        paths[i].Add(p);
                    }
                }
            }
        };

        SingleFrameSearch.StartSearch(gbs, parameters, startTile, 0, states[igtframe], 0);
        Elapsed("search");

        for(int i = 0; i < stats.Count; ++i)
        {
            foreach(Path path in paths[i])
            {
                int score1 = 0;
                int score2 = 0;
                int score15 = 0;
                int score9 = 0;
                MultiThread.For(5, gbs, (gb, it) =>
                {
                    int igtf = igtframe - 2 + it;
                    gb.LoadState(states[igtf].State);
                    gb.CpuWriteBE("wPartyMon1HP", (ushort) stats[i].hp);
                    gb.CpuWriteBE("wPartyMon1MaxHP", (ushort) stats[i].maxhp);
                    gb.CpuWriteBE("wPartyMon1Attack", (ushort) stats[i].atk);
                    gb.CpuWriteBE("wPartyMon1Defense", (ushort) stats[i].def);

                    int adr = gb.Execute(SpacePath(path.P), (gb.Maps[51][25, 12], gb.PickupItem));
                    if(adr == gb.WildEncounterAddress) {
                        return;
                    }
                    gb.Press(Joypad.A);
                    gb.ClearText(1);
                    gb.Inject(Joypad.None);
                    byte[] weedlestate = gb.SaveState();
                    for(int weedleframe = 0; weedleframe < weedleframes; ++weedleframe)
                    {
                        gb.LoadState(weedlestate);
                        string info = igtf + ": " + stats[i].atk + " " + stats[i].def + " " + stats[i].hp + "/" + stats[i].maxhp + " (" + weedleframe + ")";
                        gb.AdvanceFrames(weedleframe);
                        gb.Inject(Joypad.B);
                        gb.AdvanceFrame(Joypad.B);
                        gb.ClearText(Joypad.B);
                        gb.Press(Joypad.A, Joypad.Down, Joypad.A); // t1
                        info += LogTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP == stats[i].hp) Interlocked.Increment(ref score1);
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t2
                        info += LogTurn(gb);
                        gb.ClearText(Joypad.A);
                        if(gb.BattleMon.HP == stats[i].hp) Interlocked.Increment(ref score2);
                        gb.Press(Joypad.A, Joypad.Up, Joypad.A); // t3
                        info += LogTurn(gb);
                        gb.ClearText(Joypad.A);
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t4
                        info += LogTurn(gb);
                        gb.ClearText(Joypad.A);
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t5
                        info += LogTurn(gb);
                        gb.ClearText(Joypad.A);
                        gb.Press(Joypad.A, Joypad.Select, Joypad.A); // t6
                        info += LogTurn(gb);
                        if(gb.EnemyMon.HP == 0 && !gb.BattleMon.Poisoned && gb.BattleMon.HP >= stats[i].hp - 6)
                        {
                            Interlocked.Increment(ref score15);
                            if(igtf >= igtframe - 1 && igtf <= igtframe + 1) Interlocked.Increment(ref score9);
                        }
                        // Trace.WriteLine(info);
                    }
                });
                path.I = "1:" + score1 + " 2:" + score2 + " 6:" + score15 + "-" + score9;
                path.SS = score9 * 100 + score15 * 10 + score1 + score2;
                Console.WriteLine(path);
            }
        }
        Elapsed("check");

        for(int i = 0; i < stats.Count; ++i)
        {
            Trace.WriteLine(stats[i].atk + " " + stats[i].def + " " + stats[i].hp + "/" + stats[i].maxhp + " (" + cost + ")");
            paths[i].RemoveAll(p => p.SS < 770);
            paths[i].PrintAll(link);
        }
    }
}
