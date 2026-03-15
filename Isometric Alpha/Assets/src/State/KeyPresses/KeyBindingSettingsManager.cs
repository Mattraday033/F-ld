using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[Serializable]
public enum KeyUse { 
                        OOCInteract = 0
                    }

public delegate KeyCode SetKeybind(KeyCode code);

public class KeyBindingSettingsManager : MonoBehaviour
{

    public static Dictionary<KeyUse, SetKeybind> keyUseDictionary;

    public Dictionary<KeyUse, KeybindingButton> keybindButtons;

    public readonly static UnityEvent EnableAllKeyBindButtons = new UnityEvent();    
    public readonly static UnityEvent<KeyUse> DisableAllKeyBindButtons = new UnityEvent<KeyUse>();

    private TextMeshProUGUI currentText;
    private KeyCode previousKeyCode = KeyCode.None;

    public static bool listening = false;

    void Update()
    {
        if(listening)
        {
            if(Input.inputString.Length > 0)
            {
                Debug.LogError("Input = " + (KeyCode) Input.inputString[0]);
            }
        }
    }

    public void endListening(KeyCode newKeyCode)
    {
        listening = false;

        EnableAllKeyBindButtons.Invoke();
        // currentText.
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeKeybindDictionary()
    {
        keyUseDictionary = new Dictionary<KeyUse, SetKeybind>()
        {
            [KeyUse.OOCInteract] = (KeyCode code) => 
            { 
                if(code != KeyCode.None)
                {
                    KeyBindingList.interactKey = code; 
                }

                return KeyBindingList.interactKey;
            }  
        };

        
    }

}
