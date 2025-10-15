using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishCunningChargesScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        CunningManager.resetCunningsRemaining();
    }

}
