using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Brain : MonoBehaviour
{
    private void CreateNodes() {
        
    }
    
}

public class GameState {
    public int idTurnPlayer;

    public int p1_HP;
    public int p1_Shield;
    public int p1_SP;
    public bool p1_Stun;

    public int p2_HP;
    public int p2_Shield;
    public int p2_SP;
    public bool p2_Stun;

    public GameState(GameState prev, Move_SO move) {
        if (prev.idTurnPlayer == 1) {
            idTurnPlayer = 2;
            p1_HP = prev.p1_HP + move.attackerHP;
            p1_Shield = prev.p1_Shield + move.attackerShield;
            p1_SP = prev.p1_SP + move.attackerSP;
            p1_Stun = move.attackerStun;
            
            p2_HP = prev.p2_HP + move.targetHP;
            p2_Shield = prev.p2_Shield + move.targetShield;
            p2_SP = prev.p2_SP + move.targetSP;
            p2_Stun = move.targetStun;
        }
        else {
            idTurnPlayer = 1;
            p1_HP = prev.p1_HP + move.targetHP;
            p1_Shield = prev.p1_Shield + move.targetShield;
            p1_SP = prev.p1_SP + move.targetSP;
            p1_Stun = move.targetStun;
            
            p2_HP = prev.p2_HP + move.attackerHP;
            p2_Shield = prev.p2_Shield + move.attackerShield;
            p2_SP = prev.p2_SP + move.attackerSP;
            p2_Stun = move.attackerStun;
        }
    }
    public GameState() {
        
    }
}