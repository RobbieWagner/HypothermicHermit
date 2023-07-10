using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGridManager : MonoBehaviour
{

    public float startingDistanceFromEnemies;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private LayerMask unitLM;

    //[HideInInspector] 
    private List<Unit> gridUnits; 

    public static BattleGridManager Instance {get; private set;}
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

    public void CreateBattleGrid()
    {
        gridUnits.Clear();
        Vector2 playerPosition = GameGrid.Instance.GetTilePosition(Player.Instance.transform);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerPosition, gridSize, 0, Vector2.down, Mathf.Infinity, unitLM);
        gridUnits = new List<Unit>();
        foreach(RaycastHit2D hit in hits)
        {
            gridUnits.Add(hit.collider.GetComponent<Unit>());
        }

        foreach(Unit unit in gridUnits)
        {
            unit.AddUnitToGrid();
        }
        MoveStackedUnits();
    }

    public void MoveStackedUnits()
    {
        bool movedUnits = true;
        while(movedUnits)
        {
            movedUnits = false;
            for(int i = 0; i < gridUnits.Count-1; i++)
            {
                for(int j = i + 1; j < gridUnits.Count; j++)
                {
                    Unit unitI = gridUnits[i];
                    Unit unitJ = gridUnits[j];

                    if(unitI.transform.position.Equals(unitJ.transform.position))
                    {
                        if(isMovableUnit(unitI) || isMovableUnit(unitJ))
                        {
                            if(!isMovableUnit(unitI))
                            {
                                //move unitJ, set movedUnits to true
                            }
                            else if(!isMovableUnit(unitJ))
                            {
                                //move unitI, set movedUnits to true
                            }
                        }
                    }
                }
            }
        }
    }

    public bool isMovableUnit(Unit unit)
    {
        return !!unit.GetType().Equals(typeof(Obstacle)) && !unit.GetType().Equals(typeof(Player));
    }
}
