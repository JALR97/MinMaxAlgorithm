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
        Character.CharStats attackerStatBlock, targetStatBlock;
        ClassStats_SO attackStats, targetStats;
        if (prev.idTurnPlayer == 2) {
            idTurnPlayer = 1;

            attackerStatBlock = prev.c2Stats;
            targetStatBlock = prev.c1Stats;
            attackStats = GameManager.Instance.char2.initialStats;
            targetStats = GameManager.Instance.char1.initialStats;
        }
        else {
            idTurnPlayer = 2;
            
            attackerStatBlock = prev.c1Stats;
            targetStatBlock = prev.c2Stats;
            attackStats = GameManager.Instance.char1.initialStats;
            targetStats = GameManager.Instance.char2.initialStats;
        }

        attackerStatBlock.HP = Mathf.Clamp(attackerStatBlock.HP + move.attackerHP, 0, attackStats.maxHP);
        attackerStatBlock.Shield = Mathf.Clamp(attackerStatBlock.Shield + move.attackerShield, 0, attackStats.maxShield);
        attackerStatBlock.SP = Mathf.Clamp(attackerStatBlock.SP + move.attackerSP, 0, attackStats.maxSP);
        attackerStatBlock.Stun = move.attackerStun;

        int startingHP = targetStatBlock.HP;
        if (startingHP <= 0) {
            targetStatBlock.HP = 0;
            targetStatBlock.Shield = 0;
            targetStatBlock.SP = 0;
            targetStatBlock.Stun = false;
        }
        else {
            targetStatBlock.Shield = Mathf.Clamp(targetStatBlock.Shield + move.targetShield, 0, targetStats.maxShield);
            int totalHitPool = targetStatBlock.HP + targetStatBlock.Shield;
            totalHitPool += move.targetHP;
            targetStatBlock.HP = Mathf.Clamp(totalHitPool, 0, startingHP);
            targetStatBlock.Shield = Mathf.Clamp(totalHitPool - startingHP, 0, targetStats.maxShield);
        
            targetStatBlock.SP = Mathf.Clamp(targetStatBlock.SP + move.targetSP, 0, targetStats.maxSP);
            targetStatBlock.Stun = move.targetStun;
        }

        if (idTurnPlayer == 1) {
            c2Stats = attackerStatBlock;
            c1Stats = targetStatBlock;
        }
        else {
            c1Stats = attackerStatBlock;
            c2Stats = targetStatBlock;
        }
    }
}