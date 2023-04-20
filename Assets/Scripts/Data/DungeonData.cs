using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all the data about our dungeon.
/// Useful when creating a save / load system
/// </summary>
public class DungeonData : MonoBehaviour
{
    public List<Room> Rooms { get; set; } = new List<Room>();
    public HashSet<Vector2> Path { get; set; } = new HashSet<Vector2>();
    public HashSet<Vector2> DungeonFloor { get; set; } = new HashSet<Vector2>();
    public HashSet<Vector2> DungeonWalls { get; set; } = new HashSet<Vector2>();

    public Vector2 SpawnPoint { get; set; } = new Vector2();

    public GameObject PlayerReference { get; set; }
    public void Reset()
    {
        foreach (Room room in Rooms)
        {
            foreach (var item in room.PropObjectReferences)
            {
                Destroy(item);
            }
            foreach (var item in room.EnemiesInTheRoom)
            {
                Destroy(item);
            }
        }
        Rooms = new();
        Path = new();
        DungeonFloor = new();
        // Destroy(PlayerReference);
    }

    public IEnumerator TutorialCoroutine(Action code)
    {
        yield return new WaitForSeconds(1);
        code();
    }
}


/// <summary>
/// Holds all the data about the room
/// </summary>
public class Room
{
    public Vector2 RoomCenterPos { get; set; }
    public HashSet<Vector2> FloorTiles { get; private set; } = new HashSet<Vector2>();

    public HashSet<Vector2> NearWallTilesUp { get; set; } = new HashSet<Vector2>();
    public HashSet<Vector2> NearWallTilesDown { get; set; } = new HashSet<Vector2>();
    public HashSet<Vector2> NearWallTilesLeft { get; set; } = new HashSet<Vector2>();
    public HashSet<Vector2> NearWallTilesRight { get; set; } = new HashSet<Vector2>();
    public HashSet<Vector2> CornerTiles { get; set; } = new HashSet<Vector2>();

    public HashSet<Vector2> InnerTiles { get; set; } = new HashSet<Vector2>();

    public HashSet<Vector2> PropPositions { get; set; } = new HashSet<Vector2>();
    public List<GameObject> PropObjectReferences { get; set; } = new List<GameObject>();

    public List<Vector2> PositionsAccessibleFromPath { get; set; } = new List<Vector2>();

    public List<GameObject> EnemiesInTheRoom { get; set; } = new List<GameObject>();

    public int RoomDistanceRanking { get; set; } // 1 being the lowest and count of dungeon rooms being the highest

    public Room(Vector2 roomCenterPos, HashSet<Vector2> floorTiles)
    {
        this.RoomCenterPos = roomCenterPos;
        this.FloorTiles = floorTiles;
    }

}
