using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatResultsDescriptionPanelBuilder : CombatDescriptionPanelBuilder
{
    public Transform itemParent;

    public override Transform getParent(DescriptionPanelBuildingBlock block)
    {
        switch (block.type)
        {
            case DescriptionPanelBuildingBlockType.Item:
                return itemParent;
        }

        return base.getParent(block);
    }

    public override DescriptionPanelRow buildRow(DescriptionPanelBuildingBlock block)
    {
        if(block.type != DescriptionPanelBuildingBlockType.Item)
        {
            return base.buildRow(block);
        }

        Transform blockParent = getParent(block);

        if (blockParent == null)
        {
            return null;
        }

        DescriptionPanelRow row = Instantiate(getDescriptionPanelRowGameObject(block.type), blockParent).GetComponent<DescriptionPanelRow>();

        row.type = block.type;

        DescriptionPanel resultsItemDescriptionPanel = row.gameObject.GetComponent<DescriptionPanel>();

        blockParent.gameObject.SetActive(true);

        block.item.describeSelfFull(resultsItemDescriptionPanel);

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

    public override void activateInspectNode()
    {
        if(inspectNode != null)
        {
            inspectNode.SetActive(true);
        }
    }

}
