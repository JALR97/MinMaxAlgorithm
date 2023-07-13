using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class AI_Brain : MonoBehaviour {
    public Node root;
    
    private void CreateTree(int depth) {
        
    }

    public Move_SO MinMaxOption(int depth) {
        float timer = Time.time;
        root = new Node(GameManager.Instance.char1.Stats(), GameManager.Instance.char2.Stats());
        root.CreateNodes(depth);
        timer = Time.time - timer;

        Node.DEBUG_callstoEval = 0;
        Debug.Log($"Time creating nodes: {timer}");
        root.EvaluateNode();
        
        Node option = root.BestOption();
        return option.MoveMade;
    }

    public void AlphaBetaOption(int depth) {
        throw new NotImplementedException();
    }
}

public class GameState {
    public int idTurnPlayer;

    public Character.CharStats c1Stats;
    public Character.CharStats c2Stats;

    public GameState(Character.CharStats character1, Character.CharStats character2) {
        idTurnPlayer = 2;
        c1Stats = character1;
        c2Stats = character2;
    }

    public int EvaluateState() {
        int total = 0;
        if (c1Stats.HP <= 0) {
            total -= 100;
        }else if (c2Stats.HP <= 0) {
            total += 100;
        }

        total += (c1Stats.HP + c1Stats.Shield) - (c2Stats.HP + c2Stats.Shield);
        
        //could need change
        int SPWeight = 0;
        total += SPWeight * (c1Stats.SP - c2Stats.SP);

        return total;
    }
    
    public GameState(GameState prev, Move_SO move) {
        ClassStats_SO attackStats, targetStats;
        if (prev.idTurnPlayer == 2) {
            idTurnPlayer = 1;

            attackStats = GameManager.Instance.char2.initialStats;
            targetStats = GameManager.Instance.char1.initialStats;
        }
        else {
            idTurnPlayer = 2;
            
            attackStats = GameManager.Instance.char1.initialStats;
            targetStats = GameManager.Instance.char2.initialStats;
        }

        c1Stats.HP = Mathf.Clamp(prev.c1Stats.HP + move.attackerHP, 0, attackStats.maxHP);
        c1Stats.Shield = Mathf.Clamp(prev.c1Stats.Shield + move.attackerShield, 0, attackStats.maxShield);
        c1Stats.SP = Mathf.Clamp(prev.c1Stats.SP + move.attackerSP, 0, attackStats.maxSP);
        c1Stats.Stun = move.attackerStun;

        int startingHP = prev.c2Stats.HP;
        if (startingHP <= 0) {
            c2Stats.HP = 0;
            c2Stats.Shield = 0;
            c2Stats.SP = 0;
            c2Stats.Stun = false;
        }
        else {
            c2Stats.Shield = Mathf.Clamp(prev.c2Stats.Shield + move.targetShield, 0, targetStats.maxShield);
            int totalHitPool = prev.c2Stats.HP + c2Stats.Shield;
            totalHitPool += move.targetHP;
            c2Stats.HP = Mathf.Clamp(totalHitPool, 0, startingHP);
            c2Stats.Shield = Mathf.Clamp(totalHitPool - startingHP, 0, targetStats.maxShield);
        
            c2Stats.SP = Mathf.Clamp(prev.c2Stats.SP + move.targetSP, 0, targetStats.maxSP);
            c2Stats.Stun = move.targetStun;
        }

        if (idTurnPlayer == 1) {
            Character.CharStats temp;
            temp = c1Stats;
            c1Stats = c2Stats;
            c2Stats = temp;
        }
    }
}