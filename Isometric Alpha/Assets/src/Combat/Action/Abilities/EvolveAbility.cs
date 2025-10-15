using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolveAbility : Ability
{

	public CombatAction actionWhenInAttackMode {get; private set;}

	public EvolveAbility(CombatActionSettings settings, CombatAction actionWhenInAttackMode):
	base(settings)
	{
		this.actionWhenInAttackMode = actionWhenInAttackMode.clone();
	}

	public override int getRangeIndex()
	{
		if(inAttackMode())
		{
			return actionWhenInAttackMode.getRangeIndex();
		} else
		{
			return base.getRangeIndex();
		}
	}
	
	public override string getRangeTitle()
	{
		if(inAttackMode())
		{
			return Range.getRangeTitle(actionWhenInAttackMode.getRangeIndex());
		} else
		{
			return Range.getRangeTitle(base.getRangeIndex());
		}
	}
	
	public override string getName()
	{
		if(inAttackMode())
		{
			return actionWhenInAttackMode.getName();
		} else
		{
			return base.getName();
		}
	}
	
	public override string getIconName()
	{
		if(inAttackMode())
		{
			return actionWhenInAttackMode.getIconName();
		} else
		{
			return base.getIconName();
		}
	}
	
	public override string getUseDescription()
	{
		if(inAttackMode())
		{
			return actionWhenInAttackMode.getUseDescription();
		} else
		{
			return base.getUseDescription();
		}
	}

	public static bool inAttackMode()
	{
		return CombatStateManager.turnNumber % 2 == 1;
	}
	
	public override void setSelector(Selector newSelector)
	{
		base.setSelector(newSelector);
		actionWhenInAttackMode.setSelector(newSelector);
	}

	public override void setActor(Stats actor)
	{
		base.setActor(actor);
		actionWhenInAttackMode.setActor(actor);
		
		if (actor != null)
		{
			Debug.LogError("setActor to " + actor.position.ToString());
		}
	}
	
	/*public override void setTargetCoords(GridCoords newTargetCoords)
	{
		base.setTargetCoords(newTargetCoords);
		actionWhenInAttackMode.setTargetCoords(newTargetCoords);
	}
	*/
	public override void performCombatAction(ArrayList targets)
	{
		if(inAttackMode())
		{	
			actionWhenInAttackMode.performCombatAction(targets);
			return;
		}
		
		int coordIndex = 0;
		int projectileNumber = 1;
		
		foreach(Stats targetCombatant in targets)
		{
			projectileNumber += sendProjectileAt(targetCombatant.position, targetCombatant, projectileNumber, false);
			
			if(targetCombatant != null)
			{
				targetCombatant.evolve();
			}
			
			coordIndex++;
		}
	}

}
