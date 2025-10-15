using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCunningScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        CunningManager.getInstance().executeSkill();
    }

}
