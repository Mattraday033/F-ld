using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate string FormulaDelegate<T>(T t);

public abstract class StatBoostSourceCombiner : StatBoostSource, IEnumerable
{

    public abstract IEnumerator GetEnumerator();
    
    #region StatBoostSource

    #region Generic Stats
    
    public override string getCritFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getCritFormula());
    }
    public override string getBonusDamageFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusDamageFormula());
    }

    public override string getDamageFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getDamageFormula());
    }

    public override string getInvulnerableFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getInvulnerableFormula());
    }

    public override string getVulnerableFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getVulnerableFormula());
    }

    #endregion

    #region PrimaryStats

    public override string getBonusStrengthFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusStrengthFormula());
    }
    public override string getBonusDexterityFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusDexterityFormula());
    }
    public override string getBonusWisdomFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusWisdomFormula());
    }
    public override string getBonusCharismaFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusCharismaFormula());
    }

    #endregion

    #region Secondary Stats

    //Strength Stats
    public override string getBonusPhysicalResistanceFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusPhysicalResistanceFormula());
    }
    public override string getBonusCriticalDamageMultiplierFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusCriticalDamageMultiplierFormula());
    }
    public override string getBonusHealthFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusHealthFormula());
    }

    //Dexterity Stats
    public override string getBonusSurpriseRoundDamageFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusSurpriseRoundDamageFormula());
    }
    public override string getBonusArmorFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusArmorFormula());
    }
    public override string getBonusArmorPenetrationFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusArmorPenetrationFormula());
    }

    //Wisdom Stats
    public override string getBonusPassiveSlotsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusPassiveSlotsFormula());
    }
    public override string getBonusWeaponSlotsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusWeaponSlotsFormula());
    }
    public override string getBonusMentalResistanceFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusMentalResistanceFormula());
    }

    //Charisma Stats
    public override string getBonusSynergyFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusSynergyFormula());
    }
    public override string getBonusExuberancesFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusExuberancesFormula());
    }
    public override string getBonusZOIPotencyFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusZOIPotencyFormula());
    }

    #endregion

    #region Party Stats

    public override string getBonusRegenFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusRegenFormula());
    }

    public override string getBonusSurpriseRoundsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusSurpriseRoundsFormula());
    }
    public override string getBonusRetreatChanceFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusRetreatChanceFormula());
    }

    public override string getBonusPartyActionsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusPartyActionsFormula());
    }
    public override string getBonusPartySlotsFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusPartySlotsFormula());
    }

    public override string getBonusGoldMultiplierFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusGoldMultiplierFormula());
    }
    public override string getBonusDiscountFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusDiscountFormula());
    }

    public override string getBonusVolleyAccuracyFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusVolleyAccuracyFormula());
    }

    #endregion

    #region Skills
    public override string getBonusIntimidateChargesFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusIntimidateChargesFormula());
    }
    public override string getBonusCunningChargesFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusCunningChargesFormula());
    }
    public override string getBonusObservationLevelFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusObservationLevelFormula());
    }
    public override string getBonusLeadershipUsesFormula()
    {
        return getAllOfOneStatFormula<StatBoostSource>(this, t => t.getBonusLeadershipUsesFormula());
    }
    #endregion

    #endregion
}
