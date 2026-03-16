using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeybindingButton : MonoBehaviour
{
    private const string blankButtonText = "___";

    public TextMeshProUGUI keybindingTitle;

    public TextMeshProUGUI keybindingButtonLabel;
    public Button keybindingButton;

    public int _KeyIndex = default;

    public int keyIndex
    {
        get
        {
            return _KeyIndex;
        }
        set
        {
            _KeyIndex = value;
        }
    }

    public void populate(KeyBind keyBind)
    {
        keyIndex = keyBind.keybindIndex;

        keybindingTitle.text = keyBind.getTitle();
        keybindingButtonLabel.text = keyBind.ToString();

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

        keybindingButtonLabel.text = KeyBindingSettingsManager.keyIndexDictionary[keyIndex].ToString();
    }

    public void listenForKeyPress()
    {
        KeyBindingSettingsManager.DisableAllKeyBindButtons.Invoke(keyIndex);
        KeyBindingSettingsManager.currentKeyIndex = keyIndex;
        keybindingButtonLabel.text = blankButtonText;
    }

    public void disableButton(int keyIndexToDisable)
    {
        if(keyIndex == keyIndexToDisable)
        {
            keybindingButton.enabled = false;
            keybindingButton.interactable = true;
        } else
        {
            keybindingButton.interactable = false;
        }
    }

}
