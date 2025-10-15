using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Range
{

	public const int singleTargetIndex = 1;
	public const int doubleVerticalIndex = 2;
	public const int doubleHorizontalIndex = 3;
	public const int tripleVerticalIndex = 4;
	public const int tripleHorizontalIndex = 5;
	public const int tripleHookIndex = 6;
	public const int tripleReverseHookIndex = 7; 
	public const int tripleLIndex = 8;
	public const int tripleReverseLIndex = 9; 
	public const int quadrupleVerticalIndex = 10;
	public const int quadrupleHorizontalIndex = 11;
	public const int quadrupleBoxIndex = 12;
	public const int quintupleCrossIndex = 13;
    public const int octupleHorizontalIndex = 14;
    public const int checkeredLeftIndex = 15;
	public const int checkeredRightIndex = 16;
	public const int nontupleBoxIndex = 17;
	public const int hexadecupleBoxIndex = 18;
	
	public static string getRangeTitle(int rangeIndex)
	{
		switch(rangeIndex)
		{
			case 0:
				return "Self";
			case singleTargetIndex: 
				return "Single Target";
			case doubleVerticalIndex:
				return "Double Vertical";
			case doubleHorizontalIndex:
				return "Double Horizontal";
			case tripleVerticalIndex:
				return "Triple Vertical";
			case tripleHorizontalIndex:
				return "Triple Horizontal";
			case tripleHookIndex:
				return "Triple Hook";
			case tripleReverseHookIndex: 
				return "Triple Reverse Hook";
			case tripleLIndex:
				return "Triple L";
			case tripleReverseLIndex: 
				return "Triple Reverse L";
			case quadrupleVerticalIndex: 
				return "Quadruple Vertical";
			case quadrupleHorizontalIndex: 
				return "Quadruple Horizontal";
			case quadrupleBoxIndex:
				return "Quadruple Box";
			case quintupleCrossIndex:
				return "Quintuple Cross";
            case octupleHorizontalIndex:
                return "Octuple Horizontal";
            case checkeredLeftIndex:
				return "Checkered Left";
			case checkeredRightIndex:
				return "Checkered Right";
			case nontupleBoxIndex:
				return "Nontuple Box";
			case hexadecupleBoxIndex:
				return "Hexadecuple Box";
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
			case tripleHookIndex:
				return new GridCoords(1,2);
			case nontupleBoxIndex: 
			case tripleReverseHookIndex: 
				return new GridCoords(1,1);
			case tripleReverseLIndex:
			case quadrupleHorizontalIndex:
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
		return hexadecupleBoxIndex;
	}

	public static ArrayList getAllRangesGlossaryEntries()
	{
		ArrayList allRangesGlossaryEntries = new ArrayList();
		
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
