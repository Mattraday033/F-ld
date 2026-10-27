using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatActionUtil
{
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
            baseDamage = (int)(baseDamage * actor.getCritDamageMultiplier());
            baseDamage += (int)((float)targetCombatant.getTotalHealth() * actor.getDevastatingCriticalPercentage()); //will return 0f if not a devastatingCritical
        }

        if (CombatStateManager.isPlayerSurpriseRound())
        {
            baseDamage = (int)((float)baseDamage * actor.getSurpriseDamageMultiplier());
        }

        if(combatAction.healsTarget)
        {
            return targetCombatant.modifyIncomingHealing(baseDamage);
        } else
        {
            return targetCombatant.modifyIncomingDamage(baseDamage, actor.getArmorPenetration());
        }
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
        if (!(combatAction is VolleyAbility))
        {
            SelectorManager.declareSelectors();
        }
    }

    private static List<Stats> getActorsToHighlight(CombatAction combatAction)
    {
        List<Stats> actors = new List<Stats>();

        if (combatAction is VolleyAbility volleyAbility)
        {
            foreach (Stats actorStats in volleyAbility.allActors)
            {
                if (actorStats != null)
                {
                    actors.Add(actorStats);
                }
            }
        }
        else if (combatAction.actorStats != null)
        {
            actors.Add(combatAction.actorStats);
        }

        return actors;
    }

    #endregion
}
