using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ExuberanceTracker : MonoBehaviour, ICounter
{
    public readonly static UnityEvent<List<ActionCostType>> ActionCostCannotBePaid = new UnityEvent<List<ActionCostType>>();

    private const float timeToWaitRed = .5f;
    private const float timeToWaitFade = 1f;

    public Image[] borders;

    public TextMeshProUGUI redKnifeText;
    public TextMeshProUGUI blueShieldText;
    public TextMeshProUGUI yellowThornText;
    public TextMeshProUGUI greenLeafText;

    private Coroutine highlightCoroutine;

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

    private void startHighlight(List<ActionCostType> actionCostTypes)
    {
        if(highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
        }

        highlightCoroutine = StartCoroutine(highlightIconBorders(new bool[]{ actionCostTypes.Contains(ActionCostType.RedKnife),
                                                                             actionCostTypes.Contains(ActionCostType.BlueShield),
                                                                             actionCostTypes.Contains(ActionCostType.YellowThorn),
                                                                             actionCostTypes.Contains(ActionCostType.GreenLeaf) }));
    }

    private IEnumerator highlightIconBorders(bool[] highlights)
    {
        float timeWaited = 0f;

        for(int index = 0; index < borders.Length && index < highlights.Length; index++)
        {
            if(highlights[index])
            {
                borders[index].color = Color.red;
            } else
            {
                borders[index].color = ColorList.grey25;
            }
        }

        while(timeWaited < timeToWaitRed)
        {
            yield return null;
            timeWaited += Time.deltaTime;
        }

        timeWaited = 0f;

        while(timeWaited < timeToWaitFade)
        {
            yield return null;
            timeWaited += Time.deltaTime;

            for(int index = 0; index < borders.Length && index < highlights.Length; index++)
            {
                if(highlights[index])
                {
                    borders[index].color = new Color(   Mathf.Lerp(Color.red.r, ColorList.grey25.r, timeWaited/timeToWaitFade),
                                                        Mathf.Lerp(Color.red.g, ColorList.grey25.g, timeWaited/timeToWaitFade),
                                                        Mathf.Lerp(Color.red.b, ColorList.grey25.b, timeWaited/timeToWaitFade),
                                                        1f);
                } 
            }
        }

        yield return null;

        for(int index = 0; index < borders.Length && index < highlights.Length; index++)
        {
            if(highlights[index])
            {
                borders[index].color = ColorList.grey25;
            } 
        }

        highlightCoroutine = null;
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

        ActionCostCannotBePaid.AddListener(startHighlight);
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach(UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }

        ActionCostCannotBePaid.RemoveListener(startHighlight);
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
        listOfEvents.Add(Exuberances.OnExuberanceChange);
        listOfEvents.Add(ScreenManager.OnScreenInteriorUpdate);

        return listOfEvents;
    }
}
