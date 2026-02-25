using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveAbility : HealingAbility
{
    

	public ReviveAbility(CombatActionSettings settings) :
    base(settings)
    {

    }

	public override void performCombatAction(List<Stats> targets)
	{
        List<Stats> targetsExludingSelf = new List<Stats>();

        foreach(Stats target in targets)
        {
            if(target != null && !getActorStats().Equals(target) && !target.notResurrectable())
            {
                targetsExludingSelf.Add(target);
            }
        }

        targets = targetsExludingSelf;

		foreach(Stats targetCombatant in targets)
		{
			if(targetCombatant != null && getDamageFormula() != null && !targetCombatant.isAlive())
			{
				if(!inPreviewMode && actorIsAlly())
				{
					Exuberances.addExuberance(MultiStackProcType.GreenLeaf, singleExuberanceStack);
				}

				targetCombatant.bringBackFromDeath();
			}
		}
		
		base.performCombatAction(targets);
		
		ZoneOfInfluenceManager.getInstance().applyAllZOITraits();
	}
	

	public override bool targetMustBeDead()
	{
		return true;
	}
}