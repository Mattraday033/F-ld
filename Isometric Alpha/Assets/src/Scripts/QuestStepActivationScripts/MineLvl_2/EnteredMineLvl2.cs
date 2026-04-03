using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl2 : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        if (Flags.getFlag(FlagNameList.toldToFindNandor) &&
             !Flags.getFlag(FlagNameList.enteredMineLvl2) && 
             !Flags.getFlag(FlagNameList.enteredMineLvl2_2a) &&
             !Flags.getFlag(FlagNameList.enteredMineLvl3))
        {
            QuestList.activateQuestStep(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleTwo);
            Flags.setFlag(FlagNameList.enteredMineLvl2, true);
        }
    }

}
