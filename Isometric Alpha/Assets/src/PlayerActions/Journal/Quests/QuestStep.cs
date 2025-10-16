using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStep : IDescribable, IDescribableInBlocks
{
	private const string questCompletedPrefix = "Quest Complete: ";
	private const string questFailedPrefix = "Quest Failed: ";
	private const string questUpdatedPrefix = "Quest Updated: ";

	public Quest parentQuest;
	public bool active;
	public int stepIndex;
	public string stepName;
	public string journalDescription;

	public string mapZone;
	public string mapLocation;

	public QuestStep(Quest parentQuest, bool active, int stepIndex, string stepName, string journalDescription)
	{
		this.parentQuest = parentQuest;
		this.active = active;
		this.stepIndex = stepIndex;
		this.stepName = stepName;
		this.journalDescription = journalDescription;
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
		return stepIndex < parentQuest.currentStepIndex;
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
				return Resources.Load<GameObject>(PrefabNames.glossaryEntryRow);
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
				if (NotificationPopUpWindow.pagesFadeIn)
				{
					panelTypeName = PrefabNames.questStepNotificationDescriptionPanel;
				}
				else
				{
					panelTypeName = PrefabNames.questStepNotificationDescriptionPanelNoFadeIn;
				}

				break;
			default:
				panelTypeName = PrefabNames.writtenGlossaryEntryFull;
				break;
		}

		return DescriptionPanel.getDescriptionPanel(panelTypeName);
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

		DescriptionPanel.setText(panel.nameText, parentQuest.getName());

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

		DescriptionPanel.setText(panel.secondaryNameText, getName());
		DescriptionPanel.setText(panel.loreDescriptionText, journalDescription);
	}

	public void describeSelfRow(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, getName());
		DescriptionPanel.setText(panel.secondaryNameText, parentQuest.getName());
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
		return true;
	}
	
	public bool buildableWithBlocksRows()
    {
        return true;
    }
	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, parentQuest.getName()));
		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));
		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, journalDescription));

		return buildingBlocks;
	}
}
