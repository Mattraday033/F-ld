using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingTrait : Trait
{
    public SlowingTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int duration) :
    base(traitName, traitType, traitDescription, iconName, roundsLeft: duration, permanent: false)
    {

    }
    
    public override bool slowsTraitHolder()
    {
        return true;
    }
}
