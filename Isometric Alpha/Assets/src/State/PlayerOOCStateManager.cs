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
                            preCombat = 17,
                            Defeat = 18,
                            Loading = 19,
                            inAnimation = 20,
                            inOpeningMonologue = 21,
                            MainMenu = 22

						};
//class OOCPlayer
public static class PlayerOOCStateManager
{
    //Initialized here rather than only in initializePlayerOOCStateManager: that runs AfterSceneLoad, so with
    //the start menu scene already open in the editor its objects would Awake and ask inMainMenu() first. A
    //static initializer runs on the first read of the type, which is what the Flags constructor used to give.
    public static OOCActivity currentActivity { get; private set; } = OOCActivity.MainMenu;
    public static OOCActivity previousActivity { get; private set; } = OOCActivity.nothing;

    public readonly static UnityEvent OnStateChange = new UnityEvent();

    public readonly static UnityEvent OnStateChangeToWalking = new UnityEvent();
    public readonly static UnityEvent OnStateChangeFromWalking = new UnityEvent();

    public readonly static UnityEvent OnStateChangeToInDialogue = new UnityEvent();
    public readonly static UnityEvent OnStateChangeFromInDialogue = new UnityEvent();

    public readonly static UnityEvent OnStateChangeToInUI = new UnityEvent();
    public readonly static UnityEvent OnStateChangeFromInUI = new UnityEvent();

    public readonly static UnityEvent OnStateChangeFromInShopUI = new UnityEvent();

    public readonly static UnityEvent OnStateChangeToInMap = new UnityEvent();
    public readonly static UnityEvent OnStateChangeToInWorldMap = new UnityEvent();

    public readonly static UnityEvent OnStateChangeToSkill = new UnityEvent();
    public readonly static UnityEvent OnStateChangeFromSkill = new UnityEvent();

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
        //Every boot lands on the start menu (SceneChange.setSceneToStartMenu), so that is the state the game
        //starts in. This is what the newGame flag used to say by being true in the Flags static constructor.
        currentActivity = OOCActivity.MainMenu;
        previousActivity = OOCActivity.nothing;

        //This assigns the field directly rather than going through setCurrentActivity, so it is the one
        //state change that would otherwise leave every CustomInputAction disabled.
        updateEnabledInputActions();

        // TransitionManager.AfterTransition.AddListener(setToDefaultStateOnTransition);
        FadeToBlackManager.OnFadeBackInFinished.AddListener(checkIfWaitingOnSecondHostilityTutorial);
        OnStateChangeToWalking.AddListener(checkIfWaitingOnSecondHostilityTutorial);
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
        if (CombatStateManager.inCombat || 
            (!tutorialSequenceCheckBypass && (currentActivity == newActivity ||
            (currentActivity == OOCActivity.inTutorialSequence && newActivity != OOCActivity.walking))))
        {
            return;
        }

        if (newActivity < OOCActivity.walking)
        {
            newActivity = OOCActivity.walking;
        }

        previousActivity = currentActivity;

        currentActivity = newActivity;

        // Debug.LogError("previousActivity = " + previousActivity);
        // Debug.LogError("currentActivity = " + currentActivity);

        switch (previousActivity)
        {
            case OOCActivity.walking:
                OnStateChangeFromWalking.Invoke();
                break;
            case OOCActivity.inDialogue:
                OnStateChangeFromInDialogue.Invoke();
                break;
            case OOCActivity.inUI:
                OnStateChangeFromInUI.Invoke();
                break;
            case OOCActivity.inMap:
                break;
            case OOCActivity.intimidating:
            case OOCActivity.cunning:
            case OOCActivity.observing:
                OnStateChangeFromSkill.Invoke();
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
            case OOCActivity.Defeat:
                break;
            case OOCActivity.Loading:
                break;
            case OOCActivity.MainMenu:
                break;
        }

