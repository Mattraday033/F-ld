using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public interface ISaveChangesPromptable 
{
	public bool changesMade();

	public void bypassChangesMadePromptAndClose();
}

public class FullEditAbilityWheelPopUpWindow : EditAbilityWheelPopUpWindow, IEscapable, ISaveChangesPromptable
{
	public BinaryPanelPopUpButton discardChangesPopUpButton;
	public BinaryDescisionPanel discardChangesPopUp;

	public Transform dragAndDropParent;

	public ScrollableUIElement availableCombatActionsGrid;

	public Dictionary<string, Item> interimPocket;


	public Transform descriptionPanelParent;
	private DescriptionPanelBuilder currentDescriptionPanel;

	public static FullEditAbilityWheelPopUpWindow getFullEditInstance()
	{
		return (FullEditAbilityWheelPopUpWindow) instance;
	}

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Duplicate instances of EditAbilityWheelPopUpWindow exist");
		}

		instance = this;

		discardChangesPopUpButton = new BinaryPanelPopUpButton();
		
		populateInterimPocket();
		populateAbilityWheelEditor();
		populateAvailableAbilityWindow();
	}

	public void populateInterimPocket()
	{
		interimPocket = State.inventory.ToDictionary(entry => entry.Key,
											   entry => (Item)entry.Value.Clone());
	}

	public void populateAbilityWheelEditor()
	{
		abilityMenuManager.populateAbilityMenuFromCombatActionArray(OverallUIManager.getCurrentActionArray());

		calculateWeaponSlotsAndBonusDamage();

		populateAvailableAbilityWindow();
	}

	public void updateAbilityWheelEditor()
	{
		// weaponSlotTracker.text = Attack.getAmountOfWeaponCombatActions(abilityMenuManager) + "/" + State.playerStats.getWeaponSlots();
		// bonusDamageTracker.text = "+" + CombatActionArray.calculateBonusAbilityDamage(abilityMenuManager);

		populateAvailableAbilityWindow();
	}

	public void populateAvailableAbilityWindow()
	{
		ArrayList availableCombatActions = new ArrayList();

		availableCombatActions.Add(new FistAttack());
		availableCombatActions.AddRange(Inventory.getAllMainHandWeaponsInPocketAsCombatActions(interimPocket));

		// availableCombatActions.AddRange(AbilityList.getAllAvailableStrengthAbilities());
		// availableCombatActions.AddRange(AbilityList.getAllAvailableDexterityAbilities());
		// availableCombatActions.AddRange(AbilityList.getAllAvailableWisdomAbilities());
		// availableCombatActions.AddRange(AbilityList.getAllAvailableCharismaAbilities());

		availableCombatActions.AddRange(LessonManager.getAllAbilities());

		availableCombatActions.AddRange(Inventory.getAllUsableItemCombatActionsInPocket(interimPocket));

		availableCombatActionsGrid.populatePanels(removeUnequippableAbilities(availableCombatActions));
	}

	public ArrayList removeUnequippableAbilities(ArrayList listOfActions)
	{
		ArrayList allEquippableActionsInList = new ArrayList();

		foreach (CombatAction action in listOfActions)
		{
			//Debug.LogError(action.getName() + "'s getType() returns ("+action.getType()+") vs ("+ PassiveAbility.passiveAbilityType + ")");
			if (action != null && !action.getType().Equals(PassiveAbility.passiveAbilityType))
			{
				allEquippableActionsInList.Add(action);
			}
		}

		return allEquippableActionsInList;
	}

	public void revealDescriptionPanelSet(CombatAction actionToDescribe)
	{
		descriptionPanelSlot.setPrimaryDescribable(actionToDescribe);
	}

	private void destroyDescriptionPanel()
	{
		if (currentDescriptionPanel != null && !(currentDescriptionPanel is null))
		{
			Destroy(currentDescriptionPanel.gameObject);
			currentDescriptionPanel = null;
		}
	}

	public override void setAbilityAtIndex(CombatAction newCombatAction, int abilityIndex)
	{
		CombatAction actionAtIndex = abilityMenuManager.getAbilityAt(abilityIndex);

		if (newCombatAction != null)
		{

			if (!newCombatAction.hasAvailableSlots(abilityMenuManager) &&
				!((newCombatAction != null && newCombatAction.takesAWeaponSlot()) &&
				  (actionAtIndex != null && actionAtIndex.takesAWeaponSlot())))
			{
				return;
			}

			if (newCombatAction.getSourceItem() != null)
			{
				Item sourceItem = newCombatAction.getSourceItem();

				newCombatAction.setSourceItem(sourceItem.clone());

				if (sourceItem.removeFromInventoryWhenCreatingCombatAction())
				{
					Inventory.removeItem(sourceItem, sourceItem.getQuantity(), interimPocket);
				}
			}
		}

		if (actionAtIndex != null && actionAtIndex.getSourceItem() != null &&
			actionAtIndex.getSourceItem().removeFromInventoryWhenCreatingCombatAction())
		{
			Item sourceItem = actionAtIndex.getSourceItem();

			Inventory.addItem(sourceItem, interimPocket);
		}

		base.setAbilityAtIndex(newCombatAction, abilityIndex);

		updateAbilityWheelEditor();
	}

	public override void overWriteCurrentCombatActionArray()
	{
		base.overWriteCurrentCombatActionArray();

		// State.CombatActionArray = abilityMenuManager.getStoredCombatActionArray();
		State.inventory = interimPocket;
	}

	public bool changesMade()
	{
		// for (int actionIndex = 0; actionIndex < State.CombatActionArray.Length && actionIndex < abilityMenuManager.getMaximumNumberOfCombatActions(); actionIndex++)
		// {
		// 	CombatAction actionAtIndex = abilityMenuManager.getAbilityAt(actionIndex);

		// 	if (actionAtIndex == null && State.CombatActionArray[actionIndex] == null)
		// 	{
		// 		continue;
		// 	}

		// 	if ((actionAtIndex == null && State.CombatActionArray[actionIndex] != null) ||
		// 	   (actionAtIndex != null && State.CombatActionArray[actionIndex] == null))
		// 	{
		// 		return true;
		// 	}

		// 	if (!String.Equals(actionAtIndex.getKey(), State.CombatActionArray[actionIndex].getKey(), StringComparison.OrdinalIgnoreCase))
		// 	{
		// 		return true;
		// 	}
		// }

		return false;
	}

	public override void handleEscapePress()
	{
		if (discardChangesPopUp != null)
		{
			discardChangesPopUp.destroyWindow();
			discardChangesPopUp = null;
			return;
		} else if (changesMade() && discardChangesPopUp == null)
		{
			discardChangesPopUpButton.spawnPopUp(new ReturnToPopUp(this));

			discardChangesPopUp = discardChangesPopUpButton.getPopUpWindow() as BinaryDescisionPanel;
			return;
		}

		bypassChangesMadePromptAndClose();
	}

	public void bypassChangesMadePromptAndClose()
	{
		if (discardChangesPopUp != null)
		{
			discardChangesPopUp.destroyWindow();
			discardChangesPopUp = null;
		}

		destroyWindow();
		EscapeStack.removeAllNullObjectsFromStack();

		if (OverallUIManager.currentScreenManager != null && !(OverallUIManager.currentScreenManager is null))
		{
			OverallUIManager.currentScreenManager.updateAllStatsPanels();
		}
	}

	public override void acceptButtonPress()
	{
		overWriteCurrentCombatActionArray();

		base.acceptButtonPress();
	}
}
