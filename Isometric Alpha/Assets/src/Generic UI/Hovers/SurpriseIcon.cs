using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SurpriseIcon : SlotIconHover
{

    private const string noOneSurprisedMessage = "Neither party is surprised. Combat will proceed normally.";
    private const string enemySurprisedMessage = "You have surprised the enemy! They will not get to attack during the surprise round.";
    private const string playerSurprisedMessage = "The enemy has surprised you! You will not get to attack during the surprise round.";

    public GameObject backgroundOne;
    public GameObject backgroundTwo;

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
        switch (CombatStateManager.whoIsSurprised)
        {
            case SurpriseState.PlayerSurprised:

                if(CombatStateManager.turnNumber > Constants.sizeOne)
                {
                    iconImage.color = ColorList.surpriseIconGrey;
                    backgroundOne.SetActive(false);
                    backgroundTwo.SetActive(false);
                } else
                {
                    iconImage.color = ColorList.surpriseIconRed;
                    backgroundOne.SetActive(true);
                    backgroundTwo.SetActive(true);
                }

                break;
            case SurpriseState.EnemySurprised:

                if(CombatStateManager.turnNumber > PartyStats.getPartySurpriseRounds())
                {
                    iconImage.color = ColorList.surpriseIconGrey;
                    backgroundOne.SetActive(false);
                    backgroundTwo.SetActive(false);
                } else
                {
                    iconImage.color = ColorList.surpriseIconGreen;
                    backgroundOne.SetActive(true);
                    backgroundTwo.SetActive(true);
                }

                break;
            default:
                iconImage.color = ColorList.surpriseIconGrey;
                backgroundOne.SetActive(false);
                backgroundTwo.SetActive(false);
                break;
        }

        setHoverMessage(getHoverMessage());
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