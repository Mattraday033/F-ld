using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class GameplaySettingsManager : MonoBehaviour
{
    public readonly static UnityEvent ReturnAllGameplayOptionsToDefault = new UnityEvent();    

    public Transform scrollableArea;

    public BinaryPanelPopUpButton defaultResetPopUpButton;
    public BinaryPanelPopUpButton resetTutorialsPopUpButton;

    private void Awake()
    {
        populateGameplaySections();
    }

    private void OnDisable()
    {
    }

    private void OnDestroy()
    {
        
    }

    private void populateGameplaySections()
    {
        GameplaySection combatSection = Instantiate(Resources.Load<GameObject>(PrefabNames.gameplaySection), scrollableArea).GetComponent<GameplaySection>();

        combatSection.createGamepaySettingsPrompts("Combat", new List<GameplaySetting>() { GameplaySettingsList.autoTarget, GameplaySettingsList.combatAnimationSpeed, GameplaySettingsList.healthBarsAlwaysVisible });

        GameplaySection overworldSection = Instantiate(Resources.Load<GameObject>(PrefabNames.gameplaySection), scrollableArea).GetComponent<GameplaySection>();

        overworldSection.createGamepaySettingsPrompts("Overworld", new List<GameplaySetting>() { GameplaySettingsList.transitionIndicatorsAlwaysVisible });

        GameplaySection journalSection = Instantiate(Resources.Load<GameObject>(PrefabNames.gameplaySection), scrollableArea).GetComponent<GameplaySection>();

        journalSection.createGamepaySettingsPrompts("Journal", new List<GameplaySetting>() { GameplaySettingsList.boldImportantQuestText, GameplaySettingsList.showOnlyImportantQuestText });

        GameplaySection tutorialsEnabled = Instantiate(Resources.Load<GameObject>(PrefabNames.gameplaySection), scrollableArea).GetComponent<GameplaySection>();

        tutorialsEnabled.createGamepaySettingsPrompts("Tutorials", new List<GameplaySetting>() { GameplaySettingsList.tutorialsEnabled });
    }

    public void spawnReturnToDefaultsPopUp()
    {
        defaultResetPopUpButton.spawnPopUp();
    }

    public void spawnResetTutorialsPopUp()
    {
        resetTutorialsPopUpButton.spawnPopUp();
    }

}
