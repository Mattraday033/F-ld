using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CombatInputManager : MonoBehaviour
{
    public readonly static UnityEvent OnHideKeyBindingsList = new UnityEvent();

    private CombatEscapeMenuPopUpButton escapeButton;

    private void Awake()
    {
        escapeButton = new CombatEscapeMenuPopUpButton();
    }

    // Update is called once per frame
	void Update()   //here for Key Input
	{
		KeyPressManager.updateKeyBools();

		if (KeyPressManager.handlingPrimaryKeyPress || CombatStateManager.whoseTurn != WhoseTurn.Player)
		{
			return;
		}

        PlayerInput.showFormulaToggleCheck();

        if (Input.GetKey(KeyBindingList.showHideKeyBindingsListKey) && 
            !KeyPressManager.handlingPrimaryKeyPress)
        {
            OnHideKeyBindingsList.Invoke();
            KeyPressManager.handlingPrimaryKeyPress = true;
            return;
        }

		switch (CombatStateManager.currentActivity)
		{
			case CurrentActivity.Waiting:

				return;

			case CurrentActivity.Finished:
			case CurrentActivity.ChoosingActor:

                if (Input.GetKey(KeyBindingList.backOutKey2) && 
                    PlayerCombatActionManager.playerHasActionsInQueue() && 
                    !KeyPressManager.handlingPrimaryKeyPress)
                {
                    PlayerCombatActionManager.removeLastCombatActionFromPlayerCombatActionQueue();
                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }

                if((Input.GetKey(KeyBindingList.backOutKey1) || 
                    (Input.GetKey(KeyBindingList.backOutKey2) && !PlayerCombatActionManager.playerHasActionsInQueue())) && 
                    !KeyPressManager.handlingPrimaryKeyPress)
                {
                    escapeButton.spawnPopUp();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }

                if (Input.GetKey(KeyBindingList.resolveTurnKey) && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    KeyPressManager.handlingPrimaryKeyPress = true;
                    CombatStateManager.resolveTurn();
                }

				if (Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !SelectorManager.isMoving && !KeyPressManager.handlingPrimaryKeyPress)
				{
					SelectorManager.handleAllySelection();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				break;

			case CurrentActivity.ChoosingAbility:

				if (SelectorManager.hasCurrentlyVisibleAbilityManager() && KeyBindingList.eitherBackoutKeyIsPressed())
				{
					SelectorManager.deselectAlly();

					SelectorManager.displayCurrentHoverUI();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				break;

			case CurrentActivity.ChoosingLocation:

				if (Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !KeyPressManager.handlingPrimaryKeyPress)
				{
					SelectorManager.handleChoosingLocation();
					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				if (SelectorManager.hasCurrentAbilityManager() && KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    SelectorManager.backOutOfAbilityMenu();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                }

				break;

			case CurrentActivity.ChoosingTertiary:

				if (Input.GetKey(KeyBindingList.combatAcceptChoiceKey) && !KeyPressManager.handlingPrimaryKeyPress)
				{
					SelectorManager.handleChoosingTertiary();
					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				if (SelectorManager.hasCurrentlyVisibleAbilityManager() && KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress)
				{
                    SelectorManager.backOutOfTertiaryLocationSelection();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				break;

			case CurrentActivity.InEscapeMenu:

                if(KeyBindingList.eitherBackoutKeyIsPressed() && 
                    !KeyPressManager.handlingPrimaryKeyPress)
                {
                    EscapeStack.handleEscapePress();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }

                break;
			case CurrentActivity.Repositioning:
			case CurrentActivity.Tutorial:
			case CurrentActivity.Retreating:

				break;
			default:
				throw new IOException("Unrecognized CurrentActivity: " + CombatStateManager.currentActivity.ToString());
		}
	}
}