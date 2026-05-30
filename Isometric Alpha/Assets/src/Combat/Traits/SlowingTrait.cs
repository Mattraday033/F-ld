using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingTrait : Trait
{
    public SlowingTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int duration, string loreDescription = "") :
    base(traitName, traitType, traitDescription, iconName, roundsLeft: duration, permanent: false, loreDescription: loreDescription)
    {

    }
    
    public override bool slowsTraitHolder()
    {
        return true;
    }
}
