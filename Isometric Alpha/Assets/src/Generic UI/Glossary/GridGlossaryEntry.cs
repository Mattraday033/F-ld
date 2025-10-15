using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGlossaryEntry : GlossaryEntry
{
	private GridCoords[] coords;

	public GridGlossaryEntry(string title, string category, GridCoords[] coords):
	base(title, category)
	{
		this.coords = coords;
	}
	
	public override GameObject getDescriptionPanelFull()
	{
		return getDescriptionPanelFull(PanelType.Standard);
	}
	
	public override GameObject getDescriptionPanelFull(PanelType type)
	{
		return Resources.Load<GameObject>(PrefabNames.gridGlossaryEntryFull);
	}
	
	public override GameObject getDecisionPanel()
	{
		return null;
	}
	
	public bool withinFilter(string[] filterParameters)
	{
		return true;
	}
	
	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);
		
		applyToFormationDisplayUI(panel.gameObject.GetComponent<FormationDisplayUI>());
	}
	
	public void applyToFormationDisplayUI(FormationDisplayUI formationDisplay)
	{
		formationDisplay.setToReadOnly();
		
		foreach(GridCoords coord in coords)
		{
			formationDisplay.setColorOfGridSquare(coord, Color.red);
		}
	}

	public override void describeSelfRow(DescriptionPanel panel)
	{
		base.describeSelfRow(panel);
	}
	
	public override void setUpDecisionPanel(IDecisionPanel descisionPanel)
	{
		
	}

}
