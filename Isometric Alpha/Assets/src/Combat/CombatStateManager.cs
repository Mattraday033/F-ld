using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public enum SurpriseState { EnemySurprised, NoOneSurprised, PlayerSurprised }
public enum WhoseTurn {Start = 1, Player = 2, Resolving = 3, Won = 4, Lost = 5}
public enum CurrentActivity {Waiting = 1, ChoosingActor = 2, ChoosingAbility = 3, ChoosingLocation = 4, ChoosingTertiary = 5, Repositioning = 6, Tutorial = 7, Retreating = 8, Finished = 9 }
																	  //What the player is currently doing, used to determine what
																	  //class has priority for listening for key presses. Not to be
																	  //confused with the action class, which is used for describing 
																	  //Attacks/Abilities/Items the player has queued to be used on
																	  //their turn.
																	  //Waiting means that it's not the player's turn.	
																	  //Finished means the only thing left to do is Resolve Turn

public interface INeedsUpdateOnStateChange
{
	public void updateOnStateChange();
}

public class CombatStateManager : MonoBehaviour
{
	public static int deadMonsterCount = 0;

	public Transform combatBackgroundGrid;

	public static FadeToBlackManager fadeToBlackManager;
	public static ArrayList allQueuedSummonLocations = new ArrayList();

	public static bool inCombat = false;

	public static MonsterPack currentMonsterPack;
	//which turn it is, or N+1 where N is the amount of WhoseTurn.Resolving the combat has passed through
	public static int turnNumber = 1;

	public SelectorManager selectorManager;
	public static UnityEvent OnNewTurn = new UnityEvent();

	public CombatActionManager combatActionManager;

	public static SurpriseState whoIsSurprised;
	public static WhoseTurn whoseTurn;
	public static CurrentActivity currentActivity { get; private set; }

	public EnemySpawner enemySpawner;
	public PartySpawner partySpawner;
	public SummonSpawner summonSpawner;

	public Ticker ticker;

	public static string locationBeforeCombat;

	public static CombatStateManager instance;

	public ArrayList listOfObjectToUpdate;

	private GameOverPopUpButton gameOverPopUpButton;

