using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//[System.Serializable]
public class TraitApplicationItem : CombatItem, IJSONConvertable
{
	private const string subtype = "TraitApplication";
	private Trait traitToApply;

	public TraitApplicationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, int rangeIndex, string traitKeyToApply, bool useRequiresAnAction) : base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeIndex, useRequiresAnAction)
	{
		this.traitToApply = TraitList.getTrait(traitKeyToApply);
	}

	public TraitApplicationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, int rangeIndex, string traitKeyToApply, bool useRequiresAnAction, int quantity) : base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeIndex, useRequiresAnAction, quantity)
	{
		this.traitToApply = TraitList.getTrait(traitKeyToApply);
	}

	public override void use(Stats target)
	{
        if (!fitsUseCriteria(target))
        {
            return;
        }

        target.addTrait(traitToApply);
	}

    public override bool fitsUseCriteria(Stats stats)
    {
        return CombatStateManager.inCombat;
    }
	
}
