using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaborDialogueActivationScriptTwo : DialogueActivationScript
{
    public override bool evaluateScript()
    {
        if (Flags.getFlag(FlagNameList.directorDefeated) && !(Flags.getFlag(FlagNameList.acceptedTaborsSurrenderAfterDirectorFight) || Flags.getFlag(FlagNameList.killedTaborInManse)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
