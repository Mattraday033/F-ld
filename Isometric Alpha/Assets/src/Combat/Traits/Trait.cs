using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Trait : StatBoostSource, ICloneable, IDescribable, IDescribableInBlocks, ISortable
{

    #region Constants
    private const int traitTickDownDamageFrameDelay = 2;
    #endregion
    
    #region Global Variables
    private string traitName;
    public TraitType traitType
    {
        get;
        private set;
    }
    private string traitDescription;
    private Stats traitHolder;
    public Stats traitApplier;

    private bool pacifistic = false;
    private bool permanent;
    private int roundsLeft;
    private int maxRoundsLeft;

    private double linkedPercentage = 0.0;

    private bool immobile = false;

    private string iconName;
    #endregion

    #region Unity Events
    public readonly static UnityEvent<Trait> OnTraitApplication = new UnityEvent<Trait>();
    public readonly static UnityEvent<Trait> OnTraitRemoval = new UnityEvent<Trait>();
    #endregion
    
    public Trait(string traitName, 
                 TraitType traitType, 
                 string traitDescription = "", 
                 string iconName = "",
                 bool immobile = false, 
                 bool pacifistic = false,
                 bool permanent = true,
                 int roundsLeft = Constants.oneRoundDuration)
    {
        this.traitName = traitName;
        this.traitType = traitType;
        this.traitDescription = traitDescription;
        this.iconName = iconName;

        this.roundsLeft = roundsLeft;
        this.maxRoundsLeft = roundsLeft;

        this.permanent = permanent;
        this.immobile = immobile;
        this.pacifistic = pacifistic;
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

    public virtual Selector findTargetLocation(Selector selector, List<Stats> listOfTargets)
    {
        return null;
    }

    public virtual bool isMandatoryTrait()
    {
        switch(traitType)
        {
            case TraitType.FoeType:
            case TraitType.Influence:
            case TraitType.Passive:
            case TraitType.Positioning:
            case TraitType.OnDeath:
            case TraitType.Size:
                return true;
            default:
                return false;
        }
    }

    public virtual bool isHiddenTrait()
    {
        return false;
    }

    public static string getPermanentDescription()
    {
        return "Permanent";
    }

    public virtual string getIconName()
    {
        return iconName;
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
        switch(traitType)
        {
            case TraitType.Mental:
            case TraitType.Wound:
                return true;
            default:
                return false;
        }
    }

    public bool isBuff()
    {
        switch(traitType)
        {
            case TraitType.Boost:
            case TraitType.Charge:
            case TraitType.Protection:
                return true;
            default:
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
        if (getTraitHolder() == null || getTraitHolder().isDead())
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
                                         DamageNumberPopup.getDirectionByTargetCoords(traitHolder.position),
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

    public override Stats getStatSource()
    {
        if(!CombatStateManager.inCombat && traitApplier == null)
        {
            return OverallUIManager.getCurrentPartyMember();
        }

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

    public override string getVulnerableFormula()
    {
        if(getName().Equals(StatSourceNameList.halfHandStanceKey))
        {
            return "-1";
        }

        return base.getVulnerableFormula();
    }

    public static List<GlossaryEntry> getAllTraitTypeGlossaryEntries()
    {
        List<GlossaryEntry> allTraitTypesGlossaryEntries = new List<GlossaryEntry>();

        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Boost", "Trait Types", "These traits provide offensive effects, like increasing speed or damage dealt."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Charge", "Trait Types", "These traits allow the creature to cast powerful abilities."));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Creature Type", "Trait Types", "There are three types of creatures: Masters, Minions, and Summons. Master creatures are the leaders of a pack of creatures, and must be defeated to win in combat. Minion creatures are less powerful than Master creatures, and take orders from Master creatures. When the last Master creature falls, all Minion creatures will flee, and do not need to be defeated to win. Summons are creatures that were brought here by another creature, and like Minions, do not need to be defeated to win."));
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

    public virtual void setIdleAnimationOnApplication(AnimationManager animationManager)
    {
        //empty on purpose
    }

    public virtual void setIdleAnimationOnRemoval(AnimationManager animationManager)
    {
        //empty on purpose
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

    #region IDescribable methods

    public override string getName()
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
        DescriptionPanel.setText(panel.typeText, getType());

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

    public virtual List<IDescribable> getRelatedDescribables()
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

    #region IDescribableInBlocks methods
    public virtual List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getTraitTypeBlock(getType()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getMaxRoundsLeftForDisplay()));

        buildingBlocks.AddRange(getStatBoostDescriptionBuildingBlocks(getStatSource(), this));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getDescription()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));

        return buildingBlocks;
    }

    public bool requiresInspectNode()
    {
        return false;
    }
    #endregion

    #region ISortable
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
        switch(traitType)
        {
            case TraitType.FoeType:
                return "Foe Type";
            case TraitType.EquippedPassive:
                return "Equipped Passive";
            case TraitType.OnDeath:
                return "On Death";
            case TraitType.TargetPriority:
                return "Target Priority";
            default:
                return traitType.ToString();
        }
    }

    public string getSubtype()
    {
        return getType();
    }
    public int getLevel()
    {
        throw new NotImplementedException("Traits cannot be sorted by Level");
    }
    public int getNumber()
    {
        throw new NotImplementedException("Traits cannot be sorted by Number");
    }
	#endregion
    
    public override bool Equals(object obj)
    {
        Trait other = obj as Trait;

        if(other == null)
        {
            return false;
        }

        return other.getName().Equals(getName());
    }
}
