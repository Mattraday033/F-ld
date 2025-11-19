using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWithSecretDoorFlag : Obstacle
{

    public string secretDoorFlag;

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
