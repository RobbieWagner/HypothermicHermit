using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [HideInInspector] public bool pauseOnEscapePressed;
    [SerializeField] private List<PauseMenu> pauseMenus;
    [HideInInspector] public PauseMenu activePauseMenu;

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
            //Debug.Log("game state changed");
            if(OnGameStateChange != null) OnGameStateChange(gameState);
        }
    }

    public delegate void OnGameStateChangeDelegate(int state);
    public event OnGameStateChangeDelegate OnGameStateChange = delegate {};

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

        OnGameStateChange += ChangePauseMenu;
        GameState = (int) GameStateEnum.explore;

        pauseOnEscapePressed = true;
        activePauseMenu = pauseMenus[0];
    }

    private void ChangePauseMenu(int state)
    {
        if(state < pauseMenus.Count)
        {
            activePauseMenu = pauseMenus[state];
        }
        else 
        {
            activePauseMenu = pauseMenus[pauseMenus.Count-1];
        }
    }

    private void OnEscape()
    {
        OnEscapePressed();

        if(pauseOnEscapePressed) 
        {
            Debug.Log("pausing");
            activePauseMenu.OnPause();
        }

        pauseOnEscapePressed = true;
    }

    public delegate void OnEscapeButtonPressedDelegate();
    public event OnEscapeButtonPressedDelegate OnEscapePressed = delegate {};
}
