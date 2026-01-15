using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColumnHeader : MonoBehaviour
{
    public int defaultColumnIndex;

    public ScreenManager screenManager;
    public ScrollableUIElement grid;
    public int tabCollectionIndex;

    public Button[] headerButtons;
    public IComparer<ISortable> currentComparisonMethod;
    public SortBy defaultSortBy;

    public bool clickDefaultHeader = true;
    public bool setAllHeadersToUninteractable = false;

    private void Awake()
    {
        if(clickDefaultHeader)
        {
            if(headerButtons.Length > 0)
            {   
                headerButtons[defaultColumnIndex].onClick.Invoke();
            } else
            {
                currentComparisonMethod = ComparerList.getComparer(defaultSortBy);
            }
        }
        

        if (setAllHeadersToUninteractable)
        {
            Helpers.setInteractability(headerButtons, false);
        }
    }

    public void setCurrentHeaderButton(int buttonIndex)
    {
        Helpers.setInteractability(headerButtons, buttonIndex);
    }

    public void setComparisonMethod(IComparer<ISortable> newComparisonMethod)
    {
        currentComparisonMethod = newComparisonMethod;
    }

    public void setComparisonMethodAndPopulateGrid(IComparer<ISortable> newComparisonMethod)
    {
        setComparisonMethod(newComparisonMethod);
        populateGrid();
    }

    private void populateGrid()
    {
        // if (screenManager != null && !(screenManager is null))
        // {
        //     ScreenManager.OnScreenInteriorUpdate.Invoke();
        // }
        // else if (Flags.getFlag(FlagNameList.newGameFlagName) || CombatStateManager.inCombat)
        // {
        //     grid.populatePanels(Tab.getList(DescribableList.Saves));
        // }

        ScreenManager.OnScreenInteriorUpdate.Invoke();
    }

    public IComparer<ISortable> getComparisonMethod()
    {
        return currentComparisonMethod;
    }

}
