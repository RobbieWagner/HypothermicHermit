using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defines a Combat encounter. Only one combat encounter is allowed in a scene
public class Combat : MonoBehaviour
{
    //Serialize Field for now, will change
    public List<Enemy> combatEnemies;

    public static Combat Instance {get; private set;}
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

        InitalizeCombat();
    }

    public void InitalizeCombat()
    {
        BattleGridManager.Instance.CreateBattleGrid();
    }

    public void EndCombat()
    {
        Manager.Instance.GameState = (int) GameStateEnum.explore;
    }
}
