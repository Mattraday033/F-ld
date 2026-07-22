using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;


public class CombatEscapeMenuPopUpButton : PopUpButton
{
    private CurrentActivity previousActivity;

	public CombatEscapeMenuPopUpButton():
    base(PopUpType.CombatEscapeMenu)
    {
        
    }

    public override void spawnPopUp()
	{
        base.spawnPopUp();

        previousActivity = CombatStateManager.currentActivity;

        CombatStateManager.setCurrentActivity(CurrentActivity.InEscapeMenu);

        CombatHoverTileManager.GetHoverSelector.RemoveAllListeners();
        
        SelectorManager.declareSelectors();
    }

    public override void destroyPopUp()
    {
        base.destroyPopUp();

        CombatStateManager.setCurrentActivity(previousActivity);
    }

    public override GameObject getCurrentPopUpGameObject()
    {
        if (CombatEscapeMenuPopUpWindow.getInstance() != null && !(CombatEscapeMenuPopUpWindow.getInstance() is null))
        {
            return CombatEscapeMenuPopUpWindow.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }

}