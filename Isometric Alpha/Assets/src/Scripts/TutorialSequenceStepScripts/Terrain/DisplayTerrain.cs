using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTerrain : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {
        PlayerInput.toggleTerrainKeyCheck();
    }
}
