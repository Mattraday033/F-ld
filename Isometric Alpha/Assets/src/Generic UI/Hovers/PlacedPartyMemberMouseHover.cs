using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedPartyMemberMouseHover : NPCMouseHover
{
    public PlacedPartyMember placedPartyMember;

    //a hidden party member keeps its body collider off, so leaving a skill must not switch it back on
    protected override void enableHover()
    {
        if(placedPartyMember != null && placedPartyMember.hidden)
        {
            return;
        }

        base.enableHover();
    }
}
