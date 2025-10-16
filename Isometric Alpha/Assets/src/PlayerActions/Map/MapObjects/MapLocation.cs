using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorDisplayStatRequirements
{
    public PrimaryStat[] statRequirementTypes;
    public int[] statLevelRequirements;

	public bool playerMeetsAllStatRequirements()
	{
		for(int index = 0; index < statRequirementTypes.Length && index < statLevelRequirements.Length; index++)
		{
			if (!PartyManager.getPlayerStats().meetsStatRequirements(statRequirementTypes[index], statLevelRequirements[index]))
			{
				return false;
			}
		}

		return true;
	}

	public InteriorDisplayStatRequirements(PrimaryStat statRequirementType, int statLevelRequirement)
	{
		this.statRequirementTypes = new PrimaryStat[] { statRequirementType } ;
		this.statLevelRequirements = new int[] { statLevelRequirement };
	}

	public InteriorDisplayStatRequirements(PrimaryStat[] statRequirementTypes, int[] statLevelRequirements)
	{
		this.statRequirementTypes = statRequirementTypes;
		this.statLevelRequirements = statLevelRequirements;
	}
}

[System.Serializable]
public class MapLocation : IMapObject
{

	private string displayName;
	private string zoneKey;
	public string locationName;
	
	private bool isFastTravelDestination;
	
	public int interiors { get; private set; }
	
	public string[] adjacentLocations;
	public ZoneButtonInfo[] zoneButtons;

	public InteriorDisplayStatRequirements[] interiorDisplayStatRequirements;

    public MapLocation(string zoneKey, string locationName, string displayName, bool isFastTravelDestination, int interiors, string[] adjacentLocations)
    {
        this.zoneKey = zoneKey;
        this.locationName = locationName;
        this.displayName = displayName;

        this.isFastTravelDestination = isFastTravelDestination;

        this.interiors = interiors;

        this.adjacentLocations = adjacentLocations;
        this.zoneButtons = new ZoneButtonInfo[0];
		
		this.interiorDisplayStatRequirements = new InteriorDisplayStatRequirements[0];
    }

	public MapLocation(string zoneKey, string locationName, string displayName, bool isFastTravelDestination, int interiors, string[] adjacentLocations, ZoneButtonInfo[] zoneButtons)
	{
		this.zoneKey = zoneKey;
		this.locationName = locationName;
		this.displayName = displayName;
		
		this.isFastTravelDestination = isFastTravelDestination;
		
		this.interiors = interiors;
		
		this.adjacentLocations = adjacentLocations;
		this.zoneButtons = zoneButtons;
		
		this.interiorDisplayStatRequirements = new InteriorDisplayStatRequirements[0];
	} 

	public MapLocation(string zoneKey, string locationName, string displayName, bool isFastTravelDestination, int interiors, string[] adjacentLocations, ZoneButtonInfo[] zoneButtons, InteriorDisplayStatRequirements[] interiorDisplayStatRequirements)
	{
		this.zoneKey = zoneKey;
		this.locationName = locationName;
		this.displayName = displayName;
		
		this.isFastTravelDestination = isFastTravelDestination;
		
		this.interiors = interiors;
		
		this.adjacentLocations = adjacentLocations;
		this.zoneButtons = zoneButtons;
		
		this.interiorDisplayStatRequirements = interiorDisplayStatRequirements;
	} 
	
	public bool hasBeenDiscovered()
	{
		if (State.debugDiscoverAllLocations)
		{
			return true;
		}

		return hasBeenDiscovered(zoneKey, locationName);
	}
	
	public static bool hasBeenDiscovered(string zoneKey, string locationName)
	{
		try
		{
			ArrayList listOfLocationNames = null;
			
			if (State.allKnownMapData.TryGetValue(zoneKey, out listOfLocationNames))
			{
				if(State.allKnownMapData[zoneKey].Count > 0)
				{
					foreach(string listSceneName in State.allKnownMapData[zoneKey])
					{
						if(listSceneName.Equals(locationName))
						{
							return true;
						}
					}
					
					return false;
					
				} else
				{
					return false;
				}
				
			} else
			{
				return false;
			}
		} catch(Exception e)
		{
			return false;
		}
	}
	
	public string[] getAdjacentMapObjects()
	{
		return adjacentLocations;
	}
	
	public virtual bool isVisible()
	{
		foreach(string adjacentLocation in adjacentLocations)
		{
			if(MapObjectList.getMapObject(adjacentLocation).hasBeenDiscovered())
			{
				return true;
			}
		}

		return false;
	}

	public virtual ZoneButtonInfo[] getZoneButtons()
	{
		return zoneButtons;
	}
	
	public virtual bool getIsFastTravelDestination()
	{
		// if (State.debugDiscoverAllLocations)
		// {
		// 	return true;
		// }

		return isFastTravelDestination;
	}
	
	public virtual int getInteriors()
	{
		int combinedInteriorDifference = 0;

		foreach (InteriorDisplayStatRequirements interiorDisplayStatRequirement in interiorDisplayStatRequirements)
		{
			if(!interiorDisplayStatRequirement.playerMeetsAllStatRequirements())
			{
				combinedInteriorDifference++;
			}
		}

		if(combinedInteriorDifference > interiors && interiors > 0)
		{
			Debug.LogError("combinedInteriorDifference > interiors for " + locationName);
		}

		return interiors - combinedInteriorDifference;
	}
    
    public virtual int getInteriorIndex()
	{
		return -1;
	}
	
	public bool isInterior()
    {
        if (getInteriorIndex() < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

	public virtual ArrayList getAllQuestsInLocation()
	{
		ArrayList allQuestsInLocation = QuestList.getActiveQuestsWithObjectivesInScene(locationName);

		foreach (string adjacentSceneName in adjacentLocations)
		{
			IMapObject adjacentLocation = MapObjectList.getMapObject(adjacentSceneName);

			if (adjacentLocation.isInterior())
			{
				allQuestsInLocation.AddRange(adjacentLocation.getAllQuestsInLocation());
			}
		}

		return allQuestsInLocation;
	}

	public string getLocationName()
	{
		return locationName;
	}
	
	public string getZoneKey()
	{
		return zoneKey;
	}
	
    public virtual string getExteriorSceneName()
    {
        return null;
    }

	public string getZoneMapUIDisplayName()
    {
        return MapObjectList.getMapObject(getZoneKey()).getMapUIDisplayName();
    }

	public string getMapUIDisplayName()
	{
		return displayName;
	}
	
	public string getMapUIDisplayNameWithoutZoneName()
	{
		if (displayName.Contains(getZoneMapUIDisplayName()))
		{
			return displayName.Replace(getZoneMapUIDisplayName() + " - ", "");
		} else
		{
			return displayName;
		}
	}

    public string getNotificationDisplayName()
	{
		if (getMapUIDisplayName().Contains(getZoneMapUIDisplayName()))
		{
			return getMapUIDisplayName();
		}
		else
		{
			return getZoneMapUIDisplayName() + " - " + getMapUIDisplayName();
		}
	}
}