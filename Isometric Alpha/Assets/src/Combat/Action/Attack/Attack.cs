using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Attack : CombatAction, IJSONConvertable
{
	private Weapon mainHandWeapon;

    public override bool takesAWeaponSlot => true;
    public override bool canBePlacedInPassiveSlot => true;

    public override Stats actorStats
    {
        set
        {
            base.actorStats = value;
            EquippableItem item = sourceItem as EquippableItem;

            item.equipTarget = value;
        }
    }

    public override Item sourceItem
    {
        get
        {
            return mainHandWeapon;
        }
        set
        {
            mainHandWeapon = (Weapon)value;
        }
    }

    public Attack(Stats actor, Weapon mainHandWeapon, Catalog<IQueuingUnit> queuingUnits = null, List<TargetingLogic> targetingLogics = null) :
    base(actor, SelectorFactory.buildByTemplate(mainHandWeapon.getRangeTemplate()), queuingUnits, targetingLogics)
    {
        this.mainHandWeapon = (Weapon) mainHandWeapon.clone();
        this.mainHandWeapon.setQuantity(1);
        this.mainHandWeapon.equipTarget = actor;
    }

	public override string key => getMainHandWeapon().getKey();

	public override string iconName => getMainHandWeapon().getIconName();

	public override string displayName { get { return getMainHandWeapon().displayName; } }
	public override string uniqueName { get { return getMainHandWeapon().uniqueName; } }

	//an attack's trait comes from its weapon, which already hands back a clone
	public override Trait appliedTrait
	{
		get
		{
			Trait traitToApply = mainHandWeapon?.getAppliedTrait();

			if (traitToApply == null)
			{
				return null;
			}

			traitToApply.traitApplier = actorStats;

			return traitToApply;
		}
	}

	public override SelectorTemplate getRangeTemplate()
	{
		return getMainHandWeapon().getRangeTemplate();
	}

	public override string getRangeTitle()
	{
		return getMainHandWeapon().getRange();
	}

	//a weapon has no use text of its own; its flavour text is the attack's loreDescription
	public override string useDescription => "";

	public override string loreDescription => getMainHandWeapon().getLoreDescription();

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

    //the same weapon-change sound EquippableItem.playEquipSFX plays for a weapon, since weapons always sit in the main hand slot
    public override SFXType equipSFX => AudioClipList.getRandomSFXInRange(SFXType.Weapon1, SFXType.Weapon3);
    public override SFXType unequipSFX => equipSFX;

	public override string getDisplayType()
	{
		return AbilityList.attackActionTypeName;
	}

	public override int maximumSlots => OverallUIManager.getCurrentPartyMember().weaponSlots;

	public override bool hasAvailableSlots(CombatActionArray combatActionArray)
	{
		return combatActionArray.hasAvailableWeaponSlots();
	}

	//convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
	public override string convertToJson()
	{
		return mainHandWeapon.convertToJson();
	}

    public override EffectAnimationType effectAnimationType => sourceItem.getEffectAnimationType();

    public override int getQuantity()
    {
        return 1;
    }

	private Weapon getMainHandWeapon()
	{
		return (Weapon)sourceItem;
	}

    private EquippableItem getOffHandWeapon()
    {
        return actorStats.equippedItems.getOffHand();
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
                return sourceItem.getRowType(rowType);
            default:
                return base.getRowType(rowType);
        }
    }

	public override GameObject getDescriptionPanelFull()
	{
		return sourceItem.getDescriptionPanelFull();
	}

	public override GameObject getDecisionPanel()
	{
		return sourceItem.getDecisionPanel();
	}

	public override GameObject getDescriptionPanelFull(PanelType panelType)
	{
		switch (panelType)
		{
			case PanelType.Standard:

				return sourceItem.getDescriptionPanelFull();

			default:
				return base.getDescriptionPanelFull(panelType);
		}
	}

	public override bool withinFilter(string[] filterParameters)
	{
		return sourceItem.withinFilter(filterParameters);
	}

	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);
		sourceItem.describeSelfFull(panel);

		// if (CombatStateManager.inCombat) //will use the item classes damage/crit display methods if not in combat
		// {
        DescriptionPanel.setText(panel.damageText, getDamageTotalForDisplay());
        DescriptionPanel.setText(panel.critRatingText, getCritTotalForDisplay());
		// }

		if (panel.amountText != null && !(panel.amountText is null))
		{
			panel.amountText.text = maximumSlots + " Slot(s)";
		}

		panel.setObjectBeingDescribed(this);
	}

	public override void describeSelfRow(DescriptionPanel panel)
	{
		sourceItem.describeSelfRow(panel);

        if(actorStats != null)
        {
            actorStats.setHeadSprite(panel);
        }

        DescriptionPanel.setText(panel.damageText, getDamageTotalForDisplay());
        DescriptionPanel.setText(panel.critRatingText, getCritTotalForDisplay());

		panel.setObjectBeingDescribed(this);
	}

	public override void setUpDecisionPanel(IDecisionPanel descisionPanel)
	{
		sourceItem.setUpDecisionPanel(descisionPanel);
	}
	
	//IDescribableInBlocks methods
	public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{

		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(displayName));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType(), HoverMessageList.actionTypePrefix + getType()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(getDamageTotalForDisplay(), getDamageFormulaForDisplay()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getCritBlock(getCritTotalForDisplay(), getCritFormulaForDisplay()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getRangeBlock(getRangeTitle()));

		// if (appliedTrait != null)
		// {
		// 	buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(appliedTrait.getMaxRoundsLeftForDisplay()));
		// }

		if (useDescription.Length > 0)
		{
			buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(useDescription));
		}

		if (loreDescription.Length > 0)
		{
			buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(loreDescription));
		}

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: iconName));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: sourceItem.getSlotIconName()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: sourceItem.getTypeIconName()));

		if (sourceItem.appliesStanceStacks())
		{
			buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.stanceWeaponIconName));
		}


		// buildingBlocks.Add(DescriptionPanelBuildingBlock.getAmountBlock(sourceItem.getQuantityForDisplay())); // for testing purposes

		//buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getCritTotalForDisplay()));

		return buildingBlocks;

	}
}
