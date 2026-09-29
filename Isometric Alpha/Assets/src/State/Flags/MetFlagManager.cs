using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MetFlagManager
{
	public static Dictionary<string, bool> metNames = new Dictionary<string, bool>();

	public static void addName(string displayName)
	{
		metNames.Add(displayName, true);
	}
	
	public static bool metBefore(string displayName)
	{
		return metNames.ContainsKey(displayName);
	}

	public static void resetAllMetNpcs()
	{
		metNames = new Dictionary<string, bool>();
	}

    public static void resetAllMetNpcs(List<string> newMetNPCNames)
    {
        metNames = new Dictionary<string, bool>();

        foreach(string metName in newMetNPCNames)
        {
            addName(metName);
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        resetAllMetNpcs(blueprint.extractListOfStringsFromJson(blueprint.currentMetFlags));
    }
}
