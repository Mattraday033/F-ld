using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemCombatAction : CombatAction, IJSONConvertable
{
	private UsableItem sourceItem;

	public ItemCombatAction(Stats actor, UsableItem sourceItem):
    base(actor, null)
	{
		this.sourceItem = sourceItem.clone() as UsableItem;
        this.sourceItem.equipTarget = actor;
	}

	public ItemCombatAction(Stats actor, Selector selector, UsableItem sourceItem) :
	base(actor, selector)
	{
		this.sourceItem = sourceItem.clone() as UsableItem;
        this.sourceItem.equipTarget = actor;
	}

	public override string getDisplayType()
	{
		return "Item";
	}

	public override bool targetsAllySection()
	{
		return sourceItem.targetsAllySection();
	}

	public override string getKey()
	{
		return sourceItem.getKey();
	}

	public override int getRangeIndex()
	{
		return sourceItem.getRangeIndex();
	}

	public override string getRangeTitle()
	{
		return Range.getRangeTitle(getRangeIndex());
	}

	public override Item getSourceItem()
	{
		return (Item)sourceItem;
	}

	public override int getQuantity()
	{
		return getSourceItem().getQuantity();
	}

	public override string getUseDescription()
	{
		return sourceItem.getUseDescription();
	}

	public override void setSourceItem(Item sourceItem)
	{
		this.sourceItem = (UsableItem)sourceItem;
	}

	public override string getIconName()
	{
		return this.sourceItem.getIconName();
	}

	public override string getName()
	{
		return getKey();
	}

	public override string getDamageTotalForDisplay()
	{
		if (healsTarget() && getDamageFormulaTotal() > 0)
		{
			return "Heals " + getDamageFormula().Replace(" ", "");
		}

		return base.getDamageTotalForDisplay();
	}

	public override string getDamageFormula()
	{
		return this.sourceItem.getDamageFormula();
	}

	public override string getCritFormula()
	{
		if (inPreviewMode)
		{
			return Constants.zeroRating;
		}

		return this.sourceItem.getCritFormula();
	}

    public override string getFinalCritFormula()
    {
        return Constants.zeroRating;
    }

	public override bool requiresAnAction()
	{
		return sourceItem.useRequiresAnAction();
	}

    public override int[] findFinalDamage(Stats targetCombatant, bool isCrit)
    {
        if(healsTarget())
        {
            return new int[] { sourceItem.getAmountToHeal() };
        }

        return new int[] { -1 };
    }

	public override void performCombatAction(List<Stats> targets)
	{
		foreach (Stats targetCombatant in targets)
		{
			if (targetCombatant != null)
			{
				sourceItem.use(targetCombatant);
                if(!inPreviewMode)
                {
                    CombatAnimationManager.loadInstantEffect(getEffectAnimationType(), targetCombatant.position, false, findFinalDamage(targetCombatant, false)[0], healsTarget(), false, null, getActorCoords());
                }
			}
		}

		if (!inPreviewMode)
		{
			Inventory.removeItem(sourceItem, 1);
		}

		if (Inventory.inventoryContainsItem(sourceItem.getKey()))
		{
			sourceItem = (UsableItem)Inventory.getItem(sourceItem.getKey());
            sourceItem.equipTarget = getActorStats();
		}
		else
		{
			sourceItem.setQuantity(0);
		}
	}

    public override string getEffectAnimationType()
    {
        return sourceItem.getEffectAnimationType();
    }

	public override void onAddToAbilityMenu() //for updating things like checking for source item quantity
	{
		updateSourceItemQuantity();
	}

	private void updateSourceItemQuantity()
	{
		if (Inventory.inventoryContainsItem(getSourceItem().getKey()))
		{
			sourceItem.setQuantity(Inventory.getItem(getSourceItem().getKey()).getQuantity());
		}
		else
		{
			sourceItem.setQuantity(0);
		}
	}

	public override int getMaximumSlots()
	{
		return 1;
	}

	public override bool healsTarget()
	{
		return sourceItem.getAmountToHeal() > 0;
	}

	public override int getSaveType()
	{
		return (int)CombatActionSaveType.ItemCombatAction;
	}

	public override bool usableWithoutItemsInInventory()
	{
		return false;
	}

	public override bool needsItemQuantityPanel()
	{
		return true;
	}

	public override void activatingAction()
	{
		base.activatingAction();

		if (sourceItem.getQuantity() <= 1)
		{

		}
	}

    public override List<IDescribable> getRelatedDescribables()
    {
        return sourceItem.getRelatedDescribables();
    }

	//convertToJson is for save files, you will never need to save an actions coords so actor/target coords are not saved
	public override string convertToJson()
	{
		string itemJson = sourceItem.convertToJson();

		return sourceItem.convertToJson().Substring(0, itemJson.Length - 1) + ",\"CombatActionSaveType\":\"" + getSaveType() + "\"}";
	}

	public override GameObject getDescriptionPanelFull(PanelType panelType)
	{
		string panelTypeName = "";

		switch (panelType)
		{
			case PanelType.Standard:
			case PanelType.AbilityEditor:
				panelTypeName = PrefabNames.itemCombatActionDescPanelFull;
				break;
			default:
				return base.getDescriptionPanelFull(panelType);
		}

		return DescriptionPanel.getDescriptionPanel(panelTypeName);
	}

	public override void describeSelfFull(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, getName());
		DescriptionPanel.setText(panel.damageText, getDamageTotalForDisplay());
		DescriptionPanel.setText(panel.rangeText, getRangeTitle());
		DescriptionPanel.setText(panel.typeText, getDisplayType());
		DescriptionPanel.setText(panel.amountText, getMaximumSlotsForDisplay());
		DescriptionPanel.setText(panel.timerText, getMaximumCooldownForDisplay());
		DescriptionPanel.setText(panel.useDescriptionText, getUseDescription());
		DescriptionPanel.setText(panel.typeText, getDisplayType());


		DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getIconName()));

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

        DescriptionPanel.setText(panel.statText, sourceItem.getQuantityForDisplay());
    }

	//IDescribableInBlocks methods
	public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{

		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType()));

        if(!getDamageFormula().Equals(Constants.zeroRating))
        {
		    buildingBlocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(getDamageTotalForDisplay(), getDamageFormulaForDisplay()));
        }

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getRangeBlock(getRangeTitle()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getCritBlock(getCritTotalForDisplay(), getCritFormulaForDisplay()));

		// if (getAppliedTrait() != null)
		// {
		// 	buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getAppliedTrait().getMaxRoundsLeftForDisplay()));
		// }

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getUseDescription()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getSourceItem().getTypeIconName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getAmountBlock(getSourceItem().getQuantityForDisplay()));

		//buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getCritTotalForDisplay()));

		return buildingBlocks;

	}
}
