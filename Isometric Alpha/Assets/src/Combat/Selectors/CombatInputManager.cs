using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//Combat's key presses are CustomInputActions now, enabled per CurrentActivity by PlayerStateManager.updateEnabledInputActions
//(PlayerInputList's combat sets). What's left here is the per-frame bookkeeping those handlers lean on.
public class CombatInputManager : MonoBehaviour
{
    public readonly static UnityEvent OnHideKeyBindingsList = new UnityEvent();

    private static CombatInputManager instance;

    private CombatEscapeMenuPopUpButton escapeButton;

    private void Awake()
    {
        instance = this;
        escapeButton = new CombatEscapeMenuPopUpButton();
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

    public static void spawnEscapeMenu()
    {
        if(instance != null)
        {
            instance.escapeButton.spawnPopUp();
        }
    }

	void Update()
	{
        //Leaving for the main menu ends combat a frame or more before this scene unloads
        if(!CombatStateManager.inCombat || KeyBindingSettingsManager.listeningForKeyBinding() || InspectNode.inspecting)
        {
            return;
        }

        //releases handlingPrimaryKeyPress once every key is up, which the combat handlers use to ignore a held press
		KeyPressManager.updateKeyBools();

        //the fast forward key only records whether it's held, so the time scale follows it each frame of the resolve
        if(CombatStateManager.whoseTurn == WhoseTurn.Resolving)
        {
            CombatStateManager.setTimeScale();
        }
	}
}
