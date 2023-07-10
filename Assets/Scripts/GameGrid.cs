using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    public static GameGrid Instance {get; private set;}

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
    }

    public Vector2 GetTilePosition(Transform transform)
    {
        float posX = MathF.Truncate(transform.position.x);
        float posY = MathF.Truncate(transform.position.y);

        Vector2 position = new Vector2(posX, posY);
        return position;
    }
}
