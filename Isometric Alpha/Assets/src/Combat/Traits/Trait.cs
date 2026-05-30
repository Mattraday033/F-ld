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
    protected Stats _TraitHolder;
    protected virtual Stats traitHolder
    {
        get
        {
            return _TraitHolder;
        }
        set
        {
            _TraitHolder = value;
        }
    }
    public Stats traitApplier;

    private bool pacifistic = false;
    private bool permanent;
    private int roundsLeft;
    private int maxRoundsLeft;

    private double linkedPercentage = 0.0;

    private bool immobile = false;

    private string iconName;

    private string loreDescription;
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
                 int roundsLeft = Constants.oneRoundDuration,
                 string loreDescription = "")
    {
        this.traitName = traitName;
        this.traitType = traitType;
        this.traitDescription = traitDescription;
        this.loreDescription = loreDescription;
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

    public string getLoreDescription()
    {
        return loreDescription;
    }

    public virtual bool hasLoreDescription()
    {
        return loreDescription != null && loreDescription.Length > 0;
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

    public bool tickDown()
    {
        if (!isPermanent() && roundsLeft > 0)
        {
            roundsLeft--;
        }

        return dealTickDownDamage();
    }

    public virtual bool stackInFront()
    {
        return false;
    }

    public virtual bool stackInBack()
    {
        return false;
    }

    public virtual void removeStacks(ActionCostType costType, int stacksToRemove)
    {
        //empty on purpose
    }

    public virtual bool hasActionCostType(ActionCostType typeToCheckFor)
    {
        return false;
    }

    public virtual bool immuneToStun()
    {
        return false;
    }

    public virtual bool preventsCombatAction()
    {
        return false;
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
            case TraitType.InteractionDebuff:
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
            case TraitType.InteractionBuff:
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

    public virtual bool dealTickDownDamage()
    {
        if (getTraitHolder() == null || getTraitHolder().isDead())
        {
            return false;
        }

        if (getTickDownDamage() > 0)
        {
            traitHolder.modifyCurrentHealth(getTickDownDamage());

            if (CombatStateManager.whoseTurn == WhoseTurn.Resolving ||
                CombatStateManager.whoseTurn == WhoseTurn.TickDown)
            {
                foreach (GridCoords holderCoords in traitHolder.positions)
                {
                    DamageNumberPopup.create(holderCoords,
                                             getTickDownDamage(),
                                             CombatGrid.getPositionAt(holderCoords),
                                             DamageNumberPopup.getDirectionByTargetCoords(holderCoords),
                                             CombatAnimationManager.getInstance().damageNumberCanvas,
                                             false,
                                             false,
                                             traitTickDownDamageFrameDelay);
                }
            }

            return true;
        }

        return false;
    }

    public bool isImmobile()
    {
        return immobile;
    }

    public virtual bool hasUnusedOnDeathEffect()
    {
        return false;
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

        if(CombatStateManager.inCombat && traitApplier == null && SelectorManager.currentAbilityManager != null)
        {
            return SelectorManager.currentAbilityManager.actionArraySource;
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

    public static List<GlossaryEntry> getAllTraitTypeGlossaryEntries()
    {
        List<GlossaryEntry> allTraitTypesGlossaryEntries = new List<GlossaryEntry>();

        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Boost", "Trait Types", HoverMessageList.boostTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Charge", "Trait Types", HoverMessageList.chargeTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Foe Type", "Trait Types", HoverMessageList.foeTypeTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Equipped Passive", "Trait Types", HoverMessageList.equippedPassiveTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Interaction", "Trait Types", HoverMessageList.interactionTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Mental", "Trait Types", HoverMessageList.mentalTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("On Death", "Trait Types", HoverMessageList.onDeathTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Protection", "Trait Types", HoverMessageList.protectionTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Size", "Trait Types", HoverMessageList.sizeTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Target Priority", "Trait Types", HoverMessageList.targetPriorityTraitTypeMessage));
        allTraitTypesGlossaryEntries.Add(new WrittenGlossaryEntry("Wound", "Trait Types", HoverMessageList.woundTraitTypeMessage));

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

    public Trait clone(Stats newTraitHolder)
    {
        Trait traitClone = clone();

        traitClone.traitHolder = newTraitHolder;

        return traitClone;
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

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getTraitTypeBlock(getType(), HoverMessageList.traitTypePrefix + getType()));

        buildingBlocks.AddRange(getStatBoostDescriptionBuildingBlocks(getStatSource(), this));

        if(CombatStateManager.inCombat)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getRoundsLeftForDisplay()));
        } else
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getMaxRoundsLeftForDisplay()));
        }

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getDescription()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: getIconName()));

        if(preventsCombatAction())
        {
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.stunnedIcon));
        }

        if(isMandatoryTarget())
        {
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.mandatoryTargetIcon));
        }


        // if(TraitList.minion.Equals(this))
        // {
        //     buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.minionIcon));
        // }
        // if(TraitList.master.Equals(this))
        // {
        //     buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.masterIcon));
        // }

        if(traitType == TraitType.Stance)
        {
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.stanceIconName));
        }

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
            case TraitType.InteractionBuff:
            case TraitType.InteractionDebuff:
            case TraitType.Interaction:
                return TraitList.interactionName;
            case TraitType.FoeType:
                return TraitList.foeTypeName;
            case TraitType.Stance:
            case TraitType.EquippedPassive:
                return TraitList.equippedPassiveName;
            case TraitType.OnDeath:
                return TraitList.onDeathName;
            case TraitType.TargetPriority:
                return TraitList.targetPriorityName;
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

    public string getApplicationDescription()
    {
        return "Applies the " + getName() + " Trait to all targets.";
    }

}
