using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public enum NewAbilityUIScope { Any, PerStat }

public class NewAbilityUICounter : MonoBehaviour
{

    public NewAbilityUIScope scope = NewAbilityUIScope.Any;
    public PrimaryStat primaryStat = PrimaryStat.None;

    //the screen this counter sits in, if it sits in one. The one on the overworld HUD has none
    private ScreenManager owningScreen;

    private void Awake()
    {
        owningScreen = GetComponentInParent<ScreenManager>(true);

        ScreenManager.OnScreenInteriorUpdate.AddListener(determineVisibility);
        NewAbilityManager.AbilityMarkedAsNew.AddListener(determineVisibility);
        NewAbilityManager.AbilityNoLongerNew.AddListener(determineVisibility);
        PartySpriteGridRow.OnPartyMemberSelected.AddListener(determineVisibility);
        PlayerAbilityGridRowDescriptionPanel.AbilityNoLongerNew.AddListener(determineVisibility);

        determineVisibility();
    }

    private void OnDestroy()
    {
        ScreenManager.OnScreenInteriorUpdate.RemoveListener(determineVisibility);
        NewAbilityManager.AbilityMarkedAsNew.RemoveListener(determineVisibility);
        NewAbilityManager.AbilityNoLongerNew.RemoveListener(determineVisibility);
        GridRow.OnDescribableToDisplay.AddListener(determineVisibility);
        PlayerAbilityGridRowDescriptionPanel.AbilityNoLongerNew.RemoveListener(determineVisibility);
    }

    public void determineVisibility()
    {
        determineVisibility(null);
    }

    public void determineVisibility(object obj)
    {
        //a counter on a hidden screen is worked out again when its screen is next shown
        if (ScreenManager.isHidden(owningScreen))
        {
            return;
        }

        if(scope == NewAbilityUIScope.Any)
        {
            gameObject.SetActive(NewAbilityManager.anAbilityIsMarkedAsNew());
            return;
        }

        if(scope == NewAbilityUIScope.PerStat)
        {
            gameObject.SetActive(NewAbilityManager.anAbilityIsMarkedAsNewPerStat(OverallUIManager.getCurrentPartyMember(), primaryStat));
            return;
        }
    }
}
