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

    public override void setIdleAnimationOnApplication(AnimationManager animationManager)
    {
        animationManager.setCurrentIdle(CharacterAnimationType.Secondary_Idle);
    }

    public override void setIdleAnimationOnRemoval(AnimationManager animationManager)
    {
        animationManager.setCurrentIdle(CharacterAnimationType.Idle_Front);
    }

}

public class HiddenShieldTrait : ShieldTrait
{

    public HiddenShieldTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, double percentageDamageReduction) :
    base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor, percentageDamageReduction)
    {
    }

    public override void setIdleAnimationOnApplication(AnimationManager animationManager)
    {
        //empty on purpose
    }

    public override void setIdleAnimationOnRemoval(AnimationManager animationManager)
    {
        //empty on purpose
    }
}
