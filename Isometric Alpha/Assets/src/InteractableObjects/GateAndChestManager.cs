using Ink.Runtime;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GateAndChestManager
{
	public const bool resetDictionary = true;

    private static Dictionary<string, bool> openedGatesAndChests;

    [RuntimeInitializeOnLoadMethod]
    public static void resetGatesAndChests()
    {
        openedGatesAndChests = new Dictionary<string, bool>();
    }

    public static void resetGatesAndChests(Dictionary<string, bool> newDict)
    {
        openedGatesAndChests = newDict;
    }

    public static void addKey(string key)
    {
        if (key == null || key.Length == 0)
        {
            return;
        }

        openedGatesAndChests[key] = true;
    }

	public static bool hasBeenOpened(string key)
	{
		return openedGatesAndChests.ContainsKey(key);
	}

    public static int getKeyCount()
    {
        return openedGatesAndChests.Count;    
    }

    public static FlagWrapper[] getAllGateAndChestFlagWrappers()
    {
        return FlagWrapper.getAllFlagsInDictionary(openedGatesAndChests);
    }

	public static Story addAllVariables(Story story)
	{
        foreach (KeyValuePair<string, bool> kvp in openedGatesAndChests)
        {
			string keyWithoutDashes = kvp.Key.Replace("-", "");

            if (story.variablesState["gateFlag" + keyWithoutDashes] != null)
			{
				story.variablesState["gateFlag" + keyWithoutDashes] = true;
			}
		}

		return story;
	}
}
