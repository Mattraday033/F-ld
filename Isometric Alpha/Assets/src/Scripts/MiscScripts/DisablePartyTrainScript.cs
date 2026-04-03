using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePartyTrainScript : PlayerInteractionScript
{
    public override void runScript(GameObject target = null)
    {
        Flags.stopPartyTrainSpawning();
    }
}
