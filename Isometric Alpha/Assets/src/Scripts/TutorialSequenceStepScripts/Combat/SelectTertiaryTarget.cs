using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectTertiaryTarget : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        Selector currentSelector = SelectorManager.currentSelector;
        CombatAction loadedCombatAction = AbilityMenuManager.getInstance().getCurrentlySelectedAbility();


        loadedCombatAction.setTertiaryCoords(currentSelector.getCoords());

        currentSelector.setToOriginalColor();

        SelectorManager.getInstance().finishChoosingTertiary(loadedCombatAction);

        CombatStateManager.setCurrentActivity(CurrentActivity.Tutorial);
    }
}
