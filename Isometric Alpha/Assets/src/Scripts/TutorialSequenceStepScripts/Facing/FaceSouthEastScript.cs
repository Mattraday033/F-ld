using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSouthEastScript : TutorialSequenceStepScript
{
    private const bool moveOverride = true;
    public override void runScript(GameObject target)
    {
        State.playerFacing.setFacing(Facing.SouthEast);

        PlayerMovement playerMovement = PlayerMovement.getInstance();

        playerMovement.adjustDirectionalModifierGrid();

        playerMovement.adjustAnimator(moveOverride);
    }
    
    public static void runScript()
    {
        FaceSouthEastScript faceSouthEastScript = new FaceSouthEastScript();
        faceSouthEastScript.runScript(null);
    }
}

