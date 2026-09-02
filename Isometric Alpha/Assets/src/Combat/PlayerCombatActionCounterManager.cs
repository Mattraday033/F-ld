using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatActionCounterManager : MonoBehaviour
{

	
	public PlayerCombatActionCounterIcon[] partyMemberCombatActionPanels;
	
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
			if (!CombatGrid.combatantExistsAtCoords(action.getActorCoords(), out Stats combatant) 
                || combatant.isDead())
			{
				deadActorFound = true;
			}

			if (combatant.costsPartyCombatActions())
			{
				partyMemberCombatActionPanels[panelIndex].setToGreen();
				panelIndex++;
			}
		}

		if (deadActorFound)
		{
			DeadCombatantManager.handleDeadCombatants();
			updateCombatActionCounterPanels(CombatActionManager.lockedInCombatActionQueue);
		}
	}
	
    public static bool playerHasActionsLeft()
    {
        return PlayerCombatActionManager.playerCombatActionQueue.Count < PartyStats.getPartyMemberCombatActionSlots();
    }

	public void setCombatActionCounterPanelsToDefault()
	{
		for(int panelIndex = 0; panelIndex < partyMemberCombatActionPanels.Length; panelIndex++)
		{
			if(panelIndex < PartyStats.getPartyMemberCombatActionSlots())
			{
				partyMemberCombatActionPanels[panelIndex].setToRed();
			} else
			{
				partyMemberCombatActionPanels[panelIndex].setToInvisble();
			}
		}
	}
	
}
