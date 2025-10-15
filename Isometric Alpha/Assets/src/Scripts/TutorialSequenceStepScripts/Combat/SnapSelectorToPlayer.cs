using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapSelectorToPlayer : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        SelectorManager.currentSelector.setToLocation(PartyManager.getPlayerStats().position);
        
        SpawnHoverPanel.runInstanceOfScript();
    }
}
