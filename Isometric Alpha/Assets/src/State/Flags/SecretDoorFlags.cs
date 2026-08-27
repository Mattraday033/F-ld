using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class SecretDoorFlags
{
    public readonly static UnityEvent<string> OnSecretDoorDiscovery = new UnityEvent<string>();

    private static Dictionary<string, bool> secretDoorFlags;

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        setFromSaveData(blueprint.secretDoors);
    }

    public static void addSecretDoorFlag(string secretDoorKey)
    {
        if(secretDoorKey.Length > 0 && !secretDoorFlags.ContainsKey(secretDoorKey))
        {
            secretDoorFlags.Add(secretDoorKey, true);
        }

        OnSecretDoorDiscovery.Invoke(secretDoorKey);
    }
    
    public static bool secretDoorHasBeenDiscovered(string secretDoorKey)
    {
        return secretDoorFlags.ContainsKey(secretDoorKey);
    }

    public static string[] getSecretDoorKeys()
    {
        List<string> secretDoorKeys = new List<string>();

        foreach (KeyValuePair<string, bool> kvp in secretDoorFlags)
        {
            secretDoorKeys.Add(kvp.Key);
        }

        return secretDoorKeys.ToArray();
    }

    public static void setFromSaveData(string[] secretDoorKeys)
    {
        secretDoorFlags = new Dictionary<string, bool>();

        foreach (string key in secretDoorKeys)
        {
            secretDoorFlags.Add(key, true);
        }
    }

	public static Story addAllVariables(Story story)
	{
		return Flags.addAllFlagVariables(secretDoorFlags, 
                                            story, 
                                            flagPrefix: InkVariableNameList.secretDoorFlagPrefix, 
                                            spacesAllowed: true);
	}


    [RuntimeInitializeOnLoadMethod]
    private static void inti()
    {
        secretDoorFlags = new Dictionary<string, bool>();

        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }
}
