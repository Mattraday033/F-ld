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
                GameObject combatTileHoverGameObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.targetCombatTile), CombatHoverTileParent.getCombatHoverTileParent());

                CombatTileHover combatTileHoverComponent = combatTileHoverGameObject.GetComponent<CombatTileHover>();

                combatTileHoverComponent.setTargetCoords(row, col);

                combatTileHoverGameObject.transform.localPosition = CombatGrid.getPositionAt(row, col);

                Helpers.updateGameObjectPosition(combatTileHoverGameObject);
            }
        }
    }

}
