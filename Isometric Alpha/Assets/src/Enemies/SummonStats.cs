using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class SummonStats : EnemyStats
{
	private const string summonedTraitName = "Summoned";
	private const int testWeaponIndex = 1;
	
	//[SerializeField]
	private bool volleys;

	
	public SummonStats(string name, int armor, int tHP): 
		base(name, armor, tHP)
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
