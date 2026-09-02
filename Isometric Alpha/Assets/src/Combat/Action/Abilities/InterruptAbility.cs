using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptAbility : Ability
{
    public TraitType traitTypeToPurge = TraitType.NoType;
    private static bool critChanceSnapShot;

    private bool shouldApplyTrait = false;

    public InterruptAbility(CombatActionSettings settings):
        base(settings)
    {

    }

    public InterruptAbility(CombatActionSettings settings, TraitType traitTypeToPurge) :
    base(settings)
    {
        this.traitTypeToPurge = traitTypeToPurge;
    }

    public override void applySettings(CombatActionSettings settings)
    {
        settings.targetParams.rangeTemplate = SelectorTemplate.Single;

        settings.appliedTrait = TraitList.countered;

        base.applySettings(settings);
    }

    public override void performCombatAction(List<Stats> targets)
    {
        takeCritSnapShot();

        shouldApplyTrait = targetHasChargeTrait();

        if (!inPreviewMode && traitTypeToPurge != TraitType.NoType)
        {
            foreach(Stats target in targets)
            {
                target.removeAllTraitsOfType(traitTypeToPurge);
            }
        }

        base.performCombatAction(targets);

        resetSnapShot();
    }

    private void takeCritSnapShot()
    {
        if (CombatGrid.combatantExistsAtCoords(getTargetCoords(), out Stats target) && target.hasTraitOfType(TraitType.Charge))
        {
            critChanceSnapShot = true;
        }
        else
        {
            critChanceSnapShot = false;
        }
    }

    private void resetSnapShot()
    {
        critChanceSnapShot = false;
        shouldApplyTrait = false;
    }

    public override string getCritFormula()
    {
        //missing preview mode check on purpose

        if(CombatStateManager.inCombat)
        {
            if (critChanceSnapShot)
            {
                // Debug.Log("Crit Chance = " + DamageCalculator.critAutoSuccessThreshold);
                return "" + DamageCalculator.critAutoSuccessThreshold;
            } else
            {
                // Debug.Log("Crit Chance = " + DamageCalculator.critAutoFailureThreshold);
                return "" + DamageCalculator.critAutoFailureThreshold;
            }
        } else
        {
            // Debug.Log("Crit Chance = " + DamageCalculator.critAutoFailureThreshold);
            return "" + DamageCalculator.critAutoFailureThreshold;
        }
    }
    public override Trait getAppliedTrait()
    {
        if (shouldApplyTrait)
        {
            return base.getAppliedTrait();
        }
        else
        {
            return null;
        }
    }

    public override List<IDescribable> getRelatedDescribables()
    {
        List<IDescribable> relatedDescribables = base.getRelatedDescribables();

        relatedDescribables.Add(base.getAppliedTrait());

        return relatedDescribables;
    }

    private bool targetHasChargeTrait()
    {
        return CombatGrid.combatantExistsAtCoords(getTargetCoords(), out Stats target) && 
                target.hasTraitOfType(TraitType.Charge);
    }
}
