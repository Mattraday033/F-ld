using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MetFlagManager
{
	public static ArrayList metNames = new ArrayList();

	public static void addName(string npcName)
	{
		metNames.Add(npcName);
	}
	
	public static bool metBefore(string npcName)
	{
		return metNames.Contains(npcName);
	}

	public static void resetAllMetNpcs()
	{
		metNames = new ArrayList();
	}
}
