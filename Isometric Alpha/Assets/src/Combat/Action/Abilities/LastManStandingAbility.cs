using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastManStandingAbility : ChargeUpAbility
{
    /*
    NEEDS TESTING OF LOGIC FOR WHEN IS CHARGED/WHEN IS NOT CHARGED/WHEN SHOULD ATTACK/WHEN SHOULD DEFEND for the Cave Matron
    */

	public bool hasCharge
    {
        get
        {
            return false;
        }
        protected set
        {
            hasCharge = value;
        }
    }

	public override Stats actorStats
	{
		set
		{
			base.actorStats = value;
			actionWhenCharged.actorStats = value;
		}
	}

	public LastManStandingAbility(Trait chargeUpTrait, Ability actionWhenCharged):
		base(chargeUpTrait, actionWhenCharged)
	{
		
	}
	
	public override Selector selector
	{
		set
		{
			base.selector = value;
			actionWhenCharged.selector = value;
		}
	}

	public override void performCombatAction(List<Stats> targets)
	{
		if(charged)
		{
			actorStats.removeTrait(chargeUpTrait);
			actionWhenCharged.performCombatAction(targets);
		}
		
		if(shouldApplyTrait())
		{
            createEffectAnimation(actorCoords);
			actorStats.addTrait(chargeUpTrait);
		}
	}
	
    public override void playActivationAnimation()
    {
        if (shouldApplyTrait())
        {
            actorStats.playAttackIntoSecondaryIdleAnimation();
        }
        else
        {
            actorStats.playAttackIntoFrontIdleAnimation();
        }
    }

	public bool shouldApplyTrait()
	{
		return charged && !actorStats.hasTrait(chargeUpTrait);
	}
	
	public override bool charged
	{
        get
        {
            if(actorStats?.isAlive ?? false)
            {
                switch(actorStats.getCombatGridSide())
                {
                    case CombatGridSide.Ally:
                        return CombatGrid.getTotalAliveAllyCount() != 1;
                    case CombatGridSide.Enemy:
                        return CombatGrid.getTotalAliveEnemyCount() != 1;
                    default:
                        return false;
                }
            } 

            return false;
        }
	}

}
