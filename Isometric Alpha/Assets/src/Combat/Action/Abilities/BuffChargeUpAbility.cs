using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffChargeUpAbility : ChargeUpAbility
{

	public BuffChargeUpAbility(Trait chargeUpTrait, Ability actionWhenCharged) :
		base(chargeUpTrait, actionWhenCharged) 
	{

	}


	public override bool isSelfTargeting()
	{
        return false;
	}

	public override SelectorTemplate getRangeTemplate()
	{
		if (isCharged())
		{
			return actionWhenCharged.getRangeTemplate();
		}
		else
		{
			return SelectorTemplate.BoxThree;
		}
	}

	public override string getRangeTitle()
	{
		if (isCharged())
		{
			return actionWhenCharged.getRangeTemplate().ToFriendlyString();
		}
		else
		{
			return SelectorTemplate.BoxThree.ToFriendlyString();
		}
	}

    public override Selector getTargetSelector() //used for finding selectors when enemies are targeting
    {
        if(isCharged())
        {
            return base.getTargetSelector();
        } 
        else if(CombatGrid.positionIsOnAlliedSide(getActorCoords()))
        {
            Debug.LogError("BuffChargeUpAbility has not implemented Ally Side Targeting when not charged");

            return base.getTargetSelector();
            
        }
        else
        {

            TraitContainer newTraitContainer = new TraitContainer(getActorStats());

            newTraitContainer.addTrait(TraitList.specificHexadecupleBoxEnemySide);

            return newTraitContainer.findTargetLocation(SelectorFactory.buildByTemplate(getRangeTemplate()), new List<Stats>());
        }
    }
}
