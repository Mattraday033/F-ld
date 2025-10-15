using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAndCsalan: SpeakAtStartScript
{

    public override void runScript()
    {
        if (!DeathFlagManager.isDead("Beam"))
        {
            dialogueTrigger.TriggerDialogue();
        }
    }

}
