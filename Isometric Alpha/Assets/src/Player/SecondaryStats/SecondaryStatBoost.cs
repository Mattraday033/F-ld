using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISecondaryStatBoost
{
    public string getDamageFormula();

    public string getCritFormula();



    public string getBonusStrengthFormula();

    public string getBonusIntimidateChargesFormula();

    public string getBonusPhysicalResistanceFormula();

    public string getBonusCriticalDamageMultiplierFormula();

    public string getBonusHealthFormula();

    public string getBonusRegenFormula();



    public string getBonusDexterityFormula();

    public string getBonusCunningChargesFormula();

    public string getBonusArmorFormula();

    public string getBonusSurpriseRoundDamageFormula();

    public string getBonusSurpriseRoundsFormula();

    public string getBonusRetreatChanceFormula();



    public string getBonusWisdomFormula();

    public string getBonusObservationLevelFormula();

    public string getBonusArmorPenetrationFormula();

    public string getBonusPassiveSlotsFormula();

    public string getBonusWeaponSlotsFormula();

    public string getBonusMentalResistanceFormula();



    public string getBonusCharismaFormula();

    public string getBonusSynergyFormula();

    public string getBonusPartyActionsFormula();

    public string getBonusPartySlotsFormula();

    public string getBonusAffinityCoefficientFormula();

    public string getBonusLeadershipUsesFormula();
}

public class SecondaryStatBoost : IJSONConvertable
{
    public string key;
    public string sourceName = "";
    public bool affectsZone = false;

    //Strength Secondary Stats
    public virtual double getExtraCritDamageMultiplier()
    {
        return 0.0;
    }

    public virtual int getExtraHealth()
    {
        return 0;
    }

    public virtual double getPhysicalResistance()
    {
        return 0.0;
    }
    public virtual int getMaxIntimidateCharges()
    {
        return 0;
    }

    //Dex Secondary Stats

    public virtual float getSurpriseDamageMultiplier()
    {
        return 0f;
    }

    public virtual int getExtraArmor()
    {
        return 0;
    }

    public virtual int getMaxCunningCharges()
    {
        return 0;
    }

    //Wisdom Secondary Stats
    public virtual float getArmorPenetration()
    {
        return 0f;
    }

    public virtual double getMentalResistance()
    {
        return 0.0;
    }

    public virtual int getRetreatChance()
    {
        return 0;
    }

    //Charisma Secondary Stats
    public virtual double getDiscount()
    {
        return 0.0;
    }

    public virtual string convertToJson()
    {
        throw new IOException("Called convertToJson() from the base class.");
    }
    /*
	public static SecondaryStatBoost extractBoostFromJSON(string json)
	{
		string[] KVPs = Helpers.convertJsonStringToKVPs(json);
		
		switch(KVPs[0].Split(":")[1])
		{
			case "Strength":
				return (SecondaryStatBoost) new StrengthBoost(KVPs[1].Split(":")[1], 
																double.Parse(KVPs[2].Split(":")[1]),
																int.Parse(KVPs[3].Split(":")[1]),
																double.Parse(KVPs[4].Split(":")[1]),
																bool.Parse(KVPs[5].Split(":")[1]));
			case "Dexterity":
				return (SecondaryStatBoost) new DexterityBoost(KVPs[1].Split(":")[1],
																float.Parse(KVPs[2].Split(":")[1]),
																int.Parse(KVPs[3].Split(":")[1]),
																int.Parse(KVPs[4].Split(":")[1]),
																bool.Parse(KVPs[5].Split(":")[1]));
			case "Wisdom":
				return (SecondaryStatBoost) new WisdomBoost(KVPs[1].Split(":")[1],
															int.Parse(KVPs[2].Split(":")[1]),
															int.Parse(KVPs[3].Split(":")[1]),
															double.Parse(KVPs[4].Split(":")[1]),
																bool.Parse(KVPs[5].Split(":")[1]));
			case "Charisma":
				return (SecondaryStatBoost) new CharismaBoost(KVPs[1].Split(":")[1],
																double.Parse(KVPs[2].Split(":")[1]),
																bool.Parse(KVPs[3].Split(":")[1]));
			
			default:
				throw new IOException("Unreconized boostType: " + KVPs[0].Split(":")[0]);
		}
	}
	*/
}
