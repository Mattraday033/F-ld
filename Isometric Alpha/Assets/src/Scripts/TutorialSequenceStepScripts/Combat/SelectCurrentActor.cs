using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectCurrentActor : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {
        if(CombatGrid.combatantExistsAtCoords(SelectorManager.currentSelector.getCoords(), out Stats combatant))
        {
            return;
        }

        AbilityMenuManager currentAbilityManager = combatant.getAbilityMenuManager();

        currentAbilityManager.enableAbilityButtonCanvas();
    }

    public static bool hasActorTarget()
    {
        return CombatGrid.combatantExistsAtCoords(SelectorManager.currentSelector.getCoords());
    }
}
