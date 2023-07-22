using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    private PathFinder pathFinder;

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2f;

    [SerializeField] CombatTile tilePrefab;

    TileGrid gameGrid;

    public static Test Instance;

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

        Vector3 origin = new Vector3(-cellSize * width / 2, -cellSize * height / 2, 0);

        gameGrid = new TileGrid(width, height, cellSize, origin, null);
        for(int x = 0; x < gameGrid.GetWidth(); x++)
        {
            for(int y = 0; y < gameGrid.GetHeight(); y++)
            {
                CombatTile tile = Instantiate(tilePrefab.transform, transform).GetComponent<CombatTile>();
                gameGrid.grid[x,y].SetTile(tile);
                tile.transform.position = gameGrid.GetWorldPosition(x, y);
            }
        }

        pathFinder = new PathFinder(gameGrid);
    }

    public void FindPath(CombatTile tile)
    {
        int posX = tile.x;
        int posY = tile.y;
        
        List<Node> path = pathFinder.FindPath(0,0,posX,posY);
        if(path != null)
        {
            for(int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(gameGrid.GetWorldPosition(path[i].x, path[i].y), gameGrid.GetWorldPosition(path[i + 1].x, path[i + 1].y), Color.green, 5f);
            }
        }
    }
}
