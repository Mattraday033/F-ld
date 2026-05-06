using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Landmarks, Zones, Areas, Locations, and Interiors:

Landmarks are clickable/hoverable buttons on the world map. Clicking one opens up the local map showing all locations in a zone.

Zone: Zones are made up of multiple areas, which are then made up of multiple locations. A zone is what is displayed by the local map. 
For example: individual floors of the mine/manse, or the entirety of the camp that isn't a part of the mine/manse, are each their own Zone.

Areas: Areas are sections of Zones. The destinction between areas isn't always obvious to the player, and doesn't need to be. 
For example: The camp is split into two areas: exteriors and interiors. This is so that the exteriors can be displayed as hostile and show
enemies running around, while the interiors can be designated as "safe" and have no monsters in them. Areas are mostly an internal, code side designation.
Sometimes, aesthetic differences may occur between areas (the camp interiors will have a different background than the camp exteriors), but the player shouldn't ever need
to tell the difference between two areas mechanically.

Locations: Locations are individual sections of a zone, which can be ground into different Areas. 
For example, the center section of the lovashi camp is a location, or the first hallway of the second floor of the mine.

Interiors: Interiors are locations that don't show up on the map, except for a small set of boxes showing how many interiors are in each location. If the player is in an interior, the
player's map indicator will show them in the location that the interior is in. Most interiors are different buildings, or small rooms within larger locations.

An example of this heirarchy:

World Map Landmark: Lovashi Camp
    ||
    \/
Click on this Landmark:
    ||
    \/
Local Map now shows All locations within the Zone: Lovashi Camp
On the code side, these locations are split into Camp Exteriors, and Camp Interiors.
    ||
    \/
The Local Map Player Indicator shows the player in the Center of Camp. This is just a location, not an interior. 
The Local Map also shows how many interiors the Center of Camp has: 4. These interiors are Balint's Hut, The Guard Hut, the Stables, and the Temple.
    ||
    \/
The player closes the map, enters Balint's Hut, and then opens the local map again. His indicator still shows he is in the Center of Camp, but the location name is "Balint's Hut"
Because the player has started the revolt, the hostility of their location changes from hostile to peaceful when entering Balint's Hut because Balint's Hut is in the
Camp Interior Area, but the Center of Camp is in the Camp Exterior Area.
*/

public static class LocationNameList
{

    #region Suffixes

    public const string section1 = "-1";
    public const string section1a = "-1a";
    public const string section1b = "-1b";
    public const string section1c = "-1c";
    public const string section1d = "-1d";
    public const string section1e = "-1e";
    public const string section2 = "-2";
    public const string section2a = "-2a";
    public const string section2b = "-2b";
    public const string section2c = "-2c";
    public const string section2d = "-2d";
    public const string section2e = "-2e";
    public const string section3 = "-3";
    public const string section3a = "-3a";
    public const string section3b = "-3b";
    public const string section3c = "-3c";
    public const string section3d = "-3d";
    public const string section3e = "-3e";
    public const string section4 = "-4";
    public const string section4a = "-4a";
    public const string section4b = "-4b";
    public const string section4c = "-4c";
    public const string section4d = "-4d";
    public const string section4e = "-4e";
    public const string section5 = "-5";
    public const string section5a = "-5a";
    public const string section5b = "-5b";
    public const string section5c = "-5c";
    public const string section5d = "-5d";
    public const string section5e = "-5e";
    public const string section6 = "-6";
    public const string section6a = "-6a";
    public const string section6b = "-6b";
    public const string section6c = "-6c";
    public const string section6d = "-6d";
    public const string section6e = "-6e";
    public const string section7 = "-7";
    public const string section7a = "-7a";
    public const string section7b = "-7b";
    public const string section7c = "-7c";
    public const string section7d = "-7d";
    public const string section7e = "-7e";
    public const string section8 = "-8";
    public const string section8a = "-8a";
    public const string section8b = "-8b";
    public const string section8c = "-8c";
    public const string section8d = "-8d";
    public const string section8e = "-8e";
    public const string section9 = "-9";
    public const string section9a = "-9a";
    public const string section9b = "-9b";
    public const string section9c = "-9c";
    public const string section9d = "-9d";
    public const string section9e = "-9e";

    public const string kitchens = "-Kitchens";
    public const string office = "-Office";
    public const string stockroom = "-Stockroom";


    #endregion

    #region Camp

    #region Camp Interiors

    public const string slaveShackOne = "1SlaveShack";
    public const string slaveShackTwo = "2SlaveShack";
    public const string slaveShackThree = "3SlaveShack";
    public const string slaveShackFour = "4SlaveShack";
    public const string slaveShackFive = "5SlaveShack";
    public const string slaveShackSix = "6SlaveShack";
    public const string slaveShackSeven = "7SlaveShack";
    public const string slaveShackEight = "8SlaveShack";
    public const string slaveShackNine = "9SlaveShack";
    public const string guardHouseNorthEast = "GuardHouseNE";
    public const string guardHouseSouthWest = "GuardHouseSW";
    public const string guardHouseTopFloor = "GuardHouseTopFloor";
    public const string guardShack = "GuardShack";
    public const string messHall = "Mess Hall";
    public const string stables = "Stables";
    public const string stockhouse = "Stockhouse";
    public const string temple = "Temple";
    public const string bodyPile = "Body Pile";

    #endregion

    #region Camp Exteriors

    public const string campNorthEast = "NE" + ZoneKeyList.lovashiCamp;
    public const string campCenter = "Center" + ZoneKeyList.lovashiCamp;
    public const string campManse = "Manse" + ZoneKeyList.lovashiCamp;
    public const string campSouthEast = "SE" + ZoneKeyList.lovashiCamp;
    public const string campMineEntrance = "MineEntrance" + ZoneKeyList.lovashiCamp;
    public const string campNorthWest = "NW" + ZoneKeyList.lovashiCamp;
    #endregion

    #endregion

    #region Mine Levels
    public const string minerCamp = "-Miner Camp";

    #endregion

    #region Manse

    public const string manse = "Manse";

    public const string diningRoom = "-Dining Room";
    public const string stairsToPit = "-StairsToPit";

    #endregion

}