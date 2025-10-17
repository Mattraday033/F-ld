using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl2_2a : QuestStepActivationScript
{
    private const string questName = "Explore the Mine";

    private const int nextQuestStepIndex = 3;

    public override void runScript()
    {
        if (!Flags.getFlag(FlagNameList.enteredMineLvl2_2a))
        {
            QuestList.activateQuestStep(questName, nextQuestStepIndex);
            Flags.setFlag(FlagNameList.enteredMineLvl2_2a, true);
        }
    }

}
