using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum OOCActivity {
							walking 	 = 1, 
							inDialogue 	 = 2, 
							inUI 		 = 3, 
							inMap 		 = 4, 
							cunning 	 = 5, 
							observing 	 = 6,
							intimidating = 7,
							inChestUI 	 = 8, 
							inBookUI 	 = 9, 
							inShopUI 	 = 10,
                            inDialoguePopUp = 11, 
                            inLevelUpPopUp = 12, 
                            inTutorialPopUp = 13,
                            inTutorialSequence = 14
						};

public static class PlayerOOCStateManager
{
    public static OOCActivity currentActivity { get; private set; }
    public static OOCActivity previousActivity { get; private set; }

    public static UnityEvent OnStateChangeToWalking = new UnityEvent();
    public static UnityEvent OnStateChangeFromWalking = new UnityEvent();

    
    public static UnityEvent OnStateChangeToInDialogue = new UnityEvent();

    public static UnityEvent OnStateChangeToInUI = new UnityEvent();
    public static UnityEvent OnStateChangeFromInUI = new UnityEvent();

    public static UnityEvent OnStateChangeToInMap = new UnityEvent();
    public static UnityEvent OnStateChangeToSkill = new UnityEvent();
    public static UnityEvent OnStateChangeToInChestUI = new UnityEvent();
    public static UnityEvent OnStateChangeToInBookUI = new UnityEvent();
    public static UnityEvent OnStateChangeToInShopUI = new UnityEvent();
    public static UnityEvent OnStateChangeToInDialoguePopUp = new UnityEvent();
    public static UnityEvent OnStateChangeToInLevelUpPopUp = new UnityEvent();
    public static UnityEvent OnStateChangeToInTutorialSequence = new UnityEvent();

    public static UnityEvent OnLeavingTutorialSequenceState = new UnityEvent();

    public static void returnToPreviousActivity()
    {
        if (previousActivity == OOCActivity.inLevelUpPopUp)
        {
            setCurrentActivity(OOCActivity.walking);
        }
        else
        {
            // Debug.LogError("previousActivity = " + previousActivity.ToString());
            setCurrentActivity(previousActivity);
        }


    }

    public static void setCurrentActivity(OOCActivity newActivity)
    {
        setCurrentActivity(newActivity, false);
    }

    public static void setCurrentActivity(OOCActivity newActivity, bool tutorialSequenceCheckBypass)
    {
        if ((!tutorialSequenceCheckBypass && (currentActivity == newActivity ||
            (currentActivity == OOCActivity.inTutorialSequence && newActivity != OOCActivity.walking)))
            || CombatStateManager.inCombat)
        {
            return;
        }

        if (newActivity < OOCActivity.walking)
        {
            newActivity = OOCActivity.walking;
        }

        previousActivity = currentActivity;

        // Debug.LogError("New previousActivity = " + previousActivity.ToString());
        currentActivity = newActivity;

        // Debug.LogError("New currentActivity = " + currentActivity.ToString());

        switch (previousActivity)
        {
            case OOCActivity.walking:
                OnStateChangeFromWalking.Invoke();
                break;
            case OOCActivity.inDialogue:
                OnStateChangeFromInUI.Invoke();
                break;
            case OOCActivity.inUI:
                break;
            case OOCActivity.inMap:
                break;
            case OOCActivity.intimidating:
            case OOCActivity.cunning:
            case OOCActivity.observing:
                break;
            case OOCActivity.inChestUI:
                break;
            case OOCActivity.inBookUI:
                break;
            case OOCActivity.inShopUI:
                break;
            case OOCActivity.inDialoguePopUp:
                break;
            case OOCActivity.inLevelUpPopUp:
                break;
            case OOCActivity.inTutorialPopUp:
                break;
            case OOCActivity.inTutorialSequence:
                OnLeavingTutorialSequenceState.Invoke();
                break;
        }

        switch (currentActivity)
        {
            case OOCActivity.walking:
                OnStateChangeToWalking.Invoke();
                //EscapeStack.escapeAll();
                OOCUIManager.updateOOCUI();
                PartyMemberMovement.showPartyMemberTrain();
                break;
            case OOCActivity.inDialogue:
                PartyMemberMovement.instantiatePartyMemberTrain();
                PartyMemberMovement.hidePartyMemberTrain();
                OnStateChangeToInDialogue.Invoke();
                break;
            case OOCActivity.inUI:
                NotificationManager.OnDeleteAllNotifications.Invoke();
                OnStateChangeToInUI.Invoke();
                break;
            case OOCActivity.inMap:
                OnStateChangeToInMap.Invoke();
                break;
            case OOCActivity.intimidating:
            case OOCActivity.cunning:
            case OOCActivity.observing:
                OOCUIManager.updateOOCUI();
                OnStateChangeToSkill.Invoke();
                break;
            case OOCActivity.inChestUI:
                OnStateChangeToInChestUI.Invoke();
                break;
            case OOCActivity.inBookUI:
                OnStateChangeToInBookUI.Invoke();
                break;
            case OOCActivity.inShopUI:
                break;
            case OOCActivity.inDialoguePopUp:
                break;
            case OOCActivity.inLevelUpPopUp:
                break;
            case OOCActivity.inTutorialPopUp:
                break;
            case OOCActivity.inTutorialSequence:
                break;
        }

        PlayerMovement.setButtonPromptVisibility();
    }
}
