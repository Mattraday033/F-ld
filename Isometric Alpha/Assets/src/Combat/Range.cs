using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Range
{
	public static GridCoords getRangeAllyStartingPosition(string rangeName)
	{
		GridCoords allyStartingPosition = SelectorList.getByName(rangeName).startingCoords;

		return new GridCoords(allyStartingPosition.row + 4, allyStartingPosition.col);
	}

	// public static GridCoords getRangeEnemyStartingPosition(string rangeName)
	// {
	// 	switch(rangeName)
	// 	{
	// 		case SelectorList.singleName:
	// 		case SelectorList.hookOneName:
	// 			return new GridCoords(1,2);
	// 		case SelectorList.boxTwoName:
	// 		case SelectorList.reverseHookOneName:
	// 			return new GridCoords(1,1);
	// 		case SelectorList.reverseL_OneName:
	// 		case SelectorList.horizontalThreeName:
	// 		case SelectorList.horizontalFourName:
	// 			return new GridCoords(2,1);
	// 		default:
	// 			return new GridCoords(2,2);
	// 	}
	// }

	public static List<GlossaryEntry> getAllRangesGlossaryEntries()
	{
		List<GlossaryEntry> allRangesGlossaryEntries = new List<GlossaryEntry>();

		foreach(Selector selector in SelectorList.selectorDict.Values)
		{
            if(selector.name.Equals(SelectorList.playerCursor.name))
            {
                continue;
            }

			GridGlossaryEntry gridGlossaryEntry = new GridGlossaryEntry(selector.name,"Range",getDefaultRangeCoords(selector.name));

			allRangesGlossaryEntries.Add(gridGlossaryEntry);
		}

		return allRangesGlossaryEntries;
	}

	private static GridCoords[] getDefaultRangeCoords(string rangeName)
	{
		Selector selector = SelectorList.getByName(rangeName);

		return selector.getAllSelectorCoords();
	}
}
