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
	
	public void tickDownAllCooldowns(ArrayList allAllies)
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
	
	public void tickDownAllNonPermanentTraits(ArrayList allCombatants)
	{
		foreach(Stats combatant in allCombatants)
		{
			Trait[] traits = new Trait[0];
			
			if(combatant.traits == null)
			{
				combatant.traits = new Trait[0];
				continue;
			}
			
			foreach(Trait trait in combatant.traits)
			{
				trait.tickDown();
				
				if(trait.isPermanent() || trait.getRoundsLeft() > 0)
				{
					traits = Helpers.appendArray<Trait>(traits, trait);
				}
			}
			
			combatant.traits = traits;
		}
	}
}
