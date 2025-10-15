using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

// [System.Serializable]
public class JournalHandler : ScreenManager
{
    private const int questTabIndex = 0;
    private const int perkTabIndex = 1;
    private const int glossaryTabIndex = 2;

    private const int categoryTitlePanelIndex = 0;

    public static Dictionary<string, string> subcategoryDictionary = new Dictionary<string, string>();

    public static string[] lastCategoryOpened;

    public ScrollableUIElement subcategoryGrid;

    public DescriptionPanelSlot categoryTitlePanelSlot;

    public override ScreenType getScreenType()
    {
        return ScreenType.Journal;
    }

    public override void revealDescriptionPanelSet(IDescribable objectToDescribe)
    {
        if (getCurrentTabCollection() >= descriptionPanelSlots.Count)
        {
            categoryTitlePanelSlot.setPrimaryDescribable(objectToDescribe);
        }
        else
        {
            base.revealDescriptionPanelSet(objectToDescribe);//PanelType.GlossaryDescription;

            addSubcategoryData(getCurrentCategory().getName(), objectToDescribe.getName());
        }
    }

    public override void populateGrid(int tabCollectionIndex)
    {
        if (tabCollectionIndex < grids.Length)
        {
            base.populateGrid(tabCollectionIndex);

        }
        else
        {
            subcategoryGrid.populatePanels(getListOfSubCategories());

            string subcategoryName = getNameOfLastSubCategoryOpened(getCurrentCategory());

            if (subcategoryName != null)
            {
                subcategoryGrid.disableGridRowAndClick(subcategoryName);
            }
            else
            {
                subcategoryGrid.disableGridRowAndClick(0);
            }
        }
    }

    public override void populateAllGrids()
    {
        base.populateAllGrids();

        string lastCategory = getLastCategoryOpened();

        if (lastCategory != null)
        {
            grids[grids.Length - 1].disableGridRowAndClick(getLastCategoryOpened());

        }
        else
        {
            grids[grids.Length - 1].disableGridRowAndClick(0);
        }

        string lastSubcategory = getNameOfLastSubCategoryOpened(getLastCategoryOpened());

        if (lastSubcategory != null)
        {
            subcategoryGrid.disableGridRowAndClick(lastSubcategory);
        }
        else
        {
            subcategoryGrid.disableGridRowAndClick(0);
        }
    }

    public override void setToDefaultScreenState()
    {
        foreach (TabCollection collection in tabCollections)
        {
            collection.selectAndClickTab(0);
        }

        populateAllGrids();
    }

    private ArrayList getListOfSubCategories()
    {
        IJournalCategory currentCategory = (IJournalCategory)grids[grids.Length - 1].getDisabledRowDescribable();

        if (currentCategory == null)
        {
            return new ArrayList();
        }
        else
        {
            return currentCategory.getSubcategories();
        }
    }

    public override void setGridRowType(int gridIndex, RowType rowType)
    {
        if (gridIndex < grids.Length)
        {
            base.setGridRowType(gridIndex, rowType);
        }
        else
        {
            subcategoryGrid.setRowType(rowType);
        }
    }

    public void addSubcategoryData(string category, string subcategory)
    {
        setLastCategoryOpened(category);
        subcategoryDictionary[category] = subcategory;
    }

    public override void setCurrentTab(int tabIndex)
    {
        setFirstLastSubCategoryGridClick(tabIndex);

        IDescribable lastCategory = grids[0].getDisabledRowDescribable();
        IDescribable lastSubcategory = subcategoryGrid.getDisabledRowDescribable();

        if (lastCategory != null && !(lastCategory is null) &&
            lastSubcategory != null && !(lastSubcategory is null))
        {
            addSubcategoryData(lastCategory.getName(), lastSubcategory.getName());

            grids[0].deleteAllPanels();
            subcategoryGrid.deleteAllPanels();
        }
        else if (lastCategory != null && !(lastCategory is null))
        {
            setLastCategoryOpened(lastCategory.getName());
            grids[0].deleteAllPanels();
        }

        // destroyCategoryTitlePanel();
        hideDescriptionPanel(getCurrentTabCollection());

        base.setCurrentTab(tabIndex);
    }

    private void setFirstLastSubCategoryGridClick(int currentTabIndex)
    {
        if (subcategoryGrid == null)
        {
            return;
        }

        switch (currentTabIndex)
        {
            case questTabIndex:
                subcategoryGrid.clickFirstPanel = false;
                subcategoryGrid.clickLastPanel = true;
                break;
            case perkTabIndex:
            case glossaryTabIndex:
                subcategoryGrid.clickFirstPanel = true;
                subcategoryGrid.clickLastPanel = false;
                break;
            default:
                Debug.LogError("Unimplemented tab index: " + currentTabIndex);
                return;
        }
    }

    private void setLastCategoryOpened(string category)
    {
        setUpLastCategoryArray();

        lastCategoryOpened[tabCollections[0].getCurrentTabIndex()] = category;
    }

    private string getLastCategoryOpened()
    {
        setUpLastCategoryArray();

        return lastCategoryOpened[tabCollections[0].getCurrentTabIndex()];
    }

    private void setUpLastCategoryArray()
    {
        if (lastCategoryOpened == null)
        {
            lastCategoryOpened = new string[tabCollections[0].collection.Length];
        }
    }

    private IDescribable getCurrentCategory()
    {
        return categoryTitlePanelSlot.getCurrentDescribables()[0];
    }

    public static string getNameOfLastSubCategoryOpened(string categoryName)
    {
        if (categoryName == null || !subcategoryDictionary.ContainsKey(categoryName))
        {

            return null;
        }

        return subcategoryDictionary[categoryName];
    }

    public static string getNameOfLastSubCategoryOpened(IDescribable category)
    {
        if (category == null || !subcategoryDictionary.ContainsKey(category.getName()))
        {
            return null;
        }

        return subcategoryDictionary[category.getName()];
    }

    public static void wipeLastOpened()
    {
        subcategoryDictionary = new Dictionary<string, string>();
        lastCategoryOpened = null;
    }

}
