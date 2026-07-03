using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class StanceWeapon : Weapon
{

	public StanceWeapon(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, string rangeIndex, int worth, bool isTwoHanded, EffectAnimationType effectAnimationType = EffectAnimationType.Slash)
	: base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeIndex, worth, isTwoHanded, effectAnimationType)
	{

	}

	public override bool appliesStanceStacks()
	{
		return true;
	}

}

public class Fist : Weapon
{
	public Fist(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, string rangeIndex) :
	base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeIndex, ItemList.itemHasNoWorth, false)
	{

	}

	public override bool canBeJunk()
	{
		return false;
	}

	public override CombatAction getCombatAction(AllyStats stats)
	{
		return new FistAttack(stats);
	}
	
	public override bool appliesStanceStacks()
	{
		return true;
	}

	public override string getEffectAnimationType()
	{
		return EffectAnimationType.Blunt.ToString();
	}

}

[System.Serializable]
public class Staff : StanceWeapon
{

	public Staff(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, string rangeIndex, int worth, bool isTwoHanded)
	: base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeIndex, worth, isTwoHanded, EffectAnimationType.Blunt)
	{

	}

}
