using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//The one activity for the whole game. Nothing to InInputField belong to the overworld and menus, Waiting onwards
//to combat, and InTutorialSequence to both - CombatStateManager.inCombat says which side is live.
public enum CurrentActivity {
                            Nothing = 0,
                            Walking = 1,
                            InDialogue = 2,
                            InUI = 3,
                            InMap = 4,
                            Cunning = 5,
                            Observing = 6,
                            Intimidating = 7,
                            InChestUI = 8,
                            InBookUI = 9,
                            InShopUI = 10,
                            InDialoguePopUp = 11,
                            InLevelUpPopUp = 12,
                            InTutorialPopUp = 13,
                            InTutorialSequence = 14,
                            InWorldMap = 15,
                            InFade = 16,
                            PreCombat = 17,
                            Defeat = 18,
                            Loading = 19,
                            InAnimation = 20,
                            InOpeningMonologue = 21,
                            MainMenu = 22,
                            InInputField = 23,

                            Waiting = 24,
                            ChoosingActor = 25,
                            ChoosingAbility = 26,
                            ChoosingLocation = 27,
                            ChoosingTertiary = 28,
                            Repositioning = 29,
                            Retreating = 30,
                            InEscapeMenu = 31,
                            Finished = 32,
                            ResolveActionWarning = 33
                        };

public static class CurrentActivityExtensions
{
    //the activities that only exist inside combat
    public static bool isCombatActivity(this CurrentActivity activity)
    {
        switch (activity)
        {
            case CurrentActivity.Waiting:
            case CurrentActivity.ChoosingActor:
            case CurrentActivity.ChoosingAbility:
            case CurrentActivity.ChoosingLocation:
            case CurrentActivity.ChoosingTertiary:
            case CurrentActivity.Repositioning:
            case CurrentActivity.Retreating:
            case CurrentActivity.InEscapeMenu:
            case CurrentActivity.Finished:
            case CurrentActivity.ResolveActionWarning:
                return true;
            default:
                return false;
        }
    }

    //InTutorialSequence is the one activity both sides use
    public static bool isValidInCombat(this CurrentActivity activity)
    {
        return activity.isCombatActivity() || activity == CurrentActivity.InTutorialSequence;
    }
}

//class OOCPlayer
public static class PlayerStateManager
{
    //Initialized here rather than only in initializePlayerStateManager: that runs AfterSceneLoad, so with
    //the start menu scene already open in the editor its objects would Awake and ask inMainMenu() first. A
    //static initializer runs on the first read of the type, which is what the Flags constructor used to give.
    public static CurrentActivity currentActivity { get; private set; } = CurrentActivity.MainMenu;
    public static CurrentActivity previousActivity { get; private set; } = CurrentActivity.Nothing;

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

    public readonly static UnityEvent OnLeavingTutorialSequenceState = new UnityEvent();
    

    [RuntimeInitializeOnLoadMethod]
    private static void initializePlayerStateManager()
    {
        //Every boot lands on the start menu (SceneChange.setSceneToStartMenu), so that is the state the game
        //starts in. This is what the newGame flag used to say by being true in the Flags static constructor.
        currentActivity = CurrentActivity.MainMenu;
        previousActivity = CurrentActivity.Nothing;

        //This assigns the field directly rather than going through setCurrentActivity, so it is the one
        //state change that would otherwise leave every CustomInputAction disabled.
        updateEnabledInputActions();

        // TransitionManager.AfterTransition.AddListener(setToDefaultStateOnTransition);
        FadeToBlackManager.OnFadeBackInFinished.AddListener(checkIfWaitingOnSecondHostilityTutorial);
        OnStateChangeToWalking.AddListener(checkIfWaitingOnSecondHostilityTutorial);
    }

    // private static void setToDefaultStateOnTransition()
    // {
    //     if (currentActivity != CurrentActivity.InDialogue &&
    //         currentActivity != CurrentActivity.InTutorialSequence)
    //     {
    //         setCurrentActivity(CurrentActivity.Walking);
    //     }
    // }

    public static void returnToPreviousActivity()
    {
        if (previousActivity == CurrentActivity.InLevelUpPopUp)
        {
            setCurrentActivity(CurrentActivity.Walking);
        }
        else
        {
            // Debug.LogError("previousActivity = " + previousActivity.ToString());
            setCurrentActivity(previousActivity);
        }
    }

    public static void setCurrentActivity(CurrentActivity newActivity)
    {
        setCurrentActivity(newActivity, false);
    }

    //Combat and the overworld share this one activity, but each side keeps its own rules. An activity from the
    //side that is not live is dropped, so an overworld call made mid-fight still does nothing, as the old
    //inCombat early return made sure of, and a combat call made outside a fight does nothing either.
    public static void setCurrentActivity(CurrentActivity newActivity, bool tutorialSequenceCheckBypass)
    {
        if (CombatStateManager.inCombat ? !newActivity.isValidInCombat() : newActivity.isCombatActivity())
        {
            return;
        }

        if (CombatStateManager.inCombat)
        {
            setCombatActivity(newActivity);
        }
        else
        {
            setOOCActivity(newActivity, tutorialSequenceCheckBypass);
        }
    }

