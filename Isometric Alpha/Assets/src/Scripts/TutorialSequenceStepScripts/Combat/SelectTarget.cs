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
            loadedCombatAction = currentAbilityManager.getCurrentlySelectedAction();
        }

        SelectorManager.getInstance().finishChoosingLocation(loadedCombatAction);

        currentAbilityManager.disableAbilityButtonCanvas();

        CombatStateManager.setCurrentActivity(CurrentActivity.Tutorial);

        if (loadedCombatAction.requiresTertiaryCoords())
        {
            TutorialSequence.overrideTutorialSequence(TutorialSequenceList.getCombatTutorialSequenceForReposition());
        }

    }

    public static bool hasTargets()
    {
        Selector currentSelector = SelectorManager.currentSelector;

        if (CombatStateManager.findingEmptySpaceForReposition())
        {
            if(currentSelector.hasAtLeastOneTarget(SelectorManager.allyAndEnemyTagCriteria))
            {
                return false;
            } else
            {
                return true;
            }
        }
        else
        {
            return currentSelector.hasAtLeastOneTarget(SelectorManager.allyAndEnemyTagCriteria);
        }

    }

    public static bool canPayCost()
    {
        AbilityMenuManager currentAbilityManager = AbilityMenuManager.getInstance();
        CombatAction loadedCombatAction = currentAbilityManager.getCurrentlySelectedAction();

        return loadedCombatAction.canPayActionCost(currentAbilityManager.actionArraySource);
    }
}
