using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using UnityEngine.Events;
using System;
using System.Linq;

public enum CharacterAnimationType { 
                                    None, 
                                    Idle_Front, Idle_Back,
                                    OOC_Idle_Front, OOC_Idle_Back,
                                    Secondary_Idle, Secondary_Idle_Front, Secondary_Idle_Back,
                                    Run_Front, Run_Front_Left, Run_Front_Right, 
                                    Run_Back, Run_Back_Left, Run_Back_Right, 
                                    Wounded, Wounded_Front, Wounded_Back, OOC_Wounded_Front, OOC_Wounded_Back,
                                    Death, Death_Front, Death_Back, Death_Front_Weaponless, Death_Back_Weaponless,
                                    Attack_Normal, Attack_Normal_Front, Attack_Normal_Back, 
                                    Attack_Special, 
                                    StandUp, 
                                    Spawn 
                                    }

public class AnimationManager : MonoBehaviour, IAnimationTracker
{
    public readonly static CharacterAnimationType[] loopedAnimationTypes = new CharacterAnimationType[]
    {   
        CharacterAnimationType.Idle_Front, CharacterAnimationType.Idle_Back,
        CharacterAnimationType.OOC_Idle_Front, CharacterAnimationType.OOC_Idle_Back,
        CharacterAnimationType.Secondary_Idle, CharacterAnimationType.Secondary_Idle_Front, CharacterAnimationType.Secondary_Idle_Back,
        CharacterAnimationType.Death_Front_Weaponless, CharacterAnimationType.Death_Back_Weaponless
    };


    public readonly static CharacterAnimationType[] tempAnimationTypes = new CharacterAnimationType[]
    { 
      CharacterAnimationType.Run_Front,     CharacterAnimationType.Run_Front_Left,      CharacterAnimationType.Run_Front_Right, 
      CharacterAnimationType.Run_Back,      CharacterAnimationType.Run_Back_Left,       CharacterAnimationType.Run_Back_Right,  
      CharacterAnimationType.Wounded,       CharacterAnimationType.Wounded_Front,       CharacterAnimationType.Wounded_Back,
      CharacterAnimationType.OOC_Wounded_Front, CharacterAnimationType.OOC_Wounded_Back,
      CharacterAnimationType.Death,         CharacterAnimationType.Death_Front,         CharacterAnimationType.Death_Back,   
      CharacterAnimationType.Attack_Normal, CharacterAnimationType.Attack_Normal_Front, CharacterAnimationType.Attack_Normal_Back, 
      CharacterAnimationType.Attack_Special,
      CharacterAnimationType.Spawn,
      CharacterAnimationType.StandUp
    };

    public bool changesFacing;
    
    public CharacterFacing _Facing = new CharacterFacing();

    public CharacterFacing facing
    {
        get
        {
            return _Facing;
        }
        private set
        {
            _Facing = value;
        }
    }
    public SpriteRenderer spriteRenderer;

    private CharacterAnimationType currentIdle;
    [SerializeField]
    private SpriteRenderer shadowSprite;
    public PolygonCollider2D polygonCollider2D;
    public HealthBarManager healthBarManager;
    private Stats _LinkedStats;
    public Stats linkedStats
    {
        get
        {
            return _LinkedStats;
        }
        set
        {
            _LinkedStats = value;

            if(linkedStats != null)
            {
                npcName = linkedStats.getName();
            }
        }
    }
    public string npcName = ""; //name of specific character. i.e. : guard 1, guard 2. Used for differentiating different characters that use the same animations/sprites.
    public string animationName = ""; //name of character sprites. i.e.: Spearman, Giant Bat. Sometimes same as npcName.
    public int heartBeatRow = 0;

    public readonly static UnityEvent<string, CharacterAnimationType> SetIdleByNPCName = new UnityEvent<string, CharacterAnimationType>();
    public readonly static UnityEvent<string, CharacterAnimationType> PlayAnimationByNPCName = new UnityEvent<string, CharacterAnimationType>();

    public Dictionary<CharacterAnimationType, AnimationClip> animationDict;

    public NamedAnimancerComponent animancer;

    #region HeartBeatListener

    private void updateIdleAnimation(int rowToUpdate)
    {
        if(linkedStats != null && linkedStats.isDead() || LoadSaveFile.midLoad)
        {
            return;
        }

        if(animancer.IsPlaying())
        {
            return;
        }

        if(spriteSetByHeartBeat() && animancer.enabled)
        {
            haltAllAnimations();
        }

        if(!spriteSetByHeartBeat() || heartBeatRow != rowToUpdate)
        {
            return;
        }

        setSpriteToCurrentIdle();
    }

