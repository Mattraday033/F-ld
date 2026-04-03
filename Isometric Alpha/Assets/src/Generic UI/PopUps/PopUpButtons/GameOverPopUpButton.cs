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
        Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), PopUpScreenBlockerManager.getPopUpParent());

        setPopUpWindow(getCurrentPopUpGameObject().GetComponent<PopUpWindow>()); 
		
		getPopUpWindow().setProgenitor(this);

        if(CombatStateManager.inCombat)
        {
            CombatResultsUI.OnCombatResultsUICreation.Invoke();
        }
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
