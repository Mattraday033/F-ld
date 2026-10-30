using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemCombatAction : CombatAction, IJSONConvertable
{
	private UsableItem _SourceItem;

	public override bool targetsAllySection => _SourceItem.targetsAllySection();
	public override bool requiresAnAction => _SourceItem.useRequiresAnAction();
	public override bool healsTarget => _SourceItem.getAmountToHeal() > 0;
	public override bool usableWithoutItemsInInventory => false;
	public override bool needsItemQuantityPanel => true;

	//only ever a UsableItem; code here that needs UsableItem's own methods reads _SourceItem
	public override Item sourceItem
	{
		get
		{
			return _SourceItem;
		}
		set
		{
			_SourceItem = (UsableItem)value;
		}
	}

	public ItemCombatAction(Stats actor, UsableItem sourceItem, Catalog<IQueuingUnit> queuingUnits = null, List<TargetingLogic> targetingLogics = null):
    base(actor, SelectorFactory.buildByTemplate(sourceItem.getRangeTemplate()), queuingUnits, targetingLogics)
	{
		_SourceItem = sourceItem.clone() as UsableItem;
        _SourceItem.equipTarget = actor;

		if(_QueuingUnits[typeof(ChargeItemUse)].Count == 0)
		{
			_QueuingUnits.Add(new ChargeItemUse());
		}
	}

	public override string getDisplayType()
	{
		return AbilityList.itemActionTypeName;
	}

	public override string key => _SourceItem.getKey();

	public override SelectorTemplate getRangeTemplate()
	{
		return _SourceItem.getRangeTemplate();
	}

	public override int getQuantity()
	{
		return sourceItem.getQuantity();
	}

	public override string useDescription => _SourceItem.getUseDescription();

	public override string loreDescription => _SourceItem.getLoreDescription();

	public override string iconName => _SourceItem.getIconName();

	//item keys are written to be shown to the player
	public override string displayName { get { return key; } }
	public override string uniqueName { get { return key; } }

	public override string getDamageTotalForDisplay()
	{
		if (healsTarget && getDamageFormulaTotal() > 0)
		{
			return "Heals " + getDamageFormula().Replace(" ", "");
		}

		return base.getDamageTotalForDisplay();
	}

	public override string getDamageFormula()
	{
		return _SourceItem.getDamageFormula();
	}

	public override string getCritFormula()
	{
		if (inPreviewMode)
		{
			return Constants.zeroRating;
		}

		return _SourceItem.getCritFormula();
	}

    public override string getFinalCritFormula()
    {
        return Constants.zeroRating;
    }

    public override EffectAnimationType effectAnimationType => _SourceItem.getEffectAnimationType();

	public override void onAddToAbilityMenu() //for updating things like checking for source item quantity
	{
		updateSourceItemQuantity();
	}

	private void updateSourceItemQuantity()
	{
		if (Inventory.inventoryContainsItem(sourceItem.getKey()))
		{
			_SourceItem.setQuantity(Inventory.getItem(sourceItem.getKey()).getQuantity());
		}
		else
		{
			_SourceItem.setQuantity(0);
		}
	}

	public override int maximumSlots => 1;

    public override List<IDescribable> getRelatedDescribables()
    {
        return _SourceItem.getRelatedDescribables();
    }

	//convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
	public override string convertToJson()
	{
		return _SourceItem.convertToJson();
	}

	public override void describeSelfFull(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, displayName);
		DescriptionPanel.setText(panel.damageText, getDamageTotalForDisplay());
		DescriptionPanel.setText(panel.rangeText, getRangeTitle());
		DescriptionPanel.setText(panel.typeText, getDisplayType());
		DescriptionPanel.setText(panel.amountText, getMaximumSlotsForDisplay());
		DescriptionPanel.setText(panel.timerText, getMaximumCooldownForDisplay());
		DescriptionPanel.setText(panel.useDescriptionText, useDescription);
		DescriptionPanel.setText(panel.typeText, getDisplayType());


		DescriptionPanel.setImage(panel.iconPanel, iconSprite);

		/* commented out because apparently getIconBackgroundColor() doesn't exist (was thinking fo traits
			may implement later
		if(panel.iconBackgroundPanel != null && !(panel.iconBackgroundPanel is null))
		{
			panel.iconBackgroundPanel.color = getIconBackgroundColor();
		}*/
	}
	
    public override void describeSelfRow(DescriptionPanel panel)
    {
        base.describeSelfRow(panel);

        DescriptionPanel.setText(panel.statText, _SourceItem.getQuantityForDisplay());

        addSlotsTextToRow(panel);
    }

	//IDescribableInBlocks methods
	public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{

		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(displayName));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType(), HoverMessageList.actionTypePrefix + getType()));

        if(!getDamageFormula().Equals(Constants.zeroRating))
        {
		    buildingBlocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(getDamageTotalForDisplay(), getDamageFormulaForDisplay()));
        }

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getRangeBlock(getRangeTitle()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getCritBlock(getCritTotalForDisplay(), getCritFormulaForDisplay()));

		// if (appliedTrait != null)
		// {
		// 	buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(appliedTrait.getMaxRoundsLeftForDisplay()));
		// }

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(useDescription));

		if (loreDescription.Length > 0)
		{
			buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(loreDescription));
		}

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: iconName));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: sourceItem.getTypeIconName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getAmountBlock(sourceItem.getQuantityForDisplay()));

		//buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getCritTotalForDisplay()));

		return buildingBlocks;

	}
}
