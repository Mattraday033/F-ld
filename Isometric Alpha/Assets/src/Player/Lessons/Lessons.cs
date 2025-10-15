using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Lesson: IJSONConvertable, ILessonDisplayInfo
{
	private const string passiveType = "Passive";

	private string key;
	private string lessonDescription;
	private string mechanicalDescription;
		
	public Lesson(string key, string lessonDescription, string mechanicalDescription)
	{
		this.key = key;
		this.lessonDescription = lessonDescription;
		this.mechanicalDescription = mechanicalDescription;
	}
	
	public string getKey()
	{
		return key;
	}
	
	public string getType()
	{
		if(StatBoostList.getStatBoost(getKey())!= null)
		{
			return passiveType;
		} else
		{
			return AbilityList.lessonAbilityDictionary[getKey()].getDisplayType();
		}
	}
	
	public string getLessonDescription()
	{
		return lessonDescription;
	}
	
	public string getMechanicalDescription()
	{
		return mechanicalDescription;
	}
	
	public string convertToJson()
	{
		return "{\"key\":\"" + getKey() + "}";
	}
	
	public static string extractFromJson(string json)
	{
		return json.Replace("{","").Replace("}","").Replace("\"","");
	}
}