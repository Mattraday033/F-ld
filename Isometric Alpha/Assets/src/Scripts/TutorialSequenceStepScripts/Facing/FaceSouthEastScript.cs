using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSouthEastScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        State.playerFacing.setFacing(Facing.SouthEast);

        PlayerMovement.adjustPlayerDirectionalModifierGrid();
    }
}

