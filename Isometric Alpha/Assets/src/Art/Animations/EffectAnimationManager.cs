using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
public class EffectAnimationManager : AnimationManager
{
    public GridCoords targetCoords;

    public int damage;
    public bool crit;
    public bool healsTarget;

    float spawnDamageNumbersTime;

    public override void setAnimations(string effectType)
    {
        string folderPath = PrefabNames.abilityEffectFolderPath + effectType;

        AnimationClip animationClip = Resources.Load<AnimationClip>(folderPath);

        spawnDamageNumbersTime = animationClip.length * (3f/4f);

        animancer.Play(createClipTransitionThenDelete(animationClip));
    }
    
    private IEnumerator spawnDamageNumbers()
    {
        float elapsedTime = 0f;

        while (elapsedTime < spawnDamageNumbersTime)
        {
            yield return null;

            elapsedTime += Time.deltaTime;
        }

        DamageNumberPopup.create(targetCoords, damage, transform.position, DamageNumberPopup.getDirectionByTargetCoords(targetCoords),
                                 CombatAnimationManager.getInstance().damageNumberCanvas, crit, healsTarget);

        Stats target = CombatGrid.getCombatantAtCoords(targetCoords);

        if (target != null)
        {
            target.playAnimationOnDamage();
        }
    }

    private ClipTransition createClipTransitionThenDelete(AnimationClip clip)
    {
        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = clip;
        clipTransition.Events.OnEnd = () => removeAnimation();

        if(damage > 0)
        {
            StartCoroutine(spawnDamageNumbers());
        }

        return clipTransition;
    }

    public static EffectAnimationManager instantiatePrefab()
    {
        return Instantiate(Resources.Load<GameObject>(PrefabNames.effect)).GetComponent<EffectAnimationManager>();
    }

    public override bool spriteSetByHeartBeat()
    {
        return false;
    }

    public override void removeAnimation()
    {
        DestroyImmediate(gameObject);

        base.removeAnimation();
    }

}