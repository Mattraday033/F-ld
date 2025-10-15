using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJournalCategory
{
	public string getName();
	
	public ArrayList getSubcategories();
}


public class GlossaryCategory : IDescribable, IJournalCategory
{
	private string title;
	private ArrayList subcategories;

	public GlossaryCategory(string title)
	{
		this.title = title;
	}

	public GlossaryCategory(string title, ArrayList subcategories)
	{
		this.title = title;
		this.subcategories = subcategories;
	}

	public string getName()
	{
		return title;
	}

	public bool ineligible()
	{
		return false;
	}

	public GameObject getRowType(RowType rowType)
	{
		return Resources.Load<GameObject>(PrefabNames.glossaryCategoryRow);
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
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, getName());
	}

	public void describeSelfRow(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, getName());
	}

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
	{

	}

	public virtual ArrayList getSubcategories()
	{
		return subcategories;
	}

	public ArrayList getRelatedDescribables()
	{
		return new ArrayList();
	}

	public bool buildableWithBlocks()
	{
		return false;
	}
	
	public bool buildableWithBlocksRows()
    {
        return false;
    }
}
