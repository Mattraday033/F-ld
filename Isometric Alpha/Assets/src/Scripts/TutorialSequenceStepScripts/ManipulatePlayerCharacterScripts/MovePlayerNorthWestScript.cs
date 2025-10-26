using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerNorthWestScript  : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        FaceNorthWestScript.runScript();
        AreaManager.getMovementManager().moveAllSprites(MovementManager.distance1TileNorthWestGrid);
    }
}

