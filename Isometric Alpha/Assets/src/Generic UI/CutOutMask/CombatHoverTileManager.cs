using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectorContainer
{
    public Selector selector;
}

public static class CombatHoverTileManager
{
    public readonly static UnityEvent<SelectorContainer> GetHoverSelector = new UnityEvent<SelectorContainer>();
    private static Dictionary<GridCoords, CombatHoverTile> hoverTileDict;

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

                hoverTileDict.Add(new GridCoords(row, col), tile);
            }
        }
    }

    public static void hideAllTiles()
    {
        foreach(CombatHoverTile tile in hoverTileDict.Values)
        {
            tile.hideTile();
        }
    }

    public static void setTilesToColor(this Selector selector, Color color)
    {
        GridCoords[] allCoordsToColor = selector.getAllSelectorCoords();

        foreach(GridCoords coords in allCoordsToColor)
        {
            hoverTileDict[coords].setColor(color);
        }
    }

    public static Selector getHoverSelector()
    {
        if(CombatStateManager.whoseTurn != WhoseTurn.Player)
        {
            return null;
        }

        SelectorContainer container = new SelectorContainer();

        GetHoverSelector.Invoke(container);

        GetHoverSelector.RemoveAllListeners();

        return container.selector;
    }

    public static void resetHoverTileDict()
    {
        hoverTileDict = new Dictionary<GridCoords, CombatHoverTile>();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        hoverTileDict = new Dictionary<GridCoords, CombatHoverTile>();

        CombatStateManager.OnCombatEnd.AddListener(resetHoverTileDict);
        LoadSaveFile.OnLoadResetData.AddListener(resetHoverTileDict);
    }

}
