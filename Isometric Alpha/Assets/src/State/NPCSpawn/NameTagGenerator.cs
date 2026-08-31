using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class NameTagGenerator : MonoBehaviour, IRevealable
{
    public bool ignoreHover;
	public bool noNameTag = false;

    public SpriteOutline outline;

    public SpriteRenderer spriteRenderer;

    public INameSource nameSource;
	public DescriptionPanel nameTag;

    public OverHeadIconManager overHeadIconManager;

    private bool ignoreSecretDoors;

	private void Awake()
	{
        if(spriteRenderer != null)
        {
            outline = new SpriteOutline();
            outline.setSpriteRenderer(spriteRenderer); 
        }

        INonRevealableNameSource nonRevealableNameSource = GetComponent<INonRevealableNameSource>();

        if(nonRevealableNameSource != null)
        {
            nameSource = nonRevealableNameSource;
        } else
        {
            nameSource = GetComponent<INameSource>();
        }

        createListeners();
	}

	private void OnDestroy()
	{
		destroyListeners();
	}

	//IRevealable interface methods

	public void createListeners()
	{
		RevealManager.OnReveal.AddListener(onReveal);
        PlayerOOCStateManager.OnStateChangeFromWalking.AddListener(displayNameTagBasedOnStateChange);
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(displayNameTagBasedOnStateChange);

        PlayerOOCStateManager.OnStateChangeFromWalking.AddListener(this.revealBasedOnStateChange);
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(this.revealBasedOnStateChange);

        if(!ignoreSecretDoors)
        {
            SecretDoorFlags.OnSecretDoorDiscovery.AddListener(checkSpawnParams);
        }
	}

	public void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
		SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(checkSpawnParams);
        PlayerOOCStateManager.OnStateChangeFromWalking.RemoveListener(displayNameTagBasedOnStateChange);
        PlayerOOCStateManager.OnStateChangeToWalking.RemoveListener(displayNameTagBasedOnStateChange);
        
        PlayerOOCStateManager.OnStateChangeFromWalking.RemoveListener(this.revealBasedOnStateChange);
        PlayerOOCStateManager.OnStateChangeToWalking.RemoveListener(this.revealBasedOnStateChange);
	}

    public virtual void checkSpawnParams(string secretDoorFlag)
    {
        if(!SpawnParamsList.getSpawnParams(AreaManager.locationName, nameSource.getName()).canSpawn(nameSource.getName()))
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true); 
        }
    }

    public SpriteOutline getSpriteOutline()
    {
        return outline;
    }

	public void onReveal(bool toggleReveal)
	{
        if(!INonRevealableNameSource.nameSourceIsRevealable(this) || outline == null)
        {
            return;
        }

        if(toggleReveal && spriteRenderer != null && !spriteRenderer.color.Equals(Color.clear))
        {
            outline.createOutline(getRevealColor());

            if(!this.hasGenericName())
            {
                spawnNameTag();
            }

        } else
        {
            outline.removeOutline();

            destroyNameTag();
        }
	}

    public void setToIgnoreSecretDoors()
    {
        ignoreSecretDoors = true;
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(checkSpawnParams);   
    }

	public virtual Color getRevealColor()
	{
		return ColorList.canBeInteractedWith;
	}

	public void spawnNameTag()
	{
        if(!gameObject.activeInHierarchy || (spriteRenderer != null && spriteRenderer.color.Equals(Color.clear)))
        { 
            destroyNameTag();
            return;
        }

        if(overHeadIconManager != null)
        {
            createOverHeadNameTag();
            return;
        }

        if(gameObject.GetComponent<RectTransform>() == null)
        {
            gameObject.AddComponent<RectTransform>();
        }

		if (nameTag == null && !noNameTag)
		{
			nameTag = Instantiate(Resources.Load<GameObject>(PrefabNames.npcNameTag), transform).GetComponent<DescriptionPanel>();

			nameTag.nameText.text = getName();

            if(PlayerOOCStateManager.currentActivity == OOCActivity.inWorldMap)
            {
                Canvas canvas = nameTag.GetComponent<Canvas>();

                if(canvas != null)
                {
                    canvas.overrideSorting = true;
                    canvas.sortingLayerName = LayerAndTagManager.mapSortingLayerName;
                    canvas.sortingOrder = Constants.indexTwelve;
                }
            }
		}
	}

    private void createOverHeadNameTag()
    {
        overHeadIconManager.createOverHeadIcon(OverHeadIconType.NameTag, nameOfNPC: getName());
    }
    public string getName()
    {
        string name = nameSource.getName();

        if(name.Contains("#"))
        {
            return name;
        }

        return DialogueList.scrubNameOfEndNumbers(name);
    }

    public void destroyNameTag()
    {
        if (nameTag != null)
        {
            Destroy(nameTag.gameObject);
            nameTag = null;
        }

        if(overHeadIconManager != null)
        {
            overHeadIconManager.destroyIcon(OverHeadIconType.NameTag);
        }
    }

	public void createHoverTag()
	{
		//Empty on purpose (may add for things like portcullis controls in mine lvl 2)
	}

    public void OnPointerEnter(PointerEventData eventData) 
    {
        if (INonRevealableNameSource.nameSourceIsRevealable(this) && 
            !ignoreHover && (eventData == null || !eventData.used) &&
            !spriteRenderer.color.Equals(Color.clear))
        {
            if (eventData != null)
            {
                eventData.Use();
            }

            if(!RevealManager.currentlyRevealed && outline != null)
            {
                outline.createOutline(getRevealColor());
            }

            PlayerObject.toggleButtonPrompt(false);
            spawnNameTag();
        }
    }

	public void OnPointerExit(PointerEventData eventData)
	{
        PlayerObject.restoreButtonPrompt();

		if (!ignoreHover)
		{
            if(!RevealManager.currentlyRevealed && outline != null)
            {
                outline.removeOutline();
            }
			
            if(this.hasGenericName() || !RevealManager.currentlyRevealed)
            {
                destroyNameTag();
            }

		}
	}

    public void displayNameTagBasedOnStateChange()
    {
        if(PlayerOOCStateManager.currentActivity == OOCActivity.walking && 
            RevealManager.currentlyRevealed && 
            INonRevealableNameSource.nameSourceIsRevealable(this) && 
            !this.hasGenericName())
        {
            spawnNameTag();
        } 
    }
}