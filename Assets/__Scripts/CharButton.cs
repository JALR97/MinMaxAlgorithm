using UnityEngine;

public class CharButton : MonoBehaviour {
    public int side;
    public Character.CharacterClass character;
    public void Click() {
        GameManager.Instance.AddChar(character, side);
    }
}
