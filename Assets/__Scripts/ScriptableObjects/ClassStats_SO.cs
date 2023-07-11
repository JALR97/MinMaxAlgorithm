using UnityEngine;

[CreateAssetMenu(menuName = "Character/Stats")]
public class ClassStats_SO : ScriptableObject {
    
    //initial stats
    public int HP;
    public int shield;
    public int SP;
    public bool stun = false;
    
    //Max stats
    public int maxHP;
    public int maxShield;
    public int maxSP;
}
