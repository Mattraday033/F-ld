using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexterityBoost : SecondaryStatBoost, IJSONConvertable
{
	private float surpriseDamageMultiplier = 0f;
	private int extraArmor = 0;
	private int maxCunningCharges = 0;

	public DexterityBoost(string key, float surpriseDamageMultiplier, int extraArmor, int maxCunningCharges)
	{
		this.key = key;
		this.surpriseDamageMultiplier = surpriseDamageMultiplier;
		this.extraArmor = extraArmor;
		this.maxCunningCharges = maxCunningCharges;
		this.affectsZone = false;
	}

	public DexterityBoost(string key, float surpriseDamageMultiplier, int extraArmor, int maxCunningCharges, string sourceName)
	{
		this.key = key;
		this.surpriseDamageMultiplier = surpriseDamageMultiplier;
		this.extraArmor = extraArmor;
		this.maxCunningCharges = maxCunningCharges;
		this.sourceName = sourceName;
		this.affectsZone = true;
	}

	public override float getSurpriseDamageMultiplier()
	{
		return surpriseDamageMultiplier;
	}
	
	public override int getExtraArmor()
	{
		return extraArmor;
	}
	
	public override int getMaxCunningCharges()
	{
		return maxCunningCharges;
	}

	public override string convertToJson()
	{
		return "{\"boostType\":\"Dexterity\"," + 
				"\"key\":\"" + key + "\"," +
				"\"surpriseDamageMultiplier\":\"" + getSurpriseDamageMultiplier() + "\"," +
				"\"extraArmor\":\"" + getExtraArmor() + "\"," +
				"\"maxCunningCharges\":\"" + getMaxCunningCharges() + "\"," +
				"\"affectsZone\":\"" + affectsZone + "\"" +
				"}";
	}

}
