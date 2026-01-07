using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharGenStatHover : SlotIconHover
{
    public static PrimaryStat currentPrimaryStat = PrimaryStat.Strength;

    public PrimaryStat primaryStat;
    public Transform popUpParent;

    public override void spawnHoverIcon()
    {
        MouseHoverManager.spawnCustomHover(this, transform, getHoverPrefabName());
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (primaryStat != PrimaryStat.None)
        {
            currentPrimaryStat = primaryStat;
            MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldSpawnHoverIcon));
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        MouseHoverManager.OnHoverPanelCreation.Invoke();
        MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldDestroyHoverIcon));
    }

    private string getHoverPrefabName()
    {
        return PrefabNames.characterGenerationStatDescriptionPanel;
    }

}
