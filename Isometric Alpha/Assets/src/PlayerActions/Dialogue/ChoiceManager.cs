using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChoiceManager
{
	public static Dictionary<string, ChoiceKey> choices = new Dictionary<string, ChoiceKey>();

	public static void addChoice(string storyName, string sourcePath)
	{
		ChoiceKey choice = new ChoiceKey(storyName, sourcePath);
		choices[choice.getKey()] = choice;
	}

	public static bool hasBeenChosenBefore(string storyName, string sourcePath)
	{
		return hasBeenChosenBefore(new ChoiceKey(storyName, sourcePath));
	}
	
	public static bool hasBeenChosenBefore(ChoiceKey choice)
	{
		if(choice == null)
		{
			return false;
		}
		
		return choices.ContainsKey(choice.getKey());
	}

	public static void resetChoices()
	{
		choices = new Dictionary<string, ChoiceKey>();
	}

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        choices = blueprint.extractChoicesFromJson();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        LoadSaveFile.OnLoadResetData.AddListener(resetChoices);
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }
}
