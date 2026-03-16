using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyBind
{
    private static int nextKeybindIndex = 1;

    public int keybindIndex
    {
        get;
        private set;
    }

    private string title;

    private KeyCode currentKeyCode;
    private KeyCode defaultKeyCode;

    public List<KeyBindType> types
    {
        get;
        private set;
    }

    public KeyBind(string title, KeyCode defaultKeyCode, KeyBindType type = KeyBindType.Ignore, List<KeyBindType> types = null)
    {
        this.title = title;

        this.defaultKeyCode = defaultKeyCode;
        this.currentKeyCode = defaultKeyCode;

        if(types != null)
        {
            this.types = types;
        } else
        {
            this.types = new List<KeyBindType>();
        }

        if(type != KeyBindType.Ignore)
        {
            this.types.Add(type);
        }

        KeyBindingSettingsManager.ReturnAllKeybindsToDefault.AddListener(resetToDefault);
        KeyBindingSettingsManager.CheckForKeybindOverwrite.AddListener(declareKeybindAlreadyInUse);
        KeyBindingSettingsManager.UnassignedKeybindsCheck.AddListener(showWarningIfUnassigned);
        KeyBindingSettingsManager.SetToNone.AddListener(setToNone);

        keybindIndex = nextKeybindIndex++;
        KeyBindingSettingsManager.keyIndexDictionary[keybindIndex] = this;
    }

    public void declareKeybindAlreadyInUse(KeyCode code, List<KeyBindType> types)
    {
        foreach(KeyBindType type in types)
        {
            if(code == currentKeyCode && this.types.Contains(type) && 
            this != KeyBindingSettingsManager.keyIndexDictionary[KeyBindingSettingsManager.currentKeyIndex])
            {
                OverwriteKeybind.overwriteKeybindMessage = OverwriteKeybind.overwriteKeybindMessageStart + ToString() + OverwriteKeybind.overwriteKeybindMessageMiddle + title + OverwriteKeybind.overwriteKeybindMessageEnd;            
                KeyBindingSettingsManager.keybindToOverwrite = this;
                return;
            }
        }
    }

    public void setToNone(KeyCode code, List<KeyBindType> types)
    {
        foreach(KeyBindType type in types)
        {
            if(code == currentKeyCode && this.types.Contains(type) && 
            this != KeyBindingSettingsManager.keyIndexDictionary[KeyBindingSettingsManager.currentKeyIndex])
            {
                setCurrentKeyCode(KeyCode.None);
                return;
            }
        }
    }

    public void showWarningIfUnassigned()
    {
        if(currentKeyCode == KeyCode.None)
        {
            KeyBindingSettingsManager.ShowUnassignedKeybindsWarning.Invoke();
        }
    }

    public KeyCode getCurrentKeyCode()
    {
        return currentKeyCode;
    }

    public void setCurrentKeyCode(KeyCode newCode)
    {
        currentKeyCode = newCode;
    }

    public void resetToDefault()
    {
        currentKeyCode = defaultKeyCode;
    }

    public string getTitle()
    {
        return title + ":";
    }

    public override string ToString()
    {
        switch(currentKeyCode)
        {            
            case KeyCode.None:
                return "";
            case KeyCode.Escape:
                return "Esc";
            case KeyCode.LeftAlt:
            case KeyCode.LeftShift:
                return currentKeyCode.ToString().Replace("Left", "Left ");
            case KeyCode.RightAlt:
            case KeyCode.RightShift:
                return currentKeyCode.ToString().Replace("Right", "Right ");
            case KeyCode.LeftControl:
                return "Left Ctrl";
            case KeyCode.RightControl:
                return "Right Ctrl";
            case KeyCode.CapsLock:
                return "Caps Lock";            
            case KeyCode.BackQuote:
                return "`";
            case KeyCode.Alpha0:
            case KeyCode.Alpha1:
            case KeyCode.Alpha2:
            case KeyCode.Alpha3:
            case KeyCode.Alpha4:
            case KeyCode.Alpha5:
            case KeyCode.Alpha6:
            case KeyCode.Alpha7:
            case KeyCode.Alpha8:
            case KeyCode.Alpha9:
                return currentKeyCode.ToString().Replace("Alpha","");
            case KeyCode.LeftArrow:
            case KeyCode.UpArrow:
            case KeyCode.DownArrow:
            case KeyCode.RightArrow:
                return currentKeyCode.ToString().Replace("Arrow", "");
            default:
                return currentKeyCode.ToString();
        }
    }
}

public enum KeyBindType {
                            Ignore = 0,
                            Overworld = 1,
                            Dialogue = 2,
                            UI = 3,
                            Combat = 4,
                            ActionWheel = 5

                        }

