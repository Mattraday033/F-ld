using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWithSecretDoorFlag : Obstacle
{

    private string _SecretDoorFlag;
    public string secretDoorFlag
    {
        get
        {
            return _SecretDoorFlag;
        }
        set
        {
            _SecretDoorFlag = value;

            if(SecretDoorFlags.secretDoorHasBeenDiscovered(secretDoorFlag))
            {
                setToDown();
            }
        }
    }

    private void OnEnable()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(hideOnSecretDoorDiscovery);
    }

    private void OnDisable()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(hideOnSecretDoorDiscovery);   
    }

    private void hideOnSecretDoorDiscovery(string secretDoorFlag)
    {
        if(this.secretDoorFlag.Equals(secretDoorFlag))
        {
            setToDown();
        }
    }

}
