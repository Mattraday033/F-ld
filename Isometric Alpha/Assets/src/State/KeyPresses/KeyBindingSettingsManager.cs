using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using TMPro;


public delegate KeyCode SetKeybind(KeyCode code);

public class KeyBindingSettingsManager : MonoBehaviour
{

    public readonly static Dictionary<int, KeyBind> keyIndexDictionary = new Dictionary<int, KeyBind>();

    public readonly static UnityEvent ReturnAllKeybindsToDefault = new UnityEvent();    
    public readonly static UnityEvent<KeyCode, List<KeyBindType>> CheckForKeybindOverwrite = new UnityEvent<KeyCode, List<KeyBindType>>();
    public readonly static UnityEvent<KeyCode, List<KeyBindType>> SetToNone = new UnityEvent<KeyCode, List<KeyBindType>>();

    public readonly static UnityEvent UnassignedKeybindsCheck = new UnityEvent();    
    public readonly static UnityEvent ShowUnassignedKeybindsWarning = new UnityEvent();
    public readonly static UnityEvent EnableAllKeyBindButtons = new UnityEvent();    
    public readonly static UnityEvent<int> DisableAllKeyBindButtons = new UnityEvent<int>();

    public static KeyCode newKeyCode = KeyCode.None;
    public static int currentKeyIndex = default;

    public BinaryPanelPopUpButton defaultResetPopUpButton;
    public BinaryPanelPopUpButton overwriteKeyBindPopUpButton;

    public Transform scrollableArea;

    public static KeyBind keybindToOverwrite = null;

    public GameObject unassignedKeybindsWarning;
    public GameObject clickToExitMessage;

    private void Awake()
    {
        populateKeybindSections();

        ShowUnassignedKeybindsWarning.AddListener(showUnassignedKeybindsWarning);
        EnableAllKeyBindButtons.AddListener(checkForUnassignedKeybinds);

        checkForUnassignedKeybinds();
    }

    private void OnDisable()
    {
        stopKeyCapture();

        bool wasListening = listeningForKeyBinding();

        newKeyCode = KeyCode.None;
        currentKeyIndex = default;
        keybindToOverwrite = null;

        //Closing the screen while a bind button is armed is the third way out of listening mode. Without
        //this the overlay KeybindingButton.listenForKeyPress added would keep every action off for good.
        if (wasListening)
        {
            restoreInputAfterListening();
        }
    }

    private void OnDestroy()
    {
        ShowUnassignedKeybindsWarning.RemoveListener(showUnassignedKeybindsWarning);
        EnableAllKeyBindButtons.RemoveListener(checkForUnassignedKeybinds);
    }

    private void populateKeybindSections()
    {
        GameObject movementKeybindSectionGO = Instantiate(Resources.Load<GameObject>(PrefabNames.keybindSection), scrollableArea);
        KeybindingSection movementKeySection = movementKeybindSectionGO.GetComponent<KeybindingSection>();

        movementKeySection.createKeybindButtons("Movement", KeyBindingList.getMovementKeybindSection());

        GameObject overworldKeybindSectionGO = Instantiate(Resources.Load<GameObject>(PrefabNames.keybindSection), scrollableArea);
        KeybindingSection overworldKeySection = overworldKeybindSectionGO.GetComponent<KeybindingSection>();

        overworldKeySection.createKeybindButtons("Overworld", KeyBindingList.getOverworldKeybindSection());

        GameObject skillKeybindSectionGO = Instantiate(Resources.Load<GameObject>(PrefabNames.keybindSection), scrollableArea);
        KeybindingSection skillKeySection = skillKeybindSectionGO.GetComponent<KeybindingSection>();

        skillKeySection.createKeybindButtons("Skill", KeyBindingList.getSkillsKeybindSection());

        GameObject UIKeybindSectionGO = Instantiate(Resources.Load<GameObject>(PrefabNames.keybindSection), scrollableArea);
        KeybindingSection UIKeySection = UIKeybindSectionGO.GetComponent<KeybindingSection>();

        UIKeySection.createKeybindButtons("UI", KeyBindingList.getUIKeybindSection());

        GameObject combatKeybindSectionGO = Instantiate(Resources.Load<GameObject>(PrefabNames.keybindSection), scrollableArea);
        KeybindingSection combatKeySection = combatKeybindSectionGO.GetComponent<KeybindingSection>();

        combatKeySection.createKeybindButtons("Combat", KeyBindingList.getCombatKeybindSection());

        GameObject actionWheelKeybindSectionGO = Instantiate(Resources.Load<GameObject>(PrefabNames.keybindSection), scrollableArea);
        KeybindingSection actionWheelKeySection = actionWheelKeybindSectionGO.GetComponent<KeybindingSection>();

        actionWheelKeySection.createKeybindButtons("Action Wheel", KeyBindingList.getActionWheelKeybindSection());
    }

