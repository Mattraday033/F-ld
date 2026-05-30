using System.Collections.Generic;
using UnityEngine;

public static class GemHoverManager
{
    private static readonly Dictionary<GridCoords, EffectAnimationManager> activeGems =
        new Dictionary<GridCoords, EffectAnimationManager>();

    // Uniform vertical distance every gem sits above its tile's CombatGrid.getPositionAt(). Tunable.
    private const float gemVerticalOffset = 1f;

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        activeGems.Clear();

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

    private static void destroyGem(GridCoords tile)
    {
        EffectAnimationManager gem = activeGems[tile];
        activeGems.Remove(tile);

        if(gem != null)
        {
            // Plain destroy - do NOT call removeAnimation(), which triggers win/loss + dead-combatant checks.
            Object.DestroyImmediate(gem.gameObject);
        }
    }

    private static void clearAllGems()
    {
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
