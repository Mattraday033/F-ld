using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

//what AnimationData.animationCoroutine needs from the object it animates, and what AnimationTracker needs to track it
public interface IAnimationManager
{
    public GameObject gameObject { get; }

    //implementers take a key from AnimationTracker.getCurrentKey the first time it's read
    public int key { get; }

    //a single fire animation's object is destroyed once it's removed from AnimationTracker
    public bool singleFire { get; }

    //null for an object that isn't drawn with sprite layers
    public SpriteLayerRendererList rendererList
    {
        get;
    }

    //called once the first frame has finished showing
    public void onFirstFrameFinished();

    //called when a non-looping animation ends, or when there is nothing to draw on
    public void onAnimationFinished();
}

public class AnimationData
{
    private Dictionary<SpriteLayer, Sprite[]> sprites;
    private float[] timingInSeconds;

    private SFXType? sfx;
    private int sfxIndex;

    //one placement per frame for each layer drawn on a small canvas. Layers left out fill the body frame
    private Dictionary<SpriteLayer, CanvasPlacement[]> placements;

    public AnimationData(Dictionary<SpriteLayer, Sprite[]> sprites, float[] timingInSeconds, SFXType? sfx = null, int sfxIndex = 0,
                            Dictionary<SpriteLayer, CanvasPlacement[]> placements = null)
    {
        this.sprites = sprites;
        this.timingInSeconds = timingInSeconds;
        this.sfx = sfx;
        this.sfxIndex = sfxIndex;
        this.placements = placements;
    }

    public IEnumerator animationCoroutine(IAnimationManager animationManager, bool loopAnimation = false)
    {
        SpriteLayerRendererList rendererList = animationManager.rendererList;

        if(rendererList == null)
        {
            animationManager.onAnimationFinished();
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
                    //a layer with fewer frames than the timings (e.g. a single-frame fallback) holds its last frame
                    if(placements != null && placements.TryGetValue(kvp.Key, out CanvasPlacement[] layerPlacements))
                    {
                        rendererList.setLayerSprite(kvp.Key, kvp.Value[Math.Min(index, kvp.Value.Length - 1)], layerPlacements[Math.Min(index, layerPlacements.Length - 1)]);
                    } else
                    {
                        rendererList[kvp.Key].sprite = kvp.Value[Math.Min(index, kvp.Value.Length - 1)];
                    }
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
                    animationManager.onFirstFrameFinished();
                }

                if(index >= timingInSeconds.Length && !loopAnimation)
                {
                    animationManager.onAnimationFinished();
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

public class EffectAnimationManager : MonoBehaviour, IAnimationManager
{
    public readonly static UnityEvent<EffectAnimationType> DestroyAllEffectsOfType = new UnityEvent<EffectAnimationType>();
    
    public EffectAnimationType effectType;

    private int _Key = -1;
    public int key
    {
        get
        {
            if(_Key < 0)
            {
                _Key = AnimationTracker.getCurrentKey();
            }

            return _Key;
        }
    }

    public bool singleFire { get { return true; } }

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

    SpriteLayerRendererList IAnimationManager.rendererList
    {
        get
        {
            return rendererList;
        }
    }

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
                                                        EffectAnimationType effectType,
                                                        AnimationData animationData,
                                                        bool loops = false,
                                                        DamagePacket? damagePacket = null)
    {
        GameObject creature = Instantiate(Resources.Load<GameObject>(PrefabNames.creaturePrefab), CombatGrid.getPositionAt(coords), Quaternion.identity);

        EffectAnimationManager effect = creature.AddComponent<EffectAnimationManager>();
        effect.effectType = effectType;
        effect.targetCoords = coords;
        effect.animationData = animationData;
        effect.loops = loops;

        if(damagePacket.HasValue)
        {
            effect.damagePacket = damagePacket.Value;
        }

        effect.determineOutline();

        effect.StartCoroutine(animationData.animationCoroutine(effect, loops));

        return effect;
    }

    private void determineOutline()
    {
        switch(effectType)
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
                rendererList[SpriteLayer.Body].material = Resources.Load<Material>(PrefabNames.outlineMaterial);
                return;
        }
    }

    public void startAnimation()
    {
        if(_AnimationData != null && !started)
        {
            started = true;
            StartCoroutine(_AnimationData.animationCoroutine(this, loopAnimation: loops));
        }
    }

    public void onFirstFrameFinished()
    {
        createDamagePopUp();
    }

    public void onAnimationFinished()
    {
        //effects aren't tracked yet (see AnimationTracker.loadInstantEffect), and removing an untracked one
        //would rerun the end-of-turn checks after the tracked animations already ran them
        if(AnimationTracker.trackerBeingTracked(this))
        {
            //destroys this object too, since an effect is singleFire
            this.removeAnimation();
            return;
        }

        Destroy(gameObject);
    }

    public void createDamagePopUp()
    {
        if(damagePacket.HasValue && damagePacket.Value.damage > 0)
        {
            DamageNumberPopup.create(targetCoords, damagePacket.Value.damage, transform.position, DamageNumberPopup.getDirectionByTargetCoords(targetCoords),
                AnimationTracker.getInstance().damageNumberCanvas, damagePacket.Value.crit, damagePacket.Value.healsTarget);
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