using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//something an action does when it's queued, undone when it's unqueued.
//The action is passed in, so one unit can serve every action that uses it
public interface IQueuingUnit
{
    //costs come first, and PushBack has to work out its moves before GeneratePlaceholder reads them
    public readonly static Type[] executionOrder =  {
                                                        typeof(ChargeActionCost),
                                                        typeof(ChargeItemUse),
                                                        typeof(PushBack),
                                                        typeof(GeneratePlaceholder)
                                                    };

    public void onQueue(CombatAction action);
    public void onUnqueue(CombatAction action);
}

//shows a see-through copy of each combatant the action will move, at the spot it will move to.
//Keeps no state of its own: the moves are in the action's catalog, and each move finds its placeholder on the grid
public class GeneratePlaceholder : IQueuingUnit
{
    public void onQueue(CombatAction action)
    {
        foreach(MoveCombatant move in action.actionCatalog[typeof(MoveCombatant)])
        {
            move.generatePlaceholder(action);
        }

        if(action.targetsAllySection)
        {
            CombatActionManager.getInstance().promptLaterCombatActionsToFindNewTarget();
        }
    }

    //releases every placeholder onQueue made or shared for this action; one is only destroyed once no other queued action shows it.
    //Runs before PushBack.onUnqueue clears the catalog, so the moves are still there to find them by
    public void onUnqueue(CombatAction action)
    {
        foreach(MoveCombatant move in action.actionCatalog[typeof(MoveCombatant)])
        {
            move.releasePlaceholder(action);
        }

        CombatActionManager.getInstance().promptLaterCombatActionsToReturnToPreviousTarget();
    }

}

//works out, when the action is queued, where each target under its selector will be pushed, and adds those moves to the
//action's catalog so later units can read them. Immobile targets get a move to the spot they're already on
public class PushBack : IQueuingUnit
{
    public void onQueue(CombatAction action)
    {
        foreach(Stats target in action.selector.getAllTargets().Select(c => c.stats))
        {
            if(target.isImmobile)
            {
                action.actionCatalog.Add(new MoveCombatant(target, target.positions));
            } else
            {
                action.actionCatalog.Add(TargetingUtil.push(target));
            }
        }
    }

    //the moves only held while the action was queued, so unqueuing throws them away
    public void onUnqueue(CombatAction action)
    {
        action.actionCatalog.clear();
    }
}

//the actor pays the action's costs when it's queued, and gets them back if it's unqueued
public class ChargeActionCost : IQueuingUnit
{
    public void onQueue(CombatAction action)
    {
        if(action.hasActionCost)
        {
            action.actorStats.payActionCost(action);
        }
    }

    public void onUnqueue(CombatAction action)
    {
        if(action.hasActionCost)
        {
            action.actorStats.refundActionCost(action);
        }
    }
}

//the player uses up one of the action's source item when it's queued, and gets it back if it's unqueued.
//Items with infinite uses cost nothing, so there's nothing to take or give back
public class ChargeItemUse : IQueuingUnit
{
    public void onQueue(CombatAction action)
    {
        Item item = action.sourceItem;

        if(costsItemUse(item))
        {
            Inventory.removeItem(item, 1);
        }
    }

    public void onUnqueue(CombatAction action)
    {
        Item item = action.sourceItem;

        if(costsItemUse(item))
        {
            Item refund = item.clone();
            refund.setQuantity(1);

            Inventory.addItem(refund);
        }
    }

    private static bool costsItemUse(Item item)
    {
        return item != null && !item.infiniteUses();
    }
}
