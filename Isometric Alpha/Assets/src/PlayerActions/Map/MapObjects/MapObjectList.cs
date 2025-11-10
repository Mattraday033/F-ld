using System;
using System.IO;	
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapObject
{
	public string getZoneKey();
	public string getLocationName();
	public string getMapUIDisplayName();

	public string getMapUIDisplayNameWithoutZoneName();

	public string getNotificationDisplayName();

	public string[] getAdjacentMapObjects();
	public ZoneButtonInfo[] getZoneButtons();

	public bool getIsFastTravelDestination();
	public bool isInterior();
	public bool hasBeenDiscovered();
	public bool isVisible();

	public int getInteriors();
	public string getExteriorSceneName();
	public ArrayList getAllQuestsInLocation();
}

public struct ZoneButtonInfo
{
	public string zoneKey;
	public int buttonIndex;
	
	public ZoneButtonInfo(string zoneKey, int buttonIndex)
	{
		this.zoneKey = zoneKey;
		this.buttonIndex = buttonIndex;
	}
}


public static class MapObjectList
{
	public const int zeroInteriors = 0;
	public const int oneInterior = 1;
	public const int twoInteriors = 2;
	public const int threeInteriors = 3;
	public const int fourInteriors = 4;
	
	public const int interiorIndexZero = 0;
	public const int interiorIndexOne = 1;
	public const int interiorIndexTwo = 2;
	public const int interiorIndexThree = 3;
	
