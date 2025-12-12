using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPunishmentTransitionScript : PlayerInteractionScript
{
    public override void runScript()
    {
        if (Flags.getFlag(FlagNameList.directorDefeated) && Flags.getFlag(FlagNameList.mineLvl3CarterAndNandorInParty))
        {
            if (!Flags.getFlag(FlagNameList.enteredMessHallYardAfterRevolt))
            {
                Flags.setFlag(FlagNameList.enteredMessHallYardAfterRevolt, true);
            }

            Flags.stopPartyTrainSpawning();
        }
    }
}
