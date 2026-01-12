using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubcategoryGrid : UIListenerGrid
{


    public override void addListeners()
    {
        GridRow.OnDescribableToDisplay.AddListener(updateCounter);
    }
    
    public override void removeListeners()
    {
        GridRow.OnDescribableToDisplay.RemoveListener(updateCounter);
    }

    public override void updateCounter(IDescribable describable)
    {
        if(describable as IJournalCategory != null)
        {
            grid.populatePanels((describable as IJournalCategory).getSubcategories());
        }
    }
}
