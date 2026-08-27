using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ink.Runtime;
using Newtonsoft.Json;

public static class Flags 
{

	private static Dictionary<string, bool> flags = new Dictionary<string, bool>();

    static Flags()
    {
        flags[FlagNameList.newGameFlagName] = true; //is a new game 

    }

	public static bool getFlag(string flagName)
	{
		if (!flags.ContainsKey(flagName))
		{
			flags.Add(flagName, false);
		}
            
        return flags[flagName];
	}

	public static void setFlag(string flagName, bool flagStatus)
	{
		if (!flags.ContainsKey(flagName))
		{
			flags.Add(flagName, flagStatus);
		}
		else
		{
			flags[flagName] = flagStatus;
		}
	}

    public static void printAll()
    {   // Get and display values  
        //Dictionary<string,bool>.KeyCollection keys = flags.Keys;  
        //Dictionary<string,bool>.ValueCollection values = flags.Values;  
        foreach (KeyValuePair<string, bool> kvp in flags)
        {
            Debug.Log(kvp.Key + " is " + kvp.Value);
        }
    }

	public static Story addAllVariables(Story story)
	{
		return addAllFlagVariables(flags, story);
	}

	public static Story addAllFlagVariables(Dictionary<string, bool> flagDict, Story story, string flagPrefix = "", bool spacesAllowed = false)
	{
		foreach (KeyValuePair<string, bool> kvp in flagDict)
		{
            string key = kvp.Key;

            if(spacesAllowed)
            {
                key = key.Replace(" ", "_");
            } 

            key = flagPrefix + key;

            InkVariableNameList.setStoryVariable(story, key, kvp.Value);
		}

		return story;
	}

	public static void overwriteFlags(Dictionary<string, bool> newFlags)
	{
		flags = new Dictionary<string, bool>();

		if (newFlags == null)
		{
			return;
		}

		foreach (KeyValuePair<string, bool> flag in newFlags)
		{
			flags[flag.Key] = newFlags[flag.Key];
		}
	}
	//assumes string is a json that can be deserialized into a Dictionary<string,bool>();
	public static void overwriteFlags(string newFlags)
	{
		overwriteFlags(JsonConvert.DeserializeObject<Dictionary<string, bool>>(newFlags));
	}

	public static void resetAllFlags()
	{
		resetAllFlags(false);
	}

	public static void resetAllFlags(bool newGame)
	{
		foreach (var key in flags.Keys.ToList())
		{
			flags[key] = false;
		}

		flags["newGame"] = newGame;
	}

	public static string getFlagsForSave()
	{
		return JsonConvert.SerializeObject(flags, Formatting.Indented);
	}

	public static bool isInNewGameMode()
	{
		return getFlag(FlagNameList.newGameFlagName);
	}

	public static void exitNewGameMode()
	{
		flags[FlagNameList.newGameFlagName] = false;
	}

	public static void stopPartyTrainSpawning()
	{
		flags["disablePartyTrain"] = true;
	}

	public static void allowPartyTrainSpawning()
	{
		flags["disablePartyTrain"] = false;
	}

	public static bool shouldStopPartyTrainSpawning()
	{
		if (flags["disablePartyTrain"])
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public static string getStatTutorialFlag(AllyStats playerStats) //only use when starting new game
    {
        PrimaryStat chosenStat = playerStats.getHighestPrimaryStats()[0];

        switch (chosenStat)
        {
            case PrimaryStat.Strength:
                return "choseStrengthAtStart";
            case PrimaryStat.Dexterity:
                return "choseDexterityAtStart";
            case PrimaryStat.Wisdom:
                return "choseWisdomAtStart";
            default:
                return "choseCharismaAtStart";
        }
    }

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        overwriteFlags(blueprint.currentFlags);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        LoadSaveFile.OnLoadResetData.AddListener(resetAllFlags);
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

}
