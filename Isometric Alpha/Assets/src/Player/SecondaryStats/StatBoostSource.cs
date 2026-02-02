using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StatBoostSource : INameSource
{
    public abstract string getName();

    public abstract Stats getStatSource();

    #region Generic Stats

    public virtual string getArmorFormula()
    {
        switch (getName())
        {

        }

        return "0";
    }

    public virtual string getCritFormula()
    {
        switch (getName())
        {
            case StatSourceNameList.chewKey:
                return "4";
            case ItemList.salvagedGuardGlovesKey:
                return "D";
        }

        return "0";
    }


    public virtual string getBonusDamageFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getDamageFormula()
    {
        switch (getName())
        {
            case StatSourceNameList.halfHandStanceKey:
            case StatSourceNameList.bloodlustKey:
                return "1";
            case StatSourceNameList.predationKey:
            case StatSourceNameList.chewKey:
                return "4";
            case StatSourceNameList.cohesionKey:
                return "6";
            case StatSourceNameList.ralliedKey:
                return "8";
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getInvulnerableFormula()
    {
        switch (getName())
        {
            case StatSourceNameList.halfHandStanceKey:
                return "1";
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getVulnerableFormula()
    {
        switch (getName())
        {
            case StatSourceNameList.roastedKey:
                return "1";
            case StatSourceNameList.caveMadnessKey:
                return "3";
            case StatSourceNameList.acidVomitKey:
                return "4";
            case StatSourceNameList.bristledKey:
            case StatSourceNameList.woundedKey:
                return "5";
            case StatSourceNameList.insecureKey:
                return "7";
            default:
                return Constants.zeroRating;
        }
    }

    #endregion

    #region PrimaryStats

    public virtual string getBonusStrengthFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusDexterityFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusWisdomFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusCharismaFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    #endregion

    #region Secondary Stats

    //Strength Stats
    public virtual string getBonusPhysicalResistanceFormula()
    {
        switch (getName())
        {
            case ItemList.bronzeBadgeKey:
                return "10";
        }

        return "0";
    }

    public virtual string getBonusCriticalDamageMultiplierFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusHealthFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    //Dexterity Stats
    public virtual string getBonusSurpriseRoundDamageFormula()
    {
        switch (getName())
        {
            case NPCNameList.carter + ZoneOfInfluenceTrait.zoiTraitName:
                return "5C";
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusArmorFormula()
    {
        switch (getName())
        {
            case NPCNameList.thatch + ZoneOfInfluenceTrait.zoiTraitName:
                return Dexterity.extraArmorMultiplier + "C";
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusArmorPenetrationFormula()
    {
        switch (getName())
        {
            case StatSourceNameList.predationKey:
                return "10";
            default:
                return Constants.zeroRating;
        }
    }

    //Wisdom Stats
    public virtual string getBonusPassiveSlotsFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusWeaponSlotsFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusMentalResistanceFormula()
    {
        switch (getName())
        {
            case NPCNameList.nandor + ZoneOfInfluenceTrait.zoiTraitName:
                return Wisdom.mentalResistPerWisdom+"C";
            case ItemList.delversDreamKey:
                return "10";
        }

        return "0";
    }

    //Charisma Stats
    public virtual string getBonusSynergyFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusExuberancesFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusZOIPotencyFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    #endregion

    #region Party Stats

    public virtual string getBonusRegenFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusSurpriseRoundsFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusRetreatChanceFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusPartyActionsFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusPartySlotsFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusGoldMultiplierFormula()
    {
        switch (getName())
        {
            case ItemList.silverSpoonKey:
                return "20";
        }

        return "0";
    }
    
    public virtual string getBonusDiscountFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusVolleyAccuracyFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    #endregion

    #region Skills
    public virtual string getBonusIntimidateChargesFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusCunningChargesFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusObservationLevelFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }

    public virtual string getBonusLeadershipUsesFormula()
    {
        switch (getName())
        {
            default:
                return Constants.zeroRating;
        }
    }
    #endregion

    public delegate string SumFormulaDelegate<T>(T t);

    public static int calculateAllStatFormulas(Stats statSource, List<StatBoostSource> statBoostSources, SumFormulaDelegate<StatBoostSource> formulaDelegate)
    {
        string allFormulas = statBoostSources.Aggregate("", (a, b) => a = DamageCalculator.combineFormulas(a, formulaDelegate(b)));

        return DamageCalculator.calculateFormula(allFormulas, statSource);
    }

    public static double calculateAllStatFormulasAsPercentageDouble(Stats statSource, List<StatBoostSource> statBoostSources, SumFormulaDelegate<StatBoostSource> formulaDelegate)
    {
        string allFormulas = statBoostSources.Aggregate("", (a, b) => a = DamageCalculator.combineFormulas(a, formulaDelegate(b)));
        double percentage = ((double)DamageCalculator.calculateFormula(allFormulas, statSource)) / 100.0;

        return percentage;
    }

    public static float calculateAllStatFormulasAsPercentageFloat(Stats statSource, List<StatBoostSource> statBoostSources, SumFormulaDelegate<StatBoostSource> formulaDelegate)
    {
        string allFormulas = statBoostSources.Aggregate("", (a, b) => a = DamageCalculator.combineFormulas(a, formulaDelegate(b)));
        float percentage = ((float) DamageCalculator.calculateFormula(allFormulas, statSource)) / 100f;

        return percentage;
    }

    public static List<StatBoostSource> getAllStatBoosts(IEnumerable statBoostList)
    {
        List<StatBoostSource> statBoosts = new List<StatBoostSource>();

        foreach (StatBoostSource statBoost in statBoostList)
        {
            if (statBoost != null)
            {
                statBoosts.Add(statBoost);
            }
        }

        return statBoosts;
    }

    public static string getAllOfOneStatFormula<T>(IEnumerable enumerable, FormulaDelegate<T> getFormula)
    {
        string totalFormula = "+0";

        foreach(T source in enumerable)
        {
            if(source != null)
            {
                totalFormula = DamageCalculator.combineFormulas(totalFormula, getFormula(source));
            }
        }

        return totalFormula;   
    }

    public static List<DescriptionPanelBuildingBlock> getStatBoostDescriptionBuildingBlocks(Stats statsSource, StatBoostSource boostSource)
    {
        List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

        if(statsSource == null || boostSource == null)
        {
            return blocks;
        }

        #region Generic Stats

        if (!boostSource.getDamageFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(DamageCalculator.calculateFormula(boostSource.getDamageFormula(), statsSource).ToString(), boostSource.getDamageFormula()));
        }

        if (!boostSource.getBonusDamageFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusDamageBlock(DamageCalculator.calculateFormula(boostSource.getBonusDamageFormula(), statsSource).ToString()), boostSource.getBonusDamageFormula()));
        }

        if (!boostSource.getCritFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getCritBlock(DamageCalculator.calculateFormula(boostSource.getCritFormula(), statsSource).ToString(), boostSource.getCritFormula()));
        }

        if (!boostSource.getInvulnerableFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getInvulnerableBlock(DamageCalculator.calculateFormula(boostSource.getInvulnerableFormula(), statsSource).ToString(), boostSource.getInvulnerableFormula()));
        }

        if (!boostSource.getVulnerableFormula().Equals(Constants.zeroRating) && !boostSource.getName().Equals(StatSourceNameList.halfHandStanceKey))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getVulnerableBlock(DamageCalculator.calculateFormula(boostSource.getVulnerableFormula(), statsSource).ToString(), boostSource.getVulnerableFormula()));
        }

        #endregion

        #region PrimaryStats

        if (!boostSource.getBonusStrengthFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getStrengthBlock(DamageCalculator.calculateFormula(boostSource.getBonusStrengthFormula(), statsSource).ToString()), boostSource.getBonusStrengthFormula()));
        }

        if (!boostSource.getBonusDexterityFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getDexterityBlock(DamageCalculator.calculateFormula(boostSource.getBonusDexterityFormula(), statsSource).ToString()), boostSource.getBonusDexterityFormula()));
        }

        if (!boostSource.getBonusWisdomFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getWisdomBlock(DamageCalculator.calculateFormula(boostSource.getBonusWisdomFormula(), statsSource).ToString()), boostSource.getBonusWisdomFormula()));
        }

        if (!boostSource.getBonusCharismaFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getCharismaBlock(DamageCalculator.calculateFormula(boostSource.getBonusCharismaFormula(), statsSource).ToString()), boostSource.getBonusCharismaFormula()));
        }
        #endregion

        #region Secondary Stats

        //Strength Stats

        if (!boostSource.getBonusHealthFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusHealthBlock(DamageCalculator.calculateFormula(boostSource.getBonusHealthFormula(), statsSource).ToString()), boostSource.getBonusHealthFormula()));
        }

        if (!boostSource.getBonusCriticalDamageMultiplierFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getCriticalHitDamageBlock(DamageCalculator.calculateFormula(boostSource.getBonusCriticalDamageMultiplierFormula(), statsSource).ToString() + "%"), boostSource.getBonusCriticalDamageMultiplierFormula()));
        }

        if (!boostSource.getBonusPhysicalResistanceFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getPhysicalResistBlock(DamageCalculator.calculateFormula(boostSource.getBonusPhysicalResistanceFormula(), statsSource).ToString() + "%"), boostSource.getBonusPhysicalResistanceFormula()));
        }

        //Dexterity Stats

        if (!boostSource.getBonusSurpriseRoundDamageFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getSurpriseRoundDamageMultiplierBlock(DamageCalculator.calculateFormula(boostSource.getBonusSurpriseRoundDamageFormula(), statsSource).ToString()), boostSource.getBonusSurpriseRoundDamageFormula()));
        }

        if (!boostSource.getBonusArmorFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getArmorBlock(DamageCalculator.calculateFormula(boostSource.getBonusArmorFormula(), statsSource).ToString()), boostSource.getBonusArmorFormula()));
        }

        if (!boostSource.getBonusArmorPenetrationFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getArmorPenetrationBlock(DamageCalculator.calculateFormula(boostSource.getBonusArmorPenetrationFormula(), statsSource).ToString() + "%"), boostSource.getBonusArmorPenetrationFormula()));
        }

        //Wisdom Stats

        if (!boostSource.getBonusPassiveSlotsFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getPassiveSlotsBlock(DamageCalculator.calculateFormula(boostSource.getBonusPassiveSlotsFormula(), statsSource).ToString()), boostSource.getBonusPassiveSlotsFormula()));
        }

        if (!boostSource.getBonusWeaponSlotsFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusWeaponSlotsBlock(DamageCalculator.calculateFormula(boostSource.getBonusWeaponSlotsFormula(), statsSource).ToString()), boostSource.getBonusWeaponSlotsFormula()));
        }

        if (!boostSource.getBonusMentalResistanceFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getMentalResistBlock(DamageCalculator.calculateFormula(boostSource.getBonusMentalResistanceFormula(), statsSource).ToString() + "%"), boostSource.getBonusMentalResistanceFormula()));
        }

        //Charisma Stats

        if (!boostSource.getBonusSynergyFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getSynergyBlock(DamageCalculator.calculateFormula(boostSource.getBonusSynergyFormula(), statsSource).ToString()), boostSource.getBonusSynergyFormula()));
        }

        if (!boostSource.getBonusExuberancesFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusExuberancesBlock(DamageCalculator.calculateFormula(boostSource.getBonusExuberancesFormula(), statsSource).ToString()), boostSource.getBonusExuberancesFormula()));
        }

        // if (!boostSource.getBonusZOIPotencyFormula().Equals(Constants.zeroRating))
        // {
        //     blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getZOIBlock(DamageCalculator.calculateFormula(boostSource.getBonusZOIPotencyFormula(), statsSource).ToString()), boostSource.getBonusZOIPotencyFormula()));
        // }

        #endregion

        #region Party Stats

        if (!boostSource.getBonusRegenFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getRegenBlock(DamageCalculator.calculateFormula(boostSource.getBonusRegenFormula(), statsSource).ToString()), boostSource.getBonusRegenFormula()));
        }

        if (!boostSource.getBonusRetreatChanceFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getRetreatChanceBlock(DamageCalculator.calculateFormula(boostSource.getBonusRetreatChanceFormula(), statsSource).ToString() + "%"), boostSource.getBonusRetreatChanceFormula()));
        }

        if (!boostSource.getBonusSurpriseRoundsFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getSurpriseRoundAmountBlock(DamageCalculator.calculateFormula(boostSource.getBonusSurpriseRoundsFormula(), statsSource).ToString()), boostSource.getBonusSurpriseRoundsFormula()));
        }

        if (!boostSource.getBonusPartySlotsFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getPartySlotsBlock(DamageCalculator.calculateFormula(boostSource.getBonusPartySlotsFormula(), statsSource).ToString()), boostSource.getBonusPartySlotsFormula()));
        }

        if (!boostSource.getBonusPartyActionsFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getPartyActionsBlock(DamageCalculator.calculateFormula(boostSource.getBonusPartyActionsFormula(), statsSource).ToString()), boostSource.getBonusPartyActionsFormula()));
        }

        if (!boostSource.getBonusGoldMultiplierFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getGoldMultiplierBlock(DamageCalculator.calculateFormula(boostSource.getBonusGoldMultiplierFormula(), statsSource).ToString()), boostSource.getBonusGoldMultiplierFormula()));
        }

        if (!boostSource.getBonusDiscountFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getDiscountBlock(DamageCalculator.calculateFormula(boostSource.getBonusDiscountFormula(), statsSource).ToString()), boostSource.getBonusDiscountFormula()));
        }

        if (!boostSource.getBonusVolleyAccuracyFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getVolleyBlock(DamageCalculator.calculateFormula(boostSource.getBonusVolleyAccuracyFormula(), statsSource).ToString()), boostSource.getBonusVolleyAccuracyFormula()));
        }

        #endregion

        #region Skills

        if (!boostSource.getBonusIntimidateChargesFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getIntimidateBlock(DamageCalculator.calculateFormula(boostSource.getBonusIntimidateChargesFormula(), statsSource).ToString()), boostSource.getBonusIntimidateChargesFormula()));
        }

        if (!boostSource.getBonusCunningChargesFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getCunningBlock(DamageCalculator.calculateFormula(boostSource.getBonusCunningChargesFormula(), statsSource).ToString()), boostSource.getBonusCunningChargesFormula()));
        }

        if (!boostSource.getBonusObservationLevelFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getObservationBlock(DamageCalculator.calculateFormula(boostSource.getBonusObservationLevelFormula(), statsSource).ToString()), boostSource.getBonusObservationLevelFormula()));
        }

        if (!boostSource.getBonusLeadershipUsesFormula().Equals(Constants.zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getLeadershipBlock(DamageCalculator.calculateFormula(boostSource.getBonusLeadershipUsesFormula(), statsSource).ToString()), boostSource.getBonusLeadershipUsesFormula()));
        }

        #endregion



        return blocks;
    }
}