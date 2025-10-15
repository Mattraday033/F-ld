using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StatBoostList
{	

	public static SecondaryStatBoost clayStealthDamBoost = new DexterityBoost(LessonList.clayStealthKey, 0.1f, 0, 0);
	public static SecondaryStatBoost clayFrontalAssaultPenBoost = new WisdomBoost(LessonList.clayFrontalAssaultKey, 5, 0, 0);
	public static SecondaryStatBoost clayKeptNecklaceDiscountBoost = new CharismaBoost(LessonList.clayKeptNecklaceKey, 5.0);
	public static SecondaryStatBoost clayHeroCritBoost = new StrengthBoost(LessonList.clayHeroKey, 0.05, 0, 0.0, 0, "");
	
	public static SecondaryStatBoost nandorZOIStatBoost = new WisdomBoost(NPCNameList.nandor + AllyStats.ZOIStatBoostKey, 0, 0, 0.2, NPCNameList.nandor);
	public static SecondaryStatBoost nandorPersistentInfluence = new WisdomBoost(AllyStats.nandorPersistentInfluenceStatBoostKey, 0, 0, 0.2, NPCNameList.nandor);
	public static SecondaryStatBoost thatchZOIStatBoost = new DexterityBoost(NPCNameList.thatch + AllyStats.ZOIStatBoostKey, 0f, 30, 0, NPCNameList.thatch);
	public static SecondaryStatBoost redStalwartInfluence = new DexterityBoost(AllyStats.redStalwartInfluenceStatBoostKey, 0f, 30, 0, NPCNameList.thatch);
	public static SecondaryStatBoost carterZOIStatBoost = new DexterityBoost(NPCNameList.carter + AllyStats.ZOIStatBoostKey, .1f, 0, 0, NPCNameList.carter);
	public static SecondaryStatBoost carterCleverInfluence = new DexterityBoost(AllyStats.carterCleverInfluenceStatBoostKey, .1f, 0, 0, NPCNameList.carter);
	
	
	public static SecondaryStatBoost getStatBoost(string key)
	{
		switch(key)
		{
			case LessonList.clayKeptNecklaceKey:
				return clayKeptNecklaceDiscountBoost;
			case LessonList.clayFrontalAssaultKey:
				return clayFrontalAssaultPenBoost;
			case LessonList.clayStealthKey:
				return clayStealthDamBoost;
			case LessonList.clayHeroKey:
				return clayHeroCritBoost;
				
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
