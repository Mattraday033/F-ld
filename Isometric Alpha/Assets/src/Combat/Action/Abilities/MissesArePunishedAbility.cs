using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MissesArePunishedAbility : Ability
{
    private int missDamageMult = 3;

	public MissesArePunishedAbility(CombatActionSettings settings, int missDamageMult = 3):
	base(settings)
	{
        this.missDamageMult = missDamageMult;
	}

    public override void performCombatAction(List<Stats> targets)
    {
        if (!hasTargets())
        {
            Stats targetCombatant = getActorStats();

            sendProjectileAt(getActorCoords(), targetCombatant, 1);
        }
        else
        {
            base.performCombatAction(targets);
        }
    }

    public override int findFinalDamage(Stats targetCombatant, bool isCrit)
    {
        int finalDamage = base.findFinalDamage(targetCombatant, isCrit);

        if(!hasTargets())
        {
            finalDamage *= missDamageMult;
        } 
     
        return finalDamage;
    }

    private bool hasTargets()
    {
        if(selector == null)
        {
            return false;
        }

        List<Stats> targets = selector.getAllTargets();

        foreach (Stats targetCombatant in targets)
        {
            if (targetCombatant != null && !targetCombatant.isDead())
            {
                return true;
            }
        }

        return false;
    }

    public override bool healsTarget()
	{
		return hasTargets();
	}

	public override string getRangeTitle()
	{
		return determineRangeIndex().ToFriendlyString();
	}

	public override SelectorTemplate getRangeTemplate()
	{
		return determineRangeIndex();
	}

	private SelectorTemplate determineRangeIndex()
	{
		if(CombatStateManager.turnNumber % 4 == 1)
		{
			return SelectorTemplate.ReverseHookOne; 	//northwest
		} else if(CombatStateManager.turnNumber % 4 == 2)
		{
			return SelectorTemplate.HookOne; 			//northeast
		} else if(CombatStateManager.turnNumber % 4 == 3)
		{
			return SelectorTemplate.ReverseL_One; 	//southeast
		} else //CombatStateManager.turnNumber % 4 == 0
		{
			return SelectorTemplate.L_One; 				//southwest
		} 
	}
}
