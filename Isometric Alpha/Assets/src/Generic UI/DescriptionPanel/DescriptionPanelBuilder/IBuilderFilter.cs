using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IBuilderFilter
{

    public bool blockPassesFilter(DescriptionPanelBuildingBlock block);

}

public class BuilderFilterWhiteList : IBuilderFilter
{
    public List<DescriptionPanelBuildingBlockType> whiteList;

    public BuilderFilterWhiteList(List<DescriptionPanelBuildingBlockType> whiteList)
    {
        this.whiteList = whiteList;
    }

    public bool blockPassesFilter(DescriptionPanelBuildingBlock block)
    {
        return whiteList.Contains(block.type);
    }
}

public class BuilderFilterBlackList : IBuilderFilter
{
    public List<DescriptionPanelBuildingBlockType> blackList;

    public BuilderFilterBlackList(List<DescriptionPanelBuildingBlockType> blackList)
    {
        this.blackList = blackList;
    }

    public bool blockPassesFilter(DescriptionPanelBuildingBlock block)
    {
        return !blackList.Contains(block.type);
    }
}
