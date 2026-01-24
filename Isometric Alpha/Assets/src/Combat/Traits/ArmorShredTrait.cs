using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorShredTrait : Trait
{
    private double percentageArmorLost = 0.0;

    public ArmorShredTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int roundsLeft, double percentageArmorLost) :
    base(traitName, traitType, traitDescription, iconName, roundsLeft: roundsLeft)
    {
        this.percentageArmorLost = percentageArmorLost;
    }

    public override double getPercentageArmorLost()
    {
        return percentageArmorLost;
    }
}
