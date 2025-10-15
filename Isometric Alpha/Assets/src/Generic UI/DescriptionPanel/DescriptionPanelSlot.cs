using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DescriptionPanelSlot : MonoBehaviour
{

    public static IDescribable lastObjectToBeDescribed;

    public bool neverUseBlocks;
    public bool neverAllowDecisionPanels;
    public bool addToScreenOnCreation;
    public bool handlesRelatedDescribables;


    public DescriptionPanelBuilder prebuiltBuilder;
    public DescriptionPanelBuilderType builderType;
    public List<DescriptionPanelBuildingBlockType> whiteList;
    public DescriptionPanelSlot[] additionalSlots;

    public int collectionTabIndex;
    public PanelType panelType;
    public BlockFormatType formatType = BlockFormatType.None;

    public Transform descriptionPanelParent;
    public Transform decisionPanelParent;

    private List<GameObject> descriptionPanelGameObjects = new List<GameObject>();
    private DescriptionPanel descriptionPanelComponent;
    private DescriptionPanelBuilder descriptionPanelBuilder;
    private IDecisionPanel decisionPanel;

    public ScrollableUIElement grid;

    private List<IDescribable> primaryDescribables;

    private List<IDescribable> tempDescribables;


    private void Awake()
    {
        if (addToScreenOnCreation)
        {
            OverallUIManager.currentScreenManager.descriptionPanelSlots[collectionTabIndex] = this;
        }
    }

    // private IDescribable getMainPrimaryDescribable()
    // {
    //     return getFirstDescribableInList(primaryDescribables);
    // }


    // private IDescribable getMainTempDescribable()
    // {
    //     return getFirstDescribableInList(tempDescribables);
    // }

    // private static IDescribable getFirstDescribableInList(List<IDescribable> list)
    // {
    //     if (list == null || list.Count <= 0)
    //     {
    //         return null;
    //     }
    //     else
    //     {
    //         return list[0];
    //     }
    // }

    private static List<IDescribable> addRelatedDescribables(List<IDescribable> list, IDescribable describable)
    {
        ArrayList relatedDescribables = describable.getRelatedDescribables();

        foreach(IDescribable relatedDescribable in relatedDescribables)
        {
            list.Add(relatedDescribable);
        }

        return list;
    }

    public List<IDescribable> getCurrentDescribables()
    {
        if (tempDescribables != null)
        {
            return tempDescribables;
        }
        else
        {
            return primaryDescribables;
        }
    }

    public void resetPrimary()
    {
        if (primaryDescribables != null && primaryDescribables.Count > 0)
        {
            IDescribable oldPrimaryDescribable = primaryDescribables[0];
            removePrimaryDescribable();
            setPrimaryDescribable(oldPrimaryDescribable);
        }
    }

    public void setPrimaryDescribable(IDescribable primaryDescribable)
    {
        if (primaryDescribable == null)
        {
            return;
        }

        primaryDescribables = new List<IDescribable>();

        primaryDescribables.Add(primaryDescribable);

        if (handlesRelatedDescribables)
        {
            addRelatedDescribables(primaryDescribables, primaryDescribable);
        }

        lastObjectToBeDescribed = primaryDescribable;

        revealDescriptionPanelSet();

        if (additionalSlots == null)
        {
            return;
        }

        foreach (DescriptionPanelSlot slot in additionalSlots)
        {
            slot.setPrimaryDescribable(primaryDescribable);
        }
    }

    public void setTempDescribable(IDescribable tempDescribable)
    {
        if (tempDescribable == null)
        {
            return;
        }

        tempDescribables = new List<IDescribable>();

        tempDescribables.Add(tempDescribable);

        if (handlesRelatedDescribables)
        {
            addRelatedDescribables(tempDescribables, tempDescribable);
        }

        lastObjectToBeDescribed = tempDescribable;

        revealDescriptionPanelSet();

        if (additionalSlots == null)
        {
            return;
        }

        foreach (DescriptionPanelSlot slot in additionalSlots)
        {
            slot.setTempDescribable(tempDescribable);
        }
    }

    public void revertToPrimaryDescribable()
    {
        tempDescribables = null;

        if (primaryDescribables != null)
        {
            lastObjectToBeDescribed = primaryDescribables[0];
        }
        else
        {
            lastObjectToBeDescribed = null;
        }
        
        revealDescriptionPanelSet();

        if (additionalSlots == null)
        {
            return;
        }

        foreach (DescriptionPanelSlot slot in additionalSlots)
        {
            slot.revertToPrimaryDescribable();
        }
    }

    public void removePrimaryDescribable()
    {
        primaryDescribables = null;

        hideDescriptionPanel();

        if (additionalSlots == null)
        {
            return;
        }

        foreach (DescriptionPanelSlot slot in additionalSlots)
        {
            slot.removePrimaryDescribable();
        }
    }

    public void updateDecisionPanel()
    {
        if (decisionPanel != null && !(decisionPanel is null))
        {
            decisionPanel.updateEnabledButtons();
        }
    }

    private void revealDescriptionPanelSet()
    {
        hideDescriptionPanel();
        List<IDescribable> currentDescribables = getCurrentDescribables();

        if (currentDescribables == null)
        {
            return;
        }

        // if (descriptionPanels[currentTabCollection] != null)
        // {
        //     hideCurrentDescriptionPanel();
        // }

        foreach (IDescribable currentDescribable in currentDescribables)
        {
            if (!neverUseBlocks && currentDescribable.buildableWithBlocks())
            {
                buildDescriptionPanelWithBlocks(currentDescribable);
            }
            else
            {
                buildDescriptionPanel(currentDescribable, panelType);
            }

            if (!preventDecisionPanel())
            {
                buildDecisionPanel(currentDescribable);
            }
        }
    }

    private void buildDescriptionPanelWithBlocks(IDescribable currentDescribable)
    {
        IDescribableInBlocks describableInBlocks = (IDescribableInBlocks)currentDescribable;

        GameObject descriptionPanelGameObject;

        if (prebuiltBuilder != null)
        {
            descriptionPanelGameObject = prebuiltBuilder.gameObject;
        } else
        {
            descriptionPanelGameObject = DescriptionPanelBuilder.getDescriptionPanelBuilder(builderType, descriptionPanelParent);
        }

        DescriptionPanelBuilder descriptionPanelBuilder = descriptionPanelGameObject.GetComponent<DescriptionPanelBuilder>();

        if (whiteList.Count > 0)
        {
            descriptionPanelBuilder.filter = new BuilderFilterWhiteList(whiteList);
        }

        descriptionPanelBuilder.buildDescriptionPanel(describableInBlocks, BlockFormat.getBlockFormat(formatType));

        descriptionPanelComponent = null;

        descriptionPanelGameObjects.Add(descriptionPanelGameObject);
    }

    private void buildDescriptionPanel(IDescribable currentDescribable, PanelType panelType)
    {
        GameObject descriptionPanelGameObject = Instantiate(currentDescribable.getDescriptionPanelFull(panelType), descriptionPanelParent);

        descriptionPanelComponent = descriptionPanelGameObject.GetComponent<DescriptionPanel>();

        currentDescribable.describeSelfFull(descriptionPanelComponent);

        descriptionPanelGameObject.SetActive(true);

        descriptionPanelBuilder = null;

        descriptionPanelGameObjects.Add(descriptionPanelGameObject);
    }

    private void buildDecisionPanel(IDescribable currentDescribable)
    {
        GameObject descisionPanelTemplate = currentDescribable.getDecisionPanel();

        if (descisionPanelTemplate != null && !(descisionPanelTemplate is null))
        {
            decisionPanel = Instantiate(descisionPanelTemplate, decisionPanelParent).GetComponent<IDecisionPanel>();
            decisionPanel.setScrollableUIElement(grid);
            decisionPanel.setCollectionIndex(collectionTabIndex);

            currentDescribable.setUpDecisionPanel(decisionPanel);

            updateDecisionPanel();

            decisionPanel.getGameObject().SetActive(true);
        }
    }

    private void hideDescriptionPanel()
    {
        if (prebuiltBuilder != null)
        {
            prebuiltBuilder.destroyRows();
            return;
        }

        foreach (GameObject gameObj in descriptionPanelGameObjects)
            {
                Destroy(gameObj);
            }

        descriptionPanelGameObjects = new List<GameObject>();

        if (decisionPanel == null ||
            decisionPanel is null)
        {
            return;
        }

        Destroy(decisionPanel.getGameObject());
        decisionPanel = null;
    }

    private bool preventDecisionPanel()
    {
        if (CombatStateManager.inCombat || neverAllowDecisionPanels)
        {
            return true;
        }

        switch (panelType)
        {
            case PanelType.AbilityEditor:
                return true;
            default:
                return false;
        }
    }
}
