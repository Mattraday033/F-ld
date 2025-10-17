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
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inMap);
        }
        else
        {
            PopUpBlocker.destroyPopUpScreenBlocker();
            TutorialSequence.endCurrentTutorialSequence();
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        }
    }
}