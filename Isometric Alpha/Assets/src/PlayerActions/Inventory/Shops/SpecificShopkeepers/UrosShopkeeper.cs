using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrosShopkeeper : Shopkeeper
{

    public override float getDiscount()
    {
        if (Flags.getFlag(FlagNameList.urosBestPrices))
        {
            return .5f;
        }
        else if (Flags.getFlag(FlagNameList.urosNormalPrices))
        {
            return 1f;
        }
        else if (Flags.getFlag(FlagNameList.urosBadPrices))
        {
            return 1.5f;
        }
        else if (Flags.getFlag(FlagNameList.urosWorstPrices))
        {
            return 2f;
        }
        else
        {
            return 1f;
        }
    }
    
    public override string getShopkeeperInventoryKey()
    {
        return ShopkeeperInventoryList.urosInventoryKey;    
    }
}
