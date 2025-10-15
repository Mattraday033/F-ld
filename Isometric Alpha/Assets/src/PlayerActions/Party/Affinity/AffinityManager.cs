using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AffinityManager
{
	private const int affinityCap = 99999;
	private static int totalAffinity = 0;
	
	public static void addAffinity(int affinityToAdd)
	{
		if(totalAffinity + affinityToAdd > affinityCap)
		{
			totalAffinity = affinityCap;
		} else
		{
			totalAffinity += affinityToAdd;
		}
	}
	
	//only use when loading a save
	public static void setAffinity(int newAmount)
	{
		if(newAmount > affinityCap)
		{
			totalAffinity = affinityCap;
		} else
		{
			totalAffinity = newAmount;
		}
	}
	
	public static int getTotalAffinity()
	{
		return totalAffinity;
	}
}
