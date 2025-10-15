using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateIntimidateScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        IntimidateManager.getInstance().executeSkill();
    }

}
