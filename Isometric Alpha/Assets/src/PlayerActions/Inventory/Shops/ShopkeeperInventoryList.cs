using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopkeeperInventoryList
{
    private static Dictionary<string, Dictionary<string, Item>> shopkeeperInventories;
    private static Dictionary<string, Dictionary<string, Item>> buyBackInventories;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeShopkeeperInventoryList()
    {
        shopkeeperInventories = new Dictionary<string, Dictionary<string, Item>>();
        buyBackInventories = new Dictionary<string, Dictionary<string, Item>>();

        shopkeeperInventories[NPCNameList.kende] = new Dictionary<string, Item>();

        Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, ItemList.rationsIndex, 15), shopkeeperInventories[NPCNameList.kende]);

        Inventory.addItem(ItemList.getItem(ItemList.questItemListIndex, ItemList.candyIndex, 1), shopkeeperInventories[NPCNameList.kende]);

        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.clothGlovesIndex, 1), shopkeeperInventories[NPCNameList.kende]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.rottenSandalsIndex, 1), shopkeeperInventories[NPCNameList.kende]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.potLidIndex, 1), shopkeeperInventories[NPCNameList.kende]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.minersHelmetIndex, 1), shopkeeperInventories[NPCNameList.kende]);

        buyBackInventories[NPCNameList.kende] = new Dictionary<string, Item>();

        shopkeeperInventories[NPCNameList.uros] = new Dictionary<string, Item>();

        Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, ItemList.rationsIndex, 10), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, ItemList.properFoodIndex, 5), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, ItemList.thistleTeaIndex, 5), shopkeeperInventories[NPCNameList.uros]);

        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.salvagedGuardHelmIndex, 1), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.salvagedGuardArmorIndex, 1), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.salvagedGuardGlovesIndex, 1), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.salvagedGuardBootsIndex, 1), shopkeeperInventories[NPCNameList.uros]);

        buyBackInventories[NPCNameList.uros] = new Dictionary<string, Item>();
    }

    public static void setShopkeeperInventoryList(Dictionary<string, Dictionary<string, Item>> newShopkeeperInventories,
                                                    Dictionary<string, Dictionary<string, Item>> newBuyBackInventories)
    {
        initializeShopkeeperInventoryList();

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
                oldDict.Add(kvp.Key, kvp.Value);
            }
        }

        return oldDict;
    }

    public static Dictionary<string, Item> getShopkeeperInventory(string inventoryKey, bool buyBack)
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

    public static InventoryWrapper[] convertShopkeeperInventoriesToJson()
    {
        InventoryWrapper[] wrapperOfShopkeeperInventories = new InventoryWrapper[shopkeeperInventories.Count];

        int inventoryIndex = 0;
        foreach (KeyValuePair<string, Dictionary<string, Item>> kvp in shopkeeperInventories)
        {
            Dictionary<string, Item> inventory = kvp.Value;

            wrapperOfShopkeeperInventories[inventoryIndex].key = kvp.Key;
            wrapperOfShopkeeperInventories[inventoryIndex].inventory = SaveBlueprint.convertToJson(inventory);
            inventoryIndex++;
        }

        return wrapperOfShopkeeperInventories;
    }

    public static InventoryWrapper[] convertBuyBackInventoriesToJson()
	{
		InventoryWrapper[] wrapperOfShopkeeperInventories = new InventoryWrapper[buyBackInventories.Count];

		int inventoryIndex = 0;
		foreach (KeyValuePair<string, Dictionary<string, Item>> kvp in buyBackInventories)
		{
			Dictionary<string, Item> inventory = kvp.Value;

			wrapperOfShopkeeperInventories[inventoryIndex].key = kvp.Key;
			wrapperOfShopkeeperInventories[inventoryIndex].inventory = SaveBlueprint.convertToJson(inventory);
			inventoryIndex++;
		}

		return wrapperOfShopkeeperInventories;
	}

}
