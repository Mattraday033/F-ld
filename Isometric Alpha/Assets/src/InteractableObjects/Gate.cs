using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gate : MonoBehaviour, IRevealable
{
    private string gateKey;
    public string hoverName;

    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setKey(string gateKey)
    {
        this.gateKey = gateKey;

        checkGateStatus();
    }

    private void checkGateStatus()
    {
        if (GateAndChestManager.hasBeenOpened(gateKey))
        {
            gameObject.SetActive(false);
        }
    }

    public string getGateKey()
    {
        return gateKey;
    }

	private void OnEnable()
	{
		createListeners();
	}

	private void OnDisable()
	{
		destroyListeners();
	}

	//IRevealable interface methods

	public void createListeners()
	{
        RevealManager.OnReveal.AddListener(onReveal);
        GateAndChestManager.OnGateKeyAdd.AddListener(checkGateStatus);
	}

	public void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
        GateAndChestManager.OnGateKeyAdd.RemoveListener(checkGateStatus);
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
