using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Key: EssentialItem, IJSONConvertable
{
	public const string typeIconName = "KeyItem";
	public const string subtype = "Key";
	private int ID;
	
	public Key(ItemListID listId, string key, string loreDescription, int ID): base(listId, key, loreDescription, subtype) 
	{
		this.ID = ID;
	}
	
	public Key(ItemListID listId, string key, string loreDescription, int worth, int ID, int quantity): base(listId, key, loreDescription, subtype, worth, quantity) 
	{
		this.ID = ID;
	}
	
 	public override string getTypeIconName()
	{
		return typeIconName;
	}
	
	public override int getID()
	{
		return ID;
	}

}
