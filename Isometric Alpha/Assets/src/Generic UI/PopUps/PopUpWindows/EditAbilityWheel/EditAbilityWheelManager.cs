using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EditAbilityWheelPopUpWindow : PopUpWindow, IActionArrayStorage
{
	public AbilityMenuManager abilityMenuManager;

	public TextMeshProUGUI weaponSlotTracker;
	public TextMeshProUGUI bonusDamageTracker;

	protected static EditAbilityWheelPopUpWindow instance;

	public static EditAbilityWheelPopUpWindow getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Duplicate instances of EditAbilityWheelPopUpWindow exist");
		}

		instance = this;

		abilityMenuManager.populateAbilityMenuFromCombatActionArray();
		abilityMenuManager.setToDisplayOnly();

		updateAcceptButtonInteractability();
	}

	public void updateAcceptButtonInteractability()
	{
		if (acceptButton != null)
		{
			acceptButton.interactable = canPlaceWeaponHere();
		}
	}

	public void setCloseButtonToUninteractable()
	{
		closeButton.interactable = false;
	}

	public bool canPlaceWeaponHere()
	{
		return abilityMenuManager.canPlaceWeaponHere();
	}

	public virtual void overWriteCurrentCombatActionArray()
	{
		AbilityMenuManager.OnAbilityWheelUpdate.Invoke();
	}

	public override void handleEscapePress()
	{
		if (OverallUIManager.currentScreenManager != null && !(OverallUIManager.currentScreenManager is null))
		{
			OverallUIManager.currentScreenManager.updateAllStatsPanels();
		}

		base.handleEscapePress();
	}

	public override void acceptButtonPress()
	{
		overWriteCurrentCombatActionArray();

		EscapeStack.handleEscapePress();
	}

	public override void closeButtonPress()
	{
		EscapeStack.handleEscapePress();
	}

	public void updateWeaponSlotTracker()
	{
		// weaponSlotTracker.text = Attack.getAmountOfWeaponCombatActions(abilityMenuManager) + "/" + State.playerStats.getWeaponSlots();
	}

	public CombatActionArray getStoredCombatActionArray()
	{
		return abilityMenuManager.getStoredCombatActionArray();
	}

	public int getMaximumNumberOfCombatActions()
	{
		return abilityMenuManager.getMaximumNumberOfCombatActions();
	}

	public virtual void setAbilityAtIndex(CombatAction combatAction, int abilityIndex)
	{
		abilityMenuManager.setAbilityAtIndex(combatAction, abilityIndex, false);

		updateWeaponSlotTracker();
	}

	public CombatAction getAbilityAt(int index)
	{
		return abilityMenuManager.getAbilityAt(index);
	}


	public void removeAbility(int abilityIndex)
	{
		setAbilityAtIndex(null, abilityIndex);
	}


	public bool canAddActionToArray(CombatAction combatAction)
	{
		if (combatAction == null)
		{
			return true;
		}

		// if (combatAction.takesAWeaponSlot() && !Attack.hasAvailableWeaponSlots(abilityMenuManager))
		// {
		// 	return false;
		// }

		return combatAction.hasAvailableSlots(abilityMenuManager);
	}

	public virtual void calculateWeaponSlotsAndBonusDamage()
	{
		// weaponSlotTracker.text = Attack.getAmountOfWeaponCombatActions(abilityMenuManager) + "/" + State.playerStats.getWeaponSlots();
		bonusDamageTracker.text = "+" + getStoredCombatActionArray().calculateBonusAbilityDamage();
	}
}
