using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public ImageOutline imageOutline;

    public readonly static UnityEvent OnPartyMemberSelected = new UnityEvent();

    private void Awake()
    {
        imageOutline = new ImageOutline();
        imageOutline.setImage(descriptionPanel.iconPanel);
    }

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
        ScreenManager.currentPartyMember = Stats.convertIDescribableToStats(getObjectBeingDescribed());

        OnPartyMemberSelected.Invoke();
    }

    public override void spawnHoverIcon()
    {
        MouseHoverManager.spawnCustomHover(this, transform, PrefabNames.oocStatsHoverDescriptionPanelBuilder);
    }

    private void handlePortraitHover(string allyName, bool highlight)
    {
        if(!highlight)
        {
            imageOutline.removeOutline();
            return;
        }

        if(descriptionPanel.getObjectBeingDescribed().getName().Equals(allyName))
        {
            imageOutline.createOutline(ColorList.canBeInteractedWith);
        } else
        {
            imageOutline.removeOutline();
        }
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

        PartyGridSection.OnPortraitHover.AddListener(handlePortraitHover);
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
        
        PartyGridSection.OnPortraitHover.RemoveListener(handlePortraitHover);
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
        listOfEvents.Add(ScreenManager.OnScreenInteriorUpdate);

        return listOfEvents;
    }

    private IEnumerator updateHealthBarAfterDescribableIsSet()
    {
        yield return new WaitForEndOfFrame();

        updateCounter();
    }

}


