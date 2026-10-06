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

    //OnEnable adds them every time, so without this a counter that is switched off and on again would be listening twice
    private void OnDisable()
    {
        removeListeners();
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
        counterText.text = OverallUIManager.getCurrentActionArray().getAmountOfWeaponCombatActions() + "/" + OverallUIManager.getCurrentPartyMember().getWeaponSlots();
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(PartySpriteGridRow.OnPartyMemberSelected);
        listOfEvents.Add(CombatActionArray.OnCombatActionArrayChange);
        listOfEvents.Add(ScreenManager.OnScreenInteriorUpdate);

        return listOfEvents;
    }

}