    void Update()
    {
        KeyPressManager.updateKeyBools();

        if(listeningForKeyBinding() && !KeyPressManager.handlingPrimaryKeyPress)
        {
            clickToExitMessage.SetActive(true);   

            //The overwrite pop-up is open. Its accept and back keys are the ConfirmingKeyBindOverwrite overlay.
            if((!CombatStateManager.inCombat && EscapeStack.getEscapableObjectsCount() > 0) ||
                (CombatStateManager.inCombat && EscapeStack.getEscapableObjectsCount() > 2))
            {
                clickToExitMessage.SetActive(false);
                return;
            } else if(InputReader.isHeld(KeyCode.Mouse0) || InputReader.isHeld(KeyCode.Mouse1))
            {
                exitListeningModeWithoutChange();
                return;
            }

            if(capturedKey != KeyCode.None)
            {
                newKeyCode = capturedKey;
                capturedKey = KeyCode.None;
                endListening();
            }
        } else
        {
            clickToExitMessage.SetActive(false);
        }

        if(listeningForKeyBinding())
        {
            startKeyCapture();
        } else
        {
            stopKeyCapture();
        }
    }

    //The key for a new binding comes from the Input System's any-button-press hook rather than typed text,
    //so it's the physical key (the same one the CustomInputActions will be bound to) and every key with a
    //KeyCode can be bound. The hook runs before Update; Update applies the key.
    private IDisposable keyCaptureSubscription = null;
    private KeyCode capturedKey = KeyCode.None;

    private void startKeyCapture()
    {
        if(keyCaptureSubscription == null)
        {
            capturedKey = KeyCode.None;
            keyCaptureSubscription = InputSystem.onAnyButtonPress.Call(onAnyButtonPress);
        }
    }

    private void stopKeyCapture()
    {
        if(keyCaptureSubscription != null)
        {
            keyCaptureSubscription.Dispose();
            keyCaptureSubscription = null;
        }

        capturedKey = KeyCode.None;
    }

    private void onAnyButtonPress(InputControl control)
    {
        //Mouse buttons end listening instead (in Update), and a key still held from the press that started
        //listening is ignored, as before
        if(!(control is KeyControl keyControl) || KeyPressManager.handlingPrimaryKeyPress)
        {
            return;
        }

        //a key pressed while the overwrite pop-up is open answers the pop-up, not the binding
        if(keybindToOverwrite != null)
        {
            return;
        }

        KeyCode keyCode = keyControl.keyCode.ToKeyCode();

        if(keyCode != KeyCode.None)
        {
            capturedKey = keyCode;
        }
    }

    public void checkForUnassignedKeybinds()
    {
        unassignedKeybindsWarning.SetActive(false);

        UnassignedKeybindsCheck.Invoke();
    }

    public void showUnassignedKeybindsWarning()
    {
        unassignedKeybindsWarning.SetActive(true);
    }

    public static bool listeningForKeyBinding()
    {
        return currentKeyIndex != default(int);
    }

    public static void exitListeningModeWithoutChange()
    {
        currentKeyIndex = default;
        newKeyCode = KeyCode.None;
        keybindToOverwrite = null;
        EnableAllKeyBindButtons.Invoke();

        restoreInputAfterListening();
    }

    //Takes off the overlays listening mode added (KeybindingButton.listenForKeyPress and the overwrite
    //pop-up), which puts back the current activity's actions.
    private static void restoreInputAfterListening()
    {
        PlayerStateManager.removeInputOverlay(InputOverlay.ConfirmingKeyBindOverwrite, refresh: false);
        PlayerStateManager.removeInputOverlay(InputOverlay.ListeningForKeyBind, refresh: false);
        PlayerStateManager.updateEnabledInputActions();
    }

    public void endListening()
    {
        KeyPressManager.handlingPrimaryKeyPress = true;
        KeyPressManager.handlingSecondaryKeyPress = true;

        CheckForKeybindOverwrite.Invoke(newKeyCode, keyIndexDictionary[currentKeyIndex].types);

        if(keybindToOverwrite == null)
        {
            setNewKey();            
        } else
        {
            spawnOverwritePopUp();
        }
    }

    public static void setNewKey()
    {
        keyIndexDictionary[currentKeyIndex].setCurrentKeyCode(newKeyCode);

        currentKeyIndex = default(int);
        newKeyCode = KeyCode.None;

        KeyPressManager.handlingPrimaryKeyPress = true;
        KeyPressManager.handlingSecondaryKeyPress = true;

        EnableAllKeyBindButtons.Invoke();

        restoreInputAfterListening();
    }

    public void spawnReturnToDefaultsPopUp()
    {
        defaultResetPopUpButton.spawnPopUp();
    }

    public void spawnOverwritePopUp()
    {
        overwriteKeyBindPopUpButton.spawnPopUp();

        PlayerStateManager.addInputOverlay(InputOverlay.ConfirmingKeyBindOverwrite);
    }

}
