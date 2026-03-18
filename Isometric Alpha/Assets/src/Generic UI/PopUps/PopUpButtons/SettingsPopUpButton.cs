using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopUpButton : PopUpButton
{
	public SettingsPopUpButton():
	base(PopUpType.SettingsScreen)
	{
		
	}

    public override void spawnPopUp()
	{
		AudioManager.playChangeScreenSFX();

		Instantiate(Resources.Load<GameObject>(PrefabNames.settingsScreen), PopUpScreenBlockerManager.getPopUpParent(PopUpType.SettingsScreen)); 
		
		OverallUIManager.setCurrentScreenType(getCurrentPopUpGameObject().GetComponent<SettingsManager>());
		
		EscapeStack.addEscapableObject(getCurrentPopUpGameObject().GetComponent<SettingsManager>()); 
	}

    public override GameObject getCurrentPopUpGameObject()
    {
        if (SettingsManager.getInstance() != null && !(SettingsManager.getInstance() is null))
        {
            return SettingsManager.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
