using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopkeeperInventoryList
{
	public static Dictionary<string, Dictionary<string, Item>> shopkeeperInventories;
    public static Dictionary<string, Dictionary<string, Item>> buyBackInventories;

    public const string kendeInventoryKey = "Kende";
    public const string urosInventoryKey = "Uros";

	static ShopkeeperInventoryList()
	{
		setShopkeeperInventoryListBackToDefault();
	}
	
	public static void setShopkeeperInventoryListBackToDefault()
	{

        shopkeeperInventories = new Dictionary<string, Dictionary<string, Item>>();
        buyBackInventories = new Dictionary<string, Dictionary<string, Item>>();

		shopkeeperInventories[kendeInventoryKey] = new Dictionary<string, Item>();

        Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, 	ItemList.rationsIndex,			15),	shopkeeperInventories[kendeInventoryKey]);
		
		Inventory.addItem(ItemList.getItem(ItemList.questItemListIndex, 	ItemList.candyIndex, 			1),		shopkeeperInventories[kendeInventoryKey]);
		
		Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, 		ItemList.clothGlovesIndex, 		1),		shopkeeperInventories[kendeInventoryKey]);
		Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, 		ItemList.rottenSandalsIndex, 	1), 	shopkeeperInventories[kendeInventoryKey]);
		Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, 		ItemList.potLidIndex, 			1), 	shopkeeperInventories[kendeInventoryKey]);
		Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, 		ItemList.minersHelmetIndex, 	1), 	shopkeeperInventories[kendeInventoryKey]);
		
		buyBackInventories[kendeInventoryKey] = new Dictionary<string, Item>();
		
		shopkeeperInventories[urosInventoryKey] = new Dictionary<string, Item>();
		
		Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, 	ItemList.rationsIndex, 			10), 	shopkeeperInventories[urosInventoryKey]);
		Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, 	ItemList.properFoodIndex, 		5), 	shopkeeperInventories[urosInventoryKey]);
		Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, 	ItemList.thistleTeaIndex, 		5), 	shopkeeperInventories[urosInventoryKey]);

		Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, 	ItemList.salvagedGuardHelmIndex, 	1), 	shopkeeperInventories[urosInventoryKey]);
		Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, 	ItemList.salvagedGuardArmorIndex, 	1), 	shopkeeperInventories[urosInventoryKey]);
		Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, 	ItemList.salvagedGuardGlovesIndex, 	1), 	shopkeeperInventories[urosInventoryKey]);
		Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, 	ItemList.salvagedGuardBootsIndex, 	1), 	shopkeeperInventories[urosInventoryKey]);

		buyBackInventories[urosInventoryKey] = new Dictionary<string, Item>();
	}

	public static void setShopkeeperInventoryList(Dictionary<string, Dictionary<string, Item>> newShopkeeperInventories, 
													Dictionary<string, Dictionary<string, Item>> newBuyBackInventories)
	{
		setShopkeeperInventoryListBackToDefault();

		shopkeeperInventories = addAllKeys(shopkeeperInventories, newShopkeeperInventories);

        buyBackInventories = addAllKeys(buyBackInventories, newBuyBackInventories);
    }

	private static Dictionary<string, Dictionary<string, Item>> addAllKeys(Dictionary<string, Dictionary<string, Item>> oldDict, Dictionary<string, Dictionary<string, Item>> newDict)
	{
        foreach (KeyValuePair<string, Dictionary<string, Item>> kvp in newDict)
        {
            if (oldDict.ContainsKey(kvp.Key))
            {
                oldDict[kvp.Key] = kvp.Value;
            }
            else
            {
                oldDict .Add(kvp.Key, kvp.Value);
            }
        }

        return oldDict;
    }

    public static Dictionary<string,Item> getShopkeeperInventory(string inventoryKey, bool buyBack)
	{
        if (buyBack)
        {
            return buyBackInventories[inventoryKey];
        }
        else
        {
            return shopkeeperInventories[inventoryKey];
        }
    }
	
}
