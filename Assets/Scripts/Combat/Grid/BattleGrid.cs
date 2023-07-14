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
        Vector3 CENTER_OF_GRID = Player.Instance.transform.position;

        float minXPos = (float) (CENTER_OF_GRID.x - border - (.5 * gridSize.x * CELL_SIZE));
        float minYPos = (float) (CENTER_OF_GRID.y - border - (.5 * gridSize.y * CELL_SIZE));
        float maxXPos = (float) (CENTER_OF_GRID.x + border + (.5 * gridSize.x * CELL_SIZE));
        float maxYPos = (float) (CENTER_OF_GRID.y + border + (.5 * gridSize.y * CELL_SIZE));

        for(float i = minXPos; i < maxXPos; i++)
        {
            float xCoord = i * CELL_SIZE;
            List<CombatTile> rowTiles = new List<CombatTile>();

            for(float j = minYPos; j < maxYPos; j++)
            {
                float yCoord = j * CELL_SIZE;
                GameObject newTile;
                if(xCoord < minXPos + border|| xCoord >= maxXPos - border + 1|| yCoord < minYPos + border|| yCoord >= maxYPos - border + 1)
                {
                    newTile = Instantiate(borderTilePrefab.gameObject, gridParent);
                }
                else newTile = Instantiate(combatTilePrefab.gameObject, gridParent);

                newTile.transform.position = new Vector3( xCoord, yCoord, 0) + TILE_OFFSET;

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
                if(unitPositions.Contains(unitPosition))
                {
                    unitPosition = GameGrid.Instance.GetTilePosition(FindClosestNearbyTile(unit), CELL_SIZE);
                }
                unit.transform.position = unitPosition;
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
        //float posX = MathF.Truncate(vector.x);
        float posY = (unit.transform.position.y) - (unit.transform.position.y % CELL_SIZE) + CELL_SIZE/4;
        //float posY = MathF.Truncate(vector.y) + .25f;

        float centerOfTile = .5f * CELL_SIZE;

        if(Math.Abs(centerOfTile - posY) > Math.Abs(centerOfTile - posX))
        {
            if(posY > centerOfTile) return unit.transform.position + Vector3.up * CELL_SIZE;
            return unit.transform.position + Vector3.down * CELL_SIZE;
        }

        if(posX > centerOfTile) return unit.transform.position + Vector3.right * CELL_SIZE; 
        return unit.transform.position + Vector3.left * CELL_SIZE;
    }

    private void TrackUnitPositions()
    {
        //Keeps a dictionary of the units and the tile they are on
    }
}
