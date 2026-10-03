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

    public Image backgroundImage;
    public Image interiorImage;

    public GameObject inspectNode;
    public GameObject moreInfoNode;

    private void Awake()
    {
        if(CombatStateManager.inCombat && CombatStateManager.whoseTurn != WhoseTurn.Won && CombatStateManager.whoseTurn != WhoseTurn.Lost)
        {
            setTransparency();
            InspectNode.OnInspect.AddListener(setTransparency);

            CombatStateManager.OnActivityChangeToTutorial.AddListener(setTransparency);
            CombatStateManager.OnActivityChangeFromTutorial.AddListener(setTransparency);
        }
    }

    private void OnDestroy()
    {
        InspectNode.OnInspect.RemoveListener(setTransparency);

        CombatStateManager.OnActivityChangeToTutorial.RemoveListener(setTransparency);
        CombatStateManager.OnActivityChangeFromTutorial.RemoveListener(setTransparency);
    }
    
    private void setTransparency()
    {
        if(backgroundImage == null || interiorImage == null)
        {
            return;
        }

        if(InspectNode.inspecting || PlayerStateManager.currentActivity == CurrentActivity.InTutorialSequence)
        {
            backgroundImage.color = ColorList.grey25;
            interiorImage.enabled = true;
        } else
        {
            backgroundImage.color = ColorList.grey25Transparent;
            interiorImage.enabled = false;
        }
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

    //a full parent only takes the (in)vulnerability row. Adding the row would take the parent past maxChildren, hence >=
    protected override bool shouldSkipBlock(DescriptionPanelBuildingBlock block, Transform blockParent)
    {
        if(maxChildren <= 0 || blockParent.childCount < maxChildren)
        {
            return false;
        }

        Sprite icon = block.getIcon();

        return icon == null ||
                (CombatStateManager.inCombat && !icon.name.Equals(IconList.vulnerableIconName)) ||
                (!CombatStateManager.inCombat && !icon.name.Equals(IconList.invulnerableIconName));
    }

    protected override void applyBlockToRow(DescriptionPanelRow row, DescriptionPanelBuildingBlock block, bool newRow)
    {
        base.applyBlockToRow(row, block, newRow);

        //the pivot only needs setting once, and setting it toggles the row to settle its position
        if (newRow && setNamePivot && block.type == DescriptionPanelBuildingBlockType.Name)
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

        //none of this changes between fills, and re-setting the margin would make the text regenerate
        if(newRow && block.type == DescriptionPanelBuildingBlockType.Name && (blockOrigin as Stats != null || blockOrigin as PartyMember != null))
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
    }

    private void setPivotY(GameObject rowObject, float newPivot)
    {
        RectTransform rectTransform = rowObject.GetComponent<RectTransform>();
        rectTransform.pivot = new Vector2(rectTransform.pivot.x, newPivot);
        rectTransform.localPosition = Vector3.zero;

        GameObjectUtil.updateGameObjectPosition(rowObject);
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
