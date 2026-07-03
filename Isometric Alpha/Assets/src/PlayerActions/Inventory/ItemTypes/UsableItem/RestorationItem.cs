using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class RestorationItem : CombatItem, IJSONConvertable
{
    private const string subtype = "Restoration";

    private TraitType traitTypeToRemove;

    public RestorationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, string rangeIndex, TraitType traitTypeToRemove, PlaySFXLogic OOCOnUseSFX = null) :
    base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeIndex, useDoesNotRequireAnAction, OOCOnUseSFX: OOCOnUseSFX)
    {
        this.traitTypeToRemove = traitTypeToRemove;
    }

    public RestorationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, string rangeIndex, TraitType traitTypeToRemove, int quantity, PlaySFXLogic OOCOnUseSFX = null) :
     base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeIndex, useDoesNotRequireAnAction, quantity, OOCOnUseSFX: OOCOnUseSFX)
    {
        this.traitTypeToRemove = traitTypeToRemove;
    }

	public override string getEffectAnimationType()
	{
		return EffectAnimationType.Healing.ToString();
	}

    public string getTraitTypeToRemove()
    {
        return traitTypeToRemove.ToString();
    }

    public override void describeSelfFull(DescriptionPanel panel)
    {
        base.describeSelfFull(panel);

        if (panel.damageText != null && !(panel.damageText is null))
        {
            panel.damageText.text = "Removes 1 " + getTraitTypeToRemove() + " Trait";
        }
    }

    public override void use(Stats target)
    {
        if (!fitsUseCriteria(target))
        {
            return;
        }

        PlaySFXLogic();
        
        target.traitContainer.removeAllTraitsOfType(traitTypeToRemove);
    }

    public override bool fitsUseCriteria(Stats stats)
    {
        return CombatStateManager.inCombat;
    }
	
}
