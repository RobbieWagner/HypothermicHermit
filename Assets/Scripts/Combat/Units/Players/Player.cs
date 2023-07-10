using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Unit
{

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
        transform.position = GameGrid.Instance.GetTilePosition(transform);
    }
}
