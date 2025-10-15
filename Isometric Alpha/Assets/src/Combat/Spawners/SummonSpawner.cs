using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SummonSpawner : MonoBehaviour
{	
	public static SummonSpawner instance;
	
	public static SummonSpawner getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Instance of SummonSpawner already exists");
		}
		
		instance = this;
	}

	public bool shouldSpawn()
	{
		return State.enemyPackInfo.hasSummonsToSpawn();
	}

	public void spawn()
	{
		if(!shouldSpawn())
		{
			return;
		}
		
		SummonStats[] summonsToSpawn = SummonPackInfoList.getSummonsToSpawn(State.enemyPackInfo.getAllyGroupingKey());
		PartySpawner partySpawner = PartySpawner.getInstance();
		
		foreach(SummonStats summons in summonsToSpawn)
		{
			GridCoords randomOpenSpace = CombatGrid.findRandomOpenSpaceInAllyZone();
			
			if(randomOpenSpace.Equals(GridCoords.getDefaultCoords()))
			{
				return;
			}
			
			partySpawner.spawn(randomOpenSpace, summons.clone());
		}
	}
}
