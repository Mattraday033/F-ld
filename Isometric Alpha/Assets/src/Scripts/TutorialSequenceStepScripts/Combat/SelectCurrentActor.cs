using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectCurrentActor : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        Selector currentSelector = SelectorManager.currentSelector;

        Stats combatant = CombatGrid.getCombatantAtCoords(currentSelector.getCoords());

        if(combatant == null)
        {
            return;
        }

        AbilityMenuManager currentAbilityManager = combatant.getAbilityMenuManager();

        currentAbilityManager.enableAbilityButtonCanvas();
    }

    public static bool hasActorTarget()
    {
        Selector currentSelector = SelectorManager.currentSelector;

        Stats combatant = CombatGrid.getCombatantAtCoords(currentSelector.getCoords());

        if(combatant == null)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
