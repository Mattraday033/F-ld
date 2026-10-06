using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIListenerGrid : MonoBehaviour, ICounter
{
    [SerializeField]
    private DescribableList describableListType;
    public ScrollableUIElement grid;

    //the screen this grid sits in, if it sits in one. The shop's grid and the party strip have none
    private ScreenManager owningScreen;

    //what addListeners subscribed to, kept so removeListeners takes off the same ones whichever screen is current by then
    private List<UnityEvent> subscribedEvents = new List<UnityEvent>();

    private void Awake()
	{
        owningScreen = GetComponentInParent<ScreenManager>(true);

        addListeners();
	}

    //a grid on a hidden screen is filled when its screen is next shown
    protected bool hiddenWithItsScreen()
    {
        return ScreenManager.isHidden(owningScreen);
    }
	
    private void OnEnable()
    {
        // updateCounter();
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    public virtual void addListeners()
    {
        subscribedEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in subscribedEvents)
        {
            unityEvent.AddListener(updateCounter);
        }

    }

    public virtual void removeListeners()
    {
        foreach (UnityEvent unityEvent in subscribedEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }

    }

    public virtual void updateCounter()
    {
        if (hiddenWithItsScreen())
        {
            return;
        }

        //while its screen is being shown, the fill waits for the tab and the party member to be settled, and is done once
        if (ScreenUpdateBatch.putOff(owningScreen, updateCounter))
        {
            return;
        }

        grid.populatePanels(getDescribableList());
    }

    public virtual void updateCounter(IDescribable describable)
    {
        //empty on purpose
    }

    public virtual List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        if(AbilityGridSideTab.getCurrentDictKey() != null)
        {
            listOfEvents.AddRange(AbilityGridSideTab.getCurrentDictKey().getUpdateEvents());
        }

        return listOfEvents;
    }

    public virtual DescribableList getDescribableListType()
    {
        return describableListType;
    }

    public virtual IEnumerable<IDescribable> getDescribableList()
    {
        return Tab.getList(getDescribableListType());
    }

}
