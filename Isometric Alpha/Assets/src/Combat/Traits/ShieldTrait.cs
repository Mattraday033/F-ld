using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTrait : Trait
{
    private double percentageDamageReduction = 0.0;

    public ShieldTrait(string traitName, TraitType traitType, string traitDescription, string iconName, double percentageDamageReduction):
    base(traitName, traitType, traitDescription, iconName)
    {
        this.percentageDamageReduction = percentageDamageReduction;
    }

    public ShieldTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int roundsLeft, double percentageDamageReduction):
    base(traitName, traitType, traitDescription, iconName, roundsLeft: roundsLeft)
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

    public HiddenShieldTrait(string traitName, TraitType traitType, string traitDescription, string iconName, double percentageDamageReduction) :
    base(traitName, traitType, traitDescription, iconName, percentageDamageReduction)
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
