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

	public override int getRangeIndex()
	{
		if (isCharged())
		{
			return actionWhenCharged.getRangeIndex();
		}
		else
		{
			return Range.boxThreeIndex;
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
			return Range.getRangeTitle(Range.boxThreeIndex);
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

            return newTraitContainer.findTargetLocation(SelectorManager.getInstance().selectors[getRangeIndex()].clone(), new List<Stats>());
        }
    }
}
