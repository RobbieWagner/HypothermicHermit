using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class IUnit : MonoBehaviour
{
    [HideInInspector] public Vector2 gridPosition;
    [SerializeField] private Animator animator;

    public virtual void AddUnitToGrid()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        CombatTile combatTile = other.gameObject.GetComponent<CombatTile>();
        
        if(combatTile != null)
        {
            combatTile.AddCollidingUnit(this);
        }
    }

    public virtual void UseUnitMovement(Vector2 newPosition)
    {
        MoveUnit(newPosition);
        OnMoveUnit(this);
    }

    public virtual void MoveUnit(Vector2 newPosition, float movementDuration = 1f)
    {
        float duration = (1-Mathf.InverseLerp(1,0, transform.position.x));
        StopAllCoroutines();
        StartMovementAnimation(newPosition);
        transform.DOMove(newPosition, movementDuration, false)
            .OnComplete(StopMovement);
        gridPosition = newPosition;
    }

    public delegate void OnMoveUnitDelegate(IUnit unit);
    public event OnMoveUnitDelegate OnMoveUnit = delegate { };

    private void StopMovement()
    {
        animator.SetTrigger("combatStop");
        BattleGrid.Instance.ReenableTileClickables();
    }

    private void StartMovementAnimation(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - new Vector2(transform.position.x, transform.position.y);
        if(Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            if(direction.x >= 0) animator.SetTrigger("combatRight");
            else animator.SetTrigger("combatLeft");
        }
        else
        {
            if(direction.y >= 0) animator.SetTrigger("combatUp");
            else animator.SetTrigger("combatDown");
        }
    }
}
