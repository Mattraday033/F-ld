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

	public DescriptionPanel nameTag;

	public GameObject targetCanvas;

	private void Awake()
	{
		spawnTargetCanvas();
	}

	private void OnEnable()
	{
		createListeners();
	}

	private void OnDisable()
	{
		destroyListeners();
	}

	public void handleNameTagOnReveal()
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
		RevealManager.OnReveal.AddListener(handleNameTagOnReveal);
	}

	public void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
		RevealManager.OnReveal.RemoveListener(handleNameTagOnReveal);
	}

	public void onReveal()
	{
		RevealManager.setRevealForGameObject(gameObject, getRevealColor());
	}

	public Color getRevealColor()
	{
		return RevealManager.canBeInteractedWith;
	}

	private void spawnNameTag()
	{
		if (nameTag == null && !noNameTag)
		{
			nameTag = Instantiate(Resources.Load<GameObject>(PrefabNames.npcNameTag), transform).GetComponent<DescriptionPanel>();

			nameTag.nameText.text = getName();
		}
	}

    private string getName()
    {
        return GetComponent<DialogueTrigger>().dialogue.names[1].Replace(vaultablePrefix,"");
    }

    private void destroyNameTag()
    {
        if (nameTag != null)
        {
            Destroy(nameTag.gameObject);
            nameTag = null;
        }
    }

	public void spawnTargetCanvas()
	{
		if (targetCanvas == null && GetComponent<SpriteRenderer>() != null && !ignoreHover)
		{
			// gameObject.AddComponent<RectTransform>();
			// targetCanvas = Instantiate(Resources.Load<GameObject>(PrefabNames.targetBox), transform);
		}
	}

	public void createHoverTag()
	{
		//Empty on purpose (may add for things like portcullis controls in mine lvl 2)
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!RevealManager.currentlyRevealed && !ignoreHover && (eventData == null || !eventData.used))
        {
            if (eventData != null)
            {
                eventData.Use();
            }

            RevealManager.setOutlineColor(gameObject, getRevealColor());
            spawnNameTag();
        }
    }

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!RevealManager.currentlyRevealed && !ignoreHover)
		{
			RevealManager.setOutlineColorToDefault(gameObject);
			destroyNameTag();
		}
	}
}
