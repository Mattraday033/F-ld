using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Animancer;
using Animancer.FSM;

public enum EffectAnimationType
{
    Default,
    Slash,
    Blunt,
    Pierce,
    Positive,
    Negative,
    Healing,
    BatSwarm,
    Acid,
    SmokeBomb,
    Intimidate,
    BlastingJelly,
    FrontLvlUp,
    BackLvlUp,
    TransitionIndicator,
    Gem,
    FrontSelector,
    BackSelector,    
    FrontSelector2,
    BackSelector2,
    Bubbles,
    Splash,
    Confused
}


public class EffectAnimationManager : AnimationManager
{

    public readonly static UnityEvent<EffectAnimationType> DestroyAllEffectsOfType = new UnityEvent<EffectAnimationType>();
    
    public EffectAnimationType type;

    public bool waitBeforeSFX = true;
    public bool playSFX = true;
    private const float timeToWaitBeforeSFX = .3f;

    public GridCoords targetCoords;

    public int damage;
    public bool crit;
    public bool healsTarget;

    float spawnDamageNumbersTime;

    public bool loops = false;

    public void setAnimations(EffectAnimationType effectType)
    {
        setAnimations(effectType.ToString());
    }

    public override void setAnimations(string effectType)
    {
        string folderPath = PrefabNames.abilityEffectFolderPath + effectType;

        if(Enum.TryParse(effectType, ignoreCase: true, out EffectAnimationType animationType))
        {
            type = animationType;
        }

        setSpriteRenderer(effectType);
        determineOutline();

        AnimationClip animationClip = Resources.Load<AnimationClip>(folderPath);

        spawnDamageNumbersTime = animationClip.length * (3f/4f);

        if(loops)
        {
            createClipTransitionThenLoop(animationClip);
        } else
        {
            animancer.Play(createClipTransitionThenDelete(animationClip));
        }

        if(playSFX)
        {
            if(waitBeforeSFX)
            {
                StartCoroutine(waitThenPlaySFX(effectType));
            } else
            {
                AudioManager.playEffectAnimationSFX(effectType);
            }
        }
    }

    private void setSpriteRenderer(string effectType)
    {
        switch(effectType)
        {
            case "FrontLvlUp":
                spriteRenderer.sortingLayerName = LayerAndTagManager.firstSortingLayerName;
                spriteRenderer.sortingOrder = Constants.indexOne;
                spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
                return;
            case "BackLvlUp":
                spriteRenderer.sortingLayerName = LayerAndTagManager.firstSortingLayerName;
                spriteRenderer.sortingOrder = Constants.indexOne;
                spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
                spriteRenderer.transform.localPosition = new Vector3(0f, .15f, 0f);
                return;
        }
    }

    private void determineOutline()
    {
        switch(type)
        {
            case EffectAnimationType.BatSwarm:
            case EffectAnimationType.FrontSelector:
            case EffectAnimationType.BackSelector:                
            case EffectAnimationType.FrontSelector2:
            case EffectAnimationType.BackSelector2:
            case EffectAnimationType.Bubbles:
            case EffectAnimationType.Splash:
            case EffectAnimationType.Confused:
                return;
            default:
                spriteRenderer.material = Resources.Load<Material>(PrefabNames.outlineMaterial);
                return;
        }
    }

    private IEnumerator waitThenPlaySFX(string effectType)
    {
        float timeWaited = 0f;

        while(timeWaited < timeToWaitBeforeSFX)
        {
            yield return null;

            timeWaited += Time.deltaTime;
        }

        AudioManager.playEffectAnimationSFX(effectType);
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

    private void createClipTransitionThenLoop(AnimationClip clip)
    {
        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = clip;
        clipTransition.Events.OnEnd = () => createClipTransitionThenLoop(clip);

        animancer.Play(clipTransition);
    }


    public static EffectAnimationManager instantiatePrefab(Transform parent = null)
    {
        return Instantiate(Resources.Load<GameObject>(PrefabNames.effect), parent).GetComponent<EffectAnimationManager>();
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

    private void destroyEffectOfType(EffectAnimationType type)
    {
        if(this.type == type)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        DestroyAllEffectsOfType.AddListener(destroyEffectOfType);
    }

    private void OnDisable()
    {
        DestroyAllEffectsOfType.RemoveListener(destroyEffectOfType);
    }

}