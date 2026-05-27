using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatDescriptionPanelBuilder : DescriptionPanelBuilder
{

    public bool setNamePivot;

    private const int nameFontSize = 36;
    private const float namePivotY = 1f;
    private const float typePivotY = 1.35f;

    public int maxChildren = -1;

    public Transform nameParent;
    public Transform healthParent;
    public Transform bonusDamageParent;
    public Transform levelParent;
    public Transform descriptionParent;

    public GameObject inspectNode;
    public GameObject moreInfoNode;

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
                switch (block.iconName)
                {
                    case IconList.healthIconName:
                        return rowParent;
                    case IconList.levelIconName:
                        return levelParent;
                    default:
                        return base.getParent(block);
                }
            case DescriptionPanelBuildingBlockType.DescriptionText:
                return descriptionParent;
            case DescriptionPanelBuildingBlockType.BonusDamageText:
                return bonusDamageParent;
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

        if(maxChildren > 0 && row.transform.parent.childCount > maxChildren)
        {
            if(block.getIcon() == null || 
                (CombatStateManager.inCombat && !block.getIcon().name.Equals(IconList.vulnerableIconName)) || 
                (!CombatStateManager.inCombat && !block.getIcon().name.Equals(IconList.invulnerableIconName)))
            {
                Destroy(row.gameObject);
                return null;
            }
        }

        if (setNamePivot && block.type == DescriptionPanelBuildingBlockType.Name)
        {
            setPivotY(row.gameObject, namePivotY);
        }
        // else if (blockIsTypeBlock(block))
        // {
        //     setPivotY(row.gameObject, typePivotY);
        // }

        if(block.iconName != null && block.iconName.Equals(IconList.healthIconName))
        {
            DescriptionPanel.setTextAutoSize(row.descriptionText, true);
        }

        if(block.type == DescriptionPanelBuildingBlockType.Name && (blockOrigin as Stats != null || blockOrigin as PartyMember != null))
        {
            DescriptionPanel.setTextFontSize(row.descriptionText, nameFontSize);
            row.transform.SetAsLastSibling();
            row.descriptionText.margin = new Vector4(0f,0f,10f,0f);
        }

        if((!CombatStateManager.inCombat || 
            (CombatStateManager.inCombat && CombatStateManager.whoseTurn == WhoseTurn.Won))
             && row.hasFormula && moreInfoNode != null && inspectNode != null && 
             inspectNode.activeSelf)
        {
            moreInfoNode.SetActive(true);
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

    public override void activateInspectNode()
    {
        if(inspectNode != null && 
            (!CombatStateManager.inCombat || 
            (CombatStateManager.inCombat && CombatStateManager.whoseTurn == WhoseTurn.Won)))
        {
            inspectNode.SetActive(true);
        }
    }

    public override void deactivateInspectNode()
    {
        if(inspectNode != null)
        {
            inspectNode.SetActive(false);
        }
    }
    
}
