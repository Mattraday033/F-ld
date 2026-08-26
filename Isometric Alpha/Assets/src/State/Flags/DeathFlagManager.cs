using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

public static class DeathFlagManager
{
    public readonly static UnityEvent<string> OnDeathFlagCreated = new UnityEvent<string>();
	public static Dictionary<string, bool> deadNames = new Dictionary<string, bool>();

	public static void addName(string npcName, bool invokeOnDeathFlagCreated = true)
	{
        string newKey = npcName.Replace(" ", "");

        if(deadNames.ContainsKey(newKey))
        {
            return;
        }

		deadNames.Add(newKey, true);

        if(invokeOnDeathFlagCreated)
        {
            OnDeathFlagCreated.Invoke(newKey);
        }
	}
	
	public static bool isDead(string npcName)
	{
		return deadNames.ContainsKey(npcName.Replace(" ", ""));
	}

	public static void resetAllDeadNpcs()
	{
		deadNames = new Dictionary<string, bool>();
	}
	
    public static void resetAllDeadNpcs(List<string> newDeadNames)
    {
        deadNames = new Dictionary<string, bool>();

        foreach(string deadName in newDeadNames)
        {
            addName(deadName, invokeOnDeathFlagCreated: false);
        }
    }

	public static void printAllDeadNames()
	{
		foreach(KeyValuePair<string, bool> kvp in deadNames)
		{
			Debug.Log(kvp.Key + " is dead.");
		}
	}

	public static Story addAllVariables(Story story)
	{
		foreach(KeyValuePair<string, bool> kvp in deadNames)
		{
			if(story.variablesState[InkVariableNameList.deathFlagPrefix + kvp.Key] != null)
			{
				story.variablesState[InkVariableNameList.deathFlagPrefix + kvp.Key] = true;
			}
		}

		return story;
	}

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        resetAllDeadNpcs(blueprint.extractListOfStringsFromJson(blueprint.currentDeathFlags));
    }
}