	private static bool resolvingTurnDuringTutorial = false;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Found more than one CombatStateManager in the scene.");
		}

		instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{
		inCombat = true;
		initializeListOfObjectToUpdate();

		fadeToBlackManager = FadeToBlackManager.getInstance();

		updateTurnState(WhoseTurn.Start);

		CombatGrid.cleanCombatGrid();
		CombatActionManager.wipeLockedInCombatActionQueue();
		CombatActionManager.wipeAllCombatActions();

		enemySpawner.spawn();
		partySpawner.spawnFormation();
		summonSpawner.spawn();

		ticker = Ticker.getInstance();

		if (RevealManager.currentlyRevealed)
		{
			RevealManager.toggleReveal();
		}

		instantiateBackground();

		selectorManager.instantiateAllSelectors();

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

		CombatUI.setCurrentActivityText(currentActivity);
        OnNewTurn.Invoke();

		CombatHoverManager.instantiateCombatHovers();

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

	void Update()
	{
		if (Input.GetKey(KeyBindingList.resolveTurnKey) && canResolveTurn() && !KeyPressManager.handlingPrimaryKeyPress)
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			resolveTurn();
		}
	}

	public static CombatStateManager getInstance()
	{
		return instance;
	}

	private IEnumerator waitOneFrameThenSpawnHoverUI()
	{
		yield return null;

		Helpers.updateGameObjectPosition(PartyManager.getPlayerStats().combatSprite);

		SelectorManager.displayHoverUIForCurrentSelectorTarget();
	}

	public void initializeListOfObjectToUpdate()
	{
		listOfObjectToUpdate = new ArrayList();

		listOfObjectToUpdate.Add(RepositionManager.getInstance());
		listOfObjectToUpdate.Add(RepositionUIManager.getInstance());
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

		PopUpBlocker.destroyPopUpScreenBlocker();

		SelectorManager.displayHoverUIForCurrentSelectorTarget();

		DamagePreviewManager.resetAllDamagePreviews();

		TutorialSequence.endCurrentTutorialSequence();

		if (whoseTurn != WhoseTurn.Player)
		{
			getInstance().resolveTurn();
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
			getInstance().ticker.tickDownEverything();
			SelectorManager.displayHoverUIForCurrentSelectorTarget();
		}

		whoseTurn = wT;

		// Debug.LogError("whoseTurn = " + whoseTurn.ToString());

		CombatUI.setTurnInfoText(whoseTurn);
		getInstance().updateAllObjectsAfterStateChange();

		if (whoseTurn == WhoseTurn.Lost)
		{
			getInstance().gameOverPopUpButton.spawnPopUp();
		}
	}

	public void resolveTurn()
	{
		if (currentActivity == CurrentActivity.Tutorial)
		{
			resolvingTurnDuringTutorial = true;
		}

		updateTurnState(WhoseTurn.Resolving);
		CombatActionManager.lockInCombatActionOrder();
		selectorManager.deactivateCombatantInfoUIHoverPanel();
		setCurrentActivity(CurrentActivity.Waiting);
		CombatActionManager.getInstance().resolveACombatAction();
	}

	public void endResolvingPhase()
	{
		turnNumber++;
		resetAllQueuedSummonLocations();
		combatActionManager.decideAndShowEnemyCombatActions();
		combatActionManager.decideAndShowSummonedCombatActions();
		CombatUI.setCombatActionCounterPanelsToDefault();
		updateTurnState(WhoseTurn.Player);

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

	public static void setCurrentActivity(CurrentActivity currentActivity)
	{
		CombatStateManager.currentActivity = currentActivity;

		if (!stateAllowsDamagePreviews())
		{
			DamagePreviewManager.resetAllDamagePreviewsOnStateChange();
		}

		// Debug.LogError("CombatStateManager.currentActivity = " + CombatStateManager.currentActivity.ToString());

		CombatUI.checkAndSetResolveTurnButtonInteractability();

		RetreatUIManager.setRetreatButtonInteractibility();

		if (CombatStateManager.currentActivity == CurrentActivity.ChoosingActor)
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
	}

	public void checkForWinOrLossStates()
	{
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

        if (State.enemyPackInfo.xpDrop > 0)
        {
            PartyManager.addXP(State.enemyPackInfo.xpDrop);
        }

		CombatUI.combatResultsPopUpButton.spawnPopUp();
	}

	public static void resetCombat()
	{
		resetAllQueuedSummonLocations();

		CombatActionManager.lockedInCombatActionQueue = new ArrayList();
		CombatActionManager.onDeathCombatActionQueue = new ArrayList();
		CombatActionManager.critCombatActionQueue = new ArrayList();

		PlayerCombatActionManager.playerCombatActionQueue = new ArrayList();
		EnemyCombatActionManager.enemyCombatActionQueue = new ArrayList();
		EnemyCombatActionManager.slowedEnemyCombatActionQueue = new ArrayList();
		CombatActionManager.lockedInCombatActionQueue = new ArrayList();

		EnvironmentalCombatActionManager.deleteAllEnvironmentalCombatActions();

		ActivatedPassiveTraitManager.removeAllTraits();

		OnNewTurn.RemoveAllListeners();

		deadMonsterCount = 0;

		turnNumber = 1;

		PartyManager.resetAllPartyMemberCooldowns();

		CombatAnimationManager.flushAnimations();

		State.enteredCombatFromDialogue = false;

		StepCountScriptManager.reset();
	}

	public static void returnToOverworld(bool defeatedEnemy)
	{
		if (State.currentMonsterPackList != null &&
			(currentMonsterPack.index < State.currentMonsterPackList.monsterPacks.Length && currentMonsterPack.index >= 0))
		{
			if (!defeatedEnemy)
			{
				State.currentMonsterPackList.monsterPacks[currentMonsterPack.index].retreatCounter = 1;
			}

			//State.currentMonsterPackList.shouldReset = false;
		}

		if (!State.enteredCombatFromDialogue && defeatedEnemy)
		{
			setDefeatKey(currentMonsterPack.index, defeatedEnemy);
		}

		if (!(State.enemyPackInfo is null) && defeatedEnemy)
		{
			State.enemyPackInfo.markBossAsKilled();

			if (State.enemyPackInfo.dialogueUponSceneLoadKey != null && State.enemyPackInfo.dialogueUponSceneLoadKey.Length > 0)
			{
				State.dialogueUponSceneLoadKey = State.enemyPackInfo.dialogueUponSceneLoadKey;
			}

			if (State.enemyPackInfo.script != null)
			{
				State.enemyPackInfo.script.runScript();

				NotificationManager.skipNextNotificationSpawn();

			}
			else if (State.enemyPackInfo.questName != null && State.enemyPackInfo.questName.Length > 0)
			{
				QuestList.activateQuestStep(State.enemyPackInfo.questName, State.enemyPackInfo.questStep);
			}
		}

        if (defeatedEnemy)
        {
			State.formation.applyRegeneration();
        }

		resetCombat();

        inCombat = false;

        Debug.LogError("Return to Overworld Location not implemented");
        // sceneBeforeCombat.changeScene();
	}

	private static void setDefeatKey(int index, bool defeated)
    {
        Debug.LogError("setDefeatKey(int, bool) not implemented");

		// if (State.monsterDefeatKeys.ContainsKey(sceneBeforeCombat.sceneToLoad + "-" + index))
		// {
		// 	State.monsterDefeatKeys[sceneBeforeCombat.sceneToLoad + "-" + index] = defeated;
		// }
		// else
		// {
		// 	State.monsterDefeatKeys.Add(sceneBeforeCombat.sceneToLoad + "-" + index, defeated);
		// }
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

		updateTurnState(WhoseTurn.Resolving);
		CombatActionManager.lockInCombatActionOrder();
		selectorManager.deactivateCombatantInfoUIHoverPanel();
		setCurrentActivity(CurrentActivity.Waiting);
		CombatActionManager.getInstance().resolveACombatAction();
	}

	public static bool snappingToTargetDuringReposition()
	{
		return ((currentActivity != CurrentActivity.Repositioning && CombatStateManager.currentActivity != CurrentActivity.ChoosingTertiary) || (currentActivity == CurrentActivity.Repositioning && RepositionManager.currentRepositionActivity == CurrentRepositionActivity.ChoosingRepositionTarget));
	}

	public static bool choosingRepositionTarget()
	{
		return (currentActivity == CurrentActivity.Repositioning && RepositionManager.currentRepositionActivity == CurrentRepositionActivity.ChoosingRepositionTarget);
	}

	public static bool findingEmptySpaceForReposition()
	{
		return (currentActivity == CurrentActivity.Repositioning && RepositionManager.currentRepositionActivity == CurrentRepositionActivity.ChoosingNewLocation);
	}

	private static void resetAllQueuedSummonLocations()
	{
		allQueuedSummonLocations = new ArrayList();
	}

	private static string getCombatTutorialKey()
	{

		if (!Flags.getFlag(TutorialSequenceList.combatTutorialSeenFlag))
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
}
