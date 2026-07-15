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

    public static string getCostDescription(this ActionCostType costType, int amount)
    {
        switch(costType)
        {
            case ActionCostType.RedKnife:
            case ActionCostType.BlueShield:
            case ActionCostType.YellowThorn:
            case ActionCostType.GreenLeaf:
                return "Costs " + amount + " Stacks of the " + costType.ToFriendlyString() + " Exuberance.";
            case ActionCostType.Stance:
                return "Costs " + amount + " Stacks of any Stance.";
            default:
                return "Costs " + amount + " Stacks of the " + costType.ToFriendlyString() + " Trait.";
        }
    }

    public static string getPrimaryStatCharGenCombatDescription(this PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return "Characters who train their Strength gain access to Abilities that affect large areas with big bursts of damage. Their Critical Hits deal more damage as well, and they have higher Health pools than other Characters.\n\nCertain Strength Abilities can push enemies around, or prevent them from attacking vulnerable allies.";
            case PrimaryStat.Dexterity:
                return "Training Dexterity unlocks Abilities that focus on debilitation, damage over time, and the element of surprise. Dexterity also increases a Character's own Armor Score, their Armor Penetration, and provides a damage boost during a surprise round.\n\nMost Actions have their Critical Hit chance determined by a Character's Dexterity.";
            case PrimaryStat.Wisdom:
                return "Raising Wisdom teaches Abilities that are more tactical. A Wise Character can reposition their opponents to better deal with them, or interrupt a foes' plans with a well placed strike.\n\nWisdom governs the number of Weapons that can be held at once, and the number of Passive Abilities that can be equipped.";
            default:
                return "In Combat, Charisma measures a Character's ability to lead and coordinate with their Party Members. Charisma has Abilities that bolster their allies, and expose the weaknesses of their enemies.\n\nCharisma also provides access to Exuberances, which are a resource that is used to activate certain powerful Abilities.";
        }
    }

    public static string getPrimaryStatCharGenDialogueDescription(this PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return "Strength speech checks often involve coercing others into providing aid, or gaining another's confidence through displays of physical prowess.";
            case PrimaryStat.Dexterity:
                return "In Dialogue, Dexterity measures a Character's ability to outwit and outmaneuver. A Dexterity speech check might involve using double-talk to trick someone into giving up information, or catching their arm before they can draw a weapon in anger.";
            case PrimaryStat.Wisdom:
                return "Wisdom is the Primary Stat of perception, knowledge, and reason. Wisdom provides speech checks that allow a Character to make logical arguments, bestow sagely advice, or detect what has been obscured.";
            default:
                return "Charisma determines a Character's powers of communication and persuasion. A Charismatic Character could with one speech check gain someone's confidence, and with the next destroy their ego with mockery.";
        }
    }

    public static string getPrimaryStatCharGenMobilityDescription(this PrimaryStat stat)
    {
        switch(stat)
        {
            case PrimaryStat.Strength:
                return "Characters with high Strength can use their powerful muscles to lift boulders, break down gates, and push aside rubble. They may also use the Intimidate Skill to challenge enemies to open combat, and compel shopkeepers to lower their prices.";
            case PrimaryStat.Dexterity:
                return "Increasing Dexterity allows a Character to clamber over obstructions, squeeze into tight spaces, and operate mechanisms. Dexterous Characters also can use the Cunning Skill to activate traps and deceive enemies; either to slip past them, or assault them from behind.";
            case PrimaryStat.Wisdom:
                return "A high Wisdom unlocks the Observation Skill. This Skill can be used to find secret passages, uncover hidden objects, and even find ambushes before they are sprung.";
            default:
                return "Charisma allows a Character to use the Leadership skill to coordinate multiple Party Members at once. This allows Party Members to block the movement of enemies, or operate mechanisms as a team.";
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
                          string loreDescription = "",
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
         roundsLeft,
         loreDescription: loreDescription)
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
