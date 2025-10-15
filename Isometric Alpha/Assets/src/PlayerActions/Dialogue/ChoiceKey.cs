using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceKey: IJSONConvertable
{
	public string storyName;
	public string sourcePath;
	
	public ChoiceKey(string storyName, string sourcePath)
	{
		this.storyName = storyName;
		this.sourcePath = sourcePath;
	}
	
	public string getKey()
	{
		return storyName + "+" + sourcePath;
	}
	
	public bool Equals(ChoiceKey other)
	{
		return getKey().Equals(other.getKey());
	}
	

	public string convertToJson()
	{
		return "{\"key\":\"" + getKey() + "}";
	}

	public static ChoiceKey extractFromJson(string json)
	{
		json = json.Replace("{","").Replace("}","").Replace("\"","");

		string choiceKey = json.Split(":")[1];
		
		return new ChoiceKey(choiceKey.Split("+")[0],choiceKey.Split("+")[1]);
	}
}
