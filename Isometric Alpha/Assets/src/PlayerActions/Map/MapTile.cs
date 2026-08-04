using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//Quaternion(-0.479284018,-0.198525921,0.327160597,0.789835572)
//Vector3(321.155304,306.360229,65.2149353)
public class MapTile : MonoBehaviour, IQuestListSource
{
    public readonly static UnityEvent<string> OnJournalEntryShownOnMap = new UnityEvent<string>();

    public QuestCounter questCounter;

    public string locationName;

    public MultiTargetButton[] multiTargetButtons;

    public IMapObject mapObject;

    public FastTravelIcon fastTravelIcon;
    public InteriorCounter interiorCounter;

    public GameObject nameTagParent;
    public TextMeshProUGUI locationLabel;
    public Image northWestSouthEastMarker;
    public Image northEastSouthWestMarker;
    public Image floorImage;
    public Image mapIcon;

    public Image playerIndicator;


    public GameObject restPointIcon;
    public GameObject shopIcon;

    public void readInFormat(MapTileFormat mapTileFormat)
    {
        setSceneAndLocationName(mapTileFormat.locationName);

        if (mapTileFormat.mapIconKey != null && mapTileFormat.mapIconKey != "")
        {
            setMapIcon(Helpers.loadSpriteFromResources(MapTileSpriteList.getSpriteFullPath(mapTileFormat.mapIconKey)), mapTileFormat.flipMapIcon);
        }

        if (mapObject != null && !mapObject.hasBeenDiscovered())
        {
            setToUndiscoveredState();
            return;
        }

        setInteriorCounter();

        setFastTravelIconColors();

        setButtonInteractability();

        if (mapTileFormat.floorImageKey != null && mapTileFormat.floorImageKey != "")
        {
            setFloorImage(Helpers.loadSpriteFromResources(MapTileSpriteList.getSpriteFullPath(mapTileFormat.floorImageKey)));
        }
        else
        {
            setFloorImage(MapPopUpWindow.getDefaultFloorImage());
        }

        if (mapTileFormat.northWestSouthEastMarker)
        {
            setMarkerToNorthWestSouthEast();
        }
        else if (mapTileFormat.northEastSouthWestMarker)
        {
            setMarkerToNorthEastSouthWest();
        }

        setPlayerIndicatorVisibility();
        checkRestPointAndShopIconVisibility();
    }

    private void checkRestPointAndShopIconVisibility()
    {
        restPointIcon.SetActive(RestAndShopMapLocationList.locationHasRestPoint(locationName));
        shopIcon.SetActive(RestAndShopMapLocationList.locationHasShop(locationName));
    }

    public void setInteriorCounter()
    {
        if (mapObject != null)
        {
            interiorCounter.setInteriorCounters(mapObject.getInteriors());
        }
        else
        {
            interiorCounter.setInteriorCounters(0);
        }
    }

    public void enterFastTravelMode()
    {
        MapPopUpWindow.setFastTravelTarget(mapObject);
        MapPopUpWindow.getInstance().fastTravelPopUpButton.spawnPopUp();
    }

    private void setPlayerIndicatorVisibility()
    {
        if (locationName.Equals(AreaManager.locationName))
        {
            PartyManager.getPlayerStats().setHeadSprite(playerIndicator);
            playerIndicator.gameObject.SetActive(true);
            return;
        }

        IMapObject currentSceneMapObject = MapObjectList.getMapObject(AreaManager.locationName);

        if (mapObject != null && currentSceneMapObject.isInterior() && currentSceneMapObject.getExteriorLocationName().Equals(mapObject.getLocationName()))
        {
            PartyManager.getPlayerStats().setHeadSprite(playerIndicator);
            playerIndicator.gameObject.SetActive(true);
            return;
        }
    }

    private void setToUndiscoveredState()
    {
        nameTagParent.SetActive(false);

        mapIcon.color = Color.black;
        floorImage.color = Color.black;

        setButtonActive(false);
    }

    private void setButtonActive(bool active)
    {
        if(multiTargetButtons == null)
        {
            return;
        }

        foreach(MultiTargetButton button in multiTargetButtons)
        {
            if (button != null)
            {
                button.enabled = active;
            }
        }
    }

    private void setButtonInteractability()
    {
        if(mapObject == null || 
           !mapObject.hasBeenDiscovered() ||
           !mapObject.getIsFastTravelDestination() ||
           AreaList.areaOutsideAllowedFastTravelAreas(mapObject.getLocationName()) ||
           (AreaList.areaIsHostile(locationName) && !AreaList.areaAlwaysAllowsFastTravel(locationName)))
        {
            setButtonActive(false);
        }
        else
        {
            setButtonActive(true);
        }
    }

    private void setSceneAndLocationName(string locationName)
    {
        if (locationName == null || locationName == "")
        {
            nameTagParent.SetActive(false);
            return;
        }

        this.locationName = locationName;
        this.mapObject = MapObjectList.getMapObject(locationName);

        nameTagParent.SetActive(true);
        locationLabel.text = mapObject.getMapUIDisplayNameWithoutZoneName();

        questCounter.setQuestListSource(this);
    }

