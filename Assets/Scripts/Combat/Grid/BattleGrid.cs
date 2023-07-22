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
    private int BORDER_SIZE = 1;
    [SerializeField] public Vector3 TILE_OFFSET;

    [SerializeField] private Vector2 gridSize;

    [SerializeField]
    private List<IUnit> gridUnits;
    List<Vector2> unitPositions;
    

    [SerializeField] private CombatTile combatTilePrefab;
    [SerializeField] private CombatBorderTile borderTilePrefab;
    [SerializeField] private Transform gridParent;  

    public PathFinder pathFinder;
    public TileGrid tileGrid;

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

        CombatManager.Instance.OnCreateNewCombat += CreateBattleGrid;
        CombatManager.Instance.OnEndCombat += DestroyBattleGrid;
    }

    //Instantiate units, then display the grid based off combat units
    //*Instantiate at the start of run time to not take up processing time*    
    private void CreateBattleGrid()
    {
        AddGridUnits();

        Vector3 CENTER_OF_GRID = Player.Instance.playerInitialCombatPosition;
        Vector3 GRID_ORIGIN = new Vector3(CENTER_OF_GRID.x - (gridSize.x/2 * CELL_SIZE) - (BORDER_SIZE/2 * CELL_SIZE) + CELL_SIZE/2, 
                                          CENTER_OF_GRID.y - (gridSize.y/2 * CELL_SIZE) - (BORDER_SIZE/2 * CELL_SIZE) + CELL_SIZE/2);

        tileGrid = new TileGrid((int)gridSize.x, (int)gridSize.y, CELL_SIZE, GRID_ORIGIN);
        for(int x = -BORDER_SIZE; x < tileGrid.GetWidth() + BORDER_SIZE; x++)
        {
            for(int y = -BORDER_SIZE; y < tileGrid.GetHeight() + BORDER_SIZE; y++)
            {
                CombatTile prefab = null;
                if(x < 0 || y < 0 || x >= tileGrid.GetWidth() || y >= tileGrid.GetHeight())
                {
                    prefab = borderTilePrefab;
                }
                else prefab = combatTilePrefab;

                CombatTile combatTile = Instantiate(prefab.transform, transform).GetComponent<CombatTile>();
                combatTile.transform.position = tileGrid.GetWorldPosition(x,y) + TILE_OFFSET;
                if(x >= 0 && y >= 0 && x < tileGrid.GetWidth() && y < tileGrid.GetHeight())
                {
                    tileGrid.grid[x,y].SetTile(combatTile);
                }
            }
        }

        pathFinder = new PathFinder(tileGrid);

        TrackUnitPositions();
        DisableAllTileColliders();
    }

    private void DestroyBattleGrid()
    {
        //destroy the grid, do something to the units?
    }

    private void AddGridUnits()
    {
        gridUnits.Clear();
        Vector2 playerPosition = GameGrid.Instance.GetTilePosition(Player.Instance.transform, CELL_SIZE);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerPosition, gridSize * CELL_SIZE, 0, Vector2.down, Mathf.Infinity, Manager.Instance.unitLM);
        gridUnits = new List<IUnit>();
        foreach(RaycastHit2D hit in hits)
        {
            IUnit newUnit = hit.collider.GetComponent<IUnit>();
            if(newUnit != null && !gridUnits.Contains(newUnit)) gridUnits.Add(newUnit);
        }

        gridUnits = gridUnits.OrderBy(unit => FindUnitMoveRank(unit)).ToList();
        CombatManager.Instance.characters = gridUnits.OfType<Character>().ToList();
        CombatManager.Instance.obstacles = gridUnits.OfType<Obstacle>().ToList();
        MoveUnitsT(gridUnits);

        OnBattleGridCreated();
    }
    
    public delegate void CreateBattleGridDelegate();
    public CreateBattleGridDelegate OnBattleGridCreated;

    public void MoveUnitsT(List<IUnit> units)
    {
        unitPositions = new List<Vector2>();

        for(int i = 0; i < units.Count; i++)
        {
            IUnit unit = units[i];
            Vector2 unitPosition = GameGrid.Instance.GetTilePosition(unit.transform, CELL_SIZE);
            if(FindUnitMoveRank(unit) != (int)UnitMoveRanks.Obstacle)
            {
                int attemptsToFindEmptyTile = 0;
                while(unitPositions.Contains(unitPosition) && attemptsToFindEmptyTile < 5)
                {
                    unitPosition = Vector2.MoveTowards(unitPosition, unit.transform.position, Vector2.Distance(unitPosition, unit.transform.position)/4);
                    unitPosition = GameGrid.Instance.GetTilePosition(FindClosestNearbyTile(unit), CELL_SIZE);
                    attemptsToFindEmptyTile++;
                }
                unit.MoveUnit(unitPosition, 0);
            }

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
        float posX = (unit.transform.position.x) - (unit.transform.position.x % CELL_SIZE);
        float posY = (unit.transform.position.y) - (unit.transform.position.y % CELL_SIZE) + CELL_SIZE/4;
        float centerOfTile = .5f * CELL_SIZE;

        //Debug.Log("looking for nearby tile");
        Vector2 nearbyEmptyTile = new Vector2();

        if(Math.Abs(centerOfTile - posY) > Math.Abs(centerOfTile - posX))
        {
            if(posY > centerOfTile) nearbyEmptyTile = unit.transform.position + Vector3.up * CELL_SIZE;
            else nearbyEmptyTile = unit.transform.position + Vector3.down * CELL_SIZE;
        }
        else
        {
            if(posX > centerOfTile) nearbyEmptyTile = unit.transform.position + Vector3.right * CELL_SIZE; 
            nearbyEmptyTile = unit.transform.position + Vector3.left * CELL_SIZE;
        }

        return nearbyEmptyTile;
    }

    private void TrackUnitPositions()
    {
        //Keeps a dictionary of the units and the tile they are on
    }

    public float GetCellSize()
    {
        return CELL_SIZE;
    }

    public void EnableTileColliders()
    {
        foreach(Node node in tileGrid.grid)
        {
            node.GetTile().collidingUnits.Clear();
            node.GetTile().EnableTrigger(true);
        }
    }

    //Enables tiles around a selected unit
    public void EnableTileColliders(int speed, Vector2 center)
    {
        for(int i = (int)(center.x - speed); i <= (int)(center.x + speed); i++)
        {
            for(int j = (int)(center.y - speed); j <= (int)(center.y + speed); j++)
            {
                if(tileGrid.grid[i,j].GetTile().collidingUnits.Count == 0) 
                {
                    //Debug.Log("tile " + i + " " + j + "active");
                    tileGrid.grid[i,j].GetTile().EnableTrigger(true, true);
                }
            }
        }
    }

    public void DisableAllTileColliders()
    {
        foreach(Node node in tileGrid.grid)
        {
            node.GetTile().EnableTrigger(false);
        }
    }

    public int CalculateDistanceBetweenTiles(int tile1x, int tile1y, int tile2x, int tile2y)
    {
        int distanceX = Math.Abs(tile1x-tile2x);
        int distanceY = Math.Abs(tile1y-tile2y);

        if(distanceX > distanceY) return distanceX;
        return distanceY; 
    }

    public CombatTile GetTile(int posX, int posY)
    {
        //Debug.Log("tileXPos " + posX + " tileYPos " + posY);
        if(posY < tileGrid.grid.GetLength(0) && posX < tileGrid.grid.GetLength(1)) return tileGrid.grid[posX,posY].GetTile();
        return null;
    }
}
