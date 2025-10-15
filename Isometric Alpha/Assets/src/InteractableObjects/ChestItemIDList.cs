using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChestItemIDList
{

    private static Dictionary<string, List<ItemListID>> chestItemIDList;
    private static List<ItemListID> list;

    static ChestItemIDList()
    {
        chestItemIDList = new Dictionary<string, List<ItemListID>>();

        #region NECamp

        list = new List<ItemListID>();

        list.Add(new ItemListID(ItemList.usableItemListIndex, ItemList.rationsIndex, 3));
    
        chestItemIDList.Add(AreaNameList.campNorthEast, list);

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
