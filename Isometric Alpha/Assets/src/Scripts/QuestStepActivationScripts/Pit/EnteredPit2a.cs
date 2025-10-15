using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredPit2a : QuestStepActivationScript
{
    private const string questName = "Rescue Broglin";

    private const int nextQuestStepIndex = 4;
    public const string enteredPit2aFlag = "enteredPit-2a";

    public override void runScript()
    {
        if (!Flags.getFlag(enteredPit2aFlag))
        {
            QuestList.activateQuestStep(questName, nextQuestStepIndex);
            Flags.setFlag(enteredPit2aFlag, true);
        }
    }

}
