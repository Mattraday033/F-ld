using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        newKeyCode = KeyCode.None;
        currentKeyIndex = default;
        keybindToOverwrite = null;
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

        movementKeySection.createKeybindButtons("Movement",KeyBindingList.getMovementKeybindSection());

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

            if((!CombatStateManager.inCombat && EscapeStack.getEscapableObjectsCount() > 0) || 
                (CombatStateManager.inCombat && EscapeStack.getEscapableObjectsCount() > 2))
            {
                clickToExitMessage.SetActive(false);   

                if(KeyBindingList.continueUIKeyIsPressed())
                {
                    BinaryDescisionPanel.AcceptBinaryDecision.Invoke();
                    KeyPressManager.handlingPrimaryKeyPress = true;
                } else if(KeyBindingList.settingsScreenOrBackKeyPressed())
                {
                    EscapeStack.handleEscapePress();
                    exitListeningModeWithoutChange();
                    KeyPressManager.handlingPrimaryKeyPress = true;
                }

                return;
            } else if(Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            {
                exitListeningModeWithoutChange();
            }

            KeyCode newKey = KeyCode.None;

            if(Input.GetKey(KeyCode.LeftAlt))
            {
                newKey = KeyCode.LeftAlt;
            }
            else if(Input.GetKey(KeyCode.RightAlt))
            {
                newKey = KeyCode.RightAlt;
            }
            else if(Input.GetKey(KeyCode.LeftShift))
            {
                newKey = KeyCode.LeftShift;
            }
            else if(Input.GetKey(KeyCode.RightShift))
            {
                newKey = KeyCode.RightShift;
            }
            else if(Input.GetKey(KeyCode.LeftControl))
            {
                newKey = KeyCode.LeftControl;
            }
            else if(Input.GetKey(KeyCode.RightControl))
            {
                newKey = KeyCode.RightControl;
            } else if(Input.GetKey(KeyCode.CapsLock))
            {
                newKey = KeyCode.CapsLock;
            } else if(Input.GetKey(KeyCode.Tab))
            {
                newKey = KeyCode.Tab;
            } else if(Input.GetKey(KeyCode.Escape))
            {
                newKey = KeyCode.Escape;
            } else if(Input.GetKey(KeyCode.UpArrow))
            {
                newKey = KeyCode.UpArrow;
            } else if(Input.GetKey(KeyCode.DownArrow))
            {
                newKey = KeyCode.DownArrow;
            } else if(Input.GetKey(KeyCode.LeftArrow))
            {
                newKey = KeyCode.LeftArrow;
            } else if(Input.GetKey(KeyCode.RightArrow))
            {
                newKey = KeyCode.RightArrow;
            } else if(Input.inputString.Length > 0)
            {
                newKey = (KeyCode) Input.inputString.ToLower()[0];
            }

            if(newKey != KeyCode.None)
            {
                newKeyCode = newKey;
                endListening();
            }
        } else
        {
            clickToExitMessage.SetActive(false);   
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
    }

    public void spawnReturnToDefaultsPopUp()
    {
        defaultResetPopUpButton.spawnPopUp();
    }

    public void spawnOverwritePopUp()
    {
        overwriteKeyBindPopUpButton.spawnPopUp();
    }

}
