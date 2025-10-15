using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SurpriseIcon : SlotIconHover
{

    private const string noOneSurprisedMessage = "Neither party is surprise. Combat will proceed normally.";
    private const string enemySurprisedMessage = "You have surprised the enemy! They will not get to attack during the surprise round.";
    private const string playerSurprisedMessage = "The enemy has surprised you! You will not get to attack during the surprise round.";

    public override void Awake()
    {
        CombatStateManager.OnNewTurn.AddListener(setSurpriseIcon);
    }

    private void OnDestroy()
    {
        CombatStateManager.OnNewTurn.RemoveListener(setSurpriseIcon);
    }

    private void setSurpriseIcon()
    {
        // outlineImage.color = getSurpriseColor();
        iconImage.color = getSurpriseColor();

        setHoverMessage(getHoverMessage());
    }

    private Color getSurpriseColor()
    {
        switch (CombatStateManager.whoIsSurprised)
        {
            case SurpriseState.PlayerSurprised:
                return Color.red;
            case SurpriseState.EnemySurprised:
                return Color.green;
            default:
                return new Color32(155, 155, 155, 255);
        }
    }

    private string getHoverMessage()
    {
        switch (CombatStateManager.whoIsSurprised)
        {
            case SurpriseState.PlayerSurprised:
                return playerSurprisedMessage;
            case SurpriseState.EnemySurprised:
                return enemySurprisedMessage;
            default:
                return noOneSurprisedMessage;
        }
    }

}