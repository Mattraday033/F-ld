using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAllFollowersScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        PartyMemberPlacer.removeAllPlacedPartyMembers();
    }

}
