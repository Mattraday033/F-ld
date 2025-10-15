using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHoverTileParent : MonoBehaviour
{

    private static Transform combatHoverTileParent;

    private void Awake()
    {
        combatHoverTileParent = transform;
    }

    public static Transform getCombatHoverTileParent()
    {
        return combatHoverTileParent;
    }

}
