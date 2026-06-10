using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MapInterior : MapLocation
{
    private const string interiorKeyWord = "Interior";

    private string exteriorLocationName;
    private int interiorIndex;

    public MapInterior(string zoneKey, string locationName, string displayName, int interiorIndex, string exteriorLocationName) :
    base(zoneKey, locationName, displayName, MapObjectList.zeroInteriors, new string[] { exteriorLocationName })
    {
        this.exteriorLocationName = exteriorLocationName;
        this.interiorIndex = interiorIndex;
    }

    public override bool isVisible()
    {
        return false;
    }

    public override ZoneButtonInfo[] getZoneButtons()
    {
        return new ZoneButtonInfo[0];
    }

	public override string getBackgroundKey()
	{
        if(AreaList.getArea(locationName) != AreaList.getArea(exteriorLocationName))
        {
            return getZoneKey() + interiorKeyWord; 
        }

		return getZoneKey();
	}


    public override bool getIsFastTravelDestination()
    {
        return false;
    }

    public override int getInteriors()
    {
        return -1;
    }

    public override int getInteriorIndex()
    {
        return interiorIndex;
    }

    public override string getExteriorLocationName()
    {
        return exteriorLocationName;
    }

	public override List<QuestStep> getAllQuestStepsInLocation()
	{
		return QuestList.getActiveQuestStepsWithObjectivesInScene(locationName);
	}

}

