using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoostTrait : Trait
{
    private const string damageBoostTraitDescription = "This creature deals extra damage when it attacks.";

    public DamageBoostTrait
                (string traitName, 
                 TraitType traitType = TraitType.Boost, 
                 string traitDescription = damageBoostTraitDescription, 
                 string iconName = "",
                 bool immobile = false, 
                 bool pacifistic = false,
                 bool permanent = true,
                 int roundsLeft = Constants.oneRoundDuration) :
            base(traitName, 
                 traitType, 
                 traitDescription, 
                 iconName,
                 immobile, 
                 pacifistic,
                 permanent,
                 roundsLeft)
    {
        
    }

}
