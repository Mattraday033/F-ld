using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Attack : CombatAction, IJSONConvertable
{
	private Weapon mainHandWeapon;

	public Attack():
    base(null, null)
	{

	}

    public Attack(Weapon mainHandWeapon) :
    base(null, null)
    {
        this.mainHandWeapon = (Weapon) mainHandWeapon.clone();
        mainHandWeapon.setQuantity(1);
    }

    public Attack(Stats actor, Selector selector, Weapon mainHandWeapon) : base(actor, selector)
    {
        this.mainHandWeapon = (Weapon) mainHandWeapon.clone();
        mainHandWeapon.setQuantity(1);
        this.mainHandWeapon.equipTarget = actor;
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

		if(CombatStateManager.whoseTurn == WhoseTurn.Resolving && getSourceItem().appliesStanceStacks())
		{
            Stance.OnStanceApplyingWeaponAttack?.Invoke();
        }
	}

	public override string getKey()
	{
		return getMainHandWeapon().getKey();
	}

	public override string getIconName()
	{
		return getMainHandWeapon().getIconName();
	}

	public override string getName()
	{
        return getMainHandWeapon().getKey();
	}

	public override int getSaveType()
	{
		return (int)CombatActionSaveType.Attack;
	}

	public override Item getSourceItem()
	{
		return (Item) mainHandWeapon;
	}

	public override void setSourceItem(Item sourceItem)
	{
		mainHandWeapon = (Weapon)sourceItem;
	}

	public override int getRangeIndex()
	{
		return getMainHandWeapon().getRangeIndex();
	}

	public override string getRangeTitle()
	{
		return getMainHandWeapon().getRange();
	}

	public override string getUseDescription()
	{
		return getMainHandWeapon().getLoreDescription();
	}

	public override string getDamageFormula()
	{
		if (getMainHandWeapon().getIsTwoHanded())
		{
            return getMainHandWeapon().getDamageFormula();
		}
		else
		{
			string[] formulas = new string[] { getMainHandWeapon().getDamageFormula(), getOffHandWeapon().getDamageFormula() };
			return DamageCalculator.combineFormulas(formulas);
		}
	}

	public override string getCritFormula()
	{
		if (inPreviewMode)
		{
			return "0";
		}

		if (getMainHandWeapon().getIsTwoHanded())
		{
			return getMainHandWeapon().getCritFormula() + getActorStats().getBonusCritChance();
		}
		else
		{
			string[] formulas = new string[] { getMainHandWeapon().getCritFormula(), getOffHandWeapon().getCritFormula(), getActorStats().getBonusCritChance() };
			return DamageCalculator.combineFormulas(formulas);
		}
	}

	public override bool takesAWeaponSlot()
	{
		return true;
	}

	public override string getDisplayType()
	{
		return "Attack";
	}

	public override int getMaximumSlots()
	{
		return OverallUIManager.getCurrentPartyMember().getWeaponSlots();
	}

	public override bool hasAvailableSlots(CombatActionArray combatActionArray)
	{
		return combatActionArray.hasAvailableWeaponSlots();
	}

	//convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
	public override string convertToJson()
	{
		string itemJson = mainHandWeapon.convertToJson();

		return mainHandWeapon.convertToJson().Substring(0, itemJson.Length - 1) + ",\"CombatActionSaveType\":\"" + getSaveType() + "}";
	}

    public override bool canBePlacedInPassiveSlot()
    {
        return true;
    }

    public override void setActor(Stats actor)
    {
        base.setActor(actor);
        EquippableItem item = getSourceItem() as EquippableItem;

        item.equipTarget = actor;
    }

    // public static bool hasAvailableWeaponSlots()
    // {
    //     return hasAvailableWeaponSlots(State.CombatActionArray);
    // }

    // public static bool hasAvailableWeaponSlots(IActionArrayStorage actionArrayStorage)
    // {
    // 	return hasAvailableWeaponSlots(actionArrayStorage.getStoredCombatActionArray());
    // }

    // public static bool hasAvailableWeaponSlots(CombatAction[] actions)
    // {
    // 	return getAmountOfWeaponCombatActions(actions) < State.playerStats.getWeaponSlots();
    // }

    // public static int getAmountOfWeaponCombatActions()
    // {
    // 	return getAmountOfWeaponCombatActions(State.CombatActionArray);
    // }

    // public static int getAmountOfWeaponCombatActions(IActionArrayStorage actionArrayStorage)
    // {

    // 	return getAmountOfWeaponCombatActions(actionArrayStorage.getStoredCombatActionArray());
    // }

    // public static int getAmountOfWeaponCombatActions(CombatAction[] actions)
    // {
    // 	int amountOfWeapons = 0;

    // 	foreach (CombatAction action in actions)
    //     {
    //         if (action == null)
    //         {
    //             continue;
    //         }

    //         if (action.takesAWeaponSlot())
    //         {
    //             amountOfWeapons++;
    //         }
    //     }

    // 	return amountOfWeapons;
    // }

    public override int getQuantity()
    {
        return 1;
    }

	private Weapon getMainHandWeapon()
	{
		return (Weapon)getSourceItem();
	}

    private Weapon getOffHandWeapon()
    {
        return (Weapon) getActorStats().getEquippedItems().getOffHand();
    }


	//IDescribable methods
    public override GameObject getRowType(RowType rowType)
    {
        string rowTypeName = "";

        switch (rowType)
        {
            case RowType.Standard:
            case RowType.Equipment:
            case RowType.StatRequirements:
            case RowType.AbilityEditor:
                return getSourceItem().getRowType(rowType);
                break;
            default:
                return base.getRowType(rowType);
        }

        return Resources.Load<GameObject>(rowTypeName);
    }

	public override GameObject getDescriptionPanelFull()
	{
		return getSourceItem().getDescriptionPanelFull();
	}

	public override GameObject getDecisionPanel()
	{
		return getSourceItem().getDecisionPanel();
	}

	public override GameObject getDescriptionPanelFull(PanelType panelType)
	{
		string panelTypeName = "";

		switch (panelType)
		{
			case PanelType.Standard:

				return getSourceItem().getDescriptionPanelFull();

			case PanelType.AbilityEditor:

				if (getMainHandWeapon().getIsTwoHanded())
				{
					return base.getDescriptionPanelFull(panelType);
				}
				else
				{
					panelTypeName = PrefabNames.dualWieldCombatActionDescPanelFull;
				}

				break;
			default:
				return base.getDescriptionPanelFull(panelType);
		}

        Debug.LogError("rowTypeName = " + panelTypeName);


        return DescriptionPanel.getDescriptionPanel(panelTypeName);
	}

	public override bool withinFilter(string[] filterParameters)
	{
		return getSourceItem().withinFilter(filterParameters);
	}

	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);
		getSourceItem().describeSelfFull(panel);

		if (CombatStateManager.inCombat) //will use the item classes damage/crit display methods if not in combat
		{
			DescriptionPanel.setText(panel.damageText, getDamageTotalForDisplay());
			DescriptionPanel.setText(panel.critRatingText, getCritTotalForDisplay());
		}

		if (panel.amountText != null && !(panel.amountText is null))
		{
			panel.amountText.text = getMaximumSlots() + " Slot(s)";
		}

		panel.setObjectBeingDescribed(this);
	}

	public override void describeSelfRow(DescriptionPanel panel)
	{
		getSourceItem().describeSelfRow(panel);

		panel.setObjectBeingDescribed(this);
	}

	public override void setUpDecisionPanel(IDecisionPanel descisionPanel)
	{
		getSourceItem().setUpDecisionPanel(descisionPanel);
	}
	
	//IDescribableInBlocks methods
	public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{

		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(getDamageTotalForDisplay(), getDamageFormulaForDisplayAlternate()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getCritBlock(getCritTotalForDisplay(), getCritFormulaForDisplayAlternate()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getRangeBlock(getRangeTitle()));

		if (getAppliedTrait() != null)
		{
			buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getAppliedTrait().getMaxRoundsLeftForDisplay()));
		}

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getUseDescription()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getSourceItem().getSlotIconName()));

		if (getSourceItem().appliesStanceStacks())
		{
			buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, IconList.stanceWeaponIconName));
		}

		//buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getCritTotalForDisplay()));

		return buildingBlocks;

	}
}
