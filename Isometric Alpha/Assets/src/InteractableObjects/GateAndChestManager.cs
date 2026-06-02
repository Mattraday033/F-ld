using Ink.Runtime;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GateAndChestManager
{
    public readonly static UnityEvent OnGateKeyAdd = new UnityEvent();

    public static bool preventSFX = false;

    private static Dictionary<string, bool> openedGatesAndChests = new Dictionary<string, bool>();

    [RuntimeInitializeOnLoadMethod]
    public static void resetGatesAndChests()
    {
        openedGatesAndChests = new Dictionary<string, bool>();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        resetGatesAndChests(FlagWrapper.convertFlagWrapperListToDictionary(blueprint.currentChestAndGateFlags));
    }

    public static void resetGatesAndChests(Dictionary<string, bool> newDict)
    {
        openedGatesAndChests = newDict;
    }

    public static void addKey(string key, bool invoke = true)
    {
        if (openedGatesAndChests.ContainsKey(key))
        {
            return;
        }

        openedGatesAndChests[key] = true;

        if(invoke)
        {
            OnGateKeyAdd.Invoke();
        }
    }

    public static void removeKey(string key, bool invoke = true)
    {
        if (!openedGatesAndChests.ContainsKey(key))
        {
            return;
        }

        openedGatesAndChests.Remove(key);

        if(invoke)
        {
            OnGateKeyAdd.Invoke();
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

    public static FlagWrapper[] getAllGateAndChestFlagWrappers()
    {
        // foreach (KeyValuePair<string, bool> kvp in openedGatesAndChests)
        // {
        //     Debug.LogError("kvp.Key = " + kvp.Key);
        // }

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
