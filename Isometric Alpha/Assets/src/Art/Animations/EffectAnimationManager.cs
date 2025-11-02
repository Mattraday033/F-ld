using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
public class EffectAnimationManager : AnimationManager, IAnimationTracker
{
    public int key;

    public int damage;
    public bool crit;
    public bool healsTarget;

    float spawnDamageNumbersTime;

    public override void setAnimations(string abilityName)
    {
        string folderPath = EffectPathList.getEffectFolderPath(abilityName);

        Debug.LogError("folderPath = " + folderPath);

        AnimationClip animationClip = Resources.Load<AnimationClip>(folderPath);

        spawnDamageNumbersTime = animationClip.length / 2f;

        Helpers.debugNullCheck("animationClip", animationClip);

        animancer.Play(createClipTransitionTheDelete(animationClip));
    }
    
    private IEnumerator spawnDamageNumbers()
    {
        float elapsedTime = 0f;

        while (elapsedTime < spawnDamageNumbersTime)
        {
            yield return null;

            elapsedTime += Time.deltaTime;
        }
        
        DamageNumberPopup.create(damage, transform.position, CombatAnimationManager.getInstance().damageNumberCanvas, crit, healsTarget);
    }

    private ClipTransition createClipTransitionTheDelete(AnimationClip clip)
    {
        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = clip;
        clipTransition.Events.OnEnd = () => destroyAnimation();

        if(damage > 0)
        {
            StartCoroutine(spawnDamageNumbers());
        }

        return clipTransition;
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public static EffectAnimationManager instantiatePrefab()
    {
        return Instantiate(Resources.Load<GameObject>(PrefabNames.effect)).GetComponent<EffectAnimationManager>();
    }

    public void destroyAnimation()
    {
        CombatAnimationManager.currentAnimations.Remove(key);

        DestroyImmediate(gameObject);

        CombatAnimationManager.checkAllAnimationsFinished();
    }

}