using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SettingsManager : ScreenManager
{
    public override bool requiresPartyMemberSelectionGrid()
    {
        return false;
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();
        listOfEvents.Add(OnScreenInteriorUpdate);

        return listOfEvents;
    }
    public override DescribableList getDefaultDescribableList()
    {
        return DescribableList.Unnecessary;
    }

    public override void updateCounter()
    {
        //Empty on Purpose
    }
}