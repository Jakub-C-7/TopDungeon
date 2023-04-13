using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinding
{
    private List<PathNode> openList;
    private List<PathNode> closedList;


    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;


    public List<PathNode> FindPath(float startX, float startY, float endX, float endY)
    {
        startX = makeDivisibleBy(startX, 0.16f);//Make sure co-ordinates are a multiple of 0.16;
        startY = makeDivisibleBy(startY, 0.16f);
        endX = makeDivisibleBy(endX, 0.16f);
        endY = makeDivisibleBy(endY, 0.16f);

        PathNode startNode = new PathNode(startX, startY);
        PathNode endNode = new PathNode(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode.x == endNode.x && currentNode.y == endNode.y)
            {
                return CalculatePath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Any(node => node.x == neighbourNode.x && node.y == neighbourNode.y))
                {
                    continue;
                }
                if (Physics2D.OverlapBox(new Vector2(neighbourNode.x, neighbourNode.y), new Vector2(0.16f, 0.16f), 0, LayerMask.GetMask("Actor", "Blocking")) != null)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                //Debug.Log(neighbourNode.x + " : " + neighbourNode.y);

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Any(node => node.x == neighbourNode.x && node.y == neighbourNode.y))
                    {
                        openList.Add(neighbourNode);
                    }
                }

            }
        }
        Debug.Log("no path");

        return null;


    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();



        neighbourList.Add(CheckAndReturnNode((Mathf.RoundToInt(currentNode.x * 100) + 16) / 100f, currentNode.y));
        neighbourList.Add(CheckAndReturnNode((Mathf.RoundToInt(currentNode.x * 100) - 16) / 100f, currentNode.y));
        neighbourList.Add(CheckAndReturnNode(currentNode.x, (Mathf.RoundToInt(currentNode.y * 100) - 16) / 100f));
        neighbourList.Add(CheckAndReturnNode(currentNode.x, (Mathf.RoundToInt(currentNode.y * 100) + 16) / 100f));

        return neighbourList;
    }

    private PathNode CheckAndReturnNode(float x, float y)
    {
        foreach (PathNode node in openList)
        {
            if (node.x == x && node.y == y)
            {
                return node;
            }
        }
        return new PathNode(x, y, int.MaxValue);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;


    }


    private float makeDivisibleBy(float numToChange, float multipleOfNum)
    {
        return Mathf.RoundToInt(numToChange / multipleOfNum) * multipleOfNum;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs((int)(a.x * 100) - (int)(b.x * 100));
        int yDistance = Mathf.Abs((int)(a.y * 100) - (int)(b.y * 100));

        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;


    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}
