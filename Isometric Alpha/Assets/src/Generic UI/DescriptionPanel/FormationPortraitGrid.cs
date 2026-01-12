using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FormationPortraitGrid : UIListenerGrid
{
    public override DescribableList getDescribableListType()
    {
        return DescribableList.PartyMembersWithPlayer;
    }

    public override IEnumerable<IDescribable> getDescribableList()
    {
        return State.formation.getAllPartyStatsInFormation();
    }
}
