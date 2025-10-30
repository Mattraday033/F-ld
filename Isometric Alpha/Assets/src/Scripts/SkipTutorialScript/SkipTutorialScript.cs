using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorialScript : ScriptableObject
{
    public virtual void runScript()
    {
        IntimidateManager.getInstance().destroySkillArea();
        CunningManager.getInstance().destroySkillArea();
        ObservationManager.getInstance().destroySkillArea();

        RevealManager.OnReveal.Invoke();

        TutorialSequence.endCurrentTutorialSequence();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
    }

}

public class SkipUpgradingPartyMemberTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        if (OverallUIManager.currentScreenManager != null)
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inUI);

            PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
        }
        else
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);

            PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
        }
    }
}

public class SkipAddingAbilitiesTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        if (FullEditAbilityWheelPopUpWindow.getInstance() != null || OverallUIManager.currentScreenManager != null)
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inUI);

            if (FullEditAbilityWheelPopUpWindow.getInstance() == null)
            {
                PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
            }
        }
        else
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        }
    }
}

public class SkipEquippingItemsTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        if (OverallUIManager.currentScreenManager != null)
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inUI);
        }
        else
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        }
    }
}

public class SkipFormationTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
        TutorialSequence.endCurrentTutorialSequence();
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inUI);
    }
}

public class SkipCombatTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        CombatStateManager.skipCombatTutorial();
    }
}