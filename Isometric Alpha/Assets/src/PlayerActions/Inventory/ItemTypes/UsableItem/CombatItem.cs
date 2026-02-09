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

	private int rangeIndex;
	private bool itemUseRequiresAnAction;

	public CombatItem(ItemListID listID, string key, string loreDescription, string useDescription, string subtype, string iconName, int worth, int rangeIndex, bool useRequiresAnAction, bool targetsEnemySection = false, bool healsTarget = false) : base(listID, key, loreDescription, useDescription, subtype, iconName, worth)
	{
		this.rangeIndex = rangeIndex;
		this.itemUseRequiresAnAction = useRequiresAnAction;
		this.targetsEnemySection = targetsEnemySection;		
		this.healsTarget = healsTarget;		
	}

	public CombatItem(ItemListID listID, string key, string loreDescription, string useDescription, string subtype, string iconName, int worth, int rangeIndex, bool useRequiresAnAction, int quantity, bool targetsEnemySection = false, bool healsTarget = false) : base(listID, key, loreDescription, useDescription, subtype, iconName, worth, quantity)
	{
		this.rangeIndex = rangeIndex;
		this.itemUseRequiresAnAction = useRequiresAnAction;
		this.targetsEnemySection = targetsEnemySection;		
		this.healsTarget = healsTarget;		
	}

	public override int getRangeIndex()
	{
		return rangeIndex;
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
