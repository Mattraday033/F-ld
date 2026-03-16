using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class OverwriteKeybind : IDecision
{
    public const string overwriteKeybindMessageStart = "Are you certain you want to use '";
    public const string overwriteKeybindMessageMiddle = "'? That key is already being used as the ";
    public const string overwriteKeybindMessageEnd = " key.";

    public static string overwriteKeybindMessage = "";

    public OverwriteKeybind()
    {
        
    }

    public string getMessage()
    {
        if(getCurrentKeybind() == null)
        {
            return "Error: No KeyUse found.";
        }

        return overwriteKeybindMessage;
    }

    public KeyBind getCurrentKeybind()
    {
        return KeyBindingSettingsManager.keyIndexDictionary[KeyBindingSettingsManager.currentKeyIndex];
    }

    public void execute()
    {

        KeyBindingSettingsManager.SetToNone.Invoke(KeyBindingSettingsManager.keybindToOverwrite.getCurrentKeyCode(), 
                                                    KeyBindingSettingsManager.keybindToOverwrite.types);
        KeyBindingSettingsManager.setNewKey();
        overwriteKeybindMessage = "";
        KeyBindingSettingsManager.keybindToOverwrite = null;
        KeyBindingSettingsManager.EnableAllKeyBindButtons.Invoke();
    }

    public void backOut()
    {
        KeyBindingSettingsManager.currentKeyIndex = default;
        KeyBindingSettingsManager.newKeyCode = KeyCode.None;
        overwriteKeybindMessage = "";
        KeyBindingSettingsManager.keybindToOverwrite = null;
        KeyBindingSettingsManager.EnableAllKeyBindButtons.Invoke();
    }
}
