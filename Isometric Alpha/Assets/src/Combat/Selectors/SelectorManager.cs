using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class SelectorManager : MonoBehaviour 
{
	public static string[] allyTagCriteria = new string[]{LayerAndTagManager.playerTag,
															LayerAndTagManager.npcTag};

	public static string[] enemyTagCriteria = new string[]{LayerAndTagManager.playerTag,
															LayerAndTagManager.enemyTag,
															LayerAndTagManager.npcTag};

	public static string[] allyAndEnemyTagCriteria = new string[]{LayerAndTagManager.playerTag,
														  			LayerAndTagManager.enemyTag,
														  			LayerAndTagManager.npcTag,
														  			LayerAndTagManager.placeHolderTag};

	private GameObject pressEPrompt;

	public Transform selectorParent;
	public Selector[] selectors;

	public PlayerCombatActionManager playerCombatActionManager;
	public CombatActionManager combatActionManager;

	public CombatStateManager combatStateManager;

	public int frameCount = 0;

	public bool isMoving = false;

	public GameObject exampleSelector; //testing tool to see current selector

	public static Selector currentSelector; //selector that is currently being used. Left null in the unity inspector
	public static AbilityMenuManager currentAbilityManager; //circle of circles that shows abilities

	public HoverPanelPopUpButton hoverPanelPopUpButton;

	private Stats previousHoverTarget;
	private bool playerActed = false;

	public Selector testSelector;

	private static RowColumnChangeDelegate rowDecrement = (r => r - 1);
	private static RowColumnChangeDelegate rowIncrement = (r => r + 1);
	private static RowColumnChangeDelegate colDecrement = (c => c - 1);
	private static RowColumnChangeDelegate colIncrement = (c => c + 1);

	private static bool verticalPriority = true;

	private static SelectorManager instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Found more than one Selector Manager in the scene.");
		}

		instance = this;
		hoverPanelPopUpButton = new HoverPanelPopUpButton();
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

	// Update is called once per frame
	void Update()   //here for Key Input
	{
		KeyPressManager.updateKeyBools();

		if (KeyPressManager.handlingPrimaryKeyPress || CombatStateManager.whoseTurn != WhoseTurn.Player)
		{
			return;
		}

		switch (CombatStateManager.currentActivity)
		{
			case CurrentActivity.Waiting:

				return;

			case CurrentActivity.ChoosingActor:

				currentSelector.SetActive(true);

				moveCurrentSelector();

				if (Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !isMoving && !KeyPressManager.handlingPrimaryKeyPress)
				{
					handleAllySelection();

					// handlePlayerSelection();
					// handlePartyMemberSelection();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				break;

			case CurrentActivity.ChoosingAbility:

				currentSelector.SetActive(true);

				if (currentAbilityManager != null && currentAbilityManager.enabled && KeyBindingList.eitherBackoutKeyIsPressed())
				{
					deselectAlly();

					displayHoverUIForCurrentSelectorTarget();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				break;

			case CurrentActivity.ChoosingLocation:

				currentSelector.SetActive(true);

				if (!currentSelector.selfTargeting)
				{
					moveCurrentSelector();
				}

				if (Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !KeyPressManager.handlingPrimaryKeyPress)
				{
					handleChoosingLocation();
					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				if (currentAbilityManager != null && !currentAbilityManager.enabled && KeyBindingList.eitherBackoutKeyIsPressed())
					{
						setCurrentSelector(selectors[0]);
						previousHoverTarget = null;

						currentSelector.setToOriginalColor();

						currentAbilityManager.enableAbilityButtonCanvas();
						CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingAbility);

						DamagePreviewManager.resetAllDamagePreviews();

						displayHoverUIForCurrentSelectorTarget();

						KeyPressManager.handlingPrimaryKeyPress = true;
					}

				break;

			case CurrentActivity.ChoosingTertiary:

				currentSelector.SetActive(true);

				if (CombatStateManager.currentActivity == CurrentActivity.ChoosingTertiary && !currentSelector.selfTargeting)
				{
					moveCurrentSelector();
				}

				if (Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !KeyPressManager.handlingPrimaryKeyPress)
				{
					handleChoosingTertiary();
					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				if (currentAbilityManager != null && !currentAbilityManager.enabled && KeyBindingList.eitherBackoutKeyIsPressed())
				{
					currentSelector.setToOriginalColor();
					currentSelector.SetActive(false);

					CombatAction loadedCombatAction = currentAbilityManager.getCurrentlySelectedLoadedCombatAction();

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

					displayHoverUIForCurrentSelectorTarget();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				break;

			case CurrentActivity.Repositioning:

				currentSelector.SetActive(true);

				moveCurrentSelector();

				if (RepositionManager.currentRepositionActivity == CurrentRepositionActivity.ChoosingRepositionTarget)
				{
					// handlePlayerSelection();
					handlePartyMemberSelection();
				}

				if (RepositionManager.currentRepositionActivity == CurrentRepositionActivity.ChoosingNewLocation &&
					RepositionManager.currentRepositionType == CurrentRepositionType.SingleAlly)
				{
					handleChoosingLocation();
				}

				break;
			case CurrentActivity.Finished:

				currentSelector.SetActive(false);

				break;

			case CurrentActivity.Tutorial:

				// if(hoverPanelPopUpButton.getCurrentPopUpGameObject() != null)
				// {
				// 		hoverPanelPopUpButton.destroyPopUp();
				// }

				// break;
			case CurrentActivity.Retreating:

				break;
			default:
				throw new IOException("Unrecognized CurrentActivity: " + CombatStateManager.currentActivity.ToString());
		}
	}

	public static Selector getCurrentSelector()
	{
		return currentSelector;
	}

	public static GridCoords getCurrentSelectorCoords()
	{
		return currentSelector.getCoords();
	}

	public void deselectAlly()
	{
		currentAbilityManager.disableAbilityButtonCanvas();
		CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);

		setCurrentSelector(selectors[0], false);
	}

	public static void deselectCurrentAlly()
	{
		if (instance != null)
		{
			instance.deselectAlly();
		}
	}

	public static void displayHoverUIForCurrentSelectorTarget()
	{
		if (!currentSelector.singleTile ||
			(!Flags.getFlag(TutorialSequenceList.combatTutorialSeenFlag) &&
			CombatStateManager.currentActivity != CurrentActivity.Tutorial))
		{
			instance.hoverPanelPopUpButton.destroyPopUp();
			return;
		}

		Stats currentCombatant = CombatGrid.getCombatantAtCoords(currentSelector.getCoords());

		if (currentCombatant != null) //
		{
			if (instance.previousHoverTarget == currentCombatant)
			{
				return;
			}

			if (currentCombatant == null ||
			   currentCombatant.getName() == null ||
			   currentCombatant.getName().Length == 0 ||
			   Helpers.hasQuality<Trait>(currentCombatant.hiddenTraits, hT => hT.isUntargetable()))
			{
				instance.hoverPanelPopUpButton.destroyPopUp();
				return;
			}

			displayHoverUI(currentCombatant);


			createPressEPrompt();
		}
		else
		{
			instance.hoverPanelPopUpButton.destroyPopUp();
			instance.previousHoverTarget = null;
		}
	}

	public static void displayHoverUI(Stats stats)
	{
		if (stats != null && instance != null)
		{
			instance.previousHoverTarget = stats;
			instance.hoverPanelPopUpButton.spawnPopUp(stats);
		}
	}

	public static void createPressEPrompt()
	{
		Stats target = CombatGrid.getCombatantAtCoords(getCurrentSelectorCoords());

		destroyPressEPrompt();

		if (target == null || CombatActionManager.actorAlreadyHasCombatAction(getCurrentSelectorCoords()) ||
			(SelectionInfo.selectionIsPartyMember(getCurrentSelectorCoords()) && CombatActionManager.finishedChoosingPartyMemberCombatActions()))
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

	public void deactivateCombatantInfoUIHoverPanel()
	{
		hoverPanelPopUpButton.destroyPopUp();
		previousHoverTarget = null;
	}

	public static void handleChoosingLocation()
	{

		if (instance == null || instance.isMoving)
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
			loadedCombatAction = currentAbilityManager.getCurrentlySelectedLoadedCombatAction();
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
			DamagePreviewManager.resetAllDamagePreviews();

			previousHoverTarget = null;
			displayHoverUIForCurrentSelectorTarget();
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
		if (instance == null || instance.isMoving)
		{
			return;
		}

		CombatAction loadedCombatAction = currentAbilityManager.getCurrentlySelectedLoadedCombatAction();

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

		DamagePreviewManager.resetAllDamagePreviews();
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

	public void moveCurrentSelector()
	{
		if (KeyBindingList.movementKeyPressed())
		{
			isMoving = true;
		}

		if (KeyBindingList.jumpMoveKeyIsPressed() &&
			currentSelector.singleTile &&
			CombatStateManager.snappingToTargetDuringReposition())
		{
			moveCurrentSelectorToNextSingleTileTarget();

			displayHoverUIForCurrentSelectorTarget();
			return;
		}

		bool moved = false;

		if (isMoving && (frameCount % 150) == 0)
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

		if (KeyBindingList.noMovementKeyPressed())
		{
			frameCount = 0;
			isMoving = false;
		}
		else
		{
			frameCount++;
		}

		if (moved && CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation)
		{
			DamagePreviewManager.setUpDamagePreviews();
		}

		if (moved)
		{
			displayHoverUIForCurrentSelectorTarget();
		}
	}

	public void moveCurrentSelectorToNextSingleTileTarget()
	{
		if (isMoving && (frameCount % 150) == 0)
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
			else //if(Input.GetKey(KeyCode.W))
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

				if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation)
				{
					DamagePreviewManager.setUpDamagePreviews();
				}
			}

			updateCurrentSelectorPosition();
		}

		if (KeyBindingList.noMovementKeyPressed())
		{
			frameCount = 0;
			isMoving = false;
		}
		else
		{
			frameCount++;
		}
	}

	private GridCoords findNextSingleTileTarget(RowColumnChangeDelegate rowChange, RowColumnChangeDelegate colChange)
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
		int maxSearches;
		bool wrap = false;

		if (verticalPriority && currentSelector == selectors[0])
		{
			maxSearches = CombatGrid.colRightBounds;
		}
		else
		{
			maxSearches = CombatGrid.rowLowerBounds;
		}

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

	private GridCoords searchRowForCombatant(int rowIndex, int startCol, RowColumnChangeDelegate colChange, bool dontWrap)
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

			if (combatantAtCoords != null && combatantAtCoords.isAlive() && !Helpers.hasQuality<Trait>(combatantAtCoords.hiddenTraits, hT => hT.isUntargetable()) &&
				(currentSelector == selectors[0] || (mandatoryTarget == null || (mandatoryTarget != null && combatantAtCoords.isMandatoryTarget()))))
			{
				return new GridCoords(rowIndex, colIndex);
			}

			columnsSearched++;
		}

		return GridCoords.getDefaultCoords();
	}

	private GridCoords searchColForCombatant(int colIndex, int startRow, RowColumnChangeDelegate rowChange, bool dontWrap)
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

			if (combatantAtCoords != null && combatantAtCoords.isAlive() && !Helpers.hasQuality<Trait>(combatantAtCoords.hiddenTraits, hT => hT.isUntargetable()) &&
				(currentSelector == selectors[0] || (mandatoryTarget == null || (mandatoryTarget != null && combatantAtCoords.isMandatoryTarget()))))
			{
				return new GridCoords(rowIndex, colIndex);
			}

			rowsSearched++;
		}

		return GridCoords.getDefaultCoords();
	}

	private int checkForWrapAroundCol(int colIndex)
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

	private int checkForWrapAroundRow(int rowIndex)
	{
		if (verticalPriority && currentSelector == selectors[0])
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

	//private delegate int RowColumnChangeDelegate(int rowOrColumn);

	private Stats findNextSingleTileTarget(RowColumnChangeDelegate rowChange, RowColumnChangeDelegate colChange,
											int rowFirstIndex, int rowLastIndex, int colFirstIndex, int colLastIndex, bool verticalPriority)
	{
		int currentRow = 0;
		int currentCol = 0;

		if (verticalPriority)
		{
			currentRow = rowChange(currentSelector.currentRow);
			currentCol = currentSelector.currentCol;
		}
		else
		{
			currentRow = currentSelector.currentRow;
			currentCol = colChange(currentSelector.currentCol);
		}

		for (currentRow = rowChange(currentSelector.currentRow);
			currentRow != currentSelector.currentRow && currentCol != currentSelector.currentCol;
			currentRow = rowChange(currentRow))
		{
			for (currentCol = colChange(currentSelector.currentCol);
				currentRow != currentSelector.currentRow && currentCol != currentSelector.currentCol;
				currentCol = colChange(currentCol))
			{
				if (currentRow < rowFirstIndex)
				{
					currentRow = rowLastIndex;
				}
				else if (currentRow > rowLastIndex)
				{
					currentRow = rowFirstIndex;
				}

				if (currentCol < colFirstIndex)
				{
					currentCol = colLastIndex;
				}
				else if (currentCol > colLastIndex)
				{
					currentCol = colFirstIndex;
				}

				if (CombatGrid.getCombatantAtCoords(currentRow, currentCol) != null)
				{
					return CombatGrid.getCombatantAtCoords(currentRow, currentCol);
				}

				if (verticalPriority && currentRow != rowLastIndex)
				{
					break;
				}
			}
		}

		//if you don't find a new combatant, the selector shouldn't move
		return CombatGrid.getCombatantAtCoords(currentSelector.currentRow, currentSelector.currentCol);
	}

	private bool canMoveUp()
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

	private bool canMoveDown()
	{
		if (currentSelector.lowerBounds < 3 && currentSelector.onEnemySide())
		{
			return true;
		}

		if (currentSelector.lowerBounds < (CombatGrid.fullCombatGrid.Length - 1) && currentSelector.onAllySide())
		{
			return true;
		}

		if (currentSelector.lowerBounds < (CombatGrid.fullCombatGrid.Length - 1) && currentSelector.onEnemySide() &&
			CombatStateManager.currentActivity == CurrentActivity.ChoosingActor)
		{
			return true;
		}

		return false;
	}



	private bool canMoveLeft()
	{
		return currentSelector.leftBounds > 0;
	}

	private bool canMoveRight()
	{
		return currentSelector.rightBounds < (CombatGrid.fullCombatGrid[0].Length - 1);
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

	public void updateCurrentSelectorPosition()
	{
		currentSelector.getSelectorObject().transform.position = CombatGrid.fullCombatGrid[currentSelector.currentRow][currentSelector.currentCol];
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
		if (selectionIsAlly(coords) && CombatGrid.getCombatantAtCoords(coords).isAlive())
		{
			if (selectionIsPlayer(coords))
			{
				return !CombatActionManager.actorAlreadyHasCombatAction(coords);
			}
			else
			{
				return !CombatActionManager.finishedChoosingPartyMemberCombatActions() && !CombatActionManager.actorAlreadyHasCombatAction(coords);
			}

		} else
		{
			return false;
		}
	}

}