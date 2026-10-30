using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public interface IAppearanceSource
{
    public IAppearance appearance
    {
        get;
    }
}

public class CreatureAnimationManager : MonoBehaviour, IAnimationManager
{

    public PolygonCollider2D polygonCollider2D
    {
        get
        {
            return rendererList.bodyCollider;
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

    private int _Key = -1;
    public int key
    {
        get
        {
            if(_Key < 0)
            {
                _Key = INameSource.getCurrentKey();
            }

            return _Key;
        }
    }

    #region INameSource

    public string displayName { get { return gameObject.name; } }
    public string uniqueName { get { return gameObject.name + "_" + key; } }

    #endregion

    //the character outlives its animations, so removing one from AnimationTracker mustn't destroy it
    public bool singleFire { get { return false; } }

    private MovementTracker _MovementTracker;
    public MovementTracker movementTracker
    {
        set
        {
            if(value == null)
            {
                return;
            }

            _MovementTracker = value;
            MovementManager.AfterMoveStarted.RemoveListener(handleMovementAnimation);
            MovementManager.OnMoveFinished.RemoveListener(handleMovementAnimation);
            MovementManager.AfterMoveStarted.AddListener(handleMovementAnimation);
            MovementManager.OnMoveFinished.AddListener(handleMovementAnimation);
        }
        get
        {
            return _MovementTracker;
        }
    }

    public CharacterFacing characterFacing = new CharacterFacing();

    private CharacterAnimationType _CurrentIdle;
    public CharacterAnimationType currentIdle
    {
        set
        {
            switch(value)
            {
                case CharacterAnimationType.Idle_Back:
                case CharacterAnimationType.Idle_Front:
                case CharacterAnimationType.OOC_Idle_Back:
                case CharacterAnimationType.OOC_Idle_Front:
                case CharacterAnimationType.Death_Back:
                case CharacterAnimationType.Death_Front:
                case CharacterAnimationType.Secondary_Idle_Back:
                case CharacterAnimationType.Secondary_Idle_Front:
                case CharacterAnimationType.Secondary_Death:
                    _CurrentIdle = value;
                    return;
            }   
        }
        private get
        {
            return _CurrentIdle;
        }
    }

    public Coroutine currentAnimation;

    private IAppearanceSource _AppearanceSource;
    public IAppearanceSource appearanceSource
    {
        set
        {
            _AppearanceSource = value;

            //a new appearance may bring a new color scheme, which is applied along with its first sprites
            colorsPending = true;
        }
    }

    //true until the current appearance's colors have been applied to the renderers
    private bool colorsPending = false;

    public void setAppearanceSource(IAppearanceSource appearanceSource,
                                    CharacterAnimationType newIdle = CharacterAnimationType.None)
    {
        this.appearanceSource = appearanceSource;

        if(newIdle != CharacterAnimationType.None)
        {
            playAnimation(newIdle);
        } else
        {
            handleMovementAnimation();
        }
    }

    public IAppearance appearance
    {
        get
        {
            return _AppearanceSource.appearance;
        }
    }

    public bool changesFacing
    {
        get
        {
            return !appearance.large;
        }
    }

    public ShadowExtras shadowExtras { get; private set; }

    private void Awake()
    {
        rendererList = GetComponent<SpriteLayerRendererList>();
        rendererList.Awake();

        shadowExtras = new ShadowExtras(this);
    }

    //the name and combatant components come with spawn behaviours that run after this one is added,
    //and the shadow is chosen by name, so it waits until they're all in place
    private void Start()
    {
        shadowExtras.createShadow();
    }

    public void playAnimation(CharacterAnimationType animationType = CharacterAnimationType.None)
    {

        switch(animationType)
        {
            case CharacterAnimationType.Run_Back:
            case CharacterAnimationType.Run_Back_Left:
            case CharacterAnimationType.Run_Back_Right:
            case CharacterAnimationType.Run_Front:
            case CharacterAnimationType.Run_Front_Left:
            case CharacterAnimationType.Run_Front_Right:
            case CharacterAnimationType.OOC_Idle_Front:
            case CharacterAnimationType.OOC_Idle_Back:
            case CharacterAnimationType.Idle_Front:
            case CharacterAnimationType.Idle_Back:

                currentIdle = animationType;

                appearance.applyAppearance(rendererList, animationType, updateColors: colorsPending);
                colorsPending = false;

                return;
            case CharacterAnimationType.Attack_Normal_Front:
            case CharacterAnimationType.Attack_Normal_Back:

                AnimationData animationData = appearance.getAnimationData(animationType);

                currentAnimation = StartCoroutine(animationData.animationCoroutine(this));

                AnimationTracker.trackAnimation(key, this);

                return;
            default:
                return;
        }
    }

    //the idle a character stands in when nothing else is playing: the combat-ready idle in a hostile area and the
    //relaxed one elsewhere, facing the camera unless the character faces north. A random facing hasn't been
    //chosen yet, so it's treated as facing the camera
    public static CharacterAnimationType getDefaultIdle(Facing facing)
    {
        bool hostile = AreaList.currentAreaIsHostile();

        switch(facing)
        {
            case Facing.NorthEast:
            case Facing.NorthWest:
                return hostile ? CharacterAnimationType.Idle_Back : CharacterAnimationType.OOC_Idle_Back;
            default:
                return hostile ? CharacterAnimationType.Idle_Front : CharacterAnimationType.OOC_Idle_Front;
        }
    }

    public void handleMovementAnimation(int i)
    {
        if(i != movementTracker.getMovementIndex())
        {
            return;
        }

        handleMovementAnimation();
    }

    public void handleMovementAnimation()
    {
        if(movementTracker != null && movementTracker.isChangingCells())
        {
            switch(characterFacing.getFacing())
            {
                case Facing.NorthEast:
                case Facing.NorthWest:
                    if(State.onLeftFoot)
                    {
                        playAnimation(CharacterAnimationType.Run_Back_Left);
                    } else
                    {
                        playAnimation(CharacterAnimationType.Run_Back_Right);
                    }
                    break;
                case Facing.SouthEast:
                case Facing.SouthWest:
                    if(State.onLeftFoot)
                    {
                        playAnimation(CharacterAnimationType.Run_Front_Left);
                    } else
                    {
                        playAnimation(CharacterAnimationType.Run_Front_Right);
                    }
                    break;
            }
        } else
        {
            playAnimation(getDefaultIdle(characterFacing.getFacing()));
        }

        rendererList.setFlipX(characterFacing.flipSprite());
        shadowExtras.updateShadowFacing();
    }

    #region IAnimationManager

    public void onFirstFrameFinished()
    {
        //Empty on purpose
    }

    public void onAnimationFinished()
    {
        currentAnimation = null;

        playAnimation(currentIdle);

        if(AnimationTracker.trackerBeingTracked(this))
        {
            this.removeAnimation();
        }
    }

    #endregion

    //stops the animation partway through and returns to the current idle. Unlike finishing, this doesn't check whether
    //every animation is done, so halting the last one doesn't end the turn
    public void haltAnimation()
    {
        if(currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }

        playAnimation(currentIdle);

        AnimationTracker.removeAnimation(key);
    }

    #region Combat
    //shells for the combat animations Stats used to drive through the old AnimationManager;
    //each TODO names the commented-out AnimationManager.cs method its body should be ported from

    public void playSpawnAnimation()
    {
        //TODO: port AnimationManager.playSpawnAnimation
    }

    public void playAttackAnimation()
    {
        if(characterFacing.facingNorth())
        {
            playAnimation(CharacterAnimationType.Attack_Normal_Back);
        } else
        {
            playAnimation(CharacterAnimationType.Attack_Normal_Front);
        }
    }

    public void playAttackIntoFrontIdleAnimation()
    {
        playAttackAnimation();
        
        if(AreaList.currentAreaIsHostile())
        {
            currentIdle = CharacterAnimationType.Idle_Front;
        } else
        {
            currentIdle = CharacterAnimationType.OOC_Idle_Front;
        }
    }

    public void playAttackIntoSecondaryIdleAnimation()
    {
        playAttackAnimation();
        
        if(characterFacing.facingNorth())
        {
            currentIdle = CharacterAnimationType.Secondary_Idle_Back;
        } else
        {
            currentIdle = CharacterAnimationType.Secondary_Idle_Front;
        }
    }

    public void playSpecialAttackAnimation()
    {
        //TODO: port AnimationManager.playSpecialAttackAnimation
    }

    public void playWoundedAnimation()
    {
        //TODO: port AnimationManager.playWoundedAnimation
    }

    public void playDeathAnimation()
    {
        //TODO: port AnimationManager.playDeathAnimation
    }

    public void setToDefaultIdle()
    {
        //TODO: port AnimationManager.setToDefaultIdle
    }

    //front is true for combatants on the enemy side, who face the camera
    public void setToDeadIdle(bool front)
    {
        //TODO: port AnimationManager.setCurrentIdle(Death_Front/Death_Back)
    }

    public void setIdleAnimationOnTraitApplication(Trait trait)
    {
        //TODO: port Trait.setIdleAnimationOnApplication(AnimationManager)
    }

    public void setIdleAnimationOnTraitRemoval(Trait trait)
    {
        //TODO: port Trait.setIdleAnimationOnRemoval(AnimationManager)
    }

    public void setHeartBeatRow(int heartBeatRow)
    {
        //TODO: port AnimationManager.heartBeatRow
    }

    public void playAnimationSFX(CharacterAnimationType animationType)
    {
        //TODO: port the AnimationManager clip events that called Stats.playAnimationSFX
    }

    public float getAnimationLength(CharacterAnimationType animationType)
    {
        //TODO: port AnimationManager.getAnimationLength
        return 0f;
    }

    #endregion

    private void OnEnable()
    {
        if(movementTracker == null)
        {
            movementTracker = GetComponent<MovementTracker>();
        }

        characterFacing.OnFacingChange.AddListener(handleMovementAnimation);
    }

    private void OnDestroy()
    {
        MovementManager.AfterMoveStarted.RemoveListener(handleMovementAnimation);
        MovementManager.OnMoveFinished.RemoveListener(handleMovementAnimation);

        characterFacing.OnFacingChange.RemoveListener(handleMovementAnimation);

        shadowExtras.destroy();
    }

    //The shadow drawn under the character, and the "extras" that are hidden and shown along with it:
    //the shadow itself and, in combat, the health bar
    public class ShadowExtras
    {
        //clears every speaker outline at once, as dialogue moves from one speaker to the next
        public readonly static UnityEvent RemoveAllShadowOutlines = new UnityEvent();

        private static readonly int blackBorderSizeXID = Shader.PropertyToID("_BlackBorderSizeX");
        private static readonly int blackBorderSizeYID = Shader.PropertyToID("_BlackBorderSizeY");
        private static readonly int colorOutlineSizeXID = Shader.PropertyToID("_ColorOutlineSizeX");
        private static readonly int colorOutlineSizeYID = Shader.PropertyToID("_ColorOutlineSizeY");
        private static readonly int blackBorderColorID = Shader.PropertyToID("_BlackBorderColor");

        //a 256x128 shadow sits off centre, so it's moved to stay under a character facing away from the camera
        private static readonly Vector3 northWestShadowPosition = new Vector3(.5f, -.225f);
        private static readonly Vector3 northEastShadowPosition = new Vector3(-.5f, -.225f);

        private readonly CreatureAnimationManager owner;

        private SpriteRenderer shadowSprite;
        private string shadowPrefabName;

        //the shadow's own material, put back when its outline is removed
        private Material shadowMaterial;
        //made on the first outline, so a shadow that's never outlined doesn't get its own material
        private Material outlineMaterial;

        private bool listening = false;

        //kept apart from the shadow, so extras hidden before the shadow is made, like a train member spawned
        //on top of another, stay hidden once it is
        private bool extrasActive = true;

        //false for things that shouldn't cast one, like containers. Set by the spawn behaviour before the shadow is made
        private bool _HasShadow = true;
        public bool hasShadow
        {
            get
            {
                return _HasShadow;
            }
            set
            {
                _HasShadow = value;

                if(!_HasShadow)
                {
                    destroyShadow();
                }
            }
        }

        public ShadowExtras(CreatureAnimationManager owner)
        {
            this.owner = owner;
        }

        private Combatant combatant
        {
            get
            {
                return owner.GetComponent<Combatant>();
            }
        }

        //the first name source beside the manager that has a name to give
        private string uniqueName
        {
            get
            {
                foreach(INameSource nameSource in owner.GetComponents<INameSource>())
                {
                    //the icon manager only passes on another source's name, so it's skipped in favour of that source.
                    //The animation managers only name their own GameObject, so they're skipped as well
                    if(nameSource is OverHeadIconManager || nameSource is IAnimationManager)
                    {
                        continue;
                    }

                    string name = nameSource.uniqueName;

                    if(!string.IsNullOrEmpty(name))
                    {
                        return name;
                    }
                }

                return Constants.emptyString;
            }
        }

        public void createShadow()
        {
            if(shadowSprite != null || !hasShadow)
            {
                return;
            }

            //a reposition placeholder is drawn faded, and a full strength shadow under it would give it away
            Combatant ownerCombatant = combatant;

            if(ownerCombatant != null && ownerCombatant.isPlaceholder)
            {
                return;
            }

            shadowPrefabName = EnemyTypeFolderPathList.getShadowPrefabName(uniqueName);

            GameObject shadowPrefab = Resources.Load<GameObject>(shadowPrefabName);

            if(shadowPrefab == null)
            {
                Debug.LogError("No shadow prefab at " + shadowPrefabName);
                return;
            }

            GameObject shadow = Instantiate(shadowPrefab, owner.transform);
            shadow.transform.SetAsFirstSibling();

            shadowSprite = shadow.GetComponent<SpriteRenderer>();
            shadowMaterial = shadowSprite.sharedMaterial;

            shadow.SetActive(extrasActive);

            RemoveAllShadowOutlines.AddListener(removeShadowOutline);
            PlayerStateManager.OnStateChangeFromInDialogue.AddListener(removeShadowOutline);
            listening = true;

            updateShadowFacing();
        }

        #region Facing

        public void updateShadowFacing()
        {
            if(shadowSprite == null || !canChangeShadowFacing())
            {
                return;
            }

            Facing facing = owner.characterFacing.getFacing();

            bool flip = facing == Facing.NorthWest ||
                        facing == Facing.SouthEast ||
                        flipShadowByName();

            foreach(SpriteRenderer shadowPart in shadowSprite.GetComponentsInChildren<SpriteRenderer>(true))
            {
                shadowPart.flipX = flip;
            }

            switch(facing)
            {
                case Facing.SouthEast:
                case Facing.SouthWest:
                    shadowSprite.transform.localPosition = Vector3.zero;
                    break;
                case Facing.NorthWest:
                    shadowSprite.transform.localPosition = northWestShadowPosition;
                    break;
                case Facing.NorthEast:
                    shadowSprite.transform.localPosition = northEastShadowPosition;
                    break;
            }
        }

        //some characters' combat sprites face the other way from their shadow's default
        private bool flipShadowByName()
        {
            Combatant ownerCombatant = combatant;

            if(!CombatStateManager.inCombat || ownerCombatant == null || ownerCombatant.stats == null)
            {
                return false;
            }

            switch(ownerCombatant.stats.characterName)
            {
                case NPCNameList.captainAdela:
                    return true;
                default:
                    return false;
            }
        }

        //only the off centre 256x128 shadow turns with its character
        private bool canChangeShadowFacing()
        {
            return shadowPrefabName == PrefabNames.shadow256x128;
        }

        #endregion

        #region Extras

        public void disableExtras()
        {
            setExtrasActive(false);
        }

        public void enableExtras()
        {
            setExtrasActive(true);
        }

        private void setExtrasActive(bool active)
        {
            extrasActive = active;

            if(CombatStateManager.inCombat)
            {
                Combatant ownerCombatant = combatant;

                if(ownerCombatant != null && ownerCombatant.healthBarManager != null)
                {
                    if(active)
                    {
                        ownerCombatant.healthBarManager.show();
                    } else
                    {
                        ownerCombatant.healthBarManager.hide();
                    }
                }
            }

            if(shadowSprite != null)
            {
                shadowSprite.gameObject.SetActive(active);
            }
        }

        #endregion

        #region Outline

        public void showShadowOutline()
        {
            createShadowOutline(ColorList.canBeInteractedWith, getOutlineSize());
        }

        private void createShadowOutline(Color color, float sizeMod)
        {
            if(shadowSprite == null)
            {
                return;
            }

            if(!shadowSprite.gameObject.activeInHierarchy || shadowSprite.color.Equals(Color.clear) || shadowSprite.sprite == null)
            {
                removeShadowOutline();
                return;
            }

            if(outlineMaterial == null)
            {
                outlineMaterial = new Material(Resources.Load<Material>(PrefabNames.outlineMaterial));
            }

            outlineMaterial.color = color;
            outlineMaterial.SetColor(blackBorderColorID, Color.black);

            //the sizes are in UV space, so they're measured against the shadow's texture
            float sizeX = sizeMod/shadowSprite.sprite.texture.width;
            float sizeY = sizeMod/shadowSprite.sprite.texture.height;

            outlineMaterial.SetFloat(blackBorderSizeXID, sizeX/4f);
            outlineMaterial.SetFloat(blackBorderSizeYID, sizeY/4f);
            outlineMaterial.SetFloat(colorOutlineSizeXID, sizeX);
            outlineMaterial.SetFloat(colorOutlineSizeYID, sizeY);

            shadowSprite.sharedMaterial = outlineMaterial;
        }

        public void removeShadowOutline()
        {
            if(shadowSprite == null)
            {
                return;
            }

            shadowSprite.sharedMaterial = shadowMaterial;
        }

        //the bigger shadows are drawn from bigger textures, so they need fewer UV pixels for the same width of outline
        private float getOutlineSize()
        {
            switch(DialogueList.scrubNameOfEndNumbers(uniqueName))
            {
                case NPCNameList.horse:
                case NPCNameList.csalan:
                    return Constants.shadow512x512OutlineSize;
                default:
                    return Constants.shadow256x256OutlineSize;
            }
        }

        #endregion

        //for a shadow turned off after it was made
        private void destroyShadow()
        {
            if(shadowSprite == null)
            {
                return;
            }

            Destroy(shadowSprite.gameObject);
            shadowSprite = null;
            shadowMaterial = null;
        }

        public void destroy()
        {
            if(listening)
            {
                RemoveAllShadowOutlines.RemoveListener(removeShadowOutline);
                PlayerStateManager.OnStateChangeFromInDialogue.RemoveListener(removeShadowOutline);
                listening = false;
            }

            if(outlineMaterial != null)
            {
                Destroy(outlineMaterial);
                outlineMaterial = null;
            }
        }
    }

}
