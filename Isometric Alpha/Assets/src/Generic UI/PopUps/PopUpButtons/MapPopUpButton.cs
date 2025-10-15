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

    public override void spawnPopUp()
    {
        base.spawnPopUp();

        MapPopUpWindow.getInstance().populate(MapObjectList.getMapObject(AreaManager.locationName).getZoneKey());

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inMap);
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
