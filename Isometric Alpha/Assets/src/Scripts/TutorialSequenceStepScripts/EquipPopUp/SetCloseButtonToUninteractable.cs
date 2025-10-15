using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCloseButtonToUninteractable : TutorialSequenceStepScript
{
    public override void runScript(GameObject target)
    {
        EditAbilityWheelPopUpWindow editAbilityWheelPopUpWindow = EditAbilityWheelPopUpWindow.getInstance();

        editAbilityWheelPopUpWindow.setCloseButtonToUninteractable();
    }
}
