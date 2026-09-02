using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SelectorManager : MonoBehaviour 
{
    public readonly static string[] selectableAllyTagCriteria = new string[]{ LayerAndTagManager.playerTag };
	public readonly static string[] allyTagCriteria = new string[]{LayerAndTagManager.playerTag,
                                                                    LayerAndTagManager.npcTag};

	public readonly static string[] enemyTagCriteria = new string[]{LayerAndTagManager.playerTag,
                                                                    LayerAndTagManager.enemyTag,
                                                                    LayerAndTagManager.npcTag};

	public readonly static string[] allyAndEnemyTagCriteria = new string[]{LayerAndTagManager.playerTag,
                                                                            LayerAndTagManager.enemyTag,
                                                                            LayerAndTagManager.npcTag,
                                                                            LayerAndTagManager.placeHolderTag};

    public readonly static UnityEvent<List<Selector>> SelectorMoved = new UnityEvent<List<Selector>>();

	private GameObject pressEPrompt;

	public Transform selectorParent;

	public PlayerCombatActionManager playerCombatActionManager;

    private const int heartBeatsToWait = 6;
	public static int heartBeatCount = 0;

	public static bool isMoving = false;

	public static Selector currentSelector;
	public static AbilityMenuManager currentAbilityManager;

	public HoverPanelPopUpButton hoverPanelPopUpButton;

	private static SelectorManager instance;

	private void Awake()
	{
        instantiateSelectorManager();

		instance = this;
		hoverPanelPopUpButton = new HoverPanelPopUpButton();
	}

    private void OnEnable()
    {
        CombatStateManager.OnTurnChangeToResolving.AddListener(setFirstSelectorVisibility);
        CombatStateManager.OnTurnChangeToPlayer.AddListener(setFirstSelectorVisibility);

        CombatStateManager.OnActivityChangeToChoosingActor.AddListener(declareSelectors);
        CombatStateManager.OnActivityChangeToChoosingAbility.AddListener(declareSelectors);
        CombatStateManager.OnActivityChangeToChoosingLocation.AddListener(declareSelectors);
        CombatStateManager.OnActivityChangeToChoosingTertiary.AddListener(declareSelectors);

        CombatStateManager.OnActivityChangeToFinished.AddListener(declareSelectors);

        CombatStateManager.OnTurnChangeToWon.AddListener(declareSelectors);
        CombatStateManager.OnTurnChangeToWon.AddListener(destroyPressEPrompt);

        HeartBeatManager.FastHeartBeat.AddListener(moveCurrentSelector);

        CombatUIModule.OnHideCombatUI.AddListener(hideCurrentHoverUI);

        CombatStateManager.AfterCombatantsSpawn.AddListener(movePlayerCursorToPlayerCharacter);

        CombatActionOrderRow.OnPointerEnterCombatActionOrderRow.AddListener(hidePressEPrompt);
        CombatActionOrderRow.OnPointerExitCombatActionOrderRow.AddListener(showPressEPrompt);

        CombatStateManager.OnActivityChangeToChoosingActor.AddListener(resetCurrentSelector);
    }

    private void OnDestroy()
    {
        CombatStateManager.OnTurnChangeToResolving.RemoveListener(setFirstSelectorVisibility);
        CombatStateManager.OnTurnChangeToPlayer.RemoveListener(setFirstSelectorVisibility);

        CombatStateManager.OnActivityChangeToChoosingActor.RemoveListener(declareSelectors);
        CombatStateManager.OnActivityChangeToChoosingAbility.RemoveListener(declareSelectors);
        CombatStateManager.OnActivityChangeToChoosingLocation.RemoveListener(declareSelectors);
        CombatStateManager.OnActivityChangeToChoosingTertiary.RemoveListener(declareSelectors);

        CombatStateManager.OnActivityChangeToFinished.RemoveListener(declareSelectors);

        CombatStateManager.OnTurnChangeToWon.RemoveListener(declareSelectors);
        CombatStateManager.OnTurnChangeToWon.RemoveListener(destroyPressEPrompt);

        HeartBeatManager.FastHeartBeat.RemoveListener(moveCurrentSelector);

        CombatUIModule.OnHideCombatUI.RemoveListener(hideCurrentHoverUI);

        CombatStateManager.AfterCombatantsSpawn.RemoveListener(movePlayerCursorToPlayerCharacter);

        CombatActionOrderRow.OnPointerEnterCombatActionOrderRow.RemoveListener(hidePressEPrompt);
        CombatActionOrderRow.OnPointerExitCombatActionOrderRow.RemoveListener(showPressEPrompt);

        CombatStateManager.OnActivityChangeToChoosingActor.RemoveListener(resetCurrentSelector);
    }

    private void setFirstSelectorVisibility()
    {
        // SelectorFactory.playerCursor.getSelectorObject().SetActive(CombatStateManager.whoseTurn == WhoseTurn.Player);
    }

	public static SelectorManager getInstance()
	{
		return instance;
	}

    public static bool hasCurrentAbilityManager()
    {
        return currentAbilityManager != null;
    }

    public static bool hasCurrentlyVisibleAbilityManager()
    {
        return currentAbilityManager != null && currentAbilityManager.enabled;
    }

	public static Selector getCurrentSelector()
	{
		return currentSelector;
	}

	public static GridCoords getCurrentSelectorCoords()
	{
		return currentSelector.getCoords();
	}

    public static void backOutOfAbilityMenu()
    {
        setCurrentSelector(SelectorFactory.playerCursor, declareSelector: false);

        currentSelector.setToColor();

        currentAbilityManager.enableAbilityButtonCanvas();
        CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingAbility);

        DamagePreviewManager.wipeAllDamagePreviews();

        displayCurrentHoverUI();
    }

    public static void backOutOfTertiaryLocationSelection()
    {
        currentSelector.setToColor();

        CombatAction loadedCombatAction = currentAbilityManager.getCurrentlySelectedAction();

        currentSelector = loadedCombatAction.getSelector();

        if (loadedCombatAction.resetCoordsOnBackOutOfTertiary())
        {
            currentSelector.setToStartLocation();
        }
        else
        {
            currentSelector.setToLocation(loadedCombatAction.getTargetCoords());
        }
        
        CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingLocation);

        displayCurrentHoverUI();
    }

	public static void deselectAlly()
	{
		currentAbilityManager.disableAbilityButtonCanvas();
		CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);

		resetCurrentSelector();
	}

	public static void displayCurrentHoverUI()
	{
        if(CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

		if (!currentSelector.singleTile() ||
			(!TutorialFlags.getFlag(TutorialSequenceList.combatTutorialSeenFlag) &&
			CombatStateManager.currentActivity != CurrentActivity.Tutorial))
		{
			instance.hoverPanelPopUpButton.destroyPopUp();
			return;
		}

        instance.hoverPanelPopUpButton.spawnPopUp();
	}

    public static void hideCurrentHoverUI()
    {
        instance.hoverPanelPopUpButton.destroyPopUp();
    }

	public static void createPressEPrompt()
	{
		destroyPressEPrompt();

		if (!CombatGrid.combatantExistsAtCoords(getCurrentSelectorCoords(), out Stats target) || target.isDead() || 
            CombatActionManager.actorAlreadyHasCombatAction(getCurrentSelectorCoords()) ||
			(CombatGrid.positionIsOnAlliedSide(getCurrentSelectorCoords()) && CombatActionManager.finishedChoosingPartyMemberCombatActions()))
		{
			return;
		}

		GameObject sprite = target.combatSprite;

		if (currentSelector.singleTile() && Helpers.tagMatchesCriteria(sprite, selectableAllyTagCriteria))
		{
			if (CombatStateManager.currentActivity == CurrentActivity.ChoosingActor)
			{
				instance.pressEPrompt = Instantiate(Resources.Load<GameObject>(PrefabNames.combatSelectPrompt), sprite.transform.GetChild(sprite.transform.childCount - 1));
			}
		}

	}

	public static void destroyPressEPrompt()
	{
		if (instance != null && instance.pressEPrompt != null)
		{
			DestroyImmediate(instance.pressEPrompt);
			instance.pressEPrompt = null;
		}
	}

    private static void hidePressEPrompt()
    {
		if (instance != null && instance.pressEPrompt != null)
		{
			instance.pressEPrompt.SetActive(false);
		}
    }

    private static void showPressEPrompt()
    {
		if (instance != null && instance.pressEPrompt != null)
		{
			instance.pressEPrompt.SetActive(true);
		}
    }

	public static void deactivateCombatantInfoUIHoverPanel()
	{
		instance.hoverPanelPopUpButton.destroyPopUp();
	}

	public static void handleChoosingLocation()
	{
		if (instance == null || isMoving)
		{
			return;
		}

		CombatAction loadedCombatAction;

		if (CombatStateManager.findingEmptySpaceForReposition())
		{
			loadedCombatAction = RepositionManager.currentSingleTargetRepositionCombatAction;
		}
		else
		{
			loadedCombatAction = currentAbilityManager.getCurrentlySelectedAction();
		}

		if (loadedCombatAction.movesTarget() && currentSelector.targetsImmobileTarget())
		{
            AudioManager.playCannotChooseActorAbilityLocationSFX();
			return;
		}

		if (loadedCombatAction.targetsOnlyEmptySpace())
		{
			if (!currentSelector.hasAtLeastOneTarget(allyAndEnemyTagCriteria))
			{
				instance.finishChoosingLocation(loadedCombatAction);
			} else
            {
                AudioManager.playCannotChooseActorAbilityLocationSFX();
                return;
            }
		}
		else
		{
			if (loadedCombatAction.targetMustBeDead())
			{
				if (currentSelector.hasAtLeastOneTarget(enemyTagCriteria))
				{
					if (CombatGrid.enemyHasMandatoryTarget() && !currentSelector.hasAtLeastOneMandatoryTarget() && !loadedCombatAction.isSelfTargeting())
					{
                        AudioManager.playCannotChooseActorAbilityLocationSFX();
                        CombatantHover.HighlightAllMandatoryTargets.Invoke();
						return;
					}

					instance.finishChoosingLocation(loadedCombatAction);
				} else
                {
                    AudioManager.playCannotChooseActorAbilityLocationSFX();
                    return;
                }
			}
			else
			{
				if (loadedCombatAction.targetsAllySection())
				{
					if (currentSelector.hasAtLeastOneLivingTarget(allyTagCriteria))
                    //  && !(currentSelector.getAllSelectorCoords().Contains(loadedCombatAction.getActorCoords()) && !loadedCombatAction.repositionsCaster()))
					{
						instance.finishChoosingLocation(loadedCombatAction);
					}
				}
				else
				{
					if (currentSelector.hasAtLeastOneLivingTarget(enemyTagCriteria))
					{
						if (CombatGrid.enemyHasMandatoryTarget() && !currentSelector.hasAtLeastOneMandatoryTarget() && !loadedCombatAction.isSelfTargeting())
						{
                            AudioManager.playCannotChooseActorAbilityLocationSFX();
                            CombatantHover.HighlightAllMandatoryTargets.Invoke();
							return;
						}

						instance.finishChoosingLocation(loadedCombatAction);
					} else
                    {
                        AudioManager.playCannotChooseActorAbilityLocationSFX();
                        return;
                    }
				}
			}
		}
	}

	public void finishChoosingLocation(CombatAction loadedCombatAction)
	{
		//loadedCombatAction.setTargetCoords(new GridCoords(currentSelector.currentRow, currentSelector.currentCol));

		if (!loadedCombatAction.requiresTertiaryCoords())
		{
			if (loadedCombatAction.requiresAnAction())
			{
				playerCombatActionManager.queueCombatAction(SelectorFactory.playerCursor, currentSelector, loadedCombatAction);
                AudioManager.playChooseActorAbilityLocationSFX();
			}
			else
			{
				loadedCombatAction.performCombatAction();
			}

			if (loadedCombatAction.hasAssignedActor(out Stats loadedActorStats) && 
                loadedActorStats.positions.Count > 0)
			{
				SelectorFactory.playerCursor.setToLocation(loadedActorStats.positions[0]);
			}

			CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
			DamagePreviewManager.wipeAllDamagePreviews();

			displayCurrentHoverUI();
		}
		else
		{
			loadedCombatAction.setSecondaryCoords(loadedCombatAction.getTargetCoords());

			Selector tertiarySelector = loadedCombatAction.getTertiarySelector();

			if (loadedCombatAction.resetCoordsWhenChoosingTertiary())
			{
				tertiarySelector.setToStartLocation();
			}
			else
			{
				tertiarySelector.setToLocation(currentSelector.getCoords());
			}

            AudioManager.playChooseActorAbilityLocationSFX();

			setCurrentSelector(tertiarySelector);

			currentSelector.setToColor();

			CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingTertiary);
		}
	}

	public static void handleChoosingTertiary()
	{
		if (instance == null || isMoving)
		{
			return;
		}

		CombatAction loadedCombatAction = currentAbilityManager.getCurrentlySelectedAction();

		if (loadedCombatAction.tertiaryCoordsRequiresEmptySpace() && 
            CombatGrid.combatantExistsAtCoords(currentSelector.getCoords()))
		{
            AudioManager.playCannotChooseActorAbilityLocationSFX();
			return;
		}

		loadedCombatAction.setTertiaryCoords(currentSelector.getCoords());

		if (loadedCombatAction.tertiaryCoordsRequiresEmptySpace())
		{
			if (!currentSelector.hasAtLeastOneTarget(allyAndEnemyTagCriteria))
			{
				currentSelector.setToColor();

				instance.finishChoosingTertiary(loadedCombatAction);
			}
		}
		else
		{
			if (currentSelector.hasAtLeastOneTarget(enemyTagCriteria))
			{
				currentSelector.setToColor();

				instance.finishChoosingTertiary(loadedCombatAction);
			}
		}

		CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
	}

	public void finishChoosingTertiary(CombatAction loadedCombatAction)
	{
        if (loadedCombatAction.requiresAnAction())
        {
            playerCombatActionManager.queueCombatActionWithTertiary(SelectorFactory.playerCursor, currentSelector, loadedCombatAction);

            currentSelector.setToLocation(loadedCombatAction.getActorCoords());
        }
        else
        {
            loadedCombatAction.performCombatAction();
        }

        AudioManager.playChooseActorAbilityLocationSFX();

		CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);

		DamagePreviewManager.wipeAllDamagePreviews();
	}

	public static void handleAllySelection()
	{
		if (!SelectionInfo.selectedAllyCanAct(currentSelector.getCoords()) || 
            !CombatGrid.combatantExistsAtCoords(currentSelector.getCoords(), out Stats currentTarget))
		{
			return;
		}

		currentAbilityManager = currentTarget.combatSprite.GetComponent<AbilityMenuManager>();

		if (!currentAbilityManager.enabled)
		{
			currentAbilityManager.enableAbilityButtonCanvas();

			CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingAbility);
		}
	}

	public static void moveCurrentSelector()
	{
        moveCurrentSelector(0);
    }

	public static void moveCurrentSelector(int heartBeatRow)
	{

        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.Tutorial:
            
                if(!TutorialSequence.currentTutorialSequence.getCurrentTutorialSequenceStep().allowsMovementKeys)
                {
                    return;
                }

                break;
            case CurrentActivity.ChoosingActor:
			case CurrentActivity.ChoosingLocation:
			case CurrentActivity.ChoosingTertiary:
                break;
            default:
                return;
        }

        if(InspectNode.inspecting || CombatStateManager.whoseTurn != WhoseTurn.Player)
        {
            return;
        }

        if(heartBeatCount < heartBeatsToWait && !Input.GetKey(KeyBindingList.jumpMoveKey.getCurrentKeyCode()))
        {
            heartBeatCount++;
            return;
        }

        if(!KeyBindingList.movementKeyPressed() || 
            KeyPressManager.handlingSecondaryKeyPress ||
            currentSelector.selfTargeting)
        {
            heartBeatCount = heartBeatsToWait;
            return;
        }

        heartBeatCount = 0;
        isMoving = true;

		if (Input.GetKey(KeyBindingList.jumpMoveKey.getCurrentKeyCode()) &&
			// currentSelector.singleTile() &&
			CombatStateManager.snappingToTargetDuringReposition())
		{
			moveCurrentSelectorToNextSingleTileTarget();

			displayCurrentHoverUI();
            KeyPressManager.handlingSecondaryKeyPress = true;
			return;
		}

		bool moved = false;

		if (isMoving)
		{

			if (Input.GetKey(KeyBindingList.moveNorthKey.getCurrentKeyCode()) && canMoveUp())
			{
				currentSelector.setToLocation(new GridCoords(currentSelector.currentRow - 1, currentSelector.currentCol));
				moved = true;

			}
			else if (Input.GetKey(KeyBindingList.moveSouthKey.getCurrentKeyCode()) && canMoveDown())
			{
				currentSelector.setToLocation(new GridCoords(currentSelector.currentRow + 1, currentSelector.currentCol));
				moved = true;

			}
			else if (Input.GetKey(KeyBindingList.moveWestKey.getCurrentKeyCode()) && canMoveLeft())
			{
				currentSelector.setToLocation(new GridCoords(currentSelector.currentRow, currentSelector.currentCol - 1));
				moved = true;

			}
			else if (Input.GetKey(KeyBindingList.moveEastKey.getCurrentKeyCode()) && canMoveRight())
			{
				currentSelector.setToLocation(new GridCoords(currentSelector.currentRow, currentSelector.currentCol + 1));
				moved = true;
			}

			if (moved)
			{
				destroyPressEPrompt();
			}
		}

        isMoving = false;

		if (moved)
		{
            AudioManager.playSelectorMovedSFX();

            declareSelectors();

            if(CombatStateManager.currentActivity == CurrentActivity.ChoosingActor)
            {
                createPressEPrompt();
            }
		}
	}

    private static bool declareSelectorsQueued = false;

    // Selector declaration is batched to the end of the frame. A single state change can trigger
    // this many times over, so only the first call queues the coroutine; the rest fall out here.
    public static void declareSelectors()
    {
        if(declareSelectorsQueued)
        {
            return;
        }

        if(instance == null || !instance.isActiveAndEnabled)
        {
            declareSelectorsNow();
            return;
        }

        declareSelectorsQueued = true;
        instance.StartCoroutine(declareSelectorsAtEndOfFrame());
    }

    private static IEnumerator declareSelectorsAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        try
        {
            declareSelectorsNow();
        }
        finally
        {
            declareSelectorsQueued = false;
        }
    }

    private static void declareSelectorsNow()
    {
        CombatHoverTileManager.hideAllTiles();
        List<Selector> visibleSelectors = new List<Selector>();

        if(CombatStateManager.whoseTurn != WhoseTurn.Player)
        {
            SelectorMoved.Invoke(new List<Selector>());
            return;
        }

        visibleSelectors.Add(SelectorFactory.playerCursor);

        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingAbility:
            case CurrentActivity.ChoosingLocation:
                if(AbilityMenuManager.getInstance() != null && 
                    AbilityMenuManager.getInstance().getCurrentlySelectedAction() != null && 
                    AbilityMenuManager.getInstance().getCurrentlySelectedAction().getSelector() != null)
                {
                    Selector abilitySelector = AbilityMenuManager.getSelectorToShowWhileChoosingAbility();
                    if(abilitySelector != null)
                    {
                        visibleSelectors.Add(abilitySelector);
                    }
                }
                break;
            case CurrentActivity.ChoosingTertiary:

                CombatAction currentAction = AbilityMenuManager.getInstance().getCurrentlySelectedAction();

                Selector oldSelector = currentAction.getSelector().clone();

                oldSelector.setToLocation(currentAction.getSecondaryCoords(), declareSelectors: false);
                oldSelector.alwaysRed = true;

                visibleSelectors.Add(oldSelector);
                break;
            default:
                break;
        }

        if(currentSelector != SelectorFactory.playerCursor)
        {
            visibleSelectors.Add(currentSelector);
        }

        Selector hoverSelector = CombatHoverTileManager.getHoverSelector();

        if(hoverSelector != null)
        {
            visibleSelectors.Add(hoverSelector);
        }

        SelectorMoved.Invoke(visibleSelectors);
        updateAllDamagePreviews();
        displayCurrentHoverUI();
    }

	public static void moveCurrentSelectorToNextSingleTileTarget()
	{
		if (isMoving)
		{
			GridCoords targetPosition = GridCoords.getDefaultCoords();
            bool onAllySide = CombatGrid.positionIsOnAlliedSide(currentSelector.getCoords());

			if (Input.GetKey(KeyBindingList.moveWestKey.getCurrentKeyCode()))
			{
                if(onAllySide)
                {
				    targetPosition = CombatGrid.getPreviousAllyToJumpSelectorTo(currentSelector.getFirstCombatantCoords());
                } else
                {
				    targetPosition = CombatGrid.getPreviousEnemyToJumpSelectorTo(currentSelector.getFirstCombatantCoords());
                }
			}
			else if (Input.GetKey(KeyBindingList.moveEastKey.getCurrentKeyCode()))
			{
                if(onAllySide)
                {
				    targetPosition = CombatGrid.getNextAllyToJumpSelectorTo(currentSelector.getFirstCombatantCoords());
                } else
                {
				    targetPosition = CombatGrid.getNextEnemyToJumpSelectorTo(currentSelector.getFirstCombatantCoords());
                }
			}
			else if((Input.GetKey(KeyBindingList.moveNorthKey.getCurrentKeyCode()) || Input.GetKey(KeyBindingList.moveSouthKey.getCurrentKeyCode())) && 
                    canJumpBetweenAllyEnemySections())
			{
                if(onAllySide)
                {
				    targetPosition = CombatGrid.getNextEnemyToJumpSelectorTo(new GridCoords(0,0));
                } else
                {
				    targetPosition = CombatGrid.getNextAllyToJumpSelectorTo(new GridCoords(3,0));
                }
			}

			if (!targetPosition.Equals(GridCoords.getDefaultCoords()))
			{
				destroyPressEPrompt();

				currentSelector.setToLocation(targetPosition);

                AudioManager.playSelectorMovedSFX();

                declareSelectors();
			}
		}

        isMoving = false;
	}

    public static bool canJumpBetweenAllyEnemySections()
    {
        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingActor:
                return true;
            // case CurrentActivity.ChoosingLocation:
            //     AbilityMenuManager abilityMenuManager = AbilityMenuManager.getInstance();
            //     return abilityMenuManager != null && abilityMenuManager.getCurrentlySelectedAction() != null && 
            default: 
                return false;
        }
    }

    public static void movePlayerCursorToPlayerCharacter()
    {
        SelectorFactory.playerCursor.setToLocation(PartyManager.getPlayerStats().positions[0]);
    }

    public static void updateAllDamagePreviews()
    {
        DamagePreviewManager.wipeAllDamagePreviews();

        if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation && AbilityMenuManager.getInstance() != null)
        {
            DamagePreviewManager.UpdateDamagePreviews.Invoke(AbilityMenuManager.getInstance().getCurrentlySelectedAction());

            Selector hoverSelector = CombatHoverTileManager.getHoverSelector();

            if(hoverSelector != null)
            {
                CombatAction actionClone = AbilityMenuManager.getInstance().getCurrentlySelectedAction().clone();
                actionClone.setSelector(hoverSelector);

                DamagePreviewManager.UpdateDamagePreviews.Invoke(actionClone);
            }
        } 
    }

	private static bool canMoveUp()
	{
		if (currentSelector.upperBounds > 0 && currentSelector.onEnemySide())
		{
			return true;
		}

		if (currentSelector.upperBounds > 4 && currentSelector.onAllySide())
		{
			return true;
		}

		if (currentSelector.upperBounds > 0 && currentSelector.onAllySide() &&
			CombatStateManager.currentActivity == CurrentActivity.ChoosingActor)
		{
			return true;
		}

		return false;
	}

	private static bool canMoveDown()
	{
		if (currentSelector.lowerBounds < 3 && currentSelector.onEnemySide())
		{
			return true;
		}

		if (currentSelector.lowerBounds < CombatGrid.rowLowerBounds && currentSelector.onAllySide())
		{
			return true;
		}

		if (currentSelector.lowerBounds < CombatGrid.rowLowerBounds && currentSelector.onEnemySide() &&
			CombatStateManager.currentActivity == CurrentActivity.ChoosingActor)
		{
			return true;
		}

		return false;
	}

	private static bool canMoveLeft()
	{
		return currentSelector.leftBounds > 0;
	}

	private static  bool canMoveRight()
	{
		return currentSelector.rightBounds < CombatGrid.colRightBounds;
	}

	public bool moveWouldLeaveMandatoryTarget()
	{
		Selector testSelector = currentSelector.clone();

		if (Input.GetKey(KeyBindingList.moveNorthKey.getCurrentKeyCode()))
		{
			if (currentSelector.currentRow - 1 < CombatGrid.enemyRowUpperBounds)
			{
				return true;
			}

			testSelector.setToLocation(new GridCoords(currentSelector.currentRow - 1, currentSelector.currentCol));
		}
		else if (Input.GetKey(KeyBindingList.moveWestKey.getCurrentKeyCode()))
		{
			if (currentSelector.currentCol - 1 < CombatGrid.colLeftBounds)
			{
				return true;
			}

			testSelector.setToLocation(new GridCoords(currentSelector.currentRow, currentSelector.currentCol - 1));

		}
		else if (Input.GetKey(KeyBindingList.moveSouthKey.getCurrentKeyCode()))
		{
			if (currentSelector.currentRow + 1 > CombatGrid.enemyRowLowerBounds)
			{
				return true;
			}

			testSelector.setToLocation(new GridCoords(currentSelector.currentRow + 1, currentSelector.currentCol));
		}
		else if (Input.GetKey(KeyBindingList.moveEastKey.getCurrentKeyCode()))
		{
			if (currentSelector.currentCol + 1 > CombatGrid.colRightBounds)
			{
				return true;
			}

			testSelector.setToLocation(new GridCoords(currentSelector.currentRow, currentSelector.currentCol + 1));
		}

		return !testSelector.hasAtLeastOneMandatoryTarget();
	}

	public static GridCoords findLegalCoordsContainingMandatoryTarget(Selector selector, Stats mandatoryTarget)
	{
		GridCoords targetCoords = mandatoryTarget.positions.Count > 0 ? mandatoryTarget.positions[0] : GridCoords.getDefaultCoords();
		return findLegalCoordsContainingMandatoryTarget(selector, targetCoords);
	}

	//only use after already placing the selector at the target's position and then want to adjust it to be inside the combat zone
	public static GridCoords findLegalCoordsContainingMandatoryTarget(Selector selector, GridCoords mandatoryPosition)
	{
		Selector cloneSelector = selector.clone();
		cloneSelector.setToLocation(mandatoryPosition.clone(), declareSelectors: false);

		if (cloneSelector.allTilesAreLegal() && cloneSelector.containsTarget(mandatoryPosition))
		{
			return new GridCoords(cloneSelector.currentRow, cloneSelector.currentCol);
		}

		cloneSelector = selector.clone();

		for (int rowIndex = 0; rowIndex <= CombatGrid.rowLowerBounds; rowIndex++)
		{
			for (int colIndex = 0; colIndex <= CombatGrid.colRightBounds; colIndex++)
			{
				cloneSelector.setToLocation(new GridCoords(rowIndex, colIndex), declareSelectors: false);

				if (cloneSelector.allTilesAreLegal() && cloneSelector.containsTarget(mandatoryPosition))
				{
					return new GridCoords(cloneSelector.currentRow, cloneSelector.currentCol);
				}
			}
		}

		Debug.LogError("Could not find legal coords containing mandatory target.");

		if (CombatGrid.positionIsOnEnemySide(mandatoryPosition))
		{
			return new GridCoords(2, 2);
		}
		else
		{
			return new GridCoords(6, 2);
		}
	}

	public static void setCurrentSelector(Selector newSelector, bool declareSelector = true)
	{
		currentSelector = newSelector;

        if(declareSelector)
        {
            declareSelectors();
        }
	}

	public static void resetCurrentSelector()
	{
		setCurrentSelector(SelectorFactory.playerCursor);
	}

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateSelectorManager()
    {
        currentAbilityManager = null;
        currentSelector = null;
        instance = null;
        isMoving = false;
        heartBeatCount = 0;
        declareSelectorsQueued = false;
    }
}

public static class SelectionInfo
{
	public static bool selectionIsAlly(GridCoords coords)
	{
		return CombatGrid.combatantExistsAtCoords(coords, out Stats target) && 
                Helpers.tagMatchesCriteria(target.combatSprite, SelectorManager.allyTagCriteria);
	}

	public static bool selectionIsPartyMember(GridCoords coords)
	{
		return CombatGrid.combatantExistsAtCoords(coords, out Stats target) &&
                target.combatSprite.tag.Equals(LayerAndTagManager.playerTag);
	}

	public static bool selectedAllyCanAct(GridCoords coords)
	{
		return CombatGrid.combatantExistsAtCoords(coords, out Stats actor) &&
                actor.isAlive() && 
                !PlayerCombatActionManager.actorHasActionsInQueue(actor) 
                && PlayerCombatActionCounterManager.playerHasActionsLeft();
	}

}