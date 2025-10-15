using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerSouthWestScript  : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        FaceSouthWestScript.runScript();
        MovementManager.getInstance().moveAllSprites(MovementManager.distance1TileSouthWestGrid);
    }
}

