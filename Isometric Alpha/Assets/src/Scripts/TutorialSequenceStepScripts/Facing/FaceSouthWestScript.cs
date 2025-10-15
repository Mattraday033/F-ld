using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSouthWestScript : TutorialSequenceStepScript
{
    private const bool moveOverride = true;
    public override void runScript(GameObject target)
    {
        State.playerFacing.setFacing(Facing.SouthWest);

        PlayerMovement playerMovement = PlayerMovement.getInstance();

        playerMovement.adjustDirectionalModifierGrid();

        playerMovement.adjustAnimator(moveOverride);
    }
    
    public static void runScript()
    {
        FaceSouthWestScript faceSouthWestScript = new FaceSouthWestScript();
        faceSouthWestScript.runScript(null);
    }
}

