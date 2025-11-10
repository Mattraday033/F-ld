using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HiddenTerrainList
{

    private const string hiddenTerrainDesignator = "HT";

    private const string spriteMapFolderPath = "SpriteMaps/";
    private const string hiddenTerrainFolderPath = spriteMapFolderPath + "HiddenTerrain/";

    public static string getHiddenTerrainFolderPath(string locationName, int index)
    {
        return hiddenTerrainFolderPath + locationName + "/"  + hiddenTerrainDesignator + "-" + index;
    }

    public static string getHiddenTerrainFolderPath(string areaName, string section, int index)
    {
        return hiddenTerrainFolderPath + areaName + "/"  + hiddenTerrainDesignator + section + "-" + index;
    }

}
