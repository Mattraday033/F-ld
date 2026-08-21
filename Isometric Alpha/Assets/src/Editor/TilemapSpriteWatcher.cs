using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

#pragma warning disable UDR0001 // Editor-only: no runtime init method is required.
[InitializeOnLoad]
public static class TilemapSpriteWatcher
{
    private const int MaxAdjustmentsPerCallback = 64;

    private const string grassOverlaySpriteName = "Grass Overlay";

    private static readonly Vector3 grassOverlayOffset = new Vector3(0f, .2f, 0f);

    private static bool _isAdjusting;

    static TilemapSpriteWatcher()
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
                Debug.LogWarning($"TilemapSpriteWatcher: batch of {tiles.Length} tiles exceeded safety limit {MaxAdjustmentsPerCallback}; processing first {limit} only.", tilemap);
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

        Vector3 offset = sprite.name == grassOverlaySpriteName ? grassOverlayOffset : Vector3.zero;

        ApplyOffsetIfDifferent(tilemap, position, offset);
    }

    //Only the translation of the cell's transform matrix is the tile's offset, so whatever rotation
    //and scale the cell already carries is kept.
    private static void ApplyOffsetIfDifferent(Tilemap tilemap, Vector3Int position, Vector3 offset)
    {
        Matrix4x4 current = tilemap.GetTransformMatrix(position);
        Matrix4x4 target = Matrix4x4.TRS(offset, current.rotation, current.lossyScale);

        if (current == target) return;

        tilemap.SetTransformMatrix(position, target);
    }
}
