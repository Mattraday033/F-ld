using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatHoverManager
{

    public static void instantiateCombatHovers()
    {
        for(int row = CombatGrid.rowUpperBounds; row <= CombatGrid.rowLowerBounds; row++)
        {
            for (int col = CombatGrid.colLeftBounds; col <= CombatGrid.colRightBounds; col++)
            {
                GameObject combatHoverTileGameObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.targetCombatTile), CombatHoverTileParent.getCombatHoverTileParent());

                CombatHoverTile combatHoverTileComponent = combatHoverTileGameObject.GetComponent<CombatHoverTile>();

                combatHoverTileComponent.setTargetCoords(row, col);

                combatHoverTileGameObject.transform.localPosition = CombatGrid.getPositionAt(row, col);

                Helpers.updateGameObjectPosition(combatHoverTileGameObject);
            }
        }
    }

}
