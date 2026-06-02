using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public enum SurpriseState   { 
                                EnemySurprised, 
                                NoOneSurprised, 
                                PlayerSurprised 
                            }
                            
public enum WhoseTurn   {
                            Start = 1, 
                            Player = 2, 
                            Resolving = 3, 
                            Won = 4, 
                            Lost = 5,
                            TickDown = 6
                        }

public enum CurrentActivity {
                                Waiting = 1,
                                ChoosingActor = 2,
                                ChoosingAbility = 3, 
                                ChoosingLocation = 4, 
                                ChoosingTertiary = 5, 
                                Repositioning = 6, 
                                Tutorial = 7, 
                                Retreating = 8, 
                                InEscapeMenu = 9,
                                Finished = 10,
                                ResolveActionWarning = 11
                            }

public interface INeedsUpdateOnStateChange
{
	public void updateOnStateChange();
}

public class CombatStateManager : MonoBehaviour
{
    private const float fastForwardTimeScale = 4f;
    private const float resolvingTurnTimeScale = 1.2f;
    private const float normalTimeScale = 1f;

	public static int deadMonsterCount = 0;

	public Transform combatBackgroundGrid;

    public static bool hasReturnCell = false;
    public static Vector3Int returnCell;

	public static List<GridCoords> allQueuedSummonLocations = new List<GridCoords>();

	public static bool inCombat = false;

	//which turn it is, or N+1 where N is the amount of WhoseTurn.Resolving the combat has passed through
	public static int turnNumber = 1;

	public SelectorManager selectorManager;

    #region UnityEvents

	public readonly static UnityEvent OnNewTurn = new UnityEvent();
    public readonly static UnityEvent OnTurnChangeToPlayer = new UnityEvent();
    public readonly static UnityEvent OnTurnChangeToResolving = new UnityEvent();
    public readonly static UnityEvent OnTurnChangeToWon = new UnityEvent();
    public readonly static UnityEvent OnTurnChangeToLost = new UnityEvent();

    public readonly static UnityEvent OnActivityChangeToWaiting = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToChoosingActor = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToChoosingAbility = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToChoosingLocation = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToChoosingTertiary = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToRepositioning = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToTutorial = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToRetreating = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToInEscapeMenu = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToFinished = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeToResolveTurnWarning = new UnityEvent();


    public readonly static UnityEvent OnActivityChangeFromInEscapeMenu = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeFromTutorial = new UnityEvent();
    public readonly static UnityEvent OnActivityChangeFromResolveTurnWarning = new UnityEvent();


    public readonly static UnityEvent OnCurrentActivityChange = new UnityEvent();

	public readonly static UnityEvent OnCombatStart = new UnityEvent();
	public readonly static UnityEvent OnCombatEnd = new UnityEvent();

    #endregion

	public CombatActionManager combatActionManager;

	public static SurpriseState whoIsSurprised;
	public static WhoseTurn whoseTurn;
	public static CurrentActivity currentActivity { get; private set; }

    public static string currentDefeatKey = "";

    public Transform creatureParent;
    public Grid creatureGrid;

	public Ticker ticker;

	public static string locationBeforeCombat;

	public static CombatStateManager instance;

	private GameOverPopUpButton gameOverPopUpButton;
    private BinaryPanelPopUpButton binaryPanelPopUpButton;

