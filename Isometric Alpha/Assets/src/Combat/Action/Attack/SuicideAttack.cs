using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideAttack : Attack	
{
	public override bool killsCaster => true;

	public SuicideAttack(Stats actor, Weapon mainHandWeapon, Catalog<IQueuingUnit> queuingUnits = null, List<TargetingLogic> targetingLogics = null): base(actor, mainHandWeapon, queuingUnits, targetingLogics)
	{

	}
	
}
