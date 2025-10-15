using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnteredMineLvl1 : QuestStepActivationScript
{
    private const string questName = "Explore the Mine";

    private const int nextQuestStepIndex = 1;

    public override void runScript()
    {
        if (!Flags.getFlag(FlagNameList.enteredMineLvl1))
        {
            QuestList.activateQuestStep(questName, nextQuestStepIndex);
            Flags.setFlag(FlagNameList.enteredMineLvl1, true);
        }
    }

}
