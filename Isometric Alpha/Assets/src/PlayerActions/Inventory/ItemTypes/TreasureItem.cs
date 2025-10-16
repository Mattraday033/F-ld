using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TreasureItem: Item, IJSONConvertable
{
	
	public const string typeIconName = "TreasureItem";

    public const string type = "Treasure";
    public const string subtype = "Treasure";
	
	public TreasureItem(ItemListID listId, string key, string loreDescription, int worth): base(listId, key, loreDescription, type, subtype, worth) 
	{
		
	}
	
	public TreasureItem(ItemListID listId, string key, string loreDescription, int worth, int quantity): base(listId, key, loreDescription, type, subtype, worth, quantity) 
	{
		
	}
	
	public override string getTypeIconName()
	{
		return typeIconName;
	}
	
	public override bool mustBeJunk()
	{
		return true;
	}	
}
