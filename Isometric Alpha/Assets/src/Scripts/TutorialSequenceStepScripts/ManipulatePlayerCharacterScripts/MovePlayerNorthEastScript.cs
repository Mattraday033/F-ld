using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerNorthEastScript  : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        FaceNorthEastScript.runScript();
        MovementManager.getInstance().moveAllSprites(MovementManager.distance1TileNorthEastGrid);
    }
}

