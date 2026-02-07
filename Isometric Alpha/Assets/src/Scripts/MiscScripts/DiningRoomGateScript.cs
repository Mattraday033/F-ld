using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningRoomGateScript : PlayerInteractionScript
{

    public override void runScript()
    {
        GateAndChestManager.addKey(ZoneKeyList.manseFirstFloor + LocationNameList.section2a + NPCNameList.ancientPortcullis);
    }

}
