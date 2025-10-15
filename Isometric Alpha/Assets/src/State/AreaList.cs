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
	private static Area currentArea;
	
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
            AreaNameList.campNorthEast,
            AreaNameList.campCenter,
            AreaNameList.campManse,
            AreaNameList.campSouthEast,
            AreaNameList.campMineEntrance
        };

        string[] areasSharingHostilityWithCampExterior = new string[]
        {
            AreaNameList.campInterior,
            AreaNameList.manseFirstFloor,
            AreaNameList.manseSecondFloor,
            AreaNameList.pit
        };

        allAreas.Add(AreaNameList.campExterior, new Area(AreaNameList.campExterior, campBackgroundName, scenesInCampExterior, areasSharingHostilityWithCampExterior, fastTravelContingentOnHostility));

        string[] scenesInCampInterior = new string[]
        {
            AreaNameList.slaveShackOne,
            AreaNameList.slaveShackTwo,
            AreaNameList.slaveShackThree,
            AreaNameList.slaveShackFour,
            AreaNameList.slaveShackFive,
            AreaNameList.slaveShackSix,
            AreaNameList.slaveShackSeven,
            AreaNameList.slaveShackEight,
            AreaNameList.slaveShackNine,
            AreaNameList.guardHouseNorthEast,
            AreaNameList.guardHouseSouthWest,
            AreaNameList.guardHouseTopFloor,
            AreaNameList.guardShack,
            AreaNameList.messHall,
            AreaNameList.stables,
            AreaNameList.temple,
            AreaNameList.slaveShackTwo
        };

        string[] areasSharingHostilityWithCampInterior = new string[]
        {
            AreaNameList.campExterior,
            AreaNameList.manseFirstFloor,
            AreaNameList.manseSecondFloor,
            AreaNameList.pit
        };

        allAreas.Add(AreaNameList.campInterior, new Area(AreaNameList.campInterior, slaveShackBackgroundName, scenesInCampInterior, areasSharingHostilityWithCampInterior));

        string[] scenesInMineLvl1 = new string[]
        {
            AreaNameList.mineLvl1 + AreaNameList.section1a,
            AreaNameList.mineLvl1 + AreaNameList.section1b,
            AreaNameList.mineLvl1 + AreaNameList.section1c
        };

        string[] areasSharingHostilityWithMineLvl1 = new string[]
        {

        };

        allAreas.Add(AreaNameList.mineLvl1, new Area(AreaNameList.mineLvl1, AreaNameList.mineLvl1, startsHostile, scenesInMineLvl1, areasSharingHostilityWithMineLvl1));

        string[] scenesInMineLvl2 = new string[]
        {
            AreaNameList.mineLvl2 + AreaNameList.section1a,
            AreaNameList.mineLvl2 + AreaNameList.section1b,
            AreaNameList.mineLvl2 + AreaNameList.section1c,
            AreaNameList.mineLvl2 + AreaNameList.section2a,
            AreaNameList.mineLvl2 + AreaNameList.section2b,
            AreaNameList.mineLvl2 + AreaNameList.section3a,
            AreaNameList.mineLvl2 + AreaNameList.section3b,
            AreaNameList.mineLvl2 + AreaNameList.section4,
            AreaNameList.mineLvl2 + AreaNameList.section5a,
            AreaNameList.mineLvl2 + AreaNameList.section5b,
            AreaNameList.mineLvl2 + AreaNameList.section6,
            AreaNameList.mineLvl2 + AreaNameList.section7a,
            AreaNameList.mineLvl2 + AreaNameList.section7b
        };

        string[] areasSharingHostilityWithMineLvl2 = new string[]
        {

        };

        allAreas.Add(AreaNameList.mineLvl2, new Area(AreaNameList.mineLvl2, AreaNameList.mineLvl2, startsHostile, scenesInMineLvl2, areasSharingHostilityWithMineLvl2));

        string[] scenesInMineLvl3 = new string[]
        {
            AreaNameList.mineLvl3 + AreaNameList.section1a,
            AreaNameList.mineLvl3 + AreaNameList.section1b,
            AreaNameList.mineLvl3 + AreaNameList.section2a,
            AreaNameList.mineLvl3 + AreaNameList.section2b,
            AreaNameList.mineLvl3 + AreaNameList.section3a,
            AreaNameList.mineLvl3 + AreaNameList.section3b,
            AreaNameList.mineLvl3 + AreaNameList.section4a,
            AreaNameList.mineLvl3 + AreaNameList.section4b,
            AreaNameList.mineLvl3 + AreaNameList.section5,
            AreaNameList.mineLvl3 + AreaNameList.minerCamp,
            AreaNameList.mineLvl3 + AreaNameList.section6a,
            AreaNameList.mineLvl3 + AreaNameList.section7
        };

        string[] areasSharingHostilityWithMineLvl3 = new string[]
        {

        };

        allAreas.Add(AreaNameList.mineLvl3, new Area(AreaNameList.mineLvl3, AreaNameList.mineLvl3, startsHostile, scenesInMineLvl3, areasSharingHostilityWithMineLvl3));

        string[] scenesInManseFirstFloor = new string[]
        {
            AreaNameList.manseFirstFloor + AreaNameList.section1a,
            AreaNameList.manseFirstFloor + AreaNameList.section1b,
            AreaNameList.manseFirstFloor + AreaNameList.section1c,
            AreaNameList.manseFirstFloor + AreaNameList.kitchens,
            AreaNameList.manseFirstFloor + AreaNameList.section2a,
            AreaNameList.manseFirstFloor + AreaNameList.section2b,
            AreaNameList.manseFirstFloor + AreaNameList.section2c,
            AreaNameList.manseFirstFloor + AreaNameList.stairsToPit,
            AreaNameList.manseFirstFloor + AreaNameList.diningRoom,
            AreaNameList.manseFirstFloor + AreaNameList.section3a,
            AreaNameList.manseFirstFloor + AreaNameList.section3b,
            AreaNameList.manseFirstFloor + AreaNameList.section3c,
            AreaNameList.manseFirstFloor + AreaNameList.section3d,
            AreaNameList.manseFirstFloor + AreaNameList.section3e

        };

        string[] areasSharingHostilityWithManseFirstFloor = new string[]
        {
            AreaNameList.manseSecondFloor,
            AreaNameList.campExterior,
            AreaNameList.campInterior,
            AreaNameList.pit
        };

        allAreas.Add(AreaNameList.manseFirstFloor, new Area(AreaNameList.manseFirstFloor, manseBackgroundName, scenesInManseFirstFloor, areasSharingHostilityWithManseFirstFloor));

        string[] scenesInManseSecondFloor = new string[]
        {
            AreaNameList.manseSecondFloor + AreaNameList.section1a,
            AreaNameList.manseSecondFloor + AreaNameList.section1b,
            AreaNameList.manseSecondFloor + AreaNameList.section1c,
            AreaNameList.manseSecondFloor + AreaNameList.office,
            AreaNameList.manseSecondFloor + AreaNameList.section2a,
            AreaNameList.manseSecondFloor + AreaNameList.section2b,
            AreaNameList.manseSecondFloor + AreaNameList.section2c,
            AreaNameList.manseSecondFloor + AreaNameList.section2d,
            AreaNameList.manseSecondFloor + AreaNameList.section3a,
            AreaNameList.manseSecondFloor + AreaNameList.section3b,
            AreaNameList.manseSecondFloor + AreaNameList.section3c,
            AreaNameList.manseSecondFloor + AreaNameList.stockroom

        };

        string[] areasSharingHostilityWithManseSecondFloor = new string[]
        {
            AreaNameList.manseFirstFloor,
            AreaNameList.campExterior,
            AreaNameList.campInterior,
            AreaNameList.pit
        };

        allAreas.Add(AreaNameList.manseSecondFloor, new Area(AreaNameList.manseSecondFloor, manseBackgroundName, scenesInManseSecondFloor, areasSharingHostilityWithManseSecondFloor));

        string[] scenesInPit = new string[]
        {
            AreaNameList.pit + AreaNameList.section1a,
            AreaNameList.pit + AreaNameList.section1b,
            AreaNameList.pit + AreaNameList.section2a,
            AreaNameList.pit + AreaNameList.section2b,
            AreaNameList.pit + AreaNameList.section2c,
            AreaNameList.pit + AreaNameList.section2d

        };

        string[] areasSharingHostilityWithPit = new string[]
        {
            AreaNameList.campExterior,
            AreaNameList.campInterior,
            AreaNameList.manseFirstFloor,
            AreaNameList.manseSecondFloor
        };

        allAreas.Add(AreaNameList.pit, new Area(AreaNameList.pit, AreaNameList.mineLvl3, startsHostile, scenesInPit, areasSharingHostilityWithPit));

    }

	private static Area getCurrentArea()
	{
		if (currentArea == null)
		{
			setCurrentArea();
		}

		return currentArea;
	}


	public static void setCurrentArea()
	{
		currentArea = getArea(AreaManager.locationName);
	}
	
	public static GameObject getCurrentCombatBackgroundObject()
	{
		return getCurrentArea().getCombatBackgroundObject();
	}
	
	public static bool currentSceneIsHostile()
	{
		if(sceneAlwaysHostile(AreaManager.locationName))
		{
			return true;
		}
		
		return getCurrentArea().isHostile();
	}

	public static int getCurrentAreaHostility()
	{
		if (sceneAlwaysHostile(SceneManager.GetActiveScene().name))
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
		if(sceneAlwaysHostile(sceneName))
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
			if (!sceneName.Contains(AreaNameList.mineLvl3))
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

	private static bool sceneAlwaysHostile(string sceneName)
	{
		switch(sceneName)
		{
			case AreaNameList.slaveShackSix:
			case AreaNameList.guardHouseTopFloor:
				return true;
			default:
				return false;
		}
	}
}