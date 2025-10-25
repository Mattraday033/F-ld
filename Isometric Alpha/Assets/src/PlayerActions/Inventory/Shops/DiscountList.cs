using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiscountList
{
    public const float defaultDiscount = 1f;

    public static Dictionary<string, IDiscount> discountDictionary;

    public static float getDiscount(string key)
    {
        if (!discountDictionary.ContainsKey(key))
        {
            return defaultDiscount;
        }

        return discountDictionary[key].getDiscount();
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
            return 1.5f;
        }
        else if (Flags.getFlag(FlagNameList.urosWorstPrices))
        {
            return 2f;
        }
        else
        {
            return DiscountList.defaultDiscount;
        }
    }
}
