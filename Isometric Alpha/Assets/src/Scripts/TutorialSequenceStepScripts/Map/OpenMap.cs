using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMap : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        if (PlayerObject.getInstance() != null)
        {
            PlayerObject.getInstance().mapPopUpButton.spawnPopUp();
        }
    }
}

