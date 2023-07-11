using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitMoveRanks
{
    Obstacle,
    Player,
    Ally,
    Enemy
}

public class BattleGridManager : MonoBehaviour
{

    public float startingDistanceFromEnemies;
    [SerializeField] private Vector2 gridSize;

    [SerializeField]
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

        gridUnits = new List<Unit>();
    }

    public void CreateBattleGrid()
    {
        gridUnits.Clear();
        Vector2 playerPosition = GameGrid.Instance.GetTilePosition(Player.Instance.transform);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerPosition, gridSize, 0, Vector2.down, Mathf.Infinity, Manager.Instance.unitLM);
        gridUnits = new List<Unit>();
        foreach(RaycastHit2D hit in hits)
        {
            Unit newUnit = hit.collider.GetComponent<Unit>();
            if(!gridUnits.Contains(newUnit)) gridUnits.Add(newUnit);
        }

        gridUnits = gridUnits.OrderBy(unit => FindUnitMoveRank(unit)).ToList();
        MoveStackedUnits(gridUnits);
    }

    public void MoveStackedUnits(List<Unit> units)
    {
        List<Vector2> unitPositions = new List<Vector2>();

        for(int i = 0; i < units.Count; i++)
        {
            Unit unit = units[i];
            Vector2 unitPosition = GameGrid.Instance.GetTilePosition(unit.transform);
            if(FindUnitMoveRank(unit) != (int)UnitMoveRanks.Obstacle && unitPositions.Contains(unitPosition))
            {
                unitPosition = GameGrid.Instance.GetTilePosition(FindClosestNearbyTile(unit));
            }

            unit.transform.position = unitPosition;
            unitPositions.Add(unitPosition);
        }
    }

    public int FindUnitMoveRank(Unit unit)
    {
        Type unitType = unit.GetType();
        if(unitType.Equals(typeof(Obstacle))) return (int) UnitMoveRanks.Obstacle;
        else if(unitType.Equals(typeof(Ally))) return (int) UnitMoveRanks.Ally;
        else if(unitType.Equals(typeof(Player))) return (int) UnitMoveRanks.Player;
        else return (int) UnitMoveRanks.Enemy;
    }

    //finds the best blank spot available for the unit
    public Vector2 FindClosestNearbyTile(Unit unit)
    {
        float posX = (float)(unit.transform.position.x - Math.Truncate(unit.transform.position.x));
        float posY = (float)(unit.transform.position.y - Math.Truncate(unit.transform.position.y));

        Debug.Log("hello?");

        // if(Math.Abs(.5 - posX) < .2 && Math.Abs(.5 - posY) < .2)
        // {
        //     if(posX > .5f && posY > .5f) return unit.transform.position + Vector3.up + Vector3.right;
        //     else if(posX > .5f && posY <= .5f) return unit.transform.position + Vector3.down + Vector3.right;
        //     else if(posY > .5f) return unit.transform.position + Vector3.up + Vector3.left;
        //     else return unit.transform.position + Vector3.down + Vector3.left;
        // }

        if(Math.Abs(.5 - posY) > Math.Abs(.5 - posX))
        {
            if(posY > .5) return unit.transform.position + Vector3.up;
            return unit.transform.position + Vector3.down;
        }

        if(posX > .5) return unit.transform.position + Vector3.right;
        return unit.transform.position + Vector3.left;
    }
}
