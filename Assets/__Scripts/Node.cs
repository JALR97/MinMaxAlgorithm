using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Node {
    public Move_SO MoveMade;
    private GameState nodeState;
    
    public float valuation;
    private int alpha, beta;
    private bool isMaxNode;

    private Node parent;
    private List<Node> childs;

    public static int DEBUG_callstoEval = 0;
    
    public Node(Move_SO moveMade, Node parent) {
        MoveMade = moveMade;
        this.parent = parent;
        isMaxNode = !parent.isMaxNode;
        nodeState = new GameState(parent.nodeState, moveMade);
    }

    public Node(Character.CharStats character1, Character.CharStats character2) {
        nodeState = new GameState(character1, character2);
        isMaxNode = false;
    }

    public Node BestOption() {
        int nodeIndex = 0;
        
        if (isMaxNode) {
            for (int i = 0; i < childs.Count; i++) {
                nodeIndex = childs[nodeIndex].valuation > childs[i].valuation ? nodeIndex : i;
            }
        }
        else {
            for (int i = 0; i < childs.Count; i++) {
                nodeIndex = childs[nodeIndex].valuation < childs[i].valuation ? nodeIndex : i;
            }
        }

        return childs[nodeIndex];
    }

    public void EvaluateNode() {
        DEBUG_callstoEval += 1;
        if (childs == null || childs.Count == 0) {
            valuation = nodeState.EvaluateState();
            AI_Brain.evaluatedNodes += 1;
            return;
        }

        if (isMaxNode) {
            float currentMax = Mathf.NegativeInfinity;
            foreach (var node in childs) {
                node.EvaluateNode();
                currentMax = Mathf.Max(currentMax, node.valuation);
            }
            valuation = (int)currentMax;
        }
        else {
            float currentMin = Mathf.Infinity;
            foreach (var node in childs) {
                node.EvaluateNode();
                currentMin = Mathf.Min(currentMin, node.valuation);
            }
            valuation = (int)currentMin;
        }
    }
    
    public void CreateNodes(int depth) {
        if (depth == 0) {
            return;
        }
        
        Character activeCharacter = nodeState.idTurnPlayer == 1 ? GameManager.Instance.char1 : GameManager.Instance.char2;
        Character.CharStats characterStats = nodeState.idTurnPlayer == 1 ? nodeState.c1Stats : nodeState.c2Stats;

        if (characterStats.HP <= 0) {
            //If the character is dead they shouldn't get any moves
            return;
        }
        else if (characterStats.Stun) {
            var newN = new Node(activeCharacter.Pass, this);
            childs = new List<Node>();
            childs.Add(newN);
        }
        else {
            childs = new List<Node>();
            List<Move_SO> moves = activeCharacter.PossibleMoves(characterStats.SP);
            foreach (var move in moves) {
                var newN = new Node(move, this);
                childs.Add(newN);
            }
        }
        foreach (var node in childs) {
            node.CreateNodes(depth - 1);
        }
    }
}