using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveCurrentSelector : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {
        SelectorManager.moveCurrentSelector();

        SelectorManager.isMoving = false;

        SpawnHoverPanel.runInstanceOfScript();

        DamagePreviewManager.wipeAllDamagePreviews();
        SelectorManager.updateAllDamagePreviews();
    }
}
