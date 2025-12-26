using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjectMovement : EnemyMovement
{

    public override bool movableObject
    {
        get => true;
        set { }
    }

    public override MonsterMovementType movementType
    {
        get => MonsterMovementType.Stationary;
        set { }
    }

    public override SpriteRenderer getSpriteRenderer()
    {
        return spriteRenderer;
    }

    public override void determineDirection()
    {
        if (MovementManager.getCellWorld(PlayerMovement.getInstance().endingPosition) == MovementTracker.getCurrentCell(this))
        {
            _DirectionMod = PlayerMovement.getInstance().directionMod;
        }
        else
        {
            _DirectionMod = Vector3Int.zero;
        }

        _StartingPosition = getWorldPosition();
        _EndingPosition = AreaManager.getMasterGrid().GetCellCenterWorld(MovementTracker.getCurrentCell(this) + _DirectionMod);
    }

    public override bool isDefeated()
    {
        return false;
    }

	public override bool validTarget(SkillType skillType)
	{
        return false;
	}

    public override void prepCombat()
    {
        //Empty on purpose
    }

    public override void updateIdleDirection()
    {
        //Empty on purpose
    }

    public override void updateRunDirection()
    {
        //Empty on purpose
    }

    public override string getName()
    {
        return packName;
    }

	public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

		blocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

		return blocks;
	}

}
