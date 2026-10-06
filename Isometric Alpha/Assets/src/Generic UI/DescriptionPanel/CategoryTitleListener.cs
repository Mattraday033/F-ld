using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CategoryType : IJournalCategory, IDescribable
{
    private DescribableList listType;
    private string _NPCName;

    public CategoryType(DescribableList listType)
    {
        this.listType = listType;
        this._NPCName = NameSourceExtensions.splitCamelCase(listType.ToString());
    }

    public string displayName { get { return _NPCName; } }
    public string uniqueName { get { return listType.ToString(); } }

    public List<IDescribable> getSubcategories()
    {
        return new List<IDescribable>();
    }

    #region IDescribable

	public bool ineligible()
    {
        return false;
    }

    public GameObject getRowType(RowType rowType)
    {
		return getDescriptionPanelFull(PanelType.Standard);
    }

	public GameObject getDescriptionPanelFull()
	{
		return getDescriptionPanelFull(PanelType.Standard);
	}

	public GameObject getDescriptionPanelFull(PanelType type)
	{
		return Resources.Load<GameObject>(PrefabNames.glossaryCategoryNameFull);
	}

	public GameObject getDecisionPanel()
    {
        return null;
    }

	public bool withinFilter(string[] filterParameters)
    {
        return true;
    }

	public void describeSelfFull(DescriptionPanel panel)
    {
        DescriptionPanel.setText(panel.nameText, displayName);
    }

	public void describeSelfRow(DescriptionPanel panel)
    {
        DescriptionPanel.setText(panel.nameText, displayName);
    }

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {
        
    }

	public List<IDescribable> getRelatedDescribables()
    {
        return getSubcategories();
    }

	public bool buildableWithBlocks()
    {
        return false;
    }

	public bool buildableWithBlocksRows()
    {
        return false;
    }

    #endregion
}

public class CategoryTitleListener : UIDescriptionPanelSlot
{

    public bool listenForSubcategory = false;

    protected override void Awake()
    {
        base.Awake();

        if(!listenForSubcategory)
        {
            Tab.OnListRetrieved.AddListener(updateCounter);
        }
    }

    private void OnDestroy()
    {
        //this OnDestroy is the one Unity calls, so the base's listeners are taken off from here as well
        removeListeners();

        Tab.OnListRetrieved.RemoveListener(updateCounter);
    }


    public void updateCounter(DescribableList listType)
    {
        //every list fetched anywhere in the game comes through here, so a hidden journal would otherwise retitle itself after each one
        if (hiddenWithItsScreen())
        {
            return;
        }

        setPrimaryDescribable(new CategoryType(listType));
    }

    public override void updateCounter(IDescribable describable)
    {
        if (hiddenWithItsScreen())
        {
            return;
        }

        IJournalCategory category = describable as IJournalCategory;

        if(category == null || !listenForSubcategory)
        {
            return;
        }

        setPrimaryDescribable(describable);
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        return new List<UnityEvent>();
    }
}
