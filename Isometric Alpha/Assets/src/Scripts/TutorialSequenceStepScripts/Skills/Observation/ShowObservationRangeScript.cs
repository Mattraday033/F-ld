using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObservationRangeScript : TutorialSequenceStepScript
{

    public override void runScript(GameObject target)
    {
        ObservationManager.getInstance().createSkillArea();
        OOCUIManager.manuallySetObservationPanelColorOn();
    }

}
