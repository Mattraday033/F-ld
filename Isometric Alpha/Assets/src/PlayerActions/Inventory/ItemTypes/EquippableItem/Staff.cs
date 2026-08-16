using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class StanceWeapon : Weapon
{

	public StanceWeapon(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, SelectorTemplate rangeTemplate, int worth, bool isTwoHanded, EffectAnimationType effectAnimationType = EffectAnimationType.Slash)
	: base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeTemplate, worth, isTwoHanded, effectAnimationType)
	{

	}

	public override bool appliesStanceStacks()
	{
		return true;
	}

}

public class Fist : Weapon
{
	public Fist(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, SelectorTemplate rangeTemplate) :
	base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeTemplate, ItemList.itemHasNoWorth, false)
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

	public Staff(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, SelectorTemplate rangeTemplate, int worth, bool isTwoHanded)
	: base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeTemplate, worth, isTwoHanded, EffectAnimationType.Blunt)
	{

	}

}
