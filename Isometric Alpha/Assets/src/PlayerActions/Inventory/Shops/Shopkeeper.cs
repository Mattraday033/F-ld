using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour, IOverHeadIconSource, ISkillTarget
{
    public const bool requestNormalShopInventory = false;
    public const bool requestBuyBackInventory = true;

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

    public bool equipmentDefault;
    public string shopkeeperInventoryKey;
    private OverHeadIconManager iconManager;

    private IRevealable revealable;


    private void Awake()
    {
        iconManager = GetComponent<ComponentList>().overHeadIconManager;
        createShopkeeperIcon();
        createIntimidatedIcon();

        revealable = GetComponent<IRevealable>();
    }

    private void OnEnable()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(createShopkeeperIcon);
    }

    private void OnDisable()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.RemoveListener(createShopkeeperIcon);
    }

    public virtual float getDiscount()
    {
        return DiscountList.getDiscount(shopkeeperInventoryKey);
    }

    public Dictionary<string, Item> getInventory()
    {
        return ShopkeeperInventoryList.getShopkeeperInventory(getShopkeeperInventoryKey(), requestNormalShopInventory);
    }

    public Dictionary<string, Item> getBuyBackInventory()
    {
        return ShopkeeperInventoryList.getShopkeeperInventory(getShopkeeperInventoryKey(), requestBuyBackInventory);
    }

    public virtual string getShopkeeperInventoryKey()
    {
        if (shopkeeperInventoryKey != null && shopkeeperInventoryKey.Length > 0)
        {
            return shopkeeperInventoryKey;
        }
        else
        {
            shopkeeperInventoryKey = gameObject.GetComponent<DialogueTrigger>().getDialogue().getName();

            return shopkeeperInventoryKey;
        }
    }

    public virtual bool shouldRevealShopkeeperIcon()
    {
        return ShopkeeperInventoryList.getShopkeeperRevealStatus(getShopkeeperInventoryKey());
    }

    public void createShopkeeperIcon()
    {
        if(shouldRevealShopkeeperIcon())
        {
            iconManager.createOverHeadIcon(OverHeadIconType.Shopkeeper, this);
        }
    }

    public void createIntimidatedIcon()
    {
        if(ShopkeeperInventoryList.getShopkeeperIntimidatedFlag(shopkeeperInventoryKey))
        {
            iconManager.createOverHeadIcon(OverHeadIconType.Intimidate, this);
        }
    }

    public int getIntimidateCounter()
    {
        return -1;
    }
    public int getCunningCounter()
    {
        return -1;
    }
    public int getRetreatCounter()
    {
        return -1;
    }

    public Color getRevealColor()
    {
        return ColorList.canBeInteractedWith;
    }

	public void onReveal(bool toggleReveal)
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

    #region ISkillTarget Methods

    public int getChargeCost(SkillType skillType)
    {
        switch(skillType)
        {
            default:
                return Constants.sizeOne;
        }
    }
    
    public bool validTarget(SkillType skillType)
    {
        return skillType == SkillType.Intimidate && !ShopkeeperInventoryList.getShopkeeperIntimidatedFlag(shopkeeperInventoryKey);
    }

    public void cunning()
    {
        
    }

    public void intimidate()
    {
        ShopkeeperInventoryList.setShopkeeperIntimidatedFlag(shopkeeperInventoryKey);
        createIntimidatedIcon();
        AreaList.addHostility();
    }

	public Vector3 getTargetPosition()
	{
		return transform.position;
	}

    #endregion

}