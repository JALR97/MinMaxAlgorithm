using System.Collections.Generic;
using UnityEngine;

public class Node {
    private Move_SO MoveMade;
    private GameState nodeState;
    
    public float valuation;
    private int alpha, beta;
    private bool isMaxNode;

    private Node parent;
    private List<Node> childs;

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
        if (childs == null || childs.Count == 0) {
            valuation = nodeState.EvaluateState();
        }
        else {
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
    }
    
    public void CreateNodes(int depth) {
        Debug.Log("debug: in create node");
        if (depth == 0) {
            Debug.Log("debug: leaf");
            return;
        }

        childs = new List<Node>();
        
        Character activeCharacter = nodeState.idTurnPlayer == 1 ? GameManager.Instance.char1 : GameManager.Instance.char2;
        Character.CharStats characterStats = nodeState.idTurnPlayer == 1 ? nodeState.c1Stats : nodeState.c2Stats;

        if (characterStats.Stun) {
            var newN = new Node(activeCharacter.Pass, this);
            childs.Add(newN);
        }
        else {
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