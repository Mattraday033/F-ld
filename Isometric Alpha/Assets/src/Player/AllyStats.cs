using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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
    private const int playerBaseHealth = 140;

    public const int statMaximum = 10;

    public const int defaultStartingRow = 0;
    public const int defaultStartingCol = 1;

    #endregion

    #region UnityEvents

    public readonly static UnityEvent OnPartyMemberUpgraded = new UnityEvent();
    public readonly static UnityEvent OnAllyDeath = new UnityEvent();

    #endregion

    #region Global Variables

    //unboosted; totalStrength and the others add every boost on top
    public int strength { get; set; }
    public int dexterity { get; set; }
    public int wisdom { get; set; }
    public int charisma { get; set; }

    public int level { get; private set; }
    public int xp;

    public override IAppearance appearance
    {
        get
        {
            return equippedItems.appearance;
        }
    }

    public CombatActionArray combatActionArray;

    private EquippedItems _EquippedItems;

    public override bool usePlayerTargeting => true;

    #endregion

    #region Boolean Properties

    public override bool costsPartyCombatActions => true;
    public override bool notResurrectable => false;
    public override bool hasAvailableWeaponSlots => combatActionArray.hasAvailableWeaponSlots();
    public override bool removableFromFormation => this != PartyManager.getPlayerStats();

    #endregion

    #region Properties

    #region Animation Manager

    public Sprite spriteIcon => SpriteUtil.loadFirstSpriteOfSheetFromResources(EnemyTypeFolderPathList.getEnemyTypeFolderPath(uniqueName) + CharacterAnimationType.Idle_Front.ToString());

    #endregion

    #region Level/XP

    public virtual int levelMaximum => playerLevelMaximum;

    #endregion

    #region Health

    public virtual int baseHealth => playerBaseHealth;
    public virtual int healthPerLevelAboveOne => playerHealthPerLevelAboveOne;

    public override int totalHealth => playerBaseHealth + (playerHealthPerLevelAboveOne * (level - 1)) + bonusHealthFromAllSources;

    private int bonusHealthFromAllSources
    {
        get
        {
            int healthFromStrength = Strength.getHealthFromStrength(strength);

            int bonusFormulas = StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusHealthFormula());

            return healthFromStrength + bonusFormulas;
        }
    }

    #endregion

    #region Primary Stats

    public int[] statsAsArray => new int[] { strength, dexterity, wisdom, charisma };

    public PrimaryStat highestStat
    {
        get
        {
            int[] primaryStats = statsAsArray;

            int highestStatValue = 0;
            int highestStatIndex = 0;
            int currentIndex = 0;

            foreach (int stat in primaryStats)
            {
                if (stat > highestStatValue)
                {
                    highestStatValue = stat;
                    highestStatIndex = currentIndex;
                }

                currentIndex++;
            }

            return (PrimaryStat)highestStatIndex;
        }
    }

    public List<PrimaryStat> highestPrimaryStats
    {
        get
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
    }

    #region Strength + Secondaries

    public override int totalStrength => strength + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusStrengthFormula());

    public override double critDamageMultiplier
    {
        get
        {
            double bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageDouble(allStatBoosts, b => b.getBonusCriticalDamageMultiplierFormula());

            return (DamageCalculator.baseCriticalDamage + (Strength.critDamMultPerStrengthDouble * ((double)totalStrength))) + bonusFormulas;
        }
    }

    public string extraCritDamageForDisplay
    {
        get
        {
            int bonusFormulas = StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusCriticalDamageMultiplierFormula());

            return (totalStrength * Strength.critDamMultPerStrength) + bonusFormulas + (DamageCalculator.baseCriticalDamage * 100) + "%";
        }
    }

    public double woundResistance
    {
        get
        {
            double bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageDouble(allStatBoosts, b => b.getBonusWoundResistanceFormula());

            return Strength.woundResistBaseDouble + (((double)totalStrength) * Strength.woundResistPerStrengthDouble) + bonusFormulas;
        }
    }

    public string woundResistanceForDisplay
    {
        get
        {
            int bonusFormulas = StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusWoundResistanceFormula());

            return Strength.woundResistBase + (totalStrength * Strength.woundResistPerStrength) + bonusFormulas + "%";
        }
    }

    #endregion

    #region Dexterity + Secondaries

    public override int totalDexterity => dexterity + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusDexterityFormula());

    public override int extraArmorFromDexterity => totalDexterity * Dexterity.extraArmorMultiplier;

    public override int armorPenetration
    {
        get
        {
            int baseArmorPen = base.armorPenetration;

            switch (totalDexterity)
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
    }

    public string armorPenetrationForDisplay => armorPenetration + "%";

    public override float surpriseDamageMultiplier
    {
        get
        {
            float bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageFloat(allStatBoosts, b => b.getBonusSurpriseRoundDamageFormula());

            return Dexterity.surpriseDamMultBase + (((float)totalDexterity) * Dexterity.surpriseDamMultCoefficient) + bonusFormulas;
        }
    }

    public string surpriseDamageMultiplierForDisplay
    {
        get
        {
            float bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageFloat(allStatBoosts, b => b.getBonusSurpriseRoundDamageFormula());

            return (((float)totalDexterity) * Dexterity.surpriseDamMultCoefficient + bonusFormulas) * 100f + "%";
        }
    }

    #endregion

    #region Wisdom + Secondaries

    public override int totalWisdom => wisdom + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusWisdomFormula());

    public double mentalResistance
    {
        get
        {
            double bonusFormulas = StatBoostSource.calculateAllStatFormulasAsPercentageDouble(allStatBoosts, b => b.getBonusMentalResistanceFormula());

            return Wisdom.mentalResistBaseDouble + (((double)totalWisdom) * Wisdom.mentalResistPerWisdomDouble) + (bonusFormulas / 100.0);
        }
    }

    public string mentalResistanceForDisplay
    {
        get
        {
            int bonusFormulas = StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusMentalResistanceFormula());

            return (Wisdom.mentalResistBase + (totalWisdom * Wisdom.mentalResistPerWisdom) + bonusFormulas) + "%";
        }
    }

    public override int weaponSlots
    {
        get
        {
            int weaponSlotsFromWisdom = 1;

            if (totalWisdom >= 3 && totalWisdom < 7)
            {
                weaponSlotsFromWisdom = 2;
            }
            else if (totalWisdom >= 7)
            {
                weaponSlotsFromWisdom = Wisdom.maxNumberOfWeaponSlots;
            }

            int bonusFormula = StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusWeaponSlotsFormula());

            if ((weaponSlotsFromWisdom + bonusFormula) > Wisdom.maxNumberOfWeaponSlots)
            {
                return Wisdom.maxNumberOfWeaponSlots;
            }
            else
            {
                return weaponSlotsFromWisdom + bonusFormula;
            }
        }
    }

    public override int passiveSlotsUnlocked
    {
        get
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

            int bonusFormula = StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusPassiveSlotsFormula());

            if (passivesUnlockedFromWisdom + bonusFormula > Wisdom.maximumPassiveSlots)
            {
                return Wisdom.maximumPassiveSlots;
            }
            else
            {
                return passivesUnlockedFromWisdom + bonusFormula;
            }
        }
    }

    public int maximumRepositionsPerCombat
    {
        get
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
    }

    #endregion

    #region Charisma + Secondaries

    public override int totalCharisma => charisma + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusCharismaFormula());

    public override int zoiStat
    {
        get
        {
            switch(uniqueName)
            {
                case NPCNameList.thatch:

                    if(totalStrength > totalCharisma)
                    {
                        return totalStrength;
                    } else
                    {
                        return totalCharisma;
                    }
                default:
                    return totalCharisma;
            }
        }
    }

    public override int bonusExuberances => charisma/3;

    public override int synergyCoefficient => (totalCharisma * Charisma.playerSynergyModifierCoefficient) + StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusSynergyFormula());

    public string synergyCoefficientForDisplay => synergyCoefficient + "";

    #endregion

    #endregion

    #region Combat and Action Array

    public override int bonusAbilityDamage => combatActionArray.calculateBonusAbilityDamage();

    public override CombatActionArray actionArray => combatActionArray;

    public override EffectAnimationType volleyAnimationType => EffectAnimationType.Pierce;

    #endregion

    #region Traits

    public override ZoneOfInfluenceTrait zoneOfInfluenceTrait => new ZoneOfInfluenceTrait(this);

    #endregion

    #region Equipment

    public override EquippedItems equippedItems => _EquippedItems;

    // public override string getBonusCritChanceFromArmor()
    // {
    //     return "" + StatBoostSource.calculateAllStatFormulas(this, getAllStatBoosts(), b => b.getCritFormula());
    // }

    public override int totalArmorRating
    {
        get
        {
            int armorRating = 0;

            foreach (EquippableItem item in equippedItems)
            {
                if (item == null)
                {
                    continue;
                }

                armorRating += item.getArmorRating();
            }

            armorRating += extraArmorFromDexterity;

            return armorRating;
        }
    }

    #endregion

    #region Miscellaneous

    public override List<StatBoostSource> allStatBoosts
    {
        get
        {
            List<StatBoostSource> boosts = base.allStatBoosts;

            boosts.AddRange(StatBoostSource.getAllStatBoosts(equippedItems));

            return boosts;
        }
    }

    public override float devastatingCriticalPercentage
    {
        get
        {
            if (!hasTrait(TraitList.devastatingCriticals))
            {
                return 0f;
            }

            float percentage = (float)dexterity / 100f;

            if (CombatStateManager.isPlayerSurpriseRound())
            {
                return percentage;
            }
            else
            {
                return percentage * 2f;
            }
        }
    }

    public override int volleyAccuracy => PartyStats.getVolleyAccuracy();

    public override int bonusVolleyAccuracy => StatBoostSource.calculateAllStatFormulas(allStatBoosts, b => b.getBonusVolleyAccuracyFormula());

    #endregion

    #region IDescribable

    public string nameWithoutPlayerMarker => displayName;

    #endregion

    #endregion

    #region Constructors

    public AllyStats() : base("")
    {
        combatActionArray = new CombatActionArray(this);
        _EquippedItems = new EquippedItems(this);
        this.animationAudioClipDictionary = PartyMemberList.getAudioDictionary(uniqueName);
    }

    public AllyStats(string name, int Str, int Dex, int Wis, int Cha) : base(name)
    {
        this.characterName = name;

        this.strength = Str;
        this.dexterity = Dex;
        this.wisdom = Wis;
        this.charisma = Cha;

        this.level = 1;
        this.xp = 0;

        combatActionArray = new CombatActionArray(this);
        _EquippedItems = new EquippedItems(this);

        this.currentHealth = totalHealth;
        this.animationAudioClipDictionary = PartyMemberList.getAudioDictionary(uniqueName);
    }

    public AllyStats(StatsWrapper wrapper) : base(wrapper.key)
    {
        this.characterName = wrapper.key;

        this.strength = wrapper.strength;
        this.dexterity = wrapper.dexterity;
        this.wisdom = wrapper.wisdom;
        this.charisma = wrapper.charisma;

        this.level = wrapper.level;
        this.xp = wrapper.xp;
        this.currentHealth = wrapper.currentHealth;

        combatActionArray = new CombatActionArray(this, SaveBlueprint.extractCombatActionsFromJson(this, wrapper.combatActions));
        _EquippedItems = new EquippedItems(this, SaveBlueprint.extractEquippedItemsFromJson(wrapper.currentEquipment));
        this.animationAudioClipDictionary = PartyMemberList.getAudioDictionary(uniqueName);
    }

    #endregion

    #region Sprite and GameObject

    public override void spawningActions()
    {
        Dexterity.addExitStrategy(this);
    }

    #endregion

    #region Level/XP
    public void addXP(int earnedXP)
    {
        xp += earnedXP;

        if(canLevelUp() && PartyManager.getPartyMember(uniqueName).canJoinParty)
        {
            PlayerObject.playLevelUpEffect();
        }
    }

    public void setXPToPlayerLevel()
    {
        int playerLevel = PartyManager.getPlayerStats().level;

        if(playerLevel > level)
        {
            xp = xpNeededToLevelUp * (playerLevel - level);
        }
    }

    public void removeXPFromLevelUpOnce()
    {
        xp -= xpNeededToLevelUp;
    }

    public bool canLevelUp()
    {
        return xp >= xpNeededToLevelUp;
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

    #endregion

    #region Health

    public static int calculateTotalHealth(int potentialLevel, int potentialStrength, List<StatBoostSource> statBoostSources)
    {
        AllyStats dummyPlayer = new AllyStats();

        dummyPlayer.level = potentialLevel;
        dummyPlayer.strength = potentialStrength;

        int bonusFormulas = StatBoostSource.calculateAllStatFormulas(statBoostSources, b => b.getBonusHealthFormula());

        return dummyPlayer.totalHealth + bonusFormulas;
    }

    #endregion

    #region Primary Stats

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

    #region Strength + Secondaries

    public void incrementStrength(bool displayOnly = false)
    {
        strength++;

        if(!displayOnly)
        {
            setAbilitiesAsNew(PrimaryStat.Strength);
        }
    }

    public override bool rollAgainstWoundResistance()
    {
        double resistance = woundResistance;

        if(resistance >= Constants.autoSuccess)
        {
            return true;
        }

        return UnityEngine.Random.Range(0f, 1f) <= resistance;
    }

    #endregion

    #region Dexterity + Secondaries

    public void incrementDexterity(bool displayOnly = false)
    {
        dexterity++;

        if(!displayOnly)
        {
            setAbilitiesAsNew(PrimaryStat.Dexterity);
        }
    }

    #endregion

    #region Wisdom + Secondaries

    public void incrementWisdom(bool displayOnly = false)
    {
        wisdom++;

        if(!displayOnly)
        {
            setAbilitiesAsNew(PrimaryStat.Wisdom);
        }
    }

    public override bool rollAgainstMentalResistance()
    {
        double resistance = woundResistance;

        if(resistance >= Constants.autoSuccess)
        {
            return true;
        }

        return UnityEngine.Random.Range(0f, 1f) <= resistance;
    }

    #endregion

    #region Charisma + Secondaries

    public void incrementCharisma(bool displayOnly = false)
    {
        charisma++;

        if(!displayOnly)
        {
            setAbilitiesAsNew(PrimaryStat.Charisma);
        }
    }

    #endregion

    #endregion

    #region Combat and Action Array

	public void resetAllCooldowns()
	{
        combatActionArray.resetAllCooldowns();
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
                List<CombatAction> companionAbilities = AbilityList.getCompanionAbilities(uniqueName);

                foreach(CombatAction companionAbility in companionAbilities)
                {
                    if(companionAbility.getRequiredStatLevel() == level)
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

    #endregion

    #region Equipment

    //equippedItems overrides a get-only property on Stats, so it can't take a setter of its own
    public void setEquippedItems(EquippedItems newEquippedItems)
    {
        _EquippedItems = newEquippedItems;
    }

    public void checkStatsAfterEquipmentRemoval()
    {
        if(currentHealth > totalHealth)
        {
            currentHealth = totalHealth;
        }
    }

    #endregion

    #region Miscellaneous

    public override List<GridCoords> findLocationToSpawn()
    {
        GridCoords coords = Formation.findLocationOfStats(this);

        return new List<GridCoords> { new GridCoords(coords.row + CombatGrid.allyRowUpperBounds, coords.col) };
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

        DescriptionPanel.setImageColor(panel.iconBackgroundPanel, ColorList.grey125);
        DescriptionPanel.setImage(panel.iconPanel, spriteIcon);
    }

    #endregion

    #region IDescribableInBlocks

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {

        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getLevelBlock(level.ToString()));

        if (!CombatStateManager.inCombat)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getExperienceBlock(xp.ToString()));
        }

        buildingBlocks.AddRange(base.getDescriptionBuildingBlocks());

        if (!CombatStateManager.inCombat)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getPartyGoldBlock(Purse.getCoinsInPurse().ToString()));
        }

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusDamageBlock(bonusAbilityDamage.ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getStrengthBlock(totalStrength.ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusHealthBlock("+" + bonusHealthFromAllSources.ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCriticalHitDamageBlock(extraCritDamageForDisplay.ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWoundResistBlock(woundResistanceForDisplay.ToString()));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDexterityBlock(totalDexterity.ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getExtraArmorBlock("+" + extraArmorFromDexterity.ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getSurpriseRoundDamageMultiplierBlock(surpriseDamageMultiplierForDisplay));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorPenetrationBlock(armorPenetrationForDisplay));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWisdomBlock(totalWisdom.ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getPassiveSlotsBlock(passiveSlotsUnlocked.ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusWeaponSlotsBlock(weaponSlots.ToString()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getMentalResistBlock(mentalResistanceForDisplay));


        buildingBlocks.Add(DescriptionPanelBuildingBlock.getCharismaBlock(totalCharisma.ToString()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getSynergyBlock(synergyCoefficientForDisplay));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getBonusExuberancesBlock(bonusExuberances.ToString()));
        if(zoneOfInfluenceTrait != null)
        {
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.SecondaryStat, text: zoiStat.ToString(), iconName: IconList.ZOIIconName, symbolChar: Charisma.symbolChar[0]));
        }
        // buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getZoneOfInfluenceTrait().getIconName()));

        return buildingBlocks;
    }

    #endregion

}
