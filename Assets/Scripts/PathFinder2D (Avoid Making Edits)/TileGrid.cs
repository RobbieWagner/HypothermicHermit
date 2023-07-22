//Code adapted from // Code from https://www.youtube.com/watch?v=alU04hvz6L4&t=1s
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    // private int[,] grid;
    public Node[,] grid;
    public Node nullTile;

    public TileGrid(int width, int height, float cellSize, Vector3 originPosition, Node nullTile = null)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.nullTile = nullTile;

        grid = new Node[width, height];

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x,y] = new Node(this, x, y);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public Node GetGridObject(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        return grid[x,y];
        return nullTile;
    }

    public int GetWidth(){return width;}
    public int GetHeight(){return height;}
}
