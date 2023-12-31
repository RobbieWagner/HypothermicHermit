using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CombatPhaseEnum
{
    ally,
    enemy
}

// Manages the combat starting and execution
public class CombatManager : MonoBehaviour
{

    [SerializeField] private Combat combatPrefab;
    private GameObject combatGO;
    //[HideInInspector] 
    public List<Character> characters;
    [HideInInspector] public List<Obstacle> obstacles;
    [SerializeField] public Canvas gridWorldCanvas;

    [HideInInspector] public bool inCombat;
    [HideInInspector] public bool canEndTurn;

    private int combatPhase;
    public int CombatPhase
    {
        get{return combatPhase;}
        set
        {
            if(value == combatPhase) return;
            combatPhase = value;
            //Debug.Log(combatPhase);
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
        inCombat = false;
        canEndTurn = true;
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
        Manager.Instance.canPause = false;
        combatGO = Instantiate(combatPrefab.gameObject, transform.parent);
        OnCreateNewCombat();
    }

    public delegate void OnCreateNewCombatDelegate();
    public event OnCreateNewCombatDelegate OnCreateNewCombat;

    public void NextCombatPhase()
    {
        //Debug.Log("phase change " + CombatPhase);
        if(CombatPhase == (int) CombatPhaseEnum.ally) 
        {
            //Debug.Log("hi");
            CombatPhase = (int) CombatPhaseEnum.enemy;
            CombatCameraMovement.Instance.canMove = false;
        }
        else if(CombatPhase == (int) CombatPhaseEnum.enemy) 
        {
            //Debug.Log("hello");
            CombatPhase = (int) CombatPhaseEnum.ally;
        }
    }

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

    public void TryTakeAction(IUnit user, IUnit targetUnit)
    {
        //if(can take action)
        BattleGrid.Instance.DisableAllTileColliders();
        CursorController.Instance.UnsetSelectedClickable();
        Combat.Instance.DisableTargetClickables();
        OnTakeAction(targetUnit);
    }

    public delegate void OnTakeActionDelegate(IUnit targetUnit);
    public event OnTakeActionDelegate OnTakeAction = delegate {};
}
