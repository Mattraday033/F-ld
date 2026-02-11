using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiShieldTrait : Trait
{
    
    private EquippableItem shield;

    public AntiShieldTrait(EquippableItem shield):
    base("AntiShieldTrait",
         TraitType.Influence, 
         "This character lost access to their shield because they attacked with a Two Handed Weapon.", 
         iconName: IconList.armorShredIconName, 
         immobile: false, 
         pacifistic: false, 
         permanent: false, 
         roundsLeft: Constants.endOfRoundDuration)
    {
        this.shield = shield;        
    }

    public override bool isHiddenTrait()
    {
        return true;
    }

    private string getInvertedStatFormula<T>(FormulaDelegate<T> getFormulaToInvert, T formulaSource)
    {
        string formula = getFormulaToInvert(formulaSource);

        return DamageCalculator.invertFormula(formula);
    }

    #region Generic Stats

    public override string getArmorFormula()
    {
        return getInvertedStatFormula(t => t.getArmorFormula(), shield);
    }

    public override string getArmorShredFormula()
    {
        return getInvertedStatFormula(t => t.getArmorShredFormula(), shield);
    }

    public override string getCritFormula()
    {
        return getInvertedStatFormula(t => t.getCritFormula(), shield);
    }


    public override string getBonusDamageFormula()
    {
        return getInvertedStatFormula(t => t.getBonusDamageFormula(), shield);
    }

    public override string getDamageFormula()
    {
        return getInvertedStatFormula(t => t.getDamageFormula(), shield);
    }

    public override string getInvulnerableFormula()
    {
        return getInvertedStatFormula(t => t.getInvulnerableFormula(), shield);
    }

    public override string getVulnerableFormula()
    {
        return getInvertedStatFormula(t => t.getVulnerableFormula(), shield);
    }

    #endregion

    #region PrimaryStats

    public override string getBonusStrengthFormula()
    {
        return getInvertedStatFormula(t => t.getBonusStrengthFormula(), shield);
    }

    public override string getBonusDexterityFormula()
    {
        return getInvertedStatFormula(t => t.getBonusDexterityFormula(), shield);
    }

    public override string getBonusWisdomFormula()
    {
        return getInvertedStatFormula(t => t.getBonusWisdomFormula(), shield);
    }

    public override string getBonusCharismaFormula()
    {
        return getInvertedStatFormula(t => t.getBonusCharismaFormula(), shield);
    }

    #endregion

    #region Secondary Stats

    //Strength Stats
    public override string getBonusPhysicalResistanceFormula()
    {
        return getInvertedStatFormula(t => t.getBonusPhysicalResistanceFormula(), shield);
    }

    public override string getBonusCriticalDamageMultiplierFormula()
    {
        return getInvertedStatFormula(t => t.getBonusCriticalDamageMultiplierFormula(), shield);
    }

    public override string getBonusHealthFormula()
    {
        return getInvertedStatFormula(t => t.getBonusHealthFormula(), shield);
    }

    //Dexterity Stats
    public override string getBonusSurpriseRoundDamageFormula()
    {
        return getInvertedStatFormula(t => t.getBonusSurpriseRoundDamageFormula(), shield);
    }

    public override string getBonusArmorFormula()
    {
        return getInvertedStatFormula(t => t.getBonusArmorFormula(), shield);
    }

    public override string getBonusArmorPenetrationFormula()
    {
        return getInvertedStatFormula(t => t.getBonusArmorPenetrationFormula(), shield);
    }

    //Wisdom Stats
    public override string getBonusPassiveSlotsFormula()
    {
        return getInvertedStatFormula(t => t.getBonusPassiveSlotsFormula(), shield);
    }

    public override string getBonusWeaponSlotsFormula()
    {
        return getInvertedStatFormula(t => t.getBonusWeaponSlotsFormula(), shield);
    }

    public override string getBonusMentalResistanceFormula()
    {
        return getInvertedStatFormula(t => t.getBonusMentalResistanceFormula(), shield);
    }

    //Charisma Stats
    public override string getBonusSynergyFormula()
    {
        return getInvertedStatFormula(t => t.getBonusSynergyFormula(), shield);
    }

    public override string getBonusExuberancesFormula()
    {
        return getInvertedStatFormula(t => t.getBonusExuberancesFormula(), shield);
    }

    public override string getBonusZOIPotencyFormula()
    {
        return getInvertedStatFormula(t => t.getBonusZOIPotencyFormula(), shield);
    }

    #endregion

    #region Party Stats

    public override string getBonusRegenFormula()
    {
        return getInvertedStatFormula(t => t.getBonusRegenFormula(), shield);
    }

    public override string getBonusSurpriseRoundsFormula()
    {
        return getInvertedStatFormula(t => t.getBonusSurpriseRoundsFormula(), shield);
    }

    public override string getBonusRetreatChanceFormula()
    {
        return getInvertedStatFormula(t => t.getBonusRetreatChanceFormula(), shield);
    }

    public override string getBonusPartyActionsFormula()
    {
        return getInvertedStatFormula(t => t.getBonusPartyActionsFormula(), shield);
    }

    public override string getBonusPartySlotsFormula()
    {
        return getInvertedStatFormula(t => t.getBonusPartySlotsFormula(), shield);
    }

    public override string getBonusGoldMultiplierFormula()
    {
        return getInvertedStatFormula(t => t.getBonusGoldMultiplierFormula(), shield);
    }
    
    public override string getBonusDiscountFormula()
    {
        return getInvertedStatFormula(t => t.getBonusDiscountFormula(), shield);
    }

    public override string getBonusVolleyAccuracyFormula()
    {
        return getInvertedStatFormula(t => t.getBonusVolleyAccuracyFormula(), shield);
    }

    #endregion

    #region Skills
    public override string getBonusIntimidateChargesFormula()
    {
        return getInvertedStatFormula(t => t.getBonusIntimidateChargesFormula(), shield);
    }

    public override string getBonusCunningChargesFormula()
    {
        return getInvertedStatFormula(t => t.getBonusCunningChargesFormula(), shield);
    }

    public override string getBonusObservationLevelFormula()
    {
        return getInvertedStatFormula(t => t.getBonusObservationLevelFormula(), shield);
    }

    public override string getBonusLeadershipUsesFormula()
    {
        return getInvertedStatFormula(t => t.getBonusLeadershipUsesFormula(), shield);
    }
    #endregion

}
