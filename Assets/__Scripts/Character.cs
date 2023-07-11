using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public enum Class {
        WARRIOR,
        SORCERER,
        BARBARIAN,
        RANGER,
        ANY
    }
    [SerializeField] private Class characterClass;
    [SerializeField] private ClassStats_SO stats;
    [SerializeField] private List<Move_SO> moves;
    
}