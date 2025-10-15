using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceReapplicationAbility : HealingAbility
{
    public StanceReapplicationAbility(CombatActionSettings settings) :
    base(settings)
    {

    }

    public override void performCombatAction()
    {
        Stats actor = getActorStats();
        setAppliedTrait(actor.getTraitOfType(TraitList.stanceTraitType)); // no null check because it's ok to set appliedTrait to null

        base.performCombatAction();
    }

}