    //For crossing into and out of combat. Neither side's events fire - the side being left is being torn down
    //and the side being entered is not set up yet - but the input set still follows the new activity.
    public static void setActivityWithoutEvents(CurrentActivity newActivity)
    {
        previousActivity = currentActivity;
        currentActivity = newActivity;

        updateEnabledInputActions();
    }

    //There is no same-value check: combat sets an activity again to re-run its refresh. The from events fire
    //before the assignment because their listeners read the activity being left.
    private static void setCombatActivity(CurrentActivity newActivity)
    {
        if (CombatStateManager.whoseTurn == WhoseTurn.Won ||
            CombatStateManager.whoseTurn == WhoseTurn.Lost)
        {
            return;
        }

        CombatStateManager.fireActivityChangeFromEvents(currentActivity, newActivity);

        previousActivity = currentActivity;
        currentActivity = newActivity;

        CombatStateManager.fireActivityChangeToEvents(newActivity);

        updateEnabledInputActions();
    }

    private static void setOOCActivity(CurrentActivity newActivity, bool tutorialSequenceCheckBypass)
    {
        if (!tutorialSequenceCheckBypass && (currentActivity == newActivity ||
            (currentActivity == CurrentActivity.InTutorialSequence && newActivity != CurrentActivity.Walking)))
        {
            return;
        }

        if (newActivity < CurrentActivity.Walking)
        {
            newActivity = CurrentActivity.Walking;
        }

        previousActivity = currentActivity;

        currentActivity = newActivity;

        // Debug.LogError("previousActivity = " + previousActivity);
        // Debug.LogError("currentActivity = " + currentActivity);

        switch (previousActivity)
        {
            case CurrentActivity.Walking:
                OnStateChangeFromWalking.Invoke();
                break;
            case CurrentActivity.InDialogue:
                OnStateChangeFromInDialogue.Invoke();
                break;
            case CurrentActivity.InUI:
                OnStateChangeFromInUI.Invoke();
                break;
            case CurrentActivity.InMap:
                break;
            case CurrentActivity.Intimidating:
            case CurrentActivity.Cunning:
            case CurrentActivity.Observing:
                OnStateChangeFromSkill.Invoke();
                break;
            case CurrentActivity.InChestUI:
                OnStateChangeFromInChestUI.Invoke();
                break;
            case CurrentActivity.InBookUI:
                break;
            case CurrentActivity.InShopUI:
                OnStateChangeFromInShopUI.Invoke();
                break;
            case CurrentActivity.InDialoguePopUp:
                break;
            case CurrentActivity.InLevelUpPopUp:
                break;
            case CurrentActivity.InTutorialPopUp:
                break;
            case CurrentActivity.InTutorialSequence:
                OnLeavingTutorialSequenceState.Invoke();
                break;
            case CurrentActivity.InWorldMap:
                break;
            case CurrentActivity.Defeat:
                break;
            case CurrentActivity.Loading:
                break;
            case CurrentActivity.MainMenu:
                break;
        }

        switch (currentActivity)
        {
            case CurrentActivity.Walking:
                OnStateChangeToWalking.Invoke();
                // if(previousActivity != CurrentActivity.InFade)
                // {
                //     PartyMemberTrainManager.showPartyMemberTrain();
                // }
                break;
            case CurrentActivity.InDialogue:
                OnStateChangeToInDialogue.Invoke();
                break;
            case CurrentActivity.InUI:
                if(previousActivity != CurrentActivity.InTutorialSequence)
                {
                    NotificationManager.OnDeleteAllNotifications.Invoke();
                }
                OnStateChangeToInUI.Invoke();
                break;
            case CurrentActivity.InMap:
                OnStateChangeToInMap.Invoke();
                break;
            case CurrentActivity.Intimidating:
            case CurrentActivity.Cunning:
            case CurrentActivity.Observing:
                OOCUIManager.updateOOCUI();
                OnStateChangeToSkill.Invoke();
                break;
            case CurrentActivity.InChestUI:
                OnStateChangeToInChestUI.Invoke();
                break;
            case CurrentActivity.InBookUI:
                OnStateChangeToInBookUI.Invoke();
                break;
            case CurrentActivity.InShopUI:
                break;
            case CurrentActivity.InDialoguePopUp:
                break;
            case CurrentActivity.InLevelUpPopUp:
                break;
            case CurrentActivity.InTutorialPopUp:
                break;
            case CurrentActivity.InTutorialSequence:
                break;
            case CurrentActivity.InWorldMap:
                OnStateChangeToInWorldMap.Invoke();
                break;
            case CurrentActivity.Defeat:
                break;
            case CurrentActivity.Loading:
                break;
            case CurrentActivity.MainMenu:
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
            case CurrentActivity.Walking:
                PlayerInputList.enableWalkingInputActions();
                break;
            case CurrentActivity.InDialogue:
                PlayerInputList.enableDialogueInputActions();
                break;
            case CurrentActivity.InUI:
                PlayerInputList.enableUIInputActions();
                break;
            case CurrentActivity.InMap:
                PlayerInputList.enableMapInputActions();
                break;
            case CurrentActivity.Cunning:
                PlayerInputList.enableCunningInputActions();
                break;
            case CurrentActivity.Observing:
                PlayerInputList.enableObservingInputActions();
                break;
            case CurrentActivity.Intimidating:
                PlayerInputList.enableIntimidatingInputActions();
                break;
            case CurrentActivity.InChestUI:
                PlayerInputList.enableChestInputActions();
                break;
            case CurrentActivity.InBookUI:
                PlayerInputList.enableBookInputActions();
                break;
            case CurrentActivity.InShopUI:
                PlayerInputList.enableShopInputActions();
                break;
            case CurrentActivity.InDialoguePopUp:
                PlayerInputList.enableDialoguePopUpInputActions();
                break;
            case CurrentActivity.InLevelUpPopUp:
                PlayerInputList.enableLevelUpPopUpInputActions();
                break;
            case CurrentActivity.InTutorialPopUp:
                PlayerInputList.enableTutorialPopUpInputActions();
                break;
            case CurrentActivity.InWorldMap:
                PlayerInputList.enableWorldMapInputActions();
                break;
            case CurrentActivity.MainMenu:
                PlayerInputList.enableMainMenuInputActions();
                break;
            case CurrentActivity.InInputField:
                PlayerInputList.enableInputFieldActions();
                break;
            case CurrentActivity.InTutorialSequence:
                PlayerInputList.enableTutorialSequenceInputActions();
                break;

            //An empty set is the real answer for these - the fade, loading, defeat and animation states are
            //meant to accept no input at all, and the opening monologue has its own polled input.
            case CurrentActivity.Nothing:
            case CurrentActivity.InFade:
            case CurrentActivity.PreCombat:
            case CurrentActivity.Defeat:
            case CurrentActivity.Loading:
            case CurrentActivity.InAnimation:
            case CurrentActivity.InOpeningMonologue:
                return;

            //Combat. InTutorialSequence is shared with the overworld, above.
            case CurrentActivity.Waiting:
                PlayerInputList.enableWaitingInputActions();
                break;
            case CurrentActivity.ChoosingActor:
            case CurrentActivity.Finished:
                PlayerInputList.enableChoosingActorInputActions();
                break;
            case CurrentActivity.ChoosingAbility:
                PlayerInputList.enableChoosingAbilityInputActions();
                break;
            case CurrentActivity.ChoosingLocation:
                PlayerInputList.enableChoosingLocationInputActions();
                break;
            case CurrentActivity.ChoosingTertiary:
                PlayerInputList.enableChoosingTertiaryInputActions();
                break;
            case CurrentActivity.Repositioning:
            case CurrentActivity.Retreating:
                PlayerInputList.enableRepositioningAndRetreatingInputActions();
                break;
            case CurrentActivity.InEscapeMenu:
                PlayerInputList.enableCombatEscapeMenuInputActions();
                break;
            case CurrentActivity.ResolveActionWarning:
                PlayerInputList.enableResolveActionWarningInputActions();
                break;
            default:
                Debug.LogError("Unrecognized CurrentActivity: " + currentActivity.ToString());
                break;
        }
    }

    //True while the player is on the start menu, including character creation and the opening monologue that
    //follows it - everything before a game is actually loaded. This is what the newGame flag used to mean.
    public static bool inMainMenu()
    {
        return currentActivity == CurrentActivity.MainMenu;
    }

    //InTutorialSequence is shared with combat, so this is the check for an overworld tutorial specifically
    public static bool inOOCTutorialSequence()
    {
        return currentActivity == CurrentActivity.InTutorialSequence && !CombatStateManager.inCombat;
    }

    public static bool enemyHoversLegal()
    {
        switch(currentActivity)
        {
            case CurrentActivity.Walking:
            case CurrentActivity.Cunning:
            case CurrentActivity.Intimidating:
            case CurrentActivity.InChestUI:
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
			if (currentActivity == CurrentActivity.InTutorialSequence)
			{
				yield break;
			}
			else
			{
				yield return null;
			}
		}while (currentActivity != CurrentActivity.Walking);

        do
		{
            yield return null;
		} while (FadeToBlackManager.isMidScreenFade());

		while (currentActivity != CurrentActivity.Walking)
		{
			if (currentActivity == CurrentActivity.InTutorialSequence)
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
			setCurrentActivity(CurrentActivity.InTutorialSequence);
		}
    }
}
