using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StackOnKillTrait: StackableTrait
{

    public StackOnKillTrait(UnityEvent reapplicationEvent, int startingStacks, int stacksAppliedPerApplication, ActionCostType costType, Trait baseTrait) :
	base(reapplicationEvent, startingStacks, stacksAppliedPerApplication, costType, baseTrait)
    {

    }

    public override void onReapplicationEvent()
    {
        if(CombatActionManager.currentActor == getTraitHolder())
        {
            reapply();
        }
    }
}

public class StackOnKillOrNewTurnTrait : StackableTrait
{
    public StackOnKillOrNewTurnTrait(UnityEvent[] reapplicationEvents, int startingStacks, int stacksAppliedPerApplication, int maximumStacks, ActionCostType costType, Trait baseTrait) :
    base(reapplicationEvents, startingStacks, stacksAppliedPerApplication, maximumStacks, costType, baseTrait)
    {

    }

    public override void onReapplicationEvent()
    {
        if (CombatActionManager.currentActor == getTraitHolder() || CombatStateManager.whoseTurn == WhoseTurn.Player)
        {
            reapply();
        }
    }
}