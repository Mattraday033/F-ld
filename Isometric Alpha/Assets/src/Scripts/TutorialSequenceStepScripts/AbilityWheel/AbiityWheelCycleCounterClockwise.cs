using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWheelCycleCounterClockwise : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        AbilityMenuManager abilityMenuManager = AbilityMenuManager.getInstance();

        abilityMenuManager.moveSelectedButtonCounterClockwise();
    }
}
