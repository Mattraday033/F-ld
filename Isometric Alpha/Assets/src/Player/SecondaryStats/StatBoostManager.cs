using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IStatBoostSource : IDescribable
{
    public const string zeroRating = "0";

    #region Generic Stats
    
    public string getBonusCritFormula();
    public string getBonusDamageFormula();

    #endregion

    #region PrimaryStats

    public string getBonusStrengthFormula();
    public string getBonusDexterityFormula();
    public string getBonusWisdomFormula();
    public string getBonusCharismaFormula();

    #endregion

    #region Secondary Stats

    //Strength Stats
    public string getBonusPhysicalResistanceFormula();
    public string getBonusCriticalDamageMultiplierFormula();
    public string getBonusHealthFormula();

    //Dexterity Stats
    public string getBonusSurpriseRoundDamageFormula();
    public string getBonusArmorFormula();
    public string getBonusArmorPenetrationFormula();

    //Wisdom Stats
    public string getBonusPassiveSlotsFormula();
    public string getBonusWeaponSlotsFormula();
    public string getBonusMentalResistanceFormula();

    //Charisma Stats
    public string getBonusSynergyFormula();
    public string getBonusExuberancesFormula();
    public string getBonusZOIPotencyFormula();

    #endregion

    #region Party Stats

    public string getBonusRegenFormula();

    public string getBonusSurpriseRoundsFormula();
    public string getBonusRetreatChanceFormula();

    public string getBonusPartyActionsFormula();
    public string getBonusPartySlotsFormula();

    public string getBonusGoldMultiplierFormula();
    public string getBonusDiscountFormula();

    public string getBonusVolleyAccuracyFormula();

    #endregion

    #region Skills
    public string getBonusIntimidateChargesFormula();
    public string getBonusCunningChargesFormula();
    public string getBonusObservationLevelFormula();
    public string getBonusLeadershipUsesFormula();
    #endregion

    public Stats getStatSource();

    public static List<IStatBoostSource> getAllStatBoosts(IEnumerable statBoostList)
    {
        List<IStatBoostSource> statBoosts = new List<IStatBoostSource>();

        foreach (IStatBoostSource statBoost in statBoostList)
        {
            if (statBoost != null)
            {
                statBoosts.Add(statBoost);
            }
        }

        return statBoosts;
    }

    public static List<DescriptionPanelBuildingBlock> getStatBoostDescriptionBuildingBlocks(Stats statsSource, IStatBoostSource boostSource)
    {
        List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

        #region Generic Stats

        if (!boostSource.getBonusCritFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getCritBlock(DamageCalculator.calculateFormula(boostSource.getBonusCritFormula(), statsSource).ToString(), boostSource.getBonusCritFormula()));
        }

        if (!boostSource.getBonusDamageFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusDamageBlock(DamageCalculator.calculateFormula(boostSource.getBonusDamageFormula(), statsSource).ToString()), boostSource.getBonusDamageFormula()));
        }

        #endregion

        #region PrimaryStats

        if (!boostSource.getBonusStrengthFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getStrengthBlock(DamageCalculator.calculateFormula(boostSource.getBonusStrengthFormula(), statsSource).ToString()), boostSource.getBonusStrengthFormula()));
        }

        if (!boostSource.getBonusDexterityFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getDexterityBlock(DamageCalculator.calculateFormula(boostSource.getBonusDexterityFormula(), statsSource).ToString()), boostSource.getBonusDexterityFormula()));
        }

        if (!boostSource.getBonusWisdomFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getWisdomBlock(DamageCalculator.calculateFormula(boostSource.getBonusWisdomFormula(), statsSource).ToString()), boostSource.getBonusWisdomFormula()));
        }

        if (!boostSource.getBonusCharismaFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getCharismaBlock(DamageCalculator.calculateFormula(boostSource.getBonusCharismaFormula(), statsSource).ToString()), boostSource.getBonusCharismaFormula()));
        }
        #endregion

        #region Secondary Stats

        //Strength Stats

        if (!boostSource.getBonusHealthFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusHealthBlock(DamageCalculator.calculateFormula(boostSource.getBonusHealthFormula(), statsSource).ToString()), boostSource.getBonusHealthFormula()));
        }

        if (!boostSource.getBonusCriticalDamageMultiplierFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getCriticalHitDamageBlock(DamageCalculator.calculateFormula(boostSource.getBonusCriticalDamageMultiplierFormula(), statsSource).ToString() + "%"), boostSource.getBonusCriticalDamageMultiplierFormula()));
        }

        if (!boostSource.getBonusPhysicalResistanceFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getPhysicalResistBlock(DamageCalculator.calculateFormula(boostSource.getBonusPhysicalResistanceFormula(), statsSource).ToString() + "%"), boostSource.getBonusPhysicalResistanceFormula()));
        }

        //Dexterity Stats

        if (!boostSource.getBonusSurpriseRoundDamageFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getSurpriseRoundDamageMultiplierBlock(DamageCalculator.calculateFormula(boostSource.getBonusSurpriseRoundDamageFormula(), statsSource).ToString()), boostSource.getBonusSurpriseRoundDamageFormula()));
        }

        if (!boostSource.getBonusArmorFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getArmorBlock(DamageCalculator.calculateFormula(boostSource.getBonusArmorFormula(), statsSource).ToString()), boostSource.getBonusArmorFormula()));
        }

        if (!boostSource.getBonusArmorPenetrationFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getArmorPenetrationBlock(DamageCalculator.calculateFormula(boostSource.getBonusArmorPenetrationFormula(), statsSource).ToString() + "%"), boostSource.getBonusArmorPenetrationFormula()));
        }

        //Wisdom Stats

        if (!boostSource.getBonusPassiveSlotsFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getPassiveSlotsBlock(DamageCalculator.calculateFormula(boostSource.getBonusPassiveSlotsFormula(), statsSource).ToString()), boostSource.getBonusPassiveSlotsFormula()));
        }

        if (!boostSource.getBonusWeaponSlotsFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusWeaponSlotsBlock(DamageCalculator.calculateFormula(boostSource.getBonusWeaponSlotsFormula(), statsSource).ToString()), boostSource.getBonusWeaponSlotsFormula()));
        }

        if (!boostSource.getBonusMentalResistanceFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getMentalResistBlock(DamageCalculator.calculateFormula(boostSource.getBonusMentalResistanceFormula(), statsSource).ToString() + "%"), boostSource.getBonusMentalResistanceFormula()));
        }

        //Charisma Stats

        if (!boostSource.getBonusSynergyFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getSynergyBlock(DamageCalculator.calculateFormula(boostSource.getBonusSynergyFormula(), statsSource).ToString()), boostSource.getBonusSynergyFormula()));
        }

        if (!boostSource.getBonusExuberancesFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusExuberancesBlock(DamageCalculator.calculateFormula(boostSource.getBonusExuberancesFormula(), statsSource).ToString()), boostSource.getBonusExuberancesFormula()));
        }

        // if (!boostSource.getBonusZOIPotencyFormula().Equals(zeroRating))
        // {
        //     blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getZOIBlock(DamageCalculator.calculateFormula(boostSource.getBonusZOIPotencyFormula(), statsSource).ToString()), boostSource.getBonusZOIPotencyFormula()));
        // }

        #endregion

        #region Party Stats

        if (!boostSource.getBonusRegenFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getRegenBlock(DamageCalculator.calculateFormula(boostSource.getBonusRegenFormula(), statsSource).ToString()), boostSource.getBonusRegenFormula()));
        }

        if (!boostSource.getBonusRetreatChanceFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getRetreatChanceBlock(DamageCalculator.calculateFormula(boostSource.getBonusRetreatChanceFormula(), statsSource).ToString() + "%"), boostSource.getBonusRetreatChanceFormula()));
        }

        if (!boostSource.getBonusSurpriseRoundsFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getSurpriseRoundAmountBlock(DamageCalculator.calculateFormula(boostSource.getBonusSurpriseRoundsFormula(), statsSource).ToString()), boostSource.getBonusSurpriseRoundsFormula()));
        }

        if (!boostSource.getBonusPartySlotsFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getPartySlotsBlock(DamageCalculator.calculateFormula(boostSource.getBonusPartySlotsFormula(), statsSource).ToString()), boostSource.getBonusPartySlotsFormula()));
        }

        if (!boostSource.getBonusPartyActionsFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getPartyActionsBlock(DamageCalculator.calculateFormula(boostSource.getBonusPartyActionsFormula(), statsSource).ToString()), boostSource.getBonusPartyActionsFormula()));
        }

        if (!boostSource.getBonusGoldMultiplierFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getGoldMultiplierBlock(DamageCalculator.calculateFormula(boostSource.getBonusGoldMultiplierFormula(), statsSource).ToString()), boostSource.getBonusGoldMultiplierFormula()));
        }

        if (!boostSource.getBonusDiscountFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getDiscountBlock(DamageCalculator.calculateFormula(boostSource.getBonusDiscountFormula(), statsSource).ToString()), boostSource.getBonusDiscountFormula()));
        }

        if (!boostSource.getBonusVolleyAccuracyFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getVolleyBlock(DamageCalculator.calculateFormula(boostSource.getBonusVolleyAccuracyFormula(), statsSource).ToString()), boostSource.getBonusVolleyAccuracyFormula()));
        }

        #endregion

        #region Skills

        if (!boostSource.getBonusIntimidateChargesFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getIntimidateBlock(DamageCalculator.calculateFormula(boostSource.getBonusIntimidateChargesFormula(), statsSource).ToString()), boostSource.getBonusIntimidateChargesFormula()));
        }

        if (!boostSource.getBonusCunningChargesFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getCunningBlock(DamageCalculator.calculateFormula(boostSource.getBonusCunningChargesFormula(), statsSource).ToString()), boostSource.getBonusCunningChargesFormula()));
        }

        if (!boostSource.getBonusObservationLevelFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getObservationBlock(DamageCalculator.calculateFormula(boostSource.getBonusObservationLevelFormula(), statsSource).ToString()), boostSource.getBonusObservationLevelFormula()));
        }

        if (!boostSource.getBonusLeadershipUsesFormula().Equals(zeroRating))
        {
            blocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getLeadershipBlock(DamageCalculator.calculateFormula(boostSource.getBonusLeadershipUsesFormula(), statsSource).ToString()), boostSource.getBonusLeadershipUsesFormula()));
        }

        #endregion



        return blocks;
    }
}

