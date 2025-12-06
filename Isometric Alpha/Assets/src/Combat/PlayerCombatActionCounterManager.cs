using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatActionCounterManager : MonoBehaviour
{

	
	public Image[] partyMemberCombatActionPanels;
	
    void Start()
    {
		setCombatActionCounterPanelsToDefault();
    }

	public void updateCombatActionCounterPanels(List<CombatAction> actionOrder)
	{
		setCombatActionCounterPanelsToDefault();

		bool deadActorFound = false;

		int panelIndex = 0;

		foreach (CombatAction action in actionOrder)
		{
			Stats combatant = CombatGrid.getCombatantAtCoords(action.getActorCoords());

			if (combatant == null || combatant.isDead())
			{
				deadActorFound = true;
			}

			if (combatant.costsPartyCombatActions())
			{
				partyMemberCombatActionPanels[panelIndex].color = ColorList.usedCombatActionSlotColor;
				panelIndex++;
			}
		}

		if (deadActorFound)
		{
			DeadCombatantManager.handleDeadCombatants();
			updateCombatActionCounterPanels(CombatActionManager.lockedInCombatActionQueue);
		}
	}
	
	public void setCombatActionCounterPanelsToDefault()
	{
		for(int panelIndex = 0; panelIndex < partyMemberCombatActionPanels.Length; panelIndex++)
		{
			if(panelIndex < PartyStats.getPartyMemberCombatActionSlots())
			{
				partyMemberCombatActionPanels[panelIndex].color = ColorList.unusedCombatActionSlotColor;
			} else
			{
				partyMemberCombatActionPanels[panelIndex].color = ColorList.dormantCombatActionSlotColor;
			}
		}
	}
	
}
