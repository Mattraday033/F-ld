using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTargetScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        IRevealable revealable = target.GetComponent<IRevealable>();

        RevealManager.manuallyRevealGameObject(target, revealable.getRevealColor());
    }
}

