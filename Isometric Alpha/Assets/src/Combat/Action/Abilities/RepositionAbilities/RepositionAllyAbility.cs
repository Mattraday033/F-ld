using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RepositionAllyAbility : RepositionAbility, IJSONConvertable
{
	public RepositionAllyAbility(CombatActionSettings settings) :
		base(settings)
	{

	}

	public override void performCombatAction()
	{
		base.performCombatAction();

		getActorStats().addTrait(getAppliedTrait());
	}

	public override bool repositionsCaster()
	{
		return true;
	}

	// public override bool targetsOnlyEmptySpace()
	// {
	// 	return true;
	// }

	public override bool targetsAllySection()
	{
		return true;
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

        CombatAnimationManager.loadInstantEffect(this, targetCoords, crit, damageNumber, healsTarget, targetCanBeDead);
    }
	
}
