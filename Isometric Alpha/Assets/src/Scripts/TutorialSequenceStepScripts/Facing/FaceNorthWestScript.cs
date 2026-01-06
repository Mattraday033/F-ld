using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceNorthWestScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        State.playerFacing.setFacing(Facing.NorthWest);

        PlayerMovement.adjustPlayerDirectionalModifierGrid();
    }
}

