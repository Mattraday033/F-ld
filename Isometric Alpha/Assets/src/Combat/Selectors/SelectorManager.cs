using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SelectorManager : MonoBehaviour 
{
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
	public Selector[] selectors;

	public PlayerCombatActionManager playerCombatActionManager;

    private const int heartBeatsToWait = 6;
	public static int heartBeatCount = 0;

	public static bool isMoving = false;

	public static Selector currentSelector; //selector that is currently being used. Left null in the unity inspector
	public static AbilityMenuManager currentAbilityManager; //circle of circles that shows abilities

	public HoverPanelPopUpButton hoverPanelPopUpButton;

	private readonly static RowColumnChangeDelegate rowDecrement = (r => r - 1);
	private readonly static RowColumnChangeDelegate rowIncrement = (r => r + 1);
	private readonly static RowColumnChangeDelegate colDecrement = (c => c - 1);
	private readonly static RowColumnChangeDelegate colIncrement = (c => c + 1);

	private static bool verticalPriority = true;

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

        CombatStateManager.OnActivityChangeToChoosingActor.AddListener(activateCurrentSelector);
        CombatStateManager.OnActivityChangeToChoosingAbility.AddListener(activateCurrentSelector);
        CombatStateManager.OnActivityChangeToChoosingLocation.AddListener(activateCurrentSelector);
        CombatStateManager.OnActivityChangeToChoosingTertiary.AddListener(activateCurrentSelector);

        CombatStateManager.OnActivityChangeToFinished.AddListener(deactivateCurrentSelector);

        HeartBeatManager.FastHeartBeat.AddListener(moveCurrentSelector);
    }

    private void OnDestroy()
    {
        CombatStateManager.OnTurnChangeToResolving.RemoveListener(setFirstSelectorVisibility);
        CombatStateManager.OnTurnChangeToPlayer.RemoveListener(setFirstSelectorVisibility);

        CombatStateManager.OnActivityChangeToChoosingActor.RemoveListener(activateCurrentSelector);
        CombatStateManager.OnActivityChangeToChoosingAbility.RemoveListener(activateCurrentSelector);
        CombatStateManager.OnActivityChangeToChoosingLocation.RemoveListener(activateCurrentSelector);
        CombatStateManager.OnActivityChangeToChoosingTertiary.RemoveListener(activateCurrentSelector);

        CombatStateManager.OnActivityChangeToFinished.RemoveListener(deactivateCurrentSelector);

        HeartBeatManager.FastHeartBeat.RemoveListener(moveCurrentSelector);
    }

    private void setFirstSelectorVisibility()
    {
        selectors[Constants.indexZero].getSelectorObject().SetActive(CombatStateManager.whoseTurn == WhoseTurn.Player);
    }

	public static SelectorManager getInstance()
	{
		return instance;
	}

	public void instantiateAllSelectors()
	{
		foreach (Selector selector in selectors)
		{
			selector.getSelectorObject().transform.SetParent(selectorParent);
			selector.setToStartLocation();
		}

		selectors[0].setToCurrentSelector();
		selectors[0].setToLocation(PartyManager.getPlayerStats().position);
	}


    public static void activateCurrentSelector()
    {
        if(currentSelector == null)
        {
            return;
        }

        currentSelector.SetActive(true);
    }

    public static void deactivateCurrentSelector()
    {
        if(currentSelector == null)
        {
            return;
        }

        currentSelector.SetActive(false);
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
        setCurrentSelector(instance.selectors[0]);

        currentSelector.setToOriginalColor();

        currentAbilityManager.enableAbilityButtonCanvas();
        CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingAbility);

        DamagePreviewManager.wipeAllDamagePreviews();

        displayCurrentHoverUI();
    }

    public static void backOutOfTertiaryLocationSelection()
    {
        currentSelector.setToOriginalColor();
        currentSelector.SetActive(false);

        CombatAction loadedCombatAction = currentAbilityManager.getCurrentlySelectedAction();

        currentSelector = loadedCombatAction.getSelector();
        currentSelector.SetActive(true);

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

		setCurrentSelector(instance.selectors[0], false);
	}

	public static void displayCurrentHoverUI()
	{
		if (!currentSelector.singleTile ||
			(!Flags.getFlag(TutorialSequenceList.combatTutorialSeenFlag) &&
			CombatStateManager.currentActivity != CurrentActivity.Tutorial))
		{
			instance.hoverPanelPopUpButton.destroyPopUp();
			return;
		}

        instance.hoverPanelPopUpButton.spawnPopUp();
	}

	public static void createPressEPrompt()
	{
		Stats target = CombatGrid.getCombatantAtCoords(getCurrentSelectorCoords());

		destroyPressEPrompt();

		if (target == null || CombatActionManager.actorAlreadyHasCombatAction(getCurrentSelectorCoords()) ||
			(CombatGrid.positionIsOnAlliedSide(getCurrentSelectorCoords()) && CombatActionManager.finishedChoosingPartyMemberCombatActions()))
		{
			return;
		}

		GameObject sprite = target.combatSprite;

		if (currentSelector.singleTile && Helpers.tagMatchesCriteria(sprite, allyTagCriteria))
		{
			if (CombatStateManager.currentActivity == CurrentActivity.ChoosingActor)
			{
				instance.pressEPrompt = Instantiate(Resources.Load<GameObject>(PrefabNames.combatPressEPrompt), sprite.transform.GetChild(sprite.transform.childCount - 1));
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
			return;
		}

		if (loadedCombatAction.targetsOnlyEmptySpace())
		{
			if (!currentSelector.hasAtLeastOneTarget(allyAndEnemyTagCriteria))
			{
				instance.finishChoosingLocation(loadedCombatAction);
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
						return;
					}

					instance.finishChoosingLocation(loadedCombatAction);
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
							return;
						}

						instance.finishChoosingLocation(loadedCombatAction);
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
				playerCombatActionManager.queueCombatAction(selectors[0], currentSelector, loadedCombatAction);
			}
			else
			{
				loadedCombatAction.performCombatAction();
			}

			resetAllSelectors();
			selectors[0].setToLocation(loadedCombatAction.getActorStats().position);

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

			setCurrentSelector(tertiarySelector);

			currentSelector.setToSecondaryColor();

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

		if (loadedCombatAction.tertiaryCoordsRequiresEmptySpace() && CombatGrid.getCombatantAtCoords(currentSelector.getCoords()) != null)
		{
			return;
		}

		loadedCombatAction.setTertiaryCoords(currentSelector.getCoords());

		if (loadedCombatAction.tertiaryCoordsRequiresEmptySpace())
		{
			if (!currentSelector.hasAtLeastOneTarget(allyAndEnemyTagCriteria))
			{
				currentSelector.setToOriginalColor();

				instance.finishChoosingTertiary(loadedCombatAction);
			}
		}
		else
		{
			if (currentSelector.hasAtLeastOneTarget(enemyTagCriteria))
			{
				currentSelector.setToOriginalColor();

				instance.finishChoosingTertiary(loadedCombatAction);
			}
		}

		CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
	}

	public void finishChoosingTertiary(CombatAction loadedCombatAction)
	{
		playerCombatActionManager.queueCombatActionWithTertiary(selectors[0], currentSelector, loadedCombatAction);

		resetAllSelectors();

		currentSelector.setToLocation(loadedCombatAction.getActorCoords());

		CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);

		DamagePreviewManager.wipeAllDamagePreviews();
	}

	//may extend to all party members or make another method to handle selecting friendly minions
	// public void handlePlayerSelection()
	// {
	// 	Stats currentTarget = CombatGrid.getCombatantAtCoords(currentSelector.getCoords());

	// 	if (currentTarget == null || combatActionManager.playerCombatActionChosen())
	// 	{
	// 		return;
	// 	}

	// 	if (currentTarget.combatSprite.tag.Equals(LayerAndTagManager.playerTag) &&
	// 	   Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !isMoving && !KeyPressManager.handlingPrimaryKeyPress)
	// 	{
	// 		if (CombatStateManager.choosingRepositionTarget())
	// 		{
	// 			RepositionManager.selectSingleAllyToMove(currentSelector.getCoords());

	// 			selectors[1].setToCurrentSelector();
	// 			currentSelector.setToLocation(selectors[0].getCoords());

	// 			KeyPressManager.handlingPrimaryKeyPress = true;
	// 		}
	// 		else
	// 		{
	// 			currentAbilityManager = currentTarget.combatSprite.GetComponent<AbilityMenuManager>();

	// 			if (!currentAbilityManager.enabled)
	// 			{
	// 				currentAbilityManager.enableAbilityButtonCanvas();

	// 				CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingAbility);

	// 				KeyPressManager.handlingPrimaryKeyPress = true;
	// 			}
	// 		}
	// 	}
	// }

	public void handlePartyMemberSelection()
	{
		Stats currentTarget = CombatGrid.getCombatantAtCoords(currentSelector.getCoords());

		if (currentTarget == null)
		{
			return;
		}

		if (currentTarget.combatSprite.tag.Equals(LayerAndTagManager.npcTag) &&
		   Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !isMoving && !KeyPressManager.handlingPrimaryKeyPress)
		{

			if (CombatActionManager.finishedChoosingPartyMemberCombatActions() ||
				CombatActionManager.actorAlreadyHasCombatAction(currentSelector.getCoords()) ||
					!CombatGrid.getCombatantAtCoords(currentSelector.getCoords()).isAlive())
			{
				return;
			}

			if (CombatStateManager.choosingRepositionTarget())
			{
				RepositionManager.currentSingleTargetRepositionCombatAction.setActorCoords(currentSelector.getCoords());
				RepositionManager.currentRepositionActivity = CurrentRepositionActivity.ChoosingNewLocation;

				selectors[1].setToCurrentSelector();
				currentSelector.setToLocation(selectors[0].getCoords());

				KeyPressManager.handlingPrimaryKeyPress = true;
			}
			else
			{
				currentAbilityManager = currentTarget.combatSprite.GetComponent<AbilityMenuManager>();

				if (!currentAbilityManager.enabled)
				{
					currentAbilityManager.enableAbilityButtonCanvas();

					CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingAbility);

					KeyPressManager.handlingPrimaryKeyPress = true;
				}
			}
		}
	}

	public static void handleAllySelection()
	{
		if (!SelectionInfo.selectedAllyCanAct(currentSelector.getCoords()))
		{
			return;
		}

		Stats currentTarget = CombatGrid.getCombatantAtCoords(currentSelector.getCoords());

		currentAbilityManager = currentTarget.combatSprite.GetComponent<AbilityMenuManager>();

		if (!currentAbilityManager.enabled)
		{
			currentAbilityManager.enableAbilityButtonCanvas();

			CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingAbility);
		}
	}

	//if statements listening for if the first selector should be deactivated or not.
	public void autoAdjustSelectorAvailability()
	{
		if (CombatStateManager.whoseTurn == WhoseTurn.Player)
		{
			selectors[0].SetActive(true);
		}
		else
		{
			selectors[0].SetActive(false);
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

        if(heartBeatCount < heartBeatsToWait && !KeyBindingList.jumpMoveKeyIsPressed())
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

		if (KeyBindingList.jumpMoveKeyIsPressed() &&
			currentSelector.singleTile &&
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

			if (Input.GetKey(KeyBindingList.moveNorthKey) && canMoveUp())
			{
				currentSelector.setToLocation(new GridCoords(currentSelector.currentRow - 1, currentSelector.currentCol));
				moved = true;

			}
			else if (Input.GetKey(KeyBindingList.moveSouthKey) && canMoveDown())
			{
				currentSelector.setToLocation(new GridCoords(currentSelector.currentRow + 1, currentSelector.currentCol));
				moved = true;

			}
			else if (Input.GetKey(KeyBindingList.moveWestKey) && canMoveLeft())
			{
				currentSelector.setToLocation(new GridCoords(currentSelector.currentRow, currentSelector.currentCol - 1));
				moved = true;

			}
			else if (Input.GetKey(KeyBindingList.moveEastKey) && canMoveRight())
			{
				currentSelector.setToLocation(new GridCoords(currentSelector.currentRow, currentSelector.currentCol + 1));
				moved = true;
			}

			updateCurrentSelectorPosition();
			
			if (moved)
			{
				destroyPressEPrompt();
			}
		}

        isMoving = false;

		if (moved)
		{
            updateAllDamagePreviews();

			displayCurrentHoverUI();

            declareSelectors();

            if(CombatStateManager.currentActivity == CurrentActivity.ChoosingActor)
            {
                createPressEPrompt();
            }
		}
	}

    public static void declareSelectors()
    {
        List<Selector> visibleSelectors = new List<Selector>();
        SelectorManager selectorManager = getInstance();

        if(selectorManager == null)
        {
            return;
        } else if(CombatStateManager.whoseTurn != WhoseTurn.Player)
        {
            SelectorMoved.Invoke(visibleSelectors);
            return;
        }

        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingLocation:
                break;
            default:
                visibleSelectors.Add(selectorManager.selectors[Constants.indexZero]);
                break;
        }

        if(currentSelector != selectorManager.selectors[Constants.indexZero])
        {
            visibleSelectors.Add(currentSelector);
        }

        SelectorMoved.Invoke(visibleSelectors);
        updateAllDamagePreviews();
    }

	public static void moveCurrentSelectorToNextSingleTileTarget()
	{
		if (isMoving)
		{
			GridCoords targetPosition = GridCoords.getDefaultCoords();
			bool dontWrap = true;

			if (Input.GetKey(KeyBindingList.moveSouthKey))
			{
				verticalPriority = true;

				targetPosition = searchColForCombatant(currentSelector.getCoords().col, currentSelector.getCoords().row + 1, rowIncrement, dontWrap);

				if (targetPosition.Equals(GridCoords.getDefaultCoords()))
				{
					targetPosition = findNextSingleTileTarget(rowIncrement, colDecrement);
				}

			}
			else if (Input.GetKey(KeyBindingList.moveWestKey))
			{
				verticalPriority = false;

				targetPosition = searchRowForCombatant(currentSelector.getCoords().row, currentSelector.getCoords().col - 1, colDecrement, dontWrap);

				if (targetPosition.Equals(GridCoords.getDefaultCoords()))
				{
					targetPosition = findNextSingleTileTarget(rowIncrement, colDecrement);
				}

			}
			else if (Input.GetKey(KeyBindingList.moveEastKey))
			{
				verticalPriority = false;

				targetPosition = searchRowForCombatant(currentSelector.getCoords().row, currentSelector.getCoords().col + 1, colIncrement, dontWrap);

				if (targetPosition.Equals(GridCoords.getDefaultCoords()))
				{
					targetPosition = findNextSingleTileTarget(rowDecrement, colIncrement);
				}

			}
			else
			{
				verticalPriority = true;

				targetPosition = searchColForCombatant(currentSelector.getCoords().col, currentSelector.getCoords().row - 1, rowDecrement, dontWrap);

				if (targetPosition.Equals(GridCoords.getDefaultCoords()))
				{
					targetPosition = findNextSingleTileTarget(rowDecrement, colIncrement);
				}
			}

			if (!targetPosition.Equals(GridCoords.getDefaultCoords()))
			{
				destroyPressEPrompt();

				currentSelector.setToLocation(targetPosition);

                updateAllDamagePreviews();
                declareSelectors();
			}

			updateCurrentSelectorPosition();
		}

        isMoving = false;
	}

    public static void updateAllDamagePreviews()
    {
        DamagePreviewManager.wipeAllDamagePreviews();

        if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation && AbilityMenuManager.getInstance() != null)
        {
            DamagePreviewManager.UpdateDamagePreviews.Invoke(AbilityMenuManager.getInstance().getCurrentlySelectedAction());
        } 
    }

	private static GridCoords findNextSingleTileTarget(RowColumnChangeDelegate rowChange, RowColumnChangeDelegate colChange)
	{
		RowColumnChangeDelegate loopChange;
		RowColumnChangeDelegate searchChange;
		int loopIndex;
		int searchStartIndex;

		if (verticalPriority)
		{
			loopChange = rowChange;
			searchChange = colChange;
			loopIndex = currentSelector.currentRow;
			searchStartIndex = currentSelector.currentCol;
		}
		else
		{
			loopChange = colChange;
			searchChange = rowChange;
			loopIndex = currentSelector.currentCol;
			searchStartIndex = currentSelector.currentRow;
		}

		GridCoords targetPosition = GridCoords.getDefaultCoords();

		int startIndex = loopIndex;
		int searchesRan = 0;
		bool wrap = false;

		for (loopIndex = loopChange(loopIndex); searchesRan <= CombatGrid.colRightBounds; loopIndex = loopChange(loopIndex))
		{
			if (loopIndex == startIndex)
			{
				searchStartIndex++; //so that the place the currentSelector is at is the lowest priority
			}

			if (verticalPriority)
			{
				loopIndex = checkForWrapAroundRow(loopIndex);
				targetPosition = searchRowForCombatant(loopIndex, searchStartIndex, searchChange, wrap);
			}
			else
			{
				loopIndex = checkForWrapAroundCol(loopIndex);
				targetPosition = searchColForCombatant(loopIndex, searchStartIndex, searchChange, wrap);
			}

			if (!targetPosition.Equals(GridCoords.getDefaultCoords()))
			{
				return targetPosition;
			}

			searchesRan++;
		}

		return GridCoords.getDefaultCoords();
	}

	private delegate int RowColumnChangeDelegate(int rowOrColumn);

	private static GridCoords searchRowForCombatant(int rowIndex, int startCol, RowColumnChangeDelegate colChange, bool dontWrap)
	{
		int columnsSearched = 0;
		for (int colIndex = startCol; columnsSearched <= CombatGrid.colRightBounds; colIndex = colChange(colIndex))
		{
			int previousColIndex = colIndex;
			colIndex = checkForWrapAroundCol(colIndex);

			if (dontWrap && colIndex != previousColIndex)
			{
				return GridCoords.getDefaultCoords();
			}

			Stats combatantAtCoords = CombatGrid.getCombatantAtCoords(rowIndex, colIndex);
			Stats mandatoryTarget = CombatGrid.enemyHasMandatoryTarget();

			if (combatantAtCoords != null && combatantAtCoords.isAlive() && !Helpers.hasQuality<Trait>(combatantAtCoords.traitContainer, hT => hT.isUntargetable()) &&
				(currentSelector == instance.selectors[0] || (mandatoryTarget == null || (mandatoryTarget != null && combatantAtCoords.isMandatoryTarget()))))
			{
				return new GridCoords(rowIndex, colIndex);
			}

			columnsSearched++;
		}

		return GridCoords.getDefaultCoords();
	}

	private static GridCoords searchColForCombatant(int colIndex, int startRow, RowColumnChangeDelegate rowChange, bool dontWrap)
	{
		int rowsSearched = 0;
		for (int rowIndex = startRow; rowsSearched <= CombatGrid.colRightBounds; rowIndex = rowChange(rowIndex))
		{
			int previousRowIndex = rowIndex;
			rowIndex = checkForWrapAroundRow(rowIndex);

			if (dontWrap && rowIndex != previousRowIndex)
			{
				return GridCoords.getDefaultCoords();
			}

			Stats combatantAtCoords = CombatGrid.getCombatantAtCoords(rowIndex, colIndex);
			Stats mandatoryTarget = CombatGrid.enemyHasMandatoryTarget();

			if (combatantAtCoords != null && combatantAtCoords.isAlive() && !Helpers.hasQuality<Trait>(combatantAtCoords.traitContainer, hT => hT.isUntargetable()) &&
				(currentSelector == instance.selectors[0] || (mandatoryTarget == null || (mandatoryTarget != null && combatantAtCoords.isMandatoryTarget()))))
			{
				return new GridCoords(rowIndex, colIndex);
			}

			rowsSearched++;
		}

		return GridCoords.getDefaultCoords();
	}

	private static int checkForWrapAroundCol(int colIndex)
	{
		if (colIndex < CombatGrid.colLeftBounds)
		{
			return CombatGrid.colRightBounds;
		}
		else if (colIndex > CombatGrid.colRightBounds)
		{
			return CombatGrid.colLeftBounds;
		}

		return colIndex;
	}

	private static int checkForWrapAroundRow(int rowIndex)
	{
		if (verticalPriority && currentSelector == instance.selectors[0])
		{
			if (rowIndex < CombatGrid.rowUpperBounds)
			{
				rowIndex = CombatGrid.rowLowerBounds;
			}
			else if (rowIndex > CombatGrid.rowLowerBounds)
			{
				rowIndex = CombatGrid.rowUpperBounds;
			}
		}
		else
		{
			if (currentSelector.onEnemySide())
			{
				if (rowIndex < CombatGrid.enemyRowUpperBounds)
				{
					rowIndex = CombatGrid.enemyRowLowerBounds;
				}
				else if (rowIndex > CombatGrid.enemyRowLowerBounds)
				{
					rowIndex = CombatGrid.enemyRowUpperBounds;
				}

			}
			else if (currentSelector.onAllySide())
			{
				if (rowIndex < CombatGrid.allyRowUpperBounds)
				{
					rowIndex = CombatGrid.allyRowLowerBounds;
				}
				else if (rowIndex > CombatGrid.allyRowLowerBounds)
				{
					rowIndex = CombatGrid.allyRowUpperBounds;
				}
			}
		}

		return rowIndex;
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
		bool dontMoveGameObject = false;

		if (Input.GetKey(KeyBindingList.moveNorthKey))
		{
			if (currentSelector.currentRow - 1 < CombatGrid.enemyRowUpperBounds)
			{
				return true;
			}

			testSelector.setToLocation(new GridCoords(currentSelector.currentRow - 1, currentSelector.currentCol), dontMoveGameObject);
		}
		else if (Input.GetKey(KeyBindingList.moveWestKey))
		{
			if (currentSelector.currentCol - 1 < CombatGrid.colLeftBounds)
			{
				return true;
			}

			testSelector.setToLocation(new GridCoords(currentSelector.currentRow, currentSelector.currentCol - 1), dontMoveGameObject);

		}
		else if (Input.GetKey(KeyBindingList.moveSouthKey))
		{
			if (currentSelector.currentRow + 1 > CombatGrid.enemyRowLowerBounds)
			{
				return true;
			}

			testSelector.setToLocation(new GridCoords(currentSelector.currentRow + 1, currentSelector.currentCol), dontMoveGameObject);
		}
		else if (Input.GetKey(KeyBindingList.moveEastKey))
		{
			if (currentSelector.currentCol + 1 > CombatGrid.colRightBounds)
			{
				return true;
			}

			testSelector.setToLocation(new GridCoords(currentSelector.currentRow, currentSelector.currentCol + 1), dontMoveGameObject);
		}

		return !testSelector.hasAtLeastOneMandatoryTarget();
	}

	//puts all selectors back to their start positions, 
	//and disables all of them and enables the first one (selectors[0])
	//sets selectors[0] as currentSelector
	public void resetAllSelectors()
	{
		foreach (Selector selector in selectors)
		{
			selector.setToStartLocation();
			selector.SetActive(false);
		}

		selectors[0].setToCurrentSelector();
	}

	public static void updateCurrentSelectorPosition()
	{
		currentSelector.getSelectorObject().transform.position = CombatGrid.getPositionAt(currentSelector.currentRow, currentSelector.currentCol);
		Helpers.updateGameObjectPosition(currentSelector.getSelectorObject());
	}

	public static GridCoords findLegalCoordsContainingMandatoryTarget(Selector selector, Stats mandatoryTarget)
	{
		return findLegalCoordsContainingMandatoryTarget(selector, mandatoryTarget.position);
	}

	//only use after already placing the selector at the target's position and then want to adjust it to be inside the combat zone
	public static GridCoords findLegalCoordsContainingMandatoryTarget(Selector selector, GridCoords mandatoryPosition)
	{
		Selector cloneSelector = selector.clone();
		cloneSelector.setToLocation(mandatoryPosition.clone());

		if (cloneSelector.allTilesAreLegal() && cloneSelector.containsTarget(mandatoryPosition))
		{
			return new GridCoords(cloneSelector.currentRow, cloneSelector.currentCol);
		}

		cloneSelector = selector.clone();

		for (int rowIndex = 0; rowIndex <= CombatGrid.rowLowerBounds; rowIndex++)
		{
			for (int colIndex = 0; colIndex <= CombatGrid.colRightBounds; colIndex++)
			{
				cloneSelector.setToLocation(new GridCoords(rowIndex, colIndex));

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

	public static void setCurrentSelector(Selector newSelector)
	{
		setCurrentSelector(newSelector, true);
	}

	public static void setCurrentSelector(Selector newSelector, bool deactivatePreviousSelector)
	{
		currentSelector.SetActive(!deactivatePreviousSelector);
		currentSelector = newSelector;
		currentSelector.SetActive(true);
	}

	public static void resetCurrentSelector()
	{
		currentSelector.SetActive(false);
		currentSelector = getInstance().selectors[0];
	}

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateSelectorManager()
    {
        currentAbilityManager = null;
        currentSelector = null;
        instance = null;
        verticalPriority = true;
        isMoving = false;
        heartBeatCount = 0;
    }
}

public static class SelectionInfo
{
	public static bool selectionIsAlly(GridCoords coords)
	{
		Stats target = CombatGrid.getCombatantAtCoords(coords);

		return target != null && Helpers.tagMatchesCriteria(target.combatSprite, SelectorManager.allyTagCriteria);
	}
	public static bool selectionIsPlayer(GridCoords coords)
	{
		Stats target = CombatGrid.getCombatantAtCoords(coords);

		return target != null && target.combatSprite.tag.Equals(LayerAndTagManager.playerTag);
	}

	public static bool selectionIsPartyMember(GridCoords coords)
	{
		Stats target = CombatGrid.getCombatantAtCoords(coords);

		return target != null && target.combatSprite.tag.Equals(LayerAndTagManager.npcTag);
	}

	public static bool selectedAllyCanAct(GridCoords coords)
	{
        Stats actor = CombatGrid.getCombatantAtCoords(coords);

		return selectionIsAlly(coords) && actor.isAlive() && !PlayerCombatActionManager.actorHasActionsInQueue(actor) && PlayerCombatActionCounterManager.playerHasActionsLeft();
	}

}