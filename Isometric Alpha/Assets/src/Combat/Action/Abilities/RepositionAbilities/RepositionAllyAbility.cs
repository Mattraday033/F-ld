using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RepositionAllyAbility : RepositionAbility, IJSONConvertable
{
    private bool requiresAction;

	public RepositionAllyAbility(CombatActionSettings settings, bool requiresAction = true) :
		base(settings)
	{
        this.requiresAction = requiresAction;
	}

	public override void performCombatAction()
	{
		base.performCombatAction();

		getActorStats().addTrait(getAppliedTrait());

        if(!requiresAnAction())
        {
	        chargeActorActionCost();
            setCooldownToMax();
        }
	}

	public override bool repositionsCaster()
	{
		return false;
	}

	// public override bool targetsOnlyEmptySpace()
	// {
	// 	return true;
	// }

	public override bool targetsAllySection()
	{
		return true;
	}


    public override bool requiresAnAction()
    {
        return requiresAction;
    }

	public override bool healsTarget()
	{
		return true;
	}

    public override string getEffectAnimationType()
    {
        return EffectAnimationType.Positive.ToString();
    }

    public override void createEffectAnimation(GridCoords targetCoords, bool crit, int damageNumber, bool healsTarget, bool targetCanBeDead)
    {
        targetCoords = getDestinationCoords();

        CombatAnimationManager.loadInstantEffect(getEffectAnimationType(), targetCoords, crit, damageNumber, healsTarget, targetCanBeDead);
    }
	
}
