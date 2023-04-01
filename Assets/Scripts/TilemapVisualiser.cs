using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualiser : MonoBehaviour
{
    [SerializeField]
    public Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    public TileBase floorAndWallTile;

    // Create array here to select random floor tile

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorAndWallTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);

        }

    }

    public void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void RemoveSingleTile(Tilemap tilemap, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, null);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleBasicWallToFloor(Vector2Int position)
    {
        PaintSingleTile(floorTilemap, floorAndWallTile, position);

    }

    internal void PaintSingleBasicWallToWall(Vector2Int position)
    {
        PaintSingleTile(wallTilemap, floorAndWallTile, position);

    }

}
