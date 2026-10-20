using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryGateMouseHover : NPCMouseHover
{
    public TemporaryGate temporaryGate;

    //an open gate keeps its body collider off, so leaving a skill must not switch it back on
    protected override void enableHover()
    {
        if(temporaryGate != null && temporaryGate.open)
        {
            return;
        }

        base.enableHover();
    }
}
