using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnHoverPanel : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {

        SelectorManager.displayHoverUIForCurrentSelectorTarget();

        Canvas.ForceUpdateCanvases();
    }

    public static void runInstanceOfScript()
    {
        SpawnHoverPanel spawnHoverPanel = new SpawnHoverPanel();
        spawnHoverPanel.runScript(null);
    }
}
