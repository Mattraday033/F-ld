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
    public readonly static SortingLayerInfo behindGroundSortingLayerInfo = new SortingLayerInfo(LayerAndTagManager.groundSortingLayerName, -1);
    public readonly static SortingLayerInfo groundSortingLayerInfo = new SortingLayerInfo(LayerAndTagManager.groundSortingLayerName, Constants.indexZero);
    public readonly static SortingLayerInfo groundTwoSortingLayerInfo = new SortingLayerInfo(LayerAndTagManager.groundSortingLayerName, Constants.indexTwo);
    public readonly static SortingLayerInfo waveSortingLayerInfo = new SortingLayerInfo(LayerAndTagManager.groundSortingLayerName, Constants.indexOne);
    public readonly static SortingLayerInfo buttonSortingLayerInfo = new SortingLayerInfo(LayerAndTagManager.groundSortingLayerName, Constants.indexOne);
    public readonly static SortingLayerInfo firstSortingLayerInfo = new SortingLayerInfo(LayerAndTagManager.firstSortingLayerName, Constants.indexOne);
    public readonly static SortingLayerInfo secondSortingLayerInfo = new SortingLayerInfo(LayerAndTagManager.secondSortingLayerName, Constants.indexZero);
    public readonly static SortingLayerInfo sixthSortingLayerInfo = new SortingLayerInfo(LayerAndTagManager.sixthSortingLayerName, Constants.indexZero);

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
