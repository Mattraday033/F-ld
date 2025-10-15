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

	public override CombatAction getCombatAction()
	{
		return new FistAttack();
	}
	
	public override bool appliesStanceStacks()
	{
		return true;
	}
}
