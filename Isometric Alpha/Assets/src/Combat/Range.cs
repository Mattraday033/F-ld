using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Range
{

	public const int singleTargetIndex = 1;
	public const int verticalOneIndex = 2;
	public const int horizontalOneIndex = 3;
	public const int verticalTwoIndex = 4;
	public const int horizontalTwoIndex = 5;
	public const int hookOneIndex = 6;
	public const int reverseHookOneIndex = 7; 
	public const int L_OneIndex = 8;
	public const int reverseL_OneIndex = 9; 
	public const int verticalThreeIndex = 10;
	public const int horizontalThreeIndex = 11;
	public const int boxOneIndex = 12;
	public const int crossIndex = 13;
    public const int horizontalFourIndex = 14;
    public const int checkeredLeftIndex = 15;
	public const int checkeredRightIndex = 16;
	public const int boxTwoIndex = 17;
	public const int boxThreeIndex = 18;
	
	public static string getRangeTitle(int rangeIndex)
	{
		switch(rangeIndex)
		{
			case 0:
				return "Self";
			case singleTargetIndex: 
				return "Single";
			case verticalOneIndex:
				return "Vertical 1";
			case horizontalOneIndex:
				return "Horizontal 1";
			case verticalTwoIndex:
				return "Vertical 2";
			case horizontalTwoIndex:
				return "Horizontal 2";
			case hookOneIndex:
				return "Hook 1";
			case reverseHookOneIndex: 
				return "Reverse Hook 1";
			case L_OneIndex:
				return "L 1";
			case reverseL_OneIndex: 
				return "Reverse L 1";
			case verticalThreeIndex: 
				return "Vertical 3";
			case horizontalThreeIndex: 
				return "Horizontal 3";
			case boxOneIndex:
				return "Box 1";
			case crossIndex:
				return "Cross";
            case horizontalFourIndex:
                return "Horizontal 4";
            case checkeredLeftIndex:
				return "Checkered Left";
			case checkeredRightIndex:
				return "Checkered Right";
			case boxTwoIndex:
				return "Box 2";
			case boxThreeIndex:
				return "Box 3";
			default:
				throw new IOException("Unidentified range index: " + rangeIndex);
		}
	}

	public static GridCoords getRangeAllyStartingPosition(int rangeIndex)
	{
		GridCoords allyStartingPosition = getRangeEnemyStartingPosition(rangeIndex);

		return new GridCoords(allyStartingPosition.row + 4, allyStartingPosition.col);
	}

	public static GridCoords getRangeEnemyStartingPosition(int rangeIndex)
	{
		switch(rangeIndex)
		{
			case singleTargetIndex:
			case hookOneIndex:
				return new GridCoords(1,2);
			case boxTwoIndex: 
			case reverseHookOneIndex: 
				return new GridCoords(1,1);
			case reverseL_OneIndex:
			case horizontalThreeIndex:
				return new GridCoords(2,1);
			default:
				return new GridCoords(2,2);
		}
	}
	
	public static int getSmallestRangeIndex()
	{
		return singleTargetIndex;
	}
	
	public static int getLargestRangeIndex()
	{
		return boxThreeIndex;
	}

	public static List<GlossaryEntry> getAllRangesGlossaryEntries()
	{
		List<GlossaryEntry> allRangesGlossaryEntries = new List<GlossaryEntry>();
		
		for(int rangeIndex = getSmallestRangeIndex(); rangeIndex <= getLargestRangeIndex(); rangeIndex++)
		{
			GridGlossaryEntry gridGlossaryEntry = new GridGlossaryEntry(getRangeTitle(rangeIndex),"Range",getDefaultRangeCoords(getRangeTitle(rangeIndex)));
			
			allRangesGlossaryEntries.Add(gridGlossaryEntry);
		}
		
		return allRangesGlossaryEntries;
	}
	
	private static GridCoords[] getDefaultRangeCoords(string rangeTitle)
	{
		Selector selector = Resources.Load<Selector>(rangeTitle);
		
		return selector.getAllSelectorCoords();
	}
}
