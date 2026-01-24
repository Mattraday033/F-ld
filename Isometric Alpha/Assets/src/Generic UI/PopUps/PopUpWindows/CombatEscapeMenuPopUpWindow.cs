using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatEscapeMenuPopUpWindow : PopUpWindow
{
    private static CombatEscapeMenuPopUpWindow instance;

    private IEscapable escapableAboveThis;

    private SettingsPopUpButton settingsPopUpButton;
    private LoadScreenButton loadScreenButton;
    private BinaryPanelPopUpButton binaryPanelPopUpButton;

    public static CombatEscapeMenuPopUpWindow getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;

        settingsPopUpButton = new SettingsPopUpButton();
        loadScreenButton = new LoadScreenButton();
        binaryPanelPopUpButton = new BinaryPanelPopUpButton();
        EscapeStack.OnEscapableObjectRemovedFromStack.AddListener(enableOnEscapableRemoved);
    }

    private void OnDestroy()
    {
        EscapeStack.OnEscapableObjectRemovedFromStack.RemoveListener(enableOnEscapableRemoved);
    }

    public void openSettings()
    {
        settingsPopUpButton.spawnPopUp();
        disableOnPopUpSpawn();
    }

    public void openLoadMenu()
    {
        loadScreenButton.spawnPopUp();
        disableOnPopUpSpawn();
    }

    public void openExitToMainMenuDecisionPanel()
    {
        binaryPanelPopUpButton.spawnPopUp(new ReturnToMainMenu());
        disableOnPopUpSpawn();
    }

    public void openQuitToDesktopDecisionPanel()
    {
        binaryPanelPopUpButton.spawnPopUp(new QuitToDesktop());
        disableOnPopUpSpawn();
    }

    private void disableOnPopUpSpawn()
    {
        escapableAboveThis = EscapeStack.getTopEscapable();
        gameObject.SetActive(false);
    }

    private void enableOnEscapableRemoved(IEscapable escapable)
    {
        if(escapable == escapableAboveThis)
        {
            gameObject.SetActive(true);
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeCombatEscapeMenuPopUpWindow()
    {
        instance = null;
    }

}