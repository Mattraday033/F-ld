using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObservationRangeScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        ObservationManager.getInstance().destroySkillArea();
        OOCUIManager.manuallySetObservationPanelColorOff();
    }

}