public static class StatBoostManager
{

    // public static SecondaryStatBoost[] convertStatBoostKeys(string[] statBoostKeys)
    // {
    // 	return statBoostKeys.Aggregate(new SecondaryStatBoost[0], (a,b) => a = Helpers.appendArray<SecondaryStatBoost>(a, StatBoostList.getStatBoost(b)));
    // }

    // public static SecondaryStatBoost[] getAllPlayerStatBoosts()
    // {
    // 	return Helpers.appendArray<SecondaryStatBoost>(PartyManager.getPlayerStats().getAllStatBoostsFromTraits(), LessonManager.getAllStatBoosts());
    // }

    public delegate string SumFormulaDelegate<T>(T t);

    public static int calculateAllStatFormulas(Stats statSource, List<IStatBoostSource> statBoostSources, SumFormulaDelegate<IStatBoostSource> formulaDelegate)
    {
        string allFormulas = statBoostSources.Aggregate("", (a, b) => a = DamageCalculator.combineFormulas(a, formulaDelegate(b)));

        return DamageCalculator.calculateFormula(allFormulas, statSource);
    }

    public static double calculateAllStatFormulasAsPercentageDouble(Stats statSource, List<IStatBoostSource> statBoostSources, SumFormulaDelegate<IStatBoostSource> formulaDelegate)
    {
        string allFormulas = statBoostSources.Aggregate("", (a, b) => a = DamageCalculator.combineFormulas(a, formulaDelegate(b)));
        double percentage = ((double)DamageCalculator.calculateFormula(allFormulas, statSource)) / 100.0;

        return percentage;
    }

