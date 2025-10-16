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
            name = AreaNameList.campNorthEast;
        }

		string zoneKey = name;
		
		switch(zoneKey)
		{
			case AreaNameList.camp:
				
				return new MapZone(AreaNameList.camp, "Camp", new string[]{AreaNameList.forest});
			
			case AreaNameList.mineLvl1:
				
				return new MapZone(AreaNameList.mineLvl1, "Mine Level 1", null);
			
			case AreaNameList.mineLvl2:
				
				return new MapZone(AreaNameList.mineLvl1, "Mine Level 2", null);
			
			case AreaNameList.mineLvl3:
				
				return new MapZone(AreaNameList.mineLvl1, "Mine Level 3", null);
			
			case AreaNameList.manseFirstFloor:
				
				return new MapZone(AreaNameList.manseFirstFloor, "Manse-1F", null);
	
			case AreaNameList.manseSecondFloor:
				
				return new MapZone(AreaNameList.manseSecondFloor, "Manse-2F", null);
	
			case AreaNameList.pit:
				
				return new MapZone(AreaNameList.pit, "The Pit", null);
			
			case AreaNameList.forest:
				
				return new MapZone(AreaNameList.forest, "Forest", new string[]{AreaNameList.camp});
		}
		
		string campSceneName = name;
		switch (campSceneName)
		{
			case AreaNameList.campNorthEast:

				return new MapLocation(AreaNameList.camp, AreaNameList.campNorthEast, "Camp - North East", fastTravelAccessible, threeInteriors, new string[] { AreaNameList.campCenter, AreaNameList.slaveShackTwo, AreaNameList.slaveShackThree, AreaNameList.slaveShackSeven });

			case AreaNameList.slaveShackTwo:

				return new MapInterior(AreaNameList.camp, AreaNameList.slaveShackTwo, "Garcha's Shack", interiorIndexZero, AreaNameList.campNorthEast);

			case AreaNameList.slaveShackThree:

				return new MapInterior(AreaNameList.camp, AreaNameList.slaveShackThree, "Janos's Shack", interiorIndexOne, AreaNameList.campNorthEast);

			case AreaNameList.slaveShackSeven:

				return new MapInterior(AreaNameList.camp, AreaNameList.slaveShackSeven, "Clay's Shack", interiorIndexTwo, AreaNameList.campNorthEast);

			case AreaNameList.campCenter:

				return new MapLocation(AreaNameList.camp, AreaNameList.campCenter, "Camp - Center", notFastTravelAccessible, fourInteriors, new string[] { AreaNameList.campNorthEast, AreaNameList.campManse, AreaNameList.campSouthEast, AreaNameList.slaveShackOne, AreaNameList.stables, AreaNameList.temple, AreaNameList.guardShack }, new ZoneButtonInfo[] { new ZoneButtonInfo(AreaNameList.forest, MapPopUpWindow.eastButtonIndex) });

			case AreaNameList.slaveShackOne:

				return new MapInterior(AreaNameList.camp, AreaNameList.slaveShackOne, "Bálint's Shack", interiorIndexZero, AreaNameList.campCenter);

			case AreaNameList.stables:

				return new MapInterior(AreaNameList.camp, AreaNameList.stables, "Stables", interiorIndexOne, AreaNameList.campCenter);

			case AreaNameList.temple:

				return new MapInterior(AreaNameList.camp, AreaNameList.temple, "Temple", interiorIndexTwo, AreaNameList.campCenter);

			case AreaNameList.guardShack:

				return new MapInterior(AreaNameList.camp, AreaNameList.guardShack, "Gate Guardhouse", interiorIndexThree, AreaNameList.campCenter);

			case AreaNameList.campManse:

				return new MapLocation(AreaNameList.camp, AreaNameList.campManse, "Camp - Manse", notFastTravelAccessible, fourInteriors, new string[] { AreaNameList.campCenter, AreaNameList.guardHouseTopFloor, AreaNameList.guardHouseNorthEast, AreaNameList.slaveShackEight, AreaNameList.slaveShackNine}, new ZoneButtonInfo[] { new ZoneButtonInfo(AreaNameList.manseFirstFloor, MapPopUpWindow.westButtonIndex) });

			case AreaNameList.slaveShackEight:

				return new MapInterior(AreaNameList.camp, AreaNameList.slaveShackEight, "Sampson's Shack", interiorIndexZero, AreaNameList.campManse);

			case AreaNameList.slaveShackNine:

				return new MapInterior(AreaNameList.camp, AreaNameList.slaveShackNine, "Manse Slave Shack", interiorIndexOne, AreaNameList.campManse);

			case AreaNameList.guardHouseNorthEast:

				return new MapInterior(AreaNameList.camp, AreaNameList.guardHouseNorthEast, "Guardhouse 1F - North East", interiorIndexTwo, AreaNameList.campManse);

			case AreaNameList.campSouthEast:

				return new MapLocation(AreaNameList.camp, AreaNameList.campSouthEast, "Camp - South East", fastTravelAccessible, fourInteriors, new string[] { AreaNameList.campCenter, AreaNameList.campMineEntrance, AreaNameList.messHall, AreaNameList.slaveShackFour, AreaNameList.slaveShackFive, AreaNameList.slaveShackSix});

			case AreaNameList.messHall:

				return new MapInterior(AreaNameList.camp, AreaNameList.messHall, "Mess Hall", interiorIndexZero, AreaNameList.campSouthEast);

			case AreaNameList.slaveShackFour:

				return new MapInterior(AreaNameList.camp, AreaNameList.slaveShackFour, "Kastor's Shack", interiorIndexOne, AreaNameList.campSouthEast);

			case AreaNameList.slaveShackFive:

				return new MapInterior(AreaNameList.camp, AreaNameList.slaveShackFive, "Ervin's Shack", interiorIndexTwo, AreaNameList.campSouthEast);

			case AreaNameList.slaveShackSix:

				return new MapInterior(AreaNameList.camp, AreaNameList.slaveShackSix, "Thatch's Shack", interiorIndexThree, AreaNameList.campSouthEast);

			case AreaNameList.campMineEntrance:

				return new MapLocation(AreaNameList.camp, AreaNameList.campMineEntrance, "Camp - Mine Entrance", notFastTravelAccessible, twoInteriors, new string[] { AreaNameList.campSouthEast, AreaNameList.stockhouse, AreaNameList.guardHouseSouthWest}, new ZoneButtonInfo[] { new ZoneButtonInfo(AreaNameList.mineLvl1, MapPopUpWindow.westNorthButtonIndex) });

			case AreaNameList.stockhouse:

				return new MapInterior(AreaNameList.camp, AreaNameList.stockhouse, AreaNameList.stockhouse, interiorIndexZero, AreaNameList.campMineEntrance);

			case AreaNameList.guardHouseSouthWest:
			
				return new MapInterior(AreaNameList.camp, AreaNameList.guardHouseSouthWest, "Guardhouse 1F - South West", interiorIndexOne, AreaNameList.campMineEntrance);
			
			case AreaNameList.guardHouseTopFloor:
			
				return new MapInterior(AreaNameList.camp, AreaNameList.guardHouseTopFloor, "Guardhouse 2F", interiorIndexTwo, AreaNameList.campManse);
		}

        string mineLvl1SceneName = name;
		switch(mineLvl1SceneName.Replace(AreaNameList.mineLvl1,""))
		{
			case AreaNameList.section1a:
				
				return new MapLocation(AreaNameList.mineLvl1, AreaNameList.mineLvl1+AreaNameList.section1a, "1a - Entrance", fastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl1+AreaNameList.section1b}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.camp, MapPopUpWindow.eastButtonIndex)});
			
			case AreaNameList.section1b:
				
				return new MapLocation(AreaNameList.mineLvl1, AreaNameList.mineLvl1+AreaNameList.section1b, "1b", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl1+AreaNameList.section1a}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.mineLvl2,MapPopUpWindow.westButtonIndex)});
			
			case AreaNameList.section1c:
				
				return new MapLocation(AreaNameList.mineLvl1, AreaNameList.mineLvl1+AreaNameList.section1c, "1c", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl1+AreaNameList.section1b});
		}
		
		string mineLvl2SceneName = name;
		switch(mineLvl2SceneName.Replace(AreaNameList.mineLvl2,""))
		{
			case AreaNameList.section1a:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section1a, "1a - Stairs", fastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl2+AreaNameList.section1b, AreaNameList.mineLvl2+AreaNameList.section6}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.mineLvl1, MapPopUpWindow.southEastButtonIndex)});
			case AreaNameList.section1b:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section1b, "1b - Ruined Inn", notFastTravelAccessible, oneInterior, new string[]{AreaNameList.mineLvl2+AreaNameList.section1a, AreaNameList.mineLvl2+AreaNameList.section2a, AreaNameList.mineLvl2+AreaNameList.section1c});
			case AreaNameList.section1c:
				return new MapInterior(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section1c, "1c - Ruined Bar", interiorIndexOne, AreaNameList.mineLvl2+AreaNameList.section1b);
			
			case AreaNameList.section2a:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section2a, "2a - Stairs", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl2+AreaNameList.section1b, AreaNameList.mineLvl2+AreaNameList.section1c, AreaNameList.mineLvl2+AreaNameList.section2b}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.mineLvl3, MapPopUpWindow.westSouthButtonIndex)});
			case AreaNameList.section2b:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section2b, "2b - Armory", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl2+AreaNameList.section2a, AreaNameList.mineLvl2+AreaNameList.section3a});
			
			case AreaNameList.section3a:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section3a, "3a", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl2+AreaNameList.section2b, AreaNameList.mineLvl2+AreaNameList.section3b});
			case AreaNameList.section3b:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section3b, "3b", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl2+AreaNameList.section3a, AreaNameList.mineLvl2+AreaNameList.section4, AreaNameList.mineLvl2+AreaNameList.section7a});
			
			case AreaNameList.section4:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section4, "4", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl2+AreaNameList.section3b, AreaNameList.mineLvl2+AreaNameList.section7a});
			
			case AreaNameList.section5a:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section5a, "5a - Collapsed Vault", notFastTravelAccessible, oneInterior, new string[]{AreaNameList.mineLvl2+AreaNameList.section3a, AreaNameList.mineLvl2+AreaNameList.section5b, AreaNameList.mineLvl2+AreaNameList.section7a});
			case AreaNameList.section5b:
				return new MapInterior(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section5b, "5b - Below", interiorIndexZero, AreaNameList.mineLvl2+AreaNameList.section5a);

			case AreaNameList.section6:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section6, "6", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl2+AreaNameList.section1a, AreaNameList.mineLvl2+AreaNameList.section7a});
			
			case AreaNameList.section7a:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section7a, "7a", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl2+AreaNameList.section3b, AreaNameList.mineLvl2+AreaNameList.section4, AreaNameList.mineLvl2+AreaNameList.section6, AreaNameList.mineLvl2+AreaNameList.section7b});
			case AreaNameList.section7b:
				return new MapLocation(AreaNameList.mineLvl2, AreaNameList.mineLvl2+AreaNameList.section7b, "7b", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl2+AreaNameList.section2b, AreaNameList.mineLvl2+AreaNameList.section7a});
			default:
				break;
		}
		
		string mineLvl3SceneName = name;
		switch(mineLvl3SceneName.Replace(AreaNameList.mineLvl3,""))
		{
			case AreaNameList.section1a:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section1a, "1a - Stairs", fastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl3+AreaNameList.section1b, AreaNameList.mineLvl3+AreaNameList.section2a, AreaNameList.mineLvl3+AreaNameList.section4a}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.mineLvl2, MapPopUpWindow.eastNorthButtonIndex)});
			
			case AreaNameList.section1b:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section1b, "1b", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl3+AreaNameList.section1a});
			
			case AreaNameList.section2a:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section2a, "2a", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl3+AreaNameList.section1a, AreaNameList.mineLvl3+AreaNameList.section2b});
			
			case AreaNameList.section2b:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section2b, "2b", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl3+AreaNameList.section2a, AreaNameList.mineLvl3+AreaNameList.section3a});
			
			case AreaNameList.section3a:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section3a, "3a", fastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl3+AreaNameList.section2b, AreaNameList.mineLvl3+AreaNameList.section3b, AreaNameList.mineLvl3+AreaNameList.section6a, AreaNameList.mineLvl3+AreaNameList.section7});
			
			case AreaNameList.section3b:
				
				return new MapInterior(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section3b, "3b - Stockroom", interiorIndexOne, AreaNameList.mineLvl3+AreaNameList.section3a);
			
			case AreaNameList.section4a:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section4a, "4a", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl3+AreaNameList.section1a, AreaNameList.mineLvl3+AreaNameList.section4b});
			
			case AreaNameList.section4b:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section4b, "4b - River Source", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl3+AreaNameList.section4a, AreaNameList.mineLvl3+AreaNameList.section5});
			
			case AreaNameList.section5:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section5, "5", fastTravelAccessible, oneInterior, new string[]{AreaNameList.mineLvl3+AreaNameList.section4b, AreaNameList.mineLvl3+AreaNameList.minerCamp, AreaNameList.mineLvl3+AreaNameList.section6a});
			
			case AreaNameList.minerCamp:

				return new MapInterior(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.minerCamp, "Miner Camp", interiorIndexZero, AreaNameList.mineLvl3+AreaNameList.section5);
				
			case AreaNameList.section6a:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section6a, "6a", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl3+AreaNameList.section3a, AreaNameList.mineLvl3+AreaNameList.section5});
			
			case AreaNameList.section7:
				
				return new MapLocation(AreaNameList.mineLvl3, AreaNameList.mineLvl3+AreaNameList.section7, "7 - Final Tunnel", notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.mineLvl3+AreaNameList.section3a});
		}
		
		string manseFirstFloorSceneName = name;
		switch(manseFirstFloorSceneName.Replace(AreaNameList.manseFirstFloor,""))
		{
			case AreaNameList.section1a:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section1a, AreaNameList.section1a, fastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseFirstFloor+AreaNameList.section1b}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.camp, MapPopUpWindow.eastSouthButtonIndex)} );
			case AreaNameList.section1b:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section1b, AreaNameList.section1b, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseFirstFloor+AreaNameList.section1a, AreaNameList.manseFirstFloor+AreaNameList.section1c});
			case AreaNameList.section1c:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section1c, AreaNameList.section1c, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseFirstFloor+AreaNameList.kitchens, AreaNameList.manseFirstFloor+AreaNameList.section1b});
			case AreaNameList.kitchens:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.kitchens, AreaNameList.kitchens, fastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseFirstFloor+AreaNameList.section1c}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.camp, MapPopUpWindow.southEastButtonIndex), new ZoneButtonInfo(AreaNameList.manseSecondFloor, MapPopUpWindow.southWestButtonIndex)});

			case AreaNameList.section2a:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section2a, AreaNameList.section2a, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseFirstFloor+AreaNameList.diningRoom, AreaNameList.manseFirstFloor+AreaNameList.section2b});
			case AreaNameList.section2b:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section2b, AreaNameList.section2b, notFastTravelAccessible, oneInterior, new string[] { AreaNameList.manseFirstFloor + AreaNameList.section2a, AreaNameList.manseFirstFloor + AreaNameList.section2c, AreaNameList.manseFirstFloor + AreaNameList.section3c, AreaNameList.manseFirstFloor + AreaNameList.stairsToPit}, new ZoneButtonInfo[] { new ZoneButtonInfo(AreaNameList.pit, MapPopUpWindow.westNorthButtonIndex) });
			case AreaNameList.section2c:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section2c, AreaNameList.section2c, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseFirstFloor+AreaNameList.section2b, AreaNameList.manseFirstFloor+AreaNameList.stairsToPit}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.pit, MapPopUpWindow.westNorthButtonIndex)});
			case AreaNameList.stairsToPit:
				return new MapInterior(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.stairsToPit, "Stairs to Pit", interiorIndexZero, AreaNameList.manseFirstFloor+AreaNameList.section2b);
			case AreaNameList.diningRoom:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.diningRoom, AreaNameList.diningRoom, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseFirstFloor+AreaNameList.kitchens,AreaNameList.manseFirstFloor+AreaNameList.section1b});

			case AreaNameList.section3a:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section3a, AreaNameList.section3a, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseFirstFloor+AreaNameList.section1a, AreaNameList.manseFirstFloor+AreaNameList.section3b});
			case AreaNameList.section3b:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section3b, AreaNameList.section3b, notFastTravelAccessible, oneInterior, new string[]{AreaNameList.manseFirstFloor+AreaNameList.section3a,AreaNameList.manseFirstFloor+AreaNameList.section3c,AreaNameList.manseFirstFloor+AreaNameList.section3d});
			case AreaNameList.section3c:
				return new MapLocation(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section3c, AreaNameList.section3c + " - Library", notFastTravelAccessible, oneInterior, new string[]{AreaNameList.manseFirstFloor+AreaNameList.section2b, AreaNameList.manseFirstFloor+AreaNameList.section3b}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.manseSecondFloor, MapPopUpWindow.westButtonIndex)}, new InteriorDisplayStatRequirements[] { new InteriorDisplayStatRequirements(PrimaryStat.Wisdom, 3)});
			case AreaNameList.section3d:
				return new MapInterior(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section3e, "Page's Room", interiorIndexZero, AreaNameList.manseFirstFloor+AreaNameList.section3b);
			case AreaNameList.section3e:
				return new MapInterior(AreaNameList.manseFirstFloor, AreaNameList.manseFirstFloor+AreaNameList.section3e, "Secret Room", interiorIndexZero, AreaNameList.manseFirstFloor+AreaNameList.section3c);
		}
		
		string manseSecondFloorSceneName = name;
		switch(manseSecondFloorSceneName.Replace(AreaNameList.manseSecondFloor,""))
		{
			case AreaNameList.section1a:
				return new MapLocation(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor+AreaNameList.section1a, AreaNameList.section1a, notFastTravelAccessible, twoInteriors, new string[]{AreaNameList.manseSecondFloor+AreaNameList.section1b, AreaNameList.manseSecondFloor+AreaNameList.section1c, AreaNameList.manseSecondFloor+AreaNameList.office, AreaNameList.manseSecondFloor+AreaNameList.section2a, AreaNameList.manseSecondFloor+AreaNameList.section3c}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.manseFirstFloor, MapPopUpWindow.eastSouthButtonIndex)});
			case AreaNameList.section1b:
				return new MapInterior(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor + AreaNameList.section1b, "Director's Room", interiorIndexZero, AreaNameList.manseSecondFloor + AreaNameList.section1a);
			case AreaNameList.section1c:
				return new MapInterior(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor + AreaNameList.section1b, "Tabor's Room", interiorIndexOne, AreaNameList.manseSecondFloor + AreaNameList.section1a);
			case AreaNameList.office:
				return new MapLocation(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor+AreaNameList.office, AreaNameList.office, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseSecondFloor+AreaNameList.section1a});
			
			
			case AreaNameList.section2a:
				return new MapLocation(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor+AreaNameList.section2a, AreaNameList.section2a, notFastTravelAccessible, twoInteriors, new string[]{AreaNameList.manseSecondFloor+AreaNameList.section1a, AreaNameList.manseSecondFloor+AreaNameList.section2b, AreaNameList.manseSecondFloor+AreaNameList.section2c, AreaNameList.manseSecondFloor+AreaNameList.section2d}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.manseFirstFloor, MapPopUpWindow.northWestButtonIndex)});
			case AreaNameList.section2b:
				return new MapLocation(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor+AreaNameList.section2b, AreaNameList.section2b, fastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseSecondFloor+AreaNameList.section2a, AreaNameList.manseSecondFloor+AreaNameList.section3a});//, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.manseFirstFloor, MapPopUpWindow.northButtonIndex)});
			case AreaNameList.section2c:
				return new MapInterior(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor + AreaNameList.section2c, "Child's Room 1", interiorIndexZero, AreaNameList.manseSecondFloor + AreaNameList.section2a);
			case AreaNameList.section2d:
				return new MapInterior(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor + AreaNameList.section2d, "Child's Room 2", interiorIndexOne, AreaNameList.manseSecondFloor + AreaNameList.section2a);
			
			case AreaNameList.section3a:
				return new MapLocation(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor+AreaNameList.section3a, AreaNameList.section3a, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseSecondFloor+AreaNameList.section2b, AreaNameList.manseSecondFloor+AreaNameList.section3b, AreaNameList.manseSecondFloor+AreaNameList.section3c});
			case AreaNameList.section3b:
				return new MapLocation(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor+AreaNameList.section3b, AreaNameList.section3b, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseSecondFloor+AreaNameList.section3a, AreaNameList.manseSecondFloor+AreaNameList.stockroom});
			case AreaNameList.section3c:
				return new MapLocation(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor+AreaNameList.section3c, AreaNameList.section3c, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseSecondFloor+AreaNameList.section3a, AreaNameList.manseSecondFloor+AreaNameList.stockroom, AreaNameList.manseSecondFloor+AreaNameList.section1a});
			case AreaNameList.stockroom:
				return new MapLocation(AreaNameList.manseSecondFloor, AreaNameList.manseSecondFloor+AreaNameList.stockroom, AreaNameList.stockroom, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.manseSecondFloor+AreaNameList.section3b, AreaNameList.manseSecondFloor+AreaNameList.section3c}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.manseFirstFloor, MapPopUpWindow.southButtonIndex)});
		}
		
		string pitSceneName = name;
		switch(pitSceneName.Replace(AreaNameList.pit,""))
		{
			case AreaNameList.section1a:
				return new MapLocation(AreaNameList.pit, AreaNameList.pit+AreaNameList.section1a, AreaNameList.section1a, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.pit+AreaNameList.section1b}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.manseFirstFloor, MapPopUpWindow.northWestButtonIndex)});
			case AreaNameList.section1b:
				return new MapLocation(AreaNameList.pit, AreaNameList.pit+AreaNameList.section1b, AreaNameList.section1b, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.pit+AreaNameList.section1a}, new ZoneButtonInfo[]{new ZoneButtonInfo(AreaNameList.manseFirstFloor, MapPopUpWindow.westNorthButtonIndex)});
			case AreaNameList.section2a:
				return new MapLocation(AreaNameList.pit, AreaNameList.pit+AreaNameList.section2a, AreaNameList.section2a, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.pit+AreaNameList.section1b,AreaNameList.pit+AreaNameList.section2b,AreaNameList.pit+AreaNameList.section2c});
			case AreaNameList.section2b:
				return new MapLocation(AreaNameList.pit, AreaNameList.pit+AreaNameList.section2b, AreaNameList.section2b, notFastTravelAccessible, oneInterior, new string[]{AreaNameList.pit+AreaNameList.section2a, AreaNameList.pit+AreaNameList.section2d});
			case AreaNameList.section2c:
				return new MapLocation(AreaNameList.pit, AreaNameList.pit+AreaNameList.section2c, AreaNameList.section2c, notFastTravelAccessible, zeroInteriors, new string[]{AreaNameList.pit+AreaNameList.section2a});
			case AreaNameList.section2d:
				return new MapInterior(AreaNameList.pit, AreaNameList.pit+AreaNameList.section2d, "Trash Chute", interiorIndexZero, AreaNameList.pit+AreaNameList.section2b);
			
			default:
				return new MapLocation(name, name, name, notFastTravelAccessible, zeroInteriors, new string[0]);
		}
		
	}

	public static ArrayList getAllDiscoveredZones()
	{
		ArrayList discoveredZones = new ArrayList();
		
		if(MapZone.hasBeenDiscovered(AreaNameList.camp))
		{
			discoveredZones.Add(AreaNameList.camp);
		}
		
		return discoveredZones;
	}

}
