using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomsFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private float minRoomWidth = 4, minRoomHeight = 4; // Needs to remain divisible by 0.16
    [SerializeField]
    private float dungeonWidth = 20, dungeonHeight = 20; // Needs to remain divisible by 0.16
    [SerializeField]
    [Range(0, 10)]
    private float offset = 0.16f; // Space between rooms
    [SerializeField]
    private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        tilemapVisualiser.SetRandomTileStyle(); // Get a random style for the dungeon

        CreateRooms();
    }

    private void CreateRooms()
    {
        // var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new Bounds((Vector3)startPosition, new Vector3(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new Bounds(new Vector3(dungeonWidth / 2, dungeonHeight / 2, 0), new Vector3(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2> floor = new HashSet<Vector2>();

        if (randomWalkRooms)
        {
            floor = CreateRandomSimpleWalkRooms(roomList);
        }
        else
        {
            floor = CreateSimpleRooms(roomList);

        }

        // Get centers of all rooms
        List<Vector2> roomCenters = new List<Vector2>();
        foreach (var room in roomList)
        {
            // Scaling the center of the room to be divisible by standard tile size of 0.16
            var divisibleCenter = new Vector2(Mathf.RoundToInt(room.center.x / 0.16f) * 0.16f, Mathf.RoundToInt(room.center.y / 0.16f) * 0.16f);
            roomCenters.Add(divisibleCenter);

            // roomCenters.Add((Vector2)room.center);

        }

        //Connect rooms with corridors
        HashSet<Vector2> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        //Paint floor tiles
        tilemapVisualiser.PaintFloorTiles(floor);

        //Generate walls
        WallGenerator.CreateWalls(floor, tilemapVisualiser);

    }

    private HashSet<Vector2> ConnectRooms(List<Vector2> roomCenters)
    {
        HashSet<Vector2> corridors = new HashSet<Vector2>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            // closest other room centre to the current one
            Vector2 closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);

            // create a corridor between the closest
            HashSet<Vector2> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;

            corridors.UnionWith(newCorridor);  // union with other corridors to avoid duplicates
        }

        return corridors;
    }

    private HashSet<Vector2> CreateCorridor(Vector2 currentRoomCenter, Vector2 destination)
    {
        HashSet<Vector2> corridor = new HashSet<Vector2>();
        var position = currentRoomCenter;
        corridor.Add(position);

        // Go up or down until we reach the destination y
        while ((Mathf.RoundToInt(position.y * 100) != Mathf.RoundToInt(destination.y * 100)))
        {

            if (destination.y > position.y)
            {
                position = new Vector2(position.x, ((position.y * 100 + 16f) / 100));

            }
            else if (destination.y < position.y)
            {

                position = new Vector2(position.x, ((position.y * 100 + -16f) / 100));

            }
            corridor.Add(position);
        }
        // Go left or right until we reach the destination x
        while (Mathf.RoundToInt(position.x * 100) != Mathf.RoundToInt(destination.x * 100))
        {

            if (destination.x > position.x)
            {
                position = new Vector2((position.x * 100 + 16f) / 100, position.y);

            }
            else if (destination.x < position.x)
            {
                position = new Vector2((position.x * 100 + -16f) / 100, position.y);

            }
            corridor.Add(position);
        }
        return corridor;

    }

    private Vector2 FindClosestPointTo(Vector2 currentRoomCenter, List<Vector2> roomCenters)
    {
        Vector2 closest = Vector2.zero;
        float distance = float.MaxValue;

        foreach (var position in roomCenters) // iterate over all room centers
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);

            if (currentDistance < distance) // if current position is closer
            {
                distance = currentDistance; // reset the distance
                closest = position; // set it to be closest
            }
        }
        return closest;
    }

    private HashSet<Vector2> CreateSimpleRooms(List<Bounds> roomList)
    {
        HashSet<Vector2> floor = new HashSet<Vector2>();

        bool spawnPlaced = false;

        foreach (var room in roomList)
        {
            if (spawnPlaced == false)
            {
                PlaceSpawnPoint(new Vector2(room.center.x, room.center.y));
                spawnPlaced = true;
            }

            for (float column = offset; column < room.size.x - offset; column += 0.16f)
            {
                for (float row = offset; row < room.size.y - offset; row += 0.16f)
                {
                    Vector2 position = (Vector2)room.min + new Vector2(column, row);
                    // Vector2 position = new Vector2((column * 100 + room.min.x * 100) / 100, (row * 100 + room.min.y * 100) / 100);

                    floor.Add(position);
                }
            }
        }
        return floor;

    }

    private HashSet<Vector2> CreateRandomSimpleWalkRooms(List<Bounds> roomList)
    {
        HashSet<Vector2> floor = new HashSet<Vector2>();
        bool spawnPlaced = false;

        for (int i = 0; i < roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenter = new Vector2(roomBounds.center.x, roomBounds.center.y);

            if (spawnPlaced == false)
            {
                PlaceSpawnPoint(roomCenter);
                spawnPlaced = true;
            }

            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);

            foreach (var position in roomFloor)
            {
                // checking the offset of each room before adding it
                if (position.x >= (roomBounds.min.x + offset) && position.x <= (roomBounds.max.x - offset) && position.y >= (roomBounds.min.y + offset) && position.y <= (roomBounds.max.y - offset))
                {
                    floor.Add(position);
                }
            }
        }

        return floor;
    }
}
