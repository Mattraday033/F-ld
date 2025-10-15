using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredPit1b : QuestStepActivationScript
{
    private const string questName = "Rescue Broglin";

    private const int nextQuestStepIndex = 2;
    public const string enteredPit1bFlag = "enteredPit-1b";

    public override void runScript()
    {
        if (!Flags.getFlag(enteredPit1bFlag) && !Flags.getFlag(BookList.pitSecondEntranceNoteReadFlag))
        {
            QuestList.activateQuestStep(questName, nextQuestStepIndex);
            Flags.setFlag(enteredPit1bFlag, true);
        }
        else if (!Flags.getFlag(enteredPit1bFlag))
        {
            Flags.setFlag(enteredPit1bFlag, true);
        }
    }

}
