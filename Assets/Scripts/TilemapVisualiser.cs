using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapVisualiser : MonoBehaviour
{
    [SerializeField]
    public Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    public List<TileBase> ruleTileList;
    public TileBase currentlySelectedTile;

    public void PaintFloorTiles(IEnumerable<Vector2> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, currentlySelectedTile);
    }

    public void SetRandomTileStyle()
    {
        int randomSelection = Random.Range(1, ruleTileList.Count + 1);

        currentlySelectedTile = ruleTileList[randomSelection - 1];
    }

    private void PaintTiles(IEnumerable<Vector2> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);

        }

    }

    public void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2 position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void RemoveSingleTile(Tilemap tilemap, Vector2 position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3)position);
        tilemap.SetTile(tilePosition, null);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleBasicWallToFloor(Vector2 position)
    {
        PaintSingleTile(floorTilemap, currentlySelectedTile, position);

    }

    internal void PaintSingleBasicWallToWall(Vector2 position)
    {
        PaintSingleTile(wallTilemap, currentlySelectedTile, position);

    }

}
