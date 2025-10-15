using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ActivatedPassiveTraitManager : MonoBehaviour
{
	private static ActivatedPassiveTraitManager instance;
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Instance of ActivatedPassiveTraitManager already exists");
		}
		
		instance = this;
	}
	
	public static ActivatedPassiveTraitManager getInstance()
	{
		return instance;
	}
	
	public void addEquippedPassiveTraits()
	{
		ArrayList allAllies = CombatGrid.getAllAliveAllyCombatants();

        foreach (Stats ally in allAllies)
		{
			ally.addEquippedPassiveTraits();
		}
	}
	
	public static void removeAllTraits()
	{
		ArrayList allAllies = CombatGrid.getAllAliveAllyCombatants();
		
		foreach(Stats ally in allAllies)
		{
			ally.removeAllTraits();
		}
	}

}
