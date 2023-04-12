using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomDataExtractor : MonoBehaviour
{
    private DungeonData dungeonData;

    [SerializeField]
    private bool showGizmo = false;

    public UnityEvent OnFinishedRoomProcessing;

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>();
    }

    // public void ProcessRooms()
    // {
    //     if (dungeonData == null)
    //         return;

    //     var cardinalDirections = Direction2D.cardinalDirectionsList;

    //     HashSet<Vector2> wallList = new();

    //     foreach (Room room in dungeonData.Rooms)
    //     {
    //         //find corner, near wall and inner tiles
    //         foreach (Vector2 tilePosition in room.FloorTiles)
    //         {
    //             int neighboursCount = 4;

    //             if (room.FloorTiles.Contains(tilePosition + cardinalDirections[0]) == false)
    //             {
    //                 // Debug.Log("Adding near wall tile up");
    //                 room.NearWallTilesUp.Add(tilePosition - cardinalDirections[0]);
    //                 neighboursCount--;
    //                 wallList.Add(tilePosition);
    //             }
    //             if (room.FloorTiles.Contains(tilePosition + cardinalDirections[2]) == false)
    //             {
    //                 // Debug.Log("Adding near wall tile down");
    //                 room.NearWallTilesDown.Add(tilePosition - cardinalDirections[2]);
    //                 neighboursCount--;
    //                 wallList.Add(tilePosition);

    //             }
    //             if (room.FloorTiles.Contains(tilePosition + cardinalDirections[1]) == false)
    //             {
    //                 // Debug.Log("Adding near wall tile right");
    //                 room.NearWallTilesRight.Add(tilePosition - cardinalDirections[1]);
    //                 neighboursCount--;
    //                 wallList.Add(tilePosition);

    //             }
    //             if (room.FloorTiles.Contains(tilePosition + cardinalDirections[3]) == false)
    //             {
    //                 // Debug.Log("Adding near wall tile left");
    //                 room.NearWallTilesLeft.Add(tilePosition - cardinalDirections[3]);
    //                 neighboursCount--;
    //                 wallList.Add(tilePosition);

    //             }

    //             //find corners
    //             // if (neighboursCount <= 2)
    //             // {
    //             //     // Debug.Log("Adding corner tile");
    //             //     room.CornerTiles.Add(tilePosition);
    //             // }

    //             if (neighboursCount == 4)
    //                 room.InnerTiles.Add(tilePosition);
    //         }

    //         HashSet<Vector2> wallTilesUpLeft = new HashSet<Vector2>(room.NearWallTilesUp);
    //         wallTilesUpLeft.IntersectWith(room.NearWallTilesLeft);

    //         HashSet<Vector2> wallTilesUpRight = new HashSet<Vector2>(room.NearWallTilesUp);
    //         wallTilesUpRight.IntersectWith(room.NearWallTilesRight);

    //         HashSet<Vector2> wallTilesDownLeft = new HashSet<Vector2>(room.NearWallTilesDown);
    //         wallTilesDownLeft.IntersectWith(room.NearWallTilesLeft);

    //         HashSet<Vector2> wallTilesDownRight = new HashSet<Vector2>(room.NearWallTilesDown);
    //         wallTilesDownRight.IntersectWith(room.NearWallTilesRight);

    //         wallTilesUpLeft.UnionWith(wallTilesUpRight);
    //         wallTilesUpLeft.UnionWith(wallTilesDownLeft);
    //         wallTilesUpLeft.UnionWith(wallTilesDownRight);

    //         room.CornerTiles.UnionWith(wallTilesUpLeft);

    //         room.NearWallTilesUp.ExceptWith(room.CornerTiles);
    //         room.NearWallTilesDown.ExceptWith(room.CornerTiles);
    //         room.NearWallTilesLeft.ExceptWith(room.CornerTiles);
    //         room.NearWallTilesRight.ExceptWith(room.CornerTiles);

    //         // Except the known wall tiles
    //         room.NearWallTilesUp.ExceptWith(wallList);
    //         room.NearWallTilesDown.ExceptWith(wallList);
    //         room.NearWallTilesLeft.ExceptWith(wallList);
    //         room.NearWallTilesRight.ExceptWith(wallList);
    //         room.CornerTiles.ExceptWith(wallList);

    //         Debug.Log("nearwalltilesup: " + room.NearWallTilesUp.Count);
    //         Debug.Log("nearwalltilesdown: " + room.NearWallTilesDown.Count);
    //         Debug.Log("nearwalltilesleft: " + room.NearWallTilesLeft.Count);
    //         Debug.Log("nearwalltilesright: " + room.NearWallTilesRight.Count);
    //         Debug.Log("corners: " + room.CornerTiles.Count);
    //         Debug.Log("inner: " + room.InnerTiles.Count);

    //     }

    //     //OnFinishedRoomProcessing?.Invoke();
    //     Invoke("RunEvent", 1);
    // }

    public void ProcessRooms()
    {
        if (dungeonData == null)
            return;

        var cardinalDirections = Direction2D.cardinalDirectionsList;

        foreach (Room room in dungeonData.Rooms)
        {
            //find corner, near wall and inner tiles
            foreach (Vector2 tilePosition in room.FloorTiles)
            {
                int neighboursCount = 4;

                if (room.FloorTiles.Contains(tilePosition + cardinalDirections[0]) == false)
                {
                    // Debug.Log("Adding near wall tile up");
                    room.NearWallTilesUp.Add(tilePosition);
                    neighboursCount--;
                }
                if (room.FloorTiles.Contains(tilePosition + cardinalDirections[2]) == false)
                {
                    // Debug.Log("Adding near wall tile down");
                    room.NearWallTilesDown.Add(tilePosition);
                    neighboursCount--;

                }
                if (room.FloorTiles.Contains(tilePosition + cardinalDirections[1]) == false)
                {
                    // Debug.Log("Adding near wall tile right");
                    room.NearWallTilesRight.Add(tilePosition);
                    neighboursCount--;

                }
                if (room.FloorTiles.Contains(tilePosition + cardinalDirections[3]) == false)
                {
                    // Debug.Log("Adding near wall tile left");
                    room.NearWallTilesLeft.Add(tilePosition);
                    neighboursCount--;

                }

                //find corners
                if (neighboursCount <= 2)
                {
                    // Debug.Log("Adding corner tile");
                    room.CornerTiles.Add(tilePosition);
                }

                if (neighboursCount == 4)
                    room.InnerTiles.Add(tilePosition);
            }

            room.NearWallTilesUp.ExceptWith(room.CornerTiles);
            room.NearWallTilesDown.ExceptWith(room.CornerTiles);
            room.NearWallTilesLeft.ExceptWith(room.CornerTiles);
            room.NearWallTilesRight.ExceptWith(room.CornerTiles);

            Debug.Log("nearwalltilesup: " + room.NearWallTilesUp.Count);
            Debug.Log("nearwalltilesdown: " + room.NearWallTilesDown.Count);
            Debug.Log("nearwalltilesleft: " + room.NearWallTilesLeft.Count);
            Debug.Log("nearwalltilesright: " + room.NearWallTilesRight.Count);
            Debug.Log("corners: " + room.CornerTiles.Count);
            Debug.Log("inner: " + room.InnerTiles.Count);

        }

        //OnFinishedRoomProcessing?.Invoke();
        Invoke("RunEvent", 1);
    }

    public void RunEvent()
    {
        OnFinishedRoomProcessing?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (dungeonData == null || showGizmo == false)
            return;
        foreach (Room room in dungeonData.Rooms)
        {
            //Draw inner tiles
            Gizmos.color = Color.yellow;
            foreach (Vector2 floorPosition in room.InnerTiles)
            {
                if (dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube(floorPosition + new Vector2(0.16f, 0.16f) * 0.5f, new Vector2(0.16f, 0.16f));
            }
            //Draw near wall tiles UP
            Gizmos.color = Color.blue;
            foreach (Vector2 floorPosition in room.NearWallTilesUp)
            {
                if (dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube(floorPosition + new Vector2(0.16f, 0.16f) * 0.5f, new Vector2(0.16f, 0.16f));
            }
            //Draw near wall tiles DOWN
            Gizmos.color = Color.green;
            foreach (Vector2 floorPosition in room.NearWallTilesDown)
            {
                if (dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube(floorPosition + new Vector2(0.16f, 0.16f) * 0.5f, new Vector2(0.16f, 0.16f));
            }
            //Draw near wall tiles RIGHT
            Gizmos.color = Color.white;
            foreach (Vector2 floorPosition in room.NearWallTilesRight)
            {
                if (dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube(floorPosition + new Vector2(0.16f, 0.16f) * 0.5f, new Vector2(0.16f, 0.16f));
            }
            //Draw near wall tiles LEFT
            Gizmos.color = Color.cyan;
            foreach (Vector2 floorPosition in room.NearWallTilesLeft)
            {
                if (dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube(floorPosition + new Vector2(0.16f, 0.16f) * 0.5f, new Vector2(0.16f, 0.16f));
            }
            //Draw near wall tiles CORNERS
            Gizmos.color = Color.magenta;
            foreach (Vector2 floorPosition in room.CornerTiles)
            {
                if (dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube(floorPosition + new Vector2(0.16f, 0.16f) * 0.5f, new Vector2(0.16f, 0.16f));
            }
        }
    }
}
