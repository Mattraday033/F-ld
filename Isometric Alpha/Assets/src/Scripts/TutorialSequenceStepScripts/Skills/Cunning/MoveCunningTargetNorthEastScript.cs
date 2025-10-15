using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCunningTargetNorthEastScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        CunningManager.getInstance().moveCurrentSelectorNorthEast(); 
    }

}
