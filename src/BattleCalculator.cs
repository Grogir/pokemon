using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;

public class BattleCalculator : Red
{
    public const bool Player = false;
    public const bool Enemy = true;
    public const int OnlyCrits = 1;
    public const int OnlyNonCrits = 2;

    double Win;
    double Lose;
    double Inderterminate;

    bool AvgOnly = false;
    double LockThreshold = 0.0001;
    bool Print = true;

    Func<Info, string> DeterminePlayerMove;
    Func<Info, bool?> FightDone;

    class Turn
    {
        public bool Attacker;
        public string Move;
        public List<(double, string)> Variations;
        public int Flags;
        public Turn(bool attacker, string move, int flags = 0)
        {
            Attacker = attacker; Move = move; Flags = flags;
            Variations = move == null ? new List<(double, string)>() : null;
        }
        public bool Defender { get { return !Attacker; } }
    }
    class Info
    {
        public RbyPokemon Player;
        public RbyPokemon Enemy;
        public int Turn;
        public List<Turn> Turns;
        public int CurrentEnemy;
        public Info(RbyPokemon player, RbyPokemon enemy, int turn, List<Turn> turns = null, int currentenemy = 0)
        {
            Player = player;
            Enemy = enemy;
            Turn = turn;
            Turns = turns != null ? turns : new List<Turn>();
            CurrentEnemy = currentenemy;
        }
        public Info Copy()
        {
            return new Info(Player.Clone(), Enemy.Clone(), Turn, new List<Turn>(Turns), CurrentEnemy);
        }
        public RbyPokemon this[bool m] { get { return m == BattleCalculator.Player ? Player : Enemy; } }
        public RbyPokemon Attacker { get { return this[Turns[Turn].Attacker]; } }
        public RbyPokemon Defender { get { return this[Turns[Turn].Defender]; } }
        public string Move { get { return Turns[Turn].Move; } }
        public int Flags { get { return Turns[Turn].Flags; } }
    }

    void PrintInfo(double p, Info info, bool miss = false, bool crit = false, int roll = 0, int count = 39)
    {
        string str = ""
            + p.ToString("F3") + "\t\t"
            + new string('\t', info.Turn)
            + info.Move
            + (crit ? " Crit" : "")
            + (miss ? " Miss" : "")
            + (roll > 0 ? ": " + roll : "")
            + (count != 39 ? "x" + count + "" : "")
            // + " (" + info.Player.HP.ToString() + "-" + info.Enemy.HP.ToString() + ")"
            ;
        if(Print) Trace.WriteLine(str);
    }