public static class KeyBindingList
{
    #region Movement KeyBinds
    public readonly static KeyBind moveNorthKey = new KeyBind("Move North", KeyCode.W, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.Combat});
    public readonly static KeyBind moveWestKey = new KeyBind("Move West", KeyCode.A, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.Combat});
    public readonly static KeyBind moveSouthKey = new KeyBind("Move South", KeyCode.S, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.Combat});
    public readonly static KeyBind moveEastKey = new KeyBind("Move East", KeyCode.D, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.Combat});

    public static List<KeyBind> getMovementKeybindSection()
    {
        return new List<KeyBind>()
        {
            moveNorthKey, moveWestKey, moveSouthKey, moveEastKey
        };
    }

    public static bool movementKeyPressed()
    {
        return Input.GetKey(moveNorthKey.getCurrentKeyCode()) ||
                Input.GetKey(moveWestKey.getCurrentKeyCode()) ||
                Input.GetKey(moveSouthKey.getCurrentKeyCode()) ||
                Input.GetKey(moveEastKey.getCurrentKeyCode());
    }
    public static bool noMovementKeyPressed()
    {
        return !Input.GetKey(moveNorthKey.getCurrentKeyCode()) &&
                !Input.GetKey(moveWestKey.getCurrentKeyCode()) &&
                !Input.GetKey(moveSouthKey.getCurrentKeyCode()) &&
                !Input.GetKey(moveEastKey.getCurrentKeyCode());
    }

    #endregion

    #region Overworld Keys
    public readonly static KeyBind interactKey = new KeyBind("Interact", KeyCode.E, KeyBindType.Overworld);
    public readonly static KeyBind backOutKey = new KeyBind("Back", KeyCode.R, KeyBindType.Overworld);
    public readonly static KeyBind hideTerrainKey = new KeyBind("Hide Terrain", KeyCode.F, KeyBindType.Overworld);
    public readonly static KeyBind revealKey = new KeyBind("Reveal Interactable Objects", KeyCode.LeftShift, KeyBindType.Overworld);
    public readonly static KeyBind removePlacedCompanionMovableObjectKey = new KeyBind("Remove Object", KeyCode.Z, KeyBindType.Overworld);
    public readonly static KeyBind quicksaveKey = new KeyBind("Quick Save", KeyCode.Q, KeyBindType.Overworld);
    public readonly static KeyBind transcriptKey = new KeyBind("Open Transcript", KeyCode.T, KeyBindType.Overworld);   
    public readonly static KeyBind showHideKeyBindingsListKey = new KeyBind("Show/Hide Keybindings", KeyCode.Backspace, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.UI});   

    public static List<KeyBind> getOverworldKeybindSection()
    {
        return new List<KeyBind>()
        {
            interactKey, backOutKey, hideTerrainKey, revealKey, removePlacedCompanionMovableObjectKey, quicksaveKey, transcriptKey, showHideKeyBindingsListKey  
        };
    }

    #endregion

    #region Skill Keys
    public readonly static KeyBind skillKey = new KeyBind("Use Skill", KeyCode.Space, KeyBindType.Overworld);   
    public readonly static KeyBind cycleSkillAscendingKey = new KeyBind("Next Skill", KeyCode.Alpha1, KeyBindType.Overworld);   
    public readonly static KeyBind cycleSkillDescendingKey = new KeyBind("Previous Skill", KeyCode.Alpha2, KeyBindType.Overworld);   

    public static List<KeyBind> getSkillsKeybindSection()
    {
        return new List<KeyBind>()
        {
            skillKey, cycleSkillAscendingKey, cycleSkillDescendingKey
        };
    }

    #endregion

    #region UI Keybinds
    public readonly static KeyBind acceptKey = new KeyBind("Accept 1", KeyCode.Space, KeyBindType.UI);
    public readonly static KeyBind acceptInputKey = new KeyBind("Accept 2", KeyCode.Return, KeyBindType.UI);

    public readonly static KeyBind lastScreenKey = new KeyBind("Last Used Screen", KeyCode.Tab, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.UI});   
    public readonly static KeyBind characterScreenKey = new KeyBind("Character Screen", KeyCode.C, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.UI});   
    public readonly static KeyBind inventoryScreenKey = new KeyBind("Inventory Screen", KeyCode.I, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.UI});   
    public readonly static KeyBind partyScreenKey = new KeyBind("Party Screen", KeyCode.P, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.UI});   
    public readonly static KeyBind journalScreenKey = new KeyBind("Journal Screen", KeyCode.J, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.UI});   
    public readonly static KeyBind loadScreenKey = new KeyBind("Save/Load Screen", KeyCode.L, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.UI}); 
    public readonly static KeyBind settingsScreenKey = new KeyBind("Settings Screen", KeyCode.Escape, types: new List<KeyBindType>(){ KeyBindType.Overworld, KeyBindType.UI}); 

    public readonly static KeyBind moveLeftKey = new KeyBind("Left Screen", KeyCode.A, KeyBindType.UI);
    public readonly static KeyBind moveRightKey = new KeyBind("Right Screen", KeyCode.D, KeyBindType.UI);
    public readonly static KeyBind inspectKey = new KeyBind("Inspect", KeyCode.T, KeyBindType.UI);
    public readonly static KeyBind mapKey = new KeyBind("Open Local Map", KeyCode.M, KeyBindType.UI);
    public readonly static KeyBind worldMapKey = new KeyBind("Open World Map", KeyCode.N, KeyBindType.UI);
    public readonly static KeyBind showFormulaKey = new KeyBind("Show Formulas", KeyCode.LeftAlt, KeyBindType.UI);
    public readonly static KeyBind maxAmountKey = new KeyBind("Shop Max Amount", KeyCode.LeftShift, KeyBindType.UI);
    public readonly static KeyBind multiplyByTenAmountKey = new KeyBind("Shop Amount x10", KeyCode.LeftControl, KeyBindType.UI);

    public static List<KeyBind> getUIKeybindSection()
    {
        return new List<KeyBind>()
        {
            acceptKey, acceptInputKey, lastScreenKey, characterScreenKey, inventoryScreenKey, partyScreenKey, journalScreenKey, loadScreenKey, settingsScreenKey, moveLeftKey, moveRightKey, inspectKey, mapKey, worldMapKey, showFormulaKey, maxAmountKey, multiplyByTenAmountKey 
        };
    }
    #endregion

    #region Combat Keys
    public readonly static KeyBind combatSelectKey = new KeyBind("Select", KeyCode.E, KeyBindType.Combat);
    public readonly static KeyBind combatDeselectKey = new KeyBind("Deselect", KeyCode.R, KeyBindType.Combat);
    public readonly static KeyBind resolveTurnKey = new KeyBind("Resolve Turn", KeyCode.Space, KeyBindType.Combat);
    public readonly static KeyBind jumpMoveKey = new KeyBind("Jump Move", KeyCode.LeftShift, KeyBindType.Combat);
    public readonly static KeyBind combatSettingsScreenKey = new KeyBind("Settings Menu", KeyCode.Escape, KeyBindType.Combat); 
 
    public static List<KeyBind> getCombatKeybindSection()
    {
        return new List<KeyBind>()
        {
            combatSelectKey, combatDeselectKey, resolveTurnKey, jumpMoveKey, combatSettingsScreenKey
        };
    }

    #endregion

    #region Action Wheel

    public readonly static KeyBind moveCounterClockwiseKey = new KeyBind("Action Wheel Counter Clockwise", KeyCode.A, KeyBindType.ActionWheel);
    public readonly static KeyBind moveClockwiseKey  = new KeyBind("Action Wheel Clockwise", KeyCode.D, KeyBindType.ActionWheel);

    public static List<KeyBind> getActionWheelKeybindSection()
    {
        return new List<KeyBind>()
        {
            moveCounterClockwiseKey, moveClockwiseKey
        };
    }

    #endregion
    public static bool settingsScreenOrBackKeyPressed()
    {
        if(CombatStateManager.inCombat)
        {
            return Input.GetKey(combatSettingsScreenKey.getCurrentKeyCode()) || 
               Input.GetKey(combatDeselectKey.getCurrentKeyCode());
        } else
        {
            return Input.GetKey(settingsScreenKey.getCurrentKeyCode()) || 
               Input.GetKey(backOutKey.getCurrentKeyCode());
        }

    }

    public static bool mouseWheelScrollingUp()
    {
        return Input.mouseScrollDelta.y > 0;
    }

    public static bool mouseWheelScrollingDown()
    {
        return Input.mouseScrollDelta.y < 0;
    }

    public static bool continueUIKeyIsPressed()
    {
        if(CombatStateManager.inCombat)
        {
            return Input.GetKey(combatSelectKey.getCurrentKeyCode());
            
        } else
        {
            return Input.GetKey(interactKey.getCurrentKeyCode()) ||
                    Input.GetKey(acceptKey.getCurrentKeyCode()) ||
                    Input.GetKey(acceptInputKey.getCurrentKeyCode());
        }
    }

    public static bool screenNavigationButtonIsPressed()
    {
        return !SaveHandler.saveNameFieldIsSelected() && 
                (Input.GetKey(characterScreenKey.getCurrentKeyCode()) ||
                 Input.GetKey(inventoryScreenKey.getCurrentKeyCode()) || 
                 Input.GetKey(partyScreenKey.getCurrentKeyCode()) || 
                 Input.GetKey(journalScreenKey.getCurrentKeyCode()) || 
                 Input.GetKey(loadScreenKey.getCurrentKeyCode()) || 
                 Input.GetKey(settingsScreenKey.getCurrentKeyCode()));
    }

    public static bool skipTutorialKeysArePressed()
    {
        return (Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.RightShift)) &&
                Input.GetKey(KeyCode.Escape);
    }

    //Dialogue KeyBinds
    public static bool continueStoryKeyIsPressed()
    {
        return Input.GetKey(interactKey.getCurrentKeyCode()) ||
                Input.GetKey(KeyCode.Alpha1) ||
                Input.GetKey(acceptKey.getCurrentKeyCode()) ||
                Input.GetKey(acceptInputKey.getCurrentKeyCode());
    }

    //Misc Keys
    public static bool quickLoadKeysPressed()
    {
        return (Application.isEditor &&
                Input.GetKey(KeyCode.Q) &&
                Input.GetKey(KeyCode.LeftControl));
    }

}
