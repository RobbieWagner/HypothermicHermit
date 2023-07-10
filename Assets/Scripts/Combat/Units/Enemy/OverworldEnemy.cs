using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defines an enemy found in the overworld
public class OverworldEnemy : Enemy
{
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Player") && Combat.Instance == null)
        {
            if(Manager.Instance.GameState != (int) GameStateEnum.combat)
            {
                Manager.Instance.GameState = (int) GameStateEnum.combat;
            }
        }
    }
}
