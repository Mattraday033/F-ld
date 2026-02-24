using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStepActivationScript: PlayerInteractionScript
{
    public override void runScript()
    {
        //empty on purpose
    }

}

public class PreventTutorialsAfterBatsKilledScript : QuestStepActivationScript
{
    public override void runScript()
    {
        Flags.setFlag(TutorialSequenceList.firstHostitilityTutorialSeenFlag, true);

        if(SecretDoorFlags.secretDoorHasBeenDiscovered(SecretDoorKeyList.wisTutorialSecretDoor))
        {
            Flags.setFlag(TutorialSequenceList.observationTutorialSeenFlag, true);
        }
    }
}