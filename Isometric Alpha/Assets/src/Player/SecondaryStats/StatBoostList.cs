using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StatBoostList
{		
	public readonly static SecondaryStatBoost nandorZOIStatBoost = new WisdomBoost(NPCNameList.nandor + AllyStats.ZOIStatBoostKey, 0, 0, 0.2, NPCNameList.nandor);
	public readonly static SecondaryStatBoost nandorPersistentInfluence = new WisdomBoost(AllyStats.nandorPersistentInfluenceStatBoostKey, 0, 0, 0.2, NPCNameList.nandor);
	public readonly static SecondaryStatBoost thatchZOIStatBoost = new DexterityBoost(NPCNameList.thatch + AllyStats.ZOIStatBoostKey, 0f, 30, 0, NPCNameList.thatch);
	public readonly static SecondaryStatBoost redStalwartInfluence = new DexterityBoost(AllyStats.redStalwartInfluenceStatBoostKey, 0f, 30, 0, NPCNameList.thatch);
	public readonly static SecondaryStatBoost carterZOIStatBoost = new DexterityBoost(NPCNameList.carter + AllyStats.ZOIStatBoostKey, .1f, 0, 0, NPCNameList.carter);
	public readonly static SecondaryStatBoost carterCleverInfluence = new DexterityBoost(AllyStats.carterCleverInfluenceStatBoostKey, .1f, 0, 0, NPCNameList.carter);
	
	
	public static SecondaryStatBoost getStatBoost(string key)
	{
		switch(key)
		{				
			case NPCNameList.nandor + AllyStats.ZOIStatBoostKey:
				return nandorZOIStatBoost;
			case AllyStats.nandorPersistentInfluenceStatBoostKey:
				return nandorPersistentInfluence;
			case NPCNameList.thatch + AllyStats.ZOIStatBoostKey:
				return thatchZOIStatBoost;
			case AllyStats.redStalwartInfluenceStatBoostKey:
				return redStalwartInfluence;
			case NPCNameList.carter + AllyStats.ZOIStatBoostKey:
				return carterZOIStatBoost;
			case AllyStats.carterCleverInfluenceStatBoostKey:
				return carterCleverInfluence;

			default: 
				return null;
		}
	}

	public static SecondaryStatBoost[] getAllStatBoosts(string[] keys)
	{
		SecondaryStatBoost[] statBoosts = new SecondaryStatBoost[0];
		
		foreach(string key in keys)
		{
			if(getStatBoost(key) != null)
			{
				statBoosts = Helpers.appendArray<SecondaryStatBoost>(statBoosts, getStatBoost(key));
			}
		}
		
		return statBoosts;
	}
}
