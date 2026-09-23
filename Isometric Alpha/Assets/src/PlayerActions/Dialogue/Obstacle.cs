using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDialogueParticipant
{
    private bool ignoreSecretDoors;
    public string obstacleName;

    private SpriteLayerRendererList _RendererList;
    public SpriteLayerRendererList rendererList { get { return _RendererList; } }

    public Dialogue dialogue {
                                get
                                {
                                    return null;
                                }
                            }

	protected virtual void Awake()
	{
        _RendererList = GetComponent<SpriteLayerRendererList>();

        createListeners();
	}

	private void OnDestroy()
	{
		destroyListeners();
	}

    public string getName()
    {
        return obstacleName;
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