    static double Crit(RbyPokemon poke, string move)
    {
        int crit = poke.Species.BaseSpeed >> 1;
        if(move == "CRABHAMMER" || move == "KARATE CHOP" || move == "RAZOR LEAF" || move == "SLASH")
            crit *= 8;
        if(poke.FocusEnergyEffect)
            crit /= 4;
        return Math.Min(crit, 255) / 256.0;
    }
    static void SetStats(RbyPokemon poke, ushort hp, ushort atk, ushort def, ushort spd, ushort spc)
    {
        poke.UnmodifiedMaxHP = poke.MaxHP = poke.HP = hp;
        poke.UnmodifiedAttack = poke.Attack = atk;
        poke.UnmodifiedDefense = poke.Defense = def;
        poke.UnmodifiedSpeed = poke.Speed = spd;
        poke.UnmodifiedSpecial = poke.Special = spc;
    }
    static void AttackStage(RbyPokemon poke, int stage, Info info)
    {
        poke.Attack = poke.UnmodifiedAttack;
        if(poke == info.Player) ApplyBadgeBoosts(poke);
        poke.AttackModifider = (byte) Math.Clamp(poke.AttackModifider + stage, 1, 13);
        poke.Attack = GetModifiedStat(poke.Attack, poke.AttackModifider);
    }
    static void DefenseStage(RbyPokemon poke, int stage, Info info)
    {
        poke.Defense = poke.UnmodifiedDefense;
        if(poke == info.Player) ApplyBadgeBoosts(poke);
        poke.DefenseModifider = (byte) Math.Clamp(poke.DefenseModifider + stage, 1, 13);
        poke.Defense = GetModifiedStat(poke.Defense, poke.DefenseModifider);
    }
    static void SpeedStage(RbyPokemon poke, int stage, Info info)
    {
        poke.Speed = poke.UnmodifiedSpeed;
        if(poke == info.Player) ApplyBadgeBoosts(poke);
        poke.SpeedModifider = (byte) Math.Clamp(poke.SpeedModifider + stage, 1, 13);
        poke.Speed = GetModifiedStat(poke.Speed, poke.SpeedModifider);
    }
    static void SpecialStage(RbyPokemon poke, int stage, Info info)
    {
        poke.Special = poke.UnmodifiedSpecial;
        if(poke == info.Player) ApplyBadgeBoosts(poke);
        poke.SpecialModifider = (byte) Math.Clamp(poke.SpecialModifider + stage, 1, 13);
        poke.Special = GetModifiedStat(poke.Special, poke.SpecialModifider);
    }
    static void AccuracyStage(RbyPokemon poke, int stage, Info info)
    {
        if(poke == info.Player) ApplyBadgeBoosts(poke);
        poke.AccuracyModifider = (byte) Math.Clamp(poke.AccuracyModifider + stage, 1, 13);
    }
    static void EvasionStage(RbyPokemon poke, int stage, Info info)
    {
        if(poke == info.Player) ApplyBadgeBoosts(poke);
        poke.EvasionModifider = (byte) Math.Clamp(poke.EvasionModifider + stage, 1, 13);
    }
    static void ApplyBadgeBoosts(RbyPokemon poke)
    {
        // do we have badges...
        poke.Attack = (ushort) (poke.Attack * 9 / 8);
        poke.Defense = (ushort) (poke.Defense * 9 / 8);
        poke.Speed = (ushort) (poke.Speed * 9 / 8);
        poke.Special = (ushort) (poke.Special * 9 / 8);
    }
    static ushort GetModifiedStat(int stat, int stage)
    {
        switch(stage)
        {
            case 1: stat *= 25; break;
            case 2: stat *= 28; break;
            case 3: stat *= 33; break;
            case 4: stat *= 40; break;
            case 5: stat *= 50; break;
            case 6: stat *= 66; break;
            case 7: stat *= 100; break;
            case 8: stat *= 150; break;
            case 9: stat *= 200; break;
            case 10: stat *= 250; break;
            case 11: stat *= 300; break;
            case 12: stat *= 350; break;
            case 13: stat *= 400; break;
        }
        return (ushort) (stat / 100);
    }
    static void Damage(RbyPokemon poke, int hp)
    {
        poke.HP = (ushort) Math.Clamp(poke.HP - hp, 0, poke.MaxHP);
    }
    static void Heal(RbyPokemon poke, int hp)
    {
        Damage(poke, -hp);
    }
    bool? Fastest(RbyPokemon player, RbyPokemon enemy, string playerMove, string enemyMove)
    {
        if(Items[playerMove] != null)
            return Player;
        if(playerMove == "QUICK ATTACK" && enemyMove != "QUICK ATTACK")
            return Player;
        if(enemyMove == "QUICK ATTACK" && playerMove != "QUICK ATTACK")
            return Enemy;
        if(player.Speed > enemy.Speed)
            return Player;
        if(enemy.Speed > player.Speed)
            return Enemy;
        return null;
    }
    bool MoveCantMiss(string move)
    {
        RbyEffect effect = Moves[move].Effect;
        return effect == RbyEffect.Swift || effect == RbyEffect.Bide
            || effect == RbyEffect.Substitute || effect == RbyEffect.Splash
            || effect == RbyEffect.LightScreen || effect == RbyEffect.Reflect
            || effect == RbyEffect.Heal || effect == RbyEffect.FocusEnergy
            || effect == RbyEffect.SwitchAndTeleport || effect == RbyEffect.MirrorMove
            || (effect >= RbyEffect.AttackUp1 && effect <= RbyEffect.EvasionUp1)
            || (effect >= RbyEffect.AttackUp2 && effect <= RbyEffect.EvasionUp2);
    }

    void DoRolls(double p, Info current, bool crit, int multiply = 1)
    {
        RbyMove move = Moves[current.Move];
        var rolls = CalcDamage(current.Attacker, current.Defender, move, crit);
        for(int roll = rolls[AvgOnly ? 19 : 0]; roll <= rolls[AvgOnly ? 19 : 38]; ++roll)
        {
            int count = AvgOnly ? 39 : rolls.Count(x => x == roll);
            Info next = current.Copy();
            Damage(next.Defender, roll * multiply);
            if(move.Effect == RbyEffect.DrainHp) Heal(next.Attacker, roll / 2);
            PrintInfo(p * count / 39, next, false, crit, roll * multiply, count);
            PostMoveEffects(p * count / 39, next);
            // StartTurn(p * count / 39, next);
        }
    }

