using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIListenerGrid : MonoBehaviour, ICounter
{
    public ScrollableUIElement grid;

    private void Awake()
	{
        addListeners();
	}
	
    private void OnEnable()
    {
        updateCounter();
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    public virtual void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }

    }
    
    public virtual void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }

    }

    public virtual void updateCounter()
    {
        grid.populatePanels(Tab.getList(getDescribableList()));
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

    public abstract DescribableList getDescribableList();

}
