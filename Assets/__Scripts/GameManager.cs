using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public static event Action<Character> OnCharSwitch; 

    private int idTurnPlayer = 1;
    [SerializeField] private TMP_Text promtText;
    [SerializeField] private TMP_Text statsUI1;
    [SerializeField] private TMP_Text statsUI2;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject TreePrintUI;
    [SerializeField] private TMP_Text TreePrintText;
    [SerializeField] private TMP_InputField depthInput;
    
    [SerializeField] private Transform CharacterPosition1;
    [SerializeField] private Transform CharacterPosition2;
    
    [SerializeField] private Character Warrior;
    [SerializeField] private Character Barbarian;
    [SerializeField] private Character Sorcerer;
    [SerializeField] private Character Ranger;

    [SerializeField] private AI_Brain Brain;

    [SerializeField] private GameObject PreGameUI;
    [SerializeField] private GameObject GameUI;

    public Character char1;
    public Character char2;
    
    private int count1 = 0;
    private int count2 = 0;

    private Character.CharacterClass temp1;
    private Character.CharacterClass temp2;
    
    //Attack buttons
    [SerializeField] private List<TMP_Text> MoveButtons;
    
    
    private void Start() {
        Prompt($"Selecciones 2 personajes para cada lado [lado derecho es IA]");
    }

    public void Prompt(string message) {
        promtText.text = message;
    }

    public void AddChar(Character.CharacterClass charClass, int side) {
        if (side == 1) {
            if (count1 == 0) {
                temp1 = charClass;
                count1++;
            }
            else {
                count1++;
                SpawnChar(1, GetCharPrefab(temp1), charClass);
            }
        }
        else {
            if (count2 == 0) {
                count2++;
                temp2 = charClass;
            }
            else {
                count2++;
                SpawnChar(2, GetCharPrefab(temp2), charClass);
            }
        }

        if (count1 == 2 && count2 == 2) {
            StartGame();
        }
    }

    private void StartGame() {
        Prompt($"Jugador 1 - {char1.ToString()}. Seleccione un ataque");
        PreGameUI.SetActive(false);
        GameUI.SetActive(true);
        UpdateUI();
    }

    private void UpdateUI() {
        statsUI1.text = char1.StatsText();
        statsUI2.text = char2.StatsText();
        Debug.Log("UpdateUI");
        for (int i = 0; i < MoveButtons.Count; i++) {
            Debug.Log("update buttons");
            MoveButtons[i].text = GetCharPrefab(char1.characterClass).moves[i].name;
        }
    }

    public void ExecMove(Move_SO move) {
        Character attacker;
        Character target;
        int newIdTurn;
        if (idTurnPlayer == 1) {
             attacker = char1;
             target = char2;
             newIdTurn = 2;
        }
        else {
            attacker = char2;
            target = char1;
            newIdTurn = 1;
        }
        if (attacker.Stats().SP < Math.Abs(move.attackerSP)) {
            
            Prompt("No tiene suficientes SP");
            return;
        }
        idTurnPlayer = newIdTurn;
        
        Prompt($"{attacker.ToString()} realiza {move.name}");
        attacker.UpdateStats(move.attackerHP, move.attackerShield, move.attackerSP, move.attackerStun);
        target.UpdateStats(move.targetHP, move.targetShield, move.targetSP, move.targetStun);
        UpdateUI();        
        OnCharSwitch?.Invoke(target);
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

    public void Restart() {
        SceneManager.LoadScene(0);
    }

    public void MinMax(int buttonId) {
        Move_SO nextMove;
        int depth = Convert.ToInt32(depthInput.text);
        if (buttonId == 1) { //Regular Minmax
            nextMove = Brain.MinMaxOption(depth);
        }
        else { //Alpha-beta pruned MinMax
            Brain.AlphaBetaOption(int.Parse(depthInput.text));
            return;
        }
        ExecMove(nextMove);
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

    public void CloseTreeView() {
        TreePrintUI.SetActive(false);
    }
    
    public void ShowTree() {
        TreePrintUI.SetActive(true);
        TreePrintText.text = Brain.root.Print();
    }
    
}
