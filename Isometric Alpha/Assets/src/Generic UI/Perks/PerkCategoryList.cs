using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerkCategoryList
{
	public static PerkCategory strengthPerks = new PerkCategory("Strength", PerkSubcategoryType.Strength);
	public static PerkCategory dexterityPerks = new PerkCategory("Dexterity", PerkSubcategoryType.Dexterity);
	public static PerkCategory wisdomPerks = new PerkCategory("Wisdom", PerkSubcategoryType.Wisdom);
	public static PerkCategory charismaPerks = new PerkCategory("Charisma", PerkSubcategoryType.Charisma);
	public static PerkCategory lessonPerks = new PerkCategory("Lessons", PerkSubcategoryType.Lessons);
	public static PerkCategory backgroundPerks = new PerkCategory("Backgrounds", PerkSubcategoryType.Backgrounds);
	
	//public static PerkCategory spellPerks = new PerkCategory("Spells", PerkSubcategoryType.Spells);

	public static ArrayList allPerkCategories;

	static PerkCategoryList()
	{
		allPerkCategories = new ArrayList();
		
		allPerkCategories.Add(strengthPerks);
		allPerkCategories.Add(dexterityPerks);
		allPerkCategories.Add(wisdomPerks);
		allPerkCategories.Add(charismaPerks);
		
		//allPerkCategories.Add(lessonPerks);
		//allPerkCategories.Add(backgroundPerks);
	}

	public static ArrayList getAllPerkCategories()
	{
		return allPerkCategories;
	}

}
