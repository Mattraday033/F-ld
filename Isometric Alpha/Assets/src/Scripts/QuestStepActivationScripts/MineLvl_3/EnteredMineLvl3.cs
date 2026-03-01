using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl3 : QuestStepActivationScript
{
    public override void runScript()
    {
        if (Flags.getFlag(FlagNameList.toldToFindNandor) &&
             !Flags.getFlag(FlagNameList.enteredMineLvl3))
        {
            QuestList.finishQuest(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleFive, true);
            Flags.setFlag(FlagNameList.enteredMineLvl3, true);
        }
    }
}
