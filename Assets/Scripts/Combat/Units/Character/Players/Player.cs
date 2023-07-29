using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Character
{
    [HideInInspector] public Vector2 playerInitialCombatPosition;

    public static Player Instance {get; private set;}

    protected override void Awake() {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        base.Awake();
    }

    public override void AddUnitToGrid()
    {
        
    }

    public override IEnumerator MoveUnit(Vector2 newPosition, int spentMovement, float movementDuration = 1f)
    {
        playerInitialCombatPosition = newPosition;
        yield return StartCoroutine(base.MoveUnit(newPosition, spentMovement));
        StopCoroutine(MoveUnit(newPosition, spentMovement, movementDuration));
    }

    protected void OnActionSelectionUp()
    {
        IUnit unit = null;
        if(Combat.Instance != null) unit = Combat.Instance.GetActingUnit();
        if(unit != null && unit == this) CurrentAction++;
    }

    protected void OnActionSelectionDown()
    {
        IUnit unit = null;
        if(Combat.Instance != null) unit = Combat.Instance.GetActingUnit();
        if(unit != null && unit == this) CurrentAction--;
    }
}
