using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthBoost : SecondaryStatBoost, IJSONConvertable
{

	private double extraCritDamageMultiplier = 0.0;
	private int extraHealth = 0;
	private double physicalResistance = 0.0;
    private int extraIntimidateCharges = 0;

    public StrengthBoost(string key, double extraCritDamageMultiplier, int extraHealth, double physicalResistance, int extraIntimidateCharges)
	{
		this.key = key;

		this.extraCritDamageMultiplier = extraCritDamageMultiplier;
		this.extraHealth = extraHealth;
		this.physicalResistance = physicalResistance;
		this.extraIntimidateCharges = extraIntimidateCharges;

        this.affectsZone = false;

	}
	
	public StrengthBoost(string key, double extraCritDamageMultiplier, int extraHealth, double physicalResistance, int extraIntimidateCharges, string sourceName)
	{
		this.key = key;

		this.extraCritDamageMultiplier = extraCritDamageMultiplier;
		this.extraHealth = extraHealth;
		this.physicalResistance = physicalResistance;
        this.extraIntimidateCharges = extraIntimidateCharges;

        this.sourceName = sourceName;
		this.affectsZone = true;
	}

	public override double getExtraCritDamageMultiplier()
	{
		return extraCritDamageMultiplier;
	}
	
	public override int getExtraHealth()
	{
		return extraHealth;
	}

	public override double getPhysicalResistance()
	{
		return physicalResistance;
	}
    public override int getMaxIntimidateCharges()
    {
        return extraIntimidateCharges;
    }

    public override string convertToJson()
	{
		return "{\"boostType\":\"Strength\"," + 
				"\"key\":\"" + key + "\"," +
				"\"extraCritDamageMultiplier\":\"" + getExtraCritDamageMultiplier() + "\"," +
				"\"extraHealth\":\"" + getExtraHealth() + "\"," +
				"\"physicalResistance\":\"" + getPhysicalResistance() + "\"," +
				"\"affectsZone\":\"" + affectsZone + "\"" +
				"}";
	}

}
