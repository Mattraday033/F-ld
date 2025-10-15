using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWithGeneratedPartyTabs : ScreenManager
{

    public override bool hasGeneratedTabs()
    {
        return true;
    }

    public override void setToDefaultScreenState()
    {
        ScrollableUIElement currentScrollableUIElement = grids[0];

        currentScrollableUIElement.populatePanels(getListOfGeneratedTabDescribables());

        setUpTabs();

        if (OverallUIManager.previousPartyMember != null)
        {
            grids[0].disableGridRowAndClick(OverallUIManager.previousPartyMember.getName());
        }
        else
        {
            grids[0].disableGridRowAndClick(0);
        }
    }

    public override void populateGrid(int tabCollectionIndex)
    {
        base.populateGrid(tabCollectionIndex);

        if (tabCollectionIndex == 0)
        {
            setUpTabs();
        }
    }

    public virtual void setUpTabs()
    {
        Tab[] tabs = new Tab[grids[0].listOfRows.Count];

        int rowIndex = 0;
        foreach (GridRow row in grids[0].listOfRows)
        {
            tabs[rowIndex] = row.getAsTab(); //row.descriptionPanel.getObjectBeingDescribed().getName()

            rowIndex++;
        }

        tabCollections = new TabCollection[1] { new TabCollection(tabs) };

        // currentScrollableUIElement.disableGridRowAndClick(0);
    }

    public virtual ArrayList getListOfGeneratedTabDescribables()
    {
        return Tab.getList(DescribableList.PartyMembersWithPlayer);
    }

}
