using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl2 : QuestStepActivationScript
{
    public override void runScript()
    {
        if (!Flags.getFlag(FlagNameList.enteredMineLvl2))
        {
            QuestList.activateQuestStep(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleTwo);
            Flags.setFlag(FlagNameList.enteredMineLvl2, true);
        }
    }

}
