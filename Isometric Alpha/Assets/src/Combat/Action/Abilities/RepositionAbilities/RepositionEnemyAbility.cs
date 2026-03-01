using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RepositionEnemyAbility : RepositionAbility
{
    public RepositionEnemyAbility(CombatActionSettings settings) :
	base(settings)
	{

	}

    public override Vector3 getTertiaryPosition()
	{
		if(tertiaryCoords.row < 0 || tertiaryCoords.col < 0)
		{
			throw new IOException("tertiary Coords never set. Are you sure that this action has a tertiary yet?");
		}
		
		return CombatGrid.getPositionAt(tertiaryCoords); 
	}

    public override IEnumerator waitForAttackAnimationToStop(AnimationManager animationManager, Stats combatantToBeMoved)
    {
        yield return base.waitForAttackAnimationToStop(animationManager, combatantToBeMoved);

        if(combatantToBeMoved != null)
        {
            combatantToBeMoved.playAnimationOnDamage();
        }
    }

    public override void unqueueingAction()
    {
        if (getStatsClone() != null && !getCombatantToBeMoved().position.Equals(getStatsClone().position))
        {
            CombatGrid.setCombatantAtCoords(getStatsClone().position, null);
        }

        setStatsClone(null);
		
		destroyPlaceHolderObject();
	}
	
	public override bool movesTarget()
	{
		return true;
	}

    public override string getEffectAnimationType()
    {
        return EffectAnimationType.Negative.ToString();
    }
}
