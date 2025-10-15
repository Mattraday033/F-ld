using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CompanionAttack : CombatAction
{
	private string name;
	private string key;
	private string iconName;
	private int rangeIndex;
	private string useDescription;
	
	public CompanionAttack(string key, string name, string iconName, int rangeIndex, string useDescription):
    base(null, null)
	{
		this.key = key;
		this.name = name;
		this.iconName = iconName;
		this.rangeIndex = rangeIndex;
		this.useDescription = useDescription;
	}

	public CompanionAttack(Stats actor, Selector selector,string key, string name, string iconName, int rangeIndex): base(actor, selector)
	{
		this.key = key;
		this.name = name;
		this.iconName = iconName;
		this.rangeIndex = rangeIndex;
	}

	public override int getLevel()
	{
		return PartyManager.getPartyMember(key).stats.getLevel();
	}

	public override int getQuantity()
	{
		return 1;
	}

	public override string getKey()
	{
		return key;
	}
	
	public override string getIconName()
	{
		if(iconName == null)
		{
			return PartyMemberEquipmentManager.getWeapon(name, getLevel()).getIconName();
		} else
		{
			return iconName;
		}
		
	}
    public override void performCombatAction(ArrayList targets)
    {
        base.performCombatAction(targets);

        if (inPreviewMode)
        {
            return;
        }

        foreach (Stats targetCombatant in targets)
        {
            if (targetCombatant != null)
            {
                Exuberances.addExuberance(MultiStackProcType.RedKnife, singleExuberanceStack);
            }
        }
    }

    public override string getName()
	{
		return name;
	}
	
	public override Item getSourceItem()
	{
		return PartyMemberEquipmentManager.getWeapon(key, getLevel());
	}
	
	public override int getRangeIndex()
	{
		return rangeIndex;
	}

	public override string getRangeTitle()
	{
		return Range.getRangeTitle(rangeIndex);
	}

	public override string getDamageFormula()
	{
		return PartyMemberEquipmentManager.getWeapon(key, getLevel()).getDamageFormula();
	}
	
	public override string getCritFormula()
	{
        if (inPreviewMode)
        {
            return "0";
        }

        return PartyMemberEquipmentManager.getWeapon(key, getLevel()).getCritFormula();
	}
	
	public override bool takesAWeaponSlot()
	{
		return true;
	}

	public override string getDisplayType()
	{
		return "Attack";
	}

	public override string getUseDescription()
	{
		return useDescription;
	}

}
