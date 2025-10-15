using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[System.Serializable]
public class MapZone : IMapObject
{
	public const int worldMapCoordsIndex = 0;
	
	public string zoneKey { get; private set; }
	public string displayName { get; private set; }
	
	public string[] adjacentZones { get; private set; }
	
	public MapZone(string zoneKey, string displayName, string[] adjacentZones)
	{
		this.zoneKey = zoneKey;
		this.displayName = displayName;
		
		this.adjacentZones = adjacentZones;
	}
	
	public bool hasBeenDiscovered()
	{
		return hasBeenDiscovered(zoneKey);
	}
	
	public static bool hasBeenDiscovered(string zoneKey)
	{
		if (State.debugDiscoverAllLocations)
		{
			return true;
		}

		try
		{
			ArrayList listOfSceneNames = null;

			if (State.allKnownMapData.TryGetValue(zoneKey, out listOfSceneNames))
			{
				return State.allKnownMapData[zoneKey].Count > 0;
			}
			else
			{
				return false;
			}

		}
		catch (Exception e)
		{
			return false;
		}
	}
	
	public int getInteriors()
	{
		return -1;
	}
	
	public string[] getAdjacentMapObjects()
	{
		return adjacentZones;
	}
	
	public ZoneButtonInfo[] getZoneButtons()
	{
		return new ZoneButtonInfo[]{};
	}
	
	public string getSceneName()
	{
		return zoneKey;
	}
	
	public string getZoneKey()
	{
		return zoneKey;
	}
	
	public string getMapUIDisplayName()
	{
		return displayName;
	}

	public string getMapUIDisplayNameWithoutZoneName()
	{
		return displayName;
	}

	public string getNotificationDisplayName()
	{
		return getMapUIDisplayName();
	}
	
	public bool isInterior()
	{
		return false;
	}
	
	public bool getIsFastTravelDestination()
	{
		return false;
	}

	public bool isVisible()
	{
		foreach(string adjacentZone in adjacentZones)
		{
			if(MapObjectList.getMapObject(adjacentZone).hasBeenDiscovered())
			{
				return true;
			}
		}

		return false;
	}
	
	public string getExteriorSceneName()
	{
		return null;
	}

	public ArrayList getAllQuestsInLocation()
	{
		return new ArrayList();
	}
}
