using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SubcategoryDescriptionSlot : UIDescriptionPanelSlot
{
    public override void updateCounter(IDescribable describable)
    {   
        if(describable as IJournalSubcategory == null)
        {
            return;
        }
        
        setPrimaryDescribable(describable);
    }
}