using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUpAbility : Ability
{
	public CombatAction actionWhenCharged { get; private set; }
	public Trait chargeUpTrait { get; private set; }
	private const string chargingUpName = "Charging Up";

	public ChargeUpAbility(Trait chargeUpTrait, Ability actionWhenCharged) :
		base(CombatActionSettings.build(actionWhenCharged.getKey(), DescriptionParams.build(actionWhenCharged.getName(), actionWhenCharged.getUseDescription(), actionWhenCharged.getIconName()),
																	DamageParams.build(actionWhenCharged.getDamageFormula(), actionWhenCharged.getCritFormula()),
																	TargetParams.build(Range.singleTargetIndex, actionWhenCharged.selfTargeting),
																	FrequencyParams.build(actionWhenCharged.getMaximumSlots(), actionWhenCharged.getMaximumCooldown()),
																	CostParams.build(actionWhenCharged.getActionCostTypes(), actionWhenCharged.getActionCosts()),
																	actionWhenCharged.getAppliedTrait()) //Left off relatedTraits
			)
	{
		this.chargeUpTrait = chargeUpTrait.clone();
		this.actionWhenCharged = actionWhenCharged.clone();
	}

	public override void setSelector(Selector newSelector)
	{
		base.setSelector(newSelector);
		actionWhenCharged.setSelector(newSelector);
	}

	public override void setActor(Stats actor)
	{
		base.setActor(actor);
		actionWhenCharged.setActor(actor);
	}
	/*
	public override void setTargetCoords(GridCoords newTargetCoords)
	{
		base.setTargetCoords(newTargetCoords);
		actionWhenCharged.setTargetCoords(newTargetCoords);
	}
	*/
	public override void performCombatAction(ArrayList targets)
	{
		if (!isCharged())
		{
			getActorStats().addTrait(chargeUpTrait);
		}
		else
		{
			getActorStats().removeTrait(chargeUpTrait);
			//actionWhenCharged.setSelector(getSelector());
			actionWhenCharged.performCombatAction(targets);
		}
	}

	public override bool isSelfTargeting()
	{
		if (isCharged())
		{
			return actionWhenCharged.isSelfTargeting();
		}
		else
		{
			return true;
		}
	}

	public override int getRangeIndex()
	{
		if (isCharged())
		{
			return actionWhenCharged.getRangeIndex();
		}
		else
		{
			return base.getRangeIndex();
		}
	}

	public override string getRangeTitle()
	{
		if (isCharged())
		{
			return Range.getRangeTitle(actionWhenCharged.getRangeIndex());
		}
		else
		{
			return Range.getRangeTitle(base.getRangeIndex());
		}
	}

	public override string getName()
	{
		if (isCharged())
		{
			return base.getName();
		}
		else
		{
			return chargingUpName;
		}
	}

	public override string getIconName()
	{
		if (isCharged())
		{
			return actionWhenCharged.getIconName();
		}
		else
		{
			return chargeUpTrait.getIconName();
		}
	}

	public virtual bool isCharged()
	{
		return getActorStats().hasTrait(chargeUpTrait) >= 0;
	}
	
	public override CombatAction clone()
	{
		ChargeUpAbility clone = (ChargeUpAbility) base.Clone();

		clone.actionWhenCharged = actionWhenCharged.clone();

        return clone;
	}
	

}
