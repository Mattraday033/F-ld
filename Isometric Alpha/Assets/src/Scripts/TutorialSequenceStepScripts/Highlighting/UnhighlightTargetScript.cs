using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhighlightTargetScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        RevealManager.manuallyRevealGameObject(target, RevealManager.tutorialDefault);
    }
}