    private void setFastTravelIconColors()
    {
        if (mapObject == null || !mapObject.getIsFastTravelDestination())
        {
            if (fastTravelIcon != null)
            {
                fastTravelIcon.disableFastTravelIcon();
            }
            return;
        }

        if (fastTravelBlocked())
        {
            fastTravelIcon.setToFastTravelNotAllowed();
            return;
        }
        else
        {
            fastTravelIcon.setToFastTravelAllowed();
        }
    }

    private bool fastTravelBlocked()
    {
        return (AreaList.areaIsHostile(locationName) && !AreaList.areaAlwaysAllowsFastTravel(locationName)) 
                || AreaList.areaOutsideAllowedFastTravelAreas(locationName);
    }

    private void setFloorImage(Sprite sprite)
    {
        floorImage.sprite = sprite;
    }

    private void setMapIcon(Sprite sprite, bool flipMapIcon)
    {
        mapIcon.gameObject.SetActive(true);
        mapIcon.sprite = sprite;
        mapIcon.color = Color.white;

        if (flipMapIcon)
        {
            Helpers.flipImageByXScale(mapIcon);
        }
    }

    private void setMarkerToNorthWestSouthEast()
    {
        northWestSouthEastMarker.gameObject.SetActive(true);
        northEastSouthWestMarker.gameObject.SetActive(false);
    }

    private void setMarkerToNorthEastSouthWest()
    {
        northWestSouthEastMarker.gameObject.SetActive(false);
        northEastSouthWestMarker.gameObject.SetActive(true);
    }

    //IQuestListSource methods
    public string getListKey()
    {
        return locationName;
    }

    public bool highlightOnHover()
    {
        return true;
    }

    public int getNumberOfQuests()
    {
        return getListOfQuestStepsForDisplay().Count;
    }

    public List<QuestStep> getListOfQuestStepsForDisplay()
    {
		IMapObject location = MapObjectList.getMapObject(getListKey());

		return location.getAllQuestStepsInLocation();
    }
}

public delegate bool RestStopAvailable();

public static class RestAndShopMapLocationList
{
    
    private readonly static Dictionary<string, List<RestStopAvailable>> restStopAvailabilityDict = new Dictionary<string, List<RestStopAvailable>>();

    public static bool locationHasShop(string locationName) // locationName will always be a MapLocation, and never a MapInterior
    {
        switch(locationName)
        {
            case LocationNameList.campNorthEast:
                return SpawnParamsList.slavesInNorthEastCamp.canSpawn(NPCNameList.uros);
            case LocationNameList.campSouthEast:
                return Flags.getFlag(FlagNameList.kendeWillSellToPlayer) && 
                        SpawnParamsList.defaultBeforeRevoltSpawnParams.canSpawn(NPCNameList.kende);
            default:
                return false;
        }
    }

    public static bool locationHasRestPoint(string locationName) // locationName will always be a MapLocation, and never a MapInterior
    {
        if(!restStopAvailabilityDict.ContainsKey(locationName))
        {
            return false;
        }

        foreach(RestStopAvailable check in restStopAvailabilityDict[locationName])
        {
            if(check())
            {
                return true;
            }
        }

        return false;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        restStopAvailabilityDict[LocationNameList.campNorthEast] = new List<RestStopAvailable>()
        {
            () => { 
                    return !Flags.getFlag(FlagNameList.directorDefeated); 
                    }
        };

        restStopAvailabilityDict[LocationNameList.slaveShackTwo] = new List<RestStopAvailable>()
        {
            () => { 
                    return !Flags.getFlag(FlagNameList.revoltStarted); 
                    }
        };

        restStopAvailabilityDict[LocationNameList.campSouthEast] = new List<RestStopAvailable>()
        {
            () => { 
                    return Flags.getFlag(FlagNameList.kastorStartedRevolt) && 
                        !Flags.getFlag(FlagNameList.convincedSlavesToHelpYou) &&
                        !Flags.getFlag(FlagNameList.directorDefeated); 
                    }
        };

        restStopAvailabilityDict[ZoneKeyList.mineLvl3 + LocationNameList.section5] = new List<RestStopAvailable>()
        {
            () => { 
                    return Flags.getFlag(FlagNameList.mineLvl3CarterAndNandorInParty) && 
                        !Flags.getFlag(FlagNameList.mineLvl3BreachSealed) && 
                        !Flags.getFlag(FlagNameList.broughtNandorToKastor);
                    }
        };

        restStopAvailabilityDict[ZoneKeyList.mineLvl3 + LocationNameList.section3b] = new List<RestStopAvailable>()
        {
            () => { 
                    return Flags.getFlag(FlagNameList.mineLvl3GuardsInParty) && 
                        !Flags.getFlag(FlagNameList.mineLvl3BreachSealed) && 
                        !Flags.getFlag(FlagNameList.mineLvl2GuardsFinishedMove);
                    }
        };

        restStopAvailabilityDict[ZoneKeyList.mineLvl2 + LocationNameList.section2a] = new List<RestStopAvailable>()
        {
            () => { 
                    return  Flags.getFlag(FlagNameList.mineLvl2GuardsFinishedMove) && 
                        !Flags.getFlag(FlagNameList.mineLvl3BreachSealed);
                  }
        };

    }

}