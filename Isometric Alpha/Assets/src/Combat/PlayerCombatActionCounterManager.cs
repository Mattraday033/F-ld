using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatActionCounterManager : MonoBehaviour
{
	public static Color usedCombatActionSlotColor = Color.green;
	public static Color unusedCombatActionSlotColor = Color.red;
	public static Color dormantCombatActionSlotColor = new Color32(75,75,75,255);
	
	public Image[] partyMemberCombatActionPanels;
	
    void Start()
    {
		setCombatActionCounterPanelsToDefault();
    }

	public void updateCombatActionCounterPanels(ArrayList actionOrder)
	{
		setCombatActionCounterPanelsToDefault();

		bool deadActorFound = false;

		int panelIndex = 0;

		foreach (CombatAction action in actionOrder)
		{
			Stats combatant = CombatGrid.getCombatantAtCoords(action.getActorCoords());

			if (combatant == null || combatant.isDead)
			{
				deadActorFound = true;
			}

			if (combatant.costsPartyCombatActions())
			{
				partyMemberCombatActionPanels[panelIndex].color = usedCombatActionSlotColor;
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
				partyMemberCombatActionPanels[panelIndex].color = unusedCombatActionSlotColor;
			} else
			{
				partyMemberCombatActionPanels[panelIndex].color = dormantCombatActionSlotColor;
			}
		}
	}
	
}
