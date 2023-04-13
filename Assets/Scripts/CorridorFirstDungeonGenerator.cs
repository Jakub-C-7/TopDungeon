using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f; // Percentage of rooms frequency along the corridor


    protected override void RunProceduralGeneration()
    {
        tilemapVisualiser.SetRandomTileStyle(); // Get a random style for the dungeon

        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2> floorPositions = new HashSet<Vector2>();
        HashSet<Vector2> potentialRoomPositions = new HashSet<Vector2>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2> deadEnds = FindAllDeadEnds(floorPositions);
        CreateRoomsAtDeadEnds(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);
        tilemapVisualiser.PaintFloorTiles(floorPositions);

        WallGenerator.CreateWalls(floorPositions, tilemapVisualiser);
    }

    private void CreateRoomsAtDeadEnds(List<Vector2> deadEnds, HashSet<Vector2> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2> FindAllDeadEnds(HashSet<Vector2> floorPositions)
    {
        List<Vector2> deadEnds = new List<Vector2>();

        foreach (var position in floorPositions)
        {
            int neighbourCount = 0;
            foreach (var direciton in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direciton))
                {
                    neighbourCount++;
                }

            }
            if (neighbourCount == 1)
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2> CreateRooms(HashSet<Vector2> potentialRoomPositions)
    {
        HashSet<Vector2> roomPositions = new HashSet<Vector2>();

        bool spawnPlaced = false;

        // Calculate the number of rooms we want to create
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        // Randomly sort the potentialRoomPositions and pick the number of points corresponding to desired number of rooms
        List<Vector2> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            if (spawnPlaced == false)
            {
                PlaceSpawnPoint(new Vector2(roomPosition.x, roomPosition.y));
                spawnPlaced = true;
            }

            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;

    }

    private void CreateCorridors(HashSet<Vector2> floorPositions, HashSet<Vector2> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(startPosition);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);

        }
    }
}
