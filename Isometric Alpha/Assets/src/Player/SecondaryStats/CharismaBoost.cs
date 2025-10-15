using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharismaBoost : SecondaryStatBoost, IJSONConvertable
{
	private double discount = 0.0;

	public CharismaBoost(string key, double discount)
	{
		this.discount = discount;
		this.affectsZone = false;
	}
	
	public CharismaBoost(string key, double discount, string sourceName)
	{
		this.discount = discount;
		this.sourceName = sourceName;
		this.affectsZone = true;
	}
	
	public override double getDiscount()
	{
		return discount;
	}
	
	public override string convertToJson()
	{
		return "{\"boostType\":\"Charisma\"," + 
				"\"key\":\"" + key + "\"," +
				"\"discount\":\"" + getDiscount() + "\"," +
				"\"affectsZone\":\"" + affectsZone + "\"" +
				"}";
	}
}
