using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum SettingsManagerState { 
                                    Gameplay = 0, 
                                    Keybinds = 1, 
                                    Audio = 2, 
                                    Video = 3
                                }

[System.Serializable]
public class SettingsManager : ScreenManager, IEscapable
{
    private static SettingsManagerState state;
    private static SettingsManager instance;

    public Button gameplayButton;
    public GameObject gameplayPanel;

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

        setToState((int) state);

        if (CombatStateManager.inCombat)
        {
            TutorialSequenceStepTargetUIObject.createCutOutMask(transform);
        }
    }

    private void OnEnable()
    {
        //only the copy that is on screen answers as the settings screen, so the one combat makes and the prebuilt one never argue over it
        instance = this;
    }

    private void OnDisable()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

    protected override void onPrebuilt()
    {
        //each panel builds its contents the first time it is switched on, so all three are switched on once here, behind the loading screen
        gameplayPanel.SetActive(true);
        keybindsPanel.SetActive(true);
        audioPanel.SetActive(true);

        setToState((int) state);
    }

    protected override void prepareToShow()
    {
        setToState((int) state);

        //puts each keybind label back in step with its key, in case the copy combat makes changed one, and rechecks for unassigned keys
        KeyBindingSettingsManager.EnableAllKeyBindButtons.Invoke();
    }

    public static void setToState(int state)
    {
        SettingsManager.state = (SettingsManagerState) state;

        if(instance == null)
        {
            return;
        }

        switch(SettingsManager.state)
        {
            case SettingsManagerState.Audio:

                instance.gameplayButton.interactable = true;
                instance.gameplayPanel.SetActive(false);

                instance.keybindsButton.interactable = true;
                instance.keybindsPanel.SetActive(false);

                instance.audioSettingsButton.interactable = false;
                instance.audioPanel.SetActive(true);
                return;   

            case SettingsManagerState.Gameplay:
                instance.gameplayButton.interactable = false;
                instance.gameplayPanel.SetActive(true);

                instance.keybindsButton.interactable = true;
                instance.keybindsPanel.SetActive(false);

                instance.audioSettingsButton.interactable = true;
                instance.audioPanel.SetActive(false);
                return;   

            default:
                instance.gameplayButton.interactable = true;
                instance.gameplayPanel.SetActive(false);

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
        //the prebuilt copy never goes on the escape stack and is put away by OverallUIManager. This is for the copy combat makes
        if(isPrebuilt)
        {
            return;
        }

        EscapeStack.removeTopObjectFromStack();
        Destroy(gameObject);
    }

    public override KeyCode getExitKeyCode()
    {
        return KeyBindingList.settingsScreenKey.getCurrentKeyCode();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        state = SettingsManagerState.Gameplay;
        instance = null;
    }
}