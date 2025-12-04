using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gate : MonoBehaviour, IRevealable
{
    private string gateKey;
    public string hoverName;

    public SpriteRenderer spriteRenderer;
    public SpriteOutline outline;

    protected virtual void Awake()
    {
        outline = new SpriteOutline();
        spriteRenderer = GetComponent<SpriteRenderer>();
        outline.setSpriteRenderer(spriteRenderer);
    }

    public void setKey(string gateKey)
    {
        this.gateKey = gateKey;

        checkGateStatus();
    }

    public virtual void checkGateStatus()
    {
        if (GateAndChestManager.hasBeenOpened(getGateKey()))
        {
            gameObject.SetActive(false);
        }
    }

    public string getGateKey()
    {
        return AreaManager.locationName+gateKey;
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

    public SpriteOutline getSpriteOutline()
    {
        return outline;
    }

	public virtual void createListeners()
	{
        RevealManager.OnReveal.AddListener(onReveal);
        GateAndChestManager.OnGateKeyAdd.AddListener(checkGateStatus);
	}

	public virtual void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
        GateAndChestManager.OnGateKeyAdd.RemoveListener(checkGateStatus);
	}

	public void onReveal(bool toggleReveal)
	{
        if(toggleReveal)
        {
            outline.createOutline(getRevealColor());
        } else
        {
            outline.removeOutline();
        }
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
            outline.createOutline(getRevealColor());
			createHoverTag();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!RevealManager.currentlyRevealed)
		{
			outline.removeOutline();
		}

		MouseHoverManager.destroyMouseHoverBase();
	}
	
}
