using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDialogueParticipant
{
    private bool ignoreSecretDoors;

    private string _UniqueName = "";
    public string uniqueName
    {
        get
        {
            return _UniqueName;
        }
        set
        {
            _UniqueName = value;
            _NPCName = NameSourceExtensions.toNPCName(value);
        }
    }

    private string _NPCName = "";
    public string displayName { get { return _NPCName; } }

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
    public virtual void createListeners()
	{
        if(!ignoreSecretDoors)
        {
            SecretDoorFlags.OnSecretDoorDiscovery.AddListener(checkSpawnParams);
        }
	}

	public virtual void destroyListeners()
	{
		SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(checkSpawnParams);
	}

    private void checkSpawnParams(string secretDoorFlag)
    {
        if(!SpawnParamsList.getSpawnParams(AreaManager.locationName, uniqueName).canSpawn(uniqueName))
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true); 
        }
    }


}
