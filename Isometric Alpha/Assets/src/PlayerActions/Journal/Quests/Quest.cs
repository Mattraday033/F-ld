using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

[System.Serializable]
public class Quest: IJSONConvertable, IDescribable, IJournalCategory
{
	public string title;
	
	public bool active; //shows up in journal, finished or not
	public bool finished;
	public bool succeeded;
	
	public int currentStepIndex;

	public QuestStep[] steps;
	public DeathStep[] deathSteps;
	
	public Quest()
	{
		
	}
	
	public Quest(string title, bool active, bool finished, bool succeeded, int currentStepIndex)
	{
		this.title = title;
		
		this.active = active;
		this.finished = finished;
		this.succeeded = succeeded;
		
		this.currentStepIndex = currentStepIndex;
	}

	public string convertToJson()
	{
		string json = "{\"title\":\"" + title + "\"," + 
				"\"active\":\"" + active + "\"," +
				"\"finished\":\"" + finished + "\"," +
				"\"succeeded\":\"" + succeeded + "\"," +
				"\"currentStepIndex\":\"" + currentStepIndex + "\"," +
				"\"questStepsLength\":\"" + steps.Length + "\"," +
				"\"steps\":[" + getStepStatus() + "]," + 
				"\"deathSteps\":[" + getDeathStepStatus() + "]}";
				
		return json;
	}

	public static Quest extractFromJson(string json)
	{
		json = json.Replace("{", "").Replace("}", "").Replace("\"", "").Replace("steps:[", "").Replace("]", "");

		string[] keyValuePairs = json.Split(",");

		string title = keyValuePairs[0].Split(":")[1];

		if (State.questDictionary.ContainsKey(QuestList.checkForQuestNameChange(title)))
		{
			Quest quest = State.questDictionary[QuestList.checkForQuestNameChange(title)];

			quest.active = bool.Parse(keyValuePairs[1].Split(":")[1]);
			quest.finished = bool.Parse(keyValuePairs[2].Split(":")[1]);
			quest.succeeded = bool.Parse(keyValuePairs[3].Split(":")[1]);
			quest.currentStepIndex = int.Parse(keyValuePairs[4].Split(":")[1]);

			quest.loadInStepStatus(keyValuePairs, int.Parse(keyValuePairs[5].Split(":")[1]));
			quest.loadInDeathStepStatus(keyValuePairs, int.Parse(keyValuePairs[5].Split(":")[1]));

			return quest;
		}
		else
		{
			return null;
		}
	} 

	//stepStatus expects to be ran in extractFromJson() after {/}/"'s have been removed
	private void loadInStepStatus(string[] keyValuePairs, int questStepsLength)
	{
		int nonStepStatusKVPs = 6;

		for(int kvpIndex = nonStepStatusKVPs;

			kvpIndex < (nonStepStatusKVPs + questStepsLength) &&
			(kvpIndex - nonStepStatusKVPs) < steps.Length;

			kvpIndex++)
        {
			steps[kvpIndex - nonStepStatusKVPs].active = bool.Parse(keyValuePairs[kvpIndex].Split(":")[1]);
        }
	}
	
	//deathStepStatus expects to be ran in extractFromJson() after {/}/"'s have been removed
	private void loadInDeathStepStatus(string[] keyValuePairs, int questStepsLength)
	{
		if(deathSteps.Length == 0)
		{
			return;
		}
		
		int nonDeathStepStatusKVPs = 6 + questStepsLength;
		
		for(int kvpIndex = nonDeathStepStatusKVPs; kvpIndex < keyValuePairs.Length; kvpIndex++)
		{
			deathSteps[kvpIndex-nonDeathStepStatusKVPs].active = bool.Parse(keyValuePairs[kvpIndex].Replace("deathSteps:[","").Split(";")[0].Split(":")[1]);
			deathSteps[kvpIndex-nonDeathStepStatusKVPs].currentStepOnDeath = int.Parse(keyValuePairs[kvpIndex].Replace("]", "").Split(";")[1].Split(":")[1]);
		}
	}

	private string getStepStatus()
	{
		string stepStatus = "";
		
		for(int stepIndex = 0; stepIndex < steps.Length; stepIndex++)
		{
			stepStatus += "{\"active\":" + steps[stepIndex].active + "}";
			
			int lastIndex = (steps.Length-1);
			
			if(stepIndex < lastIndex)
			{
				stepStatus += ",";
			}
		}
		
		return stepStatus;
	}
	
	private string getDeathStepStatus()
	{
		string deathStepStatus = "";
		
		for(int deathStepIndex = 0; deathStepIndex < deathSteps.Length; deathStepIndex++)
		{
			deathStepStatus += "{\"active\":" + deathSteps[deathStepIndex].active + ";\"currentStepOnDeath\":" + deathSteps[deathStepIndex].currentStepOnDeath + "}";
			
			int lastIndex = (deathSteps.Length-1);
			
			if(deathStepIndex < lastIndex)
			{
				deathStepStatus += ",";
			}
		}
		
		return deathStepStatus;
	}
	
	public ArrayList getActiveQuestSteps()
	{
		ArrayList activeQuestSteps = new ArrayList();
		
		foreach(QuestStep step in steps)
		{
			if(step.active)
			{
				activeQuestSteps.Add(step);
				
				foreach(DeathStep deathStep in deathSteps)
				{
					if(deathStep.active && deathStep.currentStepOnDeath == step.stepIndex)
					{
						activeQuestSteps.Add(deathStep);
					}
				}
			}
		}
		
		return activeQuestSteps;
	}

	public QuestStep getCurrentQuestStep()
	{
		return steps[currentStepIndex];
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

    //IJournalCategory Methods

	public ArrayList getSubcategories()
	{
		return getActiveQuestSteps();
	}
}
