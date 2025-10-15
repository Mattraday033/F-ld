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
    public static UnityEvent<string> OnJournalEntryShownOnMap = new UnityEvent<string>();

    public QuestCounter questCounter;

    public string locationName;

    public MultiTargetButton multiTargetButton;

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

    public void readInFormat(MapTileFormat mapTileFormat)
    {
        setSceneAndLocationName(mapTileFormat.locationName);

        if (mapTileFormat.mapIconKey != null && mapTileFormat.mapIconKey != "")
        {
            setMapIcon(Helpers.loadSpriteFromResources(mapTileFormat.mapIconKey), mapTileFormat.flipMapIcon);
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
            setFloorImage(Helpers.loadSpriteFromResources(mapTileFormat.floorImageKey));
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
            playerIndicator.gameObject.SetActive(true);
            return;
        }

        IMapObject currentSceneMapObject = MapObjectList.getMapObject(SceneManager.GetActiveScene().name);

        if (mapObject != null && currentSceneMapObject.isInterior() && currentSceneMapObject.getExteriorSceneName().Equals(mapObject.getSceneName()))
        {
            playerIndicator.gameObject.SetActive(true);
            return;
        }
    }

    private void setToUndiscoveredState()
    {
        nameTagParent.SetActive(false);

        mapIcon.color = Color.black;
        floorImage.color = Color.black;

        if (multiTargetButton != null)
        {
            multiTargetButton.enabled = false;
        }
    }

    private void setButtonInteractability()
    {

        if (mapObject != null && mapObject.hasBeenDiscovered() && mapObject.getIsFastTravelDestination() && !AreaList.areaOutsideAllowedFastTravelAreas(mapObject.getSceneName()))
        {
            multiTargetButton.enabled = true;
        }
        else
        {
            multiTargetButton.enabled = false;
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

        if ((AreaList.areaIsHostile(locationName) && !AreaList.areaAlwaysAllowsFastTravel(locationName)) || AreaList.areaOutsideAllowedFastTravelAreas(locationName))
        {
            fastTravelIcon.setToFastTravelNotAllowed();
            return;
        }
        else
        {
            fastTravelIcon.setToFastTravelAllowed();
        }
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
        return getListOfQuestsForDisplay().Count;
    }

    public ArrayList getListOfQuestsForDisplay()
    {
		IMapObject location = MapObjectList.getMapObject(getListKey());

		return location.getAllQuestsInLocation();
    }
}
