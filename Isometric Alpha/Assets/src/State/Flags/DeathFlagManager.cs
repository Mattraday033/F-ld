using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public static class DeathFlagManager
{
	public static ArrayList deadNames = new ArrayList();

	public static void addName(string npcName)
	{
		deadNames.Add(npcName.Replace(" ", ""));
	}
	
	public static bool isDead(string npcName)
	{
		return deadNames.Contains(npcName.Replace(" ", ""));
	}

	public static void resetAllDeadNpcs()
	{
		deadNames = new ArrayList();
	}
	
	public static void printAllDeadNames()
	{
		foreach(string deadName in deadNames)
		{
			Debug.Log(deadName + " is dead.");
		}
	}
	
	public static Story addAllVariables(Story story)
	{
		foreach(string deadName in deadNames)  
		{  
			if(story.variablesState["deathFlag" + deadName] != null)
			{
				story.variablesState["deathFlag" + deadName] = true;
			}
		}

		return story;
	}
	
}
