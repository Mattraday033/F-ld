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
        base.spawnPopUp();

        MapPopUpWindow.getInstance().populate(zoneKey);

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inMap);
    }

    public override void spawnPopUp()
    {
        spawnPopUp(MapObjectList.getCurrentZoneKey());
    }

    public override void destroyPopUp()
    {
        base.destroyPopUp();

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
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
