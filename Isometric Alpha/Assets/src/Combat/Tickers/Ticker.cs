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
	
	public void tickDownEverything()
	{
		tickDownAllNonPermanentTraits(CombatGrid.getAllCombatants());
		tickDownAllCooldowns(CombatGrid.getAllNonsummonedAllyCombatants());
		
		GroundEffectManager.applyAllGroundEffectDamage();
		GroundEffectManager.removeAllFinishedGroundEffects();
		
		CombatUI.populateCombatActionPanels();
		
	}
	
	public void tickDownAllCooldowns(List<Stats> allAllies)
	{
		foreach(Stats ally in allAllies)
		{
			AbilityMenuButton[] abilityButtons = ally.getAbilityMenuManager().abilityButtons;
			
			foreach(AbilityMenuButton button in abilityButtons)
			{
				if(button.loadedCombatAction != null)
				{
					button.loadedCombatAction.tickDown();
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

        //combatant.removeTrait(trait);
	}
}
