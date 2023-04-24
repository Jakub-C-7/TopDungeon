using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    protected SimpleRandomWalkData randomWalkParameters;
    public UnityEvent OnFinishedRoomGeneration;
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

        // Get a random style for the dungeon
        tilemapVisualiser.SetRandomTileStyle();

        // Generate random walk room 
        HashSet<Vector2> floor = RunRandomWalk(randomWalkParameters, startPosition);
        dungeonData.DungeonFloor.UnionWith(floor);
        dungeonData.Rooms.Add(new Room(startPosition, floor));

        // Paint new room onto the tilemap
        tilemapVisualiser.Clear();
        tilemapVisualiser.PaintFloorTiles(floor);

        //Generate walls ---
        // Combine the floor and wall positions into floor
        var wallPositions = WallGenerator.GenerateRoomColliders(dungeonData, tilemapVisualiser);

        // Update floor positions with the old wall positions
        floor.UnionWith(wallPositions);
        dungeonData.DungeonFloor.UnionWith(floor);

        // Generate another layer of walls for cleanup - works better with bigger rooms with lots of holes
        var secondLayerWallPositions = WallGenerator.CreateWalls(floor, tilemapVisualiser);
        floor.UnionWith(secondLayerWallPositions);

        // Generate walls again for the clean rooms - Thin Basic
        var cleanWallPositions = WallGenerator.GenerateCleanDungeonColliderThinBasic(floor, tilemapVisualiser, dungeonData);

        floor.ExceptWith(cleanWallPositions); // Floor except the single layer of wall around
        dungeonData.DungeonFloor.UnionWith(floor);

        PlaceSpawnPoint(new Vector2(0, 0));
        dungeonData.SpawnPoint = new Vector2(0, 0);

        // Invoke finished event
        OnFinishedRoomGeneration?.Invoke();

    }

    protected override void PlaceSpawnPoint(Vector2 position)
    {
        GameObject prefab = GameManager.instance.prefabList.Find(x => x.name == "SpawnPoint");
        GameObject spawnpoint = Instantiate(prefab, position, Quaternion.identity);

        spawnpoint.name = "SpawnPoint"; // Rename from 'prefabname(clone)' back to default

    }

    protected HashSet<Vector2> RunRandomWalk(SimpleRandomWalkData parameters, Vector2 position)
    {
        var currentPosition = position;

        // var currentPosition = new Vector2(position.x * 0.16f, position.y * 0.16f);
        HashSet<Vector2> floorPositions = new HashSet<Vector2>();

        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);

            if (parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }


}
