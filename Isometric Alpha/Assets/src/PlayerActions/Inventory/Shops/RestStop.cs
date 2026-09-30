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
        //spawn behaviours add the icon manager beside this component; invisible extra spaces have none
        iconManager = GetComponent<OverHeadIconManager>();
        revealable = GetComponent<IRevealable>();
    }

    //waits for Start so the spawn behaviour can finish setting fields, like the shopkeeper's inventory key, after AddComponent
    private void Start()
    {
        tryCreateAllOverheadIcons();
    }

    private void OnEnable()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(tryCreateAllOverheadIcons);
    }

    private void OnDisable()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.RemoveListener(tryCreateAllOverheadIcons);
    }

    private void tryCreateAllOverheadIcons()
    {
        if(iconManager != null)
        {
            createAllOverheadIcons();
        }
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