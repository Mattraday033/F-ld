using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyStatsUpgradeDifference: IDescribable, IDescribableInBlocks
{
    private AllyStats lowerLevelStats;
    private AllyStats upperLevelStats;

    public AllyStatsUpgradeDifference(AllyStats lowerLevelStats, PrimaryStat PrimaryStat)
    {
        this.lowerLevelStats = lowerLevelStats;

        this.upperLevelStats = (AllyStats)lowerLevelStats.clone();
        upperLevelStats.incrementLevel(displayOnly: true);

        switch (PrimaryStat)
        {
            case PrimaryStat.Strength:
                upperLevelStats.incrementStrength(displayOnly: true);
                break;
            case PrimaryStat.Dexterity:
                upperLevelStats.incrementDexterity(displayOnly: true);
                break;
            case PrimaryStat.Wisdom:
                upperLevelStats.incrementWisdom(displayOnly: true);
                break;
            case PrimaryStat.Charisma:
                upperLevelStats.incrementCharisma(displayOnly: true);
                break;
        }
    }
    public string getDifferenceForDisplay(int higher, int lower)
    {
        return "+" + (higher - lower);
    }

    public string getDifferencePercentageForDisplay(double higher, double lower)
    {
        return "+" + Math.Round(((higher - lower)*100.0)) + "%";
    }

    public string getDifferencePercentageForDisplay(float higher, float lower)
    {
        return "+" + Math.Round(((higher - lower)*100f)) + "%";
    }

    public string getDifferencePercentageForDisplay(int higher, int lower)
    {
        return "+" + (higher - lower) + "%";
    }

    public string displayName { get { return lowerLevelStats.displayName + " Ally Stats Difference"; } }
    public string uniqueName { get { return lowerLevelStats.uniqueName + " Ally Stats Difference"; } }

    #region IDescribable Methods

    public bool ineligible()
    {
        return false;
    }

    public GameObject getRowType(RowType rowType)
    {
        return null;
    }

	public GameObject getDescriptionPanelFull()
    {
        return null;
    }

	public GameObject getDescriptionPanelFull(PanelType type)
    {
        return null;
    }

	public GameObject getDecisionPanel()
    {
        return null;
    }

	public bool withinFilter(string[] filterParameters)
    {
        return false;
    }

    public void describeSelfFull(DescriptionPanel panel)
    {

    }

    public void describeSelfRow(DescriptionPanel panel)
    {

    }

    public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

	public List<IDescribable> getRelatedDescribables()
	{
		return new List<IDescribable>();
	}

	public bool buildableWithBlocks()
    {
        return true;
    }
	public bool buildableWithBlocksRows()
    {
        return true;
    }
    #endregion

    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> listOfBlocks = new List<DescriptionPanelBuildingBlock>();

        #region Main Stat Differences

        if (upperLevelStats.level > lowerLevelStats.level)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getLevelBlock(getDifferenceForDisplay(upperLevelStats.level, lowerLevelStats.level)));
        }

        if (upperLevelStats.totalHealth > lowerLevelStats.totalHealth)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(getDifferenceForDisplay(upperLevelStats.totalHealth, lowerLevelStats.totalHealth)));
        }
        #endregion

        #region Strength Differences

        if (upperLevelStats.totalStrength > lowerLevelStats.totalStrength)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getStrengthBlock(getDifferenceForDisplay(upperLevelStats.totalStrength, lowerLevelStats.totalStrength)));
        }

        if (upperLevelStats.critDamageMultiplier > lowerLevelStats.critDamageMultiplier)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getCriticalHitDamageBlock(getDifferencePercentageForDisplay(upperLevelStats.critDamageMultiplier, lowerLevelStats.critDamageMultiplier)));
        }

        if (upperLevelStats.woundResistance > lowerLevelStats.woundResistance)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getWoundResistBlock(getDifferencePercentageForDisplay(upperLevelStats.woundResistance, lowerLevelStats.woundResistance)));
        }

        #endregion

        #region Dexterity Differences

        if (upperLevelStats.totalDexterity > lowerLevelStats.totalDexterity)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getDexterityBlock(getDifferenceForDisplay(upperLevelStats.totalDexterity, lowerLevelStats.totalDexterity)));
        }

        if (upperLevelStats.surpriseDamageMultiplier > lowerLevelStats.surpriseDamageMultiplier)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getSurpriseRoundDamageMultiplierBlock(getDifferencePercentageForDisplay(upperLevelStats.surpriseDamageMultiplier, lowerLevelStats.surpriseDamageMultiplier)));
        }

        if (upperLevelStats.armorPenetration > lowerLevelStats.armorPenetration)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getArmorPenetrationBlock(getDifferencePercentageForDisplay(upperLevelStats.armorPenetration, lowerLevelStats.armorPenetration)));
        }

        if (upperLevelStats.extraArmorFromDexterity > lowerLevelStats.extraArmorFromDexterity)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getExtraArmorBlock(getDifferenceForDisplay(upperLevelStats.extraArmorFromDexterity, lowerLevelStats.extraArmorFromDexterity)));
        }
        #endregion

        #region Wisdom Differences

        if (upperLevelStats.totalWisdom > lowerLevelStats.totalWisdom)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getWisdomBlock(getDifferenceForDisplay(upperLevelStats.totalWisdom, lowerLevelStats.totalWisdom)));
        }

        if (upperLevelStats.passiveSlotsUnlocked > lowerLevelStats.passiveSlotsUnlocked)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getPassiveSlotsBlock(getDifferenceForDisplay(upperLevelStats.passiveSlotsUnlocked, lowerLevelStats.passiveSlotsUnlocked)));
        }

        if (upperLevelStats.weaponSlots > lowerLevelStats.weaponSlots)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getBonusWeaponSlotsBlock(getDifferenceForDisplay(upperLevelStats.weaponSlots, lowerLevelStats.weaponSlots)));
        }

        if (upperLevelStats.mentalResistance > lowerLevelStats.mentalResistance)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getMentalResistBlock(getDifferencePercentageForDisplay(upperLevelStats.mentalResistance, lowerLevelStats.mentalResistance)));
        }
        #endregion

        #region Charisma Differences

        if (upperLevelStats.totalCharisma > lowerLevelStats.totalCharisma)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getCharismaBlock(getDifferenceForDisplay(upperLevelStats.totalCharisma, lowerLevelStats.totalCharisma)));
        }

        if (upperLevelStats.synergyCoefficient > lowerLevelStats.synergyCoefficient)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getSynergyBlock(getDifferenceForDisplay(upperLevelStats.synergyCoefficient, lowerLevelStats.synergyCoefficient)));
        }

        if (upperLevelStats.bonusExuberances > lowerLevelStats.bonusExuberances)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getBonusExuberancesBlock(getDifferenceForDisplay(upperLevelStats.bonusExuberances, lowerLevelStats.bonusExuberances)));
        }

        if (upperLevelStats.totalCharisma > lowerLevelStats.totalCharisma)
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getZOIBlock(getDifferenceForDisplay(upperLevelStats.totalCharisma, lowerLevelStats.totalCharisma), lowerLevelStats.zoneOfInfluenceTrait.getIconName()));
        }

        #endregion

        return listOfBlocks;
    }

    public bool requiresInspectNode()
    {
        return false;
    }

}
