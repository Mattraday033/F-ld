using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerSouthWestScript  : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        FaceSouthWestScript.runScript();
        PlayerMovement.getInstance().directionMod = MovementManager.distance1TileSouthWestGrid;
        AreaManager.getMovementManager().moveAllSprites();
    }
}

