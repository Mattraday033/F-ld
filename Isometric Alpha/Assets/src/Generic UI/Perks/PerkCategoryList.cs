using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerkCategoryList
{
	public readonly static PerkCategory strengthPerks = new PerkCategory("Strength", PerkSubcategoryType.Strength);
	public readonly static PerkCategory dexterityPerks = new PerkCategory("Dexterity", PerkSubcategoryType.Dexterity);
	public readonly static PerkCategory wisdomPerks = new PerkCategory("Wisdom", PerkSubcategoryType.Wisdom);
	public readonly static PerkCategory charismaPerks = new PerkCategory("Charisma", PerkSubcategoryType.Charisma);
	public readonly static PerkCategory lessonPerks = new PerkCategory("Lessons", PerkSubcategoryType.Lessons);
	public readonly static PerkCategory backgroundPerks = new PerkCategory("Backgrounds", PerkSubcategoryType.Backgrounds);
	
	//public static PerkCategory spellPerks = new PerkCategory("Spells", PerkSubcategoryType.Spells);

	public static List<GlossaryCategory> allPerkCategories;

    [RuntimeInitializeOnLoadMethod]
	private static void instantiatePerkCategoryList()
	{
		allPerkCategories = new List<GlossaryCategory>();
		
		allPerkCategories.Add(strengthPerks);
		allPerkCategories.Add(dexterityPerks);
		allPerkCategories.Add(wisdomPerks);
		allPerkCategories.Add(charismaPerks);
		
		//allPerkCategories.Add(lessonPerks);
		//allPerkCategories.Add(backgroundPerks);
	}

	public static List<GlossaryCategory> getAllPerkCategories()
	{
		return allPerkCategories;
	}

}
