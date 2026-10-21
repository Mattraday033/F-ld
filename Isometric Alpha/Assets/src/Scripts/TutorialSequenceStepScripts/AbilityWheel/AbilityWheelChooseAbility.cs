using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWheelChooseAbility : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {
        AbilityMenuManager abilityMenuManager = AbilityMenuManager.getInstance();

        abilityMenuManager.getCurrentlySelectedAbilityMenuButton().disableCombatActionSelectorPreview();
        abilityMenuManager.getCurrentlySelectedAbilityMenuButton().enableCombatActionSelector();
        abilityMenuManager.disableAbilityButtonCanvas();

        //the tutorial stays InTutorialSequence instead of moving to ChoosingLocation, so the activity change
        //that normally redraws the selectors never fires
        SelectorManager.declareSelectors();
    }
}
