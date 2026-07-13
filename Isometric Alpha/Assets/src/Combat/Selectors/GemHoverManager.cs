using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GemHoverManager
{
    private static readonly Dictionary<GridCoords, EffectAnimationManager> activeGems =
        new Dictionary<GridCoords, EffectAnimationManager>();

    // Tiles whose gem is scheduled to be destroyed a frame from now. Re-wanting a tile removes it
    // here, which the pending coroutine detects and bails on - so the gem survives without a flicker.
    private static readonly HashSet<GridCoords> pendingDestroys = new HashSet<GridCoords>();

    // Uniform vertical distance every gem sits above its tile's CombatGrid.getPositionAt(). Tunable.
    private const float gemVerticalOffset = 1f;

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        activeGems.Clear();
        pendingDestroys.Clear();

        SelectorManager.SelectorMoved.AddListener(reconcileGems);
        CombatStateManager.OnCombatEnd.AddListener(clearAllGems);
    }

    private static void reconcileGems(List<Selector> visibleSelectors)
    {
        HashSet<GridCoords> desired = computeDesiredTiles(visibleSelectors);

        // Remove gems no longer wanted. Collect keys first to avoid mutating while iterating.
        List<GridCoords> toRemove = new List<GridCoords>();
        foreach(GridCoords tile in activeGems.Keys)
        {
            if(!desired.Contains(tile))
            {
                toRemove.Add(tile);
            }
        }

        foreach(GridCoords tile in toRemove)
        {
            destroyGem(tile);
        }

        // Add gems for newly wanted tiles; leave existing ones untouched (no flicker / no SFX re-trigger).
        foreach(GridCoords tile in desired)
        {
            // A tile wanted again cancels any pending destruction, keeping its existing gem alive.
            pendingDestroys.Remove(tile);

            if(!activeGems.ContainsKey(tile))
            {
                activeGems[tile] = spawnGem(tile);
            }
        }
    }

    private static HashSet<GridCoords> computeDesiredTiles(List<Selector> visibleSelectors)
    {
        HashSet<GridCoords> desired = new HashSet<GridCoords>();

        if(visibleSelectors == null)
        {
            return desired;
        }

        bool choosingLocation = CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation;

        foreach(Selector selector in visibleSelectors)
        {
            foreach(GridCoords coord in selector.getAllSelectorCoords())
            {
                Stats combatant = CombatGrid.getCombatantAtCoords(coord);

                if(combatant == null)
                {
                    continue;
                }

                if(choosingLocation)
                {
                    // Only the targeted tile the selector actually overlaps.
                    desired.Add(coord);
                } else
                {
                    // Every tile this creature occupies.
                    foreach(GridCoords occupied in combatant.positions)
                    {
                        desired.Add(occupied);
                    }
                }
            }
        }

        return desired;
    }

    private static EffectAnimationManager spawnGem(GridCoords tile)
    {
        EffectAnimationManager gem = EffectAnimationManager.instantiatePrefab();

        gem.loops = true;       // re-plays forever, never self-deletes
        gem.damage = 0;         // skips spawnDamageNumbers() (guarded by damage > 0)
        gem.playSFX = false;    // persistent hover indicator - never plays SFX

        gem.setAnimations(EffectAnimationType.Gem);

        // Force every gem onto the same sorting layer.
        gem.spriteRenderer.sortingLayerName = LayerAndTagManager.fourthSortingLayerName;

        gem.transform.position = gemPosition(tile);

        return gem;
    }

    private static Vector3 gemPosition(GridCoords tile)
    {
        // Same vertical distance above each tile's base position (X/Z stay per-tile).
        return CombatGrid.getPositionAt(tile) + new Vector3(0f, gemVerticalOffset, 0f);
    }

    // Defers destruction by a frame so a gem that's immediately re-wanted (see reconcileGems) can
    // cancel it and survive without flickering.
    private static void destroyGem(GridCoords tile)
    {
        if(pendingDestroys.Contains(tile))
        {
            return;     // already scheduled - don't stack coroutines
        }

        CombatStateManager host = CombatStateManager.getInstance();

        if(host == null)
        {
            // No MonoBehaviour to run the coroutine on (e.g. combat already tearing down) - just destroy.
            destroyGemNow(tile);
            return;
        }

        pendingDestroys.Add(tile);
        host.StartCoroutine(destroyGemAfterFrame(tile));
    }

    private static IEnumerator destroyGemAfterFrame(GridCoords tile)
    {
        yield return null;

        // If the tile was wanted again while we waited, its pending-destroy was cleared - bail out.
        if(!pendingDestroys.Remove(tile))
        {
            yield break;
        }

        destroyGemNow(tile);
    }

    private static void destroyGemNow(GridCoords tile)
    {
        if(!activeGems.TryGetValue(tile, out EffectAnimationManager gem))
        {
            return;
        }

        activeGems.Remove(tile);

        if(gem != null)
        {
            // Plain destroy - do NOT call removeAnimation(), which triggers win/loss + dead-combatant checks.
            Object.DestroyImmediate(gem.gameObject);
        }
    }

    private static void clearAllGems()
    {
        // Drop any pending destroys so their coroutines bail instead of touching torn-down state.
        pendingDestroys.Clear();

        foreach(EffectAnimationManager gem in activeGems.Values)
        {
            if(gem != null)
            {
                Object.DestroyImmediate(gem.gameObject);
            }
        }

        activeGems.Clear();
    }
}
