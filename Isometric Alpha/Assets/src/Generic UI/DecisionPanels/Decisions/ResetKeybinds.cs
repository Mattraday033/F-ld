using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class ResetKeybinds : IDecision
{
    private const string resetKeybindsMessage = "Are you sure you want to reset your Keybinds? This cannot be undone.";

    public ResetKeybinds()
    {
    }

    public string getMessage()
    {
        return resetKeybindsMessage;
    }

    public void execute()
    {
        KeyBindingSettingsManager.ReturnAllKeybindsToDefault.Invoke();
        KeyBindingSettingsManager.EnableAllKeyBindButtons.Invoke();    
    }

    public void backOut()
    {

    }
}
