using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class HealingItem: UsableItem, IJSONConvertable
{
	public const string typeIconName = "HealingItem";
	public const string subtype = "Healing";
	public const bool treatAmountAsHealing = true;
	private int amountToHeal;
	
	public HealingItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, int amountToHeal): base(listId, key, loreDescription, useDescription, subtype, iconName, worth) 
	{

		this.amountToHeal = amountToHeal;
	}
	
	public HealingItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, int amountToHeal, int quantity): base(listId, key, loreDescription, useDescription, subtype, iconName, worth, quantity) 
	{
		
		this.amountToHeal = amountToHeal;
	}
	
	public override int getAmountToHeal()
	{
		return amountToHeal;
	}
	
	public override void use(Stats target)
	{
        if (!fitsUseCriteria(target))
        {
            return;
        }

		target.modifyCurrentHealth(getAmountToHeal(), treatAmountAsHealing);
	}
	
	public override bool usableOutOfCombat()
	{
		return true;
	}

	public override bool usableInCombat()
	{
		return true;
	}

    public override bool fitsUseCriteria(Stats target)
    {
        return target.currentHealth < target.getTotalHealth();
    }
	
	public override string getTypeIconName() 
	{
		return typeIconName;
	}

	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);
		
		if(panel.damageText != null && !(panel.damageText is null))
		{
			panel.damageText.text = getDamageFormula();
		}
	}
}
