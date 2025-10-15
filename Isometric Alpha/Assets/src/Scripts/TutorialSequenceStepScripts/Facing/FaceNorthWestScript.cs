using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceNorthWestScript : TutorialSequenceStepScript
{
    private const bool moveOverride = true;
    public override void runScript(GameObject target)
    {
        State.playerFacing.setFacing(Facing.NorthWest);

        PlayerMovement playerMovement = PlayerMovement.getInstance();

        playerMovement.adjustDirectionalModifierGrid();

        playerMovement.adjustAnimator(moveOverride);
    }
    
    public static void runScript()
    {
        FaceNorthWestScript faceNorthWestScript = new FaceNorthWestScript();
        faceNorthWestScript.runScript(null);
    }
}

