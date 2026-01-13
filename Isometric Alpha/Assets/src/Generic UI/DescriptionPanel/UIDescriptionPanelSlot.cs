using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UIDescriptionPanelSlot : DescriptionPanelSlot, ICounter
{
    [SerializeField]
    private bool listeningForGridRows = false;

    private void Awake()
    {
        addListeners();
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    public void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }

        if(listeningForGridRows)
        {
            GridRow.OnDescribableToDisplay.AddListener(updateCounter);
        }
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach(UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }

        if(listeningForGridRows)
        {
            GridRow.OnDescribableToDisplay.RemoveListener(updateCounter);
        }
    }

    public virtual void updateCounter()
    {
        if(OverallUIManager.currentScreenManager != null && !listeningForGridRows)
        {
            setPrimaryDescribable(ScreenManager.currentPartyMember);
        }
    }

    public virtual void updateCounter(IDescribable describable)
    {
        if(describable == null)
        {
            removePrimaryDescribable();
        } else
        {
            setPrimaryDescribable(describable);
        }
    }

    public virtual List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        if(OverallUIManager.currentScreenManager != null)
        {
            listOfEvents.AddRange(OverallUIManager.currentScreenManager.getUpdateEvents());
        }

        return listOfEvents;
    }
}