using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishIntimidateChargesScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        IntimidateManager.resetIntimidatesRemaining();
    }

}
