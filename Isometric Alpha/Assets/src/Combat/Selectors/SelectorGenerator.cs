using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SelectorGenerator
{
    private const string generatedSelectorName = "Generated Selector";
    private const int generatedSelectorWidthAndHeight = 4;

    public static Selector generate(Selector[] selectors)
    {
        if (selectors == null || selectors.Length == 0 || selectors.Contains(null))
        {
            Debug.LogError("Forced to generate a null selector");
            return null;
        }

        GridCoords[] allTileGridCoords = compileSelectorChildTileCoords(selectors);

        return generate(allTileGridCoords);
    }
	
	public static Selector generate(GridCoords[] allTileGridCoords)
	{
		if(allTileGridCoords == null || allTileGridCoords.Length == 0 || allTileGridCoords.Contains(GridCoords.getDefaultCoords()))
		{
			Debug.LogError("Forced to generate a null selector");
			return null;
		}

        GridCoords selectorCoords = default;

        if(CombatGrid.positionIsOnAlliedSide(allTileGridCoords[0]))
        {
            selectorCoords = new GridCoords(CombatGrid.allyRowUpperBounds, CombatGrid.colLeftBounds);
        } else
        {
            selectorCoords = new GridCoords(CombatGrid.enemyRowUpperBounds, CombatGrid.colLeftBounds);
        }

		return new Selector(name: generatedSelectorName,
                            width:generatedSelectorWidthAndHeight,
                            height:generatedSelectorWidthAndHeight,
                            startingCoords: selectorCoords,
                            spaces:  generateSpaces(allTileGridCoords));
	}
	
	private static GridCoords[] compileSelectorChildTileCoords(Selector[] selectors)
	{
		List<GridCoords> coordinatesOfAllChildTiles = new List<GridCoords>();
		
		foreach(Selector selector in selectors)
		{
			if(selector == null)
			{
				continue;
			}
			
			GridCoords[] childTileCoords = selector.getAllSelectorCoords();
			
			coordinatesOfAllChildTiles.AddRange(childTileCoords);
		}
		
		return coordinatesOfAllChildTiles.ToArray();
	}
	
	private static bool[,] generateSpaces(GridCoords[] allTileGridCoords)
	{
        bool[,] spaces = new bool[generatedSelectorWidthAndHeight, generatedSelectorWidthAndHeight];

        foreach(GridCoords coords in allTileGridCoords)
        {
            GridCoords coordsClone = coords.clone();

            if(coordsClone.row >= CombatGrid.allyRowUpperBounds)
            {
                coordsClone.row -= CombatGrid.allyRowUpperBounds;
            }

            spaces[coordsClone.col, coordsClone.row] = true;
        }

		return spaces;
	}
	
}
