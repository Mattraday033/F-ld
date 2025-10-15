using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class MapFormatList 
{
    public const int mapDimensions = 8;

    private const string keyElementName = "Key";
    private const string defaultFloorImageElementName = "DefaultFloorImage";
    private const string defaultMapIconElementName = "DefaultMapIcon";
    private const string flipDefaultMapIconElementName = "FlipDefaultMapIcon";
    private const string tilesElementName = "Tiles";
    private const string floorImageElementName = "floorImage";
    private const string mapIconElementName = "mapIcon";
    private const string northWestSouthEastMarkerElementName = "northWestSouthEastMarker";
    private const string northEastSouthWestMarkerElementName = "northEastSouthWestMarker";
    private const string flipMapIconElementName = "flipMapIcon";
    private const string rowElementName = "row";
    private const string colElementName = "col";
    private const string locationNameElementName = "locationName";
    private const string statRequirementTypeElementName = "statRequirementType";
    private const string statLevelRequirementElementName = "statLevelRequirement";

    public static Dictionary<string, MapFormat> mapFormats;

    static MapFormatList()
    {
        loadMapFormats();
    }

    private static void loadMapFormats()
    {
        mapFormats = new Dictionary<string, MapFormat>();
        TextAsset[] mapFormatJsons = Resources.LoadAll<TextAsset>("Maps");

        foreach (TextAsset mapFormatJson in mapFormatJsons)
        {
            MapFormat format = convertMapJsonToMapFormat(mapFormatJson);

            mapFormats.Add(format.key, format);
        }
    }

    private static MapFormat convertMapJsonToMapFormat(TextAsset mapFormatJson)
    {
        string json = mapFormatJson.ToString(); 

        dynamic jsonDynamic = JsonConvert.DeserializeObject<dynamic>(json);

        MapFormat mapFormat = new MapFormat();

        mapFormat.key = jsonDynamic[keyElementName];
        mapFormat.flipDefaultMapIcon = jsonDynamic[flipDefaultMapIconElementName];
        mapFormat.defaultFloorImage = jsonDynamic[defaultFloorImageElementName];
        mapFormat.defaultMapIcon = jsonDynamic[defaultMapIconElementName];

        MapTileFormat[,] tileFormats = new MapTileFormat[mapDimensions, mapDimensions];

        for (int row = 0; row < mapDimensions; row++)
        {
            for (int col = 0; col < mapDimensions; col++)
            {
                tileFormats[row, col] = mapFormat.getDefaultMapTileFormat();
            }
        }

        dynamic tileFormatList = jsonDynamic[tilesElementName];

        foreach (dynamic tileFormat in tileFormatList)
        {
            int row = tileFormat[rowElementName];
            int col = tileFormat[colElementName];

            tileFormats[row, col] = new MapTileFormat();
            tileFormats[row, col].locationName = tileFormat[locationNameElementName];
            tileFormats[row, col].floorImageKey = tileFormat[floorImageElementName];
            tileFormats[row, col].mapIconKey = tileFormat[mapIconElementName];
            tileFormats[row, col].northWestSouthEastMarker = tileFormat[northWestSouthEastMarkerElementName];
            tileFormats[row, col].northEastSouthWestMarker = tileFormat[northEastSouthWestMarkerElementName];
            tileFormats[row, col].flipMapIcon = tileFormat[flipMapIconElementName];
            tileFormats[row, col].row = row;
            tileFormats[row, col].col = col;
            tileFormats[row, col].parentFormat = mapFormat;

            dynamic statRequirementElement = GetFromJson.getElementFromJson(statRequirementTypeElementName, tileFormat, SaveDefaultValues.defaultStatRequirementType);
            tileFormats[row, col].statRequirementType = AllyStats.convertStringToPrimaryStat((string) statRequirementElement);
            tileFormats[row, col].statLevelRequirement = GetFromJson.getElementFromJson(statLevelRequirementElementName, tileFormat, SaveDefaultValues.defaultStatZero);

            if (tileFormats[row, col].locationName != null && !tileFormats[row, col].locationName.Equals(""))
            {
                mapFormat.locationNameToTileFormat.Add(tileFormats[row, col].locationName, tileFormats[row, col]);
            }
        }

        mapFormat.tileFormats = tileFormats;

        return mapFormat;
    }
}

public class MapFormat
{
    public string key;
    public string defaultFloorImage;
    public string defaultMapIcon;
    public bool flipDefaultMapIcon;
    public MapTileFormat[,] tileFormats;
    public Dictionary<string, MapTileFormat> locationNameToTileFormat = new Dictionary<string, MapTileFormat>();

    public MapTileFormat getDefaultMapTileFormat()
    {
        return new MapTileFormat(defaultFloorImage, defaultMapIcon, flipDefaultMapIcon, this);
    }
}

public class MapTileFormat
{
    public PrimaryStat statRequirementType;
    public int statLevelRequirement = 0;

    public string floorImageKey;
    public string mapIconKey;
    public bool northWestSouthEastMarker;
    public bool northEastSouthWestMarker;
    public bool flipMapIcon;
    public string locationName;

