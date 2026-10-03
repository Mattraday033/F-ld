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

    //the last refill skipped while the stat rows were hidden, applied once they're shown
    private IDescribableInBlocks pendingBlockOrigin;
    private BlockFormat pendingFormat;
    private List<DescriptionPanelBuildingBlock> pendingBlocks;

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

    public override void buildDescriptionPanel(IDescribableInBlocks blockOrigin, BlockFormat format, List<DescriptionPanelBuildingBlock> buildingBlocks)
    {
        //in combat the stat rows stay hidden until formulas are shown, so refilling them before then is wasted work
        if (reuseRows && CombatStateManager.inCombat && !OverallUIManager.showFormula && rowsMatch(getBlocksPassingFilter(buildingBlocks)))
        {
            this.blockOrigin = blockOrigin;

            pendingBlockOrigin = blockOrigin;
            pendingFormat = format;
            pendingBlocks = buildingBlocks;
        }
        else
        {
            clearPendingRefill();

            base.buildDescriptionPanel(blockOrigin, format, buildingBlocks);
        }

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

            //set either way, since a refilled builder may be going from a combatant with stat rows to one without
            if(moreInfoNode != null)
            {
                moreInfoNode.SetActive(parentTransformsToShow > 0);
            }

            revealExtraDescriptionPanels();
        }
    }

    private void clearPendingRefill()
    {
        pendingBlockOrigin = null;
        pendingFormat = null;
        pendingBlocks = null;
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
        //an inactive builder belongs to a layout the panel isn't showing, so its pending refill would be stale by the time it's used
        if(OverallUIManager.showFormula && pendingBlocks != null && gameObject.activeInHierarchy)
        {
            IDescribableInBlocks blockOrigin = pendingBlockOrigin;
            BlockFormat format = pendingFormat;
            List<DescriptionPanelBuildingBlock> buildingBlocks = pendingBlocks;

            clearPendingRefill();

            base.buildDescriptionPanel(blockOrigin, format, buildingBlocks);
        }

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
