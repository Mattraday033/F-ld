using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum OOCStatusEffect { None, Intimidate, Cunning, Retreat }

public class StatusEffectDisplay : SlotIconHover
{

    public EnemyMovement parentMovement;
    public Canvas canvas;

    public TextMeshProUGUI roundCounter;

    public override void Awake()
    {
        base.Awake();

        canvas.worldCamera = Camera.main;
        MovementManager.OnMoveFinished.AddListener(updateDisplay);
        SkillManager.OnSkillUse.AddListener(updateDisplay);
        updateDisplay();
    }

    private void OnDestroy()
    {
        MovementManager.OnMoveFinished.RemoveListener(updateDisplay);
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

        if(getStatusEffect() == OOCStatusEffect.None)
        {
            return;
        }

        int roundCount = getRoundCount();
        
        if(roundCount <= 0)
        {
            return;
        }

        roundCounter.text = roundCount.ToString();
    }
    
    private int getRoundCount()
    {
        switch(getStatusEffect())
        {
            case OOCStatusEffect.Intimidate:
                return parentMovement.intimidateCounter;
            case OOCStatusEffect.Cunning:
                return parentMovement.cunningStunCounter;
            case OOCStatusEffect.Retreat:
                return parentMovement.retreatStunnedCounter;
            default:
                return 0;
        }
    }

    private void setDisplay()
    {
        gameObject.SetActive(getStatusEffect() != OOCStatusEffect.None);

        switch(getStatusEffect())
        {
            case OOCStatusEffect.Intimidate:
                iconImage.sprite = Resources.Load<Sprite>(IconList.intimidateIconName);
                hoverMessageKey = IconList.intimidateIconName;
                break;
            case OOCStatusEffect.Cunning:
                iconImage.sprite = Resources.Load<Sprite>(IconList.cunningIconName);
                hoverMessageKey = IconList.cunningIconName;
                break;
            case OOCStatusEffect.Retreat:
                iconImage.sprite = Resources.Load<Sprite>(IconList.retreatChanceIconName);
                hoverMessageKey = IconList.retreatChanceIconName;
                break;
            default:
                break;
        }
    }

    private OOCStatusEffect getStatusEffect()
    {
        if(parentMovement.intimidateCounter <= 0 && 
            parentMovement.cunningStunCounter <= 0 && 
            parentMovement.retreatStunnedCounter <= 0)
        {
            return OOCStatusEffect.None;
        }

        if(parentMovement.intimidateCounter >= parentMovement.cunningStunCounter && 
            parentMovement.intimidateCounter >= parentMovement.retreatStunnedCounter)
        {
            return OOCStatusEffect.Intimidate;
        }

        if(parentMovement.cunningStunCounter >= parentMovement.intimidateCounter && 
            parentMovement.cunningStunCounter >= parentMovement.retreatStunnedCounter)
        {
            return OOCStatusEffect.Cunning;
        }

        if(parentMovement.retreatStunnedCounter >= parentMovement.cunningStunCounter && 
            parentMovement.retreatStunnedCounter >= parentMovement.intimidateCounter)
        {
            return OOCStatusEffect.Retreat;
        }

        return OOCStatusEffect.None;
    }

}