    void StartTurn(double p, Info info)
    {
        var won = FightDone(info);
        if(won != null || info.Turn >= info.Turns.Count)
        {
            if(won == true)
                Win += p;
            else
                Lose += p;
            return;
        }

        Turn t = info.Turns[info.Turn];

        if(t.Flags == 0)
        {
            double pcrit = p * Crit(info.Attacker, t.Move);
            DoRolls(p - pcrit, info, false);
            DoRolls(pcrit, info, true);
        }
        else
        {
            DoRolls(p, info, t.Flags == OnlyCrits);
        }
    }

    void Start(RbyPokemon player, RbyPokemon enemy, List<Turn> turns)
    {
        Win = 0;
        Lose = 0;
        StartTurn(1, new Info(player, enemy, 0));
        Trace.WriteLine($"total win:  {Win:F7} ({Win:P1})");
        Trace.WriteLine($"total lose: {Lose:F7} ({Lose:P1})");
    }

    void CriticalCheck(double p, Info info, int multiply = 1)
    {
        double pcrit = p * Crit(info.Attacker, info.Move);
        DoRolls(p - pcrit, info, false, multiply);
        DoRolls(pcrit, info, true, multiply);
    }

    void PreMoveEffects(double p, Info info)
    {
        string move = info.Move;
        RbyEffect effect;
        if(Items[move] != null)
        {
            Info next = info.Copy();
            if(move == "X ACCURACY")
                next.Attacker.XAccuracyEffect = true;
            else if(move == "X ATTACK")
                AttackStage(next.Attacker, 1, next);
            else if(move == "X DEFEND")
                DefenseStage(next.Attacker, 1, next);
            else if(move == "X SPEED")
                SpeedStage(next.Attacker, 1, next);
            else if(move == "X SPECIAL")
                SpecialStage(next.Attacker, 1, next);
            else if(move == "POTION")
                Heal(next.Attacker, 20);
            else if(move == "SUPER POTION")
                Heal(next.Attacker, 50);
            else if(move == "HYPER POTION")
                Heal(next.Attacker, 200);
            else if(move == "MAX POTION")
                Heal(next.Attacker, 999);
            else if(move == "FULL RESTORE")
                Heal(next.Attacker, 999);
            else
                Debug.Warning("Unimplemented item " + move);
            PrintInfo(p, next);
            EndTurn(p, next);
        }
        else if((effect = Moves[move].Effect) != RbyEffect.NoAdditional)
        {
            Info next = info.Copy();
            if(effect == RbyEffect.Ohko)
            {
                if(next.Attacker.Speed >= next.Defender.Speed)
                    next.Defender.HP = 0;
            }
            else if(effect == RbyEffect.AttackDown1)
                AttackStage(next.Defender, -1, next);
            else if(effect == RbyEffect.DefenseDown1)
                DefenseStage(next.Defender, -1, next);
            else if(effect == RbyEffect.SpeedDown1)
                SpeedStage(next.Defender, -1, next);
            else if(effect == RbyEffect.SpecialDown1)
                SpecialStage(next.Defender, -1, next);
            else if(effect == RbyEffect.AccuracyDown1)
                AccuracyStage(next.Defender, -1, next);
            else if(effect == RbyEffect.FocusEnergy)
                next.Attacker.FocusEnergyEffect = true;
            else if(effect == RbyEffect.Charge)
            {
                if(!next.Attacker.ChargingUp)
                    next.Attacker.ChargingUp = true;
                else
                {
                    next.Attacker.ChargingUp = false;
                    CriticalCheck(p, next);
                    return;
                }
            }
            else if(effect == RbyEffect.MirrorMove)
            {
                string last = LastMoveUsed(info, info.Turns[info.Turn].Defender);
                if(last != null)
                {
                    next.Turns[info.Turn].Move = last;
                    AccuracyCheck(p, next);
                    return;
                }
            }
            else if(effect == RbyEffect.TwoToFiveAttacks)
            {
                CriticalCheck(p * 0.375, info, 2);
                CriticalCheck(p * 0.375, info, 3);
                CriticalCheck(p * 0.125, info, 4);
                CriticalCheck(p * 0.125, info, 5);
                return;
            }
            else if(effect != RbyEffect.SwitchAndTeleport)
            {
                if(Print) Debug.Warning("Unimplemented effect " + move);
                CriticalCheck(p, info);
                return;
            }
            PrintInfo(p, next);
            EndTurn(p, next);
        }
        else
        {
            CriticalCheck(p, info);
        }
    }

