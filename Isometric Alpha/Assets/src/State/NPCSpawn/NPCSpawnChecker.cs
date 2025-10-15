using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class NPCSpawnChecker : MonoBehaviour, IRevealable
{
	public bool ignoreHover;

	public string name;
	public Dialogue currentDialogue;

	public string dontSpawnUntilFlag;
	public string[] dontSpawnUntilFlags;
	public string stopSpawningAfterFlag;
	public string[] stopSpawningAfterFlags;

	public bool ignoreInPartyForSpawning;
	public bool spawnWhileHostile;
	public bool onlySpawnWhileHostile;

	public bool noNameTag = false;

	public NPCNameTag nameTag;

	public PlayerInteractionScript[] scripts;

	public GameObject targetCanvas;

	private void Awake()
	{
		spawnTargetCanvas();
	}

	private void OnEnable()
	{
		spawnCheck();
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

	// void Start()
	// {
	// 	spawnCheck();
	// }

	public void spawnCheck()
	{
		AreaList.setCurrentArea();

		if (PlayerInteractionScript.evaluateAnyScript(scripts))
		{
			return;
		}

		if (!spawnWhileHostile && AreaList.currentSceneIsHostile())
		{
			gameObject.SetActive(false);
			return;
		}
		else if (onlySpawnWhileHostile && !AreaList.currentSceneIsHostile())
		{
			gameObject.SetActive(false);
			return;
		}

		if (gameObject.GetComponent<DialogueTrigger>() != null)
		{
			currentDialogue = gameObject.GetComponent<DialogueTrigger>().dialogue;
		}

		if ((name == null || name.Length == 0) && currentDialogue != null && currentDialogue.names.Length >= 1)
		{
			name = currentDialogue.names[1].Replace(" ", "");
		}

		if (stopSpawningAfterFlag != null && !stopSpawningAfterFlag.Equals(""))
		{
			if (Flags.getFlag(stopSpawningAfterFlag))
			{
				gameObject.SetActive(false);
				return;
			}
		}

		if (stopSpawningAfterFlags != null && stopSpawningAfterFlags.Length > 0)
		{
			foreach (string flag in stopSpawningAfterFlags)
			{
				if (flag != null && flag.Length > 0 && Flags.getFlag(flag))
				{
					gameObject.SetActive(false);
					return;
				}
			}
		}


		if (name != null && DeathFlagManager.isDead(name))
		{
			gameObject.SetActive(false);
			return;
		}

		if (name != null && name.Length > 0 && !ignoreInPartyForSpawning && PartyManager.nameIsInParty(name))
		{
			gameObject.SetActive(false);
			return;
		}

		if (dontSpawnUntilFlag != null && !dontSpawnUntilFlag.Equals(""))
		{
			if (!Flags.getFlag(dontSpawnUntilFlag))
			{
				gameObject.SetActive(false);
				return;
			}
		}

		if (dontSpawnUntilFlags != null && dontSpawnUntilFlags.Length > 0)
		{
			foreach (string flag in dontSpawnUntilFlags)
			{
				if (!Flags.getFlag(flag))
				{
					gameObject.SetActive(false);
					return;
				}
			}
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
			nameTag = Instantiate(Resources.Load<GameObject>(PrefabNames.oldNPCNameTag), transform).GetComponent<NPCNameTag>();

			nameTag.labelNPC(name);
		}
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
