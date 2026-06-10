using System;
using System.IO;	
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapObject
{
	public string getZoneKey();
	public string getBackgroundKey();
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
	public string getExteriorLocationName();
	public List<QuestStep> getAllQuestStepsInLocation();
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

    public static string getCurrentZoneKey()
    {		
        return getMapObject(AreaManager.locationName).getZoneKey();
    }

    public static string getCurrentBackgroundKey()
    {		
        return getMapObject(AreaManager.locationName).getBackgroundKey();
    }

	public static IMapObject getMapObject(string name)
    {		
        if(name == null)
        {
            name = LocationNameList.campNorthEast;
        }

		string zoneKey = name;
		
		switch(zoneKey)
		{
			case ZoneKeyList.lovashiCamp:
				
				return new MapZone(ZoneKeyList.lovashiCamp, MapDisplayNameList.lovashiCamp, new string[]{ZoneKeyList.forest});
			
			case ZoneKeyList.mineLvl1:
				
				return new MapZone(ZoneKeyList.mineLvl1, MapDisplayNameList.mineLvl1, null);
			
			case ZoneKeyList.mineLvl2:
				
				return new MapZone(ZoneKeyList.mineLvl1, MapDisplayNameList.mineLvl2, null);
			
			case ZoneKeyList.mineLvl3:
				
				return new MapZone(ZoneKeyList.mineLvl1, MapDisplayNameList.mineLvl3, null);
			
			case ZoneKeyList.manseFirstFloor:
				
				return new MapZone(ZoneKeyList.manseFirstFloor, MapDisplayNameList.manseFloor1, null);
	
			case ZoneKeyList.manseSecondFloor:
				
				return new MapZone(ZoneKeyList.manseSecondFloor, MapDisplayNameList.manseFloor2, null);
	
			case ZoneKeyList.pit:
				
				return new MapZone(ZoneKeyList.pit, MapDisplayNameList.thePit, null);
			
			case ZoneKeyList.forest:
				
				return new MapZone(ZoneKeyList.forest, MapDisplayNameList.forest, new string[]{ZoneKeyList.lovashiCamp});
		}
		
		string campSceneName = name;
		switch (campSceneName)
		{
			case LocationNameList.campNorthEast:

				return new MapLocation(ZoneKeyList.lovashiCamp, LocationNameList.campNorthEast, "North East", threeInteriors, new string[] { LocationNameList.campCenter, LocationNameList.slaveShackTwo, LocationNameList.slaveShackThree, LocationNameList.slaveShackSeven });

			case LocationNameList.slaveShackTwo:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.slaveShackTwo, "Géza's Shack", interiorIndexZero, LocationNameList.campNorthEast);

			case LocationNameList.slaveShackThree:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.slaveShackThree, "Janos's Shack", interiorIndexOne, LocationNameList.campNorthEast);

			case LocationNameList.slaveShackSeven:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.slaveShackSeven, "Clay's Shack", interiorIndexTwo, LocationNameList.campNorthEast);

			case LocationNameList.campCenter:

				return new MapLocation(ZoneKeyList.lovashiCamp, LocationNameList.campCenter, "Center", fourInteriors, new string[] { LocationNameList.campNorthEast, LocationNameList.campManse, LocationNameList.campSouthEast, LocationNameList.slaveShackOne, LocationNameList.stables, LocationNameList.temple, LocationNameList.guardShack }, new ZoneButtonInfo[] { new ZoneButtonInfo(ZoneKeyList.forest, MapPopUpWindow.eastButtonIndex) });

			case LocationNameList.slaveShackOne:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.slaveShackOne, "Bálint's Shack", interiorIndexZero, LocationNameList.campCenter);

			case LocationNameList.stables:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.stables, "Stables", interiorIndexOne, LocationNameList.campCenter);

			case LocationNameList.temple:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.temple, "Temple", interiorIndexTwo, LocationNameList.campCenter);

			case LocationNameList.guardShack:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.guardShack, "Gate Guardhouse", interiorIndexThree, LocationNameList.campCenter);

			case LocationNameList.campManse:

				return new MapLocation(ZoneKeyList.lovashiCamp, LocationNameList.campManse, "Manse", fourInteriors, new string[] { LocationNameList.campCenter, LocationNameList.guardHouseTopFloor, LocationNameList.guardHouseNorthEast, LocationNameList.slaveShackEight, LocationNameList.slaveShackNine}, new ZoneButtonInfo[] { new ZoneButtonInfo(ZoneKeyList.manseFirstFloor, MapPopUpWindow.westButtonIndex) });

			case LocationNameList.slaveShackEight:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.slaveShackEight, "Weft's Shack", interiorIndexZero, LocationNameList.campManse);

			case LocationNameList.slaveShackNine:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.slaveShackNine, "Manse Slave Shack", interiorIndexOne, LocationNameList.campManse);

			case LocationNameList.guardHouseNorthEast:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.guardHouseNorthEast, "Barracks 1F - North East", interiorIndexTwo, LocationNameList.campManse);

			case LocationNameList.campSouthEast:

				return new MapLocation(ZoneKeyList.lovashiCamp, LocationNameList.campSouthEast, "South East", fourInteriors, new string[] { LocationNameList.campCenter, LocationNameList.campMineEntrance, LocationNameList.messHall, LocationNameList.slaveShackFour, LocationNameList.slaveShackFive, LocationNameList.slaveShackSix});

			case LocationNameList.messHall:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.messHall, "Mess Hall", interiorIndexZero, LocationNameList.campSouthEast);

			case LocationNameList.slaveShackFour:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.slaveShackFour, "Kastor's Shack", interiorIndexOne, LocationNameList.campSouthEast);

			case LocationNameList.slaveShackFive:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.slaveShackFive, "Ervin's Shack", interiorIndexTwo, LocationNameList.campSouthEast);

			case LocationNameList.slaveShackSix:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.slaveShackSix, "Thatch's Shack", interiorIndexThree, LocationNameList.campSouthEast);

			case LocationNameList.campMineEntrance:

				return new MapLocation(ZoneKeyList.lovashiCamp, LocationNameList.campMineEntrance, "Mine Entrance", twoInteriors, new string[] { LocationNameList.campSouthEast, LocationNameList.stockhouse, LocationNameList.guardHouseSouthWest}, new ZoneButtonInfo[] { new ZoneButtonInfo(ZoneKeyList.mineLvl1, MapPopUpWindow.westNorthButtonIndex) });

			case LocationNameList.stockhouse:

				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.stockhouse, LocationNameList.stockhouse, interiorIndexZero, LocationNameList.campMineEntrance);

			case LocationNameList.guardHouseSouthWest:
			
				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.guardHouseSouthWest, "Barracks 1F - South West", interiorIndexOne, LocationNameList.campMineEntrance);
			
			case LocationNameList.guardHouseTopFloor:
			
				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.guardHouseTopFloor, "Barracks 2F", interiorIndexTwo, LocationNameList.campManse);

			case LocationNameList.campNorthWest:

				return new MapLocation(ZoneKeyList.lovashiCamp, LocationNameList.campNorthWest, "North West", oneInterior, new string[] { LocationNameList.campManse, LocationNameList.bodyPile});

			case LocationNameList.bodyPile:
			
				return new MapInterior(ZoneKeyList.lovashiCamp, LocationNameList.bodyPile, LocationNameList.bodyPile, interiorIndexZero, LocationNameList.campNorthWest);
		}

        string mineLvl1SceneName = name;
		switch(mineLvl1SceneName.Replace(ZoneKeyList.mineLvl1,""))
		{
			case LocationNameList.section1a:
				
				return new MapLocation(ZoneKeyList.mineLvl1, ZoneKeyList.mineLvl1+LocationNameList.section1a, "1a - Entrance", zeroInteriors, new string[]{ZoneKeyList.mineLvl1+LocationNameList.section1b}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.lovashiCamp, MapPopUpWindow.eastButtonIndex)});
			
			case LocationNameList.section1b:
				
				return new MapLocation(ZoneKeyList.mineLvl1, ZoneKeyList.mineLvl1+LocationNameList.section1b, "1b", zeroInteriors, new string[]{ZoneKeyList.mineLvl1+LocationNameList.section1a}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.mineLvl2,MapPopUpWindow.westButtonIndex)});
			
			case LocationNameList.section1c:
				
				return new MapLocation(ZoneKeyList.mineLvl1, ZoneKeyList.mineLvl1+LocationNameList.section1c, "1c", zeroInteriors, new string[]{ZoneKeyList.mineLvl1+LocationNameList.section1b});
		}
		
		string mineLvl2SceneName = name;
		switch(mineLvl2SceneName.Replace(ZoneKeyList.mineLvl2,""))
		{
			case LocationNameList.section1a:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section1a, "1a - Stairs", zeroInteriors, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section1b, ZoneKeyList.mineLvl2+LocationNameList.section6}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.mineLvl1, MapPopUpWindow.southEastButtonIndex)});
			case LocationNameList.section1b:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section1b, "1b - Ruined Inn", oneInterior, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section1a, ZoneKeyList.mineLvl2+LocationNameList.section2a, ZoneKeyList.mineLvl2+LocationNameList.section1c});
			case LocationNameList.section1c:
				return new MapInterior(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section1c, "1c - Ruined Bar", interiorIndexOne, ZoneKeyList.mineLvl2+LocationNameList.section1b);
			
			case LocationNameList.section2a:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section2a, "2a - Stairs", zeroInteriors, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section1b, ZoneKeyList.mineLvl2+LocationNameList.section1c, ZoneKeyList.mineLvl2+LocationNameList.section2b}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.mineLvl3, MapPopUpWindow.westSouthButtonIndex)});
			case LocationNameList.section2b:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section2b, "2b - Armory", zeroInteriors, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section2a, ZoneKeyList.mineLvl2+LocationNameList.section3a});
			
			case LocationNameList.section3a:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section3a, "3a", oneInterior, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section2b, ZoneKeyList.mineLvl2+LocationNameList.section3b, ZoneKeyList.mineLvl2+LocationNameList.section5});
			case LocationNameList.section3b:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section3b, "3b", zeroInteriors, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section3a, ZoneKeyList.mineLvl2+LocationNameList.section4, ZoneKeyList.mineLvl2+LocationNameList.section7a});
			
			case LocationNameList.section4:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section4, "4", zeroInteriors, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section3b, ZoneKeyList.mineLvl2+LocationNameList.section7a});
			
			case LocationNameList.section5:
				return new MapInterior(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section5, "5 - Collapsed Vault", interiorIndexZero, ZoneKeyList.mineLvl2+LocationNameList.section3a);

			case LocationNameList.section6:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section6, "6", zeroInteriors, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section1a, ZoneKeyList.mineLvl2+LocationNameList.section7a});
			
			case LocationNameList.section7a:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section7a, "7a", zeroInteriors, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section3b, ZoneKeyList.mineLvl2+LocationNameList.section4, ZoneKeyList.mineLvl2+LocationNameList.section6, ZoneKeyList.mineLvl2+LocationNameList.section7b});
			case LocationNameList.section7b:
				return new MapLocation(ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl2+LocationNameList.section7b, "7b", zeroInteriors, new string[]{ZoneKeyList.mineLvl2+LocationNameList.section2b, ZoneKeyList.mineLvl2+LocationNameList.section7a});
			default:
				break;
		}
		
		string mineLvl3SceneName = name;
		switch(mineLvl3SceneName.Replace(ZoneKeyList.mineLvl3,""))
		{
			case LocationNameList.section1a:
				
				return new MapLocation(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section1a, "1a - Stairs", oneInterior, new string[]{ZoneKeyList.mineLvl3+LocationNameList.section1b, ZoneKeyList.mineLvl3+LocationNameList.section2a, ZoneKeyList.mineLvl3+LocationNameList.section4a}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.mineLvl2, MapPopUpWindow.eastNorthButtonIndex)});
			
			case LocationNameList.section1b:
				
				return new MapInterior(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section1b, "1b", interiorIndexZero, ZoneKeyList.mineLvl3+LocationNameList.section1a);
			
			case LocationNameList.section2a:
				
				return new MapLocation(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section2a, "2a", zeroInteriors, new string[]{ZoneKeyList.mineLvl3+LocationNameList.section1a, ZoneKeyList.mineLvl3+LocationNameList.section2b});
			
			case LocationNameList.section2b:
				
				return new MapLocation(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section2b, "2b", zeroInteriors, new string[]{ZoneKeyList.mineLvl3+LocationNameList.section2a, ZoneKeyList.mineLvl3+LocationNameList.section3a});
			
			case LocationNameList.section3a:
				
				return new MapLocation(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section3a, "3a", oneInterior, new string[]{ZoneKeyList.mineLvl3+LocationNameList.section2b, ZoneKeyList.mineLvl3+LocationNameList.section3b, ZoneKeyList.mineLvl3+LocationNameList.section6a, ZoneKeyList.mineLvl3+LocationNameList.section7});
			
			case LocationNameList.section3b:
				
				return new MapInterior(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section3b, "3b - Stockroom", interiorIndexOne, ZoneKeyList.mineLvl3+LocationNameList.section3a);
			
			case LocationNameList.section4a:
				
				return new MapLocation(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section4a, "4a", zeroInteriors, new string[]{ZoneKeyList.mineLvl3+LocationNameList.section1a, ZoneKeyList.mineLvl3+LocationNameList.section4b});
			
			case LocationNameList.section4b:
				
				return new MapLocation(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section4b, "4b - River Source", zeroInteriors, new string[]{ZoneKeyList.mineLvl3+LocationNameList.section4a, ZoneKeyList.mineLvl3+LocationNameList.section5});
			
			case LocationNameList.section5:
				
				return new MapLocation(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section5, "5", oneInterior, new string[]{ZoneKeyList.mineLvl3+LocationNameList.section4b, ZoneKeyList.mineLvl3+LocationNameList.minerCamp, ZoneKeyList.mineLvl3+LocationNameList.section6a});
			
			case LocationNameList.minerCamp:

				return new MapInterior(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.minerCamp, "Miner Camp", interiorIndexZero, ZoneKeyList.mineLvl3+LocationNameList.section5);
				
			case LocationNameList.section6a:
				
				return new MapLocation(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section6a, "6a", zeroInteriors, new string[]{ZoneKeyList.mineLvl3+LocationNameList.section3a, ZoneKeyList.mineLvl3+LocationNameList.section5});
			
			case LocationNameList.section7:
				
				return new MapLocation(ZoneKeyList.mineLvl3, ZoneKeyList.mineLvl3+LocationNameList.section7, "7 - Final Tunnel", zeroInteriors, new string[]{ZoneKeyList.mineLvl3+LocationNameList.section3a});
		}
		
		string manseFirstFloorSceneName = name;
		switch(manseFirstFloorSceneName.Replace(ZoneKeyList.manseFirstFloor,""))
		{
			case LocationNameList.section1a:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section1a, LocationNameList.section1a, zeroInteriors, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.section1b}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.lovashiCamp, MapPopUpWindow.eastSouthButtonIndex)} );
			case LocationNameList.section1b:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section1b, LocationNameList.section1b, zeroInteriors, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.section1a, ZoneKeyList.manseFirstFloor+LocationNameList.section1c});
			case LocationNameList.section1c:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section1c, LocationNameList.section1c, zeroInteriors, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.kitchens, ZoneKeyList.manseFirstFloor+LocationNameList.section1b});
			case LocationNameList.kitchens:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.kitchens, LocationNameList.kitchens, zeroInteriors, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.section1c}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.lovashiCamp, MapPopUpWindow.southEastButtonIndex), new ZoneButtonInfo(ZoneKeyList.manseSecondFloor, MapPopUpWindow.southWestButtonIndex)});

			case LocationNameList.section2a:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section2a, LocationNameList.section2a, zeroInteriors, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.diningRoom, ZoneKeyList.manseFirstFloor+LocationNameList.section2b});
			case LocationNameList.section2b:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section2b, LocationNameList.section2b, oneInterior, new string[] { ZoneKeyList.manseFirstFloor + LocationNameList.section2a, ZoneKeyList.manseFirstFloor + LocationNameList.section2c, ZoneKeyList.manseFirstFloor + LocationNameList.section3c, ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit}, new ZoneButtonInfo[] { new ZoneButtonInfo(ZoneKeyList.pit, MapPopUpWindow.westNorthButtonIndex) });
			case LocationNameList.section2c:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section2c, LocationNameList.section2c, zeroInteriors, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.section2b, ZoneKeyList.manseFirstFloor+LocationNameList.stairsToPit}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.pit, MapPopUpWindow.westNorthButtonIndex)});
			case LocationNameList.stairsToPit:
				return new MapInterior(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.stairsToPit, "Stairs to Pit", interiorIndexZero, ZoneKeyList.manseFirstFloor+LocationNameList.section2b);
			case LocationNameList.diningRoom:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.diningRoom, LocationNameList.diningRoom, zeroInteriors, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.kitchens,ZoneKeyList.manseFirstFloor+LocationNameList.section1b});

			case LocationNameList.section3a:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section3a, LocationNameList.section3a, zeroInteriors, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.section1a, ZoneKeyList.manseFirstFloor+LocationNameList.section3b});
			case LocationNameList.section3b:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section3b, LocationNameList.section3b, oneInterior, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.section3a,ZoneKeyList.manseFirstFloor+LocationNameList.section3c,ZoneKeyList.manseFirstFloor+LocationNameList.section3d});
			case LocationNameList.section3c:
				return new MapLocation(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section3c, LocationNameList.section3c + " - Library", oneInterior, new string[]{ZoneKeyList.manseFirstFloor+LocationNameList.section2b, ZoneKeyList.manseFirstFloor+LocationNameList.section3b}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.manseSecondFloor, MapPopUpWindow.westButtonIndex)}, new InteriorDisplayStatRequirements[] { new InteriorDisplayStatRequirements(PrimaryStat.Wisdom, 3)});
			case LocationNameList.section3d:
				return new MapInterior(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section3e, "Page's Room", interiorIndexZero, ZoneKeyList.manseFirstFloor+LocationNameList.section3b);
			case LocationNameList.section3e:
				return new MapInterior(ZoneKeyList.manseFirstFloor, ZoneKeyList.manseFirstFloor+LocationNameList.section3e, "Secret Room", interiorIndexZero, ZoneKeyList.manseFirstFloor+LocationNameList.section3c);
		}
		
		string manseSecondFloorSceneName = name;
		switch(manseSecondFloorSceneName.Replace(ZoneKeyList.manseSecondFloor,""))
		{
			case LocationNameList.section1a:
				return new MapLocation(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor+LocationNameList.section1a, LocationNameList.section1a, twoInteriors, new string[]{ZoneKeyList.manseSecondFloor+LocationNameList.section1b, ZoneKeyList.manseSecondFloor+LocationNameList.section1c, ZoneKeyList.manseSecondFloor+LocationNameList.office, ZoneKeyList.manseSecondFloor+LocationNameList.section2a, ZoneKeyList.manseSecondFloor+LocationNameList.section3c}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.manseFirstFloor, MapPopUpWindow.eastSouthButtonIndex)});
			case LocationNameList.section1b:
				return new MapInterior(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor + LocationNameList.section1b, "Director's Room", interiorIndexZero, ZoneKeyList.manseSecondFloor + LocationNameList.section1a);
			case LocationNameList.section1c:
				return new MapInterior(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor + LocationNameList.section1b, "Tabor's Room", interiorIndexOne, ZoneKeyList.manseSecondFloor + LocationNameList.section1a);
			case LocationNameList.office:
				return new MapLocation(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor+LocationNameList.office, LocationNameList.office, zeroInteriors, new string[]{ZoneKeyList.manseSecondFloor+LocationNameList.section1a});
			
			
			case LocationNameList.section2a:
				return new MapLocation(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor+LocationNameList.section2a, LocationNameList.section2a, twoInteriors, new string[]{ZoneKeyList.manseSecondFloor+LocationNameList.section1a, ZoneKeyList.manseSecondFloor+LocationNameList.section2b, ZoneKeyList.manseSecondFloor+LocationNameList.section2c, ZoneKeyList.manseSecondFloor+LocationNameList.section2d}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.manseFirstFloor, MapPopUpWindow.northWestButtonIndex)});
			case LocationNameList.section2b:
				return new MapLocation(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor+LocationNameList.section2b, LocationNameList.section2b, zeroInteriors, new string[]{ZoneKeyList.manseSecondFloor+LocationNameList.section2a, ZoneKeyList.manseSecondFloor+LocationNameList.section3a});//, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.manseFirstFloor, MapPopUpWindow.northButtonIndex)});
			case LocationNameList.section2c:
				return new MapInterior(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor + LocationNameList.section2c, "Child's Room 1", interiorIndexZero, ZoneKeyList.manseSecondFloor + LocationNameList.section2a);
			case LocationNameList.section2d:
				return new MapInterior(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor + LocationNameList.section2d, "Child's Room 2", interiorIndexOne, ZoneKeyList.manseSecondFloor + LocationNameList.section2a);
			
			case LocationNameList.section3a:
				return new MapLocation(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor+LocationNameList.section3a, LocationNameList.section3a, zeroInteriors, new string[]{ZoneKeyList.manseSecondFloor+LocationNameList.section2b, ZoneKeyList.manseSecondFloor+LocationNameList.section3b, ZoneKeyList.manseSecondFloor+LocationNameList.section3c});
			case LocationNameList.section3b:
				return new MapLocation(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor+LocationNameList.section3b, LocationNameList.section3b, zeroInteriors, new string[]{ZoneKeyList.manseSecondFloor+LocationNameList.section3a, ZoneKeyList.manseSecondFloor+LocationNameList.stockroom});
			case LocationNameList.section3c:
				return new MapLocation(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor+LocationNameList.section3c, LocationNameList.section3c, zeroInteriors, new string[]{ZoneKeyList.manseSecondFloor+LocationNameList.section3a, ZoneKeyList.manseSecondFloor+LocationNameList.stockroom, ZoneKeyList.manseSecondFloor+LocationNameList.section1a});
			case LocationNameList.stockroom:
				return new MapLocation(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseSecondFloor+LocationNameList.stockroom, LocationNameList.stockroom, zeroInteriors, new string[]{ZoneKeyList.manseSecondFloor+LocationNameList.section3b, ZoneKeyList.manseSecondFloor+LocationNameList.section3c}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.manseFirstFloor, MapPopUpWindow.southButtonIndex)});
		}
		
		string pitSceneName = name;
		switch(pitSceneName.Replace(ZoneKeyList.pit,""))
		{
			case LocationNameList.section1a:
				return new MapLocation(ZoneKeyList.pit, ZoneKeyList.pit+LocationNameList.section1a, LocationNameList.section1a, zeroInteriors, new string[]{ZoneKeyList.pit+LocationNameList.section2a}, new ZoneButtonInfo[]{new ZoneButtonInfo(ZoneKeyList.manseFirstFloor, MapPopUpWindow.westButtonIndex)});
			case LocationNameList.section2a:
				return new MapLocation(ZoneKeyList.pit, ZoneKeyList.pit+LocationNameList.section2a, LocationNameList.section2a, zeroInteriors, new string[]{ZoneKeyList.pit+LocationNameList.section1a,ZoneKeyList.pit+LocationNameList.section2b,ZoneKeyList.pit+LocationNameList.section2c});
			case LocationNameList.section2b:
				return new MapLocation(ZoneKeyList.pit, ZoneKeyList.pit+LocationNameList.section2b, LocationNameList.section2b, oneInterior, new string[]{ZoneKeyList.pit+LocationNameList.section2a, ZoneKeyList.pit+LocationNameList.section2d});
			case LocationNameList.section2c:
				return new MapLocation(ZoneKeyList.pit, ZoneKeyList.pit+LocationNameList.section2c, LocationNameList.section2c, zeroInteriors, new string[]{ZoneKeyList.pit+LocationNameList.section2a});
			case LocationNameList.section2d:
				return new MapInterior(ZoneKeyList.pit, ZoneKeyList.pit+LocationNameList.section2d, "Trash Chute", interiorIndexZero, ZoneKeyList.pit+LocationNameList.section2b);
			
			default:
				return new MapLocation(name, name, name, zeroInteriors, new string[0]);
		}
		
	}

}
