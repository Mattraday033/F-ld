using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class HealingItem: UsableItem, IJSONConvertable
{
	public const string typeIconName = "Healing Item";
	public const string subtype = "Healing";
	public const bool treatAmountAsHealing = true;
	private int amountToHeal;
	
	public HealingItem(ItemListID listId, string key, string loreDescription, string iconName, int worth, int amountToHeal, PlaySFXLogic OOCOnUseSFX = null): 
    base(listId, key, loreDescription, generateUseDescription(amountToHeal), subtype, iconName, worth, OOCOnUseSFX) 
	{

		this.amountToHeal = amountToHeal;
	}
	
	public HealingItem(ItemListID listId, string key, string loreDescription, string iconName, int worth, int amountToHeal, int quantity, PlaySFXLogic OOCOnUseSFX = null): 
    base(listId, key, loreDescription, generateUseDescription(amountToHeal), subtype, iconName, worth, quantity, OOCOnUseSFX) 
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

        PlaySFXLogic();
 
        target.modifyCurrentHealth(getAmountToHeal(), treatAmountAsHealing);
	}
	

	public override string getEffectAnimationType()
	{
		return EffectAnimationType.Healing.ToString();
	}

	public override bool usableOutOfCombat()
	{
		return true;
	}

	public override bool usableInCombat()
	{
		return true;
	}

    public override bool targetsAllySection()
    {
        return true;
    }

    public override bool fitsUseCriteria(Stats target)
    {
        return target != null && target.currentHealth < target.getTotalHealth();
    }
	
	public override string getTypeIconName() 
	{
		return typeIconName;
	}

    public override void describeSelfFull(DescriptionPanel panel)
    {
        base.describeSelfFull(panel);

        if (panel.damageText != null && !(panel.damageText is null))
        {
            panel.damageText.text = getDamageFormula();
        }
    }
    
    private static string generateUseDescription(int healingAmount)
    {
        return "Heals " + healingAmount + " hp.";
    }
}
