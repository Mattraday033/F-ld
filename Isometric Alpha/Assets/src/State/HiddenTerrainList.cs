using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HiddenTerrainList
{

    private const string hiddenTerrainDesignator = "HT";

    private const string spriteMapFolderPath = "SpriteMaps/";
    private const string hiddenTerrainFolderPath = spriteMapFolderPath + "HiddenTerrain/";

    public static string getHiddenTerrainFolderPath(int index)
    {
        if(AreaManager.locationName == null)
        {
            return "";
        }

        string[] locationNameSections = AreaManager.locationName.Split("-");

        if(locationNameSections.Length <= 1)
        {
            return hiddenTerrainFolderPath + AreaManager.locationName + "/"  + hiddenTerrainDesignator + "-" + index;
        } else
        {
            string section = locationNameSections[locationNameSections.Length-1];

            return hiddenTerrainFolderPath + AreaManager.locationName.Substring(0, AreaManager.locationName.Length - (section.Length+1)) + "/"  + hiddenTerrainDesignator + "-" + section + "-" + index;
        }

    }
}
