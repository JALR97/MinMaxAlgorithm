using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    //singleton
    public static GameManager Instance;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private int idTurnPlayer = 1;
    [SerializeField] private TMP_Text statsUI1;
    [SerializeField] private TMP_Text statsUI2;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private GameObject gameOverUI;
    
    [SerializeField] private Transform CharacterPosition1;
    [SerializeField] private Transform CharacterPosition2;
    
    [SerializeField] private Character Warrior;
    [SerializeField] private Character Barbarian;
    [SerializeField] private Character Sorcerer;
    [SerializeField] private Character Ranger;

    public Character char1;
    public Character char2;
    
    //Attack buttons
    [SerializeField] private List<TMP_Text> MoveButtons;
    
    
    private void Start() {
        //
        SpawnChar(1, Warrior);
        SpawnChar(2, Barbarian);
        char2.Initialize(Character.CharacterClass.WARRIOR);

        UpdateUI();
    }

    private void UpdateUI() {
        statsUI1.text = char1.StatsText();
        statsUI2.text = char2.StatsText();

        for (int i = 0; i < MoveButtons.Count; i++) {
            MoveButtons[i].text = GetCharPrefab(char1.characterClass).moves[i].name;
        }
    }

    public void ExecMove(Move_SO move) {
        Character attacker;
        Character target;
        if (idTurnPlayer == 1) {
             attacker = char1;
             target = char2;
            idTurnPlayer = 2;
        }
        else {
            attacker = char2;
            target = char1;
            idTurnPlayer = 1;
        }
        attacker.UpdateStats(move.attackerHP, move.attackerShield, move.attackerSP, move.attackerStun);
        target.UpdateStats(move.targetHP, move.targetShield, move.targetSP, move.targetStun);
        UpdateUI();
        if (!target.Alive) {
            if (target.Backup) {
                SwitchChar(idTurnPlayer);
            }
            else {
                Win(idTurnPlayer);
            }
        }
    }

    private void SpawnChar(int id, Character character, Character.CharacterClass backupClass = Character.CharacterClass.ANY) {
        if (id == 1) {
            char1 = Instantiate(character, CharacterPosition1);
            char1.Initialize(backupClass);
        }
        else {
            char2 = Instantiate(character, CharacterPosition2);
            char2.Initialize(backupClass);
        }
        
    }
    
    private void SwitchChar(int idPlayer) {
        Character.CharacterClass newCharacterClass;
        
        if (idPlayer == 1) {
            newCharacterClass = char1.BackupClass; 
            Destroy(char1.gameObject);
        }
        else {
            newCharacterClass = char2.BackupClass;
            Destroy(char2.gameObject);
        }
        SpawnChar(idPlayer, GetCharPrefab(newCharacterClass));
        UpdateUI();
    }

    public Character GetCharPrefab(Character.CharacterClass characterClass) {
        switch (characterClass) {
            case Character.CharacterClass.WARRIOR:
                return Warrior;
            case Character.CharacterClass.SORCERER:
                return Sorcerer;
            case Character.CharacterClass.BARBARIAN:
                return Barbarian;
            case Character.CharacterClass.RANGER:
                return Ranger;
        }

        return null;
    }

    private void Win(int losingPlayer) {
        gameOverText.text = $"El Jugador #{losingPlayer} ha sido derrotado";
        gameOverUI.SetActive(true);
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            ExecMove(char1.PossibleMoves()[1]);
        }
    }
}
