using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionManager : MonoBehaviour
{
    public readonly static UnityEvent<string> ChangeAreaMusic = new UnityEvent<string>();

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

        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        ChangeAreaMusic.Invoke(blueprint.currentLocation);
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

        if(transition.destinationName.Equals(ZoneKeyList.forest))
        {
            SceneChange.changeSceneToEndOfDemo();
            return;
        }

        if (fadeToBlackOnTransition && !FadeToBlackManager.isMidScreenFade())
        {
            fadeToBlackManager.setAndStartFadeToBlack();
            ChangeAreaMusic.Invoke(transition.destinationName);
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

        for(int i = 0; i < Constants.sizeTen; i++)
        {
            yield return null;
        }
        
        AudioManager.playOnTransitionSFX();

        while (FadeToBlackManager.isMidScreenFade())
        {
            yield return null;
        }

        if(!skipAutosave)
        {
            SaveHandler.autosave(transition);
        }

        if(!transition.playScriptAfterTransition)
        {
            transition.playScript();
        }

        BeforeTransition.Invoke();

        AreaManager.changeArea(transition.destinationName);

        currentTransitions = new List<Transition>();

        CollectTransitionSpaces.Invoke();
        MouseHoverManager.OnHoverPanelCreation.Invoke();
        moveToMatchingTransition(transition);

        AfterTransition.Invoke();
        
        if(transition.playScriptAfterTransition)
        {
            transition.playScript();
        }

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

}
