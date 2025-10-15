using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIntimidateRangeScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        IntimidateManager.getInstance().createSkillArea();
    }

}
