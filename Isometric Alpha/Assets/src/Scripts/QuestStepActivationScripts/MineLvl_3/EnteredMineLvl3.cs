using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl3 : QuestStepActivationScript
{
    private const string questName = "Explore the Mine";

    private const int nextQuestStepIndex = 5;

    public override void runScript()
    {
        if (!Flags.getFlag(FlagNameList.enteredMineLvl3))
        {
            QuestList.activateQuestStep(questName, nextQuestStepIndex);
            Flags.setFlag(FlagNameList.enteredMineLvl3, true);
        }
    }

}
