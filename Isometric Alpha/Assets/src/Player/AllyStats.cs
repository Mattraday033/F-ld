using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using System.Linq;

public enum PrimaryStat{Strength = 0, Dexterity = 1, Wisdom = 2, Charisma = 3, None = 4}

public class AllyStats : Stats
{
    #region Constants

    public const int xpNeededToLevelUp = 1000;

    public const string playerCombatSpriteName = "PlayerSprite";

    private const int baseNumberOfPartyMembers = 2;
    private const int maxNumberOfPartyMembers = 16;
    private const int baseNumberOfPartyCombatActionPoints = 5;
    private const int actionPointsPerPartyAction = 5;
    private const int minimumPartycombatActionArray = 1;
    private const int maximumPartycombatActionArray = 6;

    private const string combatantTypeDescription = "Party Leader";
    private const string zoiIconBackgroundName = "ZOI-Icon";
    public const string ZOIStatBoostKey = "PartyMemberBoost";

    public const string nandorPersistentInfluenceStatBoostKey = "nandorPersistentInfluence";
    public const string redStalwartInfluenceStatBoostKey = "redStalwartInfluence";
    public const string carterCleverInfluenceStatBoostKey = "carterCleverInfluence";

    public const int playerLevelMaximum = 20;
    public const int playerActionWheelLength = 8;
    private const int playerHealthPerLevelAboveOne = 10;
    private const int playerBaseHealth = 90;

    public const int statMaximum = 10;

    public const int defaultStartingRow = 0;
    public const int defaultStartingCol = 1;

    #endregion

    #region Global Variables

    public int strength { private get; set; }
    public int dexterity { private get; set; }
    public int wisdom { private get; set; }
    public int charisma { private get; set; }

    private int level;
    public int xp;

    private int currentlyPlacedPartyMembers = -1;

    public CombatActionArray combatActionArray;
    public EquippedItems equippedItems;

    #endregion

    #region Constructors

    public AllyStats() : base("")
    {
        combatActionArray = new CombatActionArray(this); 
        equippedItems = new EquippedItems(this);
    }

    public AllyStats(string name, int Str, int Dex, int Wis, int Cha, int lvl, int xp, int cHP, int cunningsRemaining) : base(name)
    {
        this.name = name;

        this.strength = Str;
        this.dexterity = Dex;
        this.wisdom = Wis;
        this.charisma = Cha;

        this.level = lvl;
        this.xp = xp;

        this.currentHealth = cHP;

        combatActionArray = new CombatActionArray(this); 
        equippedItems = new EquippedItems(this);
    }

    public AllyStats(string name, int Str, int Dex, int Wis, int Cha) : base(name) 
    {
        this.name = name;

        this.strength = Str;
        this.dexterity = Dex;
        this.wisdom = Wis;
        this.charisma = Cha;

        this.level = 1;
        this.xp = 0;

        combatActionArray = new CombatActionArray(this); 
        equippedItems = new EquippedItems(this);

        this.currentHealth = getTotalHealth();
    }

    public AllyStats(StatsWrapper wrapper) : base(wrapper.key)
    {
        this.name = wrapper.key;

        this.strength = wrapper.strength;
        this.dexterity = wrapper.dexterity;
        this.wisdom = wrapper.wisdom;
        this.charisma = wrapper.charisma;

        this.level = wrapper.level;
        this.xp = wrapper.xp;
        this.currentHealth = wrapper.currentHealth;

        combatActionArray = new CombatActionArray(this, SaveBlueprint.extractCombatActionsFromJson(wrapper.combatActions));
        equippedItems = new EquippedItems(this, SaveBlueprint.extractEquippedItemsFromJson(wrapper.currentEquipment));
    }

    #endregion

    #region Sprite and GameObject

    public override Color getSpriteColor()
    {
        switch (getName())
        {
            case NPCNameList.thatch:
                return Color.red;
            case NPCNameList.carter:
                return Color.green;
            case NPCNameList.nandor:
                return Color.yellow;
            default:
                return Color.white;

        }
    }

    public override string getCombatSpriteName()
    {
        return playerCombatSpriteName;
    }

