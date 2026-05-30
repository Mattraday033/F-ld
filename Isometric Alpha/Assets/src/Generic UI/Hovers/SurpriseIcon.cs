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

    public GameObject skullIcon;
    public GameObject partyIcon;
    public GameObject equalsIcon;

    public override void Awake()
    {
        CombatStateManager.OnNewTurn.AddListener(setSurpriseIcon);
    }

    private void Start()
    {
        setSurpriseIcon();
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
                    setToNoOneSurprised();
                } else
                {
                    setToPlayerSurprised();
                }

                break;
            case SurpriseState.EnemySurprised:

                if(CombatStateManager.turnNumber > PartyStats.getPartySurpriseRounds())
                {
                    setToNoOneSurprised();
                } else
                {
                    setToEnemySurprised();
                }

                break;
            default:
                setToNoOneSurprised();
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

    private void setToPlayerSurprised()
    {
        skullIcon.SetActive(false);
        partyIcon.SetActive(true);
        equalsIcon.SetActive(false);
    }

    private void setToEnemySurprised()
    {
        skullIcon.SetActive(true);
        partyIcon.SetActive(false);        
        equalsIcon.SetActive(false);
    }

    private void setToNoOneSurprised()
    {
        skullIcon.SetActive(false);
        partyIcon.SetActive(false);
        equalsIcon.SetActive(true);
    }

    public override void describeSelfFull(DescriptionPanel panel)
    {
        panel.setObjectBeingDescribed(this);

        switch (CombatStateManager.whoIsSurprised)
        {
            case SurpriseState.PlayerSurprised:

                if(CombatStateManager.turnNumber > Constants.sizeOne)
                {
                    DescriptionPanel.setText(panel.nameText, "No Surprise Round");
                    DescriptionPanel.setText(panel.useDescriptionText, noOneSurprisedMessage);
                } else
                {
                    DescriptionPanel.setText(panel.nameText, "Enemy Surprise Round");
                    DescriptionPanel.setText(panel.useDescriptionText, playerSurprisedMessage);
                }

                break;
            case SurpriseState.EnemySurprised:

                if(CombatStateManager.turnNumber > PartyStats.getPartySurpriseRounds())
                {
                    DescriptionPanel.setText(panel.nameText, "No Surprise Round");
                    DescriptionPanel.setText(panel.useDescriptionText, noOneSurprisedMessage);
                } else
                {
                    DescriptionPanel.setText(panel.nameText, "Party Surprise Round");
                    DescriptionPanel.setText(panel.useDescriptionText, enemySurprisedMessage);
                }

                break;
            default:
                DescriptionPanel.setText(panel.nameText, "No Surprise Round");
                DescriptionPanel.setText(panel.useDescriptionText, noOneSurprisedMessage);
                break;
        }
    }

}