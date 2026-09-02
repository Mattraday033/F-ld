using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackAbility : RepositionEnemyAbility
{
    private readonly static int targetCombatantIndex = 0;
    private readonly static int landingCombatantIndex = 1;

    private double damageMultiplierPerSquareMoved;

    public KnockBackAbility(CombatActionSettings settings, double damageMultiplierPerSquareMoved) :
    base(settings)
    {
        this.damageMultiplierPerSquareMoved = damageMultiplierPerSquareMoved;
    }

    public override bool combatantToBeMovedExists(out Stats combatant)
    {
        return CombatGrid.combatantExistsAtCoords(getTargetCoords(), out combatant);
    }

    public override bool requiresTertiaryCoords()
    {
        return false;
    }

    private GridCoords determineDestinationCoords()
    {
        GridCoords landingCoords = getTargetCoords().clone();

        for (landingCoords.row = landingCoords.row; landingCoords.row > 0; landingCoords.row--)
        {
            if (CombatGrid.combatantExistsAtCoords(landingCoords.row - 1, landingCoords.col, out Stats combatant))
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

    public override int findFinalDamage(Stats targetCombatant, bool isCrit)
    {
        return (int) ((double) base.findFinalDamage(targetCombatant, isCrit) *  getTotalDamageMultiplier());
    }

    public override void queueingAction()
    {
        setTertiaryCoords(determineDestinationCoords());

        base.queueingAction();
    }

    public override void performCombatAction()
    {
        if(!CombatGrid.combatantExistsAtCoords(getTargetCoords(), out Stats target))
        {
            return;
        }

        List<Stats> targets = new List<Stats>(new Stats[] { target });

        if (getDestinationCoords().row > CombatGrid.rowUpperBounds)
        {
            GridCoords secondTargetCoords = getDestinationCoords();
            secondTargetCoords.row--;

            if (CombatGrid.combatantExistsAtCoords(getTargetCoords(), out Stats secondaryTarget))
            {
                targets.Add(secondaryTarget);
            }
        }

        playActivationAnimation();

        performCombatAction(targets);
    }

    public override void performCombatAction(List<Stats> targets)
    {
        if (targets.Count < 1)
        {
            return;
        }

        Stats combatantToBeMoved = targets[targetCombatantIndex];
        Stats combatantLandedOn = null;

        if (targets.Count > 1)
        {
            combatantLandedOn = targets[landingCombatantIndex];
        }


        if (combatantToBeMoved != null)
        {
            GridCoords projectileTargetCoords = combatantToBeMoved.positions.Count > 0 ? combatantToBeMoved.positions[0] : GridCoords.getDefaultCoords();

            applyTrait(combatantToBeMoved);
            // sendProjectileAt(combatantToBeMoved.position, combatantToBeMoved, 0);
            sendProjectileAt(projectileTargetCoords, combatantToBeMoved, 0);

            if (combatantLandedOn != null)
            {

                applyTrait(combatantLandedOn);
                GridCoords landedOnCoords = combatantLandedOn.positions.Count > 0 ? combatantLandedOn.positions[0] : GridCoords.getDefaultCoords();
                sendProjectileAt(landedOnCoords, combatantLandedOn, 1);
            }
            // projectileCount = 0;
        }
    }

    public override ScriptOnLanding getLandingScript()
    {
        if (!inPreviewMode)
        {
            GridCoords secondTargetCoords = getDestinationCoords();
            secondTargetCoords.row--;
            return new KnockBackOnLanding(getTargetCoords(), getDestinationCoords(), secondTargetCoords);
        } else
        {
            return null;
        }
    }

    public override string getEffectAnimationType()
    {
        return EffectAnimationType.Blunt.ToString();
    }

}