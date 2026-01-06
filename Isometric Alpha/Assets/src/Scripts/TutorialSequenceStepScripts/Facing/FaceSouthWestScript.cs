using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSouthWestScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        State.playerFacing.setFacing(Facing.SouthWest);

        PlayerMovement.adjustPlayerDirectionalModifierGrid();
    }
}

