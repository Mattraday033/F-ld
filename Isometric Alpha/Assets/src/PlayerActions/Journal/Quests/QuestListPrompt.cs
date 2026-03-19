using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestListPrompt : MonoBehaviour
{
    public TextMeshProUGUI journalPrompt;

    private void OnEnable()
    {
        journalPrompt.text = "Journal <b>["+KeyBindingList.journalScreenKey.ToString()+"]</b>";
    }


}
