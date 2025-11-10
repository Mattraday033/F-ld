using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Area
{
	public string areaKey {get; private set;}
	public string combatBackgroundName {get; private set;}
	public int hostility {get; private set;}
	public string[] scenesInArea {get; private set;}
	public string[] areasSharingHostility {get; private set;}
	public bool alwaysAllowsFastTravel {get; private set;}
	
	public const int hostilityThreshold = 5;
	private const string combatBackgroundSuffix = "_CombatBackground";
	private const int interiorHostilityPerCombat = 1;
	private const int exteriorHostilityPerCombat = 3;
	
	public Area(string areaKey, string combatBackgroundName, string[] scenesInArea, string[] areasSharingHostility)
	{
		this.areaKey = areaKey;
		this.combatBackgroundName = combatBackgroundName;
		this.hostility = 0;
		this.scenesInArea = scenesInArea;
		this.areasSharingHostility = areasSharingHostility;
		this.alwaysAllowsFastTravel = true;
	}
	
	public Area(string areaKey, string combatBackgroundName, string[] scenesInArea, string[] areasSharingHostility, bool alwaysAllowsFastTravel)
	{
		this.areaKey = areaKey;
		this.combatBackgroundName = combatBackgroundName;
		this.hostility = 0;
		this.scenesInArea = scenesInArea;
		this.areasSharingHostility = areasSharingHostility;
		this.alwaysAllowsFastTravel = alwaysAllowsFastTravel;
	}
	
	public Area(string areaKey, string combatBackgroundName, int startingHostility, string[] scenesInArea, string[] areasSharingHostility)
	{
		this.areaKey = areaKey;
		this.combatBackgroundName = combatBackgroundName;
		this.hostility = startingHostility;
		this.scenesInArea = scenesInArea;
		this.areasSharingHostility = areasSharingHostility;
		this.alwaysAllowsFastTravel = true;
	}
	
	public void addHostility()
	{
		addHostility(true);
	}
	
	internal void addHostility(bool addToSharedHositilityAreas)
	{
		return;
		
		if (MapObjectList.getMapObject(SceneManager.GetActiveScene().name).isInterior())
		{
			hostility += interiorHostilityPerCombat;
		}
		else
		{
			hostility += exteriorHostilityPerCombat;
		}
		
		if(addToSharedHositilityAreas)
		{
			foreach(string sharedHostilityAreaKey in areasSharingHostility)
			{
				AreaList.allAreas[sharedHostilityAreaKey].addHostility(false);
			}
		}
	}
	
	//for setting hostility from save file
	public void setHostility(int newHostility)
	{
		hostility = newHostility;
	}
	
	public bool contains(string sceneName)
	{
		foreach(string sceneInArea in scenesInArea)
		{
			if(sceneInArea.Equals(sceneName))
			{
				return true;
			}
		}
		
		return false;
	}
	
	public bool isHostile()
	{
		return hostility >= hostilityThreshold;
	}
	
	public GameObject getCombatBackgroundObject()
	{
		return Resources.Load<GameObject>(combatBackgroundName + combatBackgroundSuffix);
	}
}

public static class AreaList
{
	public static Dictionary<string, Area> allAreas;
	
	private const int startsHostile = 5;
	private const bool fastTravelContingentOnHostility = false;
	
	private const string campBackgroundName = "Camp";
	private const string manseBackgroundName = "Manse";
	private const string slaveShackBackgroundName = "SlaveShack";
	
	static AreaList()
	{
		resetAreaList();
	}
	
