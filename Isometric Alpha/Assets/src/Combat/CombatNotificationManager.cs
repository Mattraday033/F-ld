using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatNotificationManager : MonoBehaviour
{

    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;

    private static bool showingSurprisedMessage = false;

    private void OnEnable()
    {
        // CombatStateManager.OnTurnChangeToPlayer.AddListener(OnTurnChangeToPlayer);
        CombatStateManager.OnTurnChangeToResolving.AddListener(OnTurnChangeToResolving);
        // CombatStateManager.OnTurnChangeToWon.AddListener(OnTurnChangeToWon);
        // CombatStateManager.OnTurnChangeToLost.AddListener(OnTurnChangeToLost);

        CombatStateManager.OnActivityChangeToWaiting.AddListener(OnActivityChangeToWaiting);
        CombatStateManager.OnActivityChangeToChoosingActor.AddListener(OnActivityChangeToChoosingActor);
        CombatStateManager.OnActivityChangeToChoosingAbility.AddListener(OnActivityChangeToChoosingAbility);
        CombatStateManager.OnActivityChangeToChoosingLocation.AddListener(OnActivityChangeToChoosingLocation);
        CombatStateManager.OnActivityChangeToChoosingTertiary.AddListener(OnActivityChangeToChoosingTertiary);
        CombatStateManager.OnActivityChangeToRepositioning.AddListener(OnActivityChangeToRepositioning);
        CombatStateManager.OnActivityChangeToTutorial.AddListener(OnActivityChangeToTutorial);
        CombatStateManager.OnActivityChangeToRetreating.AddListener(OnActivityChangeToRetreating);
        CombatStateManager.OnActivityChangeToInEscapeMenu.AddListener(OnActivityChangeToInEscapeMenu);
        CombatStateManager.OnActivityChangeToFinished.AddListener(OnActivityChangeToFinished);
        CombatStateManager.OnActivityChangeToResolveTurnWarning.AddListener(OnActivityChangeToResolveTurnWarning);

        CombatStateManager.OnNewTurn.AddListener(OnNewTurn);
    }

    private void OnDisable()
    {
        // CombatStateManager.OnTurnChangeToPlayer.RemoveListener(OnTurnChangeToPlayer);
        CombatStateManager.OnTurnChangeToResolving.RemoveListener(OnTurnChangeToResolving);
        // CombatStateManager.OnTurnChangeToWon.RemoveListener(OnTurnChangeToWon);
        // CombatStateManager.OnTurnChangeToLost.RemoveListener(OnTurnChangeToLost);

        CombatStateManager.OnActivityChangeToWaiting.RemoveListener(OnActivityChangeToWaiting);
        CombatStateManager.OnActivityChangeToChoosingActor.RemoveListener(OnActivityChangeToChoosingActor);
        CombatStateManager.OnActivityChangeToChoosingAbility.RemoveListener(OnActivityChangeToChoosingAbility);
        CombatStateManager.OnActivityChangeToChoosingLocation.RemoveListener(OnActivityChangeToChoosingLocation);
        CombatStateManager.OnActivityChangeToChoosingTertiary.RemoveListener(OnActivityChangeToChoosingTertiary);
        CombatStateManager.OnActivityChangeToRepositioning.RemoveListener(OnActivityChangeToRepositioning);
        CombatStateManager.OnActivityChangeToTutorial.RemoveListener(OnActivityChangeToTutorial);
        CombatStateManager.OnActivityChangeToRetreating.RemoveListener(OnActivityChangeToRetreating);
        CombatStateManager.OnActivityChangeToInEscapeMenu.RemoveListener(OnActivityChangeToInEscapeMenu);
        CombatStateManager.OnActivityChangeToFinished.RemoveListener(OnActivityChangeToFinished);
        CombatStateManager.OnActivityChangeToResolveTurnWarning.RemoveListener(OnActivityChangeToResolveTurnWarning);

        CombatStateManager.OnNewTurn.RemoveListener(OnNewTurn);
    }

    #region Turn states

    private void OnTurnChangeToPlayer()
    {

    }

    private void OnTurnChangeToResolving()
    {
        if(CombatStateManager.turnNumber == Constants.sizeOne && 
            CombatStateManager.whoIsSurprised == SurpriseState.PlayerSurprised)
        {
            return;
        }

        notificationPanel.SetActive(false);
    }

    private void OnTurnChangeToWon()
    {

    }

    private void OnTurnChangeToLost()
    {

    }

    #endregion

    #region Activity states

    private void OnActivityChangeToWaiting()
    {        
        if(CombatStateManager.turnNumber == Constants.sizeOne && 
            CombatStateManager.whoIsSurprised == SurpriseState.PlayerSurprised)
        {
            return;
        }

        notificationPanel.SetActive(false);
    }

    private void OnActivityChangeToChoosingActor()
    {
        if(CombatStateManager.turnNumber == Constants.sizeOne && 
            CombatStateManager.whoIsSurprised == SurpriseState.EnemySurprised)
        {
            setText("You have surprised the enemy! " + chooseActorMessage);
        } else
        {
            setText(chooseActorMessage);
        }
    }

    private void OnActivityChangeToChoosingAbility()
    {
        setText(chooseAbilityMessage);
    }

    private void OnActivityChangeToChoosingLocation()
    {
        setText(chooseLocationMessage);
    }

    private void OnActivityChangeToChoosingTertiary()
    {
        setText(chooseTertiaryLocationMessage);
    }

    private void OnActivityChangeToRepositioning()
    {
        notificationPanel.SetActive(false);
    }

    private void OnActivityChangeToTutorial()
    {
        notificationPanel.SetActive(false);
    }

    private void OnActivityChangeToRetreating()
    {
        notificationPanel.SetActive(false);
    }

    private void OnActivityChangeToInEscapeMenu()
    {
        // notificationPanel.SetActive(false);
    }

    private void OnActivityChangeToFinished()
    {
        setText(finishedMessage);
    }

    private void OnActivityChangeToResolveTurnWarning()
    {
        notificationPanel.SetActive(false);
    }

    #endregion

    private const string chooseActorMessage = "Select Party Member to act.";
    private const string chooseAbilityMessage = "Select Action to use.";
    private const string chooseLocationMessage = "Select Target(s).";
    private const string chooseTertiaryLocationMessage = "Select Destination.";
    private const string finishedMessage = "Resolve Turn.";

    public void OnNewTurn()
    {
        if(CombatStateManager.turnNumber == Constants.sizeOne)
        {
            switch(CombatStateManager.whoIsSurprised)
            {
                case SurpriseState.PlayerSurprised:
                    setText("You have been surprised!");
                    break;
            }
        }
    }

    private void setText(string text)
    {
        notificationPanel.SetActive(true);
        notificationText.text = text;
    }

}
