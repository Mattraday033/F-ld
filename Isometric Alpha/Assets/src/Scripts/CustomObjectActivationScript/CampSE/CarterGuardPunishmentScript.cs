using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarterGuardPunishmentScript : CustomObjectActivationScript
{
    public override bool evaluateScript()
    {
        if (Flags.getFlag(FlagNameList.spokeWithNandorAfterPrisoners))
        {
            return false;
        }

        if (Flags.getFlag(FlagNameList.directorDefeated) &&
                Flags.getFlag(FlagNameList.disablePartyTrain) &&
                !Flags.getFlag(FlagNameList.enteredCivilizationAfterLeavingCamp))
        {
            return true;
        }

        return false;
    }

}
