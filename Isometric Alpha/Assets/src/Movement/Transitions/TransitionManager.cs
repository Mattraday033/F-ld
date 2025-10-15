using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static UnityEvent OnTransitionUpdate = new UnityEvent();
	public static TransitionManager instance;

    public static bool autosaveMade = false;
	
	public static FadeToBlackManager fadeToBlackManager;
    public static bool fadeToBlackOnTransition;

    public static List<Transition> currentTransitions = new List<Transition>();

    private NewSceneTransition[] transitions;

    private static Coroutine currentCoroutine;

    void Start()
    {
        fadeToBlackManager = FadeToBlackManager.getInstance();
        fadeToBlackOnTransition = true;


    }

    public static void changeScene(Transition transition)
    {
        if(currentCoroutine != null)
        {
            return;
        }

        if (fadeToBlackOnTransition && !FadeToBlackManager.isBlack() && !fadeToBlackManager.currentlyFadingToBlack())
        {
            fadeToBlackManager.setAndStartFadeToBlack();
        }

        currentCoroutine = instance.StartCoroutine(instance.waitForBlackScreenThenTransition(transition));
    }
    
    public static void addTransition(Transition transition)
    {
        currentTransitions.Add(transition);
    }

    private IEnumerator waitForBlackScreenThenTransition(Transition transition)
    {
        while (fadeToBlackManager.currentlyFadingToBlack())
        {
            yield return null;
        }
        
        SaveHandler.autosave(transition);

        AreaManager.changeArea(transition.destinationAreaName);

        currentTransitions = new List<Transition>();

        OnTransitionUpdate.Invoke();
        
        movePlayerToTargetTransition(transition);

        currentCoroutine = null;
    }

    public void movePlayerToTargetTransition(Transition currentTransition)
    {
        foreach (Transition destinationTransition in currentTransitions)
        {
            if (currentTransition.sharesHash(destinationTransition))
            {
                PlayerMovement.getTransform().position = AreaManager.getMasterGrid().GetCellCenterWorld(destinationTransition.getOutPutCellCoords());
                State.playerFacing.setFacing(destinationTransition.playerSpawnDirection);
                MovementManager.instance.addPlayerSprite(PlayerMovement.getTransform());
                return;
            }
        }

        Debug.LogError("No destinationTransition found");
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
















    public static void changeScene(TransitionInfo sourceTransitionInfo)
    {
        if (fadeToBlackOnTransition && !FadeToBlackManager.isBlack() && !fadeToBlackManager.currentlyFadingToBlack())
        {
            fadeToBlackManager.setAndStartFadeToBlack();
        }

        if (!fadeToBlackOnTransition || (fadeToBlackOnTransition && FadeToBlackManager.isBlack()))
        {
            State.currentSourceTransitionInfo = sourceTransitionInfo.clone();

            if (!State.currentSourceTransitionInfo.skipAutoSave)
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

            resetRelevantDataOnSceneTransitionToExactPosition();

            autosaveMade = false;

            StepCountScriptManager.reset();
            PlayerInteractionScript.runAllScripts(State.currentSourceTransitionInfo.scripts);

            if (State.currentSourceTransitionInfo.flipFacing)
            {
                State.playerFacing.setToOpposingFacing();
            }

            SceneChange.changeSceneToOverworld();
        }
    }

	public void changeSceneWithoutTrigger(TransitionInfo sourceTransitionInfo)
	{
		changeScene(sourceTransitionInfo);

		StartCoroutine(getInstance().waitForBlackScreenThenTransition(sourceTransitionInfo));
	}

	private IEnumerator waitForBlackScreenThenTransition(TransitionInfo sourceTransitionInfo)
	{
		while (!FadeToBlackManager.isBlack())
		{
			yield return null;
		}

		changeScene(sourceTransitionInfo);
	}

	public void fastTravel(TransitionInfo sourceTransitionInfo)
	{
		State.currentSourceTransitionInfo = sourceTransitionInfo.clone();

		Flags.allowPartyTrainSpawning();

		try
		{
			// SaveHandler.autosave(PlayerMovement.getTransform().position);
		}
		catch (Exception e)
		{
			Debug.LogError("An autosave was attempted but failed");
			Debug.LogError(e.StackTrace);
		}

		if (fadeToBlackOnTransition && !FadeToBlackManager.isBlack() && !fadeToBlackManager.currentlyFadingToBlack())
		{
			fadeToBlackManager.setAndStartFadeToBlack();
		}

		StartCoroutine(waitForBlackScreenThenTransition());
	}
	
	private IEnumerator waitForBlackScreenThenTransition()
    {
        while (!FadeToBlackManager.isBlack())
        {
            yield return null;
        }
		
		StepCountScriptManager.reset();
		resetRelevantDataOnSceneTransitionToExactPosition();

		SceneChange.changeSceneToOverworld();
		
		yield break;
    }
	
	public Vector3 getCurrentDestinationWorldPosition()
	{
		if(State.currentSourceTransitionInfo.transitionHash != null)
		{
			return getCurrentDestinationWorldPositionFromTransitionHash();
		}

		string destinationSquareHash = State.currentSourceTransitionInfo.hash;
		
		foreach(NewSceneTransition transition in transitions)
		{
			if(transition.getTransitionInfo().hash.Equals(destinationSquareHash))
			{
				GameObject destinationSquare = transition.currentSceneDestinationSquare;
				
				return destinationSquare.transform.position;
			}
		}
		
		return transitions[0].currentSceneDestinationSquare.transform.position;
	}

	private Vector3 getCurrentDestinationWorldPositionFromTransitionHash()
	{
		TransitionHash destinationSquareHash = State.currentSourceTransitionInfo.transitionHash;

		foreach (NewSceneTransition transition in transitions)
		{
			if(transition.getTransitionInfo().transitionHash == null)
			{
				continue;
			}

			if (transition.getTransitionInfo().transitionHash.Equals(destinationSquareHash))
			{
				GameObject destinationSquare = transition.currentSceneDestinationSquare;

				return destinationSquare.transform.position;
			}
		}
		
		return transitions[0].currentSceneDestinationSquare.transform.position;
	}
	
	public static void resetCurrentSourceTransition()
	{
		State.currentSourceTransitionInfo = null;
	}
	
	public static bool hasASourceTransition()
	{
		if(State.currentSourceTransitionInfo == null || State.currentSourceTransitionInfo is null)
		{	
			return false;
		} else
		{
			return true;
		}
	}

    public static bool hasASortingLayer()
    {
		if(!hasASourceTransition())
		{
			return false;
		}

        if (State.currentSourceTransitionInfo.sortingLayerName != null && State.currentSourceTransitionInfo.sortingLayerName.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void resetRelevantDataOnSceneTransitionToExactPosition()
	{
		//AllMonsterPackLists.setAllMonsterPackListsToReset();
		State.currentMonsterPackList = null;
		CunningManager.resetCunningsRemaining();
		IntimidateManager.resetIntimidatesRemaining();
		TrapAndButtonStateManager.removeAllActivatedTrapKeys();
		PartyMemberPlacer.removeAllPlacedPartyMembers();
	}
	
    
	private static NewSceneTransition[] getAllTransitionObjects()
	{
		GameObject[] newSceneTransitionToExactPositionObjects = GameObject.FindGameObjectsWithTag(LayerAndTagManager.transitionTag);
		NewSceneTransition[] transitions = new NewSceneTransition[0];
		
		foreach(GameObject transitionObject in newSceneTransitionToExactPositionObjects)
		{
			NewSceneTransition newSceneTransitionToExactPosition = transitionObject.GetComponent<NewSceneTransition>();
			
			if(newSceneTransitionToExactPosition == null)
			{
				continue;
			}
			
			transitions = Helpers.appendArray<NewSceneTransition>(transitions,newSceneTransitionToExactPosition);
		}
		
		return transitions;
	}		
	
	private void Awake()
	{
		if(instance != null)  
		{						
			throw new IOException("There is already an instance of TransitionManager");
		}
	
		instance = this;
		transitions = getAllTransitionObjects();
	}
	
	public static TransitionManager getInstance()
	{
		return instance;
	}
}
