using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipMapTutorialScript : SkipTutorialScript
{
    public override void runScript()
    {
        if (MapPopUpWindow.getInstance() != null)
        {
            TutorialSequence.endCurrentTutorialSequence();
            PlayerStateManager.setCurrentActivity(CurrentActivity.InMap);
        }
        else
        {
            PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
            TutorialSequence.endCurrentTutorialSequence();
            PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);
        }
    }
}