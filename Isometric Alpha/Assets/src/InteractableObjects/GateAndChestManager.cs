using Ink.Runtime;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GateAndChestManager
{
	public const bool resetDictionary = true;

	private static Dictionary<string, bool> openedGatesAndChests = new Dictionary<string, bool>();

	public static void addKey(string key)
	{
		if (key == null || key.Length == 0)
		{
			return;
		}

		openedGatesAndChests = Helpers.addToDictionary<string>(openedGatesAndChests, key);
	}

	public static void addKeys(ArrayList keys, bool resetFirst)
	{
		if (resetFirst)
		{
			resetGatesAndChests();
		}

		foreach (string key in keys)
		{
			addKey(key);
		}
	}

	public static bool hasBeenOpened(string key)
	{
		return openedGatesAndChests.ContainsKey(key);
	}

    public static int getKeyCount()
    {
        return openedGatesAndChests.Count;    
    }

    public static string[] getSaveData()
    {
        return Helpers.getAllKeys<bool>(GateAndChestManager.openedGatesAndChests);
    }

    public static void resetGatesAndChests()
    {
        openedGatesAndChests = new Dictionary<string, bool>();
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
