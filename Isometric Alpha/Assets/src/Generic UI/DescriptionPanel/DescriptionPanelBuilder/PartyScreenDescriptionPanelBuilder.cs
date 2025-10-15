using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PartyScreenDescriptionPanelBuilder : DescriptionPanelBuilder
{

    public override Transform getParent(DescriptionPanelBuildingBlock block)
    {
        switch (block.type)
        {
            default:
                return rowParent;
        }
    }

    public override GameObject getDescriptionPanelRowGameObject(DescriptionPanelBuildingBlockType type)
    {
        return Resources.Load<GameObject>(PrefabNames.descriptionPanelBuildingBlockLargeText);
    }

}
