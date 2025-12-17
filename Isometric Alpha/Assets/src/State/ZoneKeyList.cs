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

public static class ZoneKeyList
{

    public const string lovashiCamp = "Camp";

    public const string manseFirstFloor = "Manse-1F";
    public const string manseSecondFloor = "Manse-2F";

    public const string mineLvl1 = "MineLvl_1";
    public const string mineLvl2 = "MineLvl_2";
    public const string mineLvl3 = "MineLvl_3";

    public const string pit = "Pit";
    public const string forest = "Forest";
}