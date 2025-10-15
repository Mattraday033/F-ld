using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

//[System.Serializable]
public class Staff : Weapon
{

	public Staff(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int worth, int slotID, bool isTwoHanded)
	: base(listId, key, loreDescription, damageFormula, critFormula, iconName, rangeIndex, worth, slotID, isTwoHanded)
	{

	}

	public override bool appliesStanceStacks()
	{
		return true;
	}

}
