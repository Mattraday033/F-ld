using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFollowerScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        PartyMemberPlacer.placeNextPartyMember();
    }

}
