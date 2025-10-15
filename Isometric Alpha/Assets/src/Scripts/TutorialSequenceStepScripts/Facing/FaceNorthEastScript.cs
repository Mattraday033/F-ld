using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceNorthEastScript : TutorialSequenceStepScript
{
    private const bool moveOverride = true;
    public override void runScript(GameObject target)
    {
        State.playerFacing.setFacing(Facing.NorthEast);

        PlayerMovement playerMovement = PlayerMovement.getInstance();

        playerMovement.adjustDirectionalModifierGrid();

        playerMovement.adjustAnimator(moveOverride);
    }

    public static void runScript()
    {
        FaceNorthEastScript faceNorthEastScript = new FaceNorthEastScript();
        faceNorthEastScript.runScript(null);
    }
}

