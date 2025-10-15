using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WisdomBoost : SecondaryStatBoost, IJSONConvertable
{
	private float armorPenetration = 0f;
	private int retreatChance = 0;
	private double mentalResistance = 0.0;

	public WisdomBoost(string key, float armorPenetration, int retreatChance, double mentalResistance)
	{
		this.key = key;
		this.armorPenetration = armorPenetration;
		this.retreatChance = retreatChance;
		this.mentalResistance = mentalResistance;
		this.affectsZone = false;
	}

	public WisdomBoost(string key, float armorPenetration, int retreatChance, double mentalResistance, string sourceName)
	{
		this.key = key;
		this.armorPenetration = armorPenetration;
		this.retreatChance = retreatChance;
		this.mentalResistance = mentalResistance;
		this.sourceName = sourceName;
		this.affectsZone = true;
	}

	public override float getArmorPenetration()
	{
		return armorPenetration;
	}
	
	public override int getRetreatChance()
	{
		return retreatChance;
	}
	
	public override double getMentalResistance()
	{
		return mentalResistance;
	}
	
	public override string convertToJson()
	{
		return "{\"boostType\":\"Wisdom\"," + 
				"\"key\":\"" + key + "\"," +
				"\"surpriseDamageMultiplier\":\"" + getArmorPenetration() + "\"," +
				"\"retreatChance\":\"" + getRetreatChance() + "\"," +
				"\"mentalResistance\":\"" + getMentalResistance() + "\"," +
				"\"affectsZone\":\"" + affectsZone + "\"" +
				"}";
	}

}