	private static bool resolvingTurnDuringTutorial;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeCombatStateManager()
    {
        instance = null;
        currentActivity = CurrentActivity.ChoosingActor;
        resolvingTurnDuringTutorial = false;
        locationBeforeCombat = null;
        whoseTurn = WhoseTurn.Start;
        whoIsSurprised = SurpriseState.NoOneSurprised;
        turnNumber = 1;
        currentDefeatKey = "";
        inCombat = false;
        deadMonsterCount = 0;
        hasReturnCell = false;
        returnCell = new Vector3Int();
        allQueuedSummonLocations = new List<GridCoords>();
        retreatedFromIndex = -1;

        TransitionManager.BeforeTransition.AddListener(resetRetreatedFromIndex);
        LoadSaveFile.OnLoadResetData.AddListener(resetRetreatedFromIndex);
        LoadSaveFile.OnLoadResetData.AddListener(resetCombat);
    }

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Found more than one CombatStateManager in the scene.");
		}

		instance = this;
        announceCombatIsStarted();
	}

	// Start is called before the first frame update
	void Start()
	{
		updateTurnState(WhoseTurn.Start);

		CombatGrid.cleanCombatGrid();
		CombatActionManager.wipeLockedInCombatActionQueue();
		CombatActionManager.wipeAllCombatActions();

        CreatureSpawner.spawnFormation();
        CreatureSpawner.spawnEnemyPackInfo();
        CreatureSpawner.spawnAllyPackInfo();

        EquippedPassiveTraitManager.ApplyAllEquippedPassiveTraits.Invoke();

        AudioManager.endAmbience();

		ticker = Ticker.getInstance();

		if (RevealManager.currentlyRevealed)
		{
			RevealManager.toggleReveal();
		}

		instantiateBackground();

		selectorManager.instantiateAllSelectors();

        if(State.enemyPackInfo.alwaysSurprised)
        {
            whoIsSurprised = SurpriseState.EnemySurprised;
        }

		switch (whoIsSurprised)
		{
			case SurpriseState.EnemySurprised:
				updateTurnState(WhoseTurn.Player);
				setCurrentActivity(CurrentActivity.ChoosingActor);
				break;
			case SurpriseState.NoOneSurprised:
				updateTurnState(WhoseTurn.Player);
				setCurrentActivity(CurrentActivity.ChoosingActor);
				combatActionManager.decideAndShowEnemyCombatActions();
				combatActionManager.decideAndShowSummonedCombatActions();
				break;
			case SurpriseState.PlayerSurprised:
				combatActionManager.decideAndShowEnemyCombatActions();
				StartCoroutine(waitOneFrameToStartEnemyCombatActions());
				break;
		}

		gameOverPopUpButton = new GameOverPopUpButton();
        binaryPanelPopUpButton = new BinaryPanelPopUpButton();

		CombatUI.setCurrentActivityText(currentActivity);
        OnNewTurn.Invoke();

		// CombatHoverManager.instantiateCombatHovers();

		if (getCombatTutorialKey() != null)
		{
			if (getCombatTutorialKey().Equals(TutorialSequenceList.combatTutorialSeenFlag))
			{
				TutorialSequence.startTutorialSequence(TutorialSequenceList.getCombatTutorialSequence());
			}
			else
			{
				TutorialSequence.startTutorialSequence(TutorialSequenceList.getTutorialSequence(getCombatTutorialKey()));
			}
		}
		else
		{
			StartCoroutine(waitOneFrameThenSpawnHoverUI());
		}
	}

    private static void announceCombatIsStarted()
    {
        inCombat = true;
        OnCombatStart.Invoke();
    }

    private static void announceCombatFinished()
    {
        inCombat = false;
        OnCombatEnd.Invoke();
    }

    public static void setReturnCell(Vector3Int newReturnCell)
    {
        hasReturnCell = true;
        returnCell = newReturnCell;
    }

    public static Vector3Int useReturnCell()
    {
        hasReturnCell = false;
        return returnCell;
    }

	public static CombatStateManager getInstance()
	{
		return instance;
	}

	private IEnumerator waitOneFrameThenSpawnHoverUI()
	{
		yield return null;
		yield return null;
		yield return null;

		Helpers.updateGameObjectPosition(PartyManager.getPlayerStats().combatSprite);

		SelectorManager.displayCurrentHoverUI();
	}

	public void updateAllObjectsAfterStateChange()
	{
		/*
		foreach(INeedsUpdateOnStateChange managerToUpdate in listOfObjectToUpdate)
		{
			managerToUpdate.updateOnStateChange();
		}
		*/
	}

	public bool shouldMoveToFinished()
	{
		if (CombatActionManager.finishedChoosingPartyMemberCombatActions() &&
            currentActivity != CurrentActivity.Waiting &&
            currentActivity != CurrentActivity.Finished)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void instantiateBackground()
	{
		Instantiate(AreaList.getCurrentCombatBackgroundObject(), combatBackgroundGrid);
	}

	public static void skipCombatTutorial()
	{
		SelectorManager.currentSelector.setToOriginalColor();

		SelectorManager.currentSelector.SetActive(false);

		SelectorManager.currentSelector = SelectorManager.getInstance().selectors[0];

		SelectorManager.currentSelector.SetActive(true);

		if (AbilityMenuManager.getInstance() != null)
		{
			AbilityMenuManager.getInstance().disableAbilityButtonCanvas();
		}

		PopUpScreenBlockerManager.destroyPopUpScreenBlocker();

		SelectorManager.displayCurrentHoverUI();

		DamagePreviewManager.wipeAllDamagePreviews();

		TutorialSequence.endCurrentTutorialSequence();

		if (whoseTurn != WhoseTurn.Player)
		{
			resolveTurn();
		}
		else
		{
			setCurrentActivity(CurrentActivity.ChoosingActor);
			updateTurnState(WhoseTurn.Player);
		}
	}

	//updates turn state internally and sets UI to reflect that
	public static void updateTurnState(WhoseTurn wT)
	{
		if (whoseTurn == WhoseTurn.Resolving && wT == WhoseTurn.Player &&
			getInstance() != null && getInstance().ticker != null)
		{
            whoseTurn = WhoseTurn.TickDown;
			bool onDeathEffectsBeingHandled = getInstance().ticker.tickDownEverything();
			SelectorManager.displayCurrentHoverUI();

            if(whoseTurn == WhoseTurn.Won || onDeathEffectsBeingHandled)
            {
                return;
            }
		}

		whoseTurn = wT;

        switch(wT)
        {
            case WhoseTurn.Start:
                Time.timeScale = normalTimeScale;
                break;
            case WhoseTurn.Player:
                Time.timeScale = normalTimeScale;
                OnTurnChangeToPlayer.Invoke();
                break;
            case WhoseTurn.Resolving:
                Time.timeScale = resolvingTurnTimeScale;
                OnTurnChangeToResolving.Invoke();
                break;
            case WhoseTurn.Won:
                Time.timeScale = normalTimeScale;
                OnTurnChangeToWon.Invoke();
                break;
            case WhoseTurn.Lost:
                AudioManager.playDefeatMusic();
                Time.timeScale = normalTimeScale;
                OnTurnChangeToLost.Invoke();
                break;
            default:
                Debug.LogError("Unknown WhoseTurn State: " + wT.ToString());
                break;
        }

		// Debug.LogError("whoseTurn = " + whoseTurn.ToString());

		CombatUI.setTurnInfoText(whoseTurn);
		getInstance().updateAllObjectsAfterStateChange();

		if (whoseTurn == WhoseTurn.Lost)
		{
			getInstance().gameOverPopUpButton.spawnPopUp();
		}
        
        SelectorManager.declareSelectors();
	}

	public static void resolveTurn()
	{
		if (currentActivity == CurrentActivity.Tutorial)
		{
			resolvingTurnDuringTutorial = true;
		} else if(!PlayerCombatActionManager.playerHasActionsInQueue() && 
                currentActivity != CurrentActivity.ResolveActionWarning && 
                whoseTurn != WhoseTurn.Start)
        {
            spawnResolveTurnWarning();
            if(currentActivity == CurrentActivity.ResolveActionWarning)
            {
                return;
            }
        }

		updateTurnState(WhoseTurn.Resolving);
		CombatActionManager.lockInCombatActionOrder();
		SelectorManager.deactivateCombatantInfoUIHoverPanel();
		setCurrentActivity(CurrentActivity.Waiting);
        instance.StartCoroutine(waitBeforeFirstResolve());
	}

    private static void spawnResolveTurnWarning()
    {
        if(instance != null)
        {
            instance.binaryPanelPopUpButton.spawnPopUp(new ResolveTurnWithNoActions());
            setCurrentActivity(CurrentActivity.ResolveActionWarning);
        }
    }

    private static IEnumerator waitBeforeFirstResolve()
    {
        float timeElapsed = 0;

        while (timeElapsed < CombatActionManager.waitBetweenCombatActions)
        {
            yield return null;

            timeElapsed += Time.deltaTime;
        }

        CombatActionManager.getInstance().resolveACombatAction();
    }

    public void endResolvingPhase()
    {
        updateTurnState(WhoseTurn.Player);

        if(whoseTurn == WhoseTurn.TickDown)
        {
            return;
        }

        turnNumber++;
        resetAllQueuedSummonLocations();
        combatActionManager.decideAndShowEnemyCombatActions();
        combatActionManager.decideAndShowSummonedCombatActions();
        CombatUI.setCombatActionCounterPanelsToDefault();

        if (resolvingTurnDuringTutorial)
        {
            setCurrentActivity(CurrentActivity.Tutorial);
            resolvingTurnDuringTutorial = false;
            TutorialSequence.spawnCurrentTutorialPopUp();
        }
        else
        {
            setCurrentActivity(CurrentActivity.ChoosingActor);
        }

        OnNewTurn.Invoke();
    }

	public static bool canResolveTurn()
	{
		if (currentActivity == CurrentActivity.ChoosingActor || currentActivity == CurrentActivity.Finished)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static void setCurrentActivity(CurrentActivity newActivity)
	{
        switch(currentActivity)
        {
            case CurrentActivity.Waiting:
                break;
            case CurrentActivity.ChoosingActor:
                break;
            case CurrentActivity.ChoosingAbility:
                break;
            case CurrentActivity.ChoosingLocation:
                break;
            case CurrentActivity.ChoosingTertiary:
                break;
            case CurrentActivity.Tutorial:
                OnActivityChangeFromTutorial.Invoke();
                break;
            case CurrentActivity.Retreating:
                break;
            case CurrentActivity.InEscapeMenu:
                OnActivityChangeFromInEscapeMenu.Invoke();
                break;
            case CurrentActivity.Finished:
                break;
            case CurrentActivity.ResolveActionWarning:
                OnActivityChangeFromResolveTurnWarning.Invoke();
                break;
            default:
                Debug.LogError("Unknown CurrentActivity State: " + currentActivity.ToString());
                break;
        }

		currentActivity = newActivity;

        switch(newActivity)
        {
            case CurrentActivity.Waiting:
                OnActivityChangeToWaiting.Invoke();
                break;
            case CurrentActivity.ChoosingActor:
                OnActivityChangeToChoosingActor.Invoke();
                break;
            case CurrentActivity.ChoosingAbility:
                OnActivityChangeToChoosingAbility.Invoke();
                break;
            case CurrentActivity.ChoosingLocation:
                OnActivityChangeToChoosingLocation.Invoke();
                break;
            case CurrentActivity.ChoosingTertiary:
                OnActivityChangeToChoosingTertiary.Invoke();
                break;
            case CurrentActivity.Tutorial:
                OnActivityChangeToTutorial.Invoke();
                break;
            case CurrentActivity.Retreating:
                OnActivityChangeToRetreating.Invoke();
                break;
            case CurrentActivity.InEscapeMenu:
                OnActivityChangeToInEscapeMenu.Invoke();
                break;
            case CurrentActivity.Finished:
                OnActivityChangeToFinished.Invoke();
                break;
            case CurrentActivity.ResolveActionWarning:
                OnActivityChangeToResolveTurnWarning.Invoke();
                break;
            default:
                Debug.LogError("Unknown CurrentActivity State: " + newActivity.ToString());
                break;
        }

		if (!stateAllowsDamagePreviews())
		{
			DamagePreviewManager.wipeAllDamagePreviews();
		}

		// Debug.LogError("CombatStateManager.currentActivity = " + CombatStateManager.currentActivity.ToString());

		CombatUI.checkAndSetResolveTurnButtonInteractability();

		RetreatUIManager.setRetreatButtonInteractibility();

		if (currentActivity == CurrentActivity.ChoosingActor)
		{
			SelectorManager.createPressEPrompt();
			CurrentActionHoverPanelManager.removeCurrentPrimaryDescribable();	
		}
		else
		{
			SelectorManager.destroyPressEPrompt();
		}

		getInstance().updateAllObjectsAfterStateChange();
		CombatUI.setCurrentActivityText(currentActivity);
        SelectorManager.declareSelectors();
        OnCurrentActivityChange.Invoke();
	}

	public void checkForWinOrLossStates()
	{
        if(whoseTurn == WhoseTurn.Start || whoseTurn == WhoseTurn.Won || whoseTurn == WhoseTurn.Lost)
        {
            return;
        }

		if (PartyManager.getPlayerStats().currentHealth <= 0)
		{
			updateTurnState(WhoseTurn.Lost);
		}
		else if (CombatGrid.getTotalAliveEnemyCount() == 0 ||
			CombatGrid.getEnemyMasterCount() == 0)
		{
			setToWonState();
		}
	}

	public void setToWonState()
	{
		CombatUI.populateCombatActionPanels();
		updateTurnState(WhoseTurn.Won);

        if (State.enemyPackInfo.getXPDrops() > 0)
        {
            PartyManager.addXP(State.enemyPackInfo.getXPDrops());
        }

		CombatUI.combatResultsPopUpButton.spawnPopUp();
	}

	public static void resetCombat()
	{
		resetAllQueuedSummonLocations();

		CombatActionManager.lockedInCombatActionQueue = new List<CombatAction>();
		CombatActionManager.onDeathCombatActionQueue = new List<CombatAction>();
		CombatActionManager.critCombatActionQueue = new List<CombatAction>();

		PlayerCombatActionManager.playerCombatActionQueue = new List<CombatAction>();
		EnemyCombatActionManager.enemyCombatActionQueue = new List<CombatAction>();
		EnemyCombatActionManager.slowedEnemyCombatActionQueue = new List<CombatAction>();
		CombatActionManager.lockedInCombatActionQueue = new List<CombatAction>();

		EnvironmentalCombatActionManager.deleteAllEnvironmentalCombatActions();

		EquippedPassiveTraitManager.removeAllTraits();

		OnNewTurn.RemoveAllListeners();

		deadMonsterCount = 0;

		turnNumber = 1;

		PartyManager.resetAllPartyMemberCooldowns();

        currentActivity = CurrentActivity.ChoosingActor;
        whoseTurn = WhoseTurn.Start;
        
		CombatAnimationManager.flushAnimations();

		State.enteredCombatFromDialogue = false;
		State.allyPackInfo = null;

        if(LocationNameList.slaveShackSix.Equals(AreaManager.locationName))
        {
            CunningManager.resetCunningsRemaining();
        }

		StepCountScriptManager.reset();
        inCombat = false;
	}

    public static int retreatedFromIndex = -1;

    public static void resetRetreatedFromIndex()
    {
        retreatedFromIndex = -1;
    }

	public static void returnToOverworld(bool defeatedEnemy)
	{
        if (defeatedEnemy)
        {
            resetRetreatedFromIndex();
        }

		if (!State.enteredCombatFromDialogue && defeatedEnemy)
		{
			MonsterDefeatKeysList.setDefeatKey(currentDefeatKey, defeatedEnemy);
		}

		if (!(State.enemyPackInfo is null) && defeatedEnemy)
		{
			State.enemyPackInfo.markBossAsKilled();

			if (State.enemyPackInfo.dialogueUponSceneLoadKey != null && State.enemyPackInfo.dialogueUponSceneLoadKey.Length > 0)
			{
				State.dialogueUponSceneLoadKey = State.enemyPackInfo.dialogueUponSceneLoadKey;
			}

			if (State.enemyPackInfo.getQuestScript() != null)
			{
				State.enemyPackInfo.getQuestScript().runScript();

				NotificationManager.skipNextNotificationSpawn();

			}
			else if (State.enemyPackInfo.getQuestName() != null && State.enemyPackInfo.getQuestName().Length > 0)
			{
				QuestList.activateQuestStep(State.enemyPackInfo.getQuestName(), State.enemyPackInfo.getQuestStep());
			}

            if(!MonsterNameList.packNameNeverAddsHostility(State.enemyPackInfo))
            {
                AreaList.addHostility();
            }
		}

        if (defeatedEnemy)
        {
			State.formation.applyRegeneration();
        }

		resetCombat();

        announceCombatFinished();

        MovementManager.initializeMovementManager();

        TransitionManager.ChangeAreaMusic.Invoke(AreaManager.locationName);

        SceneChange.changeSceneToOverworld();
	}

	public static bool isPlayerSurpriseRound()
	{
		if (turnNumber <= PartyStats.getPartySurpriseRounds() &&
			whoIsSurprised == SurpriseState.EnemySurprised)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static bool isEnemySurpriseRound()
	{
		if (turnNumber <= 1 &&
			whoIsSurprised == SurpriseState.PlayerSurprised)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public IEnumerator waitOneFrameToStartEnemyCombatActions()
	{
		yield return null;

		if (currentActivity == CurrentActivity.Tutorial)
		{
			yield break;
		}

		CombatUI.setResolveTurnButtonInteractable(false);

		while(FadeToBlackManager.isMidScreenFade())
		{
			yield return null;
		}

		yield return new WaitForSeconds(0.1f);

		updateTurnState(WhoseTurn.Resolving);
		CombatActionManager.lockInCombatActionOrder();
		SelectorManager.deactivateCombatantInfoUIHoverPanel();
		setCurrentActivity(CurrentActivity.Waiting);
		CombatActionManager.getInstance().resolveACombatAction();
	}

	public static bool snappingToTargetDuringReposition()
	{
		return (currentActivity != CurrentActivity.Repositioning && currentActivity != CurrentActivity.ChoosingTertiary) ||
               (currentActivity == CurrentActivity.Repositioning && RepositionManager.currentRepositionActivity == CurrentRepositionActivity.ChoosingRepositionTarget);
	}

	public static bool choosingRepositionTarget()
	{
		return currentActivity == CurrentActivity.Repositioning && 
                RepositionManager.currentRepositionActivity == CurrentRepositionActivity.ChoosingRepositionTarget;
	}

	public static bool findingEmptySpaceForReposition()
	{
		return currentActivity == CurrentActivity.Repositioning && 
                RepositionManager.currentRepositionActivity == CurrentRepositionActivity.ChoosingNewLocation;
	}

	private static void resetAllQueuedSummonLocations()
	{
		allQueuedSummonLocations = new List<GridCoords>();
	}

	private static string getCombatTutorialKey()
	{

		if (!TutorialFlags.getFlag(TutorialSequenceList.combatTutorialSeenFlag))
		{
			return TutorialSequenceList.combatTutorialSeenFlag;
		}

		EnemyPackInfo packInfo = State.enemyPackInfo;

		if (!(State.enemyPackInfo is null) && State.enemyPackInfo.tutorialSequenceKey != null && State.enemyPackInfo.tutorialSequenceKey.Length > 0)
		{
			return State.enemyPackInfo.tutorialSequenceKey;
		}

		return null;
	}

    public static bool stateAllowsDamagePreviews()
    {
        return currentActivity == CurrentActivity.ChoosingLocation || currentActivity == CurrentActivity.ChoosingTertiary;
    }

    public static Transform getCreatureParent()
    {
        return instance.creatureParent;
    }
    
    public static Grid getCreatureGrid()
    {
        return instance.creatureGrid;
    }

    public static void setTimeScale()
    {
        switch(whoseTurn)
        {
            case WhoseTurn.Start:
            case WhoseTurn.Player:
            case WhoseTurn.Won:
            case WhoseTurn.Lost:
                Time.timeScale = normalTimeScale;
                break;
            case WhoseTurn.Resolving:
                if(currentActivity == CurrentActivity.Waiting && 
                    Input.GetKey(KeyBindingList.combatFastForwardAnimationKey.getCurrentKeyCode()))
                {
                    Time.timeScale = fastForwardTimeScale;
                } else
                {
                    Time.timeScale = resolvingTurnTimeScale;
                }
                break;
        }
    }
}
