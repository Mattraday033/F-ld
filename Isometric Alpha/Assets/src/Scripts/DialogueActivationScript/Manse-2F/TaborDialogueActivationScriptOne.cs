using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaborDialogueActivationScriptOne : DialogueActivationScript
{
    public override bool evaluateScript()
    {
        if (Flags.getFlag(FlagNameList.kastorStartedRevolt) && !(Flags.getFlag(FlagNameList.letTaborLive) || Flags.getFlag(FlagNameList.killedTaborInManse)))
        {
            return true;
        } else
        {
            return false;
        }
    }
}
