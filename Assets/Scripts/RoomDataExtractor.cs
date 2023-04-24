using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoomDataExtractor : MonoBehaviour
{
    private DungeonData dungeonData;

    [SerializeField]
    private bool showGizmo = false;

    public UnityEvent OnFinishedRoomProcessing;

    [SerializeField]
    private PathFinding pathFinding;

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>();
        pathFinding = new PathFinding();

    }

    public void ProcessRooms()
    {
        if (dungeonData == null)
            return;

        var cardinalDirections = Direction2D.cardinalDirectionsList;

        foreach (Room room in dungeonData.Rooms)
        {
            //find corner, near wall and inner tiles
            foreach (Vector2 tilePosition in room.FloorTiles)
            // foreach (Vector2 tilePosition in dungeonData.DungeonFloor)
            {
                int neighboursCount = 4;

                // if (room.FloorTiles.Contains(CalculateVectors(tilePosition, cardinalDirections[0])) == false)
                if (dungeonData.DungeonFloor.Contains(CalculateVectors(tilePosition, cardinalDirections[0])) == false)
                {
                    // Debug.Log("Adding near wall tile up");
                    room.NearWallTilesUp.Add(tilePosition);
                    neighboursCount--;
                }
                // if (room.FloorTiles.Contains(CalculateVectors(tilePosition, cardinalDirections[2])) == false)
                if (dungeonData.DungeonFloor.Contains(CalculateVectors(tilePosition, cardinalDirections[2])) == false)
                {
                    // Debug.Log("Adding near wall tile down");
                    room.NearWallTilesDown.Add(tilePosition);
                    neighboursCount--;

                }
                // if (room.FloorTiles.Contains(CalculateVectors(tilePosition, cardinalDirections[1])) == false)
                if (dungeonData.DungeonFloor.Contains(CalculateVectors(tilePosition, cardinalDirections[1])) == false)
                {
                    // Debug.Log("Adding near wall tile right");
                    room.NearWallTilesRight.Add(tilePosition);
                    neighboursCount--;

                }
                // if (room.FloorTiles.Contains(CalculateVectors(tilePosition, cardinalDirections[3])) == false)
                if (dungeonData.DungeonFloor.Contains(CalculateVectors(tilePosition, cardinalDirections[3])) == false)
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

            room.NearWallTilesUp.ExceptWith(dungeonData.Path);
            room.NearWallTilesDown.ExceptWith(dungeonData.Path);
            room.NearWallTilesLeft.ExceptWith(dungeonData.Path);
            room.NearWallTilesRight.ExceptWith(dungeonData.Path);
            room.CornerTiles.ExceptWith(dungeonData.Path);
            room.InnerTiles.ExceptWith(dungeonData.Path);

        }

        CalculateRoomDistances();

        OnFinishedRoomProcessing?.Invoke();
        // Invoke("RunEvent", 1);
    }

    public Dictionary<Vector2, int> CalculateRoomDistances()
    {
        Dictionary<Vector2, int> roomDistances = new();

        foreach (var room in dungeonData.Rooms)
        {

            if (dungeonData.SpawnPoint == room.RoomCenterPos)
            {
                continue;
            }

            List<PathNode> pathNodes = pathFinding.FindPath(dungeonData.SpawnPoint.x, dungeonData.SpawnPoint.y, room.RoomCenterPos.x, room.RoomCenterPos.y, dungeonData.DungeonFloor);

            roomDistances.Add(room.RoomCenterPos, pathNodes[pathNodes.Count - 1].gCost);

        }

        // Populate room distance ranking variables for each room
        foreach (var room in dungeonData.Rooms)
        {
            var roomDistance = roomDistances.FirstOrDefault(x => x.Key == room.RoomCenterPos);
            room.RoomDistanceRanking = roomDistances.OrderBy(i => i.Value).ToList().IndexOf(roomDistance) + 1;
        }

        return roomDistances;

    }

    public Vector2 CalculateVectors(Vector2 a, Vector2 b)
    {
        return new Vector2(Mathf.RoundToInt((a.x * 100) + (b.x * 100)) / 100f, Mathf.RoundToInt((a.y * 100) + (b.y * 100)) / 100f);
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
