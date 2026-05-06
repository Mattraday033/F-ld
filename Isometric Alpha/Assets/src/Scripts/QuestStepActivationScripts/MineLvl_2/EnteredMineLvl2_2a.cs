using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl2_2a : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        if ((Flags.getFlag(FlagNameList.toldToFindNandor) ||
                Flags.getFlag(FlagNameList.trainedByEmeseToUseBlasingJelly)) &&
             !Flags.getFlag(FlagNameList.enteredMineLvl2_2a) &&
             !Flags.getFlag(FlagNameList.enteredMineLvl3))
        {
            QuestList.activateQuestStep(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleThree);
            Flags.setFlag(FlagNameList.enteredMineLvl2_2a, true);
        }
    }

}
