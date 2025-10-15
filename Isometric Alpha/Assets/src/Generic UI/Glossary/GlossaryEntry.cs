using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlossaryEntry : IDescribable
{
	public string title;
	public string category;

	public GlossaryEntry(string title, string category)
	{
		this.title = title;
		this.category = category;
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
		return Resources.Load<GameObject>(PrefabNames.glossaryEntryRow);
	}

	public virtual GameObject getDescriptionPanelFull()
	{
		return null;
	}

	public virtual GameObject getDescriptionPanelFull(PanelType type)
	{
		return null;
	}

	public virtual GameObject getDecisionPanel()
	{
		return null;
	}

	public bool withinFilter(string[] filterParameters)
	{
		return true;
	}

	public virtual void describeSelfFull(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, getName());
		DescriptionPanel.setText(panel.typeText, category);
	}

	public virtual void describeSelfRow(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, getName());
	}

	public virtual void setUpDecisionPanel(IDecisionPanel descisionPanel)
	{

	}

	public ArrayList getRelatedDescribables()
	{
		return new ArrayList();
	}

	public virtual bool buildableWithBlocks()
	{
		return false;
	}
	
	public virtual bool buildableWithBlocksRows()
    {
        return false;
    }
}
