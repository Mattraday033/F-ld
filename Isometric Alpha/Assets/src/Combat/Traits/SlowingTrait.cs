using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingTrait : VulnerabilityTrait
{
    public SlowingTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int duration) :
    base(traitName, traitType, traitDescription, iconName, duration, 0)
    {

    }

    public SlowingTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int duration, int bonusDamageTaken) : 
        base(traitName, traitType, traitDescription, iconName, duration, bonusDamageTaken)
    {

    }

    public override bool slowsTraitHolder()
    {
        return true;
    }
}
