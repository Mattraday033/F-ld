using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeybindingButton : MonoBehaviour
{
    private const string blankButtonText = "___";

    public TextMeshProUGUI keybindingButtonLabel;
    public Button keybindingButton;

    public KeyUse keyUse;

    private void Awake()
    {
        keybindingButtonLabel.text = KeyBindingSettingsManager.keyUseDictionary[keyUse](KeyCode.None).ToString();

        KeyBindingSettingsManager.EnableAllKeyBindButtons.AddListener(enableButton);
        KeyBindingSettingsManager.DisableAllKeyBindButtons.AddListener(disableButton);
    }

    private void OnDestroy()
    {
        KeyBindingSettingsManager.EnableAllKeyBindButtons.RemoveListener(enableButton);
        KeyBindingSettingsManager.DisableAllKeyBindButtons.RemoveListener(disableButton);
    }

    public void enableButton()
    {
        keybindingButton.enabled = true;
        keybindingButton.interactable = true;

        keybindingButtonLabel.text = KeyBindingSettingsManager.keyUseDictionary[keyUse](KeyCode.None).ToString();
    }

    public void listenForKeyPress()
    {
        KeyBindingSettingsManager.DisableAllKeyBindButtons.Invoke(keyUse);
        KeyBindingSettingsManager.listening = true;
        keybindingButtonLabel.text = blankButtonText;
    }

    public void disableButton(KeyUse use)
    {
        if(use == keyUse)
        {
            keybindingButton.enabled = false;
            keybindingButton.interactable = true;
        } else
        {
            keybindingButton.interactable = false;
        }
    }

}
