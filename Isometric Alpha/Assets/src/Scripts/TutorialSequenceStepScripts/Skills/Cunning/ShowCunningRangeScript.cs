using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCunningRangeScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        CunningManager.getInstance().createSkillArea();
    }

}
