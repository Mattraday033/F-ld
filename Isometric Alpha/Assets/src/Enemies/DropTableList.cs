using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DropTableList
{
	public static ArrayList allDropTables = new ArrayList();

	public const string slaveMineDT1Name = "slaveMineDT1";

	public static DropTable slaveMineDT1 = new DropTable(slaveMineDT1Name, 3, 9,
														 new Item[]{ItemList.getItem(ItemList.usableItemListIndex, ItemList.rationsIndex, 1),
																	ItemList.getItem(ItemList.weaponsListIndex, ItemList.malletIndex, 1),
																	ItemList.getItem(ItemList.armorListIndex, ItemList.clothGlovesIndex, 1),
																	ItemList.getItem(ItemList.armorListIndex, ItemList.rottenSandalsIndex, 1),
																	ItemList.getItem(ItemList.armorListIndex, ItemList.potLidIndex, 1),
																	ItemList.getItem(ItemList.armorListIndex, ItemList.minersHelmetIndex, 1),
																	ItemList.getItem(ItemList.treasureItemListIndex, ItemList.ironNuggetIndex, 1),
																	null},
														 new float[]{.1f,.025f,.025f,.025f,.025f,.025f,.025f,.75f});	

	static DropTableList()
	{
		allDropTables.Add(slaveMineDT1);
	}
	
	public static DropTable getDropTable(string name)
	{
		if(name == null || name is null)
		{
			throw new IOException("DropTable name was null.");
		}
		
		foreach(DropTable dropTable in allDropTables)
		{
			if(String.Equals(name, dropTable.name, StringComparison.OrdinalIgnoreCase))
			{
				return dropTable;
			}
		}

		return (DropTable) allDropTables[0];

    }

}