	public const bool fastTravelAccessible = true;
	public const bool notFastTravelAccessible = false;
	
	
	public static IMapObject getMapObject(string name)
    {		
        if(name == null)
        {
            name = LocationNameList.campNorthEast;
        }

		string zoneKey = name;
		
		switch(zoneKey)
		{
			case LocationNameList.camp:
				
				return new MapZone(LocationNameList.camp, "Camp", new string[]{LocationNameList.forest});
			
			case LocationNameList.mineLvl1:
				
				return new MapZone(LocationNameList.mineLvl1, "Mine Level 1", null);
			
			case LocationNameList.mineLvl2:
				
				return new MapZone(LocationNameList.mineLvl1, "Mine Level 2", null);
			
			case LocationNameList.mineLvl3:
				
				return new MapZone(LocationNameList.mineLvl1, "Mine Level 3", null);
			
			case LocationNameList.manseFirstFloor:
				
				return new MapZone(LocationNameList.manseFirstFloor, "Manse-1F", null);
	
			case LocationNameList.manseSecondFloor:
				
				return new MapZone(LocationNameList.manseSecondFloor, "Manse-2F", null);
	
			case LocationNameList.pit:
				
				return new MapZone(LocationNameList.pit, "The Pit", null);
			
			case LocationNameList.forest:
				
				return new MapZone(LocationNameList.forest, "Forest", new string[]{LocationNameList.camp});
		}
		
		string campSceneName = name;
		switch (campSceneName)
		{
			case LocationNameList.campNorthEast:

				return new MapLocation(LocationNameList.camp, LocationNameList.campNorthEast, "Camp - North East", fastTravelAccessible, threeInteriors, new string[] { LocationNameList.campCenter, LocationNameList.slaveShackTwo, LocationNameList.slaveShackThree, LocationNameList.slaveShackSeven });

			case LocationNameList.slaveShackTwo:

				return new MapInterior(LocationNameList.camp, LocationNameList.slaveShackTwo, "Garcha's Shack", interiorIndexZero, LocationNameList.campNorthEast);

			case LocationNameList.slaveShackThree:

				return new MapInterior(LocationNameList.camp, LocationNameList.slaveShackThree, "Janos's Shack", interiorIndexOne, LocationNameList.campNorthEast);

			case LocationNameList.slaveShackSeven:

				return new MapInterior(LocationNameList.camp, LocationNameList.slaveShackSeven, "Clay's Shack", interiorIndexTwo, LocationNameList.campNorthEast);

			case LocationNameList.campCenter:

				return new MapLocation(LocationNameList.camp, LocationNameList.campCenter, "Camp - Center", notFastTravelAccessible, fourInteriors, new string[] { LocationNameList.campNorthEast, LocationNameList.campManse, LocationNameList.campSouthEast, LocationNameList.slaveShackOne, LocationNameList.stables, LocationNameList.temple, LocationNameList.guardShack }, new ZoneButtonInfo[] { new ZoneButtonInfo(LocationNameList.forest, MapPopUpWindow.eastButtonIndex) });

			case LocationNameList.slaveShackOne:

				return new MapInterior(LocationNameList.camp, LocationNameList.slaveShackOne, "Bálint's Shack", interiorIndexZero, LocationNameList.campCenter);

			case LocationNameList.stables:

				return new MapInterior(LocationNameList.camp, LocationNameList.stables, "Stables", interiorIndexOne, LocationNameList.campCenter);

			case LocationNameList.temple:

				return new MapInterior(LocationNameList.camp, LocationNameList.temple, "Temple", interiorIndexTwo, LocationNameList.campCenter);

			case LocationNameList.guardShack:

				return new MapInterior(LocationNameList.camp, LocationNameList.guardShack, "Gate Guardhouse", interiorIndexThree, LocationNameList.campCenter);

			case LocationNameList.campManse:

				return new MapLocation(LocationNameList.camp, LocationNameList.campManse, "Camp - Manse", notFastTravelAccessible, fourInteriors, new string[] { LocationNameList.campCenter, LocationNameList.guardHouseTopFloor, LocationNameList.guardHouseNorthEast, LocationNameList.slaveShackEight, LocationNameList.slaveShackNine}, new ZoneButtonInfo[] { new ZoneButtonInfo(LocationNameList.manseFirstFloor, MapPopUpWindow.westButtonIndex) });

			case LocationNameList.slaveShackEight:

				return new MapInterior(LocationNameList.camp, LocationNameList.slaveShackEight, "Sampson's Shack", interiorIndexZero, LocationNameList.campManse);

			case LocationNameList.slaveShackNine:

				return new MapInterior(LocationNameList.camp, LocationNameList.slaveShackNine, "Manse Slave Shack", interiorIndexOne, LocationNameList.campManse);

			case LocationNameList.guardHouseNorthEast:

				return new MapInterior(LocationNameList.camp, LocationNameList.guardHouseNorthEast, "Guardhouse 1F - North East", interiorIndexTwo, LocationNameList.campManse);

			case LocationNameList.campSouthEast:

				return new MapLocation(LocationNameList.camp, LocationNameList.campSouthEast, "Camp - South East", fastTravelAccessible, fourInteriors, new string[] { LocationNameList.campCenter, LocationNameList.campMineEntrance, LocationNameList.messHall, LocationNameList.slaveShackFour, LocationNameList.slaveShackFive, LocationNameList.slaveShackSix});

			case LocationNameList.messHall:

				return new MapInterior(LocationNameList.camp, LocationNameList.messHall, "Mess Hall", interiorIndexZero, LocationNameList.campSouthEast);

			case LocationNameList.slaveShackFour:

				return new MapInterior(LocationNameList.camp, LocationNameList.slaveShackFour, "Kastor's Shack", interiorIndexOne, LocationNameList.campSouthEast);

			case LocationNameList.slaveShackFive:

				return new MapInterior(LocationNameList.camp, LocationNameList.slaveShackFive, "Ervin's Shack", interiorIndexTwo, LocationNameList.campSouthEast);

			case LocationNameList.slaveShackSix:

				return new MapInterior(LocationNameList.camp, LocationNameList.slaveShackSix, "Thatch's Shack", interiorIndexThree, LocationNameList.campSouthEast);

			case LocationNameList.campMineEntrance:

				return new MapLocation(LocationNameList.camp, LocationNameList.campMineEntrance, "Camp - Mine Entrance", notFastTravelAccessible, twoInteriors, new string[] { LocationNameList.campSouthEast, LocationNameList.stockhouse, LocationNameList.guardHouseSouthWest}, new ZoneButtonInfo[] { new ZoneButtonInfo(LocationNameList.mineLvl1, MapPopUpWindow.westNorthButtonIndex) });

			case LocationNameList.stockhouse:

				return new MapInterior(LocationNameList.camp, LocationNameList.stockhouse, LocationNameList.stockhouse, interiorIndexZero, LocationNameList.campMineEntrance);

			case LocationNameList.guardHouseSouthWest:
			
				return new MapInterior(LocationNameList.camp, LocationNameList.guardHouseSouthWest, "Guardhouse 1F - South West", interiorIndexOne, LocationNameList.campMineEntrance);
			
			case LocationNameList.guardHouseTopFloor:
			
				return new MapInterior(LocationNameList.camp, LocationNameList.guardHouseTopFloor, "Guardhouse 2F", interiorIndexTwo, LocationNameList.campManse);
		}

        string mineLvl1SceneName = name;
		switch(mineLvl1SceneName.Replace(LocationNameList.mineLvl1,""))
		{
			case LocationNameList.section1a:
				
				return new MapLocation(LocationNameList.mineLvl1, LocationNameList.mineLvl1+LocationNameList.section1a, "1a - Entrance", fastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl1+LocationNameList.section1b}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.camp, MapPopUpWindow.eastButtonIndex)});
			
			case LocationNameList.section1b:
				
				return new MapLocation(LocationNameList.mineLvl1, LocationNameList.mineLvl1+LocationNameList.section1b, "1b", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl1+LocationNameList.section1a}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.mineLvl2,MapPopUpWindow.westButtonIndex)});
			
			case LocationNameList.section1c:
				
				return new MapLocation(LocationNameList.mineLvl1, LocationNameList.mineLvl1+LocationNameList.section1c, "1c", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl1+LocationNameList.section1b});
		}
		
		string mineLvl2SceneName = name;
		switch(mineLvl2SceneName.Replace(LocationNameList.mineLvl2,""))
		{
			case LocationNameList.section1a:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section1a, "1a - Stairs", fastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section1b, LocationNameList.mineLvl2+LocationNameList.section6}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.mineLvl1, MapPopUpWindow.southEastButtonIndex)});
			case LocationNameList.section1b:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section1b, "1b - Ruined Inn", notFastTravelAccessible, oneInterior, new string[]{LocationNameList.mineLvl2+LocationNameList.section1a, LocationNameList.mineLvl2+LocationNameList.section2a, LocationNameList.mineLvl2+LocationNameList.section1c});
			case LocationNameList.section1c:
				return new MapInterior(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section1c, "1c - Ruined Bar", interiorIndexOne, LocationNameList.mineLvl2+LocationNameList.section1b);
			
			case LocationNameList.section2a:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section2a, "2a - Stairs", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section1b, LocationNameList.mineLvl2+LocationNameList.section1c, LocationNameList.mineLvl2+LocationNameList.section2b}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.mineLvl3, MapPopUpWindow.westSouthButtonIndex)});
			case LocationNameList.section2b:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section2b, "2b - Armory", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section2a, LocationNameList.mineLvl2+LocationNameList.section3a});
			
			case LocationNameList.section3a:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section3a, "3a", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section2b, LocationNameList.mineLvl2+LocationNameList.section3b});
			case LocationNameList.section3b:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section3b, "3b", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section3a, LocationNameList.mineLvl2+LocationNameList.section4, LocationNameList.mineLvl2+LocationNameList.section7a});
			
			case LocationNameList.section4:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section4, "4", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section3b, LocationNameList.mineLvl2+LocationNameList.section7a});
			
			case LocationNameList.section5:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section5, "5 - Collapsed Vault", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section3a, LocationNameList.mineLvl2+LocationNameList.section5b, LocationNameList.mineLvl2+LocationNameList.section7a});

			case LocationNameList.section6:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section6, "6", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section1a, LocationNameList.mineLvl2+LocationNameList.section7a});
			
			case LocationNameList.section7a:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section7a, "7a", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section3b, LocationNameList.mineLvl2+LocationNameList.section4, LocationNameList.mineLvl2+LocationNameList.section6, LocationNameList.mineLvl2+LocationNameList.section7b});
			case LocationNameList.section7b:
				return new MapLocation(LocationNameList.mineLvl2, LocationNameList.mineLvl2+LocationNameList.section7b, "7b", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl2+LocationNameList.section2b, LocationNameList.mineLvl2+LocationNameList.section7a});
			default:
				break;
		}
		
		string mineLvl3SceneName = name;
		switch(mineLvl3SceneName.Replace(LocationNameList.mineLvl3,""))
		{
			case LocationNameList.section1a:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section1a, "1a - Stairs", fastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl3+LocationNameList.section1b, LocationNameList.mineLvl3+LocationNameList.section2a, LocationNameList.mineLvl3+LocationNameList.section4a}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.mineLvl2, MapPopUpWindow.eastNorthButtonIndex)});
			
			case LocationNameList.section1b:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section1b, "1b", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl3+LocationNameList.section1a});
			
			case LocationNameList.section2a:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section2a, "2a", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl3+LocationNameList.section1a, LocationNameList.mineLvl3+LocationNameList.section2b});
			
			case LocationNameList.section2b:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section2b, "2b", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl3+LocationNameList.section2a, LocationNameList.mineLvl3+LocationNameList.section3a});
			
			case LocationNameList.section3a:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section3a, "3a", fastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl3+LocationNameList.section2b, LocationNameList.mineLvl3+LocationNameList.section3b, LocationNameList.mineLvl3+LocationNameList.section6a, LocationNameList.mineLvl3+LocationNameList.section7});
			
			case LocationNameList.section3b:
				
				return new MapInterior(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section3b, "3b - Stockroom", interiorIndexOne, LocationNameList.mineLvl3+LocationNameList.section3a);
			
			case LocationNameList.section4a:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section4a, "4a", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl3+LocationNameList.section1a, LocationNameList.mineLvl3+LocationNameList.section4b});
			
			case LocationNameList.section4b:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section4b, "4b - River Source", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl3+LocationNameList.section4a, LocationNameList.mineLvl3+LocationNameList.section5});
			
			case LocationNameList.section5:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section5, "5", fastTravelAccessible, oneInterior, new string[]{LocationNameList.mineLvl3+LocationNameList.section4b, LocationNameList.mineLvl3+LocationNameList.minerCamp, LocationNameList.mineLvl3+LocationNameList.section6a});
			
			case LocationNameList.minerCamp:

				return new MapInterior(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.minerCamp, "Miner Camp", interiorIndexZero, LocationNameList.mineLvl3+LocationNameList.section5);
				
			case LocationNameList.section6a:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section6a, "6a", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl3+LocationNameList.section3a, LocationNameList.mineLvl3+LocationNameList.section5});
			
			case LocationNameList.section7:
				
				return new MapLocation(LocationNameList.mineLvl3, LocationNameList.mineLvl3+LocationNameList.section7, "7 - Final Tunnel", notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.mineLvl3+LocationNameList.section3a});
		}
		
		string manseFirstFloorSceneName = name;
		switch(manseFirstFloorSceneName.Replace(LocationNameList.manseFirstFloor,""))
		{
			case LocationNameList.section1a:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section1a, LocationNameList.section1a, fastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseFirstFloor+LocationNameList.section1b}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.camp, MapPopUpWindow.eastSouthButtonIndex)} );
			case LocationNameList.section1b:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section1b, LocationNameList.section1b, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseFirstFloor+LocationNameList.section1a, LocationNameList.manseFirstFloor+LocationNameList.section1c});
			case LocationNameList.section1c:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section1c, LocationNameList.section1c, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseFirstFloor+LocationNameList.kitchens, LocationNameList.manseFirstFloor+LocationNameList.section1b});
			case LocationNameList.kitchens:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.kitchens, LocationNameList.kitchens, fastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseFirstFloor+LocationNameList.section1c}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.camp, MapPopUpWindow.southEastButtonIndex), new ZoneButtonInfo(LocationNameList.manseSecondFloor, MapPopUpWindow.southWestButtonIndex)});

			case LocationNameList.section2a:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section2a, LocationNameList.section2a, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseFirstFloor+LocationNameList.diningRoom, LocationNameList.manseFirstFloor+LocationNameList.section2b});
			case LocationNameList.section2b:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section2b, LocationNameList.section2b, notFastTravelAccessible, oneInterior, new string[] { LocationNameList.manseFirstFloor + LocationNameList.section2a, LocationNameList.manseFirstFloor + LocationNameList.section2c, LocationNameList.manseFirstFloor + LocationNameList.section3c, LocationNameList.manseFirstFloor + LocationNameList.stairsToPit}, new ZoneButtonInfo[] { new ZoneButtonInfo(LocationNameList.pit, MapPopUpWindow.westNorthButtonIndex) });
			case LocationNameList.section2c:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section2c, LocationNameList.section2c, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseFirstFloor+LocationNameList.section2b, LocationNameList.manseFirstFloor+LocationNameList.stairsToPit}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.pit, MapPopUpWindow.westNorthButtonIndex)});
			case LocationNameList.stairsToPit:
				return new MapInterior(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.stairsToPit, "Stairs to Pit", interiorIndexZero, LocationNameList.manseFirstFloor+LocationNameList.section2b);
			case LocationNameList.diningRoom:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.diningRoom, LocationNameList.diningRoom, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseFirstFloor+LocationNameList.kitchens,LocationNameList.manseFirstFloor+LocationNameList.section1b});

			case LocationNameList.section3a:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section3a, LocationNameList.section3a, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseFirstFloor+LocationNameList.section1a, LocationNameList.manseFirstFloor+LocationNameList.section3b});
			case LocationNameList.section3b:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section3b, LocationNameList.section3b, notFastTravelAccessible, oneInterior, new string[]{LocationNameList.manseFirstFloor+LocationNameList.section3a,LocationNameList.manseFirstFloor+LocationNameList.section3c,LocationNameList.manseFirstFloor+LocationNameList.section3d});
			case LocationNameList.section3c:
				return new MapLocation(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section3c, LocationNameList.section3c + " - Library", notFastTravelAccessible, oneInterior, new string[]{LocationNameList.manseFirstFloor+LocationNameList.section2b, LocationNameList.manseFirstFloor+LocationNameList.section3b}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.manseSecondFloor, MapPopUpWindow.westButtonIndex)}, new InteriorDisplayStatRequirements[] { new InteriorDisplayStatRequirements(PrimaryStat.Wisdom, 3)});
			case LocationNameList.section3d:
				return new MapInterior(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section3e, "Page's Room", interiorIndexZero, LocationNameList.manseFirstFloor+LocationNameList.section3b);
			case LocationNameList.section3e:
				return new MapInterior(LocationNameList.manseFirstFloor, LocationNameList.manseFirstFloor+LocationNameList.section3e, "Secret Room", interiorIndexZero, LocationNameList.manseFirstFloor+LocationNameList.section3c);
		}
		
		string manseSecondFloorSceneName = name;
		switch(manseSecondFloorSceneName.Replace(LocationNameList.manseSecondFloor,""))
		{
			case LocationNameList.section1a:
				return new MapLocation(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor+LocationNameList.section1a, LocationNameList.section1a, notFastTravelAccessible, twoInteriors, new string[]{LocationNameList.manseSecondFloor+LocationNameList.section1b, LocationNameList.manseSecondFloor+LocationNameList.section1c, LocationNameList.manseSecondFloor+LocationNameList.office, LocationNameList.manseSecondFloor+LocationNameList.section2a, LocationNameList.manseSecondFloor+LocationNameList.section3c}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.manseFirstFloor, MapPopUpWindow.eastSouthButtonIndex)});
			case LocationNameList.section1b:
				return new MapInterior(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor + LocationNameList.section1b, "Director's Room", interiorIndexZero, LocationNameList.manseSecondFloor + LocationNameList.section1a);
			case LocationNameList.section1c:
				return new MapInterior(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor + LocationNameList.section1b, "Tabor's Room", interiorIndexOne, LocationNameList.manseSecondFloor + LocationNameList.section1a);
			case LocationNameList.office:
				return new MapLocation(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor+LocationNameList.office, LocationNameList.office, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseSecondFloor+LocationNameList.section1a});
			
			
			case LocationNameList.section2a:
				return new MapLocation(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor+LocationNameList.section2a, LocationNameList.section2a, notFastTravelAccessible, twoInteriors, new string[]{LocationNameList.manseSecondFloor+LocationNameList.section1a, LocationNameList.manseSecondFloor+LocationNameList.section2b, LocationNameList.manseSecondFloor+LocationNameList.section2c, LocationNameList.manseSecondFloor+LocationNameList.section2d}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.manseFirstFloor, MapPopUpWindow.northWestButtonIndex)});
			case LocationNameList.section2b:
				return new MapLocation(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor+LocationNameList.section2b, LocationNameList.section2b, fastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseSecondFloor+LocationNameList.section2a, LocationNameList.manseSecondFloor+LocationNameList.section3a});//, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.manseFirstFloor, MapPopUpWindow.northButtonIndex)});
			case LocationNameList.section2c:
				return new MapInterior(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor + LocationNameList.section2c, "Child's Room 1", interiorIndexZero, LocationNameList.manseSecondFloor + LocationNameList.section2a);
			case LocationNameList.section2d:
				return new MapInterior(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor + LocationNameList.section2d, "Child's Room 2", interiorIndexOne, LocationNameList.manseSecondFloor + LocationNameList.section2a);
			
			case LocationNameList.section3a:
				return new MapLocation(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor+LocationNameList.section3a, LocationNameList.section3a, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseSecondFloor+LocationNameList.section2b, LocationNameList.manseSecondFloor+LocationNameList.section3b, LocationNameList.manseSecondFloor+LocationNameList.section3c});
			case LocationNameList.section3b:
				return new MapLocation(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor+LocationNameList.section3b, LocationNameList.section3b, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseSecondFloor+LocationNameList.section3a, LocationNameList.manseSecondFloor+LocationNameList.stockroom});
			case LocationNameList.section3c:
				return new MapLocation(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor+LocationNameList.section3c, LocationNameList.section3c, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseSecondFloor+LocationNameList.section3a, LocationNameList.manseSecondFloor+LocationNameList.stockroom, LocationNameList.manseSecondFloor+LocationNameList.section1a});
			case LocationNameList.stockroom:
				return new MapLocation(LocationNameList.manseSecondFloor, LocationNameList.manseSecondFloor+LocationNameList.stockroom, LocationNameList.stockroom, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.manseSecondFloor+LocationNameList.section3b, LocationNameList.manseSecondFloor+LocationNameList.section3c}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.manseFirstFloor, MapPopUpWindow.southButtonIndex)});
		}
		
		string pitSceneName = name;
		switch(pitSceneName.Replace(LocationNameList.pit,""))
		{
			case LocationNameList.section1a:
				return new MapLocation(LocationNameList.pit, LocationNameList.pit+LocationNameList.section1a, LocationNameList.section1a, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.pit+LocationNameList.section1b}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.manseFirstFloor, MapPopUpWindow.northWestButtonIndex)});
			case LocationNameList.section1b:
				return new MapLocation(LocationNameList.pit, LocationNameList.pit+LocationNameList.section1b, LocationNameList.section1b, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.pit+LocationNameList.section1a}, new ZoneButtonInfo[]{new ZoneButtonInfo(LocationNameList.manseFirstFloor, MapPopUpWindow.westNorthButtonIndex)});
			case LocationNameList.section2a:
				return new MapLocation(LocationNameList.pit, LocationNameList.pit+LocationNameList.section2a, LocationNameList.section2a, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.pit+LocationNameList.section1b,LocationNameList.pit+LocationNameList.section2b,LocationNameList.pit+LocationNameList.section2c});
			case LocationNameList.section2b:
				return new MapLocation(LocationNameList.pit, LocationNameList.pit+LocationNameList.section2b, LocationNameList.section2b, notFastTravelAccessible, oneInterior, new string[]{LocationNameList.pit+LocationNameList.section2a, LocationNameList.pit+LocationNameList.section2d});
			case LocationNameList.section2c:
				return new MapLocation(LocationNameList.pit, LocationNameList.pit+LocationNameList.section2c, LocationNameList.section2c, notFastTravelAccessible, zeroInteriors, new string[]{LocationNameList.pit+LocationNameList.section2a});
			case LocationNameList.section2d:
				return new MapInterior(LocationNameList.pit, LocationNameList.pit+LocationNameList.section2d, "Trash Chute", interiorIndexZero, LocationNameList.pit+LocationNameList.section2b);
			
			default:
				return new MapLocation(name, name, name, notFastTravelAccessible, zeroInteriors, new string[0]);
		}
		
	}

	public static ArrayList getAllDiscoveredZones()
	{
		ArrayList discoveredZones = new ArrayList();
		
		if(MapZone.hasBeenDiscovered(LocationNameList.camp))
		{
			discoveredZones.Add(LocationNameList.camp);
		}
		
		return discoveredZones;
	}

}
