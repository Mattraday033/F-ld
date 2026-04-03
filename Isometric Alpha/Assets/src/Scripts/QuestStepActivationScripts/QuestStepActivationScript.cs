using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStepActivationScript: PlayerInteractionScript
{
    public override void runScript(GameObject target = null)
    {
        //empty on purpose
    }

}

public class PreventTutorialsAfterBatsKilledScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        Flags.setFlag(TutorialSequenceList.firstHostilityTutorialSeenFlag, true);

        if(SecretDoorFlags.secretDoorHasBeenDiscovered(SecretDoorKeyList.wisTutorialSecretDoor))
        {
            Flags.setFlag(TutorialSequenceList.observationTutorialSeenFlag, true);
        }
    }
}