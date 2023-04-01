using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualiser tilemapVisualiser)
    {
        var initialWallPositions = FindWallsInDirections(floorPositions, Direction2D.eightDirectionsList);

        // Create the first set of walls around the room
        foreach (var position in initialWallPositions)
        {
            tilemapVisualiser.PaintSingleBasicWallToWall(position); // add to wall tilemap
            tilemapVisualiser.PaintSingleBasicWallToFloor(position); // add to floor tilemap
            floorPositions.Add(position); // add to floor pos list

        }

        tilemapVisualiser.wallTilemap.ClearAllTiles(); // Clear the entire wall tilemap

        // Find positions of new, cleaned rooms
        var newWallPositions = FindWallsInDirectionThin(floorPositions, Direction2D.eightDirectionsList);

        // Paint new wall positions to the wall tilemap
        foreach (var position in newWallPositions)
        {
            tilemapVisualiser.PaintSingleBasicWallToWall(position);

        }

    }

    public static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;

                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }

    public static HashSet<Vector2Int> FindWallsInDirectionThin(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;

                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(position); // The position of the tile becomes the wall
                }
            }
        }
        return wallPositions;
    }
}
