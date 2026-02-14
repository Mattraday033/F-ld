using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CategoryType : IJournalCategory, IDescribable
{
    private DescribableList listType;

    public CategoryType(DescribableList listType)
    {
        this.listType = listType;
    }

    public string getName()
    {
        return listType.ToString();
    }

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
        DescriptionPanel.setText(panel.nameText, getName());
    }

	public void describeSelfRow(DescriptionPanel panel)
    {
        DescriptionPanel.setText(panel.nameText, getName());
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
        Tab.OnListRetrieved.RemoveListener(updateCounter);
    }


    public void updateCounter(DescribableList listType)
    {
        setPrimaryDescribable(new CategoryType(listType));
    }

    public override void updateCounter(IDescribable describable)
    {
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
