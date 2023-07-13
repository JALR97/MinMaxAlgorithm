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
    
    [SerializeField] public ClassStats_SO initialStats;
    [SerializeField] public List<Move_SO> moves;
    public Move_SO Pass { get; }
    
    public bool Alive { get; private set; } = true;
    public bool Backup { get; private set; }
    public CharacterClass BackupClass { get; private set; }
    
    //Functions    
    public List<Move_SO> PossibleMoves(int sp) {
        List<Move_SO> avail = new List<Move_SO>();
        
        if (_stats.HP <= 0) {
            return avail;
        }
        foreach (var move in moves) {
            if (sp >= Math.Abs(move.attackerSP)) {
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
        if (hp < 0) { //if it's a damaging attack
            int startingHP = _stats.HP;
            
            _stats.Shield = Mathf.Clamp(_stats.Shield + shield, 0, initialStats.maxShield);
            int totalHitPool = _stats.HP + _stats.Shield;
            totalHitPool += hp;
            _stats.HP = Mathf.Clamp(totalHitPool, 0, startingHP);
            _stats.Shield = Mathf.Clamp(totalHitPool - startingHP, 0, initialStats.maxShield);
        }
        else {
            _stats.HP = Mathf.Clamp(_stats.HP + hp, 0, initialStats.maxHP);
            _stats.Shield = Mathf.Clamp(_stats.Shield + shield, 0, initialStats.maxShield);
        }
        _stats.SP = Mathf.Clamp(_stats.SP + sp,0, initialStats.maxSP);
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

    public override string ToString() {
        switch (characterClass) {
            case CharacterClass.WARRIOR:
                return "WARRIOR";
            case CharacterClass.SORCERER:
                return "SORCERER";
            case CharacterClass.BARBARIAN:
                return "BARBARIAN";
            case CharacterClass.RANGER:
                return "RANGER";
            case CharacterClass.ANY:
                return "N/A";
        }
        return "";
    }
}