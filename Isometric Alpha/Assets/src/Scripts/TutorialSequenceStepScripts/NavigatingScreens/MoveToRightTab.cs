using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRightTab : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        OverallUIManager.moveToScreenToTheRight();
    }
}
