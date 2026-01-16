using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OOCStatsDescriptionPanelBuilder : StatsDescriptionPanelBuilder
{

    protected override void Awake()
    {
        base.Awake();

        BuilderFilterWhiteList newFilter = filter as BuilderFilterWhiteList;

        if(newFilter != null)
        {
            newFilter.whiteList.Add(DescriptionPanelBuildingBlockType.BonusDamageText);
            filter = newFilter;
        }

    }

}
