using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;


public enum TerrainHiddenState { None, InFrontOfTerrain, BehindTerrain, TerrainHidden}

public static class TerrainVisibilityManager
{
    public static TerrainHiddenState currentTerrainHiddenState;

    public static List<TilemapRenderer> shownWhileTerrainHiddenTilemaps;
    public static List<TilemapRenderer> terrainTilemaps;
    public static List<SpriteRenderer> terrainSprites;


    [RuntimeInitializeOnLoadMethod]
    private static void initializeTerrainVisibilityManager()
    {
        currentTerrainHiddenState = TerrainHiddenState.None;

        shownWhileTerrainHiddenTilemaps = new List<TilemapRenderer>();
        terrainTilemaps = new List<TilemapRenderer>();
        terrainSprites = new List<SpriteRenderer>();

        TransitionManager.AfterTransition.AddListener(initializeOnTransition);
        TransitionManager.AfterTransition.AddListener(waitFrameAndCheckForTerrainOnTransition);
        FadeToBlackManager.OnFadeBackInFinished.AddListener(waitFrameAndCheckForTerrainOnTransition);
        MovementManager.OnMoveFinished.AddListener(changeTerrainStateOnTerrainCollision);
    }

    private static void waitFrameAndCheckForTerrainOnTransition()
    {
        if(PlayerMovement.getInstance() == null)
        {
            return;
        }

        PlayerMovement.getInstance().StartCoroutine(waitFrameAndCheckForTerrain());
    }

    private static IEnumerator waitFrameAndCheckForTerrain()
    {
        yield return null;
        changeTerrainStateOnTerrainCollision(0);
    }

    public static void initializeOnTransition()
    {
        findTerrainObjects();
        
        adjustTerrainVisibilityToMatchState();

        changeTerrainStateOnTerrainCollision(0);
    }

    private static void adjustTerrainVisibilityToMatchState()
    {
        switch(currentTerrainHiddenState)
        {
            case TerrainHiddenState.InFrontOfTerrain:

                for (int i = 0; i < terrainTilemaps.Count; i++)
                {
                    terrainTilemaps[i].enabled = true;
                    terrainTilemaps[i].maskInteraction = SpriteMaskInteraction.None;
                }

                for (int i = 0; i < terrainSprites.Count; i++)
                {
                     terrainSprites[i].enabled = true;
                    terrainSprites[i].maskInteraction = SpriteMaskInteraction.None;
                }

                foreach (TilemapRenderer tilemap in shownWhileTerrainHiddenTilemaps)
                {
                    tilemap.enabled = false;
                }

                break;
            case TerrainHiddenState.BehindTerrain:
                    for (int i = 0; i < terrainTilemaps.Count; i++)
                    {
                        terrainTilemaps[i].enabled = true;
                        terrainTilemaps[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    }

                    for (int i = 0; i < terrainSprites.Count; i++)
                    {
                         terrainSprites[i].enabled = true;
                        terrainSprites[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    }

                    foreach (TilemapRenderer tilemap in shownWhileTerrainHiddenTilemaps)
                    {
                        tilemap.enabled = true;
                        tilemap.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    }
                break;
            case TerrainHiddenState.TerrainHidden:
                    for (int i = 0; i < terrainTilemaps.Count; i++)
                    {
                        terrainTilemaps[i].enabled = false;
                    }

                    for (int i = 0; i < terrainSprites.Count; i++)
                    {
                        terrainSprites[i].enabled = false;
                    }

                    foreach (TilemapRenderer tilemap in shownWhileTerrainHiddenTilemaps)
                    {
                        tilemap.enabled = true;
                        tilemap.maskInteraction = SpriteMaskInteraction.None;
                    }
                break;
        }
    }

    public static void changeTerrainStateOnTerrainCollision(int i)
    {
        bool needToChangeTerrainHiddenState = false;

        switch(currentTerrainHiddenState)
        {
            case TerrainHiddenState.InFrontOfTerrain:
            case TerrainHiddenState.BehindTerrain:
            case TerrainHiddenState.None:
                if(PlayerObject.isBehindTerrain())
                {
                    if(currentTerrainHiddenState != TerrainHiddenState.BehindTerrain)
                    {
                        currentTerrainHiddenState = TerrainHiddenState.BehindTerrain;
                        needToChangeTerrainHiddenState = true;
                    }
                    
                } else
                {
                    if(currentTerrainHiddenState != TerrainHiddenState.InFrontOfTerrain)
                    {
                        currentTerrainHiddenState = TerrainHiddenState.InFrontOfTerrain;
                        needToChangeTerrainHiddenState = true;
                    }
                }

                break;
            default:
                needToChangeTerrainHiddenState = false;
                break;
        }

        if(needToChangeTerrainHiddenState)
        {
            adjustTerrainVisibilityToMatchState();
        }
    }

    private static void findTerrainObjects()
    {
        GameObject[] terrainObjects = GameObject.FindGameObjectsWithTag(LayerAndTagManager.terrainTag);
        GameObject[] shownWhileTerrainHiddenObjects = GameObject.FindGameObjectsWithTag(LayerAndTagManager.shownWhileTerrainHiddenTag);

        shownWhileTerrainHiddenTilemaps = new List<TilemapRenderer>();
        terrainTilemaps = new List<TilemapRenderer>();
        terrainSprites = new List<SpriteRenderer>();

        foreach (GameObject terrainObject in terrainObjects)
        {
            TilemapRenderer terrainTilemap = terrainObject.GetComponent<TilemapRenderer>();
            SpriteRenderer terrainSprite = terrainObject.GetComponent<SpriteRenderer>();

            if (terrainTilemap != null && !(terrainTilemap is null))
            {
                terrainTilemaps.Add(terrainTilemap);
            }

            if (terrainSprite != null && !(terrainSprite is null))
            {
                terrainSprites.Add(terrainSprite);
            }
        }

        foreach (GameObject shownWhileTerrainHiddenObject in shownWhileTerrainHiddenObjects)
        {
            TilemapRenderer shownWhileTerrainHiddenTilemap = shownWhileTerrainHiddenObject.GetComponent<TilemapRenderer>();
            // SpriteRenderer terrainSprite = terrainObject.GetComponent<SpriteRenderer>();

            if (shownWhileTerrainHiddenTilemap != null && !(shownWhileTerrainHiddenTilemap is null))
            {
                shownWhileTerrainHiddenTilemaps.Add(shownWhileTerrainHiddenTilemap);
            }
        }
    }

    public static void toggleTerrainVisibility()
    {
        if(currentTerrainHiddenState != TerrainHiddenState.TerrainHidden)
        {
            currentTerrainHiddenState = TerrainHiddenState.TerrainHidden;
            State.terrainHidden = true;
            adjustTerrainVisibilityToMatchState();
        } else
        {
            currentTerrainHiddenState = TerrainHiddenState.None;
            State.terrainHidden = false;
            changeTerrainStateOnTerrainCollision(0);
        }
    }

}
