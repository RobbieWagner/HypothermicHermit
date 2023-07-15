using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Character
{
    [HideInInspector] public Vector2 playerInitialCombatPosition;

    public static Player Instance {get; private set;}

    private void Awake() {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public override void AddUnitToGrid()
    {
        
    }

    public override void MoveUnit(Vector2 newPosition, float movementDuration = 1f)
    {
        playerInitialCombatPosition = newPosition;
        base.MoveUnit(newPosition);
    }
}