    public static float calculateAllStatFormulasAsPercentageFloat(Stats statSource, List<IStatBoostSource> statBoostSources, SumFormulaDelegate<IStatBoostSource> formulaDelegate)
    {
        string allFormulas = statBoostSources.Aggregate("", (a, b) => a = DamageCalculator.combineFormulas(a, formulaDelegate(b)));
        float percentage = ((float) DamageCalculator.calculateFormula(allFormulas, statSource)) / 100f;

        return percentage;
    }

    #region Generic Stats

    public static string getBonusCritFormula(IDescribable describable)
    {
        switch (describable.getName())
        {
            case ItemList.salvagedGuardGlovesKey:
                return "D";
            case ItemList.luckyTalismanKey:
                return "2";
        }

        return "0";
    }


    public static string getBonusDamageFormula(IDescribable describable)
    {
        switch (describable.getName())
        {
            case ItemList.luckyTalismanKey:
                return "4";
        }

        return "0";
    }

    #endregion

    #region PrimaryStats

    public static string getBonusStrengthFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusDexterityFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusWisdomFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusCharismaFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    #endregion

    #region Secondary Stats

    //Strength Stats
    public static string getBonusPhysicalResistanceFormula(IDescribable describable)
    {
        switch (describable.getName())
        {
            case ItemList.bronzeBadgeKey:
                return "10";
        }

        return "0";
    }

    public static string getBonusCriticalDamageMultiplierFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusHealthFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    //Dexterity Stats
    public static string getBonusSurpriseRoundDamageFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusArmorFormula(IDescribable describable)
    {
        switch (describable.getName())
        {
            case ItemList.martialArtistsBeltKey:
                return "2W";
            case ItemList.wardensShieldKey:
                return "2S";
        }

        return "0";
    }

    public static string getBonusArmorPenetrationFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    //Wisdom Stats
    public static string getBonusPassiveSlotsFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusWeaponSlotsFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusMentalResistanceFormula(IDescribable describable)
    {
        switch (describable.getName())
        {
            case ItemList.delversDreamKey:
                return "10";
        }

        return "0";
    }

    //Charisma Stats
    public static string getBonusSynergyFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusExuberancesFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusZOIPotencyFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    #endregion

    #region Party Stats

    public static string getBonusRegenFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusSurpriseRoundsFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusRetreatChanceFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusPartyActionsFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusPartySlotsFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusGoldMultiplierFormula(IDescribable describable)
    {
        switch (describable.getName())
        {
            case ItemList.silverSpoonKey:
                return "20";
        }

        return "0";
    }
    
    public static string getBonusDiscountFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusVolleyAccuracyFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    #endregion

    #region Skills
    public static string getBonusIntimidateChargesFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusCunningChargesFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusObservationLevelFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }

    public static string getBonusLeadershipUsesFormula(IDescribable describable)
    {
        switch (describable.getName())
        {

        }

        return "0";
    }
    #endregion

}
