using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTrait : Trait
{
    public const string linkTraitType = "Link Trait";
    private static CombatAction linkAction = new CombatAction(null, null);

    private ArrayList linkedTargets;

    private double percentageOfDamageDealt;

    public LinkTrait(string traitName, string traitDescription, string traitIconName, int duration, Color traitIconBackgroundColor, double percentageOfDamageDealt) :
    base(traitName, linkTraitType, traitDescription, traitIconName, duration, traitIconBackgroundColor)
    {
        this.percentageOfDamageDealt = percentageOfDamageDealt;
    }

    public void setLinkedTargets(ArrayList targets)
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
            if(target != null && !target.isDead)
            {
                projectileNumber += linkAction.sendProjectileAt(target.position, target, projectileNumber, getDamageToDeal(incomingDamage), false);
            }
        }
    }
}
