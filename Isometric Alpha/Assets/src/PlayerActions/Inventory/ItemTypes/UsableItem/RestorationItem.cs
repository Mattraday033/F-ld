using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class RestorationItem : CombatItem, IJSONConvertable
{
    private const string subtype = "Restoration";

    private TraitType traitTypeToRemove;

    public RestorationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, int rangeIndex, TraitType traitTypeToRemove) : base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeIndex, CombatItem.useDoesNotRequireAnAction)
    {
        this.traitTypeToRemove = traitTypeToRemove;
    }

    public RestorationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, int rangeIndex, TraitType traitTypeToRemove, int quantity) : base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeIndex, CombatItem.useDoesNotRequireAnAction, quantity)
    {
        this.traitTypeToRemove = traitTypeToRemove;
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

        target.traitContainer.removeAllTraitsOfType(traitTypeToRemove);
    }

    public override bool fitsUseCriteria(Stats stats)
    {
        return CombatStateManager.inCombat;
    }
	
}
