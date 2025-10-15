using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameOverPopUpButton : PopUpButton
{
    public GameOverPopUpButton():
	base(PopUpType.GameOver)
	{

	}

    public override void spawnPopUp()
    {
        Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), PopUpBlocker.getPopUpParent());

        setPopUpWindow(getCurrentPopUpGameObject().GetComponent<PopUpWindow>()); 
		
		getPopUpWindow().setProgenitor(this);
    }

    public override GameObject getCurrentPopUpGameObject()
    {
        if (GameOverPopUpWindow.getInstance() != null && !(GameOverPopUpWindow.getInstance() is null))
        {
            return GameOverPopUpWindow.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
