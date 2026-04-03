using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTargetScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {
        IRevealable revealable = target.GetComponent<IRevealable>();

        revealable.onReveal(Constants.reveal);
    }
}

