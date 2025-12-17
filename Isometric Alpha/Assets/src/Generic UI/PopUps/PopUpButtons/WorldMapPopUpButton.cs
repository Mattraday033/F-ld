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
        if(MapPopUpWindow.getInstance() != null)
        {
            MapPopUpWindow.getInstance().popupProgenitor.destroyPopUp();
        }

        base.spawnPopUp();

        WorldMapPopUpWindow worldMapPopUpWindow = getPopUpWindow() as WorldMapPopUpWindow;

        // worldMapPopUpWindow.populate(AreaManager.locationName);
    }

    public override void destroyPopUp()
    {
        base.destroyPopUp();

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
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
