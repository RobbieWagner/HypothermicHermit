using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CombatPhaseEnum
{
    ally,
    enemy
}

public class CombatManager : MonoBehaviour
{

    [SerializeField] private Combat combatPrefab;
    private GameObject combatGO;
    //[HideInInspector] 
    public List<Character> characters;
    [HideInInspector] public List<Obstacle> obstacles;
    [SerializeField] public Canvas gridWorldCanvas;

    private int combatPhase;
    public int CombatPhase
    {
        get{return combatPhase;}
        set
        {
            if(value == combatPhase) return;
            combatPhase = value;
            OnPhaseChange(combatPhase);
        }
    }

    public delegate void OnPhaseChangeDelegate(int phase);
    public event OnPhaseChangeDelegate OnPhaseChange;

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

        combatPhase = -1;
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

    public void EnableEnemyClickables(int value)
    {
        //get enemy clickables enabled using Battle Grid
    }

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
