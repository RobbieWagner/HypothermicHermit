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

    public Vector2 GetTilePosition(Transform objectTransform, float scale)
    {
        float posX = (objectTransform.position.x) - (objectTransform.position.x % scale);
        float posY = (objectTransform.position.y) - (objectTransform.position.y % scale) + scale/4;

        //Debug.Log("GetTilePos" + posX + " " + posY);

        Vector2 position = new Vector2(posX, posY);
        return position;
    }

    public Vector2 GetTilePosition(Vector2 vector, float scale)
    {
        float posX = (vector.x) - (vector.x % scale);
        float posY = (vector.y) - (vector.y % scale) + scale/4;

        Vector2 position = new Vector2(posX, posY);
        return position;
    }
}
