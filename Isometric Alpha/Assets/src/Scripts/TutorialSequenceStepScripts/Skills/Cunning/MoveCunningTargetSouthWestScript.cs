using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCunningTargetSouthWestScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        CunningManager.getInstance().moveCurrentSelectorSouthWest(); 
    }

}
