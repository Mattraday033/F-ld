using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum SettingsManagerState { Keybinds, Audio, Video }

[System.Serializable]
public class SettingsManager : ScreenManager, IEscapable
{
    private static SettingsManagerState state = SettingsManagerState.Keybinds;
    private static SettingsManager instance;

    // public Button gameplayButton;
    // public GameObject gameplayPanel;

    public Button keybindsButton;
    public GameObject keybindsPanel;

    public Button audioSettingsButton;
    public GameObject audioPanel;

    // public Button videoSettingsButton;
    // public GameObject videoPanel;

    public static SettingsManager getInstance()
    {
        return instance;
    }

    public override void Awake()
    {
        base.Awake();

        instance = this;

        setToState(state);

        if (CombatStateManager.inCombat)
        {
            TutorialSequenceStepTargetUIObject.createCutOutMask(transform);
        }
    }

    public static void setToState(SettingsManagerState state)
    {
        SettingsManager.state = state;

        if(instance == null)
        {
            return;
        }

        switch(state)
        {
            case SettingsManagerState.Audio:
                instance.keybindsButton.interactable = true;
                instance.keybindsPanel.SetActive(false);

                instance.audioSettingsButton.interactable = false;
                instance.audioPanel.SetActive(true);
                return;   

            default:
                instance.audioSettingsButton.interactable = true;
                instance.audioPanel.SetActive(false);

                instance.keybindsButton.interactable = false;
                instance.keybindsPanel.SetActive(true);
                return;
        }
    }

    public override bool requiresPartyMemberSelectionGrid()
    {
        return false;
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();
        listOfEvents.Add(OnScreenInteriorUpdate);

        return listOfEvents;
    }
    public override DescribableList getDefaultDescribableList()
    {
        return DescribableList.Unnecessary;
    }

    public override void updateCounter()
    {
        //Empty on Purpose
    }
    public void handleEscapePress()
    {
        EscapeStack.removeTopObjectFromStack();
        Destroy(gameObject);
    }

    public override KeyCode getExitKeyCode()
    {
        return KeyBindingList.settingsScreenKey1;
    }
}