using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public enum CharacterClass {
        WARRIOR,
        SORCERER,
        BARBARIAN,
        RANGER,
        ANY
    }
    public CharacterClass characterClass;
    
    public struct CharStats {
        public int HP;
        public int Shield;
        public int SP;
        public bool Stun;
    }
    private CharStats _stats;
    
    [SerializeField] private ClassStats_SO initialStats;
    [SerializeField] public List<Move_SO> moves;
    public Move_SO Pass { get; }
    
    public bool Alive { get; private set; } = true;
    public bool Backup { get; private set; }
    public CharacterClass BackupClass { get; private set; }
    
    //Functions    
    public List<Move_SO> PossibleMoves(int sp = -1) {
        if (sp == -1) { //If -1 is sent as argument, the sp used will be the current. otherwise it'll calculate for a specific sp
            sp = _stats.SP;
        }
        
        List<Move_SO> avail = new List<Move_SO>();
        foreach (var move in moves) {
            if (-move.attackerSP <= sp) {
                avail.Add(move);
            }
        }

        return avail;
    }

    public void Initialize(CharacterClass backupClass) {
        _stats.HP = initialStats.HP;
        _stats.Shield = initialStats.shield;
        _stats.SP = initialStats.SP;
        _stats.Stun = false;
        if (backupClass != CharacterClass.ANY) {
            Backup = true;
            BackupClass = backupClass;
        }else
            Backup = false;
    }
    
    public void UpdateStats(int hp, int shield, int sp, bool stun) {
        _stats.HP += hp;
        _stats.Shield += shield;
        _stats.SP += sp;
        _stats.Stun = stun;
        if (_stats.HP <= 0) {
            Alive = false;
        }
    }

    public string StatsText() {
        var result = $"HP: {_stats.HP}/{initialStats.maxHP}\nShield: {_stats.Shield}/{initialStats.maxShield}\n"
                     + $"SP: {_stats.SP}/{initialStats.maxSP}";
        if (_stats.Stun) {
            result += $"\nstun";
        }

        return result;
    }

    public CharStats Stats() {
        return _stats;
    }
}