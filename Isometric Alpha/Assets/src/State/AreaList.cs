using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Area
{
	public string areaKey {get; private set;}
	public string combatBackgroundName {get; private set;}
	public int hostility {get; private set;}
	public string[] scenesInArea {get; private set;}
	public string[] areasSharingHostility {get; private set;}
	public bool alwaysAllowsFastTravel { get; private set; }
	public string musicPath { get; private set; }
	public FootStepType footStepType { get; private set; }
	
	public const int hostilityThreshold = 5;
	private const int interiorHostilityPerCombat = 1;
	private const int exteriorHostilityPerCombat = 2;
	
	public Area(string areaKey, string[] scenesInArea, string[] areasSharingHostility, string musicPath, string combatBackgroundName = null, int hostility = 0, bool alwaysAllowsFastTravel = true, FootStepType footStepType = FootStepType.Cave)
	{
		this.areaKey = areaKey;
        if(combatBackgroundName == null)
        {
		    this.combatBackgroundName = areaKey;
        } else
        {
		    this.combatBackgroundName = combatBackgroundName;
        }
		this.hostility = hostility;
		this.scenesInArea = scenesInArea;
		this.areasSharingHostility = areasSharingHostility;
		this.alwaysAllowsFastTravel = alwaysAllowsFastTravel;
		this.musicPath = musicPath;
        this.footStepType = footStepType;
	}
	
	public void addHostility(int hostilityToAdd = -1)
	{
		addHostility(true, hostilityToAdd);
	}
	
	internal void addHostility(bool addToSharedHositilityAreas, int hostilityToAdd = -1)
	{
        if(isHostile())
        {
            return;
        }

        if(hostilityToAdd > 0)
        {
            hostility += hostilityToAdd;
        } else if (MapObjectList.getMapObject(AreaManager.locationName).isInterior())
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
				AreaList.allAreas[sharedHostilityAreaKey].addHostility(false, hostilityToAdd);
			}
		}

        if(!TutorialFlags.getFlag(TutorialSequenceList.secondHostilityTutorialSeenFlag))
        {
            PlayerOOCStateManager.waitingOnHostilityTutorial = true;
        }

        if(isHostile())
        {
            AreaList.AreaBecameHostile.Invoke(this);
        }
	}
	
	public void setHostility(int newHostility)
	{
		hostility = newHostility;
	}
	
	public bool contains(string locationName)
	{
		foreach(string sceneInArea in scenesInArea)
		{
			if(sceneInArea.Equals(locationName))
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
        string prefabName = "";

        switch(AreaManager.locationName)
        {
            case ZoneKeyList.pit + LocationNameList.section1a:
                prefabName = ZoneKeyList.manseFirstFloor;
                break;
            default:
                prefabName = combatBackgroundName;
		        break;
        }

        return Resources.Load<GameObject>(PrefabNames.combatBackgroundFolderPath + prefabName);
	}
}

public static class AreaList
{
    public readonly static UnityEvent<Area> AreaBecameHostile = new UnityEvent<Area>();
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
            LocationNameList.campMineEntrance,
            LocationNameList.campNorthWest
        };

        string[] areasSharingHostilityWithCampExterior = new string[]
        {
            AreaNameList.lovashiCampInterior,
            ZoneKeyList.manseFirstFloor,
            ZoneKeyList.manseSecondFloor,
            ZoneKeyList.pit
        };

        allAreas.Add(AreaNameList.lovashiCampExterior, 
                    new Area(   AreaNameList.lovashiCampExterior, 
                                scenesInCampExterior, 
                                areasSharingHostilityWithCampExterior, 
                                AudioClipList.campOverworld, 
                                // alwaysAllowsFastTravel: fastTravelContingentOnHostility,
                                footStepType: FootStepType.Dirt));

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

        allAreas.Add(AreaNameList.lovashiCampInterior, new Area(AreaNameList.lovashiCampInterior, scenesInCampInterior, areasSharingHostilityWithCampInterior, AudioClipList.campInterior, footStepType: FootStepType.Dirt));

        string[] scenesInMineLvl1 = new string[]
        {
            ZoneKeyList.mineLvl1 + LocationNameList.section1a,
            ZoneKeyList.mineLvl1 + LocationNameList.section1b,
            ZoneKeyList.mineLvl1 + LocationNameList.section1c
        };

        string[] areasSharingHostilityWithMineLvl1 = new string[]
        {

        };

        allAreas.Add(ZoneKeyList.mineLvl1, new Area(ZoneKeyList.mineLvl1, scenesInMineLvl1, areasSharingHostilityWithMineLvl1, AudioClipList.caveOne, hostility: startsHostile));

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

        allAreas.Add(ZoneKeyList.mineLvl2, new Area(ZoneKeyList.mineLvl2, scenesInMineLvl2, areasSharingHostilityWithMineLvl2, AudioClipList.caveOne, hostility: startsHostile));

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

        allAreas.Add(ZoneKeyList.mineLvl3, new Area(ZoneKeyList.mineLvl3, scenesInMineLvl3, areasSharingHostilityWithMineLvl3, AudioClipList.caveTwo, hostility: startsHostile));

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

        allAreas.Add(ZoneKeyList.manseFirstFloor, new Area(ZoneKeyList.manseFirstFloor, scenesInManseFirstFloor, areasSharingHostilityWithManseFirstFloor, AudioClipList.campInterior, footStepType: FootStepType.WoodFloor));

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

        allAreas.Add(ZoneKeyList.manseSecondFloor, new Area(ZoneKeyList.manseSecondFloor, scenesInManseSecondFloor, areasSharingHostilityWithManseSecondFloor, AudioClipList.campInterior, combatBackgroundName: ZoneKeyList.manseFirstFloor, footStepType: FootStepType.WoodFloor));

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

        allAreas.Add(ZoneKeyList.pit, new Area(ZoneKeyList.pit, scenesInPit, areasSharingHostilityWithPit, AudioClipList.caveTwo, combatBackgroundName: ZoneKeyList.mineLvl3, hostility: startsHostile));

        string[] locationsInForest = new string[]
        {
            ZoneKeyList.forest

        };

        allAreas.Add(ZoneKeyList.forest, new Area(ZoneKeyList.forest, locationsInForest, new string[0], AudioClipList.caveTwo, combatBackgroundName: ZoneKeyList.mineLvl3, footStepType: FootStepType.Dirt));

    }

	public static Area getCurrentArea()
	{
        return getArea(AreaManager.locationName);
	}

	public static GameObject getCurrentCombatBackgroundObject()
	{
		return getCurrentArea().getCombatBackgroundObject();
	}
	
	public static bool currentAreaIsHostile()
	{
		if(locationAlwaysHostile(AreaManager.locationName))
		{
			return true;
		}
		
		return getCurrentArea().isHostile();
	}

    public static FootStepType getAreaFootStepType()
    {
        switch(AreaManager.locationName)
        {
            case LocationNameList.guardHouseNorthEast:
            case LocationNameList.guardHouseSouthWest:
            case LocationNameList.guardHouseTopFloor:
                return FootStepType.Cave;
            default:
                Area area = getCurrentArea();

                return area.footStepType;
        }
    }

	public static int getCurrentAreaHostility()
	{
		return getCurrentArea().hostility;
	}
	
	public static void incrementHostility()
	{
		getCurrentArea().addHostility(Constants.sizeOne);
	}

	public static void addHostility()
	{
		getCurrentArea().addHostility();
	}
	
	public static void setCurrentAreaToHostile()
	{
		getCurrentArea().setHostility(Area.hostilityThreshold);
	}
	
	public static void setAreaToHostile(string locationName)
	{
        Area area = getArea(locationName);
        
        bool areaWasHostile = area.isHostile();

		area.setHostility(Area.hostilityThreshold);

        if(area.isHostile() && area.contains(AreaManager.locationName))
        {
            NotificationManager.addHostilityAlertToNotificationQueue();
        }

        if(!areaWasHostile && area.isHostile())
        {
            AreaBecameHostile.Invoke(area);
        }
	}

	public static void setAreaToSafe(string locationName)
	{
		getArea(locationName).setHostility(Constants.sizeZero);
	}

    public static void setAreaToPassive(string locationName)
    {
        getArea(locationName).setHostility(0);
    }

    public static bool areaIsHostile(string locationName)
	{
		if(locationAlwaysHostile(locationName))
		{
			return true;
		}
		
		return getArea(locationName).isHostile();
	}
	
	public static bool areaAlwaysAllowsFastTravel(string locationName)
	{
        switch(locationName)
        {
            case LocationNameList.campNorthEast:
                return Flags.getFlag(FlagNameList.neCampOverseerKilled);
            default:
		        return getArea(locationName).alwaysAllowsFastTravel;
        }
	}

    public static Area getArea(string locationName)
	{
		foreach(KeyValuePair<string,Area> kvp in allAreas)
		{
			if(kvp.Value.contains(locationName))
			{
				return kvp.Value;
			}
		}
		
		throw new IOException("No area contains the locationName: " + locationName);
	}

    public static string getCurrentAreaMusicPath()
	{
        return getCurrentArea().musicPath;
	}

	public static bool areaOutsideAllowedFastTravelAreas(string locationName)
	{
		if (!Flags.getFlag(FlagNameList.mineLvl3GuardsInParty) &&
             Flags.getFlag(FlagNameList.mineLvl2GuardsFinishedMove) && 
             !Flags.getFlag(FlagNameList.mineLvl3BreachSealed) && 
             !Flags.getFlag(FlagNameList.mineLvl3KilledGuards))
		{
			if (!locationName.Contains(ZoneKeyList.mineLvl3))
			{
				return true;
			}
		}

		return false;
	}

	public static bool scenesInDifferentAreas(string firstlocationName, string secondlocationName)
	{
		Area areaOne = getArea(firstlocationName);
		Area areaTwo = getArea(secondlocationName);

		return areaOne.areaKey.Equals(areaTwo.areaKey);
	}

	public static bool locationAlwaysHostile(string locationName)
	{
		switch(locationName)
		{
			case LocationNameList.slaveShackSix:
			case LocationNameList.guardHouseTopFloor:
			case LocationNameList.campNorthWest:
				return true;
			default:
				return false;
		}
	}
}