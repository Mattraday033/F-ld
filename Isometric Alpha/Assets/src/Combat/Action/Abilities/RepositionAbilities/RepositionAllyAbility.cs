using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RepositionAllyAbility : RepositionAbility, IJSONConvertable
{
    private bool requiresAction;

	public override bool repositionsCaster => false;
	// public override bool targetsOnlyEmptySpace => true;
	public override bool targetsAllySection => true;
	public override bool requiresAnAction => requiresAction;
	public override bool healsTarget => true;

	public RepositionAllyAbility(CombatActionSettings settings, bool requiresAction = true, List<TargetingLogic> targetingLogics = null) :
		base(settings, targetingLogics)
	{
        this.requiresAction = requiresAction;
	}

	public override void performCombatAction()
	{
		base.performCombatAction();

		actorStats.addTrait(appliedTrait);

        if(!requiresAnAction)
        {
	        chargeActorActionCost();
            setCooldownToMax();
        }
	}

    public override EffectAnimationType effectAnimationType => EffectAnimationType.Positive;

    public override void createEffectAnimation(GridCoords targetCoords, bool crit = false, int damageNumber = noDamage)
    {
        targetCoords = getDestinationCoords();

        AnimationTracker.loadInstantEffect(effectAnimationType, targetCoords, crit, damageNumber, healsTarget, targetMustBeDead);
    }
	
}
