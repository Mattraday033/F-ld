using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePartyTrainScript : PlayerInteractionScript
{
    public override void runScript(GameObject target = null)
    {
        Flags.allowPartyTrainSpawning();
    }
}
