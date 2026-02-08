using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class StanceWeapon : Weapon
{

	public StanceWeapon(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int worth, int slotID, bool isTwoHanded, EffectAnimationType effectAnimationType = EffectAnimationType.Slash)
	: base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeIndex, worth, slotID, isTwoHanded, effectAnimationType)
	{

	}

	public override bool appliesStanceStacks()
	{
		return true;
	}

}

[System.Serializable]
public class Staff : StanceWeapon
{

	public Staff(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int worth, int slotID, bool isTwoHanded)
	: base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeIndex, worth, slotID, isTwoHanded, EffectAnimationType.Blunt)
	{

	}

}
