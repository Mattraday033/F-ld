using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Range
{
	public static GridCoords getRangeAllyStartingPosition(SelectorTemplate rangeTemplate)
	{
		GridCoords allyStartingPosition = SelectorFactory.buildByTemplate(rangeTemplate).startingCoords;

		return new GridCoords(allyStartingPosition.row + 4, allyStartingPosition.col);
	}

	// public static GridCoords getRangeEnemyStartingPosition(SelectorTemplate rangeTemplate)
	// {
	// 	switch(rangeTemplate)
	// 	{
	// 		case SelectorTemplate.Single:
	// 		case SelectorTemplate.HookOne:
	// 			return new GridCoords(1,2);
	// 		case SelectorTemplate.BoxTwo:
	// 		case SelectorTemplate.ReverseHookOne:
	// 			return new GridCoords(1,1);
	// 		case SelectorTemplate.ReverseL_One:
	// 		case SelectorTemplate.HorizontalThree:
	// 		case SelectorTemplate.HorizontalFour:
	// 			return new GridCoords(2,1);
	// 		default:
	// 			return new GridCoords(2,2);
	// 	}
	// }

	public static List<GlossaryEntry> getAllRangesGlossaryEntries()
	{
		List<GlossaryEntry> allRangesGlossaryEntries = new List<GlossaryEntry>();

		foreach(SelectorTemplate selectorTemplate in SelectorFactory.selectorDict.Keys)
		{
            if(selectorTemplate == SelectorTemplate.PlayerCursor)
            {
                continue;
            }

			GridGlossaryEntry gridGlossaryEntry = new GridGlossaryEntry(selectorTemplate.ToFriendlyString(),"Range",getDefaultRangeCoords(selectorTemplate));

			allRangesGlossaryEntries.Add(gridGlossaryEntry);
		}

		return allRangesGlossaryEntries;
	}

	private static GridCoords[] getDefaultRangeCoords(SelectorTemplate rangeTemplate)
	{
		Selector selector = SelectorFactory.buildByTemplate(rangeTemplate);

		return selector.getAllSelectorCoords();
	}
}
