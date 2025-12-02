using UnityEngine;
using UnityEngine.EventSystems;

public class NameTagGenerator : MonoBehaviour, IRevealable
{
    private const string vaultablePrefix = "Vaultable ";

    public bool ignoreHover;
	public bool noNameTag = false;

    public SpriteOutline outline;

    public SpriteRenderer spriteRenderer;

    public INameSource nameSource;
	public DescriptionPanel nameTag;

	private void Awake()
	{
        outline = new SpriteOutline();
        outline.setSpriteRenderer(spriteRenderer);

        INonRevealableNameSource nonRevealableNameSource = GetComponent<INonRevealableNameSource>();

        if(nonRevealableNameSource != null)
        {
            nameSource = nonRevealableNameSource;
        } else
        {
            nameSource = GetComponent<INameSource>();
        }

	}

	private void OnEnable()
	{
		createListeners();
	}

	private void OnDisable()
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
		SecretDoorFlags.OnSecretDoorDiscovery.AddListener(checkSpawnParams);
	}

	public void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
		SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(checkSpawnParams);
	}

    private void checkSpawnParams(string secretDoorFlag)
    {
        if(!SpawnParamList.getSpawnParams(AreaManager.locationName, nameSource.getName()).canSpawn(nameSource.getName()))
        {
            gameObject.SetActive(false);
        }
    }

    public SpriteOutline getSpriteOutline()
    {
        return outline;
    }

	public void onReveal(bool toggleReveal)
	{
        if(!nameSourceRevealable())
        {
            return;
        }

        if(toggleReveal)
        {
            outline.createOutline(getRevealColor(), getOutlineSize());

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

    private bool hasGenericName()
    {
        switch(nameSource.getName())
        {
            //inanimate object
            case NPCNameList.chest:
            case NPCNameList.shelf:
            case NPCNameList.barrels:
            case NPCNameList.barricade:
            case NPCNameList.statue:
            case NPCNameList.rubble:

            //occupation
            case NPCNameList.guard:
            case NPCNameList.branded:
            case NPCNameList.noBrand:
                return true;
        }

        return false;
    }

	public Color getRevealColor()
	{
		return ColorList.canBeInteractedWith;
	}

	public OutlineMode getOutlineSize()
    {
        return OutlineMode.Normal;
    }

	private void spawnNameTag()
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
        return DialogueList.scrubNameOfEndNumbers(nameSource.getName());
    }

    private void destroyNameTag()
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
        if (nameSourceRevealable() && (!ignoreHover && (eventData == null || !eventData.used)))
        {
            if (eventData != null)
            {
                eventData.Use();
            }

            if(!RevealManager.currentlyRevealed)
            {
                outline.createOutline(getRevealColor(), getOutlineSize());
            }

            spawnNameTag();
        }
    }

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!ignoreHover)
		{
            if(!RevealManager.currentlyRevealed)
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