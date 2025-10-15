using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDeclarer : MonoBehaviour
{
	public bool populateSingleGrid;
	public int gridIndex = -1;
	public ScrollableUIElement grid;
	
	//private void Awake()
	//{
		// declareGrid();	
	// }
	
	private void OnEnable()
	{
		declareGrid();
	}

	private void declareGrid()
	{
		OverallUIManager.currentScreenManager.grids[gridIndex] = grid;

		if (populateSingleGrid)
		{
			// OverallUIManager.currentScreenManager.populateGrid(gridIndex);
		}
		else
		{
			OverallUIManager.currentScreenManager.populateAllGrids();
		}
		
	}
}
