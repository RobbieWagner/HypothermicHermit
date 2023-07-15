using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defines an enemy found in the overworld
public class OverworldEnemy : Enemy
{
    [SerializeField] private CircleCollider2D EnemyCombatTrigger;
    [SerializeField] private float sightDistance = 8f;
    private bool canSeePlayer;

    private void Awake() 
    {
        EnemyCombatTrigger.radius = Manager.Instance.enemyNoticeRange;    
        canSeePlayer = false;

        CombatManager.Instance.OnCreateNewCombat += DisableEnemyCombatTriggers;
    }

    private void FixedUpdate() 
    {
        if(canSeePlayer && Combat.Instance == null)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, Vector2.MoveTowards(transform.position, Player.Instance.transform.position, sightDistance), Manager.Instance.playerLM); 
            if(hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                if(Manager.Instance.GameState != (int) GameStateEnum.combat)
                {
                    Manager.Instance.GameState = (int) GameStateEnum.combat;
                }
            }
        }  
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player") && !canSeePlayer) canSeePlayer = true;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        
        if(other.gameObject.CompareTag("Player")) canSeePlayer = false;
    }

    private void DisableEnemyCombatTriggers()
    {
        EnemyCombatTrigger.enabled = false;
    }
}
