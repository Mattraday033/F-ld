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
	public bool alwaysAllowsFastTravel { get; private set; }
	
	public const int hostilityThreshold = 5;
	private const int interiorHostilityPerCombat = 1;
	private const int exteriorHostilityPerCombat = 3;
	
	public Area(string areaKey, string[] scenesInArea, string[] areasSharingHostility)
	{
		this.areaKey = areaKey;
		this.combatBackgroundName = areaKey;
		this.hostility = 0;
		this.scenesInArea = scenesInArea;
		this.areasSharingHostility = areasSharingHostility;
		this.alwaysAllowsFastTravel = true;
	}
	
	public Area(string areaKey, string[] scenesInArea, string[] areasSharingHostility, bool alwaysAllowsFastTravel)
	{
		this.areaKey = areaKey;
		this.combatBackgroundName = areaKey;
		this.hostility = 0;
		this.scenesInArea = scenesInArea;
		this.areasSharingHostility = areasSharingHostility;
		this.alwaysAllowsFastTravel = alwaysAllowsFastTravel;
	}

    public Area(string areaKey, int startingHostility, string[] scenesInArea, string[] areasSharingHostility)
    {
        this.areaKey = areaKey;
        this.combatBackgroundName = areaKey;
        this.hostility = startingHostility;
        this.scenesInArea = scenesInArea;
        this.areasSharingHostility = areasSharingHostility;
        this.alwaysAllowsFastTravel = true;
    }
	
    public Area(string areaKey, string combatBackgroundName, string[] scenesInArea, string[] areasSharingHostility)
	{
		this.areaKey = areaKey;
		this.combatBackgroundName = combatBackgroundName;
		this.scenesInArea = scenesInArea;
		this.areasSharingHostility = areasSharingHostility;
		this.alwaysAllowsFastTravel = true;
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
		return Resources.Load<GameObject>(PrefabNames.combatBackgroundFolderPath + combatBackgroundName);
	}
}

public static class AreaList
{
	public static Dictionary<string, Area> allAreas;
	
	private const int startsHostile = 5;
	private const bool fastTravelContingentOnHostility = false;
	
