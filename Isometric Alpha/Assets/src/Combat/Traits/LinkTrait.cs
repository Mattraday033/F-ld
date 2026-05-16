using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTrait : Trait
{
    private Ability linkAction = new Ability(CombatActionSettings.build(DescriptionParams.build("Link Trait", "Link Damage")));

    private List<Stats> linkedTargets;

    private double percentageOfDamageDealt;
    private bool stuns;

    public LinkTrait(string traitName, string traitDescription, string iconName, int duration, double percentageOfDamageDealt, bool stuns = false) :
    base(traitName, TraitType.InteractionBuff, traitDescription, iconName, roundsLeft: duration, permanent: false)
    {
        this.percentageOfDamageDealt = percentageOfDamageDealt;
        this.stuns = stuns;
    }

    public void setLinkedTargets(List<Stats> targets)
    {
        linkedTargets = targets;
    }

    private int getDamageToDeal(int incomingDamage)
    {
        return (int) (((double) incomingDamage) * percentageOfDamageDealt);
    }

    public override void harmAllLinkedTargets(int incomingDamage)
    {
        if(getTraitHolder() == null)
        {
            return;
        }

        int projectileNumber = 0;

        if (getTraitHolder().positions.Count > 0)
        {
            linkAction.setActor(getTraitHolder());
        }

        foreach (Stats target in linkedTargets)
        {
            if(target != null && !target.isDead() && target.positions.Count > 0)
            {
                projectileNumber += linkAction.sendProjectileAt(target.positions[0], target, projectileNumber, getDamageToDeal(incomingDamage), false);
            }
        }
    }

    public override bool preventsCombatAction()
    {
        return stuns;
    }
}
