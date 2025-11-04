using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerNorthWestScript  : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        FaceNorthWestScript.runScript();
        PlayerMovement.getInstance().directionMod = MovementManager.distance1TileNorthWestGrid;
        AreaManager.getMovementManager().moveAllSprites();
    }
}

