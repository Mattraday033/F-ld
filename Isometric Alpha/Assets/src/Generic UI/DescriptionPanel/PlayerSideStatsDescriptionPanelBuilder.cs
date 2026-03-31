using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideStatsDescriptionPanelBuilder : DescriptionPanelBuilder
{

    public Transform levelParent;
    public Transform healthParent;
    public Transform goldParent;
    public Transform resistanceParent;

    private void Awake()
    {
        filter = new BuilderFilterWhiteList(new List<DescriptionPanelBuildingBlockType>() { DescriptionPanelBuildingBlockType.Text });

        formatter.setFormat(BlockFormat.getBlockFormat(BlockFormatType.PartyMemberStats));
    }

    public override Transform getParent(DescriptionPanelBuildingBlock block)
    {
        // if(!CombatStateManager.inCombat)
        // {
        //     switch(block.iconName)
        //     {
        //         case IconList.vulnerableIconName:
        //             return null;
        //     }
        // }

        switch (block.type)
        {
            case DescriptionPanelBuildingBlockType.Text:

                switch (block.iconName)
                {
                    case IconList.levelIconName:
                    case IconList.experienceIconName:
                        return levelParent;
                    case IconList.healthIconName:
                    case IconList.armorScoreIconName:
                    case IconList.invulnerableIconName:
                        return healthParent;
                    case IconList.affinityIconName:
                    case IconList.worthIconName:
                        return goldParent;
                    case IconList.vulnerableIconName:
                        return null;
                }

                break;
            case DescriptionPanelBuildingBlockType.BonusDamageText:
                return healthParent;
            case DescriptionPanelBuildingBlockType.SecondaryStat:
                switch (block.iconName)
                {
                    case IconList.mentalResistIconName:
                    case IconList.woundResistIconName:
                        return resistanceParent;
                }
                break;
        }

        return null;
    }


}
