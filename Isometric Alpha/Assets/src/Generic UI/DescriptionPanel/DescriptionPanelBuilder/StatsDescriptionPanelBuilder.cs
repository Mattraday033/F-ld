using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDescriptionPanelBuilder : DescriptionPanelBuilder
{

    public Transform primaryStatParent;
    public Transform secondaryStatParent;

    public GridLayoutGroup primaryGridLayout;

    public GridLayoutGroup secondaryGridLayout;

    public GameObject moreInfoNode;

    private List<Transform> parents = new List<Transform>();

    public float numberOfTilesPerRow = 4f;

    protected virtual void Awake()
    {
        filter = new BuilderFilterWhiteList(new List<DescriptionPanelBuildingBlockType>() { DescriptionPanelBuildingBlockType.PrimaryStat, DescriptionPanelBuildingBlockType.SecondaryStat });

        // setGridLayoutSize();
    
        parents.Add(primaryStatParent);
        parents.Add(secondaryStatParent);

        if(CombatStateManager.inCombat)
        {
            OnFormulaSwap.AddListener(revealExtraDescriptionPanels);
        }
    }

    private void OnDestroy()
    {
        OnFormulaSwap.RemoveListener(revealExtraDescriptionPanels);
    }

    public override void buildDescriptionPanel(IDescribableInBlocks blockOrigin, BlockFormat format)
    {
        base.buildDescriptionPanel(blockOrigin, format);

        int parentTransformsToShow = 0;

        if(CombatStateManager.inCombat)
        {
            foreach(Transform parentTransform in parents)
            {
                parentTransform.parent.gameObject.SetActive(false);

                if(parentTransform != null && parentTransform.childCount > 0)
                {
                    parentTransformsToShow++;
                }
            }

            if(parentTransformsToShow > 0 && moreInfoNode != null)
            {
                moreInfoNode.SetActive(true);
            }
        }
    }

    public override Transform getParent(DescriptionPanelBuildingBlock block)
    {

        switch (block.type)
        {
            case DescriptionPanelBuildingBlockType.PrimaryStat:
                return primaryStatParent;
            case DescriptionPanelBuildingBlockType.SecondaryStat:

                switch (block.symbolCharacter)
                {
                    case Strength.symbolChar:
                    case Dexterity.symbolChar:
                    case Wisdom.symbolChar:
                    case Charisma.symbolChar:
                        return secondaryStatParent;
                }

                break;
        }

        return base.getParent(block);
    }

    private void revealExtraDescriptionPanels()
    {
        foreach(Transform parentTransform in parents)
        {
            if(parentTransform != null && parentTransform.childCount > 0)
            {
                parentTransform.parent.gameObject.SetActive(OverallUIManager.showFormula);
            }
        }
    }

    private void setGridLayoutSize()
    {
        if (primaryGridLayout == null)
        {
            return;
        }

        RectTransform parentRectTrans = transform.parent.GetComponent<RectTransform>();

        primaryGridLayout.cellSize = new Vector2((parentRectTrans.rect.width - 20f) / numberOfTilesPerRow , primaryGridLayout.cellSize.y);

        secondaryGridLayout.cellSize = new Vector2((parentRectTrans.rect.width - 20f) / numberOfTilesPerRow , primaryGridLayout.cellSize.y);

        rebuildLayouts();
    }

}
