using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// using Animancer;
// using Animancer.FSM;

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


public struct DamagePacket
{
    public int damage;
    public bool crit;
    public bool healsTarget;

    public DamagePacket(int damage, bool crit = false, bool healsTarget = false)
    {
        this.damage = damage;
        this.crit = crit;
        this.healsTarget = healsTarget;
    }
}

public class AnimationData
{
    private Dictionary<SpriteLayer, Sprite[]> sprites;
    private float[] timingInSeconds;

    private SFXType? sfx;
    private int sfxIndex;

    public AnimationData(Dictionary<SpriteLayer, Sprite[]> sprites, float[] timingInSeconds, SFXType? sfx = null, int sfxIndex = 0)
    {
        this.sprites = sprites;
        this.timingInSeconds = timingInSeconds;
        this.sfx = sfx;
        this.sfxIndex = sfxIndex;
    }

    public IEnumerator animationCoroutine(SpriteLayerRendererList rendererList, EffectAnimationManager effectAnimationManager, bool loopAnimation = false)
    {
        if(rendererList == null)
        {
            GameObject.Destroy(effectAnimationManager.gameObject);
            yield break;
        }

        int index = 0;
        float wait = 0f;
        bool moveToNextSprite = true;

        while(true)
        {
            if(moveToNextSprite)
            {
                foreach(KeyValuePair<SpriteLayer, Sprite[]> kvp in sprites)
                {
                    rendererList[kvp.Key].sprite = kvp.Value[index];
                }
            }

            yield return null;

            wait += Time.deltaTime;

            if(wait >= timingInSeconds[index])
            {
                index++;
                moveToNextSprite = true;

                if(index == Constants.indexOne)
                {

                }

                if(index >= timingInSeconds.Length && !loopAnimation)
                {
                    GameObject.Destroy(effectAnimationManager.gameObject);
                    yield break;
                } else if(index >= timingInSeconds.Length)
                {
                    index = 0;
                }

                if(sfx.HasValue && index == sfxIndex)
                {
                    AudioManager.playAudioClipAsSingleton(sfx.Value);
                }

                wait = 0f;
            }
        }
    }
}

public class EffectAnimationManager : MonoBehaviour
{

    public readonly static UnityEvent<EffectAnimationType> DestroyAllEffectsOfType = new UnityEvent<EffectAnimationType>();
    
    public EffectAnimationType effectType;

    public GridCoords targetCoords = default;

    public DamagePacket? damagePacket;

    public bool loops = false;
    private bool started = false;

    private AnimationData _AnimationData;
    public AnimationData animationData
    {
        set
        {
            _AnimationData = value;
        }
    }

    public SpriteLayerRendererList rendererList;

    private bool initiated = false;

    public void Awake()
    {
        if(initiated)
        {
            return;
        }

        rendererList = rendererList ?? GetComponent<SpriteLayerRendererList>();
        rendererList.Awake();

        initiated = true;
    }

    public static EffectAnimationManager createEffect(GridCoords coords,
                                                        AnimationData animationData,
                                                        bool loops = false,
                                                        DamagePacket? damagePacket = null)
    {
        GameObject creature = Instantiate(Resources.Load<GameObject>(PrefabNames.creaturePrefab), CombatGrid.getPositionAt(coords), Quaternion.identity);

        EffectAnimationManager effect = creature.AddComponent<EffectAnimationManager>();
        effect.targetCoords = coords;
        effect.animationData = animationData;
        effect.loops = loops;

        if(damagePacket.HasValue)
        {
            effect.damagePacket = damagePacket.Value;
        }

        effect.StartCoroutine(animationData.animationCoroutine(effect.rendererList, effect, loops));

        return effect;
    }

    public void startAnimation()
    {
        if(_AnimationData != null && !started)
        {
            started = true;
            StartCoroutine(_AnimationData.animationCoroutine(rendererList, this, loopAnimation: loops));
        }
    }

    public void createDamagePopUp()
    {
        if(damagePacket.HasValue && damagePacket.Value.damage > 0)
        {
            DamageNumberPopup.create(targetCoords, damagePacket.Value.damage, transform.position, DamageNumberPopup.getDirectionByTargetCoords(targetCoords),
                CombatAnimationManager.getInstance().damageNumberCanvas, damagePacket.Value.crit, damagePacket.Value.healsTarget);
        }
    }

    // private ClipTransition createClipTransitionThenDelete(AnimationClip clip)
    // {
    //     ClipTransition clipTransition = new ClipTransition();
    //     clipTransition.Clip = clip;
    //     clipTransition.Events.OnEnd = () => removeAnimation();

    //     if(damage > 0)
    //     {
    //         StartCoroutine(handleDamageNumbersAndWoundedAnim());
    //     }

    //     return clipTransition;
    // }

    // private void createClipTransitionThenLoop(AnimationClip clip)
    // {
    //     ClipTransition clipTransition = new ClipTransition();
    //     clipTransition.Clip = clip;
    //     clipTransition.Events.OnEnd = () => createClipTransitionThenLoop(clip);

    //     animancer.Play(clipTransition);
    // }


    // public static EffectAnimationManager instantiatePrefab(Transform parent = null)
    // {
    //     return Instantiate(Resources.Load<GameObject>(PrefabNames.effect), parent).GetComponent<EffectAnimationManager>();
    // }

    // public bool spriteSetByHeartBeat()
    // {
    //     return false;
    // }

    // public void removeAnimation()
    // {
    //     DestroyImmediate(gameObject);

    //     base.removeAnimation();
    // }

    // private void destroyEffectOfType(EffectAnimationType effectType)
    // {
    //     if(this.effectType == effectType)
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // private void OnEnable()
    // {
    //     DestroyAllEffectsOfType.AddListener(destroyEffectOfType);
    // }

    // private void OnDisable()
    // {
    //     DestroyAllEffectsOfType.RemoveListener(destroyEffectOfType);
    // }

}