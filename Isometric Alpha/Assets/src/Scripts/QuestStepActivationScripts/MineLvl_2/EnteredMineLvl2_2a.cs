using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl2_2a : QuestStepActivationScript
{
    public override void runScript()
    {
        if (!Flags.getFlag(FlagNameList.enteredMineLvl2_2a))
        {
            QuestList.activateQuestStep(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleThree);
            Flags.setFlag(FlagNameList.enteredMineLvl2_2a, true);
        }
    }

}
