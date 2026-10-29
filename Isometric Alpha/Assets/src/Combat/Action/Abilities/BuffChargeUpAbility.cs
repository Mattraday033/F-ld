using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffChargeUpAbility : ChargeUpAbility
{

    public override Selector selector
    {
        get
        {
            if (selfTargeting)
            {
                _Selector = SelectorFactory.buildByTemplate(getRangeTemplate());
                _Selector.setToLocation(actorCoords);
                return _Selector;
            }

            if(actorStats?.usePlayerTargeting ?? false)
            {
                return _Selector;
            }
            
            if(charged)
            {
                return base.selector;
            } 
            else if(CombatGrid.positionIsOnAlliedSide(actorCoords))
            {
                Debug.LogError("BuffChargeUpAbility has not implemented Ally Side Targeting when not charged");

                return base.selector;
                
            }
            else
            {
                TraitContainer newTraitContainer = new TraitContainer(actorStats);

                newTraitContainer.addTrait(TraitList.specificHexadecupleBoxEnemySide);

                return newTraitContainer.findTargetLocation(SelectorFactory.buildByTemplate(getRangeTemplate()), new List<Stats>());
            }
        }
    }


	public BuffChargeUpAbility(Trait chargeUpTrait, Ability actionWhenCharged, List<TargetingLogic> targetingLogics = null) :
		base(chargeUpTrait, actionWhenCharged, targetingLogics) 
	{

	}

	public override SelectorTemplate getRangeTemplate()
	{
		if (charged)
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
		if (charged)
		{
			return actionWhenCharged.getRangeTemplate().ToFriendlyString();
		}
		else
		{
			return SelectorTemplate.BoxThree.ToFriendlyString();
		}
	}

}
