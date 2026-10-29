using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveAbility : HealingAbility
{
	public override bool targetMustBeDead => true;

	public ReviveAbility(CombatActionSettings settings, List<TargetingLogic> targetingLogics = null) :
    base(settings, targetingLogics)
    {

    }

	public override void performCombatAction(List<Stats> targets)
	{
        List<Stats> targetsExludingSelf = new List<Stats>();

        foreach(Stats target in targets)
        {
            if(target != null && !actorStats.Equals(target) && !target.notResurrectable)
            {
                targetsExludingSelf.Add(target);
            }
        }

        targets = targetsExludingSelf;

		foreach(Stats targetCombatant in targets)
		{
			if(targetCombatant != null && getDamageFormula() != null && !targetCombatant.isAlive)
			{
				if(!inPreviewMode && actorIsAlly)
				{
					Exuberances.addExuberance(MultiStackProcType.GreenLeaf, singleExuberanceStack);
				}

				targetCombatant.bringBackFromDeath();
			}
		}
		
		base.performCombatAction(targets);
		
		ZoneOfInfluenceManager.getInstance().applyAllZOITraits();
	}
	
}