using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{

    [SerializeField] private Combat combatPrefab;
    private GameObject combatGO;
    //[HideInInspector] 
    public List<Character> characters;
    [HideInInspector] public List<Obstacle> obstacles;
    [SerializeField] public Canvas gridWorldCanvas;

    public static CombatManager Instance {get; private set;}

    private void Awake() {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        Manager.Instance.OnGameStateChange += ToggleCombat;

        characters = new List<Character>();
        obstacles = new List<Obstacle>();
    }

    private void ToggleCombat(int gameState)
    {
        if(gameState == (int) GameStateEnum.combat && Combat.Instance == null)
        {
            CreateNewCombat();
        }
        else if(gameState != (int) GameStateEnum.combat)
        {
            EndCombat();
        }
    }

    private void CreateNewCombat()
    {
        combatGO = Instantiate(combatPrefab.gameObject, transform.parent);
        OnCreateNewCombat();
    }

    public delegate void OnCreateNewCombatDelegate();
    public event OnCreateNewCombatDelegate OnCreateNewCombat;

    //Stops combat by user pressing esc
    private void OnStopCombat()
    {
        if(Manager.Instance.GameState == (int) GameStateEnum.combat)
        {
            Manager.Instance.GameState = (int) GameStateEnum.explore;
        }
    }

    private void EndCombat()
    {
        OnEndCombat();
    }

    public delegate void OnEndCombatDelegate();
    public event OnEndCombatDelegate OnEndCombat;
}
