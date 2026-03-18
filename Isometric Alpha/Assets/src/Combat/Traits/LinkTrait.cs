using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTrait : Trait
{
    private Ability linkAction = new Ability(CombatActionSettings.build(DescriptionParams.build("Link Trait", "Link Damage")));

    private List<Stats> linkedTargets;

    private double percentageOfDamageDealt;

    public LinkTrait(string traitName, string traitDescription, string iconName, int duration, double percentageOfDamageDealt) :
    base(traitName, TraitType.InteractionBuff, traitDescription, iconName, roundsLeft: duration)
    {
        this.percentageOfDamageDealt = percentageOfDamageDealt;
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
        int projectileNumber = 0;
        linkAction.setActorCoords(getTraitHolder().position);

        foreach (Stats target in linkedTargets)
        {
            if(target != null && !target.isDead())
            {
                projectileNumber += linkAction.sendProjectileAt(target.position, target, projectileNumber, getDamageToDeal(incomingDamage), false);
            }
        }
    }
}
