using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
	public Fist(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int slotID) :
	base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeIndex, ItemList.itemHasNoWorth, slotID, false)
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
