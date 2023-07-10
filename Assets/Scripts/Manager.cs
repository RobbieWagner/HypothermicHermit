using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateEnum
{
    explore,
    combat
}

//Defines an instance of the entire game, controlling all factors of gameplay
public class Manager : MonoBehaviour
{

    [SerializeField] Combat combatPrefab;

    private int gameState;
    public int GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            if(value == gameState) return;
            gameState = value;
            if(OnGameStateChange != null) OnGameStateChange(gameState);
        }
    }

    public static Manager Instance {get; private set;}
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        GameState = (int) GameStateEnum.explore;
        OnGameStateChange += CreateNewCombat;
    }

    public delegate void OnGameStateChangeDelegate(int state);
    public event OnGameStateChangeDelegate OnGameStateChange;

    public void CreateNewCombat(int state)
    {
        GameObject combatGO = Instantiate(combatPrefab.gameObject);
    }

    public void CreateNewCombat(Combat combat)
    {

    }
}
