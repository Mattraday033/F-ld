using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class HoverPanelPopUpButton : PopUpButton
{
    public readonly static UnityEvent HoverPriorityRequest = new UnityEvent();

    public static Stats currentCombatantWithPriority;

	public HoverPanelPopUpButton():
	base(PopUpType.HoverPanel)
	{
		
	}

    private Stats findCurrentCombatant()
    {
        Stats currentCombatant = null;
        HoverPriorityRequest.Invoke();

        if(currentCombatantWithPriority != null)
        {
            currentCombatant = currentCombatantWithPriority;
            currentCombatantWithPriority = null;
            return currentCombatant;
        }

        if(CombatGrid.combatantExistsAtCoords(SelectorManager.getCurrentSelector().getCoords(), out currentCombatant) && currentCombatant.isRepositionClone())
        {
            return CombatGrid.findOriginalCombatant(currentCombatant);
        }

        return currentCombatant;
    }

	public override void spawnPopUp()
	{
        Stats currentCombatant = findCurrentCombatant();

        if(currentCombatant == null)
        {
            destroyPopUp();
            return;
        }

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

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateHoverPanelPopUpButton()
    {
        currentCombatantWithPriority = null;
    }
}
