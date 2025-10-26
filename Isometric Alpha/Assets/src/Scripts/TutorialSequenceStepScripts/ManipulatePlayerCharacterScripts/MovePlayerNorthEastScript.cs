using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerNorthEastScript  : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        FaceNorthEastScript.runScript();
        AreaManager.getMovementManager().moveAllSprites(MovementManager.distance1TileNorthEastGrid);
    }
}

