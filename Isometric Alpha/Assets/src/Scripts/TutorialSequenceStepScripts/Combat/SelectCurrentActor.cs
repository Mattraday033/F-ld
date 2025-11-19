using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectCurrentActor : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        Selector currentSelector = SelectorManager.currentSelector;

        AbilityMenuManager currentAbilityManager = CombatGrid.getCombatantAtCoords(currentSelector.getCoords()).getAbilityMenuManager();

        currentAbilityManager.enableAbilityButtonCanvas();
    }
}
