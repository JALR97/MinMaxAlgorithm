using System.Collections.Generic;

public class Node {
    private Move_SO MoveMade;
    private GameState nodeState;
    
    private float valuation;
    private int alpha, beta;
    private bool isMaxNode;

    private Node parent;
    private List<Node> childs;

    public Node(Move_SO moveMade, Node parent) {
        this.MoveMade = moveMade;
        this.parent = parent;
        //Update nodeState
    }

    public List<Move_SO> GenerateMoves() {
        
        return null;
    }
}


