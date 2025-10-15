using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartyGridSection : GridRow
{

    public override void OnPointerEnter(PointerEventData eventData)
    {
        FormationHandler formationHandler = OverallUIManager.currentScreenManager as FormationHandler;

        if (formationHandler != null)
        {
            formationHandler.primaryStatSlot.setTempDescribable(descriptionPanel.getObjectBeingDescribed() as AllyStats);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        FormationHandler formationHandler = OverallUIManager.currentScreenManager as FormationHandler;

        if (formationHandler != null)
        {
            formationHandler.primaryStatSlot.revertToPrimaryDescribable();
        }
    }


}
