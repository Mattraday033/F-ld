using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPunishmentConvoActivationScript : DialogueActivationScript
{
    public override void runScript()
    {
        if (Flags.getFlag(FlagNameList.kastorStartedRevolt) && Flags.getFlag(FlagNameList.directorDefeated))
        {
            if (!Flags.getFlag(FlagNameList.enteredMessHallYardAfterRevolt))
            {
                State.dialogueUponSceneLoadKey = DialogueNameList.guardPunishmentConvoKey;
                Flags.setFlag(FlagNameList.enteredMessHallYardAfterRevolt, true);
            }

            Flags.stopPartyTrainSpawning();
        }
    }
}
