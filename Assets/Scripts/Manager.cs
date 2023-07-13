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

    [SerializeField] public float enemyNoticeRange;
    [SerializeField] public LayerMask unitLM;
    [SerializeField] public LayerMask playerLM;

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

    public delegate void OnGameStateChangeDelegate(int state);
    public event OnGameStateChangeDelegate OnGameStateChange;

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
    }
}
