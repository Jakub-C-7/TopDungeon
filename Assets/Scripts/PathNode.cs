using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public float x;
    public float y;

    public int gCost;
    public int hCost;
    public int fCost;
    public PathNode cameFromNode;



    public PathNode(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public PathNode(float x, float y, int gCost)
    {
        this.x = x;
        this.y = y;
        this.gCost = gCost;
    }


    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

}