    [RuntimeInitializeOnLoadMethod]
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
            AreaNameList.lovashiCampInterior,
            ZoneKeyList.manseFirstFloor,
            ZoneKeyList.manseSecondFloor,
            ZoneKeyList.pit
        };

        allAreas.Add(AreaNameList.lovashiCampExterior, new Area(AreaNameList.lovashiCampExterior, scenesInCampExterior, areasSharingHostilityWithCampExterior, fastTravelContingentOnHostility));

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
            AreaNameList.lovashiCampExterior,
            ZoneKeyList.manseFirstFloor,
            ZoneKeyList.manseSecondFloor,
            ZoneKeyList.pit
        };

        allAreas.Add(AreaNameList.lovashiCampInterior, new Area(AreaNameList.lovashiCampInterior, scenesInCampInterior, areasSharingHostilityWithCampInterior));

        string[] scenesInMineLvl1 = new string[]
        {
            ZoneKeyList.mineLvl1 + LocationNameList.section1a,
            ZoneKeyList.mineLvl1 + LocationNameList.section1b,
            ZoneKeyList.mineLvl1 + LocationNameList.section1c
        };

        string[] areasSharingHostilityWithMineLvl1 = new string[]
        {

        };

        allAreas.Add(ZoneKeyList.mineLvl1, new Area(ZoneKeyList.mineLvl1, startsHostile, scenesInMineLvl1, areasSharingHostilityWithMineLvl1));

        string[] scenesInMineLvl2 = new string[]
        {
            ZoneKeyList.mineLvl2 + LocationNameList.section1a,
            ZoneKeyList.mineLvl2 + LocationNameList.section1b,
            ZoneKeyList.mineLvl2 + LocationNameList.section1c,
            ZoneKeyList.mineLvl2 + LocationNameList.section2a,
            ZoneKeyList.mineLvl2 + LocationNameList.section2b,
            ZoneKeyList.mineLvl2 + LocationNameList.section3a,
            ZoneKeyList.mineLvl2 + LocationNameList.section3b,
            ZoneKeyList.mineLvl2 + LocationNameList.section4,
            ZoneKeyList.mineLvl2 + LocationNameList.section5,
            ZoneKeyList.mineLvl2 + LocationNameList.section6,
            ZoneKeyList.mineLvl2 + LocationNameList.section7a,
            ZoneKeyList.mineLvl2 + LocationNameList.section7b
        };

        string[] areasSharingHostilityWithMineLvl2 = new string[]
        {

        };

        allAreas.Add(ZoneKeyList.mineLvl2, new Area(ZoneKeyList.mineLvl2, startsHostile, scenesInMineLvl2, areasSharingHostilityWithMineLvl2));

        string[] scenesInMineLvl3 = new string[]
        {
            ZoneKeyList.mineLvl3 + LocationNameList.section1a,
            ZoneKeyList.mineLvl3 + LocationNameList.section1b,
            ZoneKeyList.mineLvl3 + LocationNameList.section2a,
            ZoneKeyList.mineLvl3 + LocationNameList.section2b,
            ZoneKeyList.mineLvl3 + LocationNameList.section3a,
            ZoneKeyList.mineLvl3 + LocationNameList.section3b,
            ZoneKeyList.mineLvl3 + LocationNameList.section4a,
            ZoneKeyList.mineLvl3 + LocationNameList.section4b,
            ZoneKeyList.mineLvl3 + LocationNameList.section5,
            ZoneKeyList.mineLvl3 + LocationNameList.minerCamp,
            ZoneKeyList.mineLvl3 + LocationNameList.section6a,
            ZoneKeyList.mineLvl3 + LocationNameList.section7
        };

        string[] areasSharingHostilityWithMineLvl3 = new string[]
        {

        };

        allAreas.Add(ZoneKeyList.mineLvl3, new Area(ZoneKeyList.mineLvl3, startsHostile, scenesInMineLvl3, areasSharingHostilityWithMineLvl3));

        string[] scenesInManseFirstFloor = new string[]
        {
            ZoneKeyList.manseFirstFloor + LocationNameList.section1a,
            ZoneKeyList.manseFirstFloor + LocationNameList.section1b,
            ZoneKeyList.manseFirstFloor + LocationNameList.section1c,
            ZoneKeyList.manseFirstFloor + LocationNameList.kitchens,
            ZoneKeyList.manseFirstFloor + LocationNameList.section2a,
            ZoneKeyList.manseFirstFloor + LocationNameList.section2b,
            ZoneKeyList.manseFirstFloor + LocationNameList.section2c,
            ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit,
            ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom,
            ZoneKeyList.manseFirstFloor + LocationNameList.section3a,
            ZoneKeyList.manseFirstFloor + LocationNameList.section3b,
            ZoneKeyList.manseFirstFloor + LocationNameList.section3c,
            ZoneKeyList.manseFirstFloor + LocationNameList.section3d,
            ZoneKeyList.manseFirstFloor + LocationNameList.section3e

        };

        string[] areasSharingHostilityWithManseFirstFloor = new string[]
        {
            ZoneKeyList.manseSecondFloor,
            AreaNameList.lovashiCampExterior,
            AreaNameList.lovashiCampInterior,
            ZoneKeyList.pit
        };

        allAreas.Add(ZoneKeyList.manseFirstFloor, new Area(ZoneKeyList.manseFirstFloor, scenesInManseFirstFloor, areasSharingHostilityWithManseFirstFloor));

        string[] scenesInManseSecondFloor = new string[]
        {
            ZoneKeyList.manseSecondFloor + LocationNameList.section1a,
            ZoneKeyList.manseSecondFloor + LocationNameList.section1b,
            ZoneKeyList.manseSecondFloor + LocationNameList.section1c,
            ZoneKeyList.manseSecondFloor + LocationNameList.office,
            ZoneKeyList.manseSecondFloor + LocationNameList.section2a,
            ZoneKeyList.manseSecondFloor + LocationNameList.section2b,
            ZoneKeyList.manseSecondFloor + LocationNameList.section2c,
            ZoneKeyList.manseSecondFloor + LocationNameList.section2d,
            ZoneKeyList.manseSecondFloor + LocationNameList.section3a,
            ZoneKeyList.manseSecondFloor + LocationNameList.section3b,
            ZoneKeyList.manseSecondFloor + LocationNameList.section3c,
            ZoneKeyList.manseSecondFloor + LocationNameList.stockroom

        };

        string[] areasSharingHostilityWithManseSecondFloor = new string[]
        {
            ZoneKeyList.manseFirstFloor,
            AreaNameList.lovashiCampExterior,
            AreaNameList.lovashiCampInterior,
            ZoneKeyList.pit
        };

        allAreas.Add(ZoneKeyList.manseSecondFloor, new Area(ZoneKeyList.manseSecondFloor, ZoneKeyList.manseFirstFloor, scenesInManseSecondFloor, areasSharingHostilityWithManseSecondFloor));

        string[] scenesInPit = new string[]
        {
            ZoneKeyList.pit + LocationNameList.section1a,
            ZoneKeyList.pit + LocationNameList.section1b,
            ZoneKeyList.pit + LocationNameList.section2a,
            ZoneKeyList.pit + LocationNameList.section2b,
            ZoneKeyList.pit + LocationNameList.section2c,
            ZoneKeyList.pit + LocationNameList.section2d

        };

        string[] areasSharingHostilityWithPit = new string[]
        {
            AreaNameList.lovashiCampExterior,
            AreaNameList.lovashiCampInterior,
            ZoneKeyList.manseFirstFloor,
            ZoneKeyList.manseSecondFloor
        };

        allAreas.Add(ZoneKeyList.pit, new Area(ZoneKeyList.pit, ZoneKeyList.mineLvl3, startsHostile, scenesInPit, areasSharingHostilityWithPit));

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
			if (!sceneName.Contains(ZoneKeyList.mineLvl3))
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