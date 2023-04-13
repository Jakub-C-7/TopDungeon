using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WallGenerator
{
    public static HashSet<Vector2> CreateWalls(HashSet<Vector2> floorPositions, TilemapVisualiser tilemapVisualiser)
    {
        var initialWallPositions = FindWallsInDirections(floorPositions, Direction2D.eightDirectionsList);

        // Create the first set of walls around the room
        foreach (var position in initialWallPositions)
        {
            tilemapVisualiser.PaintSingleBasicWallToWall(position); // add to wall tilemap
            tilemapVisualiser.PaintSingleBasicWallToFloor(position); // add to floor tilemap
            floorPositions.Add(position); // add to floor pos list

        }

        HashSet<Vector2> capturedFloorPositions = new();
        capturedFloorPositions.UnionWith(floorPositions);

        tilemapVisualiser.wallTilemap.ClearAllTiles(); // Clear the entire wall tilemap

        var newWallPositions = FindWallsInDirections(floorPositions, Direction2D.eightDirectionsList);

        foreach (var position in newWallPositions)
        {
            tilemapVisualiser.PaintSingleBasicWallToWall(position);
            tilemapVisualiser.PaintSingleBasicWallToFloor(position); // add to floor tilemap
            floorPositions.Add(position); // add to floor pos list

        }

        return capturedFloorPositions;

    }

    public static void GenerateDungeonCollider(HashSet<Vector2> floorPositions, TilemapVisualiser tilemapVisualiser)
    {
        tilemapVisualiser.wallTilemap.ClearAllTiles(); // Clear the entire wall tilemap

        var initialWallPositions = FindWallsInDirectionThin(floorPositions, Direction2D.eightDirectionsList);

        // Paint new wall positions to the wall tilemap
        foreach (var position in initialWallPositions)
        {
            tilemapVisualiser.PaintSingleBasicWallToWall(position);

        }
    }

    public static HashSet<Vector2> FindWallsInDirections(HashSet<Vector2> floorPositions, List<Vector2> directionList)
    {
        HashSet<Vector2> wallPositions = new HashSet<Vector2>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                // var neighbourPosition = ((position * 100) + (direction * 100)) / 100;
                var neighbourPosition = position + direction;

                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }

    public static HashSet<Vector2> FindWallsInDirectionThin(HashSet<Vector2> floorPositions, List<Vector2> directionList)
    {
        HashSet<Vector2> wallPositions = new HashSet<Vector2>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;

                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(position);
                }
            }
        }
        return wallPositions;
    }

    // Finds the positions of the outer-most tile layer of the room to create a thin wall.
    // public static HashSet<Vector2> FindWallsInDirectionThin(HashSet<Vector2> floorPositions, List<Vector2> directionList)
    // {
    //     HashSet<Vector2> wallPositions = new HashSet<Vector2>();

    //     foreach (var position in floorPositions)
    //     {
    //         wallPositions.Add(position);
    //         // foreach (var direction in directionList)
    //         // {
    //         //     // var neighbourPosition = (position * 100) + (direction * 100) / 100;
    //         //     // var neighbourPosition = ((position * 100) + (direction * 100)) / 100;


    //         //     // if (floorPositions.Contains(neighbourPosition, new Vector2Comparer()) == false)
    //         //     // {
    //         //     wallPositions.Add(position); // The position of the tile becomes the wall
    //         //                                  // Debug.Log(position + " Position being added of neighbour: " + neighbourPosition);

    //         //     // }
    //         // }
    //     }
    //     return wallPositions;
    // }


    // Override for comparer of two vectors
    // public class Vector2Comparer : IEqualityComparer<Vector2>
    // {

    //     public bool Equals(Vector2 v1, Vector2 v2)
    //     {
    //         // if ((int)(v1.x * 100) == (int)(v2.x * 100) && (int)(v1.y * 100) == (int)(v2.y * 100))
    //         // if (Mathf.RoundToInt(v1.x * 100) == Mathf.RoundToInt(v2.x * 100) && Mathf.RoundToInt(v1.y * 100) == Mathf.RoundToInt(v2.y * 100))
    //         if (v1 == v2)
    //         {
    //             return true;

    //         }
    //         else
    //         {

    //             return false;
    //         }

    //     }

    //     public int GetHashCode(Vector2 obj)
    //     {
    //         throw new NotImplementedException("Unable to generate a hash code for thresholds, do not use this for grouping");

    //     }
    // }
}