	public static void resetAreaList()
	{
        allAreas = new Dictionary<string, Area>();


        string[] scenesInCampExterior = new string[]
        {
            LocationNameList.campNorthEast,
            LocationNameList.campCenter,
            LocationNameList.campManse,
            LocationNameList.campSouthEast,
            LocationNameList.campMineEntrance
        };

        string[] areasSharingHostilityWithCampExterior = new string[]
        {
            LocationNameList.campInterior,
            LocationNameList.manseFirstFloor,
            LocationNameList.manseSecondFloor,
            LocationNameList.pit
        };

        allAreas.Add(LocationNameList.campExterior, new Area(LocationNameList.campExterior, campBackgroundName, scenesInCampExterior, areasSharingHostilityWithCampExterior, fastTravelContingentOnHostility));

        string[] scenesInCampInterior = new string[]
        {
            LocationNameList.slaveShackOne,
            LocationNameList.slaveShackTwo,
            LocationNameList.slaveShackThree,
            LocationNameList.slaveShackFour,
            LocationNameList.slaveShackFive,
            LocationNameList.slaveShackSix,
            LocationNameList.slaveShackSeven,
            LocationNameList.slaveShackEight,
            LocationNameList.slaveShackNine,
            LocationNameList.guardHouseNorthEast,
            LocationNameList.guardHouseSouthWest,
            LocationNameList.guardHouseTopFloor,
            LocationNameList.guardShack,
            LocationNameList.messHall,
            LocationNameList.stables,
            LocationNameList.temple,
            LocationNameList.stockhouse
        };

        string[] areasSharingHostilityWithCampInterior = new string[]
        {
            LocationNameList.campExterior,
            LocationNameList.manseFirstFloor,
            LocationNameList.manseSecondFloor,
            LocationNameList.pit
        };

        allAreas.Add(LocationNameList.campInterior, new Area(LocationNameList.campInterior, slaveShackBackgroundName, scenesInCampInterior, areasSharingHostilityWithCampInterior));

        string[] scenesInMineLvl1 = new string[]
        {
            LocationNameList.mineLvl1 + LocationNameList.section1a,
            LocationNameList.mineLvl1 + LocationNameList.section1b,
            LocationNameList.mineLvl1 + LocationNameList.section1c
        };

        string[] areasSharingHostilityWithMineLvl1 = new string[]
        {

        };

        allAreas.Add(LocationNameList.mineLvl1, new Area(LocationNameList.mineLvl1, LocationNameList.mineLvl1, startsHostile, scenesInMineLvl1, areasSharingHostilityWithMineLvl1));

        string[] scenesInMineLvl2 = new string[]
        {
            LocationNameList.mineLvl2 + LocationNameList.section1a,
            LocationNameList.mineLvl2 + LocationNameList.section1b,
            LocationNameList.mineLvl2 + LocationNameList.section1c,
            LocationNameList.mineLvl2 + LocationNameList.section2a,
            LocationNameList.mineLvl2 + LocationNameList.section2b,
            LocationNameList.mineLvl2 + LocationNameList.section3a,
            LocationNameList.mineLvl2 + LocationNameList.section3b,
            LocationNameList.mineLvl2 + LocationNameList.section4,
            LocationNameList.mineLvl2 + LocationNameList.section5,
            LocationNameList.mineLvl2 + LocationNameList.section6,
            LocationNameList.mineLvl2 + LocationNameList.section7a,
            LocationNameList.mineLvl2 + LocationNameList.section7b
        };

        string[] areasSharingHostilityWithMineLvl2 = new string[]
        {

        };

        allAreas.Add(LocationNameList.mineLvl2, new Area(LocationNameList.mineLvl2, LocationNameList.mineLvl2, startsHostile, scenesInMineLvl2, areasSharingHostilityWithMineLvl2));

        string[] scenesInMineLvl3 = new string[]
        {
            LocationNameList.mineLvl3 + LocationNameList.section1a,
            LocationNameList.mineLvl3 + LocationNameList.section1b,
            LocationNameList.mineLvl3 + LocationNameList.section2a,
            LocationNameList.mineLvl3 + LocationNameList.section2b,
            LocationNameList.mineLvl3 + LocationNameList.section3a,
            LocationNameList.mineLvl3 + LocationNameList.section3b,
            LocationNameList.mineLvl3 + LocationNameList.section4a,
            LocationNameList.mineLvl3 + LocationNameList.section4b,
            LocationNameList.mineLvl3 + LocationNameList.section5,
            LocationNameList.mineLvl3 + LocationNameList.minerCamp,
            LocationNameList.mineLvl3 + LocationNameList.section6a,
            LocationNameList.mineLvl3 + LocationNameList.section7
        };

        string[] areasSharingHostilityWithMineLvl3 = new string[]
        {

        };

        allAreas.Add(LocationNameList.mineLvl3, new Area(LocationNameList.mineLvl3, LocationNameList.mineLvl3, startsHostile, scenesInMineLvl3, areasSharingHostilityWithMineLvl3));

        string[] scenesInManseFirstFloor = new string[]
        {
            LocationNameList.manseFirstFloor + LocationNameList.section1a,
            LocationNameList.manseFirstFloor + LocationNameList.section1b,
            LocationNameList.manseFirstFloor + LocationNameList.section1c,
            LocationNameList.manseFirstFloor + LocationNameList.kitchens,
            LocationNameList.manseFirstFloor + LocationNameList.section2a,
            LocationNameList.manseFirstFloor + LocationNameList.section2b,
            LocationNameList.manseFirstFloor + LocationNameList.section2c,
            LocationNameList.manseFirstFloor + LocationNameList.stairsToPit,
            LocationNameList.manseFirstFloor + LocationNameList.diningRoom,
            LocationNameList.manseFirstFloor + LocationNameList.section3a,
            LocationNameList.manseFirstFloor + LocationNameList.section3b,
            LocationNameList.manseFirstFloor + LocationNameList.section3c,
            LocationNameList.manseFirstFloor + LocationNameList.section3d,
            LocationNameList.manseFirstFloor + LocationNameList.section3e

        };

        string[] areasSharingHostilityWithManseFirstFloor = new string[]
        {
            LocationNameList.manseSecondFloor,
            LocationNameList.campExterior,
            LocationNameList.campInterior,
            LocationNameList.pit
        };

        allAreas.Add(LocationNameList.manseFirstFloor, new Area(LocationNameList.manseFirstFloor, manseBackgroundName, scenesInManseFirstFloor, areasSharingHostilityWithManseFirstFloor));

        string[] scenesInManseSecondFloor = new string[]
        {
            LocationNameList.manseSecondFloor + LocationNameList.section1a,
            LocationNameList.manseSecondFloor + LocationNameList.section1b,
            LocationNameList.manseSecondFloor + LocationNameList.section1c,
            LocationNameList.manseSecondFloor + LocationNameList.office,
            LocationNameList.manseSecondFloor + LocationNameList.section2a,
            LocationNameList.manseSecondFloor + LocationNameList.section2b,
            LocationNameList.manseSecondFloor + LocationNameList.section2c,
            LocationNameList.manseSecondFloor + LocationNameList.section2d,
            LocationNameList.manseSecondFloor + LocationNameList.section3a,
            LocationNameList.manseSecondFloor + LocationNameList.section3b,
            LocationNameList.manseSecondFloor + LocationNameList.section3c,
            LocationNameList.manseSecondFloor + LocationNameList.stockroom

        };

        string[] areasSharingHostilityWithManseSecondFloor = new string[]
        {
            LocationNameList.manseFirstFloor,
            LocationNameList.campExterior,
            LocationNameList.campInterior,
            LocationNameList.pit
        };

        allAreas.Add(LocationNameList.manseSecondFloor, new Area(LocationNameList.manseSecondFloor, manseBackgroundName, scenesInManseSecondFloor, areasSharingHostilityWithManseSecondFloor));

        string[] scenesInPit = new string[]
        {
            LocationNameList.pit + LocationNameList.section1a,
            LocationNameList.pit + LocationNameList.section1b,
            LocationNameList.pit + LocationNameList.section2a,
            LocationNameList.pit + LocationNameList.section2b,
            LocationNameList.pit + LocationNameList.section2c,
            LocationNameList.pit + LocationNameList.section2d

        };

        string[] areasSharingHostilityWithPit = new string[]
        {
            LocationNameList.campExterior,
            LocationNameList.campInterior,
            LocationNameList.manseFirstFloor,
            LocationNameList.manseSecondFloor
        };

        allAreas.Add(LocationNameList.pit, new Area(LocationNameList.pit, LocationNameList.mineLvl3, startsHostile, scenesInPit, areasSharingHostilityWithPit));

    }

