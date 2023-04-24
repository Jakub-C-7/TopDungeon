using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WallGenerator
{
    public static HashSet<Vector2> CreateWalls(HashSet<Vector2> floorPositions, TilemapVisualiser tilemapVisualiser)
    {
        tilemapVisualiser.wallTilemap.ClearAllTiles(); // Clear the entire wall tilemap

        var wallPositions = FindWallsInDirections(floorPositions, Direction2D.eightDirectionsList);

        // Create the first set of walls around the room
        foreach (var position in wallPositions)
        {
            tilemapVisualiser.PaintSingleBasicWallToWall(position); // add to wall tilemap
            tilemapVisualiser.PaintSingleBasicWallToFloor(position); // add to floor tilemap

        }
        return wallPositions;

    }

    public static HashSet<Vector2> GenerateRoomColliders(DungeonData dungeonData, TilemapVisualiser tilemapVisualiser)
    {
        tilemapVisualiser.wallTilemap.ClearAllTiles();

        //create a hashset that contains all the tiles that represent the dungeon
        HashSet<Vector2> dungeonTiles = new HashSet<Vector2>();
        foreach (Room room in dungeonData.Rooms)
        {
            dungeonTiles.UnionWith(room.FloorTiles);
        }
        dungeonTiles.UnionWith(dungeonData.Path);

        //Find the outline of the dungeon that will be our walls / collider around the dungeon
        HashSet<Vector2> colliderTiles = new HashSet<Vector2>();
        foreach (Vector2 tilePosition in dungeonTiles)
        {
            foreach (Vector2 direction in Direction2D.eightDirectionsList)
            {
                Vector2 newPosition = new Vector2((Mathf.RoundToInt(tilePosition.x * 100) + Mathf.RoundToInt(direction.x * 100)) / 100f, (Mathf.RoundToInt(tilePosition.y * 100) + Mathf.RoundToInt(direction.y * 100)) / 100f);
                if (dungeonTiles.Contains(newPosition) == false)
                {
                    colliderTiles.Add(newPosition);
                    continue;
                }
            }
        }

        foreach (Vector2 pos in colliderTiles)
        {
            tilemapVisualiser.PaintSingleBasicWallToWall(pos);
            tilemapVisualiser.PaintSingleBasicWallToFloor(pos);

        }
        return colliderTiles;

    }

    public static HashSet<Vector2> GenerateCleanDungeonColliderThin(HashSet<Vector2> floorPositions, TilemapVisualiser tilemapVisualiser, DungeonData dungeonData)
    {
        tilemapVisualiser.wallTilemap.ClearAllTiles(); // Clear the entire wall tilemap

        var wallPositions = FindWallsInDirectionThin(floorPositions, Direction2D.eightDirectionsList);

        // Remove the path from all generated wall positions
        wallPositions.ExceptWith(dungeonData.Path);

        foreach (var room in dungeonData.Rooms)
        {
            wallPositions.ExceptWith(room.FloorTiles);

        }

        wallPositions.ExceptWith(dungeonData.DungeonFloor);

        // Paint new wall positions to the wall tilemap
        foreach (var position in wallPositions)
        {
            tilemapVisualiser.PaintSingleBasicWallToWall(position);

        }
        return wallPositions;
    }

    public static HashSet<Vector2> GenerateCleanDungeonColliderThinBasic(HashSet<Vector2> floorPositions, TilemapVisualiser tilemapVisualiser, DungeonData dungeonData)
    {
        tilemapVisualiser.wallTilemap.ClearAllTiles(); // Clear the entire wall tilemap

        var wallPositions = FindWallsInDirectionThin(floorPositions, Direction2D.eightDirectionsList);

        // Paint new wall positions to the wall tilemap
        foreach (var position in wallPositions)
        {
            tilemapVisualiser.PaintSingleBasicWallToWall(position);

        }
        return wallPositions;
    }

    public static HashSet<Vector2> FindWallsInDirections(HashSet<Vector2> floorPositions, List<Vector2> directionList)
    {
        HashSet<Vector2> wallPositions = new HashSet<Vector2>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = new Vector2(((Mathf.RoundToInt(position.x * 100) + Mathf.RoundToInt(direction.x * 100)) / 100f), ((Mathf.RoundToInt(position.y * 100) + Mathf.RoundToInt(direction.y * 100)) / 100f));

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
                var neighbourPosition = new Vector2(((Mathf.RoundToInt(position.x * 100) + Mathf.RoundToInt(direction.x * 100)) / 100f), ((Mathf.RoundToInt(position.y * 100) + Mathf.RoundToInt(direction.y * 100)) / 100f));

                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(position);
                }
            }
        }
        return wallPositions;
    }

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
