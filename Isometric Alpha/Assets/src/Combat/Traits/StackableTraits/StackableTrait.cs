using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public delegate bool ReapplicationLogicDelegate<T>(T t);
public enum ActionCostType { None = 1, Stance = 2, Bloodlust = 3, Predation = 4, 
                             RedKnife = 5, BlueShield = 6, YellowThorn = 7, GreenLeaf = 8 }

public class StackableTrait: Trait
{
    private int startingStacks;
	private int numberOfStacks;
    private int maximumStacks = 99;
    private int stacksAppliedPerApplication;

	private UnityEvent[] reapplicationEvents;

	private Trait[] baseTraits;
    private ActionCostType costType; 
	
	public StackableTrait(int startingStacks, int stacksAppliedPerApplication, Trait baseTrait) :
	base(baseTrait.getName(), baseTrait.traitType, traitDescription: baseTrait.getDescription(), baseTrait.getIconName())
	{
        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = ActionCostType.None;
        this.baseTraits = new Trait[] { baseTrait };
    }

    public StackableTrait(int startingStacks, int stacksAppliedPerApplication, ActionCostType costType, Trait baseTrait) :
    base(baseTrait.getName(), baseTrait.traitType, baseTrait.getDescription(), baseTrait.getIconName())
    {
        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = costType;
        this.baseTraits = new Trait[] { baseTrait };
    }

    public StackableTrait(UnityEvent reapplicationEvent, int startingStacks, int stacksAppliedPerApplication, Trait baseTrait) :
	base(baseTrait.getName(), baseTrait.traitType, baseTrait.getDescription(), baseTrait.getIconName())
    {
        this.reapplicationEvents = new UnityEvent[] { reapplicationEvent };
        //this.stackChangeAction += reapply; removed so that the base version of the method in TraitList isn't subscribed. Each StackableTrait is subscribed in clone()

        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = ActionCostType.None;
        this.baseTraits = new Trait[] { baseTrait };
    }

    public StackableTrait(UnityEvent reapplicationEvent, int startingStacks, int stacksAppliedPerApplication, ActionCostType costType, Trait baseTrait) :
    base(baseTrait.getName(), baseTrait.traitType, baseTrait.getDescription(), baseTrait.getIconName())
    {
        this.reapplicationEvents = new UnityEvent[] { reapplicationEvent };
        //this.stackChangeAction += reapply; removed so that the base version of the method in TraitList isn't subscribed. Each StackableTrait is subscribed in clone()

        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = costType;
        this.baseTraits = new Trait[] { baseTrait };
    }
    public StackableTrait(UnityEvent reapplicationEvent, int startingStacks, int stacksAppliedPerApplication, ActionCostType costType, Trait[] baseTraits) :
    base(baseTraits[0].getName(), baseTraits[0].traitType, baseTraits[0].getDescription(), baseTraits[0].getIconName())
    {
        this.reapplicationEvents = new UnityEvent[] { reapplicationEvent };
        //this.stackChangeAction += reapply; removed so that the base version of the method in TraitList isn't subscribed. Each StackableTrait is subscribed in clone()

        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = costType;
        this.baseTraits = baseTraits;
    }

    public StackableTrait(UnityEvent[] reapplicationEvents, int startingStacks, int stacksAppliedPerApplication, int maximumStacks, ActionCostType costType, Trait baseTraits) :
    base(baseTraits.getName(), baseTraits.traitType, baseTraits.getDescription(), baseTraits.getIconName())
    {
        this.reapplicationEvents = reapplicationEvents;
        //this.stackChangeAction += reapply; removed so that the base version of the method in TraitList isn't subscribed. Each StackableTrait is subscribed in clone()

        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();
        this.maximumStacks = maximumStacks;

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = costType;
        this.baseTraits = new Trait[1] { baseTraits };
    }

    private void setStackChangeActions()
    {
        if (reapplicationEvents == null)
        {
            return;
        }

        foreach (UnityEvent unityEvent in reapplicationEvents)
        {
            unityEvent.AddListener(onReapplicationEvent);
        }
    }

    public void addStackChangeActions(UnityEvent reapplicationEvent)
    {
        this.reapplicationEvents = Helpers.appendArray<UnityEvent>(this.reapplicationEvents, reapplicationEvent);
        reapplicationEvent.AddListener(onReapplicationEvent);
    }

    public virtual void onReapplicationEvent()
    {
        if(getTraitHolder() == CombatActionManager.currentActor)
        {
            reapply();
        }
    }

    public override void onApplication()
    {
        setStackChangeActions();
    }

    public override void resetStacksToStartingAmount()
    {
        numberOfStacks = startingStacks;
    }

    public override void reapply()
	{
		base.reapply();

        if(numberOfStacks + stacksAppliedPerApplication < maximumStacks)
        {
            numberOfStacks += stacksAppliedPerApplication;
        } else
        {
            numberOfStacks = maximumStacks;
        }

        //Debug.LogError("reapply() was called, current numberOfStacks = " + numberOfStacks);
    }
	
