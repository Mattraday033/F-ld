using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CategoryTitleListener : UIDescriptionPanelSlot
{

    public override void updateCounter(IDescribable describable)
    {
        if(describable as IJournalCategory == null)
        {
            return;
        }

        setPrimaryDescribable(describable);
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        return new List<UnityEvent>();
    }
}
