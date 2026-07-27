using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class ResetGameplayOptions : IDecision
{
    private const string resetGameplayOptionsMessage = "Are you sure you want to reset all Gameplay Options to their default settings? This cannot be undone.";

    public ResetGameplayOptions()
    {
    }

    public string getMessage()
    {
        return resetGameplayOptionsMessage;
    }

    public void execute()
    {
        GameplaySettingsManager.ReturnAllGameplayOptionsToDefault.Invoke();
        GameplayOptionManager.ManualGameplayOptionUpdate.Invoke();
    }

    public void backOut()
    {

    }
}
