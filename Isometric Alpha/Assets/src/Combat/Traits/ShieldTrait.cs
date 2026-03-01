using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTrait : Trait
{

    public ShieldTrait(string traitName, TraitType traitType, string traitDescription, string iconName):
    base(traitName, traitType, traitDescription, iconName)
    {
    }

    public ShieldTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int roundsLeft, bool permanent):
    base(traitName, traitType, traitDescription, iconName, roundsLeft: roundsLeft, permanent: permanent)
    {
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

    public HiddenShieldTrait(string traitName, TraitType traitType, string traitDescription, string iconName) :
    base(traitName, traitType, traitDescription, iconName)
    {
    }

    public override bool isHiddenTrait()
    {
        return true;
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
