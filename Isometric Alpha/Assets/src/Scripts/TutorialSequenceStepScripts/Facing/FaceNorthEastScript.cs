using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceNorthEastScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        PlayerMovement.setPlayerFacing(Facing.NorthEast);
    }
}

