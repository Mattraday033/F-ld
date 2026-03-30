using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoreInfoNode : MonoBehaviour
{
    private static int nodesOnScreen = 0;

    public TextMeshProUGUI keybindText;

    private void Awake()
    {
        nodesOnScreen++;
        
        if((CombatStateManager.inCombat && CombatStateManager.whoseTurn != WhoseTurn.Won) || 
            !canShow())
        {
            gameObject.SetActive(false);
            return;
        }

        keybindText.text = "[" + KeyBindingList.showFormulaKey.ToString() + "]";
    }

    private void OnDestroy()
    {
        nodesOnScreen--;
    }

    private static bool canShow()
    {
        return nodesOnScreen == Constants.sizeZero;
    }

}

