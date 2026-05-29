using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyAbility : Ability
{
	public const string volleyName = "Volley";
	public const string volleyDescription = "A large group of combatants combine their attacks to bombard the enemy with many shots.";
	
	public BombardmentTargetPriorityTrait volleyTargetingPriority;
	public List<Stats> allActors;

	public bool alliedSide;

    public VolleyAbility(bool alliedSide) :
	base(CombatActionSettings.build(DescriptionParams.build(volleyName, useDescription: volleyDescription)))
	{
		this.alliedSide = alliedSide;
		findAllVolleyActorCoords(alliedSide);

		if(allActors.Count > 0)
		{
			setActor(allActors[0]);
		}

		this.volleyTargetingPriority = new BombardmentTargetPriorityTrait(generateVolleyGuaranteedHitChance(), allActors.Count);
	}

	public override void applySettings(CombatActionSettings settings)
	{
		base.applySettings(settings);

		cannotDealDamage = false;
    }

    public override Stats getActorStats()
	{
		List<Stats> deadActors = new List<Stats>();
		List<Stats> stunnedActors = new List<Stats>();

		foreach (Stats actor in allActors)
		{
			if (actor == null)
			{
				continue;
			}
			else if (actor.isDead())
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
			return stunnedActors[0];
		}
		else
		{
			return base.getActorStats();
		}
	}

    public override bool actorIsPartOfAction(Stats actor)
    {
        if(allActors == null)
        {
            return false;
        }

        return allActors.Contains(actor);
    }


    public override GridCoords getActorCoords()
    {
        return getActorStats().positions.Count > 0 ? getActorStats().positions[0] : GridCoords.getDefaultCoords();
    }

    public override void performCombatAction() 
	{
		GridCoords[] targetTileCoords = getSelector().getAllSelectorCoords();
		int coordIndex = 0;
		int projectileNumber = 1;
        List<Trait> appliedTraits = getAllAppliedTraits();
		
		foreach(Stats actor in allActors)
		{
			if(!isParticipating(actor))
			{
				continue;
			}

            actor.playAttackAnimation();

			int targetCoordsIndex = Random.Range(0,targetTileCoords.Length);
			Stats targetCombatant = CombatGrid.getCombatantAtCoords(targetTileCoords[targetCoordsIndex]);
			bool crit = false;
			int finalDamage;
			
			if(targetCombatant != null && !(targetCombatant is null) && targetCombatant.isAlive())
			{
				crit = DamageCalculator.isACrit(getCritFormula(), getName());
				finalDamage = findFinalDamage(targetCombatant, crit);
			
				targetCombatant.modifyCurrentHealth(finalDamage, healsTarget());
			} else
			{
				finalDamage = -1;
			}

            CombatAnimationManager.loadInstantEffect(actor.getVolleyAnimationType(), targetTileCoords[targetCoordsIndex], crit, finalDamage, healsTarget(), targetMustBeDead(), false);
			
            projectileNumber++;
			
            foreach(Trait trait in appliedTraits)
            {
                applyTrait(targetCombatant, trait);
            }
			
			coordIndex++;
		}
	}


	public List<Trait> getAllAppliedTraits()
	{
        List<Trait> appliedTraits = new List<Trait>();

        foreach(Stats stats in allActors)
        {
            VolleyParticipantStats participant = stats as VolleyParticipantStats;

            if(participant == null || 
                stats.isDead())
            {
                continue;
            }

            CombatAction action = participant.getCombatAction();

            if(action == null || 
                action.getAppliedTrait() == null || 
                appliedTraits.Contains(action.getAppliedTrait()))
            {
                continue;
            }

            appliedTraits.Add(action.getAppliedTrait());
        }

        return appliedTraits;
	}
	

    private bool isParticipating(Stats actor)
    {
        return actor != null && !actor.isDead() && !actor.isStunned();
    }

	public override Selector getTargetSelector()
	{		
		SelectorManager selectorManager = SelectorManager.getInstance();
		Selector selector = null;
		Stats actor = getActorStats();

		// Debug.LogError(actor.getName() + " is at position " + actor.position.ToString());

		List<Stats> listOfTargets;
		
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
            if(actorStats == null)
            {
                continue;
            }

            actorStats.setOutline();
		}
	}
	
	public override void removeHighlightFromActorSprites()
    {
		foreach(Stats actorStats in allActors)
		{
            if(actorStats == null)
            {
                continue;
            }

            actorStats.removeOutline();
		}
	}
	
	public void findAllVolleyActorCoords(bool alliedSide)
	{
		List<Stats> allSummonActors;
		
		if(alliedSide)
		{
			allSummonActors = CombatGrid.getAllAliveSummonedAllies();
		} else
		{
			allSummonActors = CombatGrid.getAllAliveSummonedEnemies();
		}
		
		allActors = new List<Stats>();
		
		foreach(VolleyParticipantStats summon in allSummonActors)
		{
			if(summon.isPartOfVolley())
			{
				allActors.Add(summon);
			}
		}

		if (allActors.Count > 0)
		{
			setActor(allActors[0]);
		}
	}

    private delegate string Average<T>(T t);

    private string getAverage(Average<CombatAction> average)
    {
        int totalDamage = 0;
        int aliveVolleyers = 0;

        foreach(Stats volleyer in allActors)
        {
            VolleyParticipantStats volleyParticipant = volleyer as VolleyParticipantStats;

            if(volleyParticipant != null && volleyer.isAlive() && !volleyer.isStunned())
            {
                totalDamage += DamageCalculator.calculateFormula(average(volleyParticipant.getCombatAction()), volleyParticipant); //.getDamageFormulaTotal()
                aliveVolleyers++;
            }
        }

        if(aliveVolleyers > 0)
        {
            return (totalDamage/aliveVolleyers) + "";
        } else
        {
            return Constants.zeroRating;
        }
    }

	public override string getDamageFormula()
	{
        return getAverage(t => t.getDamageFormula());
	}

	public override string getCritFormula()
	{
        return getAverage(t => t.getCritFormula());
	}

	public static int numberOfVolleyActors(bool alliedSide)
	{
		int volleyActors = 0;
		List<Stats> allSummonActors;

		if (alliedSide)
		{
			allSummonActors = CombatGrid.getAllAliveSummonedAllies();
		}
		else
		{
			allSummonActors = CombatGrid.getAllAliveSummonedEnemies();
		}

		foreach (AlliedSummonStats summon in allSummonActors)
		{
			if (summon.isPartOfVolley())
			{
				volleyActors++;
			}
		}

		return volleyActors;
	}
    private int generateVolleyGuaranteedHitChance()
	{
        if(allActors.Count == 0)
        {
            return 0;
        } 

        int totalVolleyAccuracy = 0;

        foreach(Stats stats in allActors)
        {
            totalVolleyAccuracy += stats.getVolleyAccuracy();
        }

		return totalVolleyAccuracy/allActors.Count;
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
			Debug.LogError("Volley Actor at coords " + string.Join(", ", actor.positions));
		}
	}

    public override bool multiActorAction()
    {
        return true;
    }
}
