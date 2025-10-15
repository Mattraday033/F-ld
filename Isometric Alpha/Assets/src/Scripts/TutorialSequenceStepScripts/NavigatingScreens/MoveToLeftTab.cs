using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLeftTab : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        OverallUIManager.moveToScreenToTheLeft();
    }
}
