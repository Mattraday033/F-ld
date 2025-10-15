using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTerrain : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        PlayerMovement playerMovement = PlayerMovement.getInstance();

        playerMovement.setTerrainActive(false);
    }
}
