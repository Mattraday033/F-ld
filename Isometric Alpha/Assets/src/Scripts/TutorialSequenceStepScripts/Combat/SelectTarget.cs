using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectTarget : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        Selector currentSelector = SelectorManager.currentSelector;
        CombatAction loadedCombatAction;
        AbilityMenuManager currentAbilityManager = AbilityMenuManager.getInstance();

        if (CombatStateManager.findingEmptySpaceForReposition())
        {
            loadedCombatAction = RepositionManager.currentSingleTargetRepositionCombatAction;
        }
        else
        {
            loadedCombatAction = currentAbilityManager.getCurrentlySelectedLoadedCombatAction();
        }

        SelectorManager.getInstance().finishChoosingLocation(loadedCombatAction);

        currentAbilityManager.disableAbilityButtonCanvas();

        CombatStateManager.setCurrentActivity(CurrentActivity.Tutorial);

        if (loadedCombatAction.requiresTertiaryCoords())
        {
            TutorialSequence.startTutorialSequence(TutorialSequenceList.getCombatTutorialSequenceForReposition());
        }

    }
}
