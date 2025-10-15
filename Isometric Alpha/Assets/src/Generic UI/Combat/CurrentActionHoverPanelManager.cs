using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentActionHoverPanelManager : DescriptionPanelSlot
{
    private static CurrentActionHoverPanelManager instance;

    public static CurrentActionHoverPanelManager getInstance()
    {
        return instance;
    }

    public static Transform getDescriptionPanelParent()
    {
        return getInstance().transform;
    }

    public static void addPrimaryDescriptionPanel(IDescribableInBlocks describable)
    {
        instance.setPrimaryDescribable(describable as IDescribable);
    }

    public static void addTempDescriptionPanel(IDescribableInBlocks describable)
    {
        instance.setTempDescribable(describable as IDescribable);
    }

    public static void revertToCurrentPrimaryDescribable()
    {
        instance.revertToPrimaryDescribable();
    }

    public static void removeCurrentPrimaryDescribable()
    {
        instance.removePrimaryDescribable();
    }

    public static void hidePanels()
    {
        getDescriptionPanelParent().gameObject.SetActive(false);
    }

    public static void showPanels()
    {
        getDescriptionPanelParent().gameObject.SetActive(true);
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Duplicate instances of CurrentActionHoverPanelManager exist erroneously");
        }

        instance = this;
    }


}
