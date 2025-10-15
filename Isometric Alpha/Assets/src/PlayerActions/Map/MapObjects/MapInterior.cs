using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[System.Serializable]
public class MapInterior : MapLocation
{
    private string exteriorSceneName;
    private int interiorIndex;

    public MapInterior(string zoneKey, string sceneName, string displayName, int interiorIndex, string exteriorSceneName) :
    base(zoneKey, sceneName, displayName, false, MapObjectList.zeroInteriors, new string[] { exteriorSceneName })
    {
        this.exteriorSceneName = exteriorSceneName;
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

    public override string getExteriorSceneName()
    {
        return exteriorSceneName;
    }

	public override ArrayList getAllQuestsInLocation()
	{
		return QuestList.getActiveQuestsWithObjectivesInScene(sceneName);
	}

}

