using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum OverHeadIconType { Intimidate, Cunning, Retreat, Shopkeeper, NameTag }

public interface IOverHeadIconSource
{    
    
    public int cunningStunCounter
    {
        get;
    }

    public int intimidateCounter
    {
        get;
    }
    public int retreatStunCounter
    {
        get;
    }

    public Color getRevealColor();
    public void onReveal(bool toggleReveal);
}

public class OverHeadIcon : SlotIconHover
{

    public OverHeadIconType type;

    public IOverHeadIconSource source;
    public OverHeadIconManager iconManager;
    public Canvas canvas;

    public TextMeshProUGUI roundCounter;

    public Transform descriptionPanelParent;
    public bool hovered;

    public override void Awake()
    {
        base.Awake();

        MovementManager.OnMoveFinished.AddListener(updateDisplay);
        SkillManager.OnSkillUse.AddListener(updateDisplay);

        StartCoroutine(waitFourFramesThenUpdateDisplay());
    }

    private IEnumerator waitFourFramesThenUpdateDisplay()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        canvas.worldCamera = Camera.main;
        updateDisplay();
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    private void removeListeners()
    {
        MovementManager.OnMoveFinished.RemoveListener(updateDisplay);
        SkillManager.OnSkillUse.RemoveListener(updateDisplay);
    }

    public void updateDisplay()
    {
        updateDisplay(MovementManager.playerSpriteIndex);
    }

    public void updateDisplay(int movementIndex)
    {
        if(movementIndex != MovementManager.playerSpriteIndex)
        {
            return;
        }

        setDisplay();

        int roundCount = getRoundCount();
        
        if(roundCount <= 0)
        {
            switch(type)
            {
                case OverHeadIconType.Intimidate:
                case OverHeadIconType.Cunning:
                case OverHeadIconType.Retreat:
                    DestroyImmediate(gameObject);
                    iconManager.removeAllDestroyedIcons();
                    return;
                case OverHeadIconType.Shopkeeper:
                    roundCounter.enabled = false;
                    return;
            }
        }

        roundCounter.text = roundCount.ToString();
    }
    
    private int getRoundCount()
    {
        switch(type)
        {
            case OverHeadIconType.Intimidate:
                return source.intimidateCounter;
            case OverHeadIconType.Cunning:
                return source.cunningStunCounter;
            case OverHeadIconType.Retreat:
                return source.retreatStunCounter;
            default:
                return 0;
        }
    }

    public void setDisplay(OverHeadIconType type)
    {
        this.type = type;

        switch(this.type)
        {
            case OverHeadIconType.Shopkeeper:
                removeListeners();
                break;
            default:
                break;
        }

        setDisplay();
    }

    private void setDisplay()
    {
        switch(type)
        {
            case OverHeadIconType.Intimidate:
                iconImage.sprite = Helpers.loadSpriteFromResources(IconList.intimidateIconName);
                hoverMessageKey = HoverMessageList.intimidatedKey;
                break;
            case OverHeadIconType.Cunning:
                iconImage.sprite = Helpers.loadSpriteFromResources(IconList.cunningIconName);
                hoverMessageKey = HoverMessageList.distractedKey;
                break;
            case OverHeadIconType.Retreat:
                iconImage.sprite = Helpers.loadSpriteFromResources(IconList.retreatChanceIconName);
                hoverMessageKey = HoverMessageList.evadedKey;
                break;
            case OverHeadIconType.Shopkeeper:
                iconImage.sprite = Helpers.loadSpriteFromResources(IconList.shopIcon);
                hoverMessageKey = HoverMessageList.shopkeeperIconKey;
                break;
            default:
                return;
        }

        setHoverMessage(HoverMessageList.getMessage(hoverMessageKey));
    }

    public override void spawnHoverIcon()
    {
        MouseHoverManager.spawnHoverIcon(this, descriptionPanelParent, scale: .009f, alwaysTop: true);
    }

    // private void setOutline

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(ignoreHover || (eventData != null && eventData.used))
        {
            return;
        }

        hovered = true;

        base.OnPointerEnter(eventData);

        if(source != null)
        {
            source.onReveal(true);
            outlineImage.color = source.getRevealColor();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if(ignoreHover || eventData == null)
        {
            return;
        }

        hovered = false;
        
        MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldDestroyHoverIcon));

        if(source != null)
        {
            source.onReveal(false);
            outlineImage.color = Color.black;
        }
    }

}