    void PostMoveEffects(double p, Info info)
    {
        // poison, speedfall, etc
        EndTurn(p, info);
    }

    void EndTurn(double p, Info info)
    {
        info.Turn++;
        // info.turns.RemoveAt(0);
        StartTurn2(p, info);
    }

    void AccuracyCheck(double p, Info info)
    {
        double phit;
        if(info.Attacker.XAccuracyEffect || Items[info.Move] != null || MoveCantMiss(info.Move)
            || (Moves[info.Move].Effect == RbyEffect.Charge && !info.Attacker.ChargingUp))
            phit = p;
        else
            phit = p * Moves[info.Move].Accuracy / 256; // 25% ai miss, sand
        double pmiss = p - phit;
        if(phit > 0)
            PreMoveEffects(phit, info);
        if(pmiss > 0)
        {
            info.Attacker.ChargingUp = false;
            PrintInfo(pmiss, info, true);
            EndTurn(pmiss, info.Copy());
        }
    }

    (double, string)? TrainerAI(string trainer)
    {
        if(trainer == "BLACKBELT")
            return (0.125, "X ATTACK");
        return null;
    }

    void SelectMoves1(double p, Info info)
    {
        string playerMove = DeterminePlayerMove(info);
        string enemyMove;

        var ai = TrainerAI(EnemyTrainerClass.Name);
        if(ai != null)
        {
            double pai = p * ai.Value.Item1;
            enemyMove = ai.Value.Item2;
            Info next = info.Copy();
            next.Turns.Add(new Turn(Player, playerMove));
            next.Turns.Add(new Turn(Enemy, enemyMove));
            PreMoveEffects(pai, next);
            p -= pai;
        }
        bool[] possiblemoves = { true, true, true, true };
        if(info.Enemy.ChargingUp)
        {
            possiblemoves = new bool[] { false, false, true, false };
        }
        for(int i = info.Enemy.NumMoves; i < 4; ++i)
            possiblemoves[i] = false;
        p /= possiblemoves.Count(x => x == true);
        for(int m = 0; m < info.Enemy.NumMoves; ++m)
        {
            if(!possiblemoves[m])
                continue;
            enemyMove = info.Enemy.Moves[m].Name;
            var fastest = Fastest(info.Player, info.Enemy, playerMove, enemyMove);
            double speedtie = fastest == null ? 0.5 : 1;
            if(fastest != Enemy)
            {
                Info next = info.Copy();
                next.Turns.Add(new Turn(Player, playerMove));
                next.Turns.Add(new Turn(Enemy, enemyMove));
                AccuracyCheck(p * speedtie, next);
            }
            if(fastest != Player)
            {
                Info next = info.Copy();
                next.Turns.Add(new Turn(Enemy, enemyMove));
                next.Turns.Add(new Turn(Player, playerMove));
                AccuracyCheck(p * speedtie, next);
            }
        }
    }

    string LastMoveUsed(Info info, bool user)
    {
        for(int i = 1; i <= info.Turn; ++i)
        {
            if(info.Turns[info.Turn - i].Attacker == user && Moves[info.Turns[info.Turn - i].Move] != null)
                return info.Turns[info.Turn - i].Move;
        }
        return null;
    }

