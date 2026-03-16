using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeybindingSection : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Transform buttonParent;

    public void createKeybindButtons(string title, List<KeyBind> keyBinds)
    {
        this.title.text = title;

        foreach(KeyBind keyBind in keyBinds)
        {
            GameObject keybindingButtonGO = Instantiate(Resources.Load<GameObject>(PrefabNames.keybindButton), buttonParent);
            KeybindingButton keybindingButton = keybindingButtonGO.GetComponent<KeybindingButton>();

            keybindingButton.populate(keyBind);
        }
    }

    public void setTitle(string titleText)
    {
        title.text = titleText;
    }

}
