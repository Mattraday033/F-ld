using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningRoomGateScript : PlayerInteractionScript
{

    public override void runScript(GameObject target = null)
    {
        GateAndChestManager.addKey(ZoneKeyList.manseFirstFloor + LocationNameList.section2a + NPCNameList.ancientPortcullis);
    }

}
