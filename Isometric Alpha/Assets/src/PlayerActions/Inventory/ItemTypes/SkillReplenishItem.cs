using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SkillReplenishItem: UsableItem, IJSONConvertable
{
    public const string subtype = "SkillReplenish";

	public SkillReplenishItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, PlaySFXLogic OOCOnUseSFX = null): 
    base(listId, key, loreDescription, useDescription, subtype, iconName, worth, OOCOnUseSFX) 
	{

	}
	
	public SkillReplenishItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, int quantity, PlaySFXLogic OOCOnUseSFX = null): 
    base(listId, key, loreDescription, useDescription, subtype, iconName, worth, quantity, OOCOnUseSFX) 
	{

	}
	
	public override void use(Stats target)
	{
        if (!fitsUseCriteria(target))
        {
            return;
        }

        PlaySFXLogic();

		CunningManager.addCunningsRemaining(Constants.sizeOne);
	}
	
	public override bool usableOnParty()
	{
		return false;
	}
	
	public override bool usableOutOfCombat()
	{
		return true;
	}
	
	public override bool fitsUseCriteria(Stats stats)
	{
		return stats == PartyManager.getPlayerStats() && CunningManager.getCunningsRemaining() < PartyStats.getMaxCunningCount();
	}
	
	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);
		
		if(panel.damageText != null && !(panel.damageText is null))
		{
			panel.damageText.text = "+1 Cunning Use";
		}
	}
	
}
