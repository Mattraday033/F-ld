using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MapPopUpButton : PopUpButton
{
    public MapPopUpButton() :
    base(PopUpType.Map)
    {

    }

    public void spawnPopUp(string zoneKey)
    {
        MapPopUpWindow prebuiltWindow = PrebuiltScreenManager.getMapWindow();

        if (prebuiltWindow == null)
        {
            //no prebuilt window, so one is instantiated the way it always was
            base.spawnPopUp();

            MapPopUpWindow.getInstance().populate(zoneKey);
        }
        else
        {
            using (MapPopUpWindow.showMarker.Auto())
            {
                //the steps PopUpButton.spawnPopUp takes, with the prebuilt window shown where a new one was instantiated
                PopUpScreenBlockerManager.spawnPopUpScreenBlocker();

                prebuiltWindow.show(PopUpScreenBlockerManager.getPopUpParent());

                setPopUpWindow(prebuiltWindow);

                prebuiltWindow.setProgenitor(this);

                EscapeStack.addEscapableObject(prebuiltWindow);

                AudioManager.playChangeScreenSFX();

                prebuiltWindow.populate(zoneKey);
            }
        }

        PlayerStateManager.setCurrentActivity(CurrentActivity.InMap);
    }

    public override void spawnPopUp()
    {
        spawnPopUp(MapObjectList.getCurrentZoneKey());
    }

    public override void destroyPopUp()
    {
        MapPopUpWindow window = MapPopUpWindow.getInstance();

        if (window != null && window.isPrebuilt)
        {
            //what PopUpButton.destroyPopUp does, with the window put away where it used to be destroyed
            PrebuiltScreenManager.hideMapWindow();

            EscapeStack.removeTopObjectFromStack();

            PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
        }
        else
        {
            base.destroyPopUp();
        }

        PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);
    }
    public override GameObject getCurrentPopUpGameObject()
    {
        if (MapPopUpWindow.getInstance() != null && !(MapPopUpWindow.getInstance() is null))
        {
            return MapPopUpWindow.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
