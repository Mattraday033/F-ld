using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OverHeadIconComponent : MonoBehaviour, IOverHeadIconSource
{
    protected OverHeadIconManager iconManager;

    protected IRevealable revealable;

    public int cunningStunCounter
    {
        get
        {
            return -1;
        }
    }
	public int intimidateCounter
    {
        get
        {
            return -1;
        }
    }
	public int retreatStunCounter
    {
        get
        {
            return -1;
        }
    }

    private void Awake()
    {
        iconManager = GetComponent<ComponentList>().overHeadIconManager;
        revealable = GetComponent<IRevealable>();

        createAllOverheadIcons();
    }

    private void OnEnable()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(createAllOverheadIcons);
    }

    private void OnDisable()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.RemoveListener(createAllOverheadIcons);
    }


    protected abstract void createAllOverheadIcons();

    public virtual Color getRevealColor()
    {
        return ColorList.canBeInteractedWith;
    }

	public virtual void onReveal(bool toggleReveal)
	{
        if(revealable != null && !RevealManager.currentlyRevealed)
        {
            revealable.onReveal(toggleReveal);
        }
	}

    public string getIntimidatedDescriptionKey()
    {
        return HoverMessageList.intimidatedShopkeeperKey;
    }

}

public class RestStop : OverHeadIconComponent
{
    protected override void createAllOverheadIcons()
    {
        if(shouldRevealRestStopIcon())
        {
            iconManager.createOverHeadIcon(OverHeadIconType.RestStop, this);
        }
    }

    private bool shouldRevealRestStopIcon()
    {
        return RestAndShopMapLocationList.locationHasRestPoint(AreaManager.locationName);
    }

}