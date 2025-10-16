using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class QuestItem: EssentialItem,IJSONConvertable
{
	public const string typeIconName = "QuestItem";
	public const string subtype = "Quest";
	private int questID;
	
	public QuestItem(ItemListID listId, string key, string loreDescription, int ID): base(listId, key, loreDescription, subtype) 
	{
		questID = ID;
	}
	
	public QuestItem(ItemListID listId, string key, string loreDescription, int ID, int quantity): base(listId, key, loreDescription, subtype, quantity) 
	{
		questID = ID;
	}
	
	public QuestItem(ItemListID listId, string key, string loreDescription, int worth, int ID, int quantity): base(listId, key, loreDescription, subtype, worth, quantity) 
	{
		questID = ID;
	}
	
	public override string getTypeIconName()
	{
		return typeIconName;
	}
	
	public int getQuestID(){
		
		return questID;
	}	

}