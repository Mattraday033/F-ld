using UnityEngine;
using UnityEngine.EventSystems;

public class NameTagGenerator : MonoBehaviour, IRevealable
{
    public bool ignoreHover;
	public bool noNameTag = false;

    public SpriteOutline outline;

    public SpriteRenderer spriteRenderer;

    public INameSource nameSource;
	public DescriptionPanel nameTag;

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

	public void handleNameTagOnReveal(bool toggleReveal)
	{
		if (noNameTag)
		{
			return;
		}

		if (RevealManager.currentlyRevealed)
		{   //create nameTag and label npc on reveal
			spawnNameTag();
		}
		else if (nameTag != null && !(nameTag is null))
		{   //if revealedInteractables is false, then we need to hide interactables
			destroyNameTag();
		}
	}

	//IRevealable interface methods

	public void createListeners()
	{
		RevealManager.OnReveal.AddListener(onReveal);
        PlayerOOCStateManager.OnStateChangeFromWalking.AddListener(displayNameTagBasedOnStateChange);
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(displayNameTagBasedOnStateChange);

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
	}

    private void checkSpawnParams(string secretDoorFlag)
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

    public void displayNameTagBasedOnStateChange()
    {
        if(PlayerOOCStateManager.currentActivity == OOCActivity.walking && RevealManager.currentlyRevealed && !hasGenericName() && nameSourceRevealable())
        {
            spawnNameTag();
            outline.createOutline(getRevealColor());
        } else if(PlayerOOCStateManager.currentActivity != OOCActivity.walking)
        {
            onReveal(false);
        }
    }

	public void onReveal(bool toggleReveal)
	{
        if(!nameSourceRevealable() || outline == null)
        {
            return;
        }

        if(toggleReveal)
        {
            outline.createOutline(getRevealColor());

            if(!hasGenericName())
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

    private bool hasGenericName()
    {
        switch(DialogueList.scrubNameOfEndNumbers(nameSource.getName()))
        {
            //inanimate object
            case NPCNameList.chest:
            case NPCNameList.shelf:
            case NPCNameList.crate:
            case NPCNameList.crates:
            case NPCNameList.barrels:
            case NPCNameList.barricade:
            case NPCNameList.statue:
            case NPCNameList.rubble:
            case NPCNameList.awkwardRubble:

            //occupation
            case NPCNameList.guard:
            case NPCNameList.branded:
            case NPCNameList.noBrand:
            case NPCNameList.slave:
            case NPCNameList.horse:
                return true;
        }

        return false;
    }

	public Color getRevealColor()
	{
		return ColorList.canBeInteractedWith;
	}

	public void spawnNameTag()
	{
        if(gameObject.GetComponent<RectTransform>() == null)
        {
            gameObject.AddComponent<RectTransform>();
        }

		if (nameTag == null && !noNameTag)
		{
			nameTag = Instantiate(Resources.Load<GameObject>(PrefabNames.npcNameTag), transform).GetComponent<DescriptionPanel>();

			nameTag.nameText.text = getName();
		}
	}

    private string getName()
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
    }

	public void createHoverTag()
	{
		//Empty on purpose (may add for things like portcullis controls in mine lvl 2)
	}

    public bool nameSourceRevealable()
    {
        return INonRevealableNameSource.nameSourceIsRevealable(nameSource);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (nameSourceRevealable() && (!ignoreHover && (eventData == null || !eventData.used)) && !spriteRenderer.color.Equals(Color.clear))
        {
            if (eventData != null)
            {
                eventData.Use();
            }

            if(!RevealManager.currentlyRevealed && outline != null)
            {
                outline.createOutline(getRevealColor());
            }

            spawnNameTag();
        }
    }

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!ignoreHover)
		{
            if(!RevealManager.currentlyRevealed && outline != null)
            {
                outline.removeOutline();
            }
			
            if(hasGenericName() || !RevealManager.currentlyRevealed)
            {
                destroyNameTag();
            }

		}
	}


}