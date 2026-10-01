using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorialScript
{
    public virtual void runScript()
    {
        IntimidateManager.getInstance().destroySkillArea();
        CunningManager.getInstance().destroySkillArea();
        ObservationManager.getInstance().destroySkillArea();

        RevealManager.resetReveals();

        TutorialSequence.endCurrentTutorialSequence();
        PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);
        PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
    }
}

public class SkipInteractionTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        IntimidateManager.getInstance().destroySkillArea();
        CunningManager.getInstance().destroySkillArea();
        ObservationManager.getInstance().destroySkillArea();

        RevealManager.resetReveals();

        TutorialSequence.endCurrentTutorialSequence();
        DialogueManager.getInstance().endDialogue();
        PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);
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
            PlayerStateManager.setCurrentActivity(CurrentActivity.InUI);

            PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
        }
        else
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);

            PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
        }
    }
}

public class SkipAddingAbilitiesTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        if (OverallUIManager.currentScreenManager != null)
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerStateManager.setCurrentActivity(CurrentActivity.InUI);
        }
        else
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);
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
            PlayerStateManager.setCurrentActivity(CurrentActivity.InUI);
        }
        else
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);
        }
    }
}

public class SkipFormationTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        if (OverallUIManager.currentScreenManager != null)
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerStateManager.setCurrentActivity(CurrentActivity.InUI);
        }
        else
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);
        }
    }
}

public class SkipCombatTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        CombatStateManager.skipCombatTutorial();
    }
}