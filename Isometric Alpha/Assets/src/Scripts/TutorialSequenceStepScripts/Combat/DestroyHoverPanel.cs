using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestroyHoverPanel : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {

        SelectorManager.deactivateCombatantInfoUIHoverPanel();

        Canvas.ForceUpdateCanvases();
    }
}
