using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionManager : MonoBehaviour
{
    public readonly static UnityEvent BeforeTransition = new UnityEvent();

    public readonly static UnityEvent CollectTransitionSpaces = new UnityEvent();
    public readonly static UnityEvent AfterTransition = new UnityEvent();

	public static TransitionManager instance;

    public static bool autosaveMade;
	
	public static FadeToBlackManager fadeToBlackManager;
    public static bool fadeToBlackOnTransition;

    public static List<Transition> currentTransitions;

    private static Coroutine currentCoroutine;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeTransitionManager()
    {
        instance = null;
        autosaveMade = false;
        fadeToBlackManager = null;
        fadeToBlackOnTransition = false;
        currentCoroutine = null;
        currentTransitions = new List<Transition>();
    }

    private void Awake()
    {
        if (instance != null)
        {
            throw new IOException("There is already an instance of TransitionManager");
        }

        instance = this;
        // transitions = getAllTransitionObjects();
    }
	
	public static TransitionManager getInstance()
	{
		return instance;
	}

    void Start()
    {
        fadeToBlackManager = FadeToBlackManager.getInstance();
        fadeToBlackOnTransition = true;
    }

    public static void changeLocation(Transition transition, bool skipAutosave = false)
    {
        if(currentCoroutine != null)
        {
            return;
        }

        if (fadeToBlackOnTransition && !FadeToBlackManager.isMidFade())
        {
            fadeToBlackManager.setAndStartFadeToBlack();
        }

        currentCoroutine = instance.StartCoroutine(instance.waitForBlackScreenThenTransition(transition, skipAutosave));
    }
    
    public static void addTransition(Transition transition)
    {
        currentTransitions.Add(transition);
    }

    public static void fastTravel(string targetLocationName)
	{
        changeLocation(new Transition(AreaManager.locationName, targetLocationName));
	}

    private IEnumerator waitForBlackScreenThenTransition(Transition transition, bool skipAutosave)
    {
        NotificationManager.OnDeleteAllNotifications.Invoke();

        while (FadeToBlackManager.isMidFade())
        {
            yield return null;
        }

        if(!skipAutosave)
        {
            SaveHandler.autosave(transition);
        }

        transition.playScript();

        BeforeTransition.Invoke();

        AreaManager.changeArea(transition.destinationName);

        currentTransitions = new List<Transition>();

        CollectTransitionSpaces.Invoke();
        MouseHoverManager.OnHoverPanelCreation.Invoke();
        moveToMatchingTransition(transition);

        AfterTransition.Invoke();
        
        currentCoroutine = null;
    }

    private void moveToMatchingTransition(Transition currentTransition)
    {
        foreach (Transition destinationTransition in currentTransitions)
        {
            if(currentTransition.fastTravelCapable() && !destinationTransition.fastTravelCapable())
            {
                continue;
            } else if((currentTransition.fastTravelCapable() && destinationTransition.fastTravelCapable()) ||
                        currentTransition.sharesHash(destinationTransition))
            {
                moveToTargetTransition(destinationTransition);
                return;
            }
        }

        moveToTargetTransition(currentTransitions[0]);
    }

    private void moveToTargetTransition(Transition destinationTransition)
    {
        PlayerObject.getInstanceTransform().position = AreaManager.getMasterGrid().GetCellCenterWorld(destinationTransition.getOutPutCellCoords());
        PlayerMovement.setPlayerFacing(destinationTransition.playerSpawnDirection);
        MovementManager.addMovementTracker(PlayerMovement.getInstance());
    }

    private static void makeAutosave(Vector3 autosavePos)
    {
        try
        {
            if (!autosaveMade)
            {
                // SaveHandler.autosave(getInstance().getCurrentDestinationWorldPosition());
                autosaveMade = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("An autosave was attempted but failed");
            Debug.LogError(e.StackTrace);
        }
    }
















    // public static void changeLocation(TransitionInfo sourceTransitionInfo)
    // {
    //     if (fadeToBlackOnTransition && FadeToBlackManager.isMidFade() && !fadeToBlackManager.currentlyFadingToBlack())
    //     {
    //         fadeToBlackManager.setAndStartFadeToBlack();
    //     }

    //     if (!fadeToBlackOnTransition || (fadeToBlackOnTransition && FadeToBlackManager.isBlack()))
    //     {
    //         State.currentSourceTransitionInfo = sourceTransitionInfo.clone();

    //         if (!State.currentSourceTransitionInfo.skipAutoSave)
    //         {
    //             try
    //             {
    //                 if (!autosaveMade)
    //                 {
    //                     // SaveHandler.autosave(getInstance().getCurrentDestinationWorldPosition());
    //                     autosaveMade = true;
    //                 }
    //             }
    //             catch (Exception e)
    //             {
    //                 Debug.LogError("An autosave was attempted but failed");
    //                 Debug.LogError(e.StackTrace);
    //             }
    //         }

    //         resetRelevantDataOnSceneTransitionToExactPosition();

    //         autosaveMade = false;

    //         StepCountScriptManager.reset();
    //         PlayerInteractionScript.runAllScripts(State.currentSourceTransitionInfo.scripts);

    //         if (State.currentSourceTransitionInfo.flipFacing)
    //         {
    //             State.playerFacing.setToOpposingFacing();
    //         }

    //         SceneChange.changeLocationToOverworld();
    //     }
    // }

	// public void changeLocationWithoutTrigger(TransitionInfo sourceTransitionInfo)
	// {
	// 	changeLocation(sourceTransitionInfo);

	// 	StartCoroutine(getInstance().waitForBlackScreenThenTransition(sourceTransitionInfo));
	// }

	// private IEnumerator waitForBlackScreenThenTransition(TransitionInfo sourceTransitionInfo)
	// {
	// 	while (FadeToBlackManager.isMidFade())
	// 	{
	// 		yield return null;
	// 	}

	// 	changeLocation(sourceTransitionInfo);
	// }
	
	// public Vector3 getCurrentDestinationWorldPosition()
	// {
	// 	if(State.currentSourceTransitionInfo.transitionHash != null)
	// 	{
	// 		return getCurrentDestinationWorldPositionFromTransitionHash();
	// 	}

	// 	string destinationSquareHash = State.currentSourceTransitionInfo.hash;
		
	// 	foreach(NewSceneTransition transition in transitions)
	// 	{
	// 		if(transition.getTransitionInfo().hash.Equals(destinationSquareHash))
	// 		{
	// 			GameObject destinationSquare = transition.currentSceneDestinationSquare;
				
	// 			return destinationSquare.transform.position;
	// 		}
	// 	}
		
	// 	return transitions[0].currentSceneDestinationSquare.transform.position;
	// }

	// private Vector3 getCurrentDestinationWorldPositionFromTransitionHash()
	// {
	// 	TransitionHash destinationSquareHash = State.currentSourceTransitionInfo.transitionHash;

	// 	foreach (NewSceneTransition transition in transitions)
	// 	{
	// 		if(transition.getTransitionInfo().transitionHash == null)
	// 		{
	// 			continue;
	// 		}

	// 		if (transition.getTransitionInfo().transitionHash.Equals(destinationSquareHash))
	// 		{
	// 			GameObject destinationSquare = transition.currentSceneDestinationSquare;

	// 			return destinationSquare.transform.position;
	// 		}
	// 	}
		
	// 	return transitions[0].currentSceneDestinationSquare.transform.position;
	// }
	
	// public static void resetCurrentSourceTransition()
	// {
	// 	State.currentSourceTransitionInfo = null;
	// }
	
	// public static bool hasASourceTransition()
	// {
	// 	if(State.currentSourceTransitionInfo == null || State.currentSourceTransitionInfo is null)
	// 	{	
	// 		return false;
	// 	} else
	// 	{
	// 		return true;
	// 	}
	// }

    // public static bool hasASortingLayer()
    // {
	// 	if(!hasASourceTransition())
	// 	{
	// 		return false;
	// 	}

    //     if (State.currentSourceTransitionInfo.sortingLayerName != null && State.currentSourceTransitionInfo.sortingLayerName.Length > 0)
    //     {
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }

    // private static void resetRelevantDataOnSceneTransitionToExactPosition()
	// {
	// 	//AllMonsterPackLists.setAllMonsterPackListsToReset();
	// 	CunningManager.resetCunningsRemaining();
	// 	IntimidateManager.resetIntimidatesRemaining();
	// 	TrapAndButtonStateManager.resetTrapKeys();
	// 	PartyMemberPlacer.removeAllPlacedPartyMembers();
	// }
	
    
	// private static NewSceneTransition[] getAllTransitionObjects()
	// {
	// 	GameObject[] newSceneTransitionToExactPositionObjects = GameObject.FindGameObjectsWithTag(LayerAndTagManager.transitionTag);
	// 	NewSceneTransition[] transitions = new NewSceneTransition[0];
		
	// 	foreach(GameObject transitionObject in newSceneTransitionToExactPositionObjects)
	// 	{
	// 		NewSceneTransition newSceneTransitionToExactPosition = transitionObject.GetComponent<NewSceneTransition>();
			
	// 		if(newSceneTransitionToExactPosition == null)
	// 		{
	// 			continue;
	// 		}
			
	// 		transitions = Helpers.appendArray<NewSceneTransition>(transitions,newSceneTransitionToExactPosition);
	// 	}
		
	// 	return transitions;
	// }		
}
