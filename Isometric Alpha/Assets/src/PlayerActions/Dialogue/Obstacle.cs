using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDialogueParticipant
{
    private bool ignoreSecretDoors;
    public string obstacleName;
    public SpriteRenderer spriteRenderer;

	private void Awake()
	{
        createListeners();
	}

	private void OnDestroy()
	{
		destroyListeners();
	}

    public void setObstacleName(string obstacleName)
    {
        this.obstacleName = obstacleName;
    }

    public string getName()
    {
        return obstacleName;
    }

    public Dialogue getDialogue()
    {
        return null;
    }

    public virtual void setToDown()
    {
        gameObject.SetActive(false);
    }
    
    public virtual void setToUp()
    {
        gameObject.SetActive(true);
    }

    public void setToIgnoreSecretDoors()
    {
        ignoreSecretDoors = true;
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(checkSpawnParams);   
    } 
    public void createListeners()
	{
        if(!ignoreSecretDoors)
        {
            SecretDoorFlags.OnSecretDoorDiscovery.AddListener(checkSpawnParams);
        }
	}

	public void destroyListeners()
	{
		SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(checkSpawnParams);
	}

    private void checkSpawnParams(string secretDoorFlag)
    {
        if(!SpawnParamsList.getSpawnParams(AreaManager.locationName, getName()).canSpawn(getName()))
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true); 
        }
    }


}
