using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum PerkSubcategoryType{Strength = 1, Dexterity = 2, Wisdom = 3, Charisma = 4, Lessons = 5, Backgrounds = 6, Spells = 7}

public class PerkCategory : GlossaryCategory
{
	private PerkSubcategoryType type;
	
	public PerkCategory(string title, PerkSubcategoryType type):
	base(title)
	{
		this.type = type;
	}
	
	public override ArrayList getSubcategories()
	{
		switch(type)
		{
			case PerkSubcategoryType.Strength:
			
				return AbilityList.getAllStrengthAbilities();
				
			case PerkSubcategoryType.Dexterity:
			
				return AbilityList.getAllDexterityAbilities();
				
			case PerkSubcategoryType.Wisdom:
			
				return AbilityList.getAllWisdomAbilities();
				
			case PerkSubcategoryType.Charisma:
			
				return AbilityList.getAllCharismaAbilities();
			case PerkSubcategoryType.Lessons:
			
				return null;
			case PerkSubcategoryType.Backgrounds:
			
				return null;
			case PerkSubcategoryType.Spells:
			
				return null;
			default:
				throw new IOException("Unknown PerkSubcategoryType: " + type.ToString());
		}
	}
}
