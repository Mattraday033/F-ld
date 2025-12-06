using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EquippedPassiveTraitManager : MonoBehaviour
{
	private static EquippedPassiveTraitManager instance;
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Instance of EquippedPassiveTraitManager already exists");
		}
		
		instance = this;
	}
	
	public static EquippedPassiveTraitManager getInstance()
	{
		return instance;
	}
	
	public void addEquippedPassiveTraits()
	{
		List<Stats> allAllies = CombatGrid.getAllAliveAllyCombatants();

        foreach (Stats ally in allAllies)
		{
			ally.addEquippedPassiveTraits();
		}
	}
	
	public static void removeAllTraits()
	{
		List<Stats> allAllies = CombatGrid.getAllAliveAllyCombatants();
		
		foreach(Stats ally in allAllies)
		{
			ally.removeAllTraits();
		}
	}

}
