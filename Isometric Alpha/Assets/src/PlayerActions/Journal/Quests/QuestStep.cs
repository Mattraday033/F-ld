using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJournalSubcategory : IDescribable
{
    
}

[System.Serializable]
public class QuestStep : IJournalSubcategory, IDescribableInBlocks, ISortable, ICloneable
{
	private const string questCompletedPrefix = "Quest Complete: ";
	private const string questFailedPrefix = "Quest Failed: ";
	private const string questUpdatedPrefix = "Quest Updated: ";

	public Quest parentQuest;
	public bool active
    {
        get;
        private set;
    }
    public int activationIndex;
	public string stepName;
	public string journalDescription;

	public string mapZone;
	public string mapLocation;

	public QuestStep(Quest parentQuest, QuestListStepWrapper wrapper)
	{
		this.parentQuest = parentQuest;

        this.active = false;

        this.stepName = wrapper.stepName;
        this.journalDescription = wrapper.journalDescription.Replace(Constants.boldTextStartCaps, Constants.boldTextStart).Replace(Constants.boldTextEndCaps, Constants.boldTextEnd);
        this.mapZone = wrapper.mapZone;
        this.mapLocation = wrapper.mapLocation;

        this.activationIndex = -1;
	}

	public QuestStep(Quest parentQuest, bool active, string stepName, string journalDescription)
	{
		this.parentQuest = parentQuest;
		this.active = active;
		this.stepName = stepName;
		this.journalDescription = journalDescription.Replace(Constants.boldTextStartCaps, Constants.boldTextStart).Replace(Constants.boldTextEndCaps, Constants.boldTextEnd);

        activationIndex = -1;
	}

	public bool hasTargetLocation()
	{
		return mapZone != null && mapLocation != null;
	}

	//IDescribable Methods
	public string getName()
	{
		return stepName;
	}

	public bool ineligible()
	{
		return activationIndex < parentQuest.activeQuestStepsDict.Count-1;
	}

	public GameObject getRowType(RowType rowType)
	{
		switch (rowType)
		{
			case RowType.Map:
				return Resources.Load<GameObject>(PrefabNames.mapQuestObjectiveRow);
			case RowType.MapWithoutHover:
				return Resources.Load<GameObject>(PrefabNames.mapQuestObjectiveRowWithoutHover);
			default:
				return Resources.Load<GameObject>(PrefabNames.glossaryCategoryRow);
		}
	}

	public GameObject getDescriptionPanelFull()
	{
		return getDescriptionPanelFull(PanelType.Standard);
	}

	public GameObject getDescriptionPanelFull(PanelType type)
	{
		string panelTypeName = "";

		switch (type)
		{
			case PanelType.Notification:
                panelTypeName = PrefabNames.questStepNotificationDescriptionPanel;
				break;
			default:
				panelTypeName = PrefabNames.writtenGlossaryEntryFull;
				break;
		}

		return DescriptionPanel.getDescriptionPanel(panelTypeName);
	}

    public void setActiveStatus(bool status)
    {
        if(status && !active)
        {
            this.activationIndex = parentQuest.getNextActivationIndex();
        } else if(!status)
        {
            return;
        }

        active = status;

        if(active)
        {
            parentQuest.activeQuestStepsDict[activationIndex] = this;
        }
    }

    public void setActiveStatus(bool status, int activationIndex)
    {
        active = status;
        this.activationIndex = activationIndex;

        if(active)
        {
            parentQuest.activeQuestStepsDict[activationIndex] = this;
        }
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

		// DescriptionPanel.setText(panel.nameText, parentQuest.getName());

		if (parentQuest.finished)
		{
			if (parentQuest.succeeded)
			{
				DescriptionPanel.setText(panel.notificationNameText, questCompletedPrefix + parentQuest.getName());
			}
			else
			{
				DescriptionPanel.setText(panel.notificationNameText, questFailedPrefix + parentQuest.getName());
			}

		}
		else
		{
			DescriptionPanel.setText(panel.notificationNameText, questUpdatedPrefix + parentQuest.getName());
		}

		DescriptionPanel.setText(panel.secondaryNameText, DialogueList.scrubNameOfEndNumbers(getName()));
		DescriptionPanel.setText(panel.loreDescriptionText, getJournalDescriptionForDisplay());
	}

    private string getJournalDescriptionForDisplay()
    {
        SettingOption[] boldImportantTextSO = GameplaySettingsList.boldImportantQuestText.settingOptions;
        SettingOption[] importantTextOnlySO = GameplaySettingsList.showOnlyImportantQuestText.settingOptions;

        if(!journalDescription.Contains(Constants.boldTextStart))
        {
            return Constants.boldTextStart + journalDescription + Constants.boldTextEnd;
        }

        if(importantTextOnlySO[Constants.onSettingIndex].set)
        {
            string output = "";
            string[] importantSectionStarts = journalDescription.Split(Constants.boldTextStart);

            foreach(string section in importantSectionStarts)
            {
                string[] possibleImportantSection = section.Split(Constants.boldTextEnd);

                if(possibleImportantSection.Length > 1)
                {
                    string importantSection = possibleImportantSection[Constants.indexZero];
                    importantSection = char.ToUpper(importantSection[0]) + importantSection.Substring(1);

                    if(!importantSection[importantSection.Length-1].Equals(' '))
                    {
                        importantSection += ' ';
                    }

                    output += importantSection;
                }
            }

            if(boldImportantTextSO[Constants.onSettingIndex].set)
            {
                return Constants.boldTextStart + output + Constants.boldTextEnd;
            } else
            {
                return output;
            }

        } else
        {
            if(boldImportantTextSO[Constants.onSettingIndex].set)
            {
                return journalDescription;
            } else
            {
                return journalDescription.Replace(Constants.boldTextStart, "").Replace(Constants.boldTextEnd, "");
            }
        }
    }

	public void describeSelfRow(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, DialogueList.scrubNameOfEndNumbers(getName()));
		DescriptionPanel.setText(panel.secondaryNameText, parentQuest.getName());
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
		return true;
	}
	
	public bool buildableWithBlocksRows()
    {
        return true;
    }
	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(parentQuest.getName()));
		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(DialogueList.scrubNameOfEndNumbers(getName())));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));
		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, getJournalDescriptionForDisplay()));

		return buildingBlocks;
	}

    public bool requiresInspectNode()
    {
        return false;
    }

    #region ISortable

    public int getQuantity()
    {
        return 1;
    }

    public int getWorth()
    {
        return 1;
    }

    public string getType()
    {
        return "QuestStep";
    }

    public string getSubtype()
    {
        return "QuestStep";
    }

    public int getLevel()
    {
        return 1;
    }

    public int getNumber()
    {
        if(parentQuest.finished && !parentQuest.succeeded)
        {
            return Constants.sizeFour;
        } else if(parentQuest.finished && parentQuest.succeeded)
        {
            return Constants.sizeOne;
        } else
        {
            return Constants.sizeTwo;
        }
    }

    #endregion

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public QuestStep clone()
    {
        return Clone() as QuestStep;
    }
}
