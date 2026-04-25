using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeakAtStartScript : PlayerInteractionScript
{
    public DialogueTrigger dialogueTrigger;

    public override void runScript(GameObject target = null)
    {
        //empty on purpose
    }
}

public class BeginningConversationScript: SpeakAtStartScript //Brush + Géza in the Starting Hut
{

    public override void runScript(GameObject target = null)
    {
        if (!Flags.getFlag(FlagNameList.finishedFirstDialogue))
        {
            dialogueTrigger.triggerDialogue();
        }
    }

}

public class WeftIntroScript: SpeakAtStartScript //Brush + Géza in the Starting Hut
{

    public override void runScript(GameObject target = null)
    {
        if (Flags.getFlag(FlagNameList.givenTaskByLaszlo) && !Flags.getFlag(FlagNameList.metWeft))
        {
            dialogueTrigger.triggerDialogue();
        }
    }

}

public class KendeInKitchenDuringRiotScript: SpeakAtStartScript
{

    public override void runScript(GameObject target = null)
    {
        if (Flags.getFlag(FlagNameList.revoltStarted) && !Flags.getFlag(FlagNameList.kendeUponEnteringKitchens))
        {
            dialogueTrigger.triggerDialogue();
        }
    }

}

public class ChiefTaborManseSecondFloorScript: SpeakAtStartScript
{

    public override void runScript(GameObject target = null)
    {
        if ((Flags.getFlag(FlagNameList.revoltStarted) && !Flags.getFlag(FlagNameList.letTaborLive) && !Flags.getFlag(FlagNameList.killedTaborInManse)) ||
            (Flags.getFlag(FlagNameList.directorDefeated) && !Flags.getFlag(FlagNameList.acceptedTaborsSurrenderAfterDirectorFight) && !Flags.getFlag(FlagNameList.killedTaborInManse)))
        {
            dialogueTrigger.triggerDialogue();
        }
    }

}

public class BeamAndCsalanInManseScript : SpeakAtStartScript
{

    public override void runScript(GameObject target = null)
    {
        if (!DeathFlagManager.isDead(NPCNameList.beam))
        {
            dialogueTrigger.triggerDialogue();
        }
    }

}

public class GuardPunishmentNandorStartScript : SpeakAtStartScript
{

    public override void runScript(GameObject target = null)
    {
        if (Flags.getFlag(FlagNameList.enteredMessHallYardAfterRevolt) && !Flags.getFlag(FlagNameList.nandorStartedGuardPunishmentConvo))
        {
            Flags.setFlag(FlagNameList.nandorStartedGuardPunishmentConvo, true);
            dialogueTrigger.triggerDialogue();
        }
    }

}