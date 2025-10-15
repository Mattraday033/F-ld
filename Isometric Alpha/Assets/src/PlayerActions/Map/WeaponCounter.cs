using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public interface ICounter
{
    public void addListeners();
    public void removeListeners();

    public void updateCounter();
    public List<UnityEvent> getUpdateEvents();
}

public class WeaponCounter : MonoBehaviour, ICounter
{
    public TextMeshProUGUI counterText;

    private void OnEnable()
    {
        updateCounter();
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
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach(UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
    }

    public void updateCounter()
    {
        counterText.text = OverallUIManager.getCurrentActionArray().getAmountOfWeaponCombatActions() + "/" + PartyManager.getPlayerStats().getWeaponSlots();
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(CombatActionArray.OnCombatActionArrayChange);

        return listOfEvents;
    }

}
