using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        PlayerMovement.getInstance().interact();
    }
}

