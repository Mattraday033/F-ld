using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredPit2a : QuestStepActivationScript
{
    public const string enteredPit2aFlag = "enteredPit-2a";

    public override void runScript(GameObject target = null)
    {
        if (!Flags.getFlag(enteredPit2aFlag))
        {
            QuestList.activateQuestStep(QuestNameList.rescueBroglinQuestTitle, QuestNameList.rescueBroglinStepTitleFour);
            Flags.setFlag(enteredPit2aFlag, true);
        }
    }

}
