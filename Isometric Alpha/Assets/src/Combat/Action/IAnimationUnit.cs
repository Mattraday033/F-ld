using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IAnimationUnit
{
    public void perform();

    public void halt();
}

public class StartAnimation : IAnimationUnit
{
    public Combatant target;
    public CharacterAnimationType animationType;

    public StartAnimation(Combatant target, CharacterAnimationType animationType)
    {
        this.target = target;
        this.animationType = animationType;

        AnimationUnitUtil.haltAllAnimationUnits.AddListener(halt);
    }

    public void perform()
    {
        target.animationManager.playAnimation(animationType);
    }

    public void halt()
    {
        target.animationManager.haltAnimation();
    }
}

public class CreateMessage : IAnimationUnit
{
    public string message;
    public Combatant target;
    private DamageNumberPopup messagePopUp;

    public CreateMessage(Combatant target, string message)
    {
        this.target = target;
        this.message = message;

        AnimationUnitUtil.haltAllAnimationUnits.AddListener(halt);
    }

    public void perform()
    {
        DamageNumberPopup.create(target.stats.positions[0], message, CombatGrid.getAveragePositionAt(target.stats.positions), 
                                DamageNumberPopup.getDirectionByTargetCoords(target.stats.positions[0]),
            AnimationTracker.getInstance().damageNumberCanvas, false, false);
    }

    public void halt()
    {
        if(messagePopUp != null && messagePopUp.gameObject != null)
        {
            GameObject.Destroy(messagePopUp.gameObject);
            messagePopUp = null;
        }
    }
}

public class CreateEffect : IAnimationUnit
{

    public EffectAnimationType animationType;
    public GridCoords targetLocation;
    private GameObject gameObject;

    public CreateEffect(EffectAnimationType animationType, GridCoords targetLocation)
    {
        this.animationType = animationType;
        this.targetLocation = targetLocation;

        AnimationUnitUtil.haltAllAnimationUnits.AddListener(halt);
    }

    public void perform()
    {
        AnimationData animationData = AnimationDataList.getAnimationData(animationType);

        if(animationData == null)
        {
            return;
        }

        gameObject = EffectAnimationManager.createEffect(targetLocation, animationType, animationData).gameObject;
    }

    public void halt()
    {
        //an effect destroys itself when it finishes, which Unity's null check already counts as null
        if(gameObject != null)
        {
            GameObject.Destroy(gameObject);
        }

        gameObject = null;
    }

}