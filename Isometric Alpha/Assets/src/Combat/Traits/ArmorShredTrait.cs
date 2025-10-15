using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorShredTrait : Trait
{
    private double percentageArmorLost = 0.0;

    public ArmorShredTrait(string traitName, string traitType, string traitDescription, string traitIconName, int roundsLeft, Color traitIconBackgroundColor, double percentageArmorLost) :
    base(traitName, traitType, traitDescription, traitIconName, roundsLeft, traitIconBackgroundColor)
    {
        this.percentageArmorLost = percentageArmorLost;
    }

    public override double getPercentageArmorLost()
    {
        return percentageArmorLost;
    }
}
