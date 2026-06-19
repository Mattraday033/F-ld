using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerInfo
{
    public int layerPriority;
    public string sortingLayerName;

    public SortingLayerInfo(string sortingLayerName, int layerPriority)
    {
        this.layerPriority = layerPriority;
        this.sortingLayerName = sortingLayerName;
    }

    public void setRendererSortingLayer(Renderer renderer)
    {
        renderer.sortingLayerName = sortingLayerName;
        renderer.sortingOrder = layerPriority;
    }
}

public static class SortingLayerManager
{
    public const string groundSortLayerName = "Ground";
    public const string firstSortLayerName = "First";
    public const string secondSortLayerName = "Second";

    public readonly static SortingLayerInfo behindGroundSortingLayerInfo = new SortingLayerInfo(groundSortLayerName, -1);
    public readonly static SortingLayerInfo groundSortingLayerInfo = new SortingLayerInfo(groundSortLayerName, Constants.indexZero);
    public readonly static SortingLayerInfo waveSortingLayerInfo = new SortingLayerInfo(groundSortLayerName, Constants.indexOne);
    public readonly static SortingLayerInfo buttonSortingLayerInfo = new SortingLayerInfo(groundSortLayerName, Constants.indexOne);
    public readonly static SortingLayerInfo firstSortingLayerInfo = new SortingLayerInfo(firstSortLayerName, Constants.indexOne);
    public readonly static SortingLayerInfo secondSortingLayerInfo = new SortingLayerInfo(secondSortLayerName, Constants.indexZero);

    public static Dictionary<bool, SortingLayerInfo> spikeSortingLayers;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeSortingLayerDicts()
    {
        spikeSortingLayers = new Dictionary<bool, SortingLayerInfo>();
        spikeSortingLayers.Add(true, SortingLayerManager.buttonSortingLayerInfo);
        spikeSortingLayers.Add(false, SortingLayerManager.firstSortingLayerInfo);
    }


    public static SortingLayerInfo getSpikeSortingLayerInfo(bool activated)
    {
        return spikeSortingLayers[activated];
    }

}
