using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishLinkAbility : Ability
{
    private LinkTrait linkTrait;

    public EstablishLinkAbility(CombatActionSettings settings, LinkTrait linkTrait, List<TargetingLogic> targetingLogics = null):
    base(settings, targetingLogics)
    {
        this.linkTrait = (LinkTrait) linkTrait.clone();
    }

    public override void performCombatAction(List<Stats> targets)
    {
        base.performCombatAction(targets);

        LinkTrait linkTraitClone = (LinkTrait) linkTrait.clone();
        linkTraitClone.setLinkedTargets(targets);

        actorStats.addTrait(linkTraitClone);
    }
}
