using UnityEngine;

[CreateAssetMenu(menuName = "Character/Stats")]
public class ClassStats_SO : ScriptableObject {
    
    //Max stats
    public int maxHP;
    public int maxShield;
    public int maxSP;
    
    //Initial stats
    public int initHP;
    public int initShield;
    public int initSP;
}
