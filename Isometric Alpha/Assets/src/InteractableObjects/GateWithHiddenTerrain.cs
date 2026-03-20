using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateWithHiddenTerrain : Gate
{
    public string hiddenTerrainFlag;    

    public override void checkGateStatus()
    {
        if (GateAndChestManager.hasBeenOpened(getGateKey()))
        {
            if(playSFX)
            {
                playSFX = false;
                playOpeningAudioClip();
            }

            gameObject.SetActive(false);
            SecretDoorFlags.addSecretDoorFlag(hiddenTerrainFlag);
        }
    }
}
