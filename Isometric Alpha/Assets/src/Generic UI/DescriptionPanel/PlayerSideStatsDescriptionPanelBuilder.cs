using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideStatsDescriptionPanelBuilder : DescriptionPanelBuilder
{

    public Transform levelParent;
    public Transform healthParent;
    public Transform goldParent;

    private void Awake()
    {
        filter = new BuilderFilterWhiteList(new List<DescriptionPanelBuildingBlockType>() { DescriptionPanelBuildingBlockType.Text });

        formatter.setFormat(BlockFormat.getBlockFormat(BlockFormatType.PartyMemberStats));
    }

    public override Transform getParent(DescriptionPanelBuildingBlock block)
    {
        switch (block.type)
        {
            case DescriptionPanelBuildingBlockType.Text:

                if (block.iconName != null)
                {
                    if (block.iconName.Equals(IconList.levelIconName) ||
                        block.iconName.Equals(IconList.experienceIconName))
                    {
                        return levelParent;
                    }
                    else if (block.iconName.Equals(IconList.healthIconName) ||
                        block.iconName.Equals(IconList.armorScoreIconName))
                    {
                        return healthParent;
                    }
                    else if (block.iconName.Equals(IconList.affinityIconName) ||
                        block.iconName.Equals(IconList.worthIconName))
                    {
                        return goldParent;
                    }
                }

                break;
            case DescriptionPanelBuildingBlockType.BonusDamageText:
                return healthParent;
        }

        return base.getParent(block);
    }


}
