using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum UnitMoveRanks
{
    Obstacle,
    Player,
    Ally,
    Enemy
}

public class BattleGrid : MonoBehaviour
{
    [SerializeField] private float CELL_SIZE = 1;
    private float BORDER_SIZE = 5;
    [SerializeField] private Vector3 TILE_OFFSET;

    [SerializeField] private Vector2 gridSize;

    [SerializeField]
    private List<IUnit> gridUnits;

    [SerializeField] private CombatTile combatTilePrefab;
    [SerializeField] private CombatBorderTile borderTilePrefab;
    [SerializeField] private Transform gridParent;  
    private List<List<CombatTile>> tileGrid;

    public static BattleGrid Instance {get; private set;}
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

        gridUnits = new List<IUnit>();
        tileGrid = new List<List<CombatTile>>();

        CombatManager.Instance.OnCreateNewCombat += CreateBattleGrid;
        CombatManager.Instance.OnEndCombat += DestroyBattleGrid;
    }

    //Instantiate units, then display the grid based off combat units
    private void CreateBattleGrid()
    {
        AddGridUnits();

        float border = CELL_SIZE * BORDER_SIZE;
        Vector3 centerOfGrid = Player.Instance.transform.position;

        float minXPos = (float) (centerOfGrid.x - border - (.5 * gridSize.x));
        float minYPos = (float) (centerOfGrid.y - border - (.5 * gridSize.y));
        float maxXPos = (float) (centerOfGrid.x + border + (.5 * gridSize.x));
        float maxYPos = (float) (centerOfGrid.y + border + (.5 * gridSize.y));

        for(float i = minXPos; i <= maxXPos; i += CELL_SIZE)
        {
            List<CombatTile> rowTiles = new List<CombatTile>();

            for(float j = minYPos; j <= maxYPos; j += CELL_SIZE)
            {
                GameObject newTile;
                if(i < minXPos + border - 1 || i > maxXPos - border + 1|| j < minYPos + border - 1 || j > maxYPos - border + 1)
                {
                    newTile = Instantiate(borderTilePrefab.gameObject, gridParent);
                }
                else newTile = Instantiate(combatTilePrefab.gameObject, gridParent);

                newTile.transform.position = new Vector3( i, j, 0) + TILE_OFFSET;

                rowTiles.Add(newTile.GetComponent<CombatTile>());
            }

            tileGrid.Add(rowTiles);
        }

        TrackUnitPositions();
    }

    private void DestroyBattleGrid()
    {
        //destroy the grid, do something to the units?
    }

    private void AddGridUnits()
    {
        gridUnits.Clear();
        Vector2 playerPosition = GameGrid.Instance.GetTilePosition(Player.Instance.transform);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerPosition, gridSize, 0, Vector2.down, Mathf.Infinity, Manager.Instance.unitLM);
        gridUnits = new List<IUnit>();
        foreach(RaycastHit2D hit in hits)
        {
            IUnit newUnit = hit.collider.GetComponent<IUnit>();
            if(newUnit != null && !gridUnits.Contains(newUnit)) gridUnits.Add(newUnit);
        }

        gridUnits = gridUnits.OrderBy(unit => FindUnitMoveRank(unit)).ToList();
        CombatManager.Instance.characters = gridUnits.OfType<Character>().ToList();
        CombatManager.Instance.obstacles = gridUnits.OfType<Obstacle>().ToList();
        MoveStackedUnits(gridUnits);

        OnBattleGridCreated();
    }
    
    public delegate void CreateBattleGridDelegate();
    public CreateBattleGridDelegate OnBattleGridCreated;

    public void MoveStackedUnits(List<IUnit> units)
    {
        List<Vector2> unitPositions = new List<Vector2>();

        for(int i = 0; i < units.Count; i++)
        {
            IUnit unit = units[i];
            Vector2 unitPosition = GameGrid.Instance.GetTilePosition(unit.transform);
            if(FindUnitMoveRank(unit) != (int)UnitMoveRanks.Obstacle && unitPositions.Contains(unitPosition))
            {
                unitPosition = GameGrid.Instance.GetTilePosition(FindClosestNearbyTile(unit));
            }

            unit.transform.position = unitPosition;
            unitPositions.Add(unitPosition);
        }
    }

    public int FindUnitMoveRank(IUnit unit)
    {
        Type unitType = unit.GetType();
        if(unitType.Equals(typeof(Obstacle))) return (int) UnitMoveRanks.Obstacle;
        else if(unitType.Equals(typeof(Ally))) return (int) UnitMoveRanks.Ally;
        else if(unitType.Equals(typeof(Player))) return (int) UnitMoveRanks.Player;
        else return (int) UnitMoveRanks.Enemy;
    }

    //finds the best blank spot available for the unit
    public Vector2 FindClosestNearbyTile(IUnit unit)
    {
        float posX = (float)(unit.transform.position.x - Math.Truncate(unit.transform.position.x));
        float posY = (float)(unit.transform.position.y - Math.Truncate(unit.transform.position.y));

        if(Math.Abs(.5 - posY) > Math.Abs(.5 - posX))
        {
            if(posY > .5) return unit.transform.position + Vector3.up;
            return unit.transform.position + Vector3.down;
        }

        if(posX > .5) return unit.transform.position + Vector3.right;
        return unit.transform.position + Vector3.left;
    }

    private void TrackUnitPositions()
    {
        //Keeps a dictionary of the units and the tile they are on
    }
}
