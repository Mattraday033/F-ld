using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl3 : QuestStepActivationScript
{
    public override void runScript()
    {
        if (!Flags.getFlag(FlagNameList.enteredMineLvl3))
        {
            QuestList.activateQuestStep(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleFive);
            Flags.setFlag(FlagNameList.enteredMineLvl3, true);
        }
    }

}
