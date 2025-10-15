using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GateSpawnChecker : MonoBehaviour, IRevealable
{
	public string hoverName = "Gate";
	public bool exactDimensionsHover = false;

	public string gateKey;
	public GameObject targetCanvas;

	private void Awake()
	{
		spawnTargetCanvas();
	}

	void Start()
	{
		if (GateAndChestManager.hasBeenOpened(gateKey))
		{
			gameObject.SetActive(false);
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

	public void setToOpened()
	{
		gameObject.SetActive(false);
	}

	public void setToOpenedPermanently()
	{
		setToOpened();
		GateAndChestManager.addKey(gateKey);
	}

	//IRevealable interface methods

	public void createListeners()
	{
		RevealManager.OnReveal.AddListener(onReveal);
	}

	public void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
	}

	public void onReveal()
	{
		RevealManager.setRevealForGameObject(gameObject, getRevealColor());
	}

	public Color getRevealColor()
	{
		return RevealManager.canBeInteractedWith;
	}

	public void spawnTargetCanvas()
	{
		if (targetCanvas == null)
		{
			gameObject.AddComponent<RectTransform>();
			if (exactDimensionsHover)
			{
				targetCanvas = Instantiate(Resources.Load<GameObject>(PrefabNames.targetCanvas), transform);
			}
			else
			{
				targetCanvas = Instantiate(Resources.Load<GameObject>(PrefabNames.targetBox), transform);
			}

		}
	}

	public void createHoverTag()
	{
		MouseHoverManager.getMouseHoverBase();
		MouseHoverManager.createHoverTag(hoverName);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!RevealManager.currentlyRevealed)
		{
			RevealManager.setOutlineColor(gameObject, getRevealColor());
			createHoverTag();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!RevealManager.currentlyRevealed)
		{
			RevealManager.setOutlineColorToDefault(gameObject);
		}

		MouseHoverManager.destroyMouseHoverBase();
	}
	
}
