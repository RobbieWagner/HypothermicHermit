//Code adapted from // Code from https://www.youtube.com/watch?v=alU04hvz6L4&t=1s
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    private TileGrid grid;
    public int x;
    public int y;
    
    public int gCost;
    public int hCost;
    public int fCost;

    public Node cameFromNode;

    private CombatTile tile;

    public Node(TileGrid grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    } 

    public void SetTile(CombatTile newTile)
    {
        tile = newTile;
        tile.x = x;
        tile.y = y;
    }

    public CombatTile GetTile()
    {
        return tile;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + " " + y;
    }
}