        switch (currentActivity)
        {
            case OOCActivity.walking:
                OnStateChangeToWalking.Invoke();
                // if(previousActivity != OOCActivity.inFade)
                // {
                //     PartyMemberTrainManager.showPartyMemberTrain();
                // }
                break;
            case OOCActivity.inDialogue:
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
            case OOCActivity.Defeat:
                break;
            case OOCActivity.Loading:
                break;
            case OOCActivity.MainMenu:
                break;
        }

        OnStateChange.Invoke();

        //Last, so that any listener above which changed the state itself has already run and settled.
        updateEnabledInputActions();
    }

    //Re-computes which CustomInputActions are live. This reads currentActivity rather than taking the new
    //activity as an argument: a state change listener can call setCurrentActivity itself, and reading the
    //field fresh makes the outer call's re-sync land on the state that actually won rather than a stale one.
    //
    //Anything that needs to swallow input for a while can invoke CustomInputAction.DisableAllInputActions
    //and call this to put the set back, which is why it is public.
    public static void updateEnabledInputActions()
    {
        //PlayerInputList is a static class, so its actions - and the DisableAllInputActions listeners they
        //register as they are constructed - do not exist until the class is touched. Without this the first
        //Invoke would have no listeners and would silently disable nothing.
        PlayerInputList.ensureInitialized();

        CustomInputAction.DisableAllInputActions.Invoke();

        switch (currentActivity)
        {
            case OOCActivity.walking:
                PlayerInputList.enableWalkingInputActions();
                break;
            case OOCActivity.inDialogue:
                PlayerInputList.enableDialogueInputActions();
                break;
            case OOCActivity.inUI:
                PlayerInputList.enableUIInputActions();
                break;
            case OOCActivity.inMap:
                PlayerInputList.enableMapInputActions();
                break;
            case OOCActivity.cunning:
                PlayerInputList.enableCunningInputActions();
                break;
            case OOCActivity.observing:
                PlayerInputList.enableObservingInputActions();
                break;
            case OOCActivity.intimidating:
                PlayerInputList.enableIntimidatingInputActions();
                break;
            case OOCActivity.inChestUI:
                PlayerInputList.enableChestInputActions();
                break;
            case OOCActivity.inBookUI:
                PlayerInputList.enableBookInputActions();
                break;
            case OOCActivity.inShopUI:
                PlayerInputList.enableShopInputActions();
                break;
            case OOCActivity.inDialoguePopUp:
                PlayerInputList.enableDialoguePopUpInputActions();
                break;
            case OOCActivity.inLevelUpPopUp:
                PlayerInputList.enableLevelUpPopUpInputActions();
                break;
            case OOCActivity.inTutorialPopUp:
                PlayerInputList.enableTutorialPopUpInputActions();
                break;
            case OOCActivity.inWorldMap:
                PlayerInputList.enableWorldMapInputActions();
                break;
            case OOCActivity.MainMenu:
                PlayerInputList.enableMainMenuInputActions();
                break;

            //An empty set is the real answer for these - the fade, loading, defeat and animation states are
            //meant to accept no input at all, the tutorial sequence is driven by TutorialSequenceInput, and
            //the opening monologue has its own polled input.
            case OOCActivity.nothing:
            case OOCActivity.inTutorialSequence:
            case OOCActivity.inFade:
            case OOCActivity.preCombat:
            case OOCActivity.Defeat:
            case OOCActivity.Loading:
            case OOCActivity.inAnimation:
            case OOCActivity.inOpeningMonologue:
                return;
            default:
                Debug.LogError("Unrecognized OOCActivity: " + currentActivity.ToString());
                break;
        }
    }

    //True while the player is on the start menu, including character creation and the opening monologue that
    //follows it - everything before a game is actually loaded. This is what the newGame flag used to mean.
    public static bool inMainMenu()
    {
        return currentActivity == OOCActivity.MainMenu;
    }

    public static bool enemyHoversLegal()
    {
        switch(currentActivity)
        {
            case OOCActivity.walking:
            case OOCActivity.cunning:
            case OOCActivity.intimidating:
            case OOCActivity.inChestUI:
                return true;
            default: 
                return false;
        }
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
		} while (FadeToBlackManager.isMidScreenFade());

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
