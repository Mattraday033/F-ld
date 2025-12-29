using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl1 : QuestStepActivationScript
{
    public override void runScript()
    {
        if (!Flags.getFlag(FlagNameList.enteredMineLvl1))
        {
            QuestList.activateQuestStep(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleOne);
            Flags.setFlag(FlagNameList.enteredMineLvl1, true);
        }
    }

}
