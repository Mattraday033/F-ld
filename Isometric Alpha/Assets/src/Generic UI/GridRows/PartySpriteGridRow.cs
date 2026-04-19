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

public class PartySpriteGridRow : GridRow, IPointerDownHandler, IDragAndDropSource
{

    public ImageOutline imageOutline;

    public readonly static UnityEvent OnPartyMemberSelected = new UnityEvent();

    private void Awake()
    {
        imageOutline = new ImageOutline();
        imageOutline.setImage(descriptionPanel.iconPanel);
    }

    
    private void OnEnable()
    {
        PartyGridSection.OnPortraitHover.AddListener(handlePortraitHover);
    }

    private void OnDestroy()
    {
        PartyGridSection.OnPortraitHover.RemoveListener(handlePortraitHover);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!OverallUIManager.currentScreenManager.enableSpriteRowDragAndDrop())
        {
            return;
        }

        StartCoroutine(DragAndDropManager.waitForMouseRelease(this, descriptionPanel.getObjectBeingDescribed()));
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if(descriptionPanel != null)
        {
            NewPartyMemberManager.removePartyMember(descriptionPanel.getObjectBeingDescribed() as PartyMember);
        }
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

}


