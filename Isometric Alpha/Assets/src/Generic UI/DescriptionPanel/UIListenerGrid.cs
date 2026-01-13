using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIListenerGrid : MonoBehaviour, ICounter
{
    [SerializeField]
    private DescribableList describableListType;
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
