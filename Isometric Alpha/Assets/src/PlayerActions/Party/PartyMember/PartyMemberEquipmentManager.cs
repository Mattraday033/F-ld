using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PartyMemberEquipmentManager
{
	private const int companionHeavyArmorCoefficient = 20;
	private const int companionMediumArmorCoefficient = 12;
	private const int companionLightArmorCoefficient = 5;
	
	private static ItemListID redWeaponLevel1 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.redPickLevel1Index, 1);
	private static ItemListID redWeaponLevel2 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.redPickLevel2Index, 1);
	private static ItemListID redWeaponLevel3 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.redPickLevel3Index, 1);
	private static ItemListID redWeaponLevel4 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.redPickLevel4Index, 1);
	private static ItemListID redWeaponLevel5 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.redPickLevel5Index, 1);
	
	private static ItemListID nandorWeaponLevel1 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.nandorCudgelLevel1Index, 1);
	private static ItemListID nandorWeaponLevel2 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.nandorCudgelLevel2Index, 1);
	private static ItemListID nandorWeaponLevel3 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.nandorCudgelLevel3Index, 1);
	private static ItemListID nandorWeaponLevel4 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.nandorCudgelLevel4Index, 1);
	private static ItemListID nandorWeaponLevel5 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.nandorCudgelLevel5Index, 1);
	
	private static ItemListID carterWeaponLevel1 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.carterShivLevel1Index, 1);
	private static ItemListID carterWeaponLevel2 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.carterShivLevel2Index, 1);
	private static ItemListID carterWeaponLevel3 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.carterShivLevel3Index, 1);
	private static ItemListID carterWeaponLevel4 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.carterShivLevel4Index, 1);
	private static ItemListID carterWeaponLevel5 = new ItemListID(ItemList.partyMemberWeaponListIndex, ItemList.carterShivLevel5Index, 1);
	
	public static Weapon getWeapon(string companionName, int level)
	{
		switch(companionName)
		{
			case NPCNameList.thatch:
				return getRedWeapon(level);
			case NPCNameList.nandor:
				return getNandorWeapon(level);
			case NPCNameList.carter:
				return getCarterWeapon(level);
			default:
				Debug.LogError("Did not recognize companionName: " + companionName);
				return getRedWeapon(level);
		}
	}
	
	public static int getArmor(string companionName, int level)
	{
		switch(companionName)
		{
			case NPCNameList.thatch:
				return getMediumArmor(level);
			case NPCNameList.nandor:
			case NPCNameList.carter:
				return getLightArmor(level);
			default:
				return -1;
		}
	}
	
	private static Weapon getRedWeapon(int level)
	{
		if(level <= 1)
		{
			return (Weapon) ItemList.getItem(redWeaponLevel1);
		} else if(level == 2)
		{
			return (Weapon) ItemList.getItem(redWeaponLevel2);
		} else if(level == 3)
		{
			return (Weapon) ItemList.getItem(redWeaponLevel3);
		} else if(level == 4)
		{
			return (Weapon) ItemList.getItem(redWeaponLevel4);
		} else 
		{
			return (Weapon) ItemList.getItem(redWeaponLevel5);
		}
	}

	private static Weapon getNandorWeapon(int level)
	{
		if(level <= 1)
		{
			return (Weapon) ItemList.getItem(nandorWeaponLevel1);
		} else if(level == 2)
		{
			return (Weapon) ItemList.getItem(nandorWeaponLevel2);
		} else if(level == 3)
		{
			return (Weapon) ItemList.getItem(nandorWeaponLevel3);
		} else if(level == 4)
		{
			return (Weapon) ItemList.getItem(nandorWeaponLevel4);
		} else 
		{
			return (Weapon) ItemList.getItem(nandorWeaponLevel5);
		}
	}
	
	private static Weapon getCarterWeapon(int level)
	{
		if(level <= 1)
		{
			return (Weapon) ItemList.getItem(carterWeaponLevel1);
		} else if(level == 2)
		{
			return (Weapon) ItemList.getItem(carterWeaponLevel2);
		} else if(level == 3)
		{
			return (Weapon) ItemList.getItem(carterWeaponLevel3);
		} else if(level == 4)
		{
			return (Weapon) ItemList.getItem(carterWeaponLevel4);
		} else 
		{
			return (Weapon) ItemList.getItem(carterWeaponLevel5);
		}
	}

	private static int getHeavyArmor(int level)
	{
		return level*companionHeavyArmorCoefficient;
	}
	
	private static int getMediumArmor(int level)
	{
		return level*companionMediumArmorCoefficient;
	}

	private static int getLightArmor(int level)
	{
		return level*companionLightArmorCoefficient;
	}

}