    public int row;
    public int col;

    public MapFormat parentFormat;

    public bool isPathTile()
    {
        return northWestSouthEastMarker || northEastSouthWestMarker;
    }

    public bool hasBeenDiscovered()
    {
        return MapObjectList.getMapObject(locationName).hasBeenDiscovered();
    }

    public bool isDefaultTile()
    {
        return floorImageKey.Equals(parentFormat.defaultFloorImage) && mapIconKey.Equals(parentFormat.defaultMapIcon);
    }

    public bool isVisible()
    {
        if (!PartyManager.getPlayerStats().meetsStatRequirements(statRequirementType, statLevelRequirement))
        {
            return false;
        }
        
        else if (isPathTile())
        {
            return bothPathEndsVisible();
        }
        else if (isDefaultTile() || locationName == null || locationName == "" || MapObjectList.getMapObject(locationName).hasBeenDiscovered())
        {
            return true;
        }
        else
        {
            return MapObjectList.getMapObject(locationName).isVisible();
        }
    }

    private bool bothPathEndsVisible()
    {
        ArrayList allLocationsThatShareAnAxis = new ArrayList();

        foreach (KeyValuePair<string, MapTileFormat> kvp in parentFormat.locationNameToTileFormat)
        {
            if (sharesAnAxis(kvp.Value))
            {
                allLocationsThatShareAnAxis.Add(kvp.Value);
            }
        }

        if (allLocationsThatShareAnAxis.Count == 1)
        {
            return ((MapTileFormat)allLocationsThatShareAnAxis[0]).hasBeenDiscovered();
        }

        allLocationsThatShareAnAxis = keepTwoClosestMapTileFormats(allLocationsThatShareAnAxis);

        foreach (MapTileFormat mapTileFormat in allLocationsThatShareAnAxis)
        {
            if (!mapTileFormat.isVisible())
            {
                return false;
            }
        }

        return true;
    }

    private bool sharesAnAxis(MapTileFormat tileFormat)
    {
        if (tileFormat.isPathTile())
        {
            return false;
        }

        if (northWestSouthEastMarker) //shares a row
        {
            return tileFormat.row == row;
        }
        else
        {
            return tileFormat.col == col;
        }
    }

    private ArrayList keepTwoClosestMapTileFormats(ArrayList allLocationsThatShareAnAxis)
    {
        if (allLocationsThatShareAnAxis.Count <= 2)
        {
            return allLocationsThatShareAnAxis;
        }

        if (northEastSouthWestMarker)
        {
            NESWRowDistanceComparer.currentRow = row;
            allLocationsThatShareAnAxis.Sort(new NESWRowDistanceComparer());
        }
        else
        {
            NWSEColDistanceComparer.currentCol = col;
            allLocationsThatShareAnAxis.Sort(new NWSEColDistanceComparer());
        }

        for (int index = allLocationsThatShareAnAxis.Count - 1; index >= 2; index--)
        {
            allLocationsThatShareAnAxis.RemoveAt(index);
        }

        return allLocationsThatShareAnAxis;
    }

    public MapTileFormat()
    {

    }

    public MapTileFormat(string floorImageKey, string mapIconKey, bool flipMapIcon, MapFormat parentFormat)
    {
        this.floorImageKey = floorImageKey;
        this.mapIconKey = mapIconKey;
        this.northWestSouthEastMarker = false;
        this.northEastSouthWestMarker = false;
        this.flipMapIcon = flipMapIcon;
        this.parentFormat = parentFormat;
    }

    public MapTileFormat(string locationName, string floorImageKey, string mapIconKey, bool flipMapIcon)
    {
        this.locationName = locationName;
        this.floorImageKey = floorImageKey;
        this.mapIconKey = mapIconKey;
        this.northWestSouthEastMarker = false;
        this.northEastSouthWestMarker = false;
        this.flipMapIcon = flipMapIcon;
    }
}

public class NESWRowDistanceComparer : IComparer
{
    public static int currentRow;

    public int Compare(object x, object y)
    {
        MapTileFormat xSortable = (MapTileFormat)x;
        MapTileFormat ySortable = (MapTileFormat)y;

        int xDistance = Math.Abs(currentRow - xSortable.row);
        int yDistance = Math.Abs(currentRow - ySortable.row);

        if (xDistance > yDistance)
        {
            return 1;
        } else if (xDistance < yDistance)
        {
            return -1;
        } else
        {
            return 0;
        }
    }
}

public class NWSEColDistanceComparer : IComparer
{
    public static int currentCol;

    public int Compare(object x, object y)
    {
        MapTileFormat xSortable = (MapTileFormat)x;
        MapTileFormat ySortable = (MapTileFormat)y;

        int xDistance = Math.Abs(currentCol - xSortable.col);
        int yDistance = Math.Abs(currentCol - ySortable.col);

        if (xDistance > yDistance)
        {
            return 1;
        } else if (xDistance < yDistance)
        {
            return -1;
        } else
        {
            return 0;
        }
    }
}