    private void setSpriteToCurrentIdle()
    {
        spriteRenderer.sprite = IdleDictionary.getCurrentIdleSprite(heartBeatRow,
                                                                    animationName, 
                                                                    currentIdle);

        Helpers.updatePolygonCollider(spriteRenderer, polygonCollider2D);
    }

    public virtual bool spriteSetByHeartBeat()
    {
        return !CombatAnimationManager.trackerBeingTracked(this) && 
                ((!CombatStateManager.inCombat && !PlayerMovement.getInstance().canPlayRunAnimation() && PlayerOOCStateManager.currentActivity != OOCActivity.Defeat) || 
                (CombatStateManager.inCombat && !linkedStats.isDead()));
    }

    public void playAnimationByNPCName(string npcName, CharacterAnimationType animationType)
    {
        if(this.npcName.Equals(npcName))
        {
            switch(animationType)
            {
                case CharacterAnimationType.Attack_Normal_Front:
                    playAttackFrontAnimation();
                    break;
            }     
        }
    }

    public void setCurrentIdle(string npcName, CharacterAnimationType animationType)
    {
        if(this.npcName.Equals(npcName))
        {
            setCurrentIdle(animationType);            
        }
    }

    public void setCurrentIdle(CharacterAnimationType newIdle)
    {
        newIdle = getFallBackIdleType(animationName, newIdle);

        if(!CombatStateManager.inCombat || 
            CombatStateManager.whoseTurn == WhoseTurn.Won || 
            CombatStateManager.whoseTurn == WhoseTurn.Lost)
        {
            currentIdle = newIdle;
            setSpriteToCurrentIdle();
            return;
        } 

        bool isAlly = linkedStats.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p));
        bool containsSprites = IdleDictionary.idleDictContainsSprites(animationName, newIdle);

        if((newIdle == CharacterAnimationType.Secondary_Idle || newIdle == CharacterAnimationType.Death) &&
            containsSprites)
        {
            currentIdle = newIdle;
            setSpriteToCurrentIdle();
            return;
        }

        if(isAlly)
        {
            currentIdle = CharacterAnimationType.Idle_Back;
        } else
        {
            currentIdle = CharacterAnimationType.Idle_Front;
        }

        setSpriteToCurrentIdle();
    }

    private void OnDestroy()
    {
        HeartBeatManager.getHeartBeat(animationName).RemoveListener(updateIdleAnimation);
        CombatStateManager.OnActivityChangeToInEscapeMenu.RemoveListener(disablePolygonCollider);
        CombatStateManager.OnActivityChangeToResolveTurnWarning.RemoveListener(disablePolygonCollider);
        CombatStateManager.OnActivityChangeFromInEscapeMenu.RemoveListener(enablePolygonCollider);
        CombatStateManager.OnActivityChangeFromResolveTurnWarning.RemoveListener(enablePolygonCollider);
        CombatTraitColliderDisabler.OnCombatTraitHoverEnter.RemoveListener(disablePolygonCollider);
        CombatTraitColliderDisabler.OnCombatTraitHoverExit.RemoveListener(enablePolygonCollider);
        SetIdleByNPCName.RemoveListener(setCurrentIdle);
        PlayAnimationByNPCName.RemoveListener(playAnimationByNPCName);
    }

    private void disablePolygonCollider()
    {
        polygonCollider2D.enabled = false;
    }

    private void enablePolygonCollider()
    {
        polygonCollider2D.enabled = true;
    }

    #endregion

    #region IAnimationTracker

    public int key;

    private void Awake()
    {
        key = CombatAnimationManager.getCurrentKey();
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public virtual void removeAnimation()
    {
        CombatAnimationManager.removeAnimation(key);

        if(spriteSetByHeartBeat())
        {
            enableExtras();
        }

        if(!CombatStateManager.inCombat)
        {
            return;
        }

        CombatAnimationManager.checkAllAnimationsFinished();
    }

    #endregion

    public virtual void setAnimations(string monsterName)
    {
        animationName = monsterName;
        string folderPath = EnemyTypeFolderPathList.getEnemyTypeFolderPath(animationName);

        instantiateShadow();

        if (folderPath == null)
        {
            animationDict = new Dictionary<CharacterAnimationType, AnimationClip>();
            changesFacing = false;
            return;
        }

        changesFacing = true;

        animationDict = getTempAnimations(folderPath);

        animancer.Animations = getIdleAnimations(folderPath);

        addIdleSprites(animationName, folderPath);

        setHeartBeatRow();

        HeartBeatManager.getHeartBeat(animationName).AddListener(updateIdleAnimation);
        CombatStateManager.OnActivityChangeToInEscapeMenu.AddListener(disablePolygonCollider);
        CombatStateManager.OnActivityChangeToResolveTurnWarning.AddListener(disablePolygonCollider);
        CombatStateManager.OnActivityChangeFromInEscapeMenu.AddListener(enablePolygonCollider);
        CombatStateManager.OnActivityChangeFromResolveTurnWarning.AddListener(enablePolygonCollider);
        CombatTraitColliderDisabler.OnCombatTraitHoverEnter.AddListener(disablePolygonCollider);
        CombatTraitColliderDisabler.OnCombatTraitHoverExit.AddListener(enablePolygonCollider);
        SetIdleByNPCName.AddListener(setCurrentIdle);
        PlayAnimationByNPCName.AddListener(playAnimationByNPCName);

        setToDefaultIdle();
    }

    private void instantiateShadow()
    {
        GameObject shadow = Instantiate(Resources.Load<GameObject>(EnemyTypeFolderPathList.getShadowPrefabName(animationName)), transform);
        shadow.transform.SetAsFirstSibling();
        shadowSprite = shadow.GetComponent<SpriteRenderer>();

        if(CombatStateManager.inCombat)
        {
            setFacing(facing.getFacing());
        }

        setAllShadowDirections(shadowSprite);
    }

    public void setAllShadowDirections(SpriteRenderer shadow)
    {
        if(shadow == null || !canChangeShadowFacing())
        {
            return;
        }

        shadow.flipX = facing.getFacing() == Facing.NorthWest || 
                        facing.getFacing() == Facing.SouthEast;

        switch(facing.getFacing())
        {
            case Facing.SouthEast:
            case Facing.SouthWest:
                shadowSprite.transform.localPosition = Vector3.zero;
                break;
            case Facing.NorthWest:
                shadowSprite.transform.localPosition = new Vector3(.5f, -.225f);
                break;
            case Facing.NorthEast:
                shadowSprite.transform.localPosition = new Vector3(-.5f, -.225f);
                break;
        }

        foreach(Transform transform in shadow.transform)
        {
            setAllShadowDirections(transform.GetComponent<SpriteRenderer>());
        }
    }

    private bool canChangeShadowFacing()
    {
        return EnemyTypeFolderPathList.getShadowPrefabName(animationName).Equals(PrefabNames.shadow256x128);
    }

    private void setHeartBeatRow()
    {
        if(CombatStateManager.inCombat)
        {
            heartBeatRow = linkedStats.positions.Count > 0 ? linkedStats.positions[0].row : 0;
        } else
        {
            heartBeatRow = UnityEngine.Random.Range(CombatGrid.enemyRowUpperBounds,CombatGrid.allyRowLowerBounds);
        }
    }

    public void setToDefaultIdle()
    {
        if(CombatStateManager.inCombat)
        {
            setCurrentIdle(getFallBackIdleType(animationName, CharacterAnimationType.Idle_Front));
            haltAllAnimations();

            setSpriteToCurrentIdle();
        } else
        {
            setCurrentIdle(getFallBackIdleType(animationName, CharacterAnimationType.OOC_Idle_Front));

            playCurrentIdleAnimation();
        }
    }

    private static AnimationClip[] getIdleAnimations(string folderPath)
    {
        List<AnimationClip> animationList = new List<AnimationClip>();
        List<CharacterAnimationType> animationTypes = new List<CharacterAnimationType>();

        animationTypes.AddRange(loopedAnimationTypes);
        animationTypes.AddRange(tempAnimationTypes);

        foreach (CharacterAnimationType type in animationTypes)
        {
            AnimationClip animClip = Resources.Load<AnimationClip>(folderPath + type.ToString());

            if(animClip == null)
            {
                continue;
            }

            animationList.Add(animClip);
        }

        return animationList.ToArray();
    }

    public static void resetIdleDictionaryEntry(string animationName)
    {
        addIdleSprites(animationName, EnemyTypeFolderPathList.getEnemyTypeFolderPath(animationName));
    }

    private static void addIdleSprites(string animationName, string folderPath)
    {
        foreach (CharacterAnimationType type in loopedAnimationTypes)
        {
            addIdleSpritesOfType(animationName, folderPath, type);
        }

        addIdleSpritesOfType(animationName, folderPath, CharacterAnimationType.Death);
        addIdleSpritesOfType(animationName, folderPath, CharacterAnimationType.Death_Back);
        addIdleSpritesOfType(animationName, folderPath, CharacterAnimationType.Death_Front);
    }

    private static void addIdleSpritesOfType(string animationName, string folderPath, CharacterAnimationType type)
    {
        if(IdleDictionary.idleDictContainsSprites(animationName, type))
        {
            return;
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(folderPath+type.ToString());

        if(sprites == null || sprites.Length <= 0)
        {
            return;
        }

        switch(type)
        {
            case CharacterAnimationType.Death:
            case CharacterAnimationType.Death_Back:
            case CharacterAnimationType.Death_Front:

                sprites = new Sprite[1]{sprites[sprites.Length-1]};    

                break;
        }

        IdleDictionary.addSpritesToIdleDict(animationName, type, sprites);
    }

    private static Dictionary<CharacterAnimationType, AnimationClip> getTempAnimations(string folderPath)
    {
        Dictionary<CharacterAnimationType, AnimationClip> animationDict = new Dictionary<CharacterAnimationType, AnimationClip>();

        foreach (CharacterAnimationType type in tempAnimationTypes)
        {
            AnimationClip animClip = Resources.Load<AnimationClip>(folderPath + type.ToString());

            if(animClip == null)
            {
                continue;
            }

            animationDict.Add(type, animClip);
        }

        return animationDict;
    }

    public void playRunBackAnimation()
    {
        playAnimation(getRunAnimationFooting(CharacterAnimationType.Run_Back));
    }

    public void playRunFrontAnimation()
    {
        playAnimation(getRunAnimationFooting(CharacterAnimationType.Run_Front));
    }

    private static CharacterAnimationType getRunAnimationFooting(CharacterAnimationType animationType)
    {
        if(animationType == CharacterAnimationType.Run_Front)
        {
            if(State.onLeftFoot)
            {
                return CharacterAnimationType.Run_Front_Left;
            } else
            {
                return CharacterAnimationType.Run_Front_Right;
            }
        } else
        {
            if(State.onLeftFoot)
            {
                return CharacterAnimationType.Run_Back_Left;
            } else
            {
                return CharacterAnimationType.Run_Back_Right;
            }
        }
    }

    private void playIdleAnimation(CharacterAnimationType newIdle)
    {
        switch(currentIdle)
        {
            case CharacterAnimationType.Secondary_Idle:
            case CharacterAnimationType.Death:
            case CharacterAnimationType.Death_Back:
            case CharacterAnimationType.Death_Back_Weaponless:
            case CharacterAnimationType.Death_Front:
            case CharacterAnimationType.Death_Front_Weaponless:
                return;
        }

        newIdle = getFallBackIdleType(animationName, newIdle);

        enableExtras();
        removeAnimation();
        setCurrentIdle(newIdle);
        playAnimation(newIdle);
    }

    public void playCurrentIdleAnimation()
    {
        enableExtras();
        removeAnimation();
        playAnimation(currentIdle);
    }

    public void playSpawnAnimation()
    {
        disableExtras();
        CombatAnimationManager.trackAnimation(key, this, getAnimationLength(CharacterAnimationType.Spawn));
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Spawn));
        linkedStats.playAnimationSFX(CharacterAnimationType.Spawn);
    }

    public float getAnimationLength(CharacterAnimationType type)
    {
        type = getFallBackAnimationType(type);

        if (animationDict.ContainsKey(type))
        {
            return animationDict[type].length;
        }

        return 0f;
    }

    public void playStandUpAnimation()
    {
        setCurrentIdle(CharacterAnimationType.OOC_Idle_Front);
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.StandUp));
        // removeAnimation();
    }

    public void playDeathAnimation()
    {
        if(currentIdle == getDeathAnimationType())
        {
            return;
        }

        disableExtras();
        playAnimation(createClipTransitionToDeath());
        removeAnimation();

        if(linkedStats != null)
        {
            linkedStats.playAnimationSFX(CharacterAnimationType.Death);
        }
    }

    public void playDeathAnimationThenHide()
    {
        if(currentIdle != getDeathAnimationType() && linkedStats != null)
        {
            linkedStats.playAnimationSFX(CharacterAnimationType.Death);
        }

        playAnimation(createClipTransitionToDeathThenHide());
        // removeAnimation();
    }

    public void playAttackAnimation()
    {
        CharacterAnimationType attackAnimationType = CharacterAnimationType.Attack_Normal;

        if(CombatStateManager.inCombat)
        {
            if(linkedStats.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
            {
                attackAnimationType = CharacterAnimationType.Attack_Normal_Back;
            } else
            {
                attackAnimationType = CharacterAnimationType.Attack_Normal_Front;
            }
        }

        CombatAnimationManager.trackAnimation(key, this, getAnimationLength(attackAnimationType));
        playAnimation(createClipTransitionToIdle(attackAnimationType));

        if(CombatStateManager.inCombat)
        {
            linkedStats.playAnimationSFX(CharacterAnimationType.Attack_Normal);
        }
    }

    public void playAttackFrontAnimation()
    {
        CombatAnimationManager.trackAnimation(key, this, getAnimationLength(CharacterAnimationType.Attack_Normal_Front));
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Normal_Front));

        if(CombatStateManager.inCombat)
        {
            linkedStats.playAnimationSFX(CharacterAnimationType.Attack_Normal);
        }
    }

    public void playAttackBackAnimation()
    {
        CombatAnimationManager.trackAnimation(key, this, getAnimationLength(CharacterAnimationType.Attack_Normal_Back));
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Normal_Back));

        if(CombatStateManager.inCombat)
        {
            linkedStats.playAnimationSFX(CharacterAnimationType.Attack_Normal);
        }
    }

    public void playAttackIntoFrontIdleAnimation()
    {
        CombatAnimationManager.trackAnimation(key, this, getAnimationLength(CharacterAnimationType.Attack_Normal));
        setCurrentIdle(CharacterAnimationType.Idle_Front);
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Normal));
        
        linkedStats.playAnimationSFX(CharacterAnimationType.Attack_Normal);
    }

    public void playAttackIntoSecondaryIdleAnimation()
    {
        CombatAnimationManager.trackAnimation(key, this, getAnimationLength(CharacterAnimationType.Attack_Special));
        setCurrentIdle(CharacterAnimationType.Secondary_Idle);
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Special));
        linkedStats.playAnimationSFX(CharacterAnimationType.Attack_Normal);
    }

    public void playSpecialAttackAnimation()
    {
        CombatAnimationManager.trackAnimation(key, this, getAnimationLength(CharacterAnimationType.Attack_Special));

        if(CombatStateManager.inCombat &&
            healthBarManager!= null && 
            linkedStats != null && 
            linkedStats.isDead() && 
            linkedStats.notResurrectable())
        {
            linkedStats.outline.createOutline(Color.clear);
            playAnimation(createClipTransitionSpecialAnimationThenHide());
        } else
        {
            playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Special));
        }
        
        linkedStats.playAnimationSFX(CharacterAnimationType.Attack_Special);
    }

    public void playWoundedAnimation()
    {
        if(currentIdle == getDeathAnimationType())
        {
            return;
        }

        CharacterAnimationType woundedAnimationType = CharacterAnimationType.Wounded;

        if(CombatStateManager.inCombat)
        {
            if(linkedStats.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
            {
                woundedAnimationType = CharacterAnimationType.Wounded_Back;
            } else
            {
                woundedAnimationType = CharacterAnimationType.Wounded_Front;
            }
        }

        CombatAnimationManager.trackAnimation(key, this, getAnimationLength(woundedAnimationType));
        playAnimation(createClipTransitionToIdle(woundedAnimationType));
    }

    public void playNorthEastRun()
    {
        setFacing(Facing.NorthEast);
        playRunBackAnimation();
    }

    public void playNorthWestRun()
    {
        setFacing(Facing.NorthWest);
        playRunBackAnimation();
    }

    public void playSouthEastRun()
    {
        setFacing(Facing.SouthEast);
        playRunFrontAnimation();
    }

    public void playSouthWestRun()
    {
        setFacing(Facing.SouthWest);
        playRunFrontAnimation();
    }


    public void playNorthEastOOCIdle()
    {
        setFacing(Facing.NorthEast);
        playIdleAnimation(CharacterAnimationType.OOC_Idle_Back);
    }

    public void playNorthWestOOCIdle()
    {
        setFacing(Facing.NorthWest);
        playIdleAnimation(CharacterAnimationType.OOC_Idle_Back);
    }

    public void playSouthEastOOCIdle()
    {
        setFacing(Facing.SouthEast);
        playIdleAnimation(CharacterAnimationType.OOC_Idle_Front);
    }

    public void playSouthWestOOCIdle()
    {
        setFacing(Facing.SouthWest);
        playIdleAnimation(CharacterAnimationType.OOC_Idle_Front);
    }


    public void playNorthEastSecondaryIdle()
    {
        setFacing(Facing.NorthEast);
        playIdleAnimation(CharacterAnimationType.Secondary_Idle_Back);
    }

    public void playNorthWestSecondaryIdle()
    {
        setFacing(Facing.NorthWest);
        playIdleAnimation(CharacterAnimationType.Secondary_Idle_Back);
    }

    public void playSouthEastSecondaryIdle()
    {
        setFacing(Facing.SouthEast);
        playIdleAnimation(CharacterAnimationType.Secondary_Idle_Front);
    }

    public void playSouthWestSecondaryIdle()
    {
        setFacing(Facing.SouthWest);
        playIdleAnimation(CharacterAnimationType.Secondary_Idle_Front);
    }

    public void playNorthEastIdle()
    {
        setFacing(Facing.NorthEast);
        playIdleAnimation(CharacterAnimationType.Idle_Back);
    }

    public void playNorthWestIdle()
    {
        setFacing(Facing.NorthWest);
        playIdleAnimation(CharacterAnimationType.Idle_Back);
    }

    public void playSouthEastIdle()
    {
        setFacing(Facing.SouthEast);
        playIdleAnimation(CharacterAnimationType.Idle_Front);
    }

    public void playSouthWestIdle()
    {
        setFacing(Facing.SouthWest);
        playIdleAnimation(CharacterAnimationType.Idle_Front);
    }

    private ClipTransition createClipTransitionToIdle(CharacterAnimationType type)
    {
        type = getFallBackAnimationType(type);

        if (!animationDict.ContainsKey(type))
        {
            if(type == CharacterAnimationType.Attack_Special)
            {
                return createClipTransitionToIdle(CharacterAnimationType.Attack_Normal);
            }
            removeAnimation();
            return null;
        }

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[type];
        clipTransition.Events.OnEnd = () => playCurrentIdleAnimation();

        return clipTransition;
    }

    private ClipTransition createClipTransitionToDeath()
    {        
        CharacterAnimationType deathAnimationType = getDeathAnimationType();

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[deathAnimationType];
        clipTransition.Events.OnEnd = () => haltAllAnimations();

        currentIdle = deathAnimationType;

        return clipTransition;
    }

    private ClipTransition createClipTransitionToDeathThenHide()
    {
        CharacterAnimationType deathAnimationType = getDeathAnimationType();

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[deathAnimationType];
        clipTransition.Events.OnEnd = () => hideObject();

        currentIdle = deathAnimationType;

        return clipTransition;
    }

    private ClipTransition createClipTransitionSpecialAnimationThenHide()
    {
        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[CharacterAnimationType.Attack_Special];
        clipTransition.Events.OnEnd = () => removeObjectFromBoard();

        return clipTransition;
    }

    private CharacterAnimationType getDeathAnimationType()
    {
        CharacterAnimationType deathAnimationType = CharacterAnimationType.Death;

        if(CombatStateManager.inCombat)
        {
            if(linkedStats.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
            {
                deathAnimationType = CharacterAnimationType.Death_Back;
            } else
            {
                deathAnimationType = CharacterAnimationType.Death_Front;
            }
        }

        return getFallBackAnimationType(deathAnimationType);
    }

    private void removeObjectFromBoard()
    {
        hideObject();

        linkedStats.removeFromGrid();
    }


    private void hideObject()
    {
        haltAllAnimations();
        gameObject.SetActive(false);
    }

    public void startUpAnimations()
    {
        animancer.enabled = true;
    }

    public void haltAllAnimations()
    {
        animancer.Stop();
        animancer.enabled = false;
        removeAnimation();
    }

    private void playAnimation(ClipTransition clipTransition)
    {
        if(clipTransition == null)
        {
            return;
        }

        startUpAnimations();

        animancer.Stop();
        AnimancerState state = animancer.Play(clipTransition);

        if (state == null)
        {
            removeAnimation();
            Debug.Log("No such animation for type: " + clipTransition.Clip.name);
        } else if(!CombatStateManager.inCombat)
        {
            CombatAnimationManager.trackAnimation(key, this);
            // Debug.Log("Play Animation: " + clipTransition.Clip.name);
        }
    }

    public void playAnimation(CharacterAnimationType animationType)
    {
        if(spriteSetByHeartBeat())
        {
            animationType = getFallBackIdleType(animationName, animationType);
        }

        switch(animationType)
        {
            case CharacterAnimationType.StandUp:
                playStandUpAnimation();
                break;
            case CharacterAnimationType.Secondary_Idle:
                haltAllAnimations();
                setSpriteToCurrentIdle();
                return;

            case CharacterAnimationType.Idle_Front:
            case CharacterAnimationType.Idle_Back:
            case CharacterAnimationType.OOC_Idle_Front:
            case CharacterAnimationType.OOC_Idle_Back:
            case CharacterAnimationType.Secondary_Idle_Front:
            case CharacterAnimationType.Secondary_Idle_Back:
                haltAllAnimations();
                setSpriteToCurrentIdle();
                return;
            case CharacterAnimationType.None:
                removeAnimation();
                return;
            default:
                startUpAnimations();
                break;              
        }

        animancer.Stop();

        handleFacingChange();

        AnimancerState state = animancer.TryPlay(animationType.ToString());

        if (state == null)
        {
            switch(animationType)
            {
                case CharacterAnimationType.OOC_Wounded_Front:
                case CharacterAnimationType.OOC_Wounded_Back: 
                case CharacterAnimationType.Wounded_Front:
                case CharacterAnimationType.Wounded_Back: 
                    playAnimation(CharacterAnimationType.Wounded);
                    break;
                case CharacterAnimationType.Death_Front:
                case CharacterAnimationType.Death_Back: 
                    playAnimation(CharacterAnimationType.Death);
                    break;
                case CharacterAnimationType.Run_Front_Left:
                case CharacterAnimationType.Run_Front_Right: 
                    playAnimation(CharacterAnimationType.Run_Front);
                    break;
                case CharacterAnimationType.Run_Back_Left:
                case CharacterAnimationType.Run_Back_Right: 
                    playAnimation(CharacterAnimationType.Run_Back);
                    break;
                case CharacterAnimationType.OOC_Idle_Front:
                case CharacterAnimationType.Secondary_Idle_Front:
                case CharacterAnimationType.Run_Front:
                case CharacterAnimationType.Secondary_Idle:
                case CharacterAnimationType.Spawn:
                    playIdleAnimation(CharacterAnimationType.Idle_Front);
                    break;
                case CharacterAnimationType.OOC_Idle_Back:
                case CharacterAnimationType.Secondary_Idle_Back:
                case CharacterAnimationType.Run_Back:
                    playIdleAnimation(CharacterAnimationType.Idle_Back);
                    break;
                default:
                    removeAnimation();
                    break;                
            }
        }
    }

    public void setFacing(Facing newFacing, bool enableSprite = true)
    {
        switch(currentIdle)
        {
            case CharacterAnimationType.Secondary_Idle:
            case CharacterAnimationType.Death:
            case CharacterAnimationType.Death_Front:
            case CharacterAnimationType.Death_Front_Weaponless:
            case CharacterAnimationType.Death_Back:
            case CharacterAnimationType.Death_Back_Weaponless:
                return;
        }
        
        if(CombatStateManager.inCombat)
        {
            if(linkedStats.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
            {
                newFacing = Facing.NorthEast;
            } else
            {
                newFacing = Facing.SouthWest;
            }
        }
        else if(!changesFacing)
        {
            return;
        }  

        facing.setFacing(newFacing);

        switch(facing.getFacing())
        {
            case Facing.NorthEast:
            case Facing.NorthWest:
                if(currentIdle.ToString().Contains(CharacterAnimationType.Secondary_Idle.ToString()))
                {
                    playIdleAnimation(CharacterAnimationType.Secondary_Idle_Back);
                } else
                {
                    playIdleAnimation(CharacterAnimationType.OOC_Idle_Back);
                }
                break;
            case Facing.SouthWest:
            case Facing.SouthEast:
                if(currentIdle.ToString().Contains(CharacterAnimationType.Secondary_Idle.ToString()))
                {
                    playIdleAnimation(CharacterAnimationType.Secondary_Idle_Front);
                } else
                {
                    playIdleAnimation(CharacterAnimationType.OOC_Idle_Front);
                }
                break;
        }

        handleFacingChange();
    }

    private void handleFacingChange()
    {
        if(!changesFacing)
        {
            return;
        }

        switch(facing.getFacing())
        {
            case Facing.NorthEast:
            case Facing.SouthWest:
                spriteRenderer.flipX = false;
                if(polygonCollider2D.transform != transform)
                {
                    polygonCollider2D.transform.localScale = Vector3.one;
                }
                break;
            case Facing.NorthWest:
            case Facing.SouthEast:
                spriteRenderer.flipX = true;                
                if(polygonCollider2D.transform != transform)
                {
                    polygonCollider2D.transform.localScale = new Vector3(-1, 1, 1);
                }
                break;
        }

        
        setAllShadowDirections(shadowSprite);
        Helpers.updatePolygonCollider(spriteRenderer, polygonCollider2D);
    }

    public void disableExtras()
    {
        if(CombatStateManager.inCombat)
        {
            healthBarManager.hide();
        }

        if(shadowSprite != null)
        {
            // shadowSprite.enabled = false;
            shadowSprite.gameObject.SetActive(false);
        }
    }

    public void enableExtras()
    {
        if(CombatStateManager.inCombat)
        {
            healthBarManager.show();
        }
        
        if(shadowSprite != null)
        {
            // shadowSprite.enabled = true;
            shadowSprite.gameObject.SetActive(true);
        }

    }

    public CharacterAnimationType getFallBackAnimationType(CharacterAnimationType animationType)
    {
        if(animationDict.ContainsKey(animationType))
        {
            return animationType;
        }

        switch(animationType)
        {
            case CharacterAnimationType.Wounded_Front:
            case CharacterAnimationType.Wounded_Back:
                return CharacterAnimationType.Wounded;
            case CharacterAnimationType.Death:
                return CharacterAnimationType.Death_Front;
            case CharacterAnimationType.Death_Front:
            case CharacterAnimationType.Death_Back:
                return CharacterAnimationType.Death;
            case CharacterAnimationType.Attack_Normal_Front:
            case CharacterAnimationType.Attack_Normal_Back:
            case CharacterAnimationType.Attack_Special:
                return CharacterAnimationType.Attack_Normal;
            case CharacterAnimationType.Attack_Normal:
                return CharacterAnimationType.Attack_Normal_Front;
            default:
                // Debug.LogError("No Animation Type in animationDict: " + animationType.ToString());
                return animationType;
        }
    }

    public static CharacterAnimationType getFallBackIdleType(string animationName, CharacterAnimationType animationType)
    {
        return getFallBackIdleType(animationName, animationType, false);
    }

    private static CharacterAnimationType getFallBackIdleType(string animationName, CharacterAnimationType animationType, bool retry)
    {
        if(IdleDictionary.idleDictContainsSprites(animationName, animationType))
        {
            return animationType;
        } else if(retry)
        {
            return animationType;
        }

        retry = true;

        switch(animationType)
        {
            case CharacterAnimationType.Death_Front:
            case CharacterAnimationType.Death_Front_Weaponless:
            case CharacterAnimationType.Death_Back:
            case CharacterAnimationType.Death_Back_Weaponless:
                return getFallBackIdleType(animationName, CharacterAnimationType.Death, retry);
            case CharacterAnimationType.OOC_Idle_Front:
            case CharacterAnimationType.Secondary_Idle_Front:
                return getFallBackIdleType(animationName, CharacterAnimationType.Idle_Front, retry);
            case CharacterAnimationType.OOC_Idle_Back:
            case CharacterAnimationType.Secondary_Idle_Back:
                return getFallBackIdleType(animationName, CharacterAnimationType.Idle_Back, retry);
            case CharacterAnimationType.Idle_Front:
                return getFallBackIdleType(animationName, CharacterAnimationType.OOC_Idle_Front, retry);
            case CharacterAnimationType.Idle_Back:
                return getFallBackIdleType(animationName, CharacterAnimationType.OOC_Idle_Back, retry);
            case CharacterAnimationType.Run_Front:
                return getFallBackIdleType(animationName, CharacterAnimationType.Idle_Front, retry);
            case CharacterAnimationType.Run_Back:
                return getFallBackIdleType(animationName, CharacterAnimationType.Idle_Back, retry);
            case CharacterAnimationType.Secondary_Idle:
                return getFallBackIdleType(animationName, CharacterAnimationType.Idle_Front, false);
            default:
                return animationType;
        }
    }

}
