using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class CombatItem : UsableItem, IJSONConvertable
{
	public const bool useDoesRequireAnAction = true;
	public const bool useDoesNotRequireAnAction = false;

    private bool targetsEnemySection = false;
    protected bool healsTarget = false;

	private string rangeName;
	private bool itemUseRequiresAnAction;

	public CombatItem(ItemListID listID, string key, string loreDescription, string useDescription, string subtype, string iconName, int worth, string rangeName, bool useRequiresAnAction, bool targetsEnemySection = false, bool healsTarget = false, PlaySFXLogic OOCOnUseSFX = null) :
    base(listID, key, loreDescription, useDescription, subtype, iconName, worth, OOCOnUseSFX)
	{
		this.rangeName = rangeName;
		this.itemUseRequiresAnAction = useRequiresAnAction;
		this.targetsEnemySection = targetsEnemySection;		
		this.healsTarget = healsTarget;		
	}

	public CombatItem(ItemListID listID, string key, string loreDescription, string useDescription, string subtype, string iconName, int worth, string rangeName, bool useRequiresAnAction, int quantity, bool targetsEnemySection = false, bool healsTarget = false, PlaySFXLogic OOCOnUseSFX = null) :
    base(listID, key, loreDescription, useDescription, subtype, iconName, worth, quantity, OOCOnUseSFX)
	{
		this.rangeName = rangeName;
		this.itemUseRequiresAnAction = useRequiresAnAction;
		this.targetsEnemySection = targetsEnemySection;		
		this.healsTarget = healsTarget;		
	}

    public override void use(Stats stats)
    {
        //Empty on purpose
    }

	public override string getRangeName()
	{
		return rangeName;
	}

    public override bool targetsAllySection()
    {
        return !targetsEnemySection;
    }

	public override bool usableInCombat()
	{
		return true;
	}

	public override bool usableOutOfCombat()
	{
		return false;
	}

	public override bool useRequiresAnAction()
	{
		return itemUseRequiresAnAction;
	}

}
