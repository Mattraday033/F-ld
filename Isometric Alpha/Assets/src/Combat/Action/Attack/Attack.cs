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

    public Attack(Stats actor, Weapon mainHandWeapon) :
    base(actor, null)
    {
        this.mainHandWeapon = (Weapon) mainHandWeapon.clone();
        this.mainHandWeapon.setQuantity(1);
        this.mainHandWeapon.equipTarget = actor;
    }

	public override void performCombatAction(List<Stats> targets)
	{
		if (!inPreviewMode)
		{
            foreach (Stats targetCombatant in targets)
            {
                if (targetCombatant != null && targetCombatant.isAlive())
                {
                    Exuberances.addExuberance(MultiStackProcType.RedKnife, singleExuberanceStack);
                }
            }
        }
        
		base.performCombatAction(targets);

		if (inPreviewMode)
		{
			return;
		}

		if(CombatStateManager.whoseTurn == WhoseTurn.Resolving && getSourceItem().appliesStanceStacks())
		{
            Stance.OnStanceApplyingWeaponAttack?.Invoke();
        }

        if(mainHandWeapon != null && mainHandWeapon.getIsTwoHanded())
		{
            AllyStats actor = getActorStats() as AllyStats;

            if(actor != null && actor.equippedItems != null && actor.equippedItems.getOffHand() != null)
            {
                getActorStats().addTrait(new AntiShieldTrait(actor.equippedItems.getOffHand()));
            }
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

	public override Trait getAppliedTrait()
	{
		if (mainHandWeapon == null || mainHandWeapon.getAppliedTrait() == null)
		{
			return null;
		}
		else
		{

            Trait traitToApply = mainHandWeapon.getAppliedTrait();
            traitToApply.traitApplier = getActorStats();

			return traitToApply;	
		}
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
        return getMainHandWeapon().getDamageFormula();
	}

	public override string getCritFormula()
	{
        return getMainHandWeapon().getCritFormula();
	}

    protected override string gatherAllNonActionFormulas(FormulaDelegate<StatBoostSource> getFormula)
    {
        string allStats = base.gatherAllNonActionFormulas(getFormula);

		if (getMainHandWeapon().getIsTwoHanded())
		{
            string invertedOffHandWeaponFormula = DamageCalculator.invertFormula(getFormula(getOffHandWeapon()));

            allStats = DamageCalculator.combineFormulas(invertedOffHandWeaponFormula, allStats);
		}

        return allStats;
    }

	public override bool takesAWeaponSlot()
	{
		return true;
	}

    public override void playEquipSFX()
    {
        Weapon sourceWeapon = getSourceItem() as Weapon;

        sourceWeapon.playEquipSFX();
    }

    public override void playUnequipSFX()
    {
        playEquipSFX();
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

    public override string getEffectAnimationType()
    {
        return getSourceItem().getEffectAnimationType();
    }

    public override int getQuantity()
    {
        return 1;
    }

	private Weapon getMainHandWeapon()
	{
		return (Weapon)getSourceItem();
	}

    private EquippableItem getOffHandWeapon()
    {
        return getActorStats().getEquippedItems().getOffHand();
    }


	//IDescribable methods
    public override GameObject getRowType(RowType rowType)
    {
        switch (rowType)
        {
            case RowType.Standard:
            case RowType.Equipment:
            case RowType.StatRequirements:
            case RowType.AbilityEditor:
                return getSourceItem().getRowType(rowType);
            default:
                return base.getRowType(rowType);
        }
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

        if(getActorStats() != null)
        {
            getActorStats().setHeadSprite(panel);
        }

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

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(getDamageTotalForDisplay(), getDamageFormulaForDisplay()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getCritBlock(getCritTotalForDisplay(), getCritFormulaForDisplay()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getRangeBlock(getRangeTitle()));

		// if (getAppliedTrait() != null)
		// {
		// 	buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getAppliedTrait().getMaxRoundsLeftForDisplay()));
		// }

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getUseDescription()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: getIconName()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: getSourceItem().getSlotIconName()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: getSourceItem().getTypeIconName()));

		if (getSourceItem().appliesStanceStacks())
		{
			buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.stanceWeaponIconName));
		}


		// buildingBlocks.Add(DescriptionPanelBuildingBlock.getAmountBlock(getSourceItem().getQuantityForDisplay())); // for testing purposes

		//buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getCritTotalForDisplay()));

		return buildingBlocks;

	}
}
