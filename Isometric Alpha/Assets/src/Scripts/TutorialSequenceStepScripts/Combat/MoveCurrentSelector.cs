using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveCurrentSelector : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        SelectorManager.getInstance().moveCurrentSelector();

        SelectorManager.getInstance().frameCount = 0;
        SelectorManager.getInstance().isMoving = false;

        SpawnHoverPanel.runInstanceOfScript();

        DamagePreviewManager.resetAllDamagePreviews();
        DamagePreviewManager.setUpDamagePreviews();
    }
}
