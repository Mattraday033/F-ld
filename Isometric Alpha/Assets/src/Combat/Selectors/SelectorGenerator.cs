using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SelectorGenerator
{
    private readonly static string singleTargetTitle = SelectorList.singleName;
    private static int generatedSelectorCount;
	private const string generatedSelectorName = "Generated Selector #";
	private const string childTileName = "Child Tile #";


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
		
		// Selector generatedSelector = new Selector();
		
		// generatedSelector.setSelectorObject(generateGameObject(allTileGridCoords));
		// generatedSelector.setChildTileAdjustments(generateAdjustments(allTileGridCoords));
		
		// generatedSelector.getPositionAndBoundsFromChildTileCoords(allTileGridCoords);
		
		// return (Selector) generatedSelector;
        return null;
	}
	
	private static GridCoords[] compileSelectorChildTileCoords(Selector[] selectors)
	{
		GridCoords[] coordinatesOfAllChildTiles = new GridCoords[0];
		
		//Debug.LogError("selectors.Length = " + selectors.Length);
		
		foreach(Selector selector in selectors)
		{
			if(selector == null)
			{
				continue;
			}
			
			GridCoords[] childTileCoords = selector.getAllSelectorCoords();
			
			coordinatesOfAllChildTiles = Helpers.appendArray<GridCoords>(coordinatesOfAllChildTiles, childTileCoords);
		}
		
		return coordinatesOfAllChildTiles;
	}
	
	private static GameObject generateGameObject(GridCoords[] allTileGridCoords)
	{
		Transform selectorParent = SelectorManager.getInstance().selectorParent;
		GameObject selectorGameObjectParent = GameObject.Instantiate(Resources.Load<GameObject>(singleTargetTitle), selectorParent);
		
		selectorGameObjectParent.transform.position = CombatGrid.getPositionAt(allTileGridCoords[0]);
		selectorGameObjectParent.transform.localScale = Vector3.one;
		
		selectorGameObjectParent.name = generatedSelectorName + generatedSelectorCount;
		generatedSelectorCount++;
		
		for(int coordIndex = 1; coordIndex < allTileGridCoords.Length; coordIndex++)
		{
			if(!childAlreadyExists(allTileGridCoords, coordIndex))
			{
				GameObject selectorGameObjectChild = GameObject.Instantiate(Resources.Load<GameObject>(singleTargetTitle), selectorGameObjectParent.transform);
				
				selectorGameObjectChild.transform.position = CombatGrid.getPositionAt(allTileGridCoords[coordIndex]);
				selectorGameObjectChild.transform.localScale = Vector3.one;
				
				selectorGameObjectChild.name = childTileName + coordIndex;
				selectorGameObjectChild.SetActive(true);
			}
		}
		
		Helpers.updateGameObjectPosition(selectorGameObjectParent);
		
		selectorGameObjectParent.SetActive(false);
		
		return selectorGameObjectParent;
	}
	
	private static bool childAlreadyExists(GridCoords[] allChildCoords, int startIndex)
	{
		GridCoords coordsToCheck = allChildCoords[startIndex];
		
		for(int currentIndex = (startIndex-1); currentIndex >= 0; currentIndex--)
		{
			if(coordsToCheck.Equals(allChildCoords[currentIndex]))
			{
				return true;
			}
		}
		
		return false;
	}
	
	private static GridCoords[] generateAdjustments(GridCoords[] allTileGridCoords)
	{
		GridCoords parentCoords = allTileGridCoords[0];
		GridCoords[] adjustments = new GridCoords[0];
		
		foreach(GridCoords tileCoords in allTileGridCoords)
		{
			int rowAdjust = tileCoords.row - parentCoords.row;
			int colAdjust = tileCoords.col - parentCoords.col;
			
			if(rowAdjust == 0 && colAdjust == 0)
			{
				continue;
			} else
			{
				adjustments = Helpers.appendArray<GridCoords>(adjustments, new GridCoords(rowAdjust, colAdjust));
			}
		}
		
		return adjustments;
	}
	
}
