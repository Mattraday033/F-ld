using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistUpgradePassiveAbility : PassiveAbility //passives are (currently) mostly used to explain to the player some mechanic that happens naturally,
{                                              //like regeneration
    public FistUpgradePassiveAbility(CombatActionSettings settings, List<TargetingLogic> targetingLogics = null) :
    base(settings, targetingLogics)
    {

    }

    public override CombatAction alternateActionWhenPlacedInActionSlot()
    {
        return new FistAttack(actorStats);
    }
}
