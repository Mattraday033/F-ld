using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ExuberanceTracker : MonoBehaviour, ICounter
{
    public TextMeshProUGUI redKnifeText;
    public TextMeshProUGUI blueShieldText;
    public TextMeshProUGUI yellowThornText;
    public TextMeshProUGUI greenLeafText;

    private void Awake()
    {
        if (!PartyStats.partyHasAccessToExuberances())
        {
            gameObject.SetActive(false);
        }
        else
        {
            Exuberances.setExuberancesToStartingAmount();
        }
    }

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
        redKnifeText.text = Exuberances.getRedKnife().ToString();
        blueShieldText.text = Exuberances.getBlueShield().ToString();
        yellowThornText.text = Exuberances.getYellowThorn().ToString();
        greenLeafText.text = Exuberances.getGreenLeaf().ToString();
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(CombatStateManager.OnNewTurn);
        listOfEvents.Add(Exuberances.OnExuberanceChance);

        return listOfEvents;
    }
}
