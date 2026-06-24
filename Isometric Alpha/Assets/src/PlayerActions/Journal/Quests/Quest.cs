using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class Quest: IDescribable, IJournalCategory
{
    private const string failureOnAreaHostilityVarName = "failureOnAreaHostility";
    private const string failureOnDeathVarName = "failureOnDeath";

	public string title;
	
	public bool active; //shows up in journal, finished or not
	public bool finished;
	public bool succeeded;

    public List<QuestFailureCondition> failureConditions = new List<QuestFailureCondition>();

    public Dictionary<string, QuestStep> steps = new Dictionary<string, QuestStep>();
    public Dictionary<string, DeathStep> deathSteps = new Dictionary<string, DeathStep>();

    public Dictionary<int, QuestStep> activeQuestStepsDict = new Dictionary<int, QuestStep>();

    public Quest(string jsonString)
	{
		dynamic jsonDynamic = JsonConvert.DeserializeObject<dynamic>(jsonString);

		this.title = GetFromJson.getElementFromJson(InteriorDefaultValues.badString, nameof(title), jsonDynamic, InteriorDefaultValues.badString);

		this.active = GetFromJson.getElementFromJson(InteriorDefaultValues.badString, nameof(active), jsonDynamic, InteriorDefaultValues.badBool);
		this.finished = GetFromJson.getElementFromJson(InteriorDefaultValues.badString, nameof(finished), jsonDynamic, InteriorDefaultValues.badBool);
		this.succeeded = GetFromJson.getElementFromJson(InteriorDefaultValues.badString, nameof(succeeded), jsonDynamic, InteriorDefaultValues.badBool);

        QuestListStepWrapper[] questListStepWrappers = GetFromJson.getElementFromJson(InteriorDefaultValues.badString, nameof(steps), jsonDynamic, InteriorDefaultValues.defaultEmptyQuestListStepWrapperArray).ToObject<QuestListStepWrapper[]>();

        foreach(QuestListStepWrapper questListStepWrapper in questListStepWrappers)
        {
            steps.Add(questListStepWrapper.stepName, new QuestStep(this, questListStepWrapper));
        }

        addFailureConditions(failureOnAreaHostilityVarName, jsonDynamic, (Func<string, AreaHostilityFailureCondition>)(areaName => new AreaHostilityFailureCondition(areaName)));

        addFailureConditions(failureOnDeathVarName, jsonDynamic, (Func<string, CharacterDeathFailureCondition>)(characterName => new CharacterDeathFailureCondition(characterName)));

		AreaList.AreaBecameHostile.AddListener(checkFailState);
        DeathFlagManager.OnDeathFlagCreated.AddListener(checkFailState);
	}

    private void addFailureConditions<T>(string failureVarName, dynamic jsonDynamic, Func<string, T> createCondition) where T : QuestFailureNameCheckCondition
    {
        string[] failureConditionNames = GetFromJson.getElementFromJson<string[]>(InteriorDefaultValues.badString, failureVarName, jsonDynamic, InteriorDefaultValues.defaultEmptyStringArray);

        foreach(string name in failureConditionNames)
        {
            T questFailureCondition = createCondition(name);
            failureConditions.Add(questFailureCondition);
            addUniversalQuestStep(questFailureCondition);
        }
    }

    private void addUniversalQuestStep(QuestFailureNameCheckCondition condition)
    {
        if(UniversalQuestStepList.getQuestStep(condition.getFailureQuestStepName(), out QuestStep questStep))
        {
            questStep.parentQuest = this;
            steps[questStep.stepName] = questStep;
        }
    }

    private void checkFailState(object o)
    {
        if(finished || !active || o == null)
        {
            return;
        }

        foreach(QuestFailureCondition failureCondition in failureConditions)
        {
            if(failureCondition.causesFailure(o))
            {
                QuestList.finishQuest(this.title, failureCondition.getFailureQuestStepName(), false);
            }
        }
    }

    public void removeListeners()
    {
        AreaList.AreaBecameHostile.RemoveListener(checkFailState);  
        DeathFlagManager.OnDeathFlagCreated.RemoveListener(checkFailState);
    }

    public List<IDescribable> getActiveQuestSteps()
	{
		List<IDescribable> activeQuestStepsList = new List<IDescribable>();
		
        if(activeQuestStepsDict.Count == 0)
        {
            return activeQuestStepsList;
        }

        for(int index = activeQuestStepsDict.Count-1; index >= 0; index--)
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

[System.Serializable]
public struct QuestListStepWrapper
{
    public string stepName;
    public string mapZone;
    public string mapLocation;
    public string journalDescription;
}