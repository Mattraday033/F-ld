using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MetFlagManager
{
	public static Dictionary<string, bool> metNames = new Dictionary<string, bool>();

	public static void addName(string npcName)
	{
		metNames.Add(npcName, true);
	}
	
	public static bool metBefore(string npcName)
	{
		return metNames.ContainsKey(npcName);
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
}
