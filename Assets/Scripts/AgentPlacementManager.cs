using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentPlacementManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab, bossPrefab, playerPrefab;

    [SerializeField]
    private List<GameObject> enemyList;

    [SerializeField]
    private List<GameObject> bossList;

    [SerializeField]
    private int playerRoomIndex;

    // [SerializeField]
    // private List<int> roomEnemiesCount;

    DungeonData dungeonData;

    [SerializeField]
    private bool showGizmo = false;

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>();
        playerPrefab = FindObjectOfType<Player>().gameObject;

    }

    public void PlaceAgents()
    {
        if (dungeonData == null)
            return;

        // If there is only one room, fill it with actors, else, skip it and let it be a safe room
        int count = 0;
        if (dungeonData.Rooms.Count > 1)
        {
            count = 1;
        }

        //Loop for each room
        for (int i = 0 + count; i < dungeonData.Rooms.Count; i++)
        {
            count = 0;

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

            // --- Placing enemies ---

            // Max number of enemies to place in the room
            int maxEnemyCount = room.PositionsAccessibleFromPath.Count;

            //If the room is the second furthest away from spawn, make it a boss room
            if (room.RoomDistanceRanking == dungeonData.Rooms.Count - 2)
            {
                // Place Boss enemy randomly from the available boss list
                PlaceEnemies(room, 1, PickRandomGameObject(bossList));
            }
            else if (maxEnemyCount > 0) // Normal enemy room
            {
                int enemyCount = UnityEngine.Random.Range(1, 10);

                //If enemy count is greater than the max number of enemies we can place in the room, cap it at that number
                if (enemyCount > maxEnemyCount)
                {
                    enemyCount = maxEnemyCount;
                }

                // PlaceEnemies(room, enemyCount, enemyPrefab);
                PlaceEnemies(room, enemyCount, PickRandomGameObject(enemyList)); // Place a random enemy type in each room

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
    private void PlaceEnemies(Room room, int enemyCount, GameObject enemyToPlace)
    {
        for (int k = 0; k < enemyCount; k++)
        {
            // If we want to place more enemies than there is space for in the room, return
            if (room.PositionsAccessibleFromPath.Count <= k)
            {
                return;
            }
            GameObject enemy = Instantiate(enemyToPlace);
            enemy.transform.localPosition = (Vector2)room.PositionsAccessibleFromPath[k] + new Vector2(0.16f, 0.16f) * 0.5f;
            room.EnemiesInTheRoom.Add(enemy);
        }
    }

    // Picks a random GameObject from the List of GameObjects and returns it
    private GameObject PickRandomGameObject(List<GameObject> gameObjects)
    {
        return gameObjects[UnityEngine.Random.Range(0, gameObjects.Count)];
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
                Vector2 newPos = new Vector2(((Mathf.RoundToInt(pos.x * 100) + Mathf.RoundToInt(direction.x * 100)) / 100f), ((Mathf.RoundToInt(pos.y * 100) + Mathf.RoundToInt(direction.y * 100)) / 100f));

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
