using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        if (hasStatsClone(out Stats statsClone) && combatantToBeMovedExists(out Stats combatantToBeMoved) && 
            combatantToBeMoved.positions.Any(p => statsClone.positions.Contains(p)))
        {
            foreach (GridCoords cloneCoords in statsClone.positions)
            {
                CombatGrid.setCombatantAtCoords(cloneCoords, null);
            }
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
