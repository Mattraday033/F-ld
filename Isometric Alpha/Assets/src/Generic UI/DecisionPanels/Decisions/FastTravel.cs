using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravel : IDecision
{
	private const string fastTravelMessageStart = "Would you like to fast travel to ";
    private const string fastTravelMessageEnd = "?";

    public IMapObject targetMapObject;

    public FastTravel(IMapObject targetMapObject)
    {
        this.targetMapObject = targetMapObject;
    }
	
	public string getMessage()
    {
        return fastTravelMessageStart + targetMapObject.getMapUIDisplayName() + fastTravelMessageEnd;
    }

    public void execute()
    {
        MapPopUpWindow.fastTravelPanelCloseButtonPress();
        PlayerObject.getMapPopUpButton().destroyPopUp();

		TransitionManager.fastTravel(targetMapObject.getLocationName());
    }
    
    public void backOut()
    {
        MapPopUpWindow.leaveFastTravelMode();
    }
}
