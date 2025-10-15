using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerSouthEastScript  : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        FaceSouthEastScript.runScript();
        MovementManager.getInstance().moveAllSprites(MovementManager.distance1TileSouthEastGrid);
    }
}

