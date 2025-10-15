using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingTrait : VulnerabilityTrait
{
    public SlowingTrait(string traitName, string traitType, string traitDescription, string traitIconName, int duration, Color traitIconBackgroundColor) :
    base(traitName, traitType, traitDescription, traitIconName, duration, traitIconBackgroundColor, 0)
    {

    }

    public SlowingTrait(string traitName, string traitType, string traitDescription, string traitIconName, int duration, Color traitIconBackgroundColor, int bonusDamageTaken) : 
        base(traitName, traitType, traitDescription, traitIconName, duration, traitIconBackgroundColor, bonusDamageTaken)
    {

    }

    public override bool slowsTraitHolder()
    {
        return true;
    }
}
