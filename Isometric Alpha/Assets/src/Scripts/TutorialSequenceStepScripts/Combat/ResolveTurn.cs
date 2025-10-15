using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveTurn : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        CombatStateManager.getInstance().resolveTurn();
    }

}
