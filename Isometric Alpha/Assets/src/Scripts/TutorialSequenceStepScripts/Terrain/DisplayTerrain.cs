using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTerrain : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        PlayerMovement playerMovement = PlayerMovement.getInstance();

        playerMovement.setTerrainActive(true);
    }
}