    public override GameObject instantiateCombatSprite()
    {
        base.instantiateCombatSprite();

        // Debug.LogError(getName() + " is now " + spriteColor.ToString());

        combatSprite.GetComponent<SpriteRenderer>().color = getSpriteColor();

        combatSprite.GetComponent<AbilityMenuManager>().actionArraySource = this;

        Helpers.updateGameObjectPosition(combatSprite);

        return combatSprite;
    }

    #endregion

    #region Level/XP
    public void addXP(int earnedXP)
    {
        xp += earnedXP;
    }

    public void removeXPFromLevelUp()
    {
        xp = xp % xpNeededToLevelUp;
    }

    public void removeXPFromLevelUpOnce()
    {
        xp -= xpNeededToLevelUp;
    }

    public int getLevelUpsAvailable()
    {
        return xp / xpNeededToLevelUp;
    }

    public override int getLevel()
    {
        return level;
    }

    public override void setLevel(int newLevel)
    {
        this.level = newLevel;
    }

    public override void incrementLevel()
    {
        level++;
    }

    public virtual int getLevelMaximum()
    {
        return playerLevelMaximum;
    }

    #endregion

    #region Health

    public virtual int getBaseHealth()
    {
        return playerBaseHealth;
    }

    public virtual int getHealthPerLevelAboveOne()
    {
        return playerHealthPerLevelAboveOne;
    }

    public override int getTotalHealth()
    {
        int healthFromLevel = playerBaseHealth + (playerHealthPerLevelAboveOne * (level - 1));
        int healthFromStrength = Strength.getHealthFromStrength(strength);

        int bonusFormulas = StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusHealthFormula());

