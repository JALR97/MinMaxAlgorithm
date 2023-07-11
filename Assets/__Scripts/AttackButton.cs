using UnityEngine;

public class AttackButton : MonoBehaviour {

    [SerializeField] private int buttonId;

    public void Click() {
        Move_SO move = GameManager.Instance.char1.moves[buttonId - 1];
        if (move.attackerSP <= GameManager.Instance.char1.Stats().SP) {
            GameManager.Instance.ExecMove(move);
        }
        Debug.Log($"Click en botton, attack: {move.name}");
    }
}
