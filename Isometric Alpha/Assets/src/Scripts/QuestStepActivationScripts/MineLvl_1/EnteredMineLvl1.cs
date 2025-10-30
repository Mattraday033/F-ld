using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredMineLvl1 : QuestStepActivationScript
{
    private const string questName = "Explore the Mine";

    private const int nextQuestStepIndex = 1;

    public override void runScript()
    {
        Debug.LogError("B4 IF");
        if (!Flags.getFlag(FlagNameList.enteredMineLvl1))
        {
            Debug.LogError("Inside IF");
            QuestList.activateQuestStep(questName, nextQuestStepIndex);
            Flags.setFlag(FlagNameList.enteredMineLvl1, true);
        }
    }

}
