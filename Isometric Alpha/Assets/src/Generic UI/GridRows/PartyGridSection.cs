using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PartyGridSection : GridRow
{
    public readonly static UnityEvent<string, bool> OnPortraitHover = new UnityEvent<string, bool>();

    public GameObject namePanel;
    public TextMeshProUGUI nameText;


    public override void OnPointerEnter(PointerEventData eventData)
    {
        FormationHandler formationHandler = OverallUIManager.currentScreenManager as FormationHandler;

        if (formationHandler != null)
        {
            formationHandler.primaryStatSlot.setTempDescribable(descriptionPanel.getObjectBeingDescribed() as AllyStats);

            if(namePanel != null && nameText != null)
            {
                namePanel.SetActive(true);
                nameText.text = descriptionPanel.getObjectBeingDescribed().getName().Replace(PartyManager.playerMarker, "");

                OnPortraitHover.Invoke(descriptionPanel.getObjectBeingDescribed().getName(), true);
            }
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        FormationHandler formationHandler = OverallUIManager.currentScreenManager as FormationHandler;

        if (formationHandler != null)
        {
            formationHandler.primaryStatSlot.revertToPrimaryDescribable();
            formationHandler.primaryStatSlot.setPrimaryDescribable(State.formation);
        }

        if(namePanel != null && nameText != null)
        {
            namePanel.SetActive(false);
            nameText.text = "";
            OnPortraitHover.Invoke("", false);
        }
    }

}
