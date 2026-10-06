using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapPopUpButton : PopUpButton
{
    public WorldMapPopUpButton() :
    base(PopUpType.WorldMap)
    {

    }

    public override void spawnPopUp()
    {
        ScreenOpenProbe.begin(MapPopUpWindow.getInstance() == null ? "World Map" : "Map to World Map");

        if(MapPopUpWindow.getInstance() != null)
        {
            MapPopUpWindow.getInstance().popupProgenitor.destroyPopUp();

            ScreenOpenProbe.step("hide Map");
        }

        WorldMapPopUpWindow prebuiltWindow = PrebuiltScreenManager.getWorldMapWindow();

        if(prebuiltWindow == null)
        {
            ScreenOpenProbe.note("not prebuilt");

            //no prebuilt window, so one is instantiated the way it always was
            base.spawnPopUp();
        }
        else
        {
            //the steps PopUpButton.spawnPopUp takes, with the prebuilt window shown where a new one was instantiated
            PopUpScreenBlockerManager.spawnPopUpScreenBlocker();

            prebuiltWindow.show(PopUpScreenBlockerManager.getPopUpParent());

            setPopUpWindow(prebuiltWindow);

            prebuiltWindow.setProgenitor(this);

            EscapeStack.addEscapableObject(prebuiltWindow);

            AudioManager.playChangeScreenSFX();
        }

        ScreenOpenProbe.step("show");

        PlayerStateManager.setCurrentActivity(CurrentActivity.InWorldMap);

        ScreenOpenProbe.step("setCurrentActivity");

        WorldMapPopUpWindow worldMapPopUpWindow = getPopUpWindow() as WorldMapPopUpWindow;

        worldMapPopUpWindow.populate();

        ScreenOpenProbe.step("populate");
    }

    public override void destroyPopUp()
    {
        WorldMapPopUpWindow window = WorldMapPopUpWindow.getInstance();

        if(window != null && window.isPrebuilt)
        {
            //what PopUpButton.destroyPopUp does, with the window put away where it used to be destroyed
            PrebuiltScreenManager.hideWorldMapWindow();

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
        if (WorldMapPopUpWindow.getInstance() != null && !(WorldMapPopUpWindow.getInstance() is null))
        {
            return WorldMapPopUpWindow.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
