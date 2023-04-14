using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentPlacementManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab, playerPrefab;

    [SerializeField]
    private int playerRoomIndex;

    // [SerializeField]
    // private CinemachineVirtualCamera vCamera;

    [SerializeField]
    private List<int> roomEnemiesCount;

    DungeonData dungeonData;

    [SerializeField]
    private bool showGizmo = false;

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>();
    }

    public void PlaceAgents()
    {
        if (dungeonData == null)
            return;

        //Loop for each room
        for (int i = 0; i < dungeonData.Rooms.Count; i++)
        {
            //To place enemies we need to analyse the room tiles to find those accesible from the path
            Room room = dungeonData.Rooms[i];
            RoomGraph roomGraph = new RoomGraph(room.FloorTiles);

            //Find the Path inside this specific room
            HashSet<Vector2> roomFloor = new HashSet<Vector2>(room.FloorTiles);
            //Find the tiles belonging to both the path and the room
            // roomFloor.IntersectWith(dungeonData.Path);
            roomFloor.UnionWith(dungeonData.Path);

            //Run the BFS to find all the tiles in the room accessible from the path
            Dictionary<Vector2, Vector2> roomMap = roomGraph.RunBFS(roomFloor.First(), room.PropPositions);

            //Positions that we can reach + path == positions where we can place enemies
            room.PositionsAccessibleFromPath = roomMap.Keys.OrderBy(x => Guid.NewGuid()).ToList();

            // If the current room's enemy count has been specified
            if (roomEnemiesCount.Count > i)
            {
                PlaceEnemies(room, roomEnemiesCount[i]); // Place
            }

            // Spawn the player / Camera - Optional
            // if (i == playerRoomIndex)
            // {
            //     GameObject player = Instantiate(playerPrefab);
            // player.transform.localPosition = dungeonData.Rooms[i].RoomCenterPos + new Vector2(0.16f, 0.16f) * 0.5f;
            //     //Make the camera follow the player
            //     // vCamera.Follow = player.transform;
            //     // vCamera.LookAt = player.transform;
            //     dungeonData.PlayerReference = player;
            // }
        }
    }

    /// <summary>
    /// Places enemies in the positions accessible from the path
    /// </summary>
    /// <param name="room"></param>
    /// <param name="enemyCount"></param>
    private void PlaceEnemies(Room room, int enemyCount)
    {
        for (int k = 0; k < enemyCount; k++)
        {
            // If we want to place more enemies than there is space for in the room, return
            if (room.PositionsAccessibleFromPath.Count <= k)
            {
                return;
            }
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.localPosition = (Vector2)room.PositionsAccessibleFromPath[k] + new Vector2(0.16f, 0.16f) * 0.5f;
            room.EnemiesInTheRoom.Add(enemy);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (dungeonData == null || showGizmo == false)
            return;
        foreach (Room room in dungeonData.Rooms)
        {
            Color color = Color.green;
            color.a = 0.3f;
            Gizmos.color = color;

            foreach (Vector2 pos in room.PositionsAccessibleFromPath)
            {
                Gizmos.DrawCube((Vector2)pos + new Vector2(0.16f, 0.16f) * 0.5f, new Vector2(0.16f, 0.16f));
            }
        }
    }
}

public class RoomGraph
{

    Dictionary<Vector2, List<Vector2>> graph = new Dictionary<Vector2, List<Vector2>>();

    public RoomGraph(HashSet<Vector2> roomFloor)
    {
        foreach (Vector2 pos in roomFloor)
        {
            List<Vector2> neighbours = new List<Vector2>();
            foreach (Vector2 direction in Direction2D.cardinalDirectionsList)
            {
                Vector2 newPos = pos + direction;
                if (roomFloor.Contains(newPos))
                {
                    neighbours.Add(newPos);
                }
            }
            graph.Add(pos, neighbours);
        }
    }

    /// <summary>
    /// Creates a map of reachable tiles in our dungeon.
    /// </summary>
    /// <param name="startPos">Door position or tile position on the path between rooms inside this room</param>
    /// <param name="occupiedNodes"></param>
    /// <returns></returns>
    public Dictionary<Vector2, Vector2> RunBFS(Vector2 startPos, HashSet<Vector2> occupiedNodes)
    {
        //BFS related variables
        Queue<Vector2> nodesToVisit = new Queue<Vector2>();
        nodesToVisit.Enqueue(startPos);

        HashSet<Vector2> visitedNodes = new HashSet<Vector2>();
        visitedNodes.Add(startPos);

        //The dictionary that we will return 
        Dictionary<Vector2, Vector2> map = new Dictionary<Vector2, Vector2>();
        map.Add(startPos, startPos);

        while (nodesToVisit.Count > 0)
        {
            //get the data about specific position
            Vector2 node = nodesToVisit.Dequeue();
            List<Vector2> neighbours = graph[node];

            //loop through each neighbour position
            foreach (Vector2 neighbourPosition in neighbours)
            {
                //add the neighbour position to our map if it is valid
                if (visitedNodes.Contains(neighbourPosition) == false &&
                    occupiedNodes.Contains(neighbourPosition) == false)
                {
                    nodesToVisit.Enqueue(neighbourPosition);
                    visitedNodes.Add(neighbourPosition);
                    map[neighbourPosition] = node;
                }
            }
        }

        return map;
    }
}
