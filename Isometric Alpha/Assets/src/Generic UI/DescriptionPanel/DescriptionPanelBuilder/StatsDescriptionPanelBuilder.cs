using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDescriptionPanelBuilder : DescriptionPanelBuilder
{

    public Transform primaryStatParent;

    public Transform strStatParent;
    public Transform dexStatParent;
    public Transform wisStatParent;
    public Transform chaStatParent;

    public GridLayoutGroup primaryGridLayout;

    public GridLayoutGroup[] secondaryGridLayouts;

    public float numberOfTilesPerRow = 4f;

    private void Awake()
    {
        filter = new BuilderFilterWhiteList(new List<DescriptionPanelBuildingBlockType>() { DescriptionPanelBuildingBlockType.PrimaryStat, DescriptionPanelBuildingBlockType.SecondaryStat });

        setGridLayoutSize();
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
                        return strStatParent;
                    case Dexterity.symbolChar:
                        return dexStatParent;
                    case Wisdom.symbolChar:
                        return wisStatParent;
                    case Charisma.symbolChar:
                        return chaStatParent;
                }

                break;
        }

        return base.getParent(block);
    }

    private void setGridLayoutSize()
    {
        if (primaryGridLayout == null)
        {
            return;
        }

        RectTransform parentRectTrans = transform.parent.GetComponent<RectTransform>();

        primaryGridLayout.cellSize = new Vector2((parentRectTrans.rect.width - 20f) / numberOfTilesPerRow , primaryGridLayout.cellSize.y);

        foreach (GridLayoutGroup grid in secondaryGridLayouts)
        {
            grid.cellSize = new Vector2((parentRectTrans.rect.width - 20f) / numberOfTilesPerRow , primaryGridLayout.cellSize.y);
        }

        rebuildLayouts();
    }

}
