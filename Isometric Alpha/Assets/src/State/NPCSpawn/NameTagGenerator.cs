using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

	public GameObject targetCanvas;

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
		// RevealManager.OnReveal.AddListener(handleNameTagOnReveal);
	}

	public void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
		// RevealManager.OnReveal.RemoveListener(handleNameTagOnReveal);
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
        } else
        {
            outline.removeOutline();
        }
	}

	public Color getRevealColor()
	{
		return ColorList.canBeInteractedWith;
	}

	public OutlineMode getOutlineSize()
    {
        return OutlineMode.Bold;
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
                outline.createOutline(getRevealColor(), OutlineMode.Bold);
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
			destroyNameTag();
		}
	}


}