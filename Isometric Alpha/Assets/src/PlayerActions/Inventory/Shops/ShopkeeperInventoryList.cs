using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public delegate bool ShopkeeperRevealFlag();

public static class ShopkeeperInventoryList
{
    private static Dictionary<string, Dictionary<string, Item>> shopkeeperInventories;
    private static Dictionary<string, Dictionary<string, Item>> buyBackInventories;

    private static Dictionary<string, ShopkeeperRevealFlag> shopkeeperRevealFlags;
    private static Dictionary<string, bool> shopkeeperIntimidatedFlags;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeShopkeeperInventoryList()
    {
        shopkeeperInventories = new Dictionary<string, Dictionary<string, Item>>();
        buyBackInventories = new Dictionary<string, Dictionary<string, Item>>();
        shopkeeperRevealFlags = new Dictionary<string, ShopkeeperRevealFlag>();
        shopkeeperIntimidatedFlags = new Dictionary<string, bool>();

        #region Kende

        shopkeeperInventories[NPCNameList.kende] = new Dictionary<string, Item>();

        Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, ItemList.rationsIndex, 15), shopkeeperInventories[NPCNameList.kende]);

        Inventory.addItem(ItemList.getItem(ItemList.questItemListIndex, ItemList.candyIndex, 1), shopkeeperInventories[NPCNameList.kende]);

        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.clothGlovesIndex, 1), shopkeeperInventories[NPCNameList.kende]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.rottenSandalsIndex, 1), shopkeeperInventories[NPCNameList.kende]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.potLidIndex, 1), shopkeeperInventories[NPCNameList.kende]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.minersHelmetIndex, 1), shopkeeperInventories[NPCNameList.kende]);

        buyBackInventories[NPCNameList.kende] = new Dictionary<string, Item>();

        shopkeeperRevealFlags[NPCNameList.kende] = () => {
                                                            return Flags.getFlag(FlagNameList.kendeWillSellToPlayer) && !Flags.getFlag(FlagNameList.revoltStarted);
                                                         };

        #endregion

        #region Uros

        shopkeeperInventories[NPCNameList.uros] = new Dictionary<string, Item>();

        Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, ItemList.rationsIndex, 10), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, ItemList.properFoodIndex, 5), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.usableItemListIndex, ItemList.thistleTeaIndex, 5), shopkeeperInventories[NPCNameList.uros]);

        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.salvagedGuardHelmIndex, 1), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.salvagedGuardArmorIndex, 1), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.salvagedGuardGlovesIndex, 1), shopkeeperInventories[NPCNameList.uros]);
        Inventory.addItem(ItemList.getItem(ItemList.armorListIndex, ItemList.salvagedGuardBootsIndex, 1), shopkeeperInventories[NPCNameList.uros]);

        buyBackInventories[NPCNameList.uros] = new Dictionary<string, Item>();

        shopkeeperRevealFlags[NPCNameList.uros] = () => {
                                                            return Flags.getFlag(FlagNameList.revoltStarted);
                                                         };
        #endregion
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        LoadSaveFile.OnLoadResetData.AddListener(resetAllShopkeeperIntimidatedFlags);
        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        Dictionary<string, Dictionary<string, Item>> newShopkeeperInventories = SaveBlueprint.extractShopkeeperInventoriesFromJson(blueprint.currentShopkeeperInventories);
        Dictionary<string, Dictionary<string, Item>> newBuyBackInventories = SaveBlueprint.extractShopkeeperInventoriesFromJson(blueprint.currentBuyBackInventories);

        setShopkeeperInventoryList(newShopkeeperInventories, newBuyBackInventories);

        //must come after setShopkeeperInventoryList, which rebuilds the intimidated flags from scratch
        overwriteShopkeeperIntimidatedFlags(blueprint.currentShopkeeperIntimidatedFlags);
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

    public static bool getShopkeeperRevealStatus(string key)
    {
        if(shopkeeperRevealFlags.ContainsKey(key))
        {
            return shopkeeperRevealFlags[key]();
        }

        return true;
    }

    #region Shopkeeper Intimidated Flags

    public static bool getShopkeeperIntimidatedFlag(string shopkeeperInventoryKey)
    {
        if (!shopkeeperIntimidatedFlags.ContainsKey(shopkeeperInventoryKey))
        {
            shopkeeperIntimidatedFlags.Add(shopkeeperInventoryKey, false);
        }

        return shopkeeperIntimidatedFlags[shopkeeperInventoryKey];
    }

    public static void setShopkeeperIntimidatedFlag(string shopkeeperInventoryKey, bool flagStatus = true)
    {
        if (!shopkeeperIntimidatedFlags.ContainsKey(shopkeeperInventoryKey))
        {
            shopkeeperIntimidatedFlags.Add(shopkeeperInventoryKey, flagStatus);
        }
        else
        {
            shopkeeperIntimidatedFlags[shopkeeperInventoryKey] = flagStatus;
        }
    }

    public static void overwriteShopkeeperIntimidatedFlags(Dictionary<string, bool> newFlags)
    {
        shopkeeperIntimidatedFlags = new Dictionary<string, bool>();

        if (newFlags == null)
        {
            return;
        }

        foreach (KeyValuePair<string, bool> flag in newFlags)
        {
            shopkeeperIntimidatedFlags[flag.Key] = flag.Value;
        }
    }

    //assumes string is a json that can be deserialized into a Dictionary<string,bool>();
    public static void overwriteShopkeeperIntimidatedFlags(string newFlags)
    {
        if (newFlags == null || newFlags.Equals(""))
        {
            overwriteShopkeeperIntimidatedFlags((Dictionary<string, bool>) null);

            return;
        }

        overwriteShopkeeperIntimidatedFlags(JsonConvert.DeserializeObject<Dictionary<string, bool>>(newFlags));
    }

    public static void resetAllShopkeeperIntimidatedFlags()
    {
        foreach (string key in shopkeeperIntimidatedFlags.Keys.ToList())
        {
            shopkeeperIntimidatedFlags[key] = false;
        }
    }

    public static string getShopkeeperIntimidatedFlagsForSave()
    {
        return JsonConvert.SerializeObject(shopkeeperIntimidatedFlags, Formatting.Indented);
    }

    #endregion

}
