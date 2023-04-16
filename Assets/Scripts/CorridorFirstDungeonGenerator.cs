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
    private DungeonData dungeonData;

    protected override void RunProceduralGeneration()
    {
        // Do routine reset and get of DungeonData
        dungeonData = FindObjectOfType<DungeonData>();

        if (dungeonData == null)
        {
            dungeonData = gameObject.AddComponent<DungeonData>();
            return;
        }

        dungeonData.Reset();

        tilemapVisualiser.SetRandomTileStyle(); // Get a random style for the dungeon

        CorridorFirstGeneration();

        OnFinishedRoomGeneration?.Invoke();

    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2> floorPositions = new HashSet<Vector2>();
        HashSet<Vector2> potentialRoomPositions = new HashSet<Vector2>();

        // Create corridors
        CreateCorridors(floorPositions, potentialRoomPositions);
        floorPositions.UnionWith(dungeonData.Path);

        // Create rooms at potential room positions
        HashSet<Vector2> roomPositions = CreateRooms(potentialRoomPositions);
        floorPositions.UnionWith(roomPositions);

        // Create rooms at corridor dead ends
        List<Vector2> deadEnds = FindAllDeadEnds(floorPositions);
        var deadEndRooms = CreateRoomsAtDeadEnds(deadEnds, floorPositions);
        floorPositions.UnionWith(deadEndRooms);

        // Update dungeon data
        dungeonData.DungeonFloor.UnionWith(floorPositions);

        //Paint floor
        tilemapVisualiser.PaintFloorTiles(floorPositions);

        // WallGenerator.CreateWalls(floorPositions, tilemapVisualiser);
        var wallPositions = WallGenerator.GenerateRoomColliders(dungeonData, tilemapVisualiser);

        // Update floor positions with the old wall positions
        floorPositions.UnionWith(wallPositions);

        // Generate walls again for the clean rooms - Thin
        var cleanWallPositions = WallGenerator.GenerateCleanDungeonColliderThin(floorPositions, tilemapVisualiser);

        floorPositions.ExceptWith(cleanWallPositions); // Floor except the single layer of wall around
        dungeonData.DungeonFloor.UnionWith(floorPositions);

    }

    private HashSet<Vector2> CreateRoomsAtDeadEnds(List<Vector2> deadEnds, HashSet<Vector2> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            // if (roomFloors.Contains(position) == false)
            if (dungeonData.Path.Contains(position) == true)
            {
                var roomFloor = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(roomFloor);
                dungeonData.Rooms.Add(new Room(position, roomFloor));
            }
        }
        dungeonData.DungeonFloor.UnionWith(roomFloors);

        return roomFloors;

    }

    private List<Vector2> FindAllDeadEnds(HashSet<Vector2> floorPositions)
    {
        List<Vector2> deadEnds = new List<Vector2>();

        foreach (var position in floorPositions)
        {
            int neighbourCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                var posToCheck = new Vector2((Mathf.RoundToInt(position.x * 100) + Mathf.RoundToInt(direction.x * 100)) / 100f, (Mathf.RoundToInt(position.y * 100) + Mathf.RoundToInt(direction.y * 100)) / 100f);

                if (floorPositions.Contains(posToCheck))
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
        HashSet<Vector2> roomFloors = new HashSet<Vector2>();

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
            dungeonData.Rooms.Add(new Room(roomPosition, roomFloor));
            roomFloors.UnionWith(roomFloor);
        }

        dungeonData.DungeonFloor.UnionWith(roomFloors);

        return roomFloors;

    }

    private void CreateCorridors(HashSet<Vector2> floorPositions, HashSet<Vector2> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(startPosition);
        List<Vector2> corridor = new();

        for (int i = 0; i < corridorCount; i++)
        {
            corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);

            dungeonData.Path.UnionWith(corridor);

        }
    }
}
