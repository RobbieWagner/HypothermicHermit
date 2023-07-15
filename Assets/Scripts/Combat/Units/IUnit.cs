using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class IUnit : MonoBehaviour
{
    [HideInInspector] public Vector2 gridPosition;

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
        BattleGrid.Instance.ReenableTileClickables();
        OnMoveUnit(this);
    }

    public virtual void MoveUnit(Vector2 newPosition, float movementDuration = 1f)
    {
        float duration = (1-Mathf.InverseLerp(1,0, transform.position.x));
        transform.DOMove(newPosition, movementDuration, false);
        Debug.Log(transform.position.ToString() + " tween");
        gridPosition = newPosition;
    }

    public delegate void OnMoveUnitDelegate(IUnit unit);
    public event OnMoveUnitDelegate OnMoveUnit = delegate { };


}
