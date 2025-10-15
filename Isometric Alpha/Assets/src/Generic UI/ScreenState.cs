using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenState
{
	public int[] currentTabIndexes;
	public string[] rowNames;

	public ScreenState(ScreenManager screenManager)
	{
		setCurrentTabIndexes(screenManager);
		setRowNames(screenManager);
	}

	private void setCurrentTabIndexes(ScreenManager screenManager)
	{
		currentTabIndexes = new int[screenManager.tabCollections.Length];
		
		int index = 0;
		foreach(TabCollection tabCollection in screenManager.tabCollections)
		{
			

			currentTabIndexes[index] = tabCollection.getCurrentTabIndex();
			
			index++;
		}
	}

	private void setRowNames(ScreenManager screenManager)
	{
		rowNames = new string[screenManager.grids.Length];
		
		int index = 0;
		
		foreach (ScrollableUIElement grid in screenManager.grids)
		{
			if (grid != null && !(grid is null))
			{
				rowNames[index] = grid.getDisabledRowName();
			}

			index++;
		}
	}
}
