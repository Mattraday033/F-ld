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

    public SpriteOutline getSpriteOutline()
    {
        return null;
    }

	public void onReveal(bool toggleReveal)
	{
		// RevealManager.setRevealForGameObject(gameObject, getRevealColor());
	}

	public Color getRevealColor()
	{
		return ColorList.canBeInteractedWith;
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
			// RevealManager.setOutlineColor(gameObject, getRevealColor());
			PlayerObject.toggleButtonPrompt(false);
			createHoverTag();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		PlayerObject.restoreButtonPrompt();

		if (!RevealManager.currentlyRevealed)
		{
			// RevealManager.setOutlineColorToDefault(gameObject);
		}

		MouseHoverManager.destroyMouseHoverBase();
	}
	
}
