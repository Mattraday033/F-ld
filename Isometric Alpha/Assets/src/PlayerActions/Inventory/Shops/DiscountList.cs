using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiscountList
{
    public const float defaultDiscount = 0f;

    public static Dictionary<string, IDiscount> discountDictionary;

    public static float getDiscount(string key)
    {
        if (key == null)
        {
            return defaultDiscount;
        } else if( !discountDictionary.ContainsKey(key))
        {
            return defaultDiscount + calculateIntimidationDiscount(key);
        }

        return discountDictionary[key].getDiscount() + calculateIntimidationDiscount(key);
    }

    private static float calculateIntimidationDiscount(string key)
    {
        if(!ShopkeeperInventoryList.getShopkeeperIntimidatedFlag(key))
        {
            return 0f;
        }

        return PartyStats.getHighestStrength() * Charisma.discountPerCharismaPoint * 4;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeDiscounts()
    {
        discountDictionary = new Dictionary<string, IDiscount>();

        discountDictionary.Add(NPCNameList.uros, new UrosDiscount());
    }


}

public interface IDiscount
{
    public float getDiscount();

}

public class UrosDiscount : IDiscount
{
    public float getDiscount()
    {
        if (Flags.getFlag(FlagNameList.urosBestPrices))
        {
            return .5f;
        }
        else if (Flags.getFlag(FlagNameList.urosBadPrices))
        {
            return -.5f;
        }
        else if (Flags.getFlag(FlagNameList.urosWorstPrices))
        {
            return -1f;
        }
        else
        {
            return DiscountList.defaultDiscount;
        }
    }
}
