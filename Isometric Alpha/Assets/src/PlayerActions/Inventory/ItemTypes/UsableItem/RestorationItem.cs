using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//[System.Serializable]
public class RestorationItem : CombatItem, IJSONConvertable
{
    private const string subtype = "Restoration";

    private string traitTypeToRemove;

    public RestorationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, int rangeIndex, string traitTypeToRemove) : base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeIndex, CombatItem.useDoesNotRequireAnAction)
    {
        this.traitTypeToRemove = traitTypeToRemove;
    }

    public RestorationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, int rangeIndex, string traitTypeToRemove, int quantity) : base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeIndex, CombatItem.useDoesNotRequireAnAction, quantity)
    {
        this.traitTypeToRemove = traitTypeToRemove;
    }

    public string getTraitTypeToRemove()
    {
        return traitTypeToRemove;
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

        for (int traitIndex = 0; traitIndex < target.traits.Length; traitIndex++)
        {
            if (target.traits[traitIndex].getType().Equals(traitTypeToRemove))
            {
                target.removeTrait(target.traits[traitIndex]);
                return;
            }

            //traitIndex--;
        }
    }

    public override bool fitsUseCriteria(Stats stats)
    {
        return CombatStateManager.inCombat;
    }
	
}
