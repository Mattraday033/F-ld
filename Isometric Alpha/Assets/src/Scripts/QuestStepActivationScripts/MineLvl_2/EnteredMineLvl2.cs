using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl2 : QuestStepActivationScript
{
    private const string questName = "Explore the Mine";

    private const int nextQuestStepIndex = 2;

    public override void runScript()
    {
        if (!Flags.getFlag(FlagNameList.enteredMineLvl2))
        {
            QuestList.activateQuestStep(questName, nextQuestStepIndex);
            Flags.setFlag(FlagNameList.enteredMineLvl2, true);
        }
    }

}
