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

    private const float outlinePulseDuration = 2f;
    private Coroutine outlinePulseCoroutine;

    public readonly static UnityEvent OnPartyMemberSelected = new UnityEvent();

    private void Awake()
    {
        imageOutline = new ImageOutline();
        imageOutline.setImage(descriptionPanel.iconPanel);
    }

    
    private void OnEnable()
    {
        PartyGridSection.OnPortraitHover.AddListener(handlePortraitHover);
        OnPartyMemberSelected.AddListener(removeOutlineOnPartyMemberSelection);
    }

    private void OnDestroy()
    {
        PartyGridSection.OnPortraitHover.RemoveListener(handlePortraitHover);
        OnPartyMemberSelected.RemoveListener(removeOutlineOnPartyMemberSelection);
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

        if(imageOutline != null)
        {
            imageOutline.createOutline(ColorList.canBeInteractedWith);
        }
    }

    public override void spawnHoverIcon()
    {
        MouseHoverManager.spawnCustomHover(this, transform, PrefabNames.oocStatsHoverDescriptionPanelBuilder);
    }

    private void handlePortraitHover(string allyName, bool highlight)
    {
        if(!highlight)
        {
            stopOutlinePulse();
            imageOutline.removeOutline();

            if(ScreenManager.currentPartyMember != null && 
                ScreenManager.currentPartyMember.getName().Equals(descriptionPanel.getObjectBeingDescribed().getName()))
            {
                imageOutline.createOutline(ColorList.canBeInteractedWith);
            } 
            return;
        }

        if(descriptionPanel.getObjectBeingDescribed().getName().Equals(allyName))
        {
            imageOutline.createOutline(ColorList.canBeInteractedWith);
            startOutlinePulse();
        } 
        // else 
        // {
        //     stopOutlinePulse();
        //     imageOutline.removeOutline();
        // }
    }

    private void startOutlinePulse()
    {
        stopOutlinePulse();
        outlinePulseCoroutine = StartCoroutine(pulseOutline());
    }

    private void stopOutlinePulse()
    {
        if(outlinePulseCoroutine != null)
        {
            StopCoroutine(outlinePulseCoroutine);
            outlinePulseCoroutine = null;
        }
    }

    private IEnumerator pulseOutline()
    {
        float halfDuration = outlinePulseDuration / 2f;

        imageOutline.setOpacity(1f);

        while(true)
        {
            float elapsed = 0f;
            while(elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                imageOutline.setOpacity(Mathf.Lerp(1f, 0f, elapsed / halfDuration));
                yield return null;
            }

            elapsed = 0f;
            while(elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                imageOutline.setOpacity(Mathf.Lerp(0f, 1f, elapsed / halfDuration));
                yield return null;
            }
        }
    }

    private void removeOutlineOnPartyMemberSelection()
    {
        imageOutline.removeOutline();
    }

}


