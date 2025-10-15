using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishLinkAbility : Ability
{
    private LinkTrait linkTrait;

    public EstablishLinkAbility(CombatActionSettings settings, LinkTrait linkTrait):
    base(settings)
    {
        this.linkTrait = (LinkTrait) linkTrait.clone();
    }

    public override void performCombatAction(ArrayList targets)
    {
        base.performCombatAction(targets);

        LinkTrait linkTraitClone = (LinkTrait) linkTrait.clone();
        linkTraitClone.setLinkedTargets(targets);

        getActorStats().addTrait(linkTraitClone);
    }
}
