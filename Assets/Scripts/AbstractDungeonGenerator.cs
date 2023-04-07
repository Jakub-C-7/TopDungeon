using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualiser tilemapVisualiser = null;
    [SerializeField]
    protected Vector2 startPosition = Vector2.zero;

    public void GenerateDungeon()
    {
        tilemapVisualiser.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();

    protected abstract void PlaceSpawnPoint(Vector2 position);

}
