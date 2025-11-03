using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTrait : Trait
{
    private double percentageDamageReduction = 0.0;

    public ShieldTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, double percentageDamageReduction):
    base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor)
    {
        this.percentageDamageReduction = percentageDamageReduction;
    }

    public ShieldTrait(string traitName, string traitType, string traitDescription, string traitIconName, int roundsLeft, Color traitIconBackgroundColor, double percentageDamageReduction):
    base(traitName, traitType, traitDescription, traitIconName, roundsLeft, traitIconBackgroundColor)
    {
        this.percentageDamageReduction = percentageDamageReduction;
    }

    public override double getPercentageDamageReduction()
    {
        return percentageDamageReduction;
    }

    public override CharacterAnimationType getAnimationOnApplication()
    {
        return CharacterAnimationType.Secondary_Idle;
    }

    public override CharacterAnimationType getAnimationOnRemoval()
    {
        return CharacterAnimationType.Idle_Front;
    }
}

public class HiddenShieldTrait : ShieldTrait
{

    public HiddenShieldTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, double percentageDamageReduction) :
    base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor, percentageDamageReduction)
    {
    }

    public override CharacterAnimationType getAnimationOnApplication()
    {
        return CharacterAnimationType.None;
    }

    public override CharacterAnimationType getAnimationOnRemoval()
    {
        return CharacterAnimationType.None;
    }
}