	public override int getNumberOfStacks()
	{
		return numberOfStacks;
	}
    public override void removeStacks(ActionCostType costType, int stacksToRemove)
    {
        if (numberOfStacks - stacksToRemove >= 0)
        {
            numberOfStacks -= stacksToRemove;
        } else
        {
            numberOfStacks = 0;
        }
    }

    #region Generic Stats

    public override string getArmorFormula()
    {
        return DamageCalculator.multiplyFormula(base.getArmorFormula(), getNumberOfStacks());
    }

    public override string getCritFormula()
    {
        return DamageCalculator.multiplyFormula(base.getCritFormula(), getNumberOfStacks());
    }

    public override string getBonusDamageFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusDamageFormula(), getNumberOfStacks());
    }

    public override string getDamageFormula()
    {
        return DamageCalculator.multiplyFormula(base.getDamageFormula(), getNumberOfStacks());
    }

    public override string getInvulnerableFormula()
    {
        return DamageCalculator.multiplyFormula(base.getInvulnerableFormula(), getNumberOfStacks());
    }

    public override string getVulnerableFormula()
    {
        return DamageCalculator.multiplyFormula(base.getVulnerableFormula(), getNumberOfStacks());
    }

    #endregion

    #region PrimaryStats

    public override string getBonusStrengthFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusStrengthFormula(), getNumberOfStacks());
    }

    public override string getBonusDexterityFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusDexterityFormula(), getNumberOfStacks());
    }

    public override string getBonusWisdomFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusWisdomFormula(), getNumberOfStacks());
    }

    public override string getBonusCharismaFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusCharismaFormula(), getNumberOfStacks());
    }

    #endregion

    #region Secondary Stats

    //Strength Stats
    public override string getBonusPhysicalResistanceFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusPhysicalResistanceFormula(), getNumberOfStacks());
    }

    public override string getBonusCriticalDamageMultiplierFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusCriticalDamageMultiplierFormula(), getNumberOfStacks());
    }

    public override string getBonusHealthFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusHealthFormula(), getNumberOfStacks());
    }

    //Dexterity Stats
    public override string getBonusSurpriseRoundDamageFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusSurpriseRoundDamageFormula(), getNumberOfStacks());
    }

    public override string getBonusArmorFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusArmorFormula(), getNumberOfStacks());
    }

    public override string getBonusArmorPenetrationFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusArmorPenetrationFormula(), getNumberOfStacks());
    }

    //Wisdom Stats
    public override string getBonusPassiveSlotsFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusPassiveSlotsFormula(), getNumberOfStacks());
    }

    public override string getBonusWeaponSlotsFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusWeaponSlotsFormula(), getNumberOfStacks());
    }

    public override string getBonusMentalResistanceFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusMentalResistanceFormula(), getNumberOfStacks());
    }

    //Charisma Stats
    public override string getBonusSynergyFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusSynergyFormula(), getNumberOfStacks());
    }

    public override string getBonusExuberancesFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusExuberancesFormula(), getNumberOfStacks());
    }

    public override string getBonusZOIPotencyFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusZOIPotencyFormula(), getNumberOfStacks());
    }

    #endregion

    #region Party Stats

    public override string getBonusRegenFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusRegenFormula(), getNumberOfStacks());
    }

    public override string getBonusSurpriseRoundsFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusSurpriseRoundsFormula(), getNumberOfStacks());
    }

    public override string getBonusRetreatChanceFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusRetreatChanceFormula(), getNumberOfStacks());
    }

    public override string getBonusPartyActionsFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusPartyActionsFormula(), getNumberOfStacks());
    }

    public override string getBonusPartySlotsFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusPartySlotsFormula(), getNumberOfStacks());
    }

    public override string getBonusGoldMultiplierFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusGoldMultiplierFormula(), getNumberOfStacks());
    }
    
    public override string getBonusDiscountFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusDiscountFormula(), getNumberOfStacks());
    }

    public override string getBonusVolleyAccuracyFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusVolleyAccuracyFormula(), getNumberOfStacks());
    }

    #endregion

    #region Skills
    public override string getBonusIntimidateChargesFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusIntimidateChargesFormula(), getNumberOfStacks());
    }

    public override string getBonusCunningChargesFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusCunningChargesFormula(), getNumberOfStacks());
    }

    public override string getBonusObservationLevelFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusObservationLevelFormula(), getNumberOfStacks());
    }

    public override string getBonusLeadershipUsesFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusLeadershipUsesFormula(), getNumberOfStacks());
    }
    #endregion

    public override bool hasActionCostType(ActionCostType typeToCheckFor)
    {
        return costType == typeToCheckFor;
    }

    public override Trait clone()
    {
        StackableTrait cloneOfTrait = (StackableTrait) Clone();

        return (Trait) cloneOfTrait;
    }

    //IDescribable Methods
    public override GameObject getRowType(RowType rowType)
    {
        return Resources.Load<GameObject>(PrefabNames.stackableTraitSquareRowPanel);
    }

    public override void describeSelfRow(DescriptionPanel panel)
    {
        base.describeSelfRow(panel);

        DescriptionPanel.setText(panel.amountText, getNumberOfStacks());
    }
}
