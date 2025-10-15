using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastManStandingAbility : ChargeUpAbility
{
	private bool hasCharge = false;
	
	public LastManStandingAbility(Trait chargeUpTrait, Ability actionWhenCharged):
		base(chargeUpTrait, actionWhenCharged)
	{
		
	}
	
	public void setCharge(bool hasCharge)
	{
		this.hasCharge = hasCharge;
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
		
		if (actor != null)
		{
			Debug.LogError("setActor to " + actor.position.ToString());
		}
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
		if(isCharged())
		{
			getActorStats().removeTrait(chargeUpTrait);
			actionWhenCharged.performCombatAction(targets);
		}
		
		if(shouldApplyTrait())
		{
			getActorStats().addTrait(chargeUpTrait);
		}
	}
	
	//a bit of a hack, but when isSelfTargeting is called is when the game is asking if it should target itself or not
	// so it should lock in that choice when isSelfTargeting is called
	public override bool isSelfTargeting()
	{
		if(shouldApplyTrait())
		{
			setCharge(false);
			return true;
		} else
		{
			setCharge(true);
			return false;
		}
	}
	
	public bool shouldApplyTrait()
	{
		return CombatGrid.getTotalAliveEnemyCount() > 1;
	}
	
	public override bool isCharged()
	{
		return hasCharge;
	}
}
