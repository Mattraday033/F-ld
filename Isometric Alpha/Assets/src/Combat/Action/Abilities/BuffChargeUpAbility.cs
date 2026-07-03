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

	public override string getRangeIndex()
	{
		if (isCharged())
		{
			return actionWhenCharged.getRangeIndex();
		}
		else
		{
			return SelectorList.boxThreeName;
		}
	}

	public override string getRangeTitle()
	{
		if (isCharged())
		{
			return actionWhenCharged.getRangeIndex();
		}
		else
		{
			return SelectorList.boxThreeName;
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

            return newTraitContainer.findTargetLocation(SelectorList.getByName(getRangeIndex()), new List<Stats>());
        }
    }
}
