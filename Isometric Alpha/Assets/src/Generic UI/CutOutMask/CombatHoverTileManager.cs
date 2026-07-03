using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class CombatHoverTileManager
{
    
    public static void createCombatTileHoverGrid()
    {
        Transform parent = CombatHoverTileParent.getCombatHoverTileParent();

        for(int row = CombatGrid.enemyRowUpperBounds; row <= CombatGrid.allyRowLowerBounds; row++)
        {
            for(int col = CombatGrid.colLeftBounds; col <= CombatGrid.colRightBounds; col++)
            {
                CombatHoverTile tile = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.combatHoverTile), parent).GetComponent<CombatHoverTile>();

                tile.setTargetCoords(row, col);
                tile.transform.position = CombatGrid.getPositionAt(row, col);

                
            }
        }
    }


}
