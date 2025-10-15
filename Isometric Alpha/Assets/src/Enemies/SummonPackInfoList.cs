using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SummonPackInfoList
{
    
	public static Dictionary<string, SummonStats[]> allSummonPackInfo;
	
	public const string guardOverseerGameObjectKey = "GuardOverseer";
	public const string guardsGameObjectKey = "CampGuard";
	
	public const string guardsMineLvl3Key = "mineLvl3GuardsInParty";
	public const string smallSlaveRiotKey = "kastorStartedRevolt";
	public const string largeSlaveRiotKey = "convincedSlavesToHelpYou";
	
	static SummonPackInfoList()
	{	
		allSummonPackInfo = new Dictionary<string, SummonStats[]>();
		
		SummonStats overseerGaspar = new SummonStats(Resources.Load<GameObject>(guardOverseerGameObjectKey), 
													 guardOverseerGameObjectKey, "Overseer Gáspár", 20, 65, AbilityList.summonsWhipAttackKey, TraitList.predatory.getName());
		
		SummonStats guardReka = new SummonStats(Resources.Load<GameObject>(guardsGameObjectKey), 
												guardsGameObjectKey, "Guard Réka", 20, 40, true);
		
		SummonStats guardVirag = new SummonStats(Resources.Load<GameObject>(guardsGameObjectKey), 
												 guardsGameObjectKey, "Guard Virág", 20, 40, true);
												 
		allSummonPackInfo.Add(guardsMineLvl3Key, new SummonStats[]{overseerGaspar, guardReka, guardVirag});
		
		SummonStats slaveRioter = new SummonStats(Resources.Load<GameObject>(guardsGameObjectKey), 
												  guardsGameObjectKey, "Slave Rioter", 5, 5, true);
		
		allSummonPackInfo.Add(smallSlaveRiotKey, new SummonStats[]{slaveRioter, slaveRioter, slaveRioter, slaveRioter});
		
		allSummonPackInfo.Add(largeSlaveRiotKey, new SummonStats[]{slaveRioter, slaveRioter, slaveRioter, slaveRioter,
																   slaveRioter, slaveRioter, slaveRioter, slaveRioter,
																   slaveRioter, slaveRioter, slaveRioter, slaveRioter,
																   slaveRioter, slaveRioter, slaveRioter, slaveRioter});
	}
	
	public static SummonStats[] getSummonsToSpawn(string key)
	{
		return allSummonPackInfo[key];
	}
	
}
