using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeTrait : Trait
{
	private int damageOnTickDown = 0;
	private string damageFormula = "";

	public DamageOverTimeTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int damageOnTickDown) :
	base(traitName, traitType, traitDescription, iconName)
	{
		this.damageOnTickDown = damageOnTickDown;
	}

	public DamageOverTimeTrait(string traitName, TraitType traitType, string traitDescription, string iconName, string damageFormula) :
	base(traitName, traitType, traitDescription, iconName)
	{
		this.damageFormula = damageFormula;
	}

	public override int getTickDownDamage()
	{
		if (damageFormula.Length <= 0)
		{
			return damageOnTickDown;
		}
		else
		{
			return DamageCalculator.calculateFormula(damageFormula, traitApplier);
		}
	}
	
	public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getTraitTypeBlock(getType()));

		if (damageFormula.Length <= 0)
		{
			buildingBlocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(damageOnTickDown.ToString(), damageOnTickDown.ToString()));
		}
		else
		{
			buildingBlocks.Add(DescriptionPanelBuildingBlock.getDamageBlock(DamageCalculator.calculateFormula(damageFormula, getStatSource()).ToString(), damageFormula));
		}

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDurationBlock(getMaxRoundsLeftForDisplay()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getDescription()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));

		return buildingBlocks;
	}
	
}
