using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ScrollableTabList : ScrollableUIElement
{
	public int tabCollectionIndexToCreate = -1;
	public List<Tab> newTabs;

	public override void populatePanels(List<IDescribable> listOfDescribables)
	{
		newTabs = new List<Tab>();

		deleteAllPanels();

		int rowIndex = 0;
		foreach (IDescribable describable in listOfDescribables)
		{
			GridRow currentRow = populatePanel(describable, rowIndex);

			// if (describable != null && !(describable is null))
			// {
			// 	newTabs.Add(new Tab(currentRow, tabCollectionIndexToCreate, describable.getName()));
			// }

			listOfRows.Add(currentRow);

			rowIndex++;
		}

		//OverallUIManager.currentScreenManager.setCurrentTabCollection(tabCollectionIndexToCreate);
		// OverallUIManager.currentScreenManager.tabCollections[tabCollectionIndexToCreate] = createTabCollection(newTabs);
		// OverallUIManager.currentScreenManager.grids[tabCollectionIndexToCreate - 1] = null;
		
		if (clickFirstPanel)
		{
			clickFirstPanelInList();
		}
		else if (clickLastPanel)
		{
			clickLastPanelInList();
		}
	}
	
	public override void deleteAllPanels()
	{
		foreach(GridRow row in listOfRows)
		{
			Destroy(row.gameObject);	
		}

		newTabs = new List<Tab>();
		listOfRows = new List<GridRow>();
	}
}
