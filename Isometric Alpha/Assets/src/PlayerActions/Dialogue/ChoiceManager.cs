using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChoiceManager // KastorPlan // 3a.0.2.b.1.8
{
	public static Dictionary<string, ChoiceKey> choices;

	public static void addChoice(string storyName, string sourcePath)
	{
		ChoiceKey choice = new ChoiceKey(storyName, sourcePath);
		choices[choice.getKey()] = choice;
	}

    public static void removeChoice(string storyName, string sourcePath)
    {
        ChoiceKey choice = new ChoiceKey(storyName, sourcePath);

        if(choices.ContainsKey(choice.getKey()))
        {
            choices.Remove(choice.getKey());
        }
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
		choices = new Dictionary<string, ChoiceKey>();
        LoadSaveFile.OnLoadResetData.AddListener(resetChoices);
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }
}
