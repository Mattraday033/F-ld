using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

//One-shot pass over an already-authored map: every Ground cell painted with the baked-in
//GrassPatch sprite becomes plain dirt, and the grass moves onto the First tilemap as a
//separate overlay tile. TilemapSpriteWatcher keeps newly painted overlays aligned; this
//converter is what retrofits the ones that were painted before the overlay existed.
public static class GrassPatchOverlayConverter
{
    private const string necampPrefabPath = "Assets/Resources/SpriteMaps/CampExteriors/Resources/NECamp Test.prefab";

    private const string groundTilemapName = "Ground";
    private const string overlayTilemapName = "First";

    private const string grassPatchSpriteName = "ISO_Tile_Dirt_01_GrassPatch_01";

    private const string dirtTilePath = "Assets/Resources/Sprites/Tiles/Ground/Dirt/ISO_Tile_Dirt_01.asset";
    private const string grassOverlayTilePath = "Assets/Resources/Sprites/TestSprites/Grass Overlay.asset";

    private static readonly Vector3Int overlayCellOffset = new Vector3Int(1, 1, 0);

    private static readonly Vector3 grassOverlayOffset = new Vector3(0f, .2f, 0f);

    [MenuItem("Tools/Tilemaps/Convert Grass Patches To Overlays (NECamp Test)")]
    private static void convertNECampTest()
    {
        TileBase dirtTile = AssetDatabase.LoadAssetAtPath<TileBase>(dirtTilePath);
        TileBase grassOverlayTile = AssetDatabase.LoadAssetAtPath<TileBase>(grassOverlayTilePath);

        if (dirtTile == null || grassOverlayTile == null)
        {
            Debug.LogError($"GrassPatchOverlayConverter: could not load '{dirtTilePath}' and/or '{grassOverlayTilePath}'.");
            return;
        }

        //Editing the open prefab stage rather than the asset avoids fighting the stage over
        //who wrote the file last; outside prefab mode we load and save the asset directly.
        PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
        bool isOpenInStage = stage != null && stage.assetPath == necampPrefabPath;

        GameObject root = isOpenInStage ? stage.prefabContentsRoot : PrefabUtility.LoadPrefabContents(necampPrefabPath);
        if (root == null)
        {
            Debug.LogError($"GrassPatchOverlayConverter: could not open '{necampPrefabPath}'.");
            return;
        }

        try
        {
            Tilemap ground = findUniqueTilemap(root, groundTilemapName);
            Tilemap overlay = findUniqueTilemap(root, overlayTilemapName);

            if (ground == null || overlay == null) return;

            int converted = convert(ground, overlay, dirtTile, grassOverlayTile);

            if (converted == 0)
            {
                Debug.Log($"GrassPatchOverlayConverter: no convertible '{grassPatchSpriteName}' tiles on '{groundTilemapName}'; nothing changed.");
                return;
            }

            if (isOpenInStage)
            {
                EditorSceneManager.MarkSceneDirty(stage.scene);
            }
            else
            {
                PrefabUtility.SaveAsPrefabAsset(root, necampPrefabPath);
            }

            Debug.Log($"GrassPatchOverlayConverter: converted {converted} '{grassPatchSpriteName}' tile(s) on '{groundTilemapName}' to dirt and placed grass overlays on '{overlayTilemapName}'.");
        }
        finally
        {
            if (!isOpenInStage)
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
        }
    }

    private static int convert(Tilemap ground, Tilemap overlay, TileBase dirtTile, TileBase grassOverlayTile)
    {
        //Collected up front because SetTile on the overlay can grow bounds mid-iteration.
        List<Vector3Int> patches = new List<Vector3Int>();

        foreach (Vector3Int position in ground.cellBounds.allPositionsWithin)
        {
            if (getSprite(ground.GetTile(position))?.name == grassPatchSpriteName)
            {
                patches.Add(position);
            }
        }

        int converted = 0;
        int occupied = 0;

        foreach (Vector3Int position in patches)
        {
            Vector3Int overlayPosition = position + overlayCellOffset;

            //An overlay cell that already renders something is left alone, and so is its Ground
            //partner: the pair is only ever converted together or not at all.
            if (overlay.GetSprite(overlayPosition) != null)
            {
                occupied++;
                continue;
            }

            ground.SetTile(position, dirtTile);

            overlay.SetTile(overlayPosition, grassOverlayTile);
            applyOffset(overlay, overlayPosition, grassOverlayOffset);

            converted++;
        }

        if (occupied > 0)
        {
            Debug.Log($"GrassPatchOverlayConverter: skipped {occupied} of {patches.Count} patch(es) because the '{overlayTilemapName}' cell at +{overlayCellOffset} already had a sprite; their '{groundTilemapName}' tiles were left unchanged.", overlay);
        }

        return converted;
    }

    private static Sprite getSprite(TileBase tileBase)
    {
        return tileBase is Tile tile ? tile.sprite : null;
    }

    //Mirrors TilemapSpriteWatcher: only the translation of the cell's transform matrix is the
    //tile's offset, so whatever rotation and scale the cell already carries is kept.
    private static void applyOffset(Tilemap tilemap, Vector3Int position, Vector3 offset)
    {
        Matrix4x4 current = tilemap.GetTransformMatrix(position);
        Matrix4x4 target = Matrix4x4.TRS(offset, current.rotation, current.lossyScale);

        if (current == target) return;

        tilemap.SetTransformMatrix(position, target);
    }

    private static Tilemap findUniqueTilemap(GameObject root, string gameObjectName)
    {
        Tilemap found = null;

        foreach (Tilemap tilemap in root.GetComponentsInChildren<Tilemap>(true))
        {
            if (tilemap.name != gameObjectName) continue;

            if (found != null)
            {
                Debug.LogError($"GrassPatchOverlayConverter: more than one Tilemap named '{gameObjectName}' under '{root.name}'; aborting rather than guessing.");
                return null;
            }

            found = tilemap;
        }

        if (found == null)
        {
            Debug.LogError($"GrassPatchOverlayConverter: no Tilemap named '{gameObjectName}' under '{root.name}'.");
        }

        return found;
    }
}
