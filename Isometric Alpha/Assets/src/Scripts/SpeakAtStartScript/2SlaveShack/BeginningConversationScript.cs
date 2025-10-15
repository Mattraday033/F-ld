using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningConversationScript: SpeakAtStartScript
{

    public override void runScript()
    {
        if (!Flags.getFlag(FlagNameList.finishedFirstDialogue))
        {
            dialogueTrigger.TriggerDialogue();
        }
    }

}
