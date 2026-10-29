using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceReapplicationAbility : HealingAbility
{
    public StanceReapplicationAbility(CombatActionSettings settings, List<TargetingLogic> targetingLogics = null) :
    base(settings, targetingLogics)
    {

    }

    public override void performCombatAction()
    {
        Stats actor = actorStats;
        setAppliedTrait(actor.getTraitOfType(TraitType.Stance)); // no null check because it's ok to set appliedTrait to null

        base.performCombatAction();
    }

}
