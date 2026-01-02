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

    private const string titleVarName = "title";
    public const string stepsVarName = "steps";
    private const string deathStepsVarName = "deathSteps";
    private const string succeedOnActivationVarName = "succeedOnActivation";
    private const string stepNameVarName = "stepName";
    private const string journalDescriptionVarName = "journalDescription";
    private const string mapZoneVarName = "MapZone";
    private const string mapLocationVarName = "MapLocation";
    private const string failureOnAreaHostilityVarName = "failureOnAreaHostility";

	private static Dictionary<string, Quest> questDict = new Dictionary<string, Quest>();

    static QuestList()
	{
		//Do not change quest add order. Would need to either write something to find the quest in the list as if it's not an ordered list (time consuming to write and run)
		//or change back. Otherwise would break the save using the old order.

		buildQuestListFromScratch();
	}
 
	public static Quest convertJsonTextAssetToQuest(TextAsset textAsset)
	{



		// string stepName;
		// string journalDescription;

		// //Debug.LogError("filePath = " + filePath);

		// string jsonString = textAsset.ToString();

        // //Debug.LogError("jsonString = " + jsonString);

		// dynamic jsonDynamic = JsonConvert.DeserializeObject<dynamic>(jsonString); 
		
		// int stepNum = jsonDynamic[stepsVarName].Count;
		
		// Quest quest = new Quest();
		
		// quest.title = jsonDynamic[titleVarName];

		// quest.steps = new Dictionary<string, QuestStep>();

		// for (int i = 0; i < stepNum; i++)
		// {
		// 	stepName = jsonDynamic[stepsVarName][i][stepNameVarName];
		// 	journalDescription = jsonDynamic[stepsVarName][i][journalDescriptionVarName];

		// 	quest.steps[stepName] = new QuestStep(quest, false, stepName, journalDescription);

		// 	if (jsonDynamic[stepsVarName][i][mapZoneVarName] != null && jsonDynamic[stepsVarName][i][mapLocationVarName] != null)
		// 	{
		// 		quest.steps[stepName].mapZone = jsonDynamic[stepsVarName][i][mapZoneVarName];
		// 		quest.steps[stepName].mapLocation = jsonDynamic[stepsVarName][i][mapLocationVarName];
		// 	}
		// }
		
		// quest.deathSteps = new Dictionary<string, DeathStep>();
        string jsonString = textAsset.ToString();
        Quest quest = new Quest(jsonString);

		return quest;
	}
 
	public static Quest getQuest(string questTitle)
	{
		return questDict[questTitle];
	}
 
    public static QuestWrapper[] getQuestWrappers()
    {
        List<QuestWrapper> questWrappers = new List<QuestWrapper>();

        foreach(KeyValuePair<string, Quest> kvp in questDict)
        {
            questWrappers.Add(new QuestWrapper(kvp.Value));
        }

        return questWrappers.ToArray();   
    }

	public static void checkForDeadNames()
	{
		foreach(KeyValuePair<string, Quest> kvpQuest in questDict)
		{
			Quest quest = kvpQuest.Value;

            if (quest.active && !quest.finished)
			{
				foreach(KeyValuePair<string, DeathStep> kvpDeathStep in quest.deathSteps)
				{
					if(DeathFlagManager.isDead(kvpDeathStep.Value.deadName))
					{
						if(kvpDeathStep.Value.currentStepOnDeath < 0) //how you tell you shouldn't activate any more death steps. If currentStepOnDeath < 0 but
						{									 //the character is dead, you need to activate. If not, whatever the correct kvpDeathStep.Value is 
															 //should already be activated
							if(kvpDeathStep.Value.currentStepOnDeath >= kvpDeathStep.Value.firstStep &&
							   kvpDeathStep.Value.currentStepOnDeath <= kvpDeathStep.Value.lastStep) //if the kvpDeathStep.Value.currentStepOnDeath (and thus quest.currentStepIndex)
							{													   //is within the kvpDeathStep.Values purview, activate that kvpDeathStep.Value
								kvpDeathStep.Value.setActiveStatus(true);
							}
						}
					}
				}
			}
		}
	}
 
    private static void removeAllListeners()
    {
        if(questDict == null || questDict.Count <= 0)
        {
            return;
        }

        foreach(KeyValuePair<string, Quest> kvp in questDict)
        {
            kvp.Value.removeListeners();
        }
    }

	public static void buildQuestListFromScratch()
	{
        removeAllListeners();

		questDict = new Dictionary<string, Quest>();

        TextAsset[] questTextAssets = Resources.LoadAll<TextAsset>(pathToQuestFolder);

		foreach (TextAsset textAsset in questTextAssets)
		{
			Quest quest = convertJsonTextAssetToQuest(textAsset);

			questDict.Add(quest.getName(), quest);
		}
    }
	
	public static Quest activateQuestStep(string questTitle, string questStepName)
	{
		Quest quest = questDict[questTitle];

		if (quest.steps.ContainsKey(questStepName) &&
			!quest.steps[questStepName].active)
		{
            quest.steps[questStepName].setActiveStatus(true);

            if(!quest.finished)
            {
                quest.active = true;
                NotificationManager.addToNotificationQueue(quest.steps[questStepName]);
            }

			return quest;
		}
		else if (quest.steps.ContainsKey(questStepName) &&
			quest.steps[questStepName].active)
		{
			Debug.LogError("Step (" + questStepName + ") already active for quest: " + questTitle);
		}
		else
		{
			Debug.LogError("Unknown quest: " + questTitle);
		}

		return null;
	}

    public static void finishQuest(string questTitle, string questStepName, bool questSuccessful)
    {
		Quest questToFinish = activateQuestStep(questTitle, questStepName); 
		questToFinish.finished = true;
        questToFinish.succeeded = questSuccessful;
    }

    public static List<Quest> getActiveUnfinishedQuests()
	{
		List<Quest> activeUnfinishedQuests = new List<Quest>();
		
		foreach(KeyValuePair<string, Quest> kvp in questDict)
		{
			Quest quest = kvp.Value;

            if (quest.active && !quest.finished)
			{
				activeUnfinishedQuests.Add(quest);
			} 
		}
		
		return activeUnfinishedQuests;
	}

	public static List<QuestStep> getActiveQuestStepsWithObjectivesInScene(string sceneName)
	{

		List<Quest> activeUnfinishedQuests = getActiveUnfinishedQuests();
		List<QuestStep> questStepsInScene = new List<QuestStep>();

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

	public static List<QuestStep> getActiveUnfinishedQuestStepsInZone(string zoneKey)
	{
		List<Quest> activeUnfinishedQuests = getActiveUnfinishedQuests();
		List<QuestStep> activeUnfinishedQuestsInZone = new List<QuestStep>();

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

	public static List<IDescribable> getActiveFinishedQuests()
	{
		List<IDescribable> activeQuests = new List<IDescribable>();

		foreach (KeyValuePair<string, Quest> kvp in questDict)
		{
			Quest quest = kvp.Value;

			if (quest.active && quest.finished)
			{
				activeQuests.Add(quest);
			}
		}

		return activeQuests;
	}

	public static List<IDescribable> getActiveQuests()
	{
		List<IDescribable> activeQuests = new List<IDescribable>();

		activeQuests.AddRange(getActiveUnfinishedQuests());
		activeQuests.AddRange(getActiveFinishedQuests());

		return activeQuests;
	}

	public static void resetAndOverwriteQuestDictionary(QuestWrapper[] questWrappers)
	{
		buildQuestListFromScratch();

		foreach (QuestWrapper questWrapper in questWrappers)
		{
            if(questDict.ContainsKey(questWrapper.title))
            {
                questDict[questWrapper.title] = questWrapper.unwrapQuest(questDict[questWrapper.title]);
            }
		}
	}
}
