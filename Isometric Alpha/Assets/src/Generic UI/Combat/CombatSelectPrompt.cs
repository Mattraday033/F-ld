using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatSelectPrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText;

    private void OnEnable()
    {
        promptText.text = "Press <b>[" + KeyBindingList.combatSelectKey.ToString() + "]</b>";
    }

}
