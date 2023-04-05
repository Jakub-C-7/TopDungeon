using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    protected SimpleRandomWalkData randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        tilemapVisualiser.SetRandomTileStyle(); // Get a random style for the dungeon
        HashSet<Vector2> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        tilemapVisualiser.Clear();
        tilemapVisualiser.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualiser);

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
