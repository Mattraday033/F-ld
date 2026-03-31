using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Ink.Runtime;
using System.Linq;

public enum PrimaryStat{Strength = 0, Dexterity = 1, Wisdom = 2, Charisma = 3, None = 4}

public class AllyStats : Stats
{
    #region Constants

    public const int xpNeededToLevelUp = 1000;

    private const string combatantTypeDescription = "Party Leader";
    public const string ZOIStatBoostKey = "PartyMemberBoost";

    public const int playerLevelMaximum = 20;
    private const int playerHealthPerLevelAboveOne = 10;
    private const int playerBaseHealth = 90;

    public const int statMaximum = 10;

    public const int defaultStartingRow = 0;
    public const int defaultStartingCol = 1;

    #endregion

    #region UnityEvents

    public readonly static UnityEvent OnPartyMemberUpgraded = new UnityEvent();

    #endregion

    #region Global Variables

    public int strength { private get; set; }
    public int dexterity { private get; set; }
    public int wisdom { private get; set; }
    public int charisma { private get; set; }

    private int level;
    public int xp;

    public AbilityMenuManager lastCombatAbilityMenuManager;

    public CombatActionArray combatActionArray;
    public EquippedItems equippedItems;

    #endregion

    #region Constructors

    public AllyStats() : base("")
    {
        combatActionArray = new CombatActionArray(this); 
        equippedItems = new EquippedItems(this);
        this.animationAudioClipDictionary = AnimationSFXDictionaryList.maleHumanAudioDictionary;
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
        this.animationAudioClipDictionary = AnimationSFXDictionaryList.maleHumanAudioDictionary;
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

        combatActionArray = new CombatActionArray(this, SaveBlueprint.extractCombatActionsFromJson(this, wrapper.combatActions));
        equippedItems = new EquippedItems(this, SaveBlueprint.extractEquippedItemsFromJson(wrapper.currentEquipment));
        this.animationAudioClipDictionary = AnimationSFXDictionaryList.maleHumanAudioDictionary;
    }

    #endregion

    #region Sprite and GameObject

    public override string getCombatSpriteName()
    {
        return PrefabNames.allyCombatSpriteName;
    }

    public override void setUpComponents(ComponentList list)
    {
        base.setUpComponents(list);

        lastCombatAbilityMenuManager = list.abilityMenuManager;
        lastCombatAbilityMenuManager.actionArraySource = this;
    }

    public override void spawningActions()
    {
        Dexterity.addExitStrategy(this);
    }

    #endregion

    #region Animation Manager
    
    public Sprite getSpriteIcon()
    {
        return Resources.LoadAll<Sprite>(EnemyTypeFolderPathList.getEnemyTypeFolderPath(getName()) + CharacterAnimationType.Idle_Front.ToString())[0];
    }

    #endregion

    #region Level/XP
    public void addXP(int earnedXP)
    {
        xp += earnedXP;
    }

    public void removeXPFromLevelUpOnce()
    {
        xp -= xpNeededToLevelUp;
    }

    public bool canLevelUp()
    {
        return xp >= xpNeededToLevelUp;
    }

    public int getLevel()
    {
        return level;
    }

    public void setLevel(int newLevel)
    {
        this.level = newLevel;
    }

    public void incrementLevel(bool displayOnly = false)
    {
        level++;

        if(!displayOnly)
        {
            setAbilitiesAsNew(PrimaryStat.None);
        }
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
        return playerBaseHealth + (playerHealthPerLevelAboveOne * (level - 1)) + getBonusHealthFromAllSources();
    }

    private int getBonusHealthFromAllSources()
    {
        int healthFromStrength = Strength.getHealthFromStrength(strength);

        int bonusFormulas = StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusHealthFormula());

