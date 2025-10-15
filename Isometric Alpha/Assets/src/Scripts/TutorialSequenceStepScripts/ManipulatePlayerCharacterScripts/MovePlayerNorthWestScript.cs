using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerNorthWestScript  : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        FaceNorthWestScript.runScript();
        MovementManager.getInstance().moveAllSprites(MovementManager.distance1TileNorthWestGrid);
    }
}

