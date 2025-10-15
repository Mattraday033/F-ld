using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public interface IDragAndDropSource
{
    public string getDragAndDropPrefabName();
}

public class PartySpriteGridRow : GridRow, IPointerDownHandler, IDragAndDropSource, ICounter
{

    public TextMeshProUGUI healthText;
    public HealthBarManager healthBar;

    public static UnityEvent OnPartyMemberSelected = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!OverallUIManager.currentScreenManager.enableSpriteRowDragAndDrop())
        {
            return;
        }

        StartCoroutine(DragAndDropManager.waitForMouseRelease(this, descriptionPanel.getObjectBeingDescribed()));
    }

    public string getDragAndDropPrefabName()
    {
        return PrefabNames.partyMemberSpriteDragAndDrop;
    }

    public override void displayDescribable()
    {
        base.displayDescribable();

        OnPartyMemberSelected.Invoke();

        if (OverallUIManager.currentScreenManager.grids.Length <= 2)
        {
            return;
        }

        // OverallUIManager.currentScreenManager.descriptionPanelSlots[2].setPrimaryDescribable(Stats.convertIDescribableToStats(descriptionPanel.getObjectBeingDescribed()));
        OverallUIManager.currentScreenManager.populateObjectAttachedToSpriteRowButton(PartyManager.getPartyMember(descriptionPanel.getObjectBeingDescribed().getName()));
    }

    //ICounter
    private void OnEnable()
    {
        addListeners();

        StartCoroutine(updateHealthBarAfterDescribableIsSet());
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

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
    }

    public void updateCounter()
    {
        Stats stats = Stats.convertIDescribableToStats(descriptionPanel.getObjectBeingDescribed());

        healthText.text = stats.currentHealth + "/" + stats.getTotalHealth();

        healthBar.setTotalHealth(stats.getTotalHealth());
        healthBar.setMissingHealth(stats.getTotalHealth() - stats.currentHealth);
    }
    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(Stats.OnHealthChange);

        return listOfEvents;
    }

    private IEnumerator updateHealthBarAfterDescribableIsSet()
    {
        yield return new WaitForEndOfFrame();

        updateCounter();
    }

}


