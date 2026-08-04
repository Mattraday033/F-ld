using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : OverHeadIconComponent, ISkillTarget
{
    public const bool requestNormalShopInventory = false;
    public const bool requestBuyBackInventory = true;

    public bool equipmentDefault;
    public string shopkeeperInventoryKey;

    // private void Awake()
    // {
    //     iconManager = GetComponent<ComponentList>().overHeadIconManager;
    //     createShopkeeperIcon();
    //     createIntimidatedIcon();

    //     revealable = GetComponent<IRevealable>();
    // }

    protected override void createAllOverheadIcons()
    {
        createShopkeeperIcon();
        createIntimidatedIcon();
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

    public bool shouldRevealShopkeeperIcon()
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