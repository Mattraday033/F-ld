using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectCurrentActor : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {
        if(!CombatGrid.tryGetCombatantAt(SelectorManager.currentSelector.getCoords(), out Combatant combatant))
        {
            return;
        }

        AbilityMenuManager.getShared().showFor(combatant.stats);
    }

    public static bool hasActorTarget()
    {
        return CombatGrid.combatantExistsAtCoords(SelectorManager.currentSelector.getCoords());
    }
}
