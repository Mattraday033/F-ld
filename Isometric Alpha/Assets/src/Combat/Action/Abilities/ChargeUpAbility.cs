using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUpAbility : Ability
{
	public const string chargingUpName = "Charging Up";
    public const string chargingUpDescription = "Target creature gains a Charge Trait. Charge Type Traits enable creatures to use more powerful abilities, and often provide other passive benefits.";


    public EffectAnimationType? currentEffectType = null;
	public CombatAction actionWhenCharged { get; private set; }
	public Trait chargeUpTrait { get; private set; }

	public virtual bool charged => actorStats?.hasTrait(chargeUpTrait) ?? false;

	public override Stats actorStats
	{
		set
		{
			base.actorStats = value;
			actionWhenCharged.actorStats = value;
		}
	}

	public ChargeUpAbility(Trait chargeUpTrait, Ability actionWhenCharged, List<TargetingLogic> targetingLogics = null) :
		base(CombatActionSettings.build(actionWhenCharged.getKey(), DescriptionParams.build(actionWhenCharged.displayName, iconName: actionWhenCharged.iconName, useDescription: actionWhenCharged.getUseDescription(), loreDescription: actionWhenCharged.getLoreDescription()),
																	DamageParams.build(actionWhenCharged.getDamageFormula(), actionWhenCharged.getCritFormula()),
																	TargetParams.build(SelectorTemplate.Single, actionWhenCharged.selfTargeting),
																	FrequencyParams.build(actionWhenCharged.getMaximumSlots(), actionWhenCharged.maximumCooldown),
																	costParams: CostParams.build(actionWhenCharged),
																	appliedTrait: actionWhenCharged.appliedTrait) //Left off relatedTraits
			, targetingLogics)
	{
		this.chargeUpTrait = chargeUpTrait.clone();
		this.actionWhenCharged = actionWhenCharged.clone();
	}

	public override Selector selector
	{
		set
		{
			base.selector = value;
			actionWhenCharged.selector = value;
		}
	}

    /*
	public override void setTargetCoords(GridCoords newTargetCoords)
	{
		base.setTargetCoords(newTargetCoords);
		actionWhenCharged.setTargetCoords(newTargetCoords);
	}
	*/
    public override void performCombatAction(List<Stats> targets)
    {
        if (!charged)
        {
            foreach(Stats target in targets)
            {
                if(target != null)
                {
                    foreach (GridCoords targetCoords in target.positions)
                    {
                        createEffectAnimation(targetCoords);
                    }
                    target.addTrait(chargeUpTrait);
                }
            }
        }
        else
        {
            actorStats.removeTrait(chargeUpTrait);
            //actionWhenCharged.selector = selector;
            actionWhenCharged.performCombatAction(targets);
        }
    }
    
    public override void playActivationAnimation()
    {
        // if (!charged)
        // {
            // actorStats.playAttackIntoSecondaryIdleAnimation();
        // }
        // else
        // {
            actorStats.playAttackIntoFrontIdleAnimation();
        // }
    }

	public override bool selfTargeting
	{
        get
        {
            if (charged)
            {
                return actionWhenCharged.selfTargeting;
            }
            else
            {
                return true;
            }
        }
	}

	public override SelectorTemplate getRangeTemplate()
	{
		if (charged)
		{
			return actionWhenCharged.getRangeTemplate();
		}
		else
		{
			return base.getRangeTemplate();
		}
	}

	public override string getRangeTitle()
	{
		if (charged)
		{
			return actionWhenCharged.getRangeTemplate().ToFriendlyString();
		}
		else
		{
			return base.getRangeTemplate().ToFriendlyString();
		}
	}

    //only the displayed name changes while charging, the ability's key stays the same
    public override string displayName
    {
        get
        {
            if (charged)
            {
                return base.displayName;
            }
            else
            {
                return chargingUpName;
            }
        }
    }
    
    public override EffectAnimationType effectAnimationType
    {
        get
        {
            if(currentEffectType == null)
            {
                if(!charged)
                {
                    currentEffectType = EffectAnimationType.Positive;
                } else
                {
                    currentEffectType = actionWhenCharged.effectAnimationType;
                }

                CombatStateManager.OnNewTurn.AddListener(removeCurrentEffectType);
                CombatStateManager.OnCombatEnd.AddListener(removeCurrentEffectType);
            }

            return currentEffectType.Value;
        }
    }

    private void removeCurrentEffectType()
    {
        currentEffectType = null;
        CombatStateManager.OnNewTurn.RemoveListener(removeCurrentEffectType);
        CombatStateManager.OnCombatEnd.RemoveListener(removeCurrentEffectType);
    }

    public override string iconName
    {
        get
        {
            if (charged)
            {
                return actionWhenCharged.iconName;
            }
            else
            {
                return chargeUpTrait.getIconName();
            }
        }
    }
	
	public override CombatAction clone()
	{
		ChargeUpAbility clone = (ChargeUpAbility) base.Clone();

		clone.actionWhenCharged = actionWhenCharged.clone();

        return clone;
	}

    public override string getUseDescription()
	{
        if(charged)
        {
            return actionWhenCharged.getUseDescription();
        } 
        else
        {
            return chargingUpDescription;
        }
	}
	

}
