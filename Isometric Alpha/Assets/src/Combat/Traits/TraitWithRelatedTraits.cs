using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitWithRelatedTraits : Trait
{
    private List<IDescribable> relatedDescribables;

    public TraitWithRelatedTraits(string traitName, 
                 TraitType traitType, 
                 List<IDescribable> relatedDescribables,
                 string traitDescription = "", 
                 string iconName = "",
                 bool immobile = false, 
                 bool pacifistic = false,
                 bool permanent = true,
                 int roundsLeft = Constants.oneRoundDuration):
    base(traitName, traitType, traitDescription, iconName, immobile, pacifistic, permanent, roundsLeft)
    {
        this.relatedDescribables = relatedDescribables;
    }


    public override List<IDescribable> getRelatedDescribables()
    {
        return relatedDescribables;
    }

}