        return healthFromStrength + bonusFormulas;
    }

    public static int calculateTotalHealth(int potentialLevel, int potentialStrength, List<StatBoostSource> statBoostSources)
    {
        AllyStats dummyPlayer = new AllyStats();

        dummyPlayer.level = potentialLevel;
        dummyPlayer.strength = potentialStrength;

        int bonusFormulas = StatBoostSource.calculateAllStatFormulas(dummyPlayer, statBoostSources, b => b.getBonusHealthFormula());

        return dummyPlayer.getTotalHealth() + bonusFormulas;
    }

    #endregion

    #region Primary Stats

    // public int getAllStatBoosts

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

    public bool meetsStatRequirements(PrimaryStat PrimaryStat, int statLevel)
    {
        switch (PrimaryStat)
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

        if (highestStatLvl <= 1 || highestStats.Count >= 4)
        {
            return new List<PrimaryStat>() { PrimaryStat.Strength };
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
        return strength + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusStrengthFormula());
    }

    public void incrementStrength(bool displayOnly = false)
    {
        strength++;

        if(!displayOnly)
        {
            setAbilitiesAsNew(PrimaryStat.Strength);
        }
    }



    public override double getCritDamageMultiplier()
    {
        double bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageDouble(this, getAllStatBoosts(), b => b.getBonusCriticalDamageMultiplierFormula());

        return (DamageCalculator.baseCriticalDamage + (Strength.critDamMultPerStrengthDouble * ((double)getStrength()))) + bonusFormulas;
    }

    public string getExtraCritDamageForDisplay()
    {
        int bonusFormulas = StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusCriticalDamageMultiplierFormula());

        return (getStrength() * Strength.critDamMultPerStrength) + bonusFormulas + (DamageCalculator.baseCriticalDamage * 100) + "%";
    }

    public double getWoundResistance()
    {
        double bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageDouble(this, getAllStatBoosts(), b => b.getBonusWoundResistanceFormula());

        return Strength.physResistBaseDouble + (((double)getStrength()) * Strength.physResistPerStrengthDouble) + bonusFormulas;
    }

    public string getWoundResistanceForDisplay()
    {
        int bonusFormulas = StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusWoundResistanceFormula());

        return Strength.physResistBase + (getStrength() * Strength.physResistPerStrength) + bonusFormulas + "%";
    }

    public override bool rollAgainstWoundResistance()
    {
        if(getWoundResistance() >= Constants.autoSuccess)
        {
            return true;
        }

        return UnityEngine.Random.Range(0f, 1f) <= getWoundResistance();
    }

    #endregion

    #region Dexterity + Secondaries

    public int getDexterityWithoutBoosts()
    {
        return dexterity;
    }

    public override int getDexterity()
    {
        return dexterity + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusDexterityFormula());
    }

    public void incrementDexterity(bool displayOnly = false)
    {
        dexterity++;

        if(!displayOnly)
        {
            setAbilitiesAsNew(PrimaryStat.Dexterity);
        }
    }

    public override int getExtraArmorFromDexterity()
    {
        // int bonusFormulas = StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusArmorFormula());

        return getDexterity() * Dexterity.extraArmorMultiplier;
    }

    public override int getArmorPenetration()
    {
        int baseArmorPen = base.getArmorPenetration();

		switch (getDexterity())
		{
			case 2:
			case 3:
                baseArmorPen += 5;
                break;
			case 4:
			case 5:
                baseArmorPen += 10;
                break;
			case 6:
			case 7:
                baseArmorPen += 15;
                break;
			case 8:
			case 9:
                baseArmorPen += 20;
                break;
			case >= 10:
                baseArmorPen += 25;
                break;
		}

        return baseArmorPen;
    }

    public string getArmorPenetrationForDisplay()
    {
        return getArmorPenetration() + "%";
    }

    public override float getSurpriseDamageMultiplier()
    {
        float bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageFloat(this, getAllStatBoosts(), b => b.getBonusSurpriseRoundDamageFormula());

        return Dexterity.surpriseDamMultBase + (((float)getDexterity()) * Dexterity.surpriseDamMultCoefficient) + bonusFormulas;
    }

    public string getSurpriseDamageMultiplierForDisplay()
    {
        float bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageFloat(this, getAllStatBoosts(), b => b.getBonusSurpriseRoundDamageFormula());

        return (((float)getDexterity()) * Dexterity.surpriseDamMultCoefficient + bonusFormulas) * 100f + "%";
    }

    #endregion

    #region Wisdom + Secondaries

    public int getWisdomWithoutBoosts()
    {
        return wisdom;
    }

    public override int getWisdom()
    {
        return wisdom + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusWisdomFormula());
    }

    public void incrementWisdom(bool displayOnly = false)
    {
        wisdom++;
        
        if(!displayOnly)
        {
            setAbilitiesAsNew(PrimaryStat.Wisdom);
        }
    }

    public double getMentalResistance()
    {
        double bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageDouble(this, getAllStatBoosts(), b => b.getBonusMentalResistanceFormula());

        return Wisdom.mentalResistBaseDouble + (((double)getWisdom()) * Wisdom.mentalResistPerWisdomDouble) + (bonusFormulas / 100.0);
    }

    public string getMentalResistanceForDisplay()
    {
        int bonusFormulas = StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusMentalResistanceFormula());

        return (Wisdom.mentalResistBase + (getWisdom() * Wisdom.mentalResistPerWisdom) + bonusFormulas) + "%";
    }

    public override bool rollAgainstMentalResistance()
    {
        if(getWoundResistance() >= Constants.autoSuccess)
        {
            return true;
        }

        return UnityEngine.Random.Range(0f, 1f) <= getWoundResistance();
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

        int bonusFormula = StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusWeaponSlotsFormula());

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

        int bonusFormula = StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusPassiveSlotsFormula());

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
        return charisma + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusCharismaFormula());
    }

    public override int getZOIStat()
    {
        switch(getName())
        {
            case NPCNameList.thatch:

                if(getStrength() > getCharisma())
                {
                    return getStrength();
                } else
                {
                    return getCharisma();
                }
            default:
                return getCharisma();
        }
    }

    public void incrementCharisma(bool displayOnly = false)
    {
        charisma++;

        if(!displayOnly)
        {
            setAbilitiesAsNew(PrimaryStat.Charisma);
        }
    }

    public override int getBonusExuberances()
    {
        return charisma/3;
    }

    public override int getSynergyCoefficient()
    {
        return (getCharisma() * Charisma.playerSynergyModifierCoefficient) + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getBonusSynergyFormula());
    }

    public string getSynergyCoefficientForDisplay()
    {
        return getSynergyCoefficient() + "";
    }

    #endregion

    #endregion

    #region Combat and Action Array

    public override int getBonusAbilityDamage()
    {
        return combatActionArray.calculateBonusAbilityDamage();
    }

    public override AbilityMenuManager getAbilityMenuManager()
    {
        return lastCombatAbilityMenuManager;
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

    public override string getVolleyAnimationType()
    {
        return EffectAnimationType.Pierce.ToString();
    }

    private void setAbilitiesAsNew(PrimaryStat primaryStat)
    {
        List<CombatAction> newCombatActions;

        switch(primaryStat)
        {
            case PrimaryStat.Strength:
                newCombatActions = AbilityList.getAllAvailableAbilitiesOfStat(primaryStat, strength, strength);
                break;
            case PrimaryStat.Dexterity:
                newCombatActions = AbilityList.getAllAvailableAbilitiesOfStat(primaryStat, dexterity, dexterity);
                break;
            case PrimaryStat.Wisdom:
                newCombatActions = AbilityList.getAllAvailableAbilitiesOfStat(primaryStat, wisdom, wisdom);
                break;
            case PrimaryStat.Charisma:
                newCombatActions = AbilityList.getAllAvailableAbilitiesOfStat(primaryStat, charisma, charisma);
                break;
            default:
                newCombatActions = new List<CombatAction>();
                List<CombatAction> companionAbilities = AbilityList.getCompanionAbilities(getName());

                foreach(CombatAction companionAbility in companionAbilities)
                {
                    if(companionAbility.getRequiredStatLevel() == getLevel())
                    {
                        newCombatActions.Add(companionAbility);
                    }
                }

                break;
        }

        foreach(CombatAction action in newCombatActions)
        {
            Ability ability = action as Ability;

            if(ability != null)
            {
                NewAbilityManager.setAbilityAsNew(this, ability);
            }
        }
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

            Trait traitClone = trait.clone();
            traitClone.traitApplier = this;

            addTrait(traitClone);
        }
    }

    public override bool notResurrectable()
    {
        return false;
    } 

    #region Zone of Influence

    public override ZoneOfInfluenceTrait getZoneOfInfluenceTrait()
    {
        return new ZoneOfInfluenceTrait(this);
    }

    #endregion

    #endregion

    #region Equipment
    public override EquippedItems getEquippedItems()
    {
        return equippedItems;
    }

    // public override string getBonusCritChanceFromArmor()
    // {
    //     return "" + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getCritFormula());
    // }

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

        return totalArmorRating;
    }

    public override bool hasAvailableWeaponSlots()
    {
        return combatActionArray.hasAvailableWeaponSlots();
    }

    public void checkStatsAfterEquipmentRemoval()
    {
        if(currentHealth > getTotalHealth())
        {
            currentHealth = getTotalHealth();
        }
    }

    #endregion

    #region Miscellaneous

    public override GridCoords findLocationToSpawn()
    {
        GridCoords coords = Formation.findLocationOfStats(this);

        return new GridCoords(coords.row + CombatGrid.allyRowUpperBounds, coords.col);
    }

    public override List<StatBoostSource> getAllStatBoosts()
    {
        List<StatBoostSource> boosts = base.getAllStatBoosts();

        boosts.AddRange(StatBoostSource.getAllStatBoosts(equippedItems));

        return boosts;
    }

    public override float getDevastatingCriticalPercentage()
    {
        if (hasTrait(TraitList.devastatingCriticals))
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
        if (currentStory.variablesState[InkVariableNameList.playerName] != null)
        {
            currentStory.variablesState[InkVariableNameList.playerName] = PartyManager.getPlayerNameForDisplay();
        }

        if (currentStory.variablesState[InkVariableNameList.strengthVarName] != null)
        {
            currentStory.variablesState[InkVariableNameList.strengthVarName] = strength;
        }

        if (currentStory.variablesState[InkVariableNameList.dexterityVarName] != null)
        {
            currentStory.variablesState[InkVariableNameList.dexterityVarName] = dexterity;
        }

        if (currentStory.variablesState[InkVariableNameList.wisdomVarName] != null)
        {
            currentStory.variablesState[InkVariableNameList.wisdomVarName] = wisdom;
        }

        if (currentStory.variablesState[InkVariableNameList.charismaVarName] != null)
        {
            currentStory.variablesState[InkVariableNameList.charismaVarName] = charisma;
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


    public override int getVolleyAccuracy()
    {
        return PartyStats.getVolleyAccuracy();
    }

    #endregion

    #region IDescribable

    public string getNameWithoutPlayerMarker()
    {
        return getName().Replace(PartyManager.playerMarker, "");
    }

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

        DescriptionPanel.setImageColor(panel.iconBackgroundPanel, ColorList.grey125);
        DescriptionPanel.setImage(panel.iconPanel, getSpriteIcon());
    }

    #endregion

    #region IDescribableInBlocks

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {

        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getLevelBlock(getLevel().ToString()));

        if (!CombatStateManager.inCombat)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getExperienceBlock(xp.ToString()));
        }

        buildingBlocks.AddRange(base.getDescriptionBuildingBlocks());

        if (!CombatStateManager.inCombat)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getPartyGoldBlock(Purse.getCoinsInPurse().ToString()));
        }

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusDamageBlock(getBonusAbilityDamage().ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getStrengthBlock(getStrength().ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusHealthBlock(getBonusHealthFromAllSources().ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCriticalHitDamageBlock(getExtraCritDamageForDisplay().ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWoundResistBlock(getWoundResistanceForDisplay().ToString()));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDexterityBlock(getDexterity().ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getExtraArmorBlock(getExtraArmorFromDexterity().ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getSurpriseRoundDamageMultiplierBlock(getSurpriseDamageMultiplierForDisplay()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorPenetrationBlock(getArmorPenetrationForDisplay()));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWisdomBlock(getWisdom().ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getPassiveSlotsBlock(getPassiveSlotsUnlocked().ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusWeaponSlotsBlock((getWeaponSlots() - 1).ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getMentalResistBlock(getMentalResistanceForDisplay()));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCharismaBlock(getCharisma().ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getSynergyBlock(getSynergyCoefficientForDisplay()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusExuberancesBlock(getBonusExuberances().ToString()));
        if(getZoneOfInfluenceTrait() != null)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getZOIBlock(getZOIStat().ToString(), getZoneOfInfluenceTrait().getIconName()));
        }
        // buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getZoneOfInfluenceTrait().getIconName()));

        return buildingBlocks;
    }

    #endregion

}
