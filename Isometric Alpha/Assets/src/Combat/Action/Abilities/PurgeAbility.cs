using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PurgeAbility : Ability
{
    private int traitsToRemove = -1;
    private TraitType traitType = TraitType.Protection;

	public PurgeAbility(CombatActionSettings settings, TraitType traitType = TraitType.Protection, int traitsToRemove = -1):
	base(settings)
	{
        this.traitsToRemove = traitsToRemove;
        this.traitType = traitType;
	}

    public override void performCombatAction(List<Stats> targets)
    {
        foreach(Stats target in targets)
        {
            if(traitsToRemove <= 0)
            {
                target.removeAllTraitsOfType(traitType);
            } else
            {
                for(int traitsRemoved = 0; traitsRemoved < traitsToRemove; traitsRemoved++)
                {
                    target.removeFirstTraitOfType(traitType);
                }
            }

            sendProjectileAt(getActorCoords(), target, 1);

        }
    }

    public override string getEffectAnimationType()
    {
        return EffectAnimationType.Negative.ToString();
    }

}
