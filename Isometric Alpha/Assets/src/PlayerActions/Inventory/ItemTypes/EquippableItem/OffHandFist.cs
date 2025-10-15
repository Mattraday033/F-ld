using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffHandFist : Fist
{
	public OffHandFist(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName) :
	base(listId, key, loreDescription, damageFormula, critFormula, iconName, Range.singleTargetIndex, Weapon.offHandSlotIndex)
	{ //ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int slotID

	}

	public override bool isEquipped(AllyStats target)
	{
		if (target.equippedItems.getOffHand() == null)
		{
			return true;
		}

		return false;
	}

	public override bool isUnequippable()
	{
		return false;
	}
	
	public override CombatAction getCombatAction()
	{
		return null;
	}

}
