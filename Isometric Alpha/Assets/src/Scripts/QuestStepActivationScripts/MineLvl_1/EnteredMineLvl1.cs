using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl1 : QuestStepActivationScript
{
    public override void runScript()
    {
        if (Flags.getFlag(FlagNameList.toldToFindNandor) &&
             !Flags.getFlag(FlagNameList.enteredMineLvl1) && 
             !Flags.getFlag(FlagNameList.enteredMineLvl2_2a) &&
             !Flags.getFlag(FlagNameList.enteredMineLvl2) && 
             !Flags.getFlag(FlagNameList.enteredMineLvl3))
        {
            QuestList.activateQuestStep(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleOne);
            Flags.setFlag(FlagNameList.enteredMineLvl1, true);
        }
    }

}
