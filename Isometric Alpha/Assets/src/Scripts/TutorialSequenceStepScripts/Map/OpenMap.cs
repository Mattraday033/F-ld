using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OpenMap : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        if (PlayerMovement.getInstance() != null)
        {
            PlayerMovement.getInstance().mapPopUpButton.spawnPopUp();
        }
    }
}

