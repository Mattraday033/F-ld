using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WinConditionUI : SlotIconHover
{

    public Collider2D boxCollider;

    private WinCondition winCon;

    public override void Awake()
    {
        if(State.enemyPackInfo == null || State.enemyPackInfo.winCon == null)
        {
            return;
        }

        winCon = State.enemyPackInfo.winCon;

        setHoverMessage(winCon.getName(), winCon.getWinConDescription());
        iconImage.sprite = winCon.getSprite();

        CombatStateManager.OnActivityChangeToTutorial.AddListener(enableBoxCollider);
        CombatStateManager.OnActivityChangeFromTutorial.AddListener(disableBoxCollider);
    }

    private void OnDestroy()
    {
        CombatStateManager.OnActivityChangeToTutorial.RemoveListener(enableBoxCollider);
        CombatStateManager.OnActivityChangeFromTutorial.RemoveListener(disableBoxCollider);
    }

    private void enableBoxCollider()
    {
        if(TutorialSequence.currentTutorialSequence.tutorialSeenFlagName.Equals(TutorialSequenceList.winConUITutorialSeenFlag))
        {
            EventSystem.current.SetSelectedGameObject(null);
            boxCollider.enabled = true;   
        }
    }

    private void disableBoxCollider()
    {
        boxCollider.enabled = false;     
    }

    public override void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.nameText, winCon.getName());
        DescriptionPanel.setText(panel.useDescriptionText, winCon.getWinConDescription());
    }

}
