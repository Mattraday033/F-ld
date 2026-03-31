using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoreInfoNode : MonoBehaviour
{
    public TextMeshProUGUI keybindText;

    private void Awake()
    {
        keybindText.text = "[" + KeyBindingList.showFormulaKey.ToString() + "]";
    }

}

