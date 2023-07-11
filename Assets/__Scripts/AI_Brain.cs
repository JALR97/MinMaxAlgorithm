using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Brain : MonoBehaviour {
    private Node root;

    private void CreateTree(int depth) {
        root = new Node(GameManager.Instance.char1.Stats(), GameManager.Instance.char2.Stats());
        root.CreateNodes(2);
        root.EvaluateNode();
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            CreateTree(4);
            
            Debug.Log(root.BestOption().valuation);
        }
    }
}

public class GameState {
    public int idTurnPlayer;

    public Character.CharStats c1Stats;
    public Character.CharStats c2Stats;

    public GameState(Character.CharStats character1, Character.CharStats character2) {
        idTurnPlayer = 1;
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
        int SPWeight = 1;
        total += SPWeight * (c1Stats.SP - c2Stats.SP);

        return total;
    }
    
    public GameState(GameState prev, Move_SO move) {
        if (prev.idTurnPlayer == 2) {
            Character.CharStats temp;
            temp = c1Stats;
            c1Stats = c2Stats;
            c2Stats = temp;
            idTurnPlayer = 1;
        }
        else 
            idTurnPlayer = 2;
        
        c1Stats.HP = prev.c1Stats.HP + move.attackerHP;
        c1Stats.Shield = prev.c1Stats.Shield + move.attackerShield;
        c1Stats.SP = prev.c1Stats.SP + move.attackerSP;
        c1Stats.Stun = move.attackerStun;
        
        c2Stats.HP = prev.c2Stats.HP + move.targetHP;
        c2Stats.Shield = prev.c2Stats.Shield + move.targetShield;
        c2Stats.SP = prev.c2Stats.SP + move.targetSP;
        c2Stats.Stun = move.targetStun;
        
        if (prev.idTurnPlayer == 1) {
            Character.CharStats temp;
            temp = c1Stats;
            c1Stats = c2Stats;
            c2Stats = temp;
        }
    }
}