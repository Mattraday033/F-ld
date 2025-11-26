using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChestItemIDList
{

    private static Dictionary<string, List<ItemListID>> chestItemIDList;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeChestItemIDList()
    {
        chestItemIDList = new Dictionary<string, List<ItemListID>>();
        List<ItemListID> list;

        #region NECamp

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.rationsIndex, 3));

        chestItemIDList.Add(LocationNameList.campNorthEast, list);

        #endregion

        #region SECamp

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.bandagesIndex, 4));

        chestItemIDList.Add(LocationNameList.campSouthEast, list);

        #endregion

        #region MineLvl_1-1c

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.weaponsListIndex, ItemList.malletIndex));
        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.rationsIndex, 3));

        chestItemIDList.Add(LocationNameList.mineLvl1 + LocationNameList.section1c, list);

        #endregion

        #region MineLvl_2

        #region MineLvl_2-1b

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.rockCakeIndex, 2));
        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.leatherBootsIndex));

        chestItemIDList.Add(LocationNameList.mineLvl2 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_2-1c

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.rockCakeIndex, 2));
        list.Add(new ItemListID(ItemList.weaponsListIndex, ItemList.bronzeDirkIndex));

        chestItemIDList.Add(LocationNameList.mineLvl2 + LocationNameList.section1c, list);

        #endregion

        #region MineLvl_2-2b

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.questItemListIndex, ItemList.toolBundleIndex));
        list.Add(new ItemListID(ItemList.weaponsListIndex, ItemList.heavyPickIndex));
        list.Add(new ItemListID(ItemList.weaponsListIndex, ItemList.lightPickIndex));
        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.paddedArmorIndex));

        chestItemIDList.Add(LocationNameList.mineLvl2 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_2-3a

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.plumedHelmetIndex));

        chestItemIDList.Add(LocationNameList.mineLvl2 + LocationNameList.section3a, list);

        #endregion

        #region MineLvl_2-4

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.weaponsListIndex, ItemList.bronzeBarIndex));

        chestItemIDList.Add(LocationNameList.mineLvl2 + LocationNameList.section4, list);

        #endregion

        #region MineLvl_2-5

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.thistleTeaIndex, Constants.sizeFour));
        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.rockCakeIndex, Constants.sizeTwo));
        list.Add(new ItemListID(ItemList.weaponsListIndex, ItemList.wickedKnifeIndex));
        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.chokegrassBombIndex, Constants.sizeTwo));

        chestItemIDList.Add(LocationNameList.mineLvl2 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_2-6

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.weaponsListIndex, ItemList.staffIndex));

        chestItemIDList.Add(LocationNameList.mineLvl2 + LocationNameList.section6, list);

        #endregion

        #region MineLvl_2-7b

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.rockCakeIndex));
        list.Add(new ItemListID(ItemList.questItemListIndex, ItemList.winchIndex));
    
        chestItemIDList.Add(LocationNameList.mineLvl2 + LocationNameList.section7b, list);

        #endregion

        #endregion

        #region MineLvl_3

        #region MineLvl_3-1b

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.rockCakeIndex, Constants.sizeTwo));
        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.delversDreamIndex));

        chestItemIDList.Add(LocationNameList.mineLvl3 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_3-2a

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.weaponsListIndex, ItemList.heavyPickIndex));
        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.leatherBootsIndex));

        chestItemIDList.Add(LocationNameList.mineLvl3 + LocationNameList.section2a, list);

        #endregion

        #region MineLvl_3-3b

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.rationsIndex, Constants.sizeFive));
        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.bronzeBadgeArmorValue));

        chestItemIDList.Add(LocationNameList.mineLvl3 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_3-4a

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.leatherGlovesIndex));

        chestItemIDList.Add(LocationNameList.mineLvl3 + LocationNameList.section4a, list);

        #endregion

        #region MineLvl_3-5

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.paddedArmorIndex));

        chestItemIDList.Add(LocationNameList.mineLvl3 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_3-6a

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.treasureItemListIndex, ItemList.ironNuggetIndex));
        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.bronzeHelmetIndex));

        chestItemIDList.Add(LocationNameList.mineLvl3 + LocationNameList.section6a, list);

        #endregion
        #region MineLvl_3-7

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.armorListIndex, ItemList.salvagedGuardGlovesIndex));

        chestItemIDList.Add(LocationNameList.mineLvl3 + LocationNameList.section7, list);

        #endregion
        #endregion

    }

    public static Item getChestItem(string key, int index)
    {

        if(!chestItemIDList.ContainsKey(key))
        {
            Debug.LogError("Chest "+key+" has no item in list");
            return ItemList.getItem(0,0,10);
        }

        return ItemList.getItem(chestItemIDList[key][index]);
    }

}
