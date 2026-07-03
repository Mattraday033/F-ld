using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MissesArePunishedAbility : Ability
{
	public static bool currentlyHealsTarget = true;
	
	public MissesArePunishedAbility(CombatActionSettings settings):
	base(settings)
	{

	}

    public override void performCombatAction(List<Stats> targets)
    {
        bool hasTargets = false;

        foreach (Stats targetCombatant in targets)
        {
            if (targetCombatant != null && !targetCombatant.isDead())
            {
                hasTargets = true;
                break;
            }
        }

        if (!hasTargets)
        {
            currentlyHealsTarget = false;

            Stats targetCombatant = getActorStats();

            sendProjectileAt(getActorCoords(), targetCombatant, 1);
        }
        else
        {
            currentlyHealsTarget = true;

            base.performCombatAction(targets);
        }
    }

    public override bool healsTarget()
	{
		return currentlyHealsTarget;
	}

	public override string getRangeTitle()
	{
		return determineRangeIndex();
	}

	public override string getRangeIndex()
	{
		return determineRangeIndex();
	}

	private string determineRangeIndex()
	{
		if(CombatStateManager.turnNumber % 4 == 1)
		{
			return SelectorList.reverseHookOneName; 	//northwest
		} else if(CombatStateManager.turnNumber % 4 == 2)
		{
			return SelectorList.hookOneName; 			//northeast
		} else if(CombatStateManager.turnNumber % 4 == 3)
		{
			return SelectorList.reverseL_OneName; 	//southeast
		} else if(CombatStateManager.turnNumber % 4 == 0)
		{
			return SelectorList.L_OneName; 				//southwest
		} else
		{
			throw new IOException("Unexpected number : " + CombatStateManager.turnNumber);	//should never happen
		}
	}
}
