using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest: IDescribable, IJournalCategory
{
	public string title;
	
	public bool active; //shows up in journal, finished or not
	public bool finished;
	public bool succeeded;
	
    public Dictionary<string, QuestStep> steps;
    public Dictionary<string, DeathStep> deathSteps;

    public Dictionary<int, QuestStep> activeQuestStepsDict = new Dictionary<int, QuestStep>();

	public Quest()
	{
		
	}
	
	public Quest(string title, bool active, bool finished, bool succeeded, Dictionary<string, QuestStep> steps, Dictionary<string, DeathStep> deathSteps)
	{
		this.title = title;
		
		this.active = active;
		this.finished = finished;
		this.succeeded = succeeded;
		
        this.steps = steps;
        this.deathSteps = deathSteps;
	}

	public List<IDescribable> getActiveQuestSteps()
	{
		List<IDescribable> activeQuestStepsList = new List<IDescribable>();
		
        for(int index = 0; index < activeQuestStepsDict.Count; index++)
        {
            activeQuestStepsList.Add(activeQuestStepsDict[index]);
        }

		return activeQuestStepsList;
	}

    public int getNextActivationIndex()
    {
        int activationIndex = 0;

        foreach(KeyValuePair<string, QuestStep> kvp in steps)
        {
            if(kvp.Value.active)
            {
                activationIndex++;
            }
        }

        foreach(KeyValuePair<string, DeathStep> kvp in deathSteps)
        {
            if(kvp.Value.active)
            {
                activationIndex++;
            }
        }

        return activationIndex;
    }

	public QuestStep getCurrentQuestStep()
	{
		return activeQuestStepsDict[activeQuestStepsDict.Count-1];
	}

	//IDescribable Methods

	public string getName()
	{
		return title;
	}

	public bool ineligible()
	{
		return finished;
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
		return false;
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
	
	public List<IDescribable> getRelatedDescribables()
	{
		return new List<IDescribable>();
	}
	
	public bool buildableWithBlocks()
    {
        return false;
    }

	public bool buildableWithBlocksRows()
    {
        return false;
    }

    //IJournalCategory Methods

	public List<IDescribable> getSubcategories()
	{
		return getActiveQuestSteps();
	}
}
