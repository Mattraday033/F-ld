using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public delegate bool ReapplicationLogicDelegate<T>(T t);
public enum ActionCostType { 
                                None = 1, 
                                Stance = 2, 
                                Bloodlust = 3, 
                                Predation = 4, 
                                RedKnife = 5, 
                                BlueShield = 6, 
                                YellowThorn = 7, 
                                GreenLeaf = 8 
                            }

public static class EnumDescriptionList
{
    public static string ToFriendlyString(this ActionCostType costType)
    {
        string name = costType.ToString();
        string newName = "";

        int index = 0;
        foreach(char c in name)
        {
            if(Char.IsUpper(c) && index != 0)
            {
                newName += " " + c;
            } else
            {
                newName += c;
            }

            index++;
        }

        return newName;
    }

    public static Trait getCostTrait(this ActionCostType costType)
    {
        switch(costType)
        {
            case ActionCostType.Bloodlust:
                return TraitList.bloodlust.clone();
            case ActionCostType.Predation:
                return TraitList.predation.clone();
            default:
                return null;
        }
    }
}

public class StackableTrait: Trait
{
    private const int maximumStacksPossible = 99;

    private int startingStacks;
	private int numberOfStacks;
    private int maximumStacks;
    private int stacksAppliedPerApplication;

	private List<UnityEvent> personalReapplicationEvents = new List<UnityEvent>();
	private List<UnityEvent> impersonalReapplicationEvents = new List<UnityEvent>();

    private ActionCostType costType; 
	
	public StackableTrait(string traitName, 
                          TraitType traitType, 
                          string traitDescription = "", 
                          string iconName = "",
                          bool immobile = false, 
                          bool pacifistic = false,
                          bool permanent = true,
                          int roundsLeft = Constants.oneRoundDuration,
                          int startingStacks = 1, 
                          int stacksAppliedPerApplication = 1,
                          int maximumStacks = maximumStacksPossible,
                          ActionCostType costType = ActionCostType.None,
                          List<UnityEvent> personalReapplicationEvents = null,
                          List<UnityEvent> impersonalReapplicationEvents = null) :
	base(traitName, 
         traitType, 
         traitDescription, 
         iconName,
         immobile,
         pacifistic,
         permanent,
         roundsLeft)
	{
        this.startingStacks = startingStacks;
        resetStacksToStartingAmount();

        this.stacksAppliedPerApplication = stacksAppliedPerApplication;

        this.costType = costType;
        this.maximumStacks = maximumStacks;

        if(personalReapplicationEvents != null)
        {
            this.personalReapplicationEvents = personalReapplicationEvents;
        }

        if(impersonalReapplicationEvents != null)
        {
            this.impersonalReapplicationEvents = impersonalReapplicationEvents;
        }
    }

    private void setStackChangeActions()
    {
        foreach (UnityEvent unityEvent in personalReapplicationEvents)
        {
            unityEvent.AddListener(onPersonalReapplicationEvent);
        }

        foreach (UnityEvent unityEvent in impersonalReapplicationEvents)
        {
            unityEvent.AddListener(reapply);
        }
    }

    public override void onApplication()
    {
        setStackChangeActions();
    }

    public void onPersonalReapplicationEvent()
    {
        if(getTraitHolder() == CombatActionManager.currentActor)
        {
            reapply();
        }
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
    public override string getBonusWoundResistanceFormula()
    {
        return DamageCalculator.multiplyFormula(base.getBonusWoundResistanceFormula(), getNumberOfStacks());
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
