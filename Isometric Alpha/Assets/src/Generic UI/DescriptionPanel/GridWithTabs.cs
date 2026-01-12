using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridWithTabs : UIListenerGrid
{
    public override DescribableList getDescribableListType()
    {
        return AbilityGridSideTab.getDescribableListType();
    }
}
