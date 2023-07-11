using UnityEngine;

[CreateAssetMenu(menuName = "Character/Move")]
public class Move_SO : ScriptableObject {
    //Action info
    public new string name;
    public Character.CharacterClass characterCharacterClass;
    
    //Attacker info
    public int attackerHP;
    public int attackerShield;
    public int attackerSP;
    public bool attackerStun;
    
    //Target info
    public int targetHP;
    public int targetShield;
    public int targetSP;
    public bool targetStun;
}
