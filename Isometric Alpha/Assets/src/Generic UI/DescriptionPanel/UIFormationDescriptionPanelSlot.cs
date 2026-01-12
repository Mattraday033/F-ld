using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class UIFormationDescriptionPanelSlot : UIDescriptionPanelSlot
{
    public override void updateCounter()
    {
        if(OverallUIManager.currentScreenManager != null)
        {
            setPrimaryDescribable(State.formation);
        }
    }
}