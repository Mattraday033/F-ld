using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CombatActionUtil
{
    #region Queuing Units

    //runs each queuing unit's onQueue in IQueuingUnit.executionOrder
    public static void performAllQueuingUnits(this CombatAction combatAction)
    {
        List<IQueuingUnit> queuingUnits = getQueuingUnitsInOrder(combatAction);

        foreach(IQueuingUnit unit in queuingUnits)
        {
            unit.onQueue(combatAction);
        }
    }

    //runs each queuing unit's onUnqueue in reverse order, so each unit is undone before the units it relied on
    public static void performAllUnqueuingUnits(this CombatAction combatAction)
    {
        List<IQueuingUnit> reversedQueuingUnits = getQueuingUnitsInOrder(combatAction).Reverse<IQueuingUnit>().ToList();

        foreach(IQueuingUnit unit in reversedQueuingUnits)
        {
            unit.onUnqueue(combatAction);
        }
    }

    //like Catalog.GetValuesInOrder: a unit whose type isn't in the execution order is left out
    private static List<IQueuingUnit> getQueuingUnitsInOrder(CombatAction combatAction)
    {
        return combatAction.queuingUnits.Where(u => Array.IndexOf(IQueuingUnit.executionOrder, u.GetType()) >= 0)
                                        .OrderBy(u => Array.IndexOf(IQueuingUnit.executionOrder, u.GetType()))
                                        .ToList();
    }

    #endregion

    #region SFX

    public static void playEquipSFX(this CombatAction combatAction)
    {
        AudioManager.playAudioClipAsSingleton(combatAction.equipSFX);
    }

    public static void playUnequipSFX(this CombatAction combatAction)
    {
        AudioManager.playAudioClipAsSingleton(combatAction.unequipSFX);
    }

    #endregion

    public static int calculateDamage(this CombatAction combatAction, Stats targetCombatant, bool isCrit)
    {
        int baseDamage = combatAction.getDamageFormulaTotal();

        if (targetCombatant == null || baseDamage == 0)
        {
            return -1;
        }

        Stats actor = combatAction.actorStats;

        if (isCrit)
        {
            baseDamage = (int)(baseDamage * actor.critDamageMultiplier);
            baseDamage += (int)((float)targetCombatant.totalHealth * actor.devastatingCriticalPercentage); //will return 0f if not a devastatingCritical
        }

        if (CombatStateManager.isPlayerSurpriseRound())
        {
            baseDamage = (int)((float)baseDamage * actor.surpriseDamageMultiplier);
        }

        if(combatAction.healsTarget)
        {
            return targetCombatant.modifyIncomingHealing(baseDamage);
        } else
        {
            return targetCombatant.modifyIncomingDamage(baseDamage, actor.armorPenetration);
        }
    }


    public static bool actorIsPartOfAction(this CombatAction combatAction, Stats actor)
    {
        if(actor == null)
        {
            return false;
        }

        //uses deprecated combat actions (VolleyAbility), see Assets/src/DepricatedCombatActions
        // if (combatAction is VolleyAbility volleyAbility)
        // {
        //     return volleyAbility.allActors != null && volleyAbility.allActors.Contains(actor);
        // }

        return actor.Equals(combatAction.actorStats);
    }

    #region Highlight Actor Sprite

    public static void highlightActorSprites(this CombatAction combatAction)
    {
        foreach (Stats actorStats in getActorsToHighlight(combatAction))
        {
            actorStats.setOutline();
        }
    }

    public static void removeHighlightFromActorSprites(this CombatAction combatAction)
    {
        foreach (Stats actorStats in getActorsToHighlight(combatAction))
        {
            actorStats.removeOutline();
        }

        //volleys never redeclared selectors here, so keep that behaviour
        //uses deprecated combat actions (VolleyAbility), see Assets/src/DepricatedCombatActions
        // if (!(combatAction is VolleyAbility))
        // {
            SelectorManager.declareSelectors();
        // }
    }

    private static List<Stats> getActorsToHighlight(CombatAction combatAction)
    {
        List<Stats> actors = new List<Stats>();

        //uses deprecated combat actions (VolleyAbility), see Assets/src/DepricatedCombatActions
        // if (combatAction is VolleyAbility volleyAbility)
        // {
        //     foreach (Stats actorStats in volleyAbility.allActors)
        //     {
        //         if (actorStats != null)
        //         {
        //             actors.Add(actorStats);
        //         }
        //     }
        // }
        // else
        if (combatAction.actorStats != null)
        {
            actors.Add(combatAction.actorStats);
        }

        return actors;
    }

    #endregion
}
