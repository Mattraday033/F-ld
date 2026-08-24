using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : PlayerInteractionScript
{
    
}

public class AddSecretDoorFlagOnTransitionScript : PlayerInteractionScript
{
    
    private string secretDoorFlag = "";

    public AddSecretDoorFlagOnTransitionScript(string secretDoorFlag)
    {
        this.secretDoorFlag = secretDoorFlag;
    }

    public override void runScript(GameObject target = null)
    {
        if(!SecretDoorFlags.secretDoorHasBeenDiscovered(secretDoorFlag))
        {
            SecretDoorFlags.addSecretDoorFlag(secretDoorFlag);
        }
    }

}