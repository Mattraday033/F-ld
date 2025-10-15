using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

public static class QuestList 
{
	private const string pathToQuestFolder = "Quests";
	private const string jsonFileExtension = ".json";
    private const string metaFileExtension = ".meta";

    static QuestList()
	{
		//Do not change quest add order. Would need to either write something to find the quest in the list as if it's not an ordered list (time consuming to write and run)
		//or change back. Otherwise would break the save using the old order.

		buildQuestListFromScratch();
	}
 
	public static Quest convertJsonTextAssetToQuest(TextAsset textAsset)
	{
		string stepName;
		string journalDescription;
		string deadName;
		int firstStep;
		int lastStep;
		bool failOnActivation;
		bool succeedOnActivation;

		//Debug.LogError("filePath = " + filePath);

		string jsonString = textAsset.ToString();

        //Debug.LogError("jsonString = " + jsonString);

		dynamic jsonDynamic = JsonConvert.DeserializeObject<dynamic>(jsonString); 
		
		int stepNum = jsonDynamic["steps"].Count;
		int deathStepNum = jsonDynamic["deathSteps"].Count;
		
		Quest quest = new Quest();
		
		quest.title = jsonDynamic["title"];

		quest.steps = new QuestStep[stepNum];

		for (int i = 0; i < stepNum; i++)
		{
			stepName = jsonDynamic["steps"][i]["stepName"];
			journalDescription = jsonDynamic["steps"][i]["journalDescription"];

			quest.steps[i] = new QuestStep(quest, false, i, stepName, journalDescription);

			if (jsonDynamic["steps"][i]["MapZone"] != null && jsonDynamic["steps"][i]["MapLocation"] != null)
			{
				quest.steps[i].mapZone = jsonDynamic["steps"][i]["MapZone"];
				quest.steps[i].mapLocation = jsonDynamic["steps"][i]["MapLocation"];
			}
		}
		
		quest.deathSteps = new DeathStep[deathStepNum];
		
		for(int i = 0; i < deathStepNum; i++)
		{
			stepName = jsonDynamic["deathSteps"][i]["stepName"];
			journalDescription = jsonDynamic["deathSteps"][i]["journalDescription"];
			deadName = jsonDynamic["deathSteps"][i]["deadName"];
			firstStep = jsonDynamic["deathSteps"][i]["firstStep"];
			lastStep = jsonDynamic["deathSteps"][i]["lastStep"];
			
			try
			{
				failOnActivation = jsonDynamic["deathSteps"][i]["failOnActivation"];
			} catch(Exception e)
			{
				failOnActivation = false;
			}
			
			try
			{
				succeedOnActivation = jsonDynamic["deathSteps"][i]["succeedOnActivation"];
			} catch(Exception e)
			{
				succeedOnActivation = false;
			}
			
			quest.deathSteps[i] = new DeathStep(quest, false, -1, stepName, journalDescription, deadName, firstStep, lastStep, failOnActivation, succeedOnActivation);
		}
		
		return quest;
	}
 
	public static Quest getQuest(string questTitle)
	{
		return State.questDictionary[questTitle];
	}
 
	public static void checkForDeadNames()
	{
		foreach(KeyValuePair<string, Quest> kvp in State.questDictionary)
		{
			Quest quest = kvp.Value;

            if (quest.active && !quest.finished)
			{
				foreach(DeathStep deathStep in quest.deathSteps)
				{
					if(DeathFlagManager.isDead(deathStep.deadName))
					{
						if(deathStep.currentStepOnDeath < 0) //how you tell you shouldn't activate any more death steps. If currentStepOnDeath < 0 but
						{									 //the character is dead, you need to activate. If not, whatever the correct deathstep is 
															 //should already be activated
							deathStep.currentStepOnDeath = quest.currentStepIndex; // set currentStepOnDeath to > 0
							if(deathStep.currentStepOnDeath >= deathStep.firstStep &&
							   deathStep.currentStepOnDeath <= deathStep.lastStep) //if the deathStep.currentStepOnDeath (and thus quest.currentStepIndex)
							{													   //is within the deathsteps purview, activate that deathstep
								deathStep.active = true;
							}
						}
					}
				}
			}
		}
	}
 
	public static void buildQuestListFromScratch()
	{
		//Always add quests to the end of the order.

		State.questDictionary = new Dictionary<string, Quest>();

        TextAsset[] questTextAssets = Resources.LoadAll<TextAsset>(pathToQuestFolder);

		foreach (TextAsset textAsset in questTextAssets)
		{
			Quest quest = convertJsonTextAssetToQuest(textAsset);

			State.questDictionary.Add(quest.getName(), quest);
		}
    }
	
	public static Quest activateQuestStep(string questTitle, int questStepIndex)
	{
		Quest quest = State.questDictionary[questTitle];

		if (quest.steps.Length > questStepIndex &&
			!quest.steps[questStepIndex].active)
		{
			quest.active = true;
			quest.steps[questStepIndex].active = true;
			quest.currentStepIndex = questStepIndex;

			NotificationManager.addToNotificationQueue(quest.steps[questStepIndex]);
			return quest;
		}
		else if (quest.steps.Length > questStepIndex &&
			quest.steps[questStepIndex].active)
		{
			Debug.LogError("Step at index (" + questStepIndex + ") already active for quest: " + questTitle);
		}
		else
		{
			Debug.LogError("Unknown quest: " + questTitle);
		}

		return null;
	}

    public static void finishQuest(string questTitle, int questStepIndex, bool questSuccessful)
    {
		Quest questToFinish = activateQuestStep(questTitle, questStepIndex); 
		questToFinish.finished = true;
        questToFinish.succeeded = questSuccessful;
    }

    public static ArrayList getActiveUnfinishedQuests()
	{
		ArrayList activeUnfinishedQuests = new ArrayList();
		
		foreach(KeyValuePair<string, Quest> kvp in State.questDictionary)
		{
			Quest quest = kvp.Value;

            if (quest.active && !quest.finished)
			{
				activeUnfinishedQuests.Add(quest);
			} 
		}
		
		return activeUnfinishedQuests;
	}

	public static ArrayList getActiveQuestsWithObjectivesInScene(string sceneName)
	{

		ArrayList activeUnfinishedQuests = getActiveUnfinishedQuests();
		ArrayList questStepsInScene = new ArrayList();

		foreach (Quest quest in activeUnfinishedQuests)
		{
			QuestStep step = quest.getCurrentQuestStep();

			if (step.hasTargetLocation() && step.mapLocation.Equals(sceneName))
			{
				questStepsInScene.Add(step);
			}
		}

		return questStepsInScene;
	}

	public static ArrayList getActiveUnfinishedQuestStepsInZone(string zoneKey)
	{
		ArrayList activeUnfinishedQuests = getActiveUnfinishedQuests();
		ArrayList activeUnfinishedQuestsInZone = new ArrayList();

		foreach (Quest quest in activeUnfinishedQuests)
		{
			if (quest.getCurrentQuestStep().hasTargetLocation() &&
				quest.getCurrentQuestStep().mapZone.Equals(zoneKey))
			{
				activeUnfinishedQuestsInZone.Add(quest.getCurrentQuestStep());				
			}
		}

		return activeUnfinishedQuestsInZone;
	}

	public static int getNumberOfActiveUnfinishedQuestsInZone(string zoneKey)
	{
		return getActiveUnfinishedQuestStepsInZone(zoneKey).Count;
	}

	public static ArrayList getActiveFinishedQuests()
	{
		ArrayList activeQuests = new ArrayList();

		foreach (KeyValuePair<string, Quest> kvp in State.questDictionary)
		{
			Quest quest = kvp.Value;

			if (quest.active && quest.finished)
			{
				activeQuests.Add(quest);
			}
		}

		return activeQuests;
	}

	public static ArrayList getActiveQuests()
	{
		ArrayList activeQuests = new ArrayList();

		activeQuests.AddRange(getActiveUnfinishedQuests());
		activeQuests.AddRange(getActiveFinishedQuests());

		return activeQuests;
	}

	public static string checkForQuestNameChange(string questName) //checks if a quest name has been changed and if so, returns the name it was changed to.
																   //otherwise, returns the old name as is.
	{															   //no quests have been changed yet, so empty list
		switch (questName)
		{
			default:
				return questName;
		}
	}
}
