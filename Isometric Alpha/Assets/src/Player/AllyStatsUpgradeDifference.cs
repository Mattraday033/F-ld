using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyStatsUpgradeDifference: IDescribable, IDescribableInBlocks
{
    private AllyStats lowerLevelStats;
    private AllyStats upperLevelStats;

    public AllyStatsUpgradeDifference(AllyStats lowerLevelStats, PrimaryStat statType)
    {
        this.lowerLevelStats = lowerLevelStats;

        this.upperLevelStats = (AllyStats)lowerLevelStats.clone();
        upperLevelStats.incrementLevel();

        switch (statType)
        {
            case PrimaryStat.Strength:
                upperLevelStats.incrementStrength();
                break;
            case PrimaryStat.Dexterity:
                upperLevelStats.incrementDexterity();
                break;
            case PrimaryStat.Wisdom:
                upperLevelStats.incrementWisdom();
                break;
            case PrimaryStat.Charisma:
                upperLevelStats.incrementCharisma();
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

    public string getName()
    {
        return lowerLevelStats.getName() + " Ally Stats Difference";
    }

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

	public ArrayList getRelatedDescribables()
    {
        return new ArrayList();
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

        if (upperLevelStats.getLevel() > lowerLevelStats.getLevel())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getLevelBlock(getDifferenceForDisplay(upperLevelStats.getLevel(), lowerLevelStats.getLevel())));
        }

        if (upperLevelStats.getTotalHealth() > lowerLevelStats.getTotalHealth())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getHealthBlock(getDifferenceForDisplay(upperLevelStats.getTotalHealth(), lowerLevelStats.getTotalHealth())));
        }
        #endregion

        #region Strength Differences

        if (upperLevelStats.getStrength() > lowerLevelStats.getStrength())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getStrengthBlock(getDifferenceForDisplay(upperLevelStats.getStrength(), lowerLevelStats.getStrength())));
        }

        if (upperLevelStats.getCritDamageMultiplier() > lowerLevelStats.getCritDamageMultiplier())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getCriticalHitDamageBlock(getDifferencePercentageForDisplay(upperLevelStats.getCritDamageMultiplier(), lowerLevelStats.getCritDamageMultiplier())));
        }

        if (upperLevelStats.getPhysicalResistance() > lowerLevelStats.getPhysicalResistance())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getPhysicalResistBlock(getDifferencePercentageForDisplay(upperLevelStats.getPhysicalResistance(), lowerLevelStats.getPhysicalResistance())));
        }

        #endregion

        #region Dexterity Differences

        if (upperLevelStats.getDexterity() > lowerLevelStats.getDexterity())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getDexterityBlock(getDifferenceForDisplay(upperLevelStats.getDexterity(), lowerLevelStats.getDexterity())));
        }

        if (upperLevelStats.getSurpriseDamageMultiplier() > lowerLevelStats.getSurpriseDamageMultiplier())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getSurpriseRoundDamageMultiplierBlock(getDifferencePercentageForDisplay(upperLevelStats.getSurpriseDamageMultiplier(), lowerLevelStats.getSurpriseDamageMultiplier())));
        }

        if (upperLevelStats.getArmorPenetration() > lowerLevelStats.getArmorPenetration())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getArmorPenetrationBlock(getDifferencePercentageForDisplay(upperLevelStats.getArmorPenetration(), lowerLevelStats.getArmorPenetration())));
        }

        if (upperLevelStats.getExtraArmorFromDexterity() > lowerLevelStats.getExtraArmorFromDexterity())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getExtraArmorBlock(getDifferenceForDisplay(upperLevelStats.getExtraArmorFromDexterity(), lowerLevelStats.getExtraArmorFromDexterity())));
        }
        #endregion

        #region Wisdom Differences

        if (upperLevelStats.getWisdom() > lowerLevelStats.getWisdom())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getWisdomBlock(getDifferenceForDisplay(upperLevelStats.getWisdom(), lowerLevelStats.getWisdom())));
        }

        if (upperLevelStats.getPassiveSlotsUnlocked() > lowerLevelStats.getPassiveSlotsUnlocked())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getPassiveSlotsBlock(getDifferenceForDisplay(upperLevelStats.getPassiveSlotsUnlocked(), lowerLevelStats.getPassiveSlotsUnlocked())));
        }

        if (upperLevelStats.getWeaponSlots() > lowerLevelStats.getWeaponSlots())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getBonusWeaponSlotsBlock(getDifferenceForDisplay(upperLevelStats.getWeaponSlots(), lowerLevelStats.getWeaponSlots())));
        }

        if (upperLevelStats.getMentalResistance() > lowerLevelStats.getMentalResistance())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getMentalResistBlock(getDifferencePercentageForDisplay(upperLevelStats.getMentalResistance(), lowerLevelStats.getMentalResistance())));
        }
        #endregion

        #region Charisma Differences

        if (upperLevelStats.getCharisma() > lowerLevelStats.getCharisma())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getCharismaBlock(getDifferenceForDisplay(upperLevelStats.getCharisma(), lowerLevelStats.getCharisma())));
        }

        if (upperLevelStats.getSynergyCoefficient() > lowerLevelStats.getSynergyCoefficient())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getSynergyBlock(getDifferenceForDisplay(upperLevelStats.getSynergyCoefficient(), lowerLevelStats.getSynergyCoefficient())));
        }

        if (upperLevelStats.getBonusExuberances() > lowerLevelStats.getBonusExuberances())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getBonusExuberancesBlock(getDifferenceForDisplay(upperLevelStats.getBonusExuberances(), lowerLevelStats.getBonusExuberances())));
        }

        if (upperLevelStats.getCharisma() > lowerLevelStats.getCharisma())
        {
            listOfBlocks.Add(DescriptionPanelBuildingBlock.getZOIBlock(getDifferenceForDisplay(upperLevelStats.getCharisma(), lowerLevelStats.getCharisma()), lowerLevelStats.getZoneOfInfluenceTrait().getIconName()));
        }

        #endregion

        return listOfBlocks;
    }

}