    void SelectMoves(double p, Info info)
    {
        string playerMove = DeterminePlayerMove(info);
        string enemyMove;

        var ai = TrainerAI(EnemyTrainerClass.Name);
        int[] possiblemoves = { 63, 64, 63, 66 };
        if(info.Enemy.ChargingUp)
        {
            string move = LastMoveUsed(info, Enemy);
            for(int m = 0; m < info.Enemy.NumMoves; ++m)
                if(info.Enemy.Moves[m].Name != move)
                    possiblemoves[m] = 0;
        }
        for(int i = info.Enemy.NumMoves; i < 4; ++i)
            possiblemoves[i] = 0;
        int sumpossibles = possiblemoves.Sum();
        bool?[] fastest = new bool?[4];
        int sumplayerfirst = 0;
        int sumenemyfirst = 0;
        int sumspeedtie = 0;
        for(int m = 0; m < info.Enemy.NumMoves; ++m)
        {
            if(possiblemoves[m] != 0)
            {
                enemyMove = info.Enemy.Moves[m].Name;
                fastest[m] = Fastest(info.Player, info.Enemy, playerMove, enemyMove);
                if(fastest[m] == Player)
                    sumplayerfirst += possiblemoves[m];
                else if(fastest[m] == Enemy)
                    sumenemyfirst += possiblemoves[m];
                else
                    sumspeedtie += possiblemoves[m];
            }
        }
        Turn playerfirst = new Turn(Enemy, null);
        double pnonai = 1;
        if(ai != null)
        {
            pnonai -= ai.Value.Item1;
            if(sumenemyfirst > 0 || sumspeedtie > 0)
            {
                Info next = info.Copy();
                next.Turns.Add(new Turn(Enemy, ai.Value.Item2));
                next.Turns.Add(new Turn(Player, playerMove));
                double pai = p * ai.Value.Item1 * (sumenemyfirst + sumspeedtie * 0.5) / sumpossibles;
                PreMoveEffects(pai, next);
            }
            if(sumplayerfirst > 0 || sumspeedtie > 0)
            {
                playerfirst.Variations.Add(ai.Value);
            }
        }
        for(int m = 0; m < info.Enemy.NumMoves; ++m)
        {
            if(possiblemoves[m] != 0)
            {
                enemyMove = info.Enemy.Moves[m].Name;
                double speedtie = fastest[m] == null ? 0.5 : 1;
                if(fastest[m] != Enemy)
                {
                    playerfirst.Variations.Add((pnonai * speedtie * possiblemoves[m] / (sumplayerfirst + sumspeedtie * 0.5), enemyMove));
                }
                if(fastest[m] != Player)
                {
                    Info next = info.Copy();
                    next.Turns.Add(new Turn(Enemy, enemyMove));
                    next.Turns.Add(new Turn(Player, playerMove));
                    AccuracyCheck(p * pnonai * speedtie * possiblemoves[m] / sumpossibles, next);
                }
            }
        }
        if(playerfirst.Variations.Count > 0)
        {
            Info next = info.Copy();
            next.Turns.Add(new Turn(Player, playerMove));
            next.Turns.Add(playerfirst);
            AccuracyCheck(p * (sumplayerfirst + sumspeedtie * 0.5) / sumpossibles, next);
        }
    }

    void StartTurn2(double p, Info info)
    {
        var won = FightDone(info);
        if(won != null || p < LockThreshold)
        {
            if(won == true)
                Win += p;
            else if(won == false)
                Lose += p;
            else
                Inderterminate += p;
            return;
        }

        if(info.Enemy.HP == 0)
        {
            info.Turns.RemoveRange(info.Turn, info.Turns.Count - info.Turn);
            EnemySendNextPokemon(ref info);
        }

        if(info.Turns.Count > info.Turn)
        {
            if(info.Turns[info.Turn].Variations != null)
            {
                foreach((double pvar, string move) in info.Turns[info.Turn].Variations)
                {
                    Info next = info.Copy();
                    next.Turns[next.Turn].Move = move;
                    AccuracyCheck(p * pvar, next);
                }
            }
            else
            {
                AccuracyCheck(p, info);
            }
        }
        else
        {
            SelectMoves(p, info);
        }
    }

    void Start2(RbyPokemon player, RbyPokemon enemy)
    {
        Win = 0;
        Lose = 0;
        Inderterminate = 0;
        StartTurn2(1, new Info(player, enemy, 0));
        Trace.WriteLine($"total win:  {Win:F7} ({Win:P1})");
        Trace.WriteLine($"total lose: {Lose:F7} ({Lose:P1})");
        Trace.WriteLine($"total ind:  {Inderterminate:F7} ({Inderterminate:P1})");
    }

    void EnemySendNextPokemon(ref Info info)
    {
        // levelup...
        info.CurrentEnemy++;
        info.Enemy = EnemyParty[info.CurrentEnemy];
        info.Enemy.CalculateUnmodifiedStats();
        info.Enemy.AttackModifider = 7;
        info.Enemy.DefenseModifider = 7;
        info.Enemy.SpeedModifider = 7;
        info.Enemy.SpecialModifider = 7;
        info.Enemy.AccuracyModifider = 7;
        info.Enemy.EvasionModifider = 7;
    }

