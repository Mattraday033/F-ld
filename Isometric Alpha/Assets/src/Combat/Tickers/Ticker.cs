using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Ticker : MonoBehaviour
{
	private static Ticker instance;
	
	public static Ticker getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("There exists more than one Ticker");
		}
		
		instance = this;
	}
	
	public bool tickDownEverything()
	{
		tickDownAllNonPermanentTraits(CombatGrid.getAllCombatants());
		tickDownAllCooldowns(CombatGrid.getAllNonsummonedAllyCombatants());
		
		GroundEffectManager.applyAllGroundEffectDamage();
		GroundEffectManager.removeAllFinishedGroundEffects();

        if(CombatActionManager.onDeathCombatActionQueue.Count > 0)
        {
            CombatActionManager.getInstance().resolveACombatAction();
            return true;
        } else
        {
		    CombatUI.populateCombatActionPanels();
            return false;
        }
	}
	
	public void tickDownAllCooldowns(List<Stats> allAllies)
	{
		//ticks each ally's own actions, since the one shared ability menu only holds the last actor's.
		//Only the activatable slots are ticked, matching the actions the menu can show
		foreach(Stats ally in allAllies)
		{
			if(!(ally is AllyStats))
			{
				continue;
			}

			CombatAction[] actions = ally.getActionArray().getActions();

			for(int index = 0; index < actions.Length && index < CombatActionArray.numberOfActivatablePlayerCombatActions; index++)
			{
				if(actions[index] != null)
				{
					actions[index].tickDown();
				}
			}
		}
	}
	
	public void tickDownAllNonPermanentTraits(List<Stats> allCombatants)
	{
        List<KeyValuePair<Stats, Trait>> traitsToRemove = new  List<KeyValuePair<Stats, Trait>>();

		foreach(Stats combatant in allCombatants)
		{
			foreach(Trait trait in combatant.traitContainer)
			{
                trait.tickDown();

				if(!trait.isPermanent() && trait.getRoundsLeft() <= 0)
				{
					traitsToRemove.Add(new KeyValuePair<Stats, Trait>(combatant, trait));
				} 
			}
		}

        foreach(KeyValuePair<Stats, Trait> kvp in traitsToRemove)
        {
            kvp.Key.removeTrait(kvp.Value);
        }
        
        DeadCombatantManager.getInstance().cleanUpAllDeadCombatants();
        CombatStateManager.getInstance().checkForWinOrLossStates();
	}
}
