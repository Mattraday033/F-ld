using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerNorthEastScript  : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        FaceNorthEastScript.runScript();
        PlayerMovement.getInstance().directionMod = MovementManager.distance1TileNorthEastGrid;
        AreaManager.getMovementManager().moveAllSprites();
    }
}

