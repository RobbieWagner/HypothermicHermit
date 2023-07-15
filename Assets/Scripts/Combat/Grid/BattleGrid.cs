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
    private float BORDER_SIZE = 1;
    [SerializeField] private Vector3 TILE_OFFSET;

    [SerializeField] private Vector2 gridSize;

    [SerializeField]
    private List<IUnit> gridUnits;
    List<Vector2> unitPositions;
    

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
        Vector3 CENTER_OF_GRID = Player.Instance.playerInitialCombatPosition;

        float minXPos = (float) (CENTER_OF_GRID.x - (.5 * gridSize.x * CELL_SIZE));
        float minYPos = (float) (CENTER_OF_GRID.y - (.5 * gridSize.y * CELL_SIZE));
        float maxXPos = (float) (CENTER_OF_GRID.x + (.5 * gridSize.x * CELL_SIZE));
        float maxYPos = (float) (CENTER_OF_GRID.y + (.5 * gridSize.y * CELL_SIZE));

        for(float i = minXPos - border; i < maxXPos + border; i += CELL_SIZE)
        {
            float xCoord = i;
            List<CombatTile> rowTiles = new List<CombatTile>();

            for(float j = minYPos - border; j < maxYPos + border; j += CELL_SIZE)
            {
                float yCoord = j;
                GameObject newTile;
                if(xCoord < minXPos|| xCoord >= maxXPos|| yCoord < minYPos|| yCoord >= maxYPos)
                {
                    newTile = Instantiate(borderTilePrefab.gameObject, gridParent);
                }
                else newTile = Instantiate(combatTilePrefab.gameObject, gridParent);

                newTile.transform.position = new Vector3( xCoord, yCoord, 0) + TILE_OFFSET;
                newTile.name = "tile " + i + " " + j; 

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
                Debug.Log(unitPosition.ToString() + " unit pos");
                unit.MoveUnit(unitPosition);
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

        Debug.Log("looking for nearby tile");
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

    public void ReenableTileClickables()
    {
        foreach(List<CombatTile> row in tileGrid)
        {
            foreach(CombatTile tile in row)
            {
                tile.collidingUnits.Clear();
                tile.tileCollider.enabled = true;
            }
        }
    }
}