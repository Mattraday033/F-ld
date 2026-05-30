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
        if(KeyBindingSettingsManager.listeningForKeyBinding() || InspectNode.inspecting)
        {
            return;
        }

		KeyPressManager.updateKeyBools();
        PlayerInput.showFormulaToggleCheck();

		if (KeyPressManager.handlingPrimaryKeyPress || (CombatStateManager.whoseTurn != WhoseTurn.Player && CombatStateManager.currentActivity != CurrentActivity.Tutorial))
		{
            if(CombatStateManager.whoseTurn == WhoseTurn.Resolving)
            {
                CombatStateManager.setTimeScale();
            }
			return;
		}

        if (Input.GetKey(KeyBindingList.showHideKeyBindingsListKey.getCurrentKeyCode()) && 
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

                if (Input.GetKey(KeyBindingList.combatDeselectKey.getCurrentKeyCode()) && 
                    PlayerCombatActionManager.playerHasActionsInQueue() && 
                    !KeyPressManager.handlingPrimaryKeyPress)
                {
                    PlayerCombatActionManager.removeLastCombatActionFromPlayerCombatActionQueue();
                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }

                if((Input.GetKey(KeyBindingList.combatSettingsScreenKey.getCurrentKeyCode()) || 
                    (Input.GetKey(KeyBindingList.combatDeselectKey.getCurrentKeyCode()) && !PlayerCombatActionManager.playerHasActionsInQueue())) && 
                    !KeyPressManager.handlingPrimaryKeyPress)
                {
                    escapeButton.spawnPopUp();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }

                if (Input.GetKey(KeyBindingList.resolveTurnKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    KeyPressManager.handlingPrimaryKeyPress = true;
                    CombatStateManager.resolveTurn();
                }

				if (Input.GetKey(KeyBindingList.combatSelectKey.getCurrentKeyCode()) && !SelectorManager.isMoving && !KeyPressManager.handlingPrimaryKeyPress)
				{
					SelectorManager.handleAllySelection();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				break;

			case CurrentActivity.ChoosingAbility:

				if (SelectorManager.hasCurrentlyVisibleAbilityManager() && Input.GetKey(KeyBindingList.combatDeselectKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
				{
					SelectorManager.deselectAlly();

					SelectorManager.displayCurrentHoverUI();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				if (Input.GetKey(KeyBindingList.combatSettingsScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    escapeButton.spawnPopUp();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }


				break;

			case CurrentActivity.ChoosingLocation:

				if (Input.GetKey(KeyBindingList.combatSelectKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
				{
					SelectorManager.handleChoosingLocation();
					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				if (SelectorManager.hasCurrentAbilityManager() && Input.GetKey(KeyBindingList.combatDeselectKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    SelectorManager.backOutOfAbilityMenu();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                }

				if (Input.GetKey(KeyBindingList.combatSettingsScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    escapeButton.spawnPopUp();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }

				break;

			case CurrentActivity.ChoosingTertiary:

				if (Input.GetKey(KeyBindingList.combatSelectKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
				{
					SelectorManager.handleChoosingTertiary();
					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				if (Input.GetKey(KeyBindingList.combatDeselectKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
				{
                    SelectorManager.backOutOfTertiaryLocationSelection();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

				if (Input.GetKey(KeyBindingList.combatSettingsScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    escapeButton.spawnPopUp();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }

				break;

			case CurrentActivity.InEscapeMenu:

                if(KeyBindingList.settingsScreenOrBackKeyPressed() && 
                    !KeyPressManager.handlingPrimaryKeyPress)
                {
                    EscapeStack.handleEscapePress();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }

                break;

			case CurrentActivity.Tutorial:

                TutorialSequenceInput.handleCombatTutorialInput();
                break;

            case CurrentActivity.ResolveActionWarning:

                if(KeyBindingList.settingsScreenOrBackKeyPressed() && 
                    !KeyPressManager.handlingPrimaryKeyPress)
                {
                    ResolveTurnWithNoActions.backOutOfCurrentDecision();
                    
                    EscapeStack.handleEscapePress();

                    KeyPressManager.handlingPrimaryKeyPress = true;
                    return;
                }

				if ((Input.GetKey(KeyBindingList.combatSelectKey.getCurrentKeyCode()) || 
                        Input.GetKey(KeyBindingList.resolveTurnKey.getCurrentKeyCode())) &&
                         !KeyPressManager.handlingPrimaryKeyPress)
				{
					ResolveTurnWithNoActions.executeCurrentDecision();

					KeyPressManager.handlingPrimaryKeyPress = true;
				}

                break;

			case CurrentActivity.Repositioning:
			case CurrentActivity.Retreating:

				break;
                
			default:
				throw new IOException("Unrecognized CurrentActivity: " + CombatStateManager.currentActivity.ToString());
		}
	}
}