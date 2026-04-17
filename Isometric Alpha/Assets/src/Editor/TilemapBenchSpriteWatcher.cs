using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

#pragma warning disable UDR0001 // Editor-only: no runtime init method is required.
[InitializeOnLoad]
public static class TilemapBenchSpriteWatcher
{
    private const int MaxAdjustmentsPerCallback = 64;

    private static bool _isAdjusting;

    static TilemapBenchSpriteWatcher()
    {
        Tilemap.tilemapTileChanged -= OnTilemapTileChanged;
        Tilemap.tilemapTileChanged += OnTilemapTileChanged;
    }

    private static void OnTilemapTileChanged(Tilemap tilemap, Tilemap.SyncTile[] tiles)
    {
        if (_isAdjusting) return;
        if (tilemap == null || tiles == null) return;

        _isAdjusting = true;
        try
        {
            int limit = Mathf.Min(tiles.Length, MaxAdjustmentsPerCallback);
            if (tiles.Length > MaxAdjustmentsPerCallback)
            {
                Debug.LogWarning($"TilemapBenchSpriteWatcher: batch of {tiles.Length} tiles exceeded safety limit {MaxAdjustmentsPerCallback}; processing first {limit} only.", tilemap);
            }

            for (int i = 0; i < limit; i++)
            {
                TileBase tileBase = tiles[i].tile;
                if (tileBase == null)
                {
                    tileBase = tilemap.GetTile(tiles[i].position);
                }
                makeAdjustmentBasedOnName(tileBase, tilemap, tiles[i].position);
            }
        }
        finally
        {
            _isAdjusting = false;
        }
    }

    private static void makeAdjustmentBasedOnName(TileBase tileBase, Tilemap tilemap, Vector3Int position)
    {
        Sprite sprite = null;

        if (tileBase is Tile tile)
        {
            sprite = tile.sprite;
        }

        if (sprite == null)
        {
            return;
        }

        switch (sprite.name)
        {
            case "Bench_Front":
            case "Bench_Back":
                ApplyIfDifferent(tilemap, position, Matrix4x4.TRS(new Vector3(0f, 0.08f, 0f), Quaternion.identity, new Vector3(0.96f, 1f, 1f)));
                return;
            case "LeafPile TestTile":
                ApplyIfDifferent(tilemap, position, Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(0.5f, .5f, 1f)));
                return;
            default:
                return;
        }
    }

    private static void ApplyIfDifferent(Tilemap tilemap, Vector3Int position, Matrix4x4 target)
    {
        Matrix4x4 current = tilemap.GetTransformMatrix(position);
        if (current == target) return;
        tilemap.SetTransformMatrix(position, target);
    }
}
