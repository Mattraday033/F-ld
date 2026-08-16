using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TraitApplicationItem : CombatItem, IJSONConvertable
{
	private const string subtype = "TraitApplication";
	private Trait traitToApply;

	public TraitApplicationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, SelectorTemplate rangeTemplate, Trait traitToApply, bool useRequiresAnAction, bool targetsEnemySection = false, bool healsTarget = false) :
    base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeTemplate, useRequiresAnAction, targetsEnemySection, healsTarget)
	{
		this.traitToApply = traitToApply;
	}

	public TraitApplicationItem(ItemListID listId, string key, string loreDescription, string useDescription, string iconName, int worth, SelectorTemplate rangeTemplate, Trait traitToApply, bool useRequiresAnAction, int quantity, bool targetsEnemySection = false, bool healsTarget = false) :
    base(listId, key, loreDescription, useDescription, subtype, iconName, worth, rangeTemplate, useRequiresAnAction, quantity, targetsEnemySection, healsTarget)
	{
		this.traitToApply = traitToApply;
	}

	public override void use(Stats target)
	{
        if (!fitsUseCriteria(target))
        {
            return;
        }

        PlaySFXLogic();

        if(healsTarget)
        {
            target.modifyCurrentHealth(getAmountToHeal(), healsTarget); 
        }

        target.addTrait(traitToApply);
	}

    public override int getAmountToHeal()
    {
        if(healsTarget) 
        {
            return DamageCalculator.calculateFormula(getDamageFormula(), getStatSource());
        }

        return base.getAmountToHeal();
    }

	public override string getEffectAnimationType()
	{
        if(traitToApply.isBuff())
        {
		    return EffectAnimationType.Positive.ToString(); 
        } else
        {
		    return EffectAnimationType.Negative.ToString();
        }
	}

    public override Trait getAppliedTrait()
    {
        return traitToApply;
    }

    public override bool fitsUseCriteria(Stats stats)
    {
        return CombatStateManager.inCombat;
    }
	
}
