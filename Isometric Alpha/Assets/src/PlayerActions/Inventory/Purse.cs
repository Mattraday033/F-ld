using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public static class Purse
{
	public const string moneySymbol = "g";
	
	private static int coins = 0;

	public static bool canAfford(int cost)
	{
		return coins >= Math.Abs(cost);
	}

	public static void addCoins(string newCoins)
	{
		addCoins(int.Parse(newCoins));
	}
	
	public static void addCoins(int newCoins)
	{
		coins += Math.Abs(newCoins);
	}

	public static void removeCoins(string cost)
	{
		removeCoins(int.Parse(cost));
		
	}

	public static void removeCoins(int cost)
	{
		if(cost > getCoinsInPurse())
		{
			throw new IOException("cost > coinsInPurse()");
		}
		
		coins -= Math.Abs(cost);
	}

	public static int getCoinsInPurse()
	{
		return coins;
	}

    public static string getCoinsInPurseForDisplay()
    {
        return coins + moneySymbol;
    }

    //only to be used when loading the game
    public static void setCoinsInPurse(int newCoinsInPurse)
	{
		coins = newCoinsInPurse;
	}

	public static Story addCoinsToStory(Story story)
	{
        if (story.variablesState["coins"] != null)
        {
            story.variablesState["coins"] = coins;
        }

        return story;
	}

}
