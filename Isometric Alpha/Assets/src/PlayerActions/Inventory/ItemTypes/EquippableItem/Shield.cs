using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class Shield : Armor
{
	public Shield(ItemListID listID, string key, string loreDescription, int armorRating, int slotID) :
	base(listID, key, loreDescription, armorRating, Armor.offHandSlotIndex)
    {
        setWorth(calculateWorth(armorRating));
	}	
    
    public Shield(ItemListID listID, string key, string loreDescription, string armorFormula, int slotID) :
	base(listID, key, loreDescription, armorFormula, Armor.offHandSlotIndex)
    {
        setWorth(calculateWorth(DamageCalculator.calculateFormula(armorFormula, worthStatsSource)));
	}

	private static int calculateWorth(int armorRating)
	{
		return (armorRating / 2);
	}
}