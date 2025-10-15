using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatHoverManager
{

    // private static Vector3 offset = new Vector3(-0.02f, .435f, 0f);

    /*
        public const int rowLowerBounds = 7;
        public const int colLeftBounds = 0;
        public const int rowUpperBounds = 0;
        public const int colRightBounds = 3;
    */

    public static void instantiateCombatHovers()
    {
        ArrayList allCombatants = CombatGrid.getAllCombatants();

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

    // addStatsHover(stats);

    // GameObject tileHoverTarget = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.targetCombatTile), stats.combatSprite.transform);

    // tileHoverTarget.transform.SetAsFirstSibling();




}
