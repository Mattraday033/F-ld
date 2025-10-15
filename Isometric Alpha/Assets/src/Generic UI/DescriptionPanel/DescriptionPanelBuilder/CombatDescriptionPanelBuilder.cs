using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatDescriptionPanelBuilder : DescriptionPanelBuilder
{

    public bool setNamePivot;

    private const float namePivotY = 1f;
    private const float typePivotY = 1.35f;

    public Transform nameParent;
    public Transform levelParent;
    public Transform descriptionParent;

    private void Awake()
    {
        // filter = new BuilderFilterBlackList(new List<DescriptionPanelBuildingBlockType>() { DescriptionPanelBuildingBlockType.PrimaryStat, DescriptionPanelBuildingBlockType.SecondaryStat });
    }

    public override Transform getParent(DescriptionPanelBuildingBlock block)
    {

        switch (block.type)
        {
            case DescriptionPanelBuildingBlockType.Name:
                return nameParent;
            case DescriptionPanelBuildingBlockType.Text:

                if (block.iconName != null)
                {
                    if (block.iconName.Equals(IconList.levelIconName))
                    {
                        return levelParent;
                    }
                    else if (blockIsTypeBlock(block))
                    {
                        return nameParent;
                    }
                }

                return base.getParent(block);
            case DescriptionPanelBuildingBlockType.DescriptionText:
                return descriptionParent;
        }

        return base.getParent(block);
    }

    public override DescriptionPanelRow buildRow(DescriptionPanelBuildingBlock block)
    {
        DescriptionPanelRow row = base.buildRow(block);

        if (row == null)
        {
            return null;
        }

        if (setNamePivot && block.type == DescriptionPanelBuildingBlockType.Name)
        {
            setPivotY(row.gameObject, namePivotY);
        }
        else if (blockIsTypeBlock(block))
        {
            setPivotY(row.gameObject, typePivotY);
        }

        return row;
    }

    private void setPivotY(GameObject rowObject, float newPivot)
    {
        RectTransform rectTransform = rowObject.GetComponent<RectTransform>();
        rectTransform.pivot = new Vector2(rectTransform.pivot.x, newPivot);
        rectTransform.localPosition = Vector3.zero;

        Helpers.updateGameObjectPosition(rowObject);
    }

    private bool blockIsTypeBlock(DescriptionPanelBuildingBlock block)
    {
        if (block.iconName != null &&
        (block.iconName.Equals(IconList.actionTypeIconName) ||
                block.iconName.Equals(IconList.traitTypeIconName)))
        {
            return true;
        }

        return false;
    }

}
