using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalPrompt : MonoBehaviour
{
    private void Awake()
    {
        if(CombatStateManager.inCombat || PlayerOOCStateManager.currentActivity != OOCActivity.walking)
        {
            gameObject.SetActive(false);
        }
    }

}
