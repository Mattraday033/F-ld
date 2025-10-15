using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackAbility: RepositionEnemyAbility
{
    private static int targetCombatantIndex = 0;
    private static int landingCombatantIndex = 1;

    private double damageMultiplierPerSquareMoved;

	public KnockBackAbility(CombatActionSettings settings, double damageMultiplierPerSquareMoved): 
	base(settings)
	{
		this.damageMultiplierPerSquareMoved = damageMultiplierPerSquareMoved;
	}
	private double calculateFinalDamageMultiplier(GridCoords startCoords, GridCoords landingCoords)
	{
		return (double) (1 + ((startCoords.row - landingCoords.row) * damageMultiplierPerSquareMoved));
	}

    public override Stats getCombatantToBeMoved()
    {
        return CombatGrid.getCombatantAtCoords(getTargetCoords());
    }

    public override bool requiresTertiaryCoords()
    {
        return false;
    }

    private GridCoords determineDestinationCoords()
    {
        GridCoords landingCoords = getTargetCoords().clone();
		Stats combatantHit = null;
		
		for(landingCoords.row = landingCoords.row; landingCoords.row > 0; landingCoords.row--)
		{
			combatantHit = CombatGrid.getCombatantAtCoords(landingCoords.row-1, landingCoords.col);
			
			if(combatantHit != null)
			{
				break;
			}
		}
		
		return landingCoords;
	}

    private double getTotalDamageMultiplier()
    {
        return 1.0 + ((double)getSquaresMoved() * damageMultiplierPerSquareMoved);
    }

    private int getSquaresMoved()
    {
        return Math.Abs(getDestinationCoords().row - getTargetCoords().row);
    }

    public override int[] findFinalDamage(Stats targetCombatant, bool isCrit)
    {
        return new int[] {
                            ((int) ((double) base.findFinalDamage(targetCombatant, isCrit)[0] *  getTotalDamageMultiplier()))
                         };
    }

    public override void queueingAction()
    {
        setTertiaryCoords(determineDestinationCoords());

        base.queueingAction();
    }

    public override void performCombatAction()
    {
		ArrayList targets = new ArrayList();

		targets.Add(CombatGrid.getCombatantAtCoords(getTargetCoords()));

		if(getDestinationCoords().row > CombatGrid.rowUpperBounds) 
		{
			GridCoords secondTargetCoords = getDestinationCoords();
			secondTargetCoords.row--;

			if(CombatGrid.getCombatantAtCoords(secondTargetCoords) != null)
			{
                targets.Add(CombatGrid.getCombatantAtCoords(secondTargetCoords));
            }
        }

		performCombatAction(targets);
    }

    public override void performCombatAction(ArrayList targets)
	{
        if(targets.Count < 1)
        {
            return;
        }

        Stats combatantToBeMoved = (Stats) targets[targetCombatantIndex];
        Stats combatantLandedOn = null;

        if(targets.Count > 1)
        {
            combatantLandedOn = (Stats) targets[landingCombatantIndex];
        }


        if (combatantToBeMoved != null)
        {
            if (!inPreviewMode)
            {
                combatantToBeMoved.moveTo(getDestinationCoords());
            }
            
            applyTrait(combatantToBeMoved);
            sendProjectileAt(combatantToBeMoved.position, combatantToBeMoved, 0);
            
            if(combatantLandedOn != null)
            {
                applyTrait(combatantLandedOn);
                sendProjectileAt(combatantLandedOn.position, combatantLandedOn, 1);
            }
        }
    }

    /*

//Don't use the base class's findFinalDamage, it doesn't account for the knockback
public int[] dealDamage(GridCoords landingCoords, Stats targetCombatant, Stats landingCombatant, bool isCrit)
{
    double finalDamageMultiplier = calculateFinalDamageMultiplier(getTargetCoords(), landingCoords);
    int baseDamage = (int) (DamageCalculator.calculateFormula(getDamageFormula()) * finalDamageMultiplier);
    Stats actor = CombatGrid.getCombatantAtCoords(getActorCoords());

    baseDamage = actor.modifyOutgoingDamage(baseDamage);

    if(isCrit)
    {	
        baseDamage = (int) (baseDamage * actor.getCritDamageMultiplier());
        baseDamage += (int) ((float) targetCombatant.getTotalHealth() * actor.getDevastatingCriticalPercentage()); //will return 0f if not a devastatingCritical
    }

    if(CombatStateManager.isPlayerSurpriseRound())
    {
        baseDamage = (int) (baseDamage * actor.getSurpriseDamageMultiplier());
    }

    int[] finalDamage = new int[]{0,0};

    if(targetCombatant != null)
    {
        finalDamage[targetCombatantDamageIndex] = targetCombatant.modifyIncomingDamage(baseDamage);
        targetCombatant.modifyCurrentHealth(finalDamage[targetCombatantDamageIndex]);
    } else
    {
        return new int[]{-1,-1};
    }

    if(landingCombatant != null)
    {
        finalDamage[landingCombatantDamageIndex] = landingCombatant.modifyIncomingDamage(baseDamage);
        if (!inPreviewMode)
        {
            landingCombatant.modifyCurrentHealth(finalDamage[landingCombatantDamageIndex]);
        }
    } else
    {
        finalDamage[landingCombatantDamageIndex] = -1;
    }

    return finalDamage;
}
*/

}