	private static Area getCurrentArea()
	{
        return getArea(AreaManager.locationName);
	}

	public static GameObject getCurrentCombatBackgroundObject()
	{
		return getCurrentArea().getCombatBackgroundObject();
	}
	
	public static bool currentSceneIsHostile()
	{
		if(locationAlwaysHostile(AreaManager.locationName))
		{
			return true;
		}
		
		return getCurrentArea().isHostile();
	}

	public static int getCurrentAreaHostility()
	{
		if (locationAlwaysHostile(AreaManager.locationName))
		{
			return Area.hostilityThreshold;
		}

		return getCurrentArea().hostility;
	}
	
	public static void addHostility()
	{
		getCurrentArea().addHostility();
	}
	
	public static void setCurrentAreaToHostile()
	{
		getCurrentArea().setHostility(Area.hostilityThreshold);
	}
	
	public static void setAreaToHostile(string sceneName)
	{
		getArea(sceneName).setHostility(Area.hostilityThreshold);
	}

    public static void setAreaHostility(string sceneName, int hostility)
    {
        getArea(sceneName).setHostility(hostility);
    }

    public static void setAreaToPassive(string sceneName)
    {
        getArea(sceneName).setHostility(0);
    }

    public static bool areaIsHostile(string sceneName)
	{
		if(locationAlwaysHostile(sceneName))
		{
			return true;
		}
		
		return getArea(sceneName).isHostile();
	}
	
	public static bool areaAlwaysAllowsFastTravel(string sceneName)
	{
		return getArea(sceneName).alwaysAllowsFastTravel;
	}

    private static Area getArea(string sceneName)
	{
		foreach(KeyValuePair<string,Area> kvp in allAreas)
		{
			if(kvp.Value.contains(sceneName))
			{
				return kvp.Value;
			}
		}
		
		throw new IOException("No area contains the sceneName: " + sceneName);
	}

	public static bool areaOutsideAllowedFastTravelAreas(string sceneName)
	{
		if (Flags.getFlag(FlagNameList.mineLvl2GuardsFinishedMove) && !Flags.getFlag(FlagNameList.mineLvl3BreachSealed))
		{
			if (!sceneName.Contains(LocationNameList.mineLvl3))
			{
				return true;
			}
		}

		return false;
	}

	public static bool scenesInDifferentAreas(string firstSceneName, string secondSceneName)
	{
		Area areaOne = getArea(firstSceneName);
		Area areaTwo = getArea(secondSceneName);

		return areaOne.areaKey.Equals(areaTwo.areaKey);
	}

	private static bool locationAlwaysHostile(string sceneName)
	{
		switch(sceneName)
		{
			case LocationNameList.slaveShackSix:
			case LocationNameList.guardHouseTopFloor:
				return true;
			default:
				return false;
		}
	}
}