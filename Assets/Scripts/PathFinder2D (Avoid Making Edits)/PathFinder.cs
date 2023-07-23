//Code adapted from // Code from https://www.youtube.com/watch?v=alU04hvz6L4&t=1s
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{

    private TileGrid grid;
    private List<Node> openList;
    private List<Node> closedList;

    public PathFinder(TileGrid grid)
    {
        this.grid = grid;
    }

    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        Node start = grid.GetGridObject(startX, startY);
        Node end = grid.GetGridObject(endX, endY);

        openList = new List<Node>(){ start };
        closedList = new List<Node>();

        for(int x = 0; x < grid.GetWidth(); x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                Node tile = grid.GetGridObject(x, y);
                tile.gCost = int.MaxValue;
                tile.CalculateFCost();
                tile.cameFromNode = null;
            }
        }

        start.gCost = 0;
        //returning null
        //Debug.Log(grid.grid[startX, startY].ToString() + grid.grid[endX,endY].ToString());
        start.hCost = CalculateDistance(start, end);
        start.CalculateFCost();

        while (openList.Count > 0)
        {
            Node current = GetLowestFCostNode(openList);
            if(current == end)
            {
                return CalculatePath(end);
            }

            openList.Remove(current);
            closedList.Add(current);
            
            if(current.GetTile().collidingUnits.Count == 0 || current == start)
            {
                foreach (Node neighbor in GetNeighbors(current))
                {
                    if(closedList.Contains(neighbor)) continue;

                    int GCost = current.gCost + CalculateDistance(current, neighbor);
                    if(GCost < neighbor.gCost)
                    {
                        neighbor.cameFromNode = current;
                        neighbor.gCost = GCost;
                        neighbor.hCost = CalculateDistance(neighbor, end);
                        neighbor.CalculateFCost();

                        if(!openList.Contains(neighbor))
                        {
                            //Debug.Log(neighbor.gCost);
                            openList.Add(neighbor);
                        }
                    }
                }
            }
        }
        return null;
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighborNodes = new List<Node>();

        if(node.x - 1 >= 0)
            neighborNodes.Add(GetNode(node.x - 1, node.y));
        if(node.y - 1 >= 0) 
            neighborNodes.Add(GetNode(node.x, node.y - 1));
        if(node.y + 1 < grid.GetHeight()) 
            neighborNodes.Add(GetNode(node.x, node.y + 1));
        if(node.x + 1 < grid.GetWidth())
            neighborNodes.Add(GetNode(node.x + 1, node.y));

        if(node.x + 1 < grid.GetWidth())
        {
            if(node.y - 1 >= 0) 
                neighborNodes.Add(GetNode(node.x + 1, node.y - 1));
            if(node.y + 1 < grid.GetHeight()) 
                neighborNodes.Add(GetNode(node.x + 1, node.y + 1));
        }

        if(node.x - 1 >= 0)
        {
            if(node.y - 1 >= 0) 
                neighborNodes.Add(GetNode(node.x - 1, node.y - 1));
            if(node.y + 1 < grid.GetHeight()) 
                neighborNodes.Add(GetNode(node.x - 1, node.y + 1));
        }

        return neighborNodes;
    }

    private List<Node> CalculatePath(Node end)
    {
        List<Node> pathNodes = new List<Node>();

        pathNodes.Add(end);
        Node current = end;
        while(current.cameFromNode != null)
        {
            pathNodes.Add(current.cameFromNode);
            current = current.cameFromNode;
        }
        pathNodes.Reverse();
        pathNodes.RemoveAt(0);
        return pathNodes;
    }

    public int CalculateDistance(Node start, Node end)
    {
        int xDistance = Mathf.Abs(start.x - end.x);
        int yDistance = Mathf.Abs(start.y - end.y);
        int distance = Mathf.Abs(xDistance - yDistance);
        return Mathf.Min(xDistance, yDistance) + distance;
    }

    private Node GetLowestFCostNode(List<Node> nodeList)
    {
        Node lowestFCostNode = nodeList[0];
        for(int i = 1; i < nodeList.Count; i++)
        {
            if(nodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = nodeList[i];
            }
        }

        return lowestFCostNode;
    }

    private Node GetNode(int x, int y)
    {
        return grid.grid[x,y];
    }

    public TileGrid GetGrid()
    {
        return grid;
    }
}