    bool? DefaultFightDone(Info info)
    {
        if(info.Player.HP == 0)
            return false;
        if(info.Enemy.HP == 0 && (info.CurrentEnemy == EnemyParty.Length - 1 || info.Player.XAccuracyEffect))
            return true;
        return null;
    }

    public void Zubat()
    {
        RbyPokemon nido = new RbyPokemon(Species["NIDORANM"], 14, 0xffef);
        SetStats(nido, 41, 28, 21, 23, 20);
        RbyPokemon zubat = new RbyPokemon(Species["ZUBAT"], 13);
        zubat.DefenseModifider--;
        zubat.Defense = 10;

        var turns = new List<Turn> {
            new Turn(Player, "HORN ATTACK"),
            new Turn(Enemy, "LEECH LIFE"),
            new Turn(Player, "TACKLE"),
        };
        // AvgOnly = true;
        Start(nido, zubat, turns);
    }

    public void Pidgeotto()
    {
        LoadState("basesaves/red/bridgerival.gqs");
        var turns = new List<Turn> {
            new Turn(Player, "HORN ATTACK", OnlyCrits),
            new Turn(Player, "MEGA PUNCH"),
        };
        // AvgOnly = true;
        Start(BattleMon, EnemyMon, turns);
    }

    public BattleCalculator() : base()
    {
        FightDone = DefaultFightDone;
    }

    public void Bridge3()
    {
        LoadState("basesaves/red/bridge3.gqs");
        ClearText();
        AvgOnly = true;
        LockThreshold = 0.001;
        DeterminePlayerMove = info =>
        {
            return "MEGA PUNCH";
        };
        Start2(BattleMon, EnemyMon);
    }

    public void Blackbelt()
    {
        LoadState("basesaves/red/blackbelt.gqs");
        CpuWriteBE<ushort>("wPartyMon2HP", 34);
        ClearText();
        AvgOnly = true;
        LockThreshold = 0.001;
        DeterminePlayerMove = info =>
        {
            if(!info.Player.XAccuracyEffect)
                return "X ACCURACY";
            return "HORN DRILL";
        };
        Start2(BattleMon, EnemyMon);
    }

    public void Champ()
    {
        LoadState("basesaves/red/champ.gqs");
        ClearText();
        AvgOnly = true;
        LockThreshold = 0.001;
        DeterminePlayerMove = info =>
        {
            if(info.Turn == 0)
                return "X SPECIAL";
            if(!info.Player.XAccuracyEffect)
                if(info.Enemy.Species.Name == "PIDGEOT" && LastMoveUsed(info, Enemy) == "SKY ATTACK")
                    return "BLIZZARD";
                else if(info.Enemy.Species.Name == "ALAKAZAM")
                    return "EARTHQUAKE";
                else
                    return "X ACCURACY";
            return "HORN DRILL";
        };
        DeterminePlayerMove = info =>
        {
            if(info.Turn == 0)
                return "X SPECIAL";
            if(!info.Player.XAccuracyEffect)
                if(info.Enemy.Species.Name == "PIDGEOT" && LastMoveUsed(info, Enemy) == "SKY ATTACK")
                    return "BLIZZARD";
                else if(info.Enemy.Species.Name == "PIDGEOT" && info.Player.HP <= 14 && info.Turn <= 2)
                    return "POTION";
                else if(info.Enemy.Species.Name == "ALAKAZAM")
                    return "EARTHQUAKE";
                else
                    return "X ACCURACY";
            return "HORN DRILL";
        };
        Start2(BattleMon, EnemyMon);
    }

    public void ClassicBlackbelt()
    {
        LoadState("basesaves/red/blackbeltclassic.gqs");
        ClearText();
        LockThreshold = 0.000000001;
        Print = false;
        DeterminePlayerMove = info =>
        {
            if(info.Enemy.HP < 50)
                return "THUNDERBOLT";
            return "EARTHQUAKE";
        };
        for(int atk = 104; atk <= 121; ++atk)
        {
            Trace.WriteLine(atk + " Atk");
            var nido = BattleMon;
            nido.Attack = (ushort)(atk * 9 / 8);
            var w = Stopwatch.StartNew();
            Start2(nido, EnemyMon);
            Console.WriteLine(w.Elapsed.TotalSeconds + "s");
            Trace.WriteLine("");
        }
    }
}
