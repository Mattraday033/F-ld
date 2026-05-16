using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapSelectorToPlayer : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {
        Stats playerStats = PartyManager.getPlayerStats();
        if (playerStats != null && playerStats.positions.Count > 0)
        {
            SelectorManager.currentSelector.setToLocation(playerStats.positions[0]);
        }

        SpawnHoverPanel.runInstanceOfScript();

        KeyPressManager.handlingPrimaryKeyPress = true;
        KeyPressManager.handlingSecondaryKeyPress = true;
    }
}