        return healthFromLevel + healthFromStrength +  bonusFormulas;
    }


    public static int calculateTotalHealth(int potentialLevel, int potentialStrength, List<IStatBoostSource> statBoostSources)
    {
        AllyStats dummyPlayer = new AllyStats();

        dummyPlayer.level = potentialLevel;
        dummyPlayer.strength = potentialStrength;

        int bonusFormulas = StatBoostManager.calculateAllStatFormulas(dummyPlayer, statBoostSources, b => b.getBonusHealthFormula());

        return dummyPlayer.getTotalHealth() + bonusFormulas;
    }

    #endregion

    #region Primary Stats
    private int[] getStatsAsArray()
    {
        return new int[] { strength, dexterity, wisdom, charisma };
    }

    public PrimaryStat getHighestStat()
    {
        int[] primaryStats = getStatsAsArray();

        int highestStat = 0;
        int highestStatIndex = 0;
        int currentIndex = 0;

        foreach (int stat in primaryStats)
        {
            if (stat > highestStat)
            {
                highestStat = stat;
                highestStatIndex = currentIndex;
            }

            currentIndex++;
        }

        return (PrimaryStat)highestStatIndex;
    }

    public static PrimaryStat convertStringToPrimaryStat(string statName)
    {
        switch (statName)
        {
            case "Strength":
                return PrimaryStat.Strength;
            case "Dexterity":
                return PrimaryStat.Dexterity;
            case "Wisdom":
                return PrimaryStat.Wisdom;
            case "Charisma":
                return PrimaryStat.Charisma;
            default:
                return PrimaryStat.None;
        }
    }

    public bool meetsStatRequirements(PrimaryStat statType, int statLevel)
    {
        switch (statType)
        {
            case PrimaryStat.Strength:

                return strength >= statLevel;

            case PrimaryStat.Dexterity:

                return dexterity >= statLevel;

            case PrimaryStat.Wisdom:

                return wisdom >= statLevel;

            case PrimaryStat.Charisma:

                return charisma >= statLevel;

            default:
                return true;
        }
    }

    public List<PrimaryStat> getHighestPrimaryStats()
    {
        int[] allPrimaryStats = new int[] { strength, dexterity, wisdom, charisma };
        List<PrimaryStat> highestStats = new List<PrimaryStat>();

        int highestStatLvl = allPrimaryStats.Max(x => x);

        if (strength >= highestStatLvl)
        {
            highestStats.Add(PrimaryStat.Strength);
        }

        if (dexterity >= highestStatLvl)
        {
            highestStats.Add(PrimaryStat.Dexterity);
        }

        if (wisdom >= highestStatLvl)
        {
            highestStats.Add(PrimaryStat.Wisdom);
        }

        if (charisma >= highestStatLvl)
        {
            highestStats.Add(PrimaryStat.Charisma);
        }

        if (highestStatLvl <= 1)
        {
            //Debug.LogError("No primary stats are above 1");
            return new List<PrimaryStat>() { PrimaryStat.None };
        }
        if (highestStats.Count >= 4)
        {
            //Debug.LogError("All primary stats are at the same level");
            return new List<PrimaryStat>() { PrimaryStat.None };
        }

        return highestStats;
    }

    #region Strength + Secondaries

    public int getStrengthWithoutBoosts()
    {
        return strength;
    }

    public override int getStrength()
    {
        return strength + StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusStrengthFormula());
    }

    public void incrementStrength()
    {
        strength++;
    }

    public override double getCritDamageMultiplier()
    {
        double bonusFormulas = StatBoostManager.calculateAllStatFormulasAsPercentageDouble(this, getAllStatBoosts(), b => b.getBonusCriticalDamageMultiplierFormula());

        return (DamageCalculator.baseCriticalDamage + (Strength.critDamMultPerStrengthDouble * ((double)getStrength()))) + bonusFormulas;
    }

    public string getExtraCritDamageForDisplay()
    {
        int bonusFormulas = StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusCriticalDamageMultiplierFormula());

        return (getStrength() * Strength.critDamMultPerStrength) + bonusFormulas + "%";
    }

    public double getPhysicalResistance()
    {
        double bonusFormulas = StatBoostManager.calculateAllStatFormulasAsPercentageDouble(this, getAllStatBoosts(), b => b.getBonusPhysicalResistanceFormula());

        return Strength.physResistBaseDouble + (((double)getStrength()) * Strength.physResistPerStrengthDouble) + bonusFormulas;
    }

    public string getPhysicalResistanceForDisplay()
    {
        int bonusFormulas = StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusPhysicalResistanceFormula());

        return Strength.physResistBase + (getStrength() * Strength.physResistPerStrength) + bonusFormulas + "%";
    }

    #endregion

    #region Dexterity + Secondaries

    public int getDexterityWithoutBoosts()
    {
        return dexterity;
    }

    public override int getDexterity()
    {
        return dexterity + StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusDexterityFormula());
    }

    public void incrementDexterity()
    {
        dexterity++;
    }

    public override float getSurpriseDamageMultiplier()
    {
        float bonusFormulas = StatBoostManager.calculateAllStatFormulasAsPercentageFloat(this, getAllStatBoosts(), b => b.getBonusSurpriseRoundDamageFormula());

        return Dexterity.surpriseDamMultBase + (((float)getDexterity()) * Dexterity.surpriseDamMultCoefficient) + bonusFormulas;
    }

    public string getSurpriseDamageMultiplierForDisplay()
    {
        float bonusFormulas = StatBoostManager.calculateAllStatFormulasAsPercentageFloat(this, getAllStatBoosts(), b => b.getBonusSurpriseRoundDamageFormula());

        return (((float)getDexterity()) * Dexterity.surpriseDamMultCoefficient + bonusFormulas) * 100f + "%";
    }

    public override int getExtraArmorFromDexterity()
    {
        int bonusFormulas = StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusArmorFormula());

        return getDexterity() * Dexterity.extraArmorMultiplier + bonusFormulas;
    }

    #endregion

    #region Wisdom + Secondaries

    public int getWisdomWithoutBoosts()
    {
        return wisdom;
    }

    public override int getWisdom()
    {
        return wisdom + StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusWisdomFormula());
    }

    public void incrementWisdom()
    {
        wisdom++;
    }

    public float getArmorPenetration()
    {
        float baseArmorPen = 0;

        if (getWisdom() < Wisdom.minorArmorPenetrationLevel)
        {
            baseArmorPen = 0;
        }
        else if (getWisdom() < Wisdom.lesserArmorPenetrationLevel)
        {
            baseArmorPen = .05f;
        }
        else if (getWisdom() < Wisdom.improvedArmorPenetrationLevel)
        {
            baseArmorPen = .10f;
        }
        else if (getWisdom() < Wisdom.greaterArmorPenetrationLevel)
        {
            baseArmorPen = .15f;
        }
        else if (getWisdom() < Wisdom.majorArmorPenetrationLevel)
        {
            baseArmorPen = .20f;
        }
        else
        {
            baseArmorPen = .25f;
        }

        float bonusFormulas = StatBoostManager.calculateAllStatFormulasAsPercentageFloat(this, getAllStatBoosts(), b => b.getBonusArmorPenetrationFormula());

        return (baseArmorPen + bonusFormulas);
    }

    public string getArmorPenetrationForDisplay()
    {
        return (getArmorPenetration() * 100f) + "%";
    }

    public double getMentalResistance()
    {
        double bonusFormulas = StatBoostManager.calculateAllStatFormulasAsPercentageDouble(this, getAllStatBoosts(), b => b.getBonusMentalResistanceFormula());

        return Wisdom.mentalResistBaseDouble + (((double)getWisdom()) * Wisdom.mentalResistPerWisdomDouble) + (bonusFormulas / 100.0);
    }

    public string getMentalResistanceForDisplay()
    {
        int bonusFormulas = StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusMentalResistanceFormula());

        return (Wisdom.mentalResistBase + (getWisdom() * Wisdom.mentalResistPerWisdom) + bonusFormulas) + "%";
    }

    public override int getWeaponSlots()
    {
        int weaponSlotsFromWisdom = 1;

        if (getWisdom() >= 3 && getWisdom() < 7)
        {
            weaponSlotsFromWisdom = 2;
        }
        else if (getWisdom() >= 7)
        {
            weaponSlotsFromWisdom = Wisdom.maxNumberOfWeaponSlots;
        }

        int bonusFormula = StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusWeaponSlotsFormula());

        if ((weaponSlotsFromWisdom + bonusFormula) > Wisdom.maxNumberOfWeaponSlots)
        {
            return Wisdom.maxNumberOfWeaponSlots;
        }
        else
        {
            return weaponSlotsFromWisdom + bonusFormula;
        }
    }

    public override int getPassiveSlotsUnlocked()
    {
        int passivesUnlockedFromWisdom = 1;

        switch (wisdom)
        {
            case >= Wisdom.thirdPassiveSlotUnlockLevel:
                passivesUnlockedFromWisdom = Wisdom.maximumPassiveSlots;
                break;
            case >= Wisdom.secondPassiveSlotUnlockLevel:
                passivesUnlockedFromWisdom = 3;
                break;
            case >= Wisdom.firstPassiveSlotUnlockLevel:
                passivesUnlockedFromWisdom = 2;
                break;
        }

        int bonusFormula = StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusPassiveSlotsFormula());

        if (passivesUnlockedFromWisdom + bonusFormula > Wisdom.maximumPassiveSlots)
        {
            return Wisdom.maximumPassiveSlots;
        }
        else
        {
            return passivesUnlockedFromWisdom + bonusFormula;
        }
    }

    public int getMaximumRepositionsPerCombat()
    {
        if (wisdom < Wisdom.oneRepositionLevel)
        {
            return 0;
        }
        else if (wisdom < Wisdom.twoRepositionLevel)
        {
            return 1;
        }
        else if (wisdom < Wisdom.threeRepositionLevel)
        {
            return 2;
        }
        else if (wisdom < Wisdom.fourRepositionLevel)
        {
            return 3;
        }
        else if (wisdom < Wisdom.fiveRepositionLevel)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    #endregion

    #region Charisma + Secondaries

    public int getCharismaWithoutBoosts()
    {
        return charisma;
    }

    public override int getCharisma()
    {
        return charisma + StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusCharismaFormula());
    }

    public void incrementCharisma()
    {
        charisma++;
    }

    public override int getBonusExuberances()
    {
        return charisma/3;
    }

    public override int getSynergyCoefficient()
    {
        return (getCharisma() * Charisma.playerSynergyModifierCoefficient) + StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusSynergyFormula());
    }

    public string getSynergyCoefficientForDisplay()
    {
        return getSynergyCoefficient() + "";
    }

    #endregion

    #endregion

    #region Combat And Action Array

    public override int getBonusAbilityDamage()
    {
        return combatActionArray.calculateBonusAbilityDamage() + StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusDamageFormula());
    }

    public override AbilityMenuManager getAbilityMenuManager()
    {
        return combatSprite.GetComponent<AbilityMenuManager>();
    }

    public override CombatActionArray getActionArray()
    {
        return combatActionArray;
    }

	public override bool costsPartyCombatActions()
	{
		return true;
	}

	public void resetAllCooldowns()
	{
        combatActionArray.resetAllCooldowns();
	}


    #endregion

    #region Traits

    public override void addEquippedPassiveTraits()
    {
        List<Trait> equippedPassiveTraits = combatActionArray.getAllEquippedPassiveTraits();

        foreach (Trait trait in equippedPassiveTraits)
        {
            if (trait == null)
            {
                continue;
            }

            addTrait(trait);
        }
    }

    // public override SecondaryStatBoost[] getAllStatBoostsFromTraits()
    // {
    //     return Helpers.appendArray<SecondaryStatBoost>(base.getAllStatBoostsFromTraits(), getZoneOfInfluenceTrait().getStatBoosts());
    // }

    // public override string[] getAllStatBoostKeysFromTraits()
    // {
    //     string[] statBoostKeys = base.getAllStatBoostKeysFromTraits();

    //     return Helpers.appendArray<string>(statBoostKeys, getName() + ZOIStatBoostKey);
    // }

    #endregion

    #region Zone of Influence

    public override Trait getZoneOfInfluenceTrait()
    {
        string[] allStatBoostKeys = getAllZoneOfInfluenceBoostKeys(State.lessonsLearned);
        string[] zoiStatBoostKeys = new string[0];

        foreach (string statBoostKey in allStatBoostKeys)
        {
            SecondaryStatBoost boost = StatBoostList.getStatBoost(statBoostKey);

            if (boost.affectsZone && boost.sourceName.Equals(getName()) || boost.sourceName.Equals(""))
            {
                zoiStatBoostKeys = Helpers.appendArray<string>(zoiStatBoostKeys, statBoostKey);
            }
        }

        return new ZoneOfInfluenceTrait(getName() + zoiTraitName, getZOITraitDescription(), zoiIconBackgroundName, zoiStatBoostKeys);
    }

    public string getZOITraitDescription()
    {
        switch (getName())
        {
            case NPCNameList.thatch:
                return "Creature's in this companion's Zone of Influence gain 30 extra armor. This increases to 60 if the companion is at least level 4.";
            case NPCNameList.carter:
                return "Creature's in this companion's Zone of Influence deal 10% extra damage during a surprise round. This increases to 20% if the companion is at least level 4.";
            case NPCNameList.nandor:
                return "Creature's in this companion's Zone of Influence gain 20% extra Mental Resistance. This increases to 40% if the companion is at least level 4.";
            default:
                return zoiTraitDescription;
        }
    }
    
    #endregion

    #region Equipment
    public override EquippedItems getEquippedItems()
    {
        return equippedItems;
    }

    public override string getBonusCritChanceFromArmor()
    {
        return "" + StatBoostManager.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusCritFormula());
    }

    public override int getTotalArmorRating()
    {
        int totalArmorRating = 0;

        foreach (EquippableItem item in equippedItems)
        {
            if (item == null)
            {
                continue;
            }

            totalArmorRating += item.getArmorRating();
        }

        totalArmorRating += getExtraArmorFromDexterity();

        return (int)((double)totalArmorRating * getCurrentTotalArmorPercentage());
    }

    public override bool hasAvailableWeaponSlots()
    {
        return combatActionArray.hasAvailableWeaponSlots();
    }

    #endregion

    #region Miscellaneous

    public List<IStatBoostSource> getAllStatBoosts()
    {
        List<IStatBoostSource> boosts = new List<IStatBoostSource>();

        boosts.AddRange(IStatBoostSource.getAllStatBoosts(combatActionArray));
        boosts.AddRange(IStatBoostSource.getAllStatBoosts(equippedItems));
        boosts.AddRange(IStatBoostSource.getAllStatBoosts(traits));

        return boosts;
    }

    public override float getDevastatingCriticalPercentage()
    {
        if (hasTrait(TraitList.devastatingCriticals) < 0)
        {
            return 0f;
        }

        float devastatingCriticalPercentage = (float)dexterity / 100f;

        if (CombatStateManager.isPlayerSurpriseRound())
        {
            return devastatingCriticalPercentage;
        }
        else
        {
            return devastatingCriticalPercentage * 2f;
        }
    }

    public Story addAllStats(Story currentStory)
    {
        if (currentStory.variablesState["playerName"] != null)
        {
            currentStory.variablesState["playerName"] = getName();
        }

        if (currentStory.variablesState["strength"] != null)
        {
            currentStory.variablesState["strength"] = strength;
        }

        if (currentStory.variablesState["dexterity"] != null)
        {
            currentStory.variablesState["dexterity"] = dexterity;
        }

        if (currentStory.variablesState["wisdom"] != null)
        {
            currentStory.variablesState["wisdom"] = wisdom;
        }

        if (currentStory.variablesState["charisma"] != null)
        {
            currentStory.variablesState["charisma"] = charisma;
        }

        return currentStory;
    }

    public override bool removableFromFormation()
    {
        if (this == PartyManager.getPlayerStats())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion

    #region IDescribable

	public override GameObject getRowType(RowType rowType)
	{
		return Resources.Load<GameObject>(PrefabNames.party2x3GridSection);
	}

    public override void describeSelfFull(DescriptionPanel panel)
    {
        base.describeSelfFull(panel);

        DescriptionPanel.setText(panel.typeText, combatantTypeDescription);

        if (panel.iconPanel != null)
        {
            panel.iconPanel.gameObject.SetActive(true);
        }

        DescriptionPanel.setImageColor(panel.iconBackgroundPanel, new Color32(125,125,125,255));
        DescriptionPanel.setImageColor(panel.iconPanel, getSpriteColor());
    }

    #endregion

    #region IDescribableInBlocks

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {

        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        // if (!CombatStateManager.inCombat)
        // {
        //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getIntimidateBlock(getMaxIntimidateCount().ToString()));
        //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getCunningBlock(getMaxCunningCount().ToString()));
        //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getObservationBlock(getObservationLevel().ToString()));
        //     buildingBlocks.Add(DescriptionPanelBuildingBlock.getLeadershipBlock(getMaxPlacablePartyMembers().ToString()));
        // }

        // buildingBlocks.Add(DescriptionPanelBuildingBlock.getPartySlotsBlock(getMaximumPartyMemberSlots().ToString()));



        // buildingBlocks.Add(DescriptionPanelBuildingBlock.getAffinityMultiplierBlock((State.playerStats.getAffinityCoefficient() *
        //                      (PartyManager.getCurrentPartyMemberSlotsUsed() - 1)).ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getLevelBlock(getLevel().ToString()));

        if (!CombatStateManager.inCombat)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getExperienceBlock(xp.ToString()));
        }

        buildingBlocks.AddRange(base.getDescriptionBuildingBlocks());

        if (!CombatStateManager.inCombat)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getWorthBlock(Purse.getCoinsInPurse().ToString()));
        }

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusDamageBlock(getBonusAbilityDamage().ToString()));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getStrengthBlock(getStrength().ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCriticalHitDamageBlock(getExtraCritDamageForDisplay().ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getPhysicalResistBlock(getPhysicalResistanceForDisplay().ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusHealthBlock(Strength.getHealthFromStrength(getStrength()).ToString()));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDexterityBlock(getDexterity().ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getSurpriseRoundDamageMultiplierBlock(getSurpriseDamageMultiplierForDisplay()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getExtraArmorBlock(getExtraArmorFromDexterity().ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorPenetrationBlock(getArmorPenetrationForDisplay()));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWisdomBlock(getWisdom().ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getPassiveSlotsBlock(getPassiveSlotsUnlocked().ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusWeaponSlotsBlock((getWeaponSlots() - 1).ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getMentalResistBlock(getMentalResistanceForDisplay()));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCharismaBlock(getCharisma().ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getSynergyBlock(getSynergyCoefficientForDisplay()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusExuberancesBlock(getBonusExuberances().ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getZOIBlock(getCharisma().ToString(), getZoneOfInfluenceTrait().getIconName()));

        // buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getZoneOfInfluenceTrait().getIconName()));

        return buildingBlocks;
    }

    #endregion

}
