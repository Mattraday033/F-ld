using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UIDescriptionPanelSlot : DescriptionPanelSlot, ICounter
{
    [SerializeField]
    private bool listeningForGridRows = false;

    //the screen this slot sits in, if it sits in one
    private ScreenManager owningScreen;

    //what addListeners subscribed to, kept so removeListeners takes off the same ones whichever screen is current by then
    private List<UnityEvent> subscribedEvents = new List<UnityEvent>();

    protected virtual void Awake()
    {
        owningScreen = GetComponentInParent<ScreenManager>(true);

        addListeners();
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    //a slot on a hidden screen is filled when its screen is next shown
    protected bool hiddenWithItsScreen()
    {
        return ScreenManager.isHidden(owningScreen);
    }

    //while its screen is being shown, the description waits for the party member to be settled, and is built once
    protected bool putOffWhileItsScreenIsShown()
    {
        return ScreenUpdateBatch.putOff(owningScreen, updateCounter);
    }

    public void addListeners()
    {
        subscribedEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in subscribedEvents)
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
        foreach(UnityEvent unityEvent in subscribedEvents)
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
        if (hiddenWithItsScreen())
        {
            return;
        }

        if (putOffWhileItsScreenIsShown())
        {
            return;
        }

        if(OverallUIManager.currentScreenManager != null && !listeningForGridRows)
        {
            setPrimaryDescribable(ScreenManager.currentPartyMember);
        }
    }

    public virtual void updateCounter(IDescribable describable)
    {
        if (hiddenWithItsScreen())
        {
            return;
        }

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