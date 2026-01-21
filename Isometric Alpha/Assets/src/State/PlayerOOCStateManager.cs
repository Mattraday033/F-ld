using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum OOCActivity {
							nothing 	 = 0, 
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
                            inTutorialSequence = 14,
                            inWorldMap = 15,
                            inFade = 16,
                            preCombat = 17
						};

public static class PlayerOOCStateManager
{
    public static OOCActivity currentActivity { get; private set; }
    public static OOCActivity previousActivity { get; private set; }

    public readonly static UnityEvent OnStateChangeToWalking = new UnityEvent();
    public readonly static UnityEvent OnStateChangeFromWalking = new UnityEvent();

    
    public readonly static UnityEvent OnStateChangeToInDialogue = new UnityEvent();

    public readonly static UnityEvent OnStateChangeToInUI = new UnityEvent();
    public readonly static UnityEvent OnStateChangeFromInUI = new UnityEvent();

    public readonly static UnityEvent OnStateChangeFromInShopUI = new UnityEvent();

    public readonly static UnityEvent OnStateChangeToInMap = new UnityEvent();
    public readonly static UnityEvent OnStateChangeToInWorldMap = new UnityEvent();
    public readonly static UnityEvent OnStateChangeToSkill = new UnityEvent();
    public readonly static UnityEvent OnStateChangeToInChestUI = new UnityEvent();
    public readonly static UnityEvent OnStateChangeFromInChestUI = new UnityEvent();
    public readonly static UnityEvent OnStateChangeToInBookUI = new UnityEvent();
    public readonly static UnityEvent OnStateChangeToInShopUI = new UnityEvent();
    public readonly static UnityEvent OnStateChangeToInDialoguePopUp = new UnityEvent();
    public readonly static UnityEvent OnStateChangeToInLevelUpPopUp = new UnityEvent();
    public readonly static UnityEvent OnStateChangeToInTutorialSequence = new UnityEvent();

    public readonly static UnityEvent OnLeavingTutorialSequenceState = new UnityEvent();
    

    [RuntimeInitializeOnLoadMethod]
    private static void initializePlayerOOCStateManager()
    {
        currentActivity = OOCActivity.walking;
        previousActivity = OOCActivity.nothing;

        // TransitionManager.AfterTransition.AddListener(setToDefaultStateOnTransition);
        FadeToBlackManager.OnFadeBackInFinished.AddListener(checkIfWaitingOnSecondHostilityTutorial);
    }

    // private static void setToDefaultStateOnTransition()
    // {
    //     if (currentActivity != OOCActivity.inDialogue &&
    //         currentActivity != OOCActivity.inTutorialSequence)
    //     {
    //         setCurrentActivity(OOCActivity.walking);
    //     }
    // }

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

        currentActivity = newActivity;

        switch (previousActivity)
        {
            case OOCActivity.walking:
                OnStateChangeFromWalking.Invoke();
                break;
            case OOCActivity.inDialogue:
                break;
            case OOCActivity.inUI:
                OnStateChangeFromInUI.Invoke();
                break;
            case OOCActivity.inMap:
                break;
            case OOCActivity.intimidating:
            case OOCActivity.cunning:
            case OOCActivity.observing:
                break;
            case OOCActivity.inChestUI:
                OnStateChangeFromInChestUI.Invoke();
                break;
            case OOCActivity.inBookUI:
                break;
            case OOCActivity.inShopUI:
                OnStateChangeFromInShopUI.Invoke();
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
            case OOCActivity.inWorldMap:
                break;
        }

        switch (currentActivity)
        {
            case OOCActivity.walking:
                OnStateChangeToWalking.Invoke();
                //EscapeStack.escapeAll();
                OOCUIManager.updateOOCUI();
                PartyMemberTrainManager.showPartyMemberTrain();
                break;
            case OOCActivity.inDialogue:
                PartyMemberTrainManager.createPartyMemberTrain();
                PartyMemberTrainManager.hidePartyMemberTrain();
                OnStateChangeToInDialogue.Invoke();
                break;
            case OOCActivity.inUI:
                if(previousActivity != OOCActivity.inTutorialSequence)
                {
                    NotificationManager.OnDeleteAllNotifications.Invoke();
                }
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
            case OOCActivity.inWorldMap:
                OnStateChangeToInWorldMap.Invoke();
                break;
        }

        PlayerObject.setButtonPromptVisibility(Constants.indexZero);
    }

    public static bool waitingOnHostilityTutorial;

    private static void checkIfWaitingOnSecondHostilityTutorial()
    {
        if(waitingOnHostilityTutorial)
        {
            waitingOnHostilityTutorial = false;
            PlayerObject.getInstance().StartCoroutine(waitForHostilityTutorial());
        }
    }

    private static IEnumerator waitForHostilityTutorial()
    {

		do
		{
			if (currentActivity == OOCActivity.inTutorialSequence)
			{
				yield break;
			}
			else
			{
				yield return null;
			}
		}while (currentActivity != OOCActivity.walking);

        do
		{
            yield return null;
		} while (FadeToBlackManager.isMidFade());

		while (currentActivity != OOCActivity.walking)
		{
			if (currentActivity == OOCActivity.inTutorialSequence)
			{
				yield break;
			}
			else
			{
				yield return null;
			}
		}

		if (!TutorialSequence.currentlyInTutorialSequence() && TutorialSequence.startTutorialSequence(TutorialSequenceList.secondHostilityTutorialSequenceKey))
		{
			setCurrentActivity(OOCActivity.inTutorialSequence);
		}
    }
}
