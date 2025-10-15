using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    public const bool requestNormalShopInventory = false;
    public const bool requestBuyBackInventory = true;

    public bool equipmentDefault;
    public string shopkeeperInventoryKey;

    public virtual float getDiscount()
    {
        return 1f;
    }

    public Dictionary<string, Item> getInventory()
    {
        return ShopkeeperInventoryList.getShopkeeperInventory(getShopkeeperInventoryKey(), requestNormalShopInventory);
    }

    public Dictionary<string, Item> getBuyBackInventory()
    {
        return ShopkeeperInventoryList.getShopkeeperInventory(getShopkeeperInventoryKey(), requestBuyBackInventory);
    }

    public virtual string getShopkeeperInventoryKey()
    {
        if (shopkeeperInventoryKey != null && shopkeeperInventoryKey.Length > 0)
        {
            return shopkeeperInventoryKey;
        }
        else
        {
            shopkeeperInventoryKey = gameObject.GetComponent<DialogueTrigger>().getDialogue().getMainNPCName();

            return shopkeeperInventoryKey;
        }
    }

}