using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu] 
public class SummonStats : EnemyStats
{
	private const string summonedTraitName = "Summoned";
	private const int testWeaponIndex = 1;
	
	//[SerializeField]
	private bool volleys;
	
	public SummonStats(GameObject combatSprite, string combatSpriteName, string name, int armor, int tHP, bool volleys): 
		base(combatSprite, combatSpriteName, name, armor, tHP, testWeaponIndex, new string[]{summonedTraitName, TraitList.chaotic.getName()})
	{
		this.volleys = volleys;
	}
	
	public SummonStats(GameObject combatSprite, string combatSpriteName, string name, int armor, int tHP, string actionKey, string targetPriorityTraitName): 
		base(combatSprite, combatSpriteName, name, armor, tHP, actionKey, new string[]{summonedTraitName, targetPriorityTraitName})
	{
		this.volleys = false;
	}
	
	public override bool isPartOfVolley()
	{
		return volleys;
	}
	
	public override bool wasSummoned()
	{
		return true;
	}
	
	public override Color getOutlineColor()
	{
		return RevealManager.canBeInteractedWith;
	}
	
	public override Trait getZoneOfInfluenceTrait()
	{
		return null;
	}
	
	public override IDescribable getHoverPanelDescribable()
	{
		if(isPartOfVolley())
		{
			return new VolleyAbility(true);
		} else
		{
			return getCombatAction();
		}
	}
	
	//IDescribable methods
	
	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);		

		DescriptionPanel.setText(panel.typeText, summonedTraitName);
	} 
}
