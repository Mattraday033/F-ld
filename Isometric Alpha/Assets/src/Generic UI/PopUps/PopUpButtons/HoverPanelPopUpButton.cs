using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HoverPanelPopUpButton : PopUpButton
{
	private Stats currentCombatant;

	public HoverPanelPopUpButton():
	base(PopUpType.HoverPanel)
	{
		
	}

	public void spawnPopUp(Stats newCombatant)
	{
        if(currentCombatant != null && currentCombatant.Equals(newCombatant))
        {
            return;
        }

		this.currentCombatant = newCombatant;
		
		spawnPopUp();
	}

	public override void spawnPopUp()
	{
		Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), PopUpScreenBlockerManager.getPopUpParent());

		setPopUpWindow(getCurrentPopUpGameObject().GetComponent<PopUpWindow>());
		
		getPopUpWindow().setProgenitor(this);
		
		HoverPanel currentWindow = (HoverPanel) getPopUpWindow();
		
		currentWindow.populate(currentCombatant);
	}

    public override GameObject getCurrentPopUpGameObject()
    {
        if (HoverPanel.getInstance() != null && !(HoverPanel.getInstance() is null))
        {
            return HoverPanel.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }
}
