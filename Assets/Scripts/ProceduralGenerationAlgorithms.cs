using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{

    public static HashSet<Vector2> SimpleRandomWalk(Vector2 startposition, int walkLength)
    {
        HashSet<Vector2> path = new HashSet<Vector2>();

        path.Add(startposition);
        var previousPosition = startposition;

        for (int i = 0; i < walkLength; i++)
        {
            var cardDir = Direction2D.GetRandomCardinalDirection();
            var newPosition = new Vector2((Mathf.RoundToInt(previousPosition.x * 100) + Mathf.RoundToInt(cardDir.x * 100)) / 100f, (Mathf.RoundToInt(previousPosition.y * 100) + Mathf.RoundToInt(cardDir.y * 100)) / 100f);

            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }

    public static List<Vector2> RandomWalkCorridor(Vector2 startPosition, int corridorLength)
    {
        List<Vector2> corridor = new List<Vector2>();
        var direction = Direction2D.GetRandomCardinalDirection(); // Get a random direction to start 
        var currentPosition = startPosition;
        corridor.Add(currentPosition); // Add starting pos

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition = new Vector2((Mathf.RoundToInt(currentPosition.x * 100) + Mathf.RoundToInt(direction.x * 100)) / 100f, (Mathf.RoundToInt(currentPosition.y * 100) + Mathf.RoundToInt(direction.y * 100)) / 100f);

            corridor.Add(currentPosition);
        }
        return corridor;

    }

    public static List<Bounds> BinarySpacePartitioning(Bounds spaceToSplit, float minWidth, float minHeight)
    {
        Queue<Bounds> roomsQueue = new Queue<Bounds>();
        List<Bounds> roomsList = new List<Bounds>();

        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();

            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);

                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);

                    }
                    else
                    {
                        roomsList.Add(room);

                    }

                }
                else
                {

                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);

                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);

                    }
                    else
                    {
                        roomsList.Add(room);

                    }

                }
            }
        }

        return roomsList;
    }

    public static Vector3 CalculateDivisibleBy(Vector3 makeDivisible, float divisibleBy)
    {
        return new Vector3(Mathf.RoundToInt(makeDivisible.x / divisibleBy) * divisibleBy, Mathf.RoundToInt(makeDivisible.y / divisibleBy) * divisibleBy, 0);
    }

    private static void SplitVertically(float minWidth, Queue<Bounds> roomsQueue, Bounds room)
    {
        var xSplit = Random.Range(0.16f, room.size.x);

        // Bounds adapted to find the center as the first parameter instead of position in BoundsInt
        Bounds room1 = new Bounds(CalculateDivisibleBy(new Vector3(room.min.x + (xSplit / 2), room.min.y + room.size.y / 2, 0), 0.16f), new Vector3(xSplit, room.size.y, room.size.z));
        Bounds room2 = new Bounds(CalculateDivisibleBy(new Vector3(room.min.x + xSplit + (room.size.x - xSplit) / 2, room.min.y + room.size.y / 2, 0), 0.16f), new Vector3(room.size.x - xSplit, room.size.y, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);

    }

    private static void SplitHorizontally(float minHeight, Queue<Bounds> roomsQueue, Bounds room)
    {
        var ySplit = Random.Range(0.16f, room.size.y); // (minHeight, room.size.y - minheight) - for a grid like structure

        // Bounds adapted to find the center as the first parameter instead of position in BoundsInt
        Bounds room1 = new Bounds(CalculateDivisibleBy(new Vector3(room.min.x + (room.size.x / 2), room.min.y + ySplit / 2, 0), 0.16f), new Vector3(room.size.x, ySplit, room.size.z));
        Bounds room2 = new Bounds(CalculateDivisibleBy(new Vector3(room.min.x + room.size.x / 2, room.min.y + ySplit + (room.size.y - ySplit) / 2, room.min.z), 0.16f), new Vector3(room.size.x, room.size.y - ySplit, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);

    }
}

public static class Direction2D
{

    public static List<Vector2> cardinalDirectionsList = new List<Vector2>{
        new Vector2(0, 0.16f), // Up
        new Vector2(0.16f, 0), // Right
        new Vector2(0, -0.16f), // Down
        new Vector2(-0.16f, 0) // Left

    };

    public static List<Vector2> eightDirectionsList = new List<Vector2>{
        new Vector2(0, 0.16f), // Up
        new Vector2(0.16f, 0.16f), // Up-Right
        new Vector2(0.16f, 0), // Right
        new Vector2(0.16f, -0.16f), // Right-Down
        new Vector2(0, -0.16f), // Down
        new Vector2(-0.16f, -0.16f), // Left Down
        new Vector2(-0.16f, 0), // Left
        new Vector2(-0.16f, 0.16f) // Left-Up
        
    };

    public static Vector2 GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }

}
