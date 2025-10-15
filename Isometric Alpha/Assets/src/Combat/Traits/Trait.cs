using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Trait : ICloneable, IDescribable, IDescribableInBlocks, ISortable, IStatBoostSource
{
    private const int traitTickDownDamageFrameDelay = 2;

    private string traitName;
    private string traitType;
    private string traitDescription;
    private Stats traitHolder;
    public Stats traitApplier;

    private bool pacifistic = false;
    private bool permanent;
    private int roundsLeft;
    private int maxRoundsLeft;

    public string[] statBoostKeys = new string[0];

    private double linkedPercentage = 0.0;

    private bool mandatoryTrait = false;
    private bool immobile = false;

    private string traitIconName;
    private Color traitIconBackgroundColor;

    public Trait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, bool mandatoryTrait)
    {
        this.traitName = traitName;
        this.traitType = traitType;
        this.traitDescription = traitDescription;

        this.traitIconName = traitIconName;
        this.traitIconBackgroundColor = traitIconBackgroundColor;

        this.permanent = true;
        this.mandatoryTrait = mandatoryTrait;
    }

    public Trait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, bool mandatoryTrait, bool immobile)
    {
        this.traitName = traitName;
        this.traitType = traitType;
        this.traitDescription = traitDescription;

        this.traitIconName = traitIconName;
        this.traitIconBackgroundColor = traitIconBackgroundColor;

        this.permanent = true;
        this.mandatoryTrait = mandatoryTrait;
        this.immobile = immobile;
    }

    public Trait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, bool mandatoryTrait, bool immobile, bool pacifistic)
    {
        this.traitName = traitName;
        this.traitType = traitType;
        this.traitDescription = traitDescription;

        this.traitIconName = traitIconName;
        this.traitIconBackgroundColor = traitIconBackgroundColor;

        this.permanent = true;
        this.mandatoryTrait = mandatoryTrait;
        this.immobile = immobile;
        this.pacifistic = pacifistic;
    }

    public Trait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor)
    {
        this.traitName = traitName;
        this.traitType = traitType;
        this.traitDescription = traitDescription;

        this.traitIconName = traitIconName;
        this.traitIconBackgroundColor = traitIconBackgroundColor;

        this.permanent = true;
    }

    public Trait(string traitName, string traitType, string traitDescription, string traitIconName, int roundsLeft, Color traitIconBackgroundColor)
    {
        this.traitName = traitName;
        this.traitType = traitType;
        this.traitDescription = traitDescription;

        this.traitIconName = traitIconName;
        this.traitIconBackgroundColor = traitIconBackgroundColor;

        this.permanent = false;
        this.roundsLeft = roundsLeft;
        this.maxRoundsLeft = roundsLeft;
    }

    //Hidden Traits use this constructor
    public Trait(string traitName)
    {
        this.traitName = traitName;
    }

    public virtual string getDescription()
    {
        return traitDescription;
    }

    public virtual bool deleteIfDead()
    {
        return false;
    }

    public virtual Selector findTargetLocation(Selector selector, ArrayList listOfTargets)
    {
        return null;
    }

    public virtual bool isMandatoryTrait()
    {
        return mandatoryTrait;
    }

    public static string getPermanentDescription()
    {
        return "Permanent";
    }

    public virtual string getIconName()
    {
        return traitIconName;
    }

    public virtual Color getTraitIconBackgroundColor()
    {
        return traitIconBackgroundColor;

    }

    public Sprite getIconSprite()
    {
        return Helpers.loadSpriteFromResources(getIconName());
    }

    public virtual bool isMandatoryTarget()
    {
        return false;
    }

    public void setLinkedPercentage(double linkedPercentage)
    {
        this.linkedPercentage = linkedPercentage;
    }

    public double getLinkedPercentage()
    {
        return linkedPercentage;
    }

    public bool isPermanent()
    {
        return permanent;
    }

    public void setRoundsLeft(int newRoundsLeft)
    {
        roundsLeft = newRoundsLeft;
    }

    public int getRoundsLeft()
    {
        return roundsLeft;
    }

    public string getRoundsLeftForDisplay()
    {
        if (isPermanent())
        {
            return getPermanentDescription();
        }
        else if (roundsLeft == 1)
        {
            return roundsLeft + " Round";
        }
        else
        {
            return roundsLeft + " Rounds";
        }
    }

    public string getMaxRoundsLeftForDisplay()
    {
        if (isPermanent())
        {
            return getPermanentDescription();
        }
        else
        {
            return maxRoundsLeft + " Rounds";
        }
    }

    public void tickDown()
    {
        dealTickDownDamage();

        if (!isPermanent() && roundsLeft > 0)
        {
            roundsLeft--;
        }
    }

    public virtual bool stackInFront()
    {
        return false;
    }

    public virtual bool stackInBack()
    {
        return false;
    }

    public virtual int getBonusDamageDealt()
    {
        return 0;
    }

    public int addBonusDamageDealt(int damage)
    {
        int modifiedDamage = damage;

        modifiedDamage += getBonusDamageDealt();

        return modifiedDamage;
    }

    public virtual int getBonusCritChance()
    {
        return 0;
    }

    public virtual int getBonusDamageTaken()
    {
        return 0;
    }

    public int addBonusDamageTaken(int damage)
    {
        int modifiedDamage = damage;

        modifiedDamage += getBonusDamageTaken();

        return modifiedDamage;
    }

    public virtual void removeStacks(ActionCostType costType, int stacksToRemove)
    {
        //empty on purpose
    }

    public virtual double getPercentageArmorLost()
    {
        return 0.0;
    }

    public virtual bool hasActionCostType(ActionCostType typeToCheckFor)
    {
        return false;
    }

    public virtual bool preventsCombatAction()
    {
        return false;
    }

    public int reduceDamageByPercentage(int damage)
    {
        int modifiedDamage = damage;

        modifiedDamage = (int)(((double)modifiedDamage) * (1.0 - getPercentageDamageReduction()));

        return modifiedDamage;
    }

    public virtual double getPercentageDamageReduction()
    {
        return 0.0;
    }

    public virtual bool isPacifist()
    {
        return pacifistic;
    }

    public virtual void reapply()
    {
        if (!permanent)
        {
            roundsLeft = maxRoundsLeft;
        }
    }

    public virtual bool isRemovedOnDamage()
    {
        return false;
    }

    public virtual void onApplication()
    {
        //empty on purpose
    }

    public int getMaxRounds()
    {
        return maxRoundsLeft;
    }

    public virtual bool fromZoneOfInfluence()
    {
        return false;
    }

    public virtual string[] getStatBoostKeys()
    {
        return statBoostKeys;
    }

    public virtual void onDeathEffect(Stats actor)
    {
        //purposefully empty
    }

    public virtual bool preventsResurrection()
    {
        return false;
    }

    public virtual bool isUntargetable()
    {
        return false;
    }

    public virtual int damageOnDebuffApplication()
    {
        return 0;
    }

    public virtual int damageOnBuffApplication()
    {
        return 0;
    }

    public bool isDebuff()
    {
        if (TraitList.buffDebuffTraitTypes.ContainsKey(traitType))
        {
            return !TraitList.buffDebuffTraitTypes[traitType];
        }
        else
        {
            return false;
        }
    }

    public bool isBuff()
    {
        if (TraitList.buffDebuffTraitTypes.ContainsKey(traitType))
        {
            return TraitList.buffDebuffTraitTypes[traitType];
        }
        else
        {
            return false;
        }
    }
    public virtual void resetStacksToStartingAmount()
    {
        //empty on purpose
    }

    public virtual int getTickDownDamage()
    {
        return 0;
    }

    public virtual void dealTickDownDamage()
    {
        if (getTraitHolder() == null || getTraitHolder().isDead)
        {
            return;
        }

        if (getTickDownDamage() > 0)
        {
            traitHolder.modifyCurrentHealth(getTickDownDamage());

            if (CombatStateManager.whoseTurn == WhoseTurn.Resolving)
            {
                DamageNumberPopup.create(getTickDownDamage(),
                                         CombatGrid.getPositionAt(traitHolder.position),
                                         CombatAnimationManager.getInstance().damageNumberCanvas,
                                         false,
                                         false,
                                         traitTickDownDamageFrameDelay);
            }

            DeadCombatantManager.getInstance().cleanUpAllDeadCombatants();
            CombatStateManager.getInstance().checkForWinOrLossStates();
        }
    }

    public bool isImmobile()
    {
        return immobile;
    }

    public void setTraitHolder(Stats traitHolder)
    {
        this.traitHolder = traitHolder;
    }

    public Stats getTraitHolder()
    {
        return traitHolder;
    }

    public Stats getStatSource()
    {
        return traitApplier;
    }

    public virtual bool slowsTraitHolder()
    {
        return false;
    }

    public virtual int getNumberOfStacks()
    {
        return 1;
    }

    public virtual int getNumberOfStacks(ActionCostType costType)
    {
        if (!hasActionCostType(costType))
        {
            return 0;
        }

        return getNumberOfStacks();
    }

    public virtual void harmAllLinkedTargets(int incomingDamage)
    {
        //empty on purpose
    }

    public static ArrayList getAllTraitTypeGlossaryEntries()
    {
        ArrayList allTraitTypesGlossaryEntries = new ArrayList();

        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Boost", "Trait Types", "These traits provide offensive effects, like increasing speed or damage dealt."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Charge", "Trait Types", "These traits allow the creature to cast powerful abilities."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Creature Type", "Trait Types", "There are three types of creatures: Masters, Masters, and Summons. Master creatures are the leaders of a pack of creatures, and must be defeated to win in combat. Minion creatures are less powerful than Master creatures, and take orders from Master creatures. When the last Master creature falls, all Minion creatures will flee, and do not need to be defeated to win. Summons are creatures that were brought here by another creature, and like Minions, do not need to be defeated to win."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Equipped Passive", "Trait Types", "A trait that is provided by an Equipped Passive Ability. Permanent so long as the Ability that provides it is equipped to your Action Wheel."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Interaction", "Trait Types", "These traits explains how this creature interacts with certain types of Actions, granting immunities to certain effects or enabling others."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Mental", "Trait Types", "Mental traits are deleterious effects applied to a creature. You and your allies have a chance to resist wounds with your Mental Resistance."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Positioning", "Trait Types", "Positioning traits determine where a creature will spawn at the beginning of battle. Creatures without a Positioning trait spawn randomly."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Protection", "Trait Types", "These traits provide defensive effects, like reducing incoming damage or preventing targeting."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Size", "Trait Types", "A Size trait means that a creature takes up multiple squares. Attacking more than one square the creature occupies will hurt that creature multiple times."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Stance", "Trait Types", "Stance traits are traits that give a stackable buff. Gain stacks of your stance by attacking with a Stance Weapon, such as your fists or a staff. Only one Stance can be equipped at a time."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Target Priority", "Trait Types", "These traits determine who and where a creature is allowed to attack, such as prioritizing closer targets, or attacking randomly."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Wound", "Trait Types", "Wound traits are deleterious effects applied to a creature. You and your allies have a chance to resist wounds with your Physical Resistance."));

        return allTraitTypesGlossaryEntries;
    }


    //ICloneable methods
    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public virtual Trait clone()
    {
        return (Trait)Clone();
    }

    //IDescribable methods

    public virtual string getName()
    {
        return traitName;
    }

    public bool ineligible()
    {
        return false;
    }

    public virtual GameObject getRowType(RowType rowType)
    {
        return Resources.Load<GameObject>(PrefabNames.traitSquareRowPanel);
    }

    public GameObject getDescriptionPanelFull()
    {
        return getDescriptionPanelFull(PanelType.Standard);
    }

    public virtual GameObject getDescriptionPanelFull(PanelType panelType)
    {
        string panelTypeName = "";

        switch (panelType)
        {
            case PanelType.Standard:
            case PanelType.CombatHover:
                panelTypeName = PrefabNames.traitHoverDescriptionPanel;
                break;
            default:
                throw new IOException("Unknown PanelType: " + panelType);
        }

        return DescriptionPanel.getDescriptionPanel(panelTypeName);
    }

    public GameObject getDecisionPanel()
    {
        return null;
    }

    public bool withinFilter(string[] filterParameters)
    {
        return true;
    }

    public virtual void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, getName());
        DescriptionPanel.setText(panel.useDescriptionText, traitDescription);
        DescriptionPanel.setText(panel.timerText, getRoundsLeftForDisplay());
        DescriptionPanel.setText(panel.typeText, traitType);

        DescriptionPanel.setImage(panel.iconPanel, getIconSprite());
    }

    public virtual void describeSelfRow(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setImage(panel.iconPanel, getIconSprite());
    }

    public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {

    }

    public virtual ArrayList getRelatedDescribables()
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

    //IDescribableInBlocks methods

    public virtual List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getTraitTypeBlock(getType()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getMaxRoundsLeftForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getDescription()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));

        return buildingBlocks;
    }

    //ISortable methods
    public int getQuantity()
    {
        return getNumberOfStacks();
    }
    public int getWorth()
    {
        throw new NotImplementedException("Traits cannot be sorted by Worth");
    }
    public string getType()
    {
        return traitType;
    }
    public string getSubtype()
    {
        return traitType;
    }
    public int getLevel()
    {
        throw new NotImplementedException("Traits cannot be sorted by Level");
    }
    public int getNumber()
    {
        throw new NotImplementedException("Traits cannot be sorted by Number");
    }
	
    
    #region IStatBoostSource Methods
    #region Generic Stats

    public string getBonusCritFormula()
    {
        return StatBoostManager.getBonusCritFormula(this);
    }

    public string getBonusDamageFormula()
    {
        return StatBoostManager.getBonusDamageFormula(this);
    }

    #endregion

    #region PrimaryStats

    public string getBonusStrengthFormula()
    {
        return StatBoostManager.getBonusStrengthFormula(this);
    }
    
    public string getBonusDexterityFormula()
    {
        return StatBoostManager.getBonusDexterityFormula(this);
    }
 
    public string getBonusWisdomFormula()
    {
        return StatBoostManager.getBonusWisdomFormula(this);
    }
 
    public string getBonusCharismaFormula()
    {
        return StatBoostManager.getBonusCharismaFormula(this);
    }
 

    #endregion

    #region Secondary Stats

    //Strength Stats
    public string getBonusPhysicalResistanceFormula()
    {
        return StatBoostManager.getBonusPhysicalResistanceFormula(this);
    }
 
    public string getBonusCriticalDamageMultiplierFormula()
    {
        return StatBoostManager.getBonusCriticalDamageMultiplierFormula(this);
    }
 
    public string getBonusHealthFormula()
    {
        return StatBoostManager.getBonusHealthFormula(this);
    }
 

    //Dexterity Stats
    public string getBonusSurpriseRoundDamageFormula()
    {
        return StatBoostManager.getBonusSurpriseRoundDamageFormula(this);
    }
 
    public string getBonusArmorFormula()
    {
        return StatBoostManager.getBonusArmorFormula(this);
    }
 
    public string getBonusArmorPenetrationFormula()
    {
        return StatBoostManager.getBonusArmorPenetrationFormula(this);
    }
 

    //Wisdom Stats
    public string getBonusPassiveSlotsFormula()
    {
        return StatBoostManager.getBonusPassiveSlotsFormula(this);
    }
 
    public string getBonusWeaponSlotsFormula()
    {
        return StatBoostManager.getBonusWeaponSlotsFormula(this);
    }
 
    public string getBonusMentalResistanceFormula()
    {
        return StatBoostManager.getBonusMentalResistanceFormula(this);
    }
 

    //Charisma Stats
    public string getBonusSynergyFormula()
    {
        return StatBoostManager.getBonusSynergyFormula(this);
    }
 
    public string getBonusExuberancesFormula()
    {
        return StatBoostManager.getBonusExuberancesFormula(this);
    }
 
    public string getBonusZOIPotencyFormula()
    {
        return StatBoostManager.getBonusZOIPotencyFormula(this);
    }
 

    #endregion

    #region Party Stats

    public string getBonusRegenFormula()
    {
        return StatBoostManager.getBonusRegenFormula(this);
    }
 

    public string getBonusSurpriseRoundsFormula()
    {
        return StatBoostManager.getBonusSurpriseRoundsFormula(this);
    }
 
    public string getBonusRetreatChanceFormula()
    {
        return StatBoostManager.getBonusRetreatChanceFormula(this);
    }
 

    public string getBonusPartyActionsFormula()
    {
        return StatBoostManager.getBonusPartyActionsFormula(this);
    }
 
    public string getBonusPartySlotsFormula()
    {
        return StatBoostManager.getBonusPartySlotsFormula(this);
    }
 

    public string getBonusGoldMultiplierFormula()
    {
        return StatBoostManager.getBonusGoldMultiplierFormula(this);
    }
 
    public string getBonusDiscountFormula()
    {
        return StatBoostManager.getBonusDiscountFormula(this);
    }
 

    public string getBonusVolleyAccuracyFormula()
    {
        return StatBoostManager.getBonusVolleyAccuracyFormula(this);
    }
 

    #endregion

    #region Skills
    public string getBonusIntimidateChargesFormula()
    {
        return StatBoostManager.getBonusIntimidateChargesFormula(this);
    }
 
    public string getBonusCunningChargesFormula()
    {
        return StatBoostManager.getBonusCunningChargesFormula(this);
    }
 
    public string getBonusObservationLevelFormula()
    {
        return StatBoostManager.getBonusObservationLevelFormula(this);
    }
 
    public string getBonusLeadershipUsesFormula()
    {
        return StatBoostManager.getBonusLeadershipUsesFormula(this);
    }
 
    #endregion
    #endregion


}
