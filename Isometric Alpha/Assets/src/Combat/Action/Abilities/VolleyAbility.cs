using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyAbility : Ability
{
	public const string volleyName = "Volley";
	public const string volleyDescription = "A large group of combatants combine their attacks to bombard the enemy with many shots.";
	public const string alliedVolleyDamageFormula = "6+4C";
	public const string alliedVolleyCritFormula = "2+3C";
	public const string enemyVolleyDamageFormula = "10";
	public const string enemyVolleyCritFormula = "5";
	public const int volleyBaseAccuracy = 50;
	
	public BombardmentTargetPriorityTrait volleyTargetingPriority;
	public Stats[] allActors;

	public bool alliedSide;

    public VolleyAbility(bool alliedSide) :
	base(CombatActionSettings.build(DescriptionParams.build(volleyName, volleyDescription)))
	{
		this.alliedSide = alliedSide;
		findAllVolleyActorCoords(alliedSide);

		if(allActors.Length > 0)
		{
			setActor(allActors[0]);
		}

		this.volleyTargetingPriority = new BombardmentTargetPriorityTrait(generateVolleyGuaranteedHitChance(), allActors.Length);
	}

	public override void applySettings(CombatActionSettings settings)
	{
		base.applySettings(settings);

		cannotDealDamage = false;
    }

    public override Stats getActorStats()
	{
		ArrayList deadActors = new ArrayList();
		ArrayList stunnedActors = new ArrayList();

		foreach (Stats actor in allActors)
		{
			if (actor == null)
			{
				continue;
			}
			else if (actor.isDead)
			{
				deadActors.Add(actor);
			}
			else if (actor.isStunned())
			{
				stunnedActors.Add(actor);
			}
			else
			{
				return actor;
			}
		}

		//at this point all actors are either stunned, dead, or null
		if (stunnedActors.Count > 0)
		{
			return (Stats)stunnedActors[0];
		}
		else
		{
			return base.getActorStats();
		}
	}

    public override GridCoords getActorCoords()
    {
        return getActorStats().position;
    }

    public override void performCombatAction() 
	{
		GridCoords[] targetTileCoords = getSelector().getAllSelectorCoords();
		int coordIndex = 0;
		int projectileNumber = 1;
		
		foreach(Stats actor in allActors)
		{
			if(actor == null || actor.isDead || actor.isStunned())
			{
				continue;
			}

			int targetCoordsIndex = UnityEngine.Random.Range(0,targetTileCoords.Length);
			Stats targetCombatant = CombatGrid.getCombatantAtCoords(targetTileCoords[targetCoordsIndex]);
			bool crit = false;
			int finalDamage;
			
			if(targetCombatant != null && !(targetCombatant is null))
			{
				crit = DamageCalculator.isACrit(getCritFormula(), getName());
				finalDamage = findFinalDamage(targetCombatant, crit)[0];
			
				targetCombatant.modifyCurrentHealth(finalDamage, healsTarget());
			} else
			{
				finalDamage = -1;
			}

			CombatAnimationManager.getInstance().loadProjectile(actor.position, targetTileCoords[targetCoordsIndex], crit, finalDamage, (projectileNumber)*framesToWaitPerProjectile, healsTarget(), targetMustBeDead());
			projectileNumber++;
			
			applyTrait(targetCombatant);
			
			coordIndex++;
		}
		
	}
	
	public override Selector getTargetSelector()
	{		
		SelectorManager selectorManager = SelectorManager.getInstance();
		Selector selector = null;
		Stats actor = getActorStats();

		// Debug.LogError(actor.getName() + " is at position " + actor.position.ToString());

		ArrayList listOfTargets;
		
		if(actor.shouldTargetEnemy())
		{
			listOfTargets = CombatGrid.getAllAliveEnemyCombatants();
		} else
		{
			listOfTargets = CombatGrid.getAllAliveAllyCombatants();
		}
		
		selector = volleyTargetingPriority.findTargetLocation(selectorManager.selectors[getRangeIndex()].clone(), listOfTargets);
		
		return selector;
	}

	public override void highlightActorSprites()
	{
		foreach(Stats actorStats in allActors)
		{
			GameObject combatSprite = actorStats.combatSprite;
		
			if(combatSprite != null && !(combatSprite is null))
			{
				combatSprite.GetComponent<SpriteOutline>().color = actorStats.getOutlineColor();
				Helpers.updateColliderPosition(combatSprite);
			}
		}
	}
	
	public override void removeHighlightFromActorSprites()
	{
        foreach (Stats actorStats in allActors)
        {
			if(actorStats != null)
			{	
				GameObject combatSprite = actorStats.combatSprite;
				
				if(combatSprite != null && !(combatSprite is null))
				{
					combatSprite.GetComponent<SpriteOutline>().color = RevealManager.defaultWhenNotRevealed;
					Helpers.updateColliderPosition(combatSprite);
				}
			}
		}
	}
	
	public void findAllVolleyActorCoords(bool alliedSide)
	{
		ArrayList allSummonActors;
		
		if(alliedSide)
		{
			allSummonActors = CombatGrid.getAllAliveSummonedAllies();
		} else
		{
			allSummonActors = CombatGrid.getAllAliveSummonedEnemies();
		}
		
		allActors = new Stats[0];
		
		foreach(SummonStats summon in allSummonActors)
		{
			if(summon.isPartOfVolley())
			{
				allActors = Helpers.appendArray<Stats>(allActors, summon);
			}
		}

		if (allActors.Length > 0)
		{
			setActor(allActors[0]);
		}
	}

	public override string getDamageFormula()
	{
		if (alliedSide)
		{
			return alliedVolleyDamageFormula;
		}
		else
		{
			return enemyVolleyDamageFormula;
		}
	}

	public override string getCritFormula()
	{
		if (alliedSide)
		{
			return alliedVolleyCritFormula;
		}
		else
		{
			return enemyVolleyCritFormula;
		}
	}

	public static int numberOfVolleyActors(bool alliedSide)
	{
		int volleyActors = 0;
		ArrayList allSummonActors;

		if (alliedSide)
		{
			allSummonActors = CombatGrid.getAllAliveSummonedAllies();
		}
		else
		{
			allSummonActors = CombatGrid.getAllAliveSummonedEnemies();
		}

		foreach (SummonStats summon in allSummonActors)
		{
			if (summon.isPartOfVolley())
			{
				volleyActors++;
			}
		}

		return volleyActors;
	}
    private static int generateVolleyGuaranteedHitChance()
	{
		return volleyBaseAccuracy + PartyStats.getVolleyAccuracy();
	}

	public override void setActor(Stats actor)
	{
		base.setActor(actor);
		allActors[0] = actor;
		
		// if (actor != null)
		// {
		// 	Debug.LogError("setActor to " + actor.position.ToString());
		// }
	}

	public void printAllActorCoords()
	{
		foreach(Stats actor in allActors)
		{
			Debug.LogError("Volley Actor at coords " + actor.position.ToString());
		}
	}

    public override bool multiActorAction()
    {
        return true;
    }
}
