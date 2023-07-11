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

    public Vector2 GetTilePosition(Transform objectTransform)
    {
        float posX = MathF.Truncate(objectTransform.position.x);
        float posY = MathF.Truncate(objectTransform.position.y) + .25f;

        Vector2 position = new Vector2(posX, posY);
        return position;
    }

    public Vector2 GetTilePosition(Vector2 vector)
    {
        float posX = MathF.Truncate(vector.x);
        float posY = MathF.Truncate(vector.y) + .25f;

        Vector2 position = new Vector2(posX, posY);
        return position;
    }
}
