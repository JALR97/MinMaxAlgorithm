using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour {

    [SerializeField] private int buttonId;
    [SerializeField] private bool PlayerButtons;
    
    public void Click() {
        if (PlayerButtons) {
            Move_SO move = GameManager.Instance.char1.moves[buttonId - 1];
            if (move.attackerSP <= GameManager.Instance.char1.Stats().SP) {
                GameManager.Instance.ExecMove(move);
            }
        }
        else {
            GameManager.Instance.MinMax(buttonId);
        }
    }

    private void Awake() {
        GameManager.OnCharSwitch += Toggle;
    }
    
    private void OnDestroy() {
        GameManager.OnCharSwitch -= Toggle;
    }
  
    private void Toggle(Character obj) {
        var button = GetComponent<Button>();
        button.interactable = !button.interactable;
    }
}
