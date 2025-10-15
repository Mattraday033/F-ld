using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatResultsPopUpButton : PopUpButton
{
	public CombatResultsPopUpButton():
	base(PopUpType.CombatResults)
	{

	}

	public override void spawnPopUp()
	{
		base.spawnPopUp();
		
		CombatResultsUI combatResultsUI = (CombatResultsUI) getPopUpWindow();
		
		combatResultsUI.displayDrops(State.enemyPackInfo);
	}

    public override GameObject getCurrentPopUpGameObject()
    {
        if (CombatResultsUI.getInstance() != null && !(CombatResultsUI.getInstance() is null))
        {
            return CombatResultsUI.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }

}
