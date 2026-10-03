using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class PlayerObject : MonoBehaviour
{
    private static PlayerObject instance;

    public static PlayerObject getInstance()
    {
        return instance;
    }

    public static bool hasCustomPromptMessage;
    public Collider2D transitionCollider;
    public CapsuleCollider2D terrainCollider;
    public Transform UIParent;
    public GameObject pressButtonPrompt;
    public LayoutElement pressButtonPromptBackgroundLayout;
    public TextMeshProUGUI pressButtonPromptText;
    // public AnimationManager animationManager;
    // public SpriteRenderer playerSpriteRenderer;

    public MapPopUpButton mapPopUpButton;
    public WorldMapPopUpButton worldMapPopUpButton;
    public GameOverPopUpButton gameOverPopUpButton;

    public PlayerMovement playerMovement;
    public Coroutine levelUpRoutine;

    public static bool playLevelUpOnSceneStart;

    [RuntimeInitializeOnLoadMethod]
    private static void initializePlayerMovement()
    {
        hasCustomPromptMessage = false;
        instance = null;
        playLevelUpOnSceneStart = false;
    }

    private void Awake()
    {
        instance = this;

        setAsCameraTarget();
        // animationManager.setAnimations(PartyManager.getPlayer().uniqueName);

        playerMovement.Awake();
        playerMovement.updateFacing();
        // playerMovement.updateIdleDirection();
        
        TerrainVisibilityManager.initializeOnTransition();

        //OnMoveFinished only fires once the player stops, so the prompt would lag behind while a direction is held
        MovementManager.OnStepFinished.AddListener(setButtonPromptVisibility);
        MovementManager.OnMoveFinished.AddListener(setButtonPromptVisibility);
        FadeToBlackManager.OnFadeBackInFinished.AddListener(setButtonPromptVisibility);
        PlayerStateManager.OnStateChange.AddListener(setButtonPromptVisibility);
        EnemyMovement.ToggleHoverColliders.AddListener(toggleHover);
    }

    void Start()
    {
        StartCoroutine(setCameraSpeed());

        if(playLevelUpOnSceneStart)
        {
            playLevelUpEffect();
        }
    }

    private IEnumerator setCameraSpeed()
    {
        yield return null;

        if(PlayerStateManager.currentActivity != CurrentActivity.InDialogue)
        {
            DialogueManager.setCameraToDefaultSpeed();  
        }
    }

    private void OnDestroy()
    {
        MovementManager.OnStepFinished.RemoveListener(setButtonPromptVisibility);
        MovementManager.OnMoveFinished.RemoveListener(setButtonPromptVisibility);
        FadeToBlackManager.OnFadeBackInFinished.RemoveListener(setButtonPromptVisibility);
        PlayerStateManager.OnStateChange.RemoveListener(setButtonPromptVisibility);
        EnemyMovement.ToggleHoverColliders.RemoveListener(toggleHover);
    }

    public void toggleHover(bool toggleHover)
    {
        transitionCollider.enabled = toggleHover;
        terrainCollider.enabled = toggleHover;
        // animationManager.polygonCollider2D.enabled = toggleHover;
    }

    private void setAsCameraTarget()
    {
        CinemachineVirtualCamera mainCM = GameObject.FindWithTag(LayerAndTagManager.mainVirtualCameraTag).GetComponent<CinemachineVirtualCamera>();
        mainCM.Follow = gameObject.transform;
    }

    public static Transform getInstanceTransform()
    {
        if (instance == null)
        {
            return null;
        }

        return instance.gameObject.transform;
    }

    public static Transform getUIParentTransform()
    {
        return instance.UIParent;
    }

    public static void hidePlayerSprite()
    {
        // if(instance == null || 
        //     instance.playerSpriteRenderer == null ||
        //     instance.animationManager == null)
        // {
        //     return;
        // }

        // instance.playerSpriteRenderer.color = Color.clear;
        // instance.animationManager.disableExtras();
    }

    public static void showPlayerSprite()
    {
        // if(instance == null || 
        //     instance.playerSpriteRenderer == null ||
        //     instance.animationManager == null)
        // {
        //     return;
        // }

        // instance.playerSpriteRenderer.color = Color.white;
        // instance.animationManager.enableExtras();
    }

    public static bool isBehindTerrain()
    {
        return Helpers.hasCollision(getInstance().terrainCollider, LayerAndTagManager.terrainLayerMask);
    }

    public static bool onTopOfTransitionOrTutorial()
    {
        if (Helpers.hasCollision(getInstance().transitionCollider) && !FadeToBlackManager.isMidScreenFade())
        {
            if (Helpers.hasCollision(getInstance().transitionCollider, LayerAndTagManager.transitionLayerMask))
            {
                Transition transition = Helpers.getCollision(getInstance().transitionCollider, LayerAndTagManager.transitionLayerMask).transform.GetComponent<TransitionSpace>().getTransition();
                TransitionManager.changeLocation(transition);
                return true;
            }

            if (PlayerStateManager.currentActivity != CurrentActivity.InTutorialSequence && Helpers.hasCollision(getInstance().transitionCollider, LayerAndTagManager.tutorialLayerMask))
            {
                Collider2D tutorialCollider = Helpers.getCollision(getInstance().transitionCollider, LayerAndTagManager.tutorialLayerMask);

                if (TutorialSequence.startTutorialSequence(tutorialCollider.gameObject))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static void createCustomButtonPrompt(string promptMessage)
    {
        PlayerObject player = getInstance();

        if (player == null)
        {
            return;
        }

        if (promptMessage.Length > 0)
        {
            player.pressButtonPrompt.SetActive(true);
            player.pressButtonPromptText.text = promptMessage;

            rebuildPromptBackground();

            hasCustomPromptMessage = true;
        }
        else
        {
            player.pressButtonPrompt.SetActive(false);
        }
    }

    private static void rebuildPromptBackground()
    {
        PlayerObject player = getInstance();

        if (player == null)
        {
            return;
        }

        RectTransform textRect = player.pressButtonPromptText.GetComponent<RectTransform>();

        LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);
        player.pressButtonPromptBackgroundLayout.preferredWidth = textRect.rect.width;
        player.pressButtonPromptBackgroundLayout.preferredHeight = textRect.rect.height;
    }

    public static void setButtonPromptVisibility()
    {
        setButtonPromptVisibility(MovementManager.playerSpriteIndex);
    }

    public static void setButtonPromptVisibility(int index)
    {
        PlayerObject player = getInstance();

        if (player == null || index != PlayerMovement.getInstance().getMovementIndex())
        {
            return;
        }

        if (PlayerStateManager.currentActivity == CurrentActivity.Walking || 
            PlayerStateManager.currentActivity == CurrentActivity.Cunning || 
            PlayerStateManager.currentActivity == CurrentActivity.Intimidating)
        {
            string promptMessage = player.getPromptMessage();

            if (promptMessage.Length > 0)
            {

                player.pressButtonPrompt.SetActive(true);
                player.pressButtonPromptText.text = promptMessage;

                rebuildPromptBackground();

                hasCustomPromptMessage = false;
            }
            else if (hasCustomPromptMessage)
            {
                return;
            }
            else
            {
                player.pressButtonPrompt.SetActive(false);
            }
        }
        else
        {
            player.pressButtonPrompt.SetActive(false);
        }
    }

    public static void toggleButtonPrompt(bool activate)
    {
        PlayerObject player = getInstance();

        if(player != null)
        {
            player.pressButtonPrompt.SetActive(activate);
        }
    }

    public static void restoreButtonPrompt()
    {
        if(hasCustomPromptMessage)
        {
            toggleButtonPrompt(true);
            return;
        }

        setButtonPromptVisibility();
    }

    private string getPromptMessage()
    {
        switch(PlayerStateManager.currentActivity)
        {
            case CurrentActivity.Walking:
                if (PlayerMovement.canInteract())
                {
                    return KeyBindingList.interactKey.ToString() + ": Interact";
                }

                Collider2D npcCollider = PositionQuery.npcAtPosition(PlayerMovement.getColliderWorldPosition());

                if (npcCollider != null && npcCollider.gameObject.tag.Equals(LayerAndTagManager.partyMemberTag))
                {
                    return KeyBindingList.removePlacedCompanionMovableObjectKey.ToString() + ": Remove";
                }

                Collider2D moveableObjectCollider = PositionQuery.moveableObjectAtPosition(PlayerMovement.getColliderWorldPosition());

                if (moveableObjectCollider != null)
                {
                    EnemyMovement movableObject = moveableObjectCollider.gameObject.GetComponent<EnemyMovement>();

                    if (movableObject != null && movableObject.canBePutBackToStartingPosition())
                    {
                        return KeyBindingList.removePlacedCompanionMovableObjectKey.ToString() + ": Return";
                    }
                }
                break;
            case CurrentActivity.Cunning:
                if (CunningManager.getInstance().canUseSkill())
                {
                    return KeyBindingList.interactKey.ToString() + ": Cunning";
                } else if(CunningManager.getInstance().hasTooExpensiveTarget())
                {
                    return Constants.needMoreChargesMessage;
                } else
                {
                    return KeyBindingList.moveNorthKey.ToString() + "/" + 
                            KeyBindingList.moveWestKey.ToString() + "/" + 
                            KeyBindingList.moveSouthKey.ToString() + "/" + 
                            KeyBindingList.moveEastKey.ToString() + ": Move";
                }
            case CurrentActivity.Intimidating:
                if (IntimidateManager.getInstance().canUseSkill())
                {
                    return KeyBindingList.interactKey.ToString() + ": Intimidate";
                }

                if(!IntimidateManager.hasEnoughCharges())
                {
                    return Constants.needMoreChargesMessage;
                }

                break;
        }

        return "";
    }

    public static MapPopUpButton getMapPopUpButton()
    {
        return instance.mapPopUpButton;
    }

    public static WorldMapPopUpButton getWorldMapPopUpButton()
    {
        return instance.worldMapPopUpButton;
    }

    // public static AnimationManager getAnimationManager()
    // {
    //     return null;
    //     // return instance.animationManager;
    // }

    public static void playLevelUpEffect()
    {
        if(CombatStateManager.inCombat)
        {
            playLevelUpOnSceneStart = true;
            return;
        } else if(getInstance() != null && getInstance().levelUpRoutine == null)
        {
            getInstance().levelUpRoutine =  getInstance().StartCoroutine(runLevelUpEffect());
            playLevelUpOnSceneStart = false;
        }
    }

    private static IEnumerator runLevelUpEffect()
    {
        do
        {
            yield return null;
        } while(PlayerStateManager.currentActivity == CurrentActivity.InFade);

        AudioManager.playLvlUpSFX();

        EffectAnimationManager frontEffect = createLevelUpEffect(EffectAnimationType.FrontLvlUp, AnimationDataList.frontLvlUp);
        EffectAnimationManager backEffect = createLevelUpEffect(EffectAnimationType.BackLvlUp, AnimationDataList.backLvlUp);

        //each effect destroys itself when its animation finishes
        while (frontEffect != null || backEffect != null)
        {
            yield return null;
        }
    }

    private static EffectAnimationManager createLevelUpEffect(EffectAnimationType effectType, AnimationData animationData)
    {
        EffectAnimationManager effect = Instantiate(Resources.Load<GameObject>(PrefabNames.effect), instance.transform).GetComponent<EffectAnimationManager>();
        effect.transform.position = instance.transform.position;

        effect.effectType = effectType;
        effect.animationData = animationData;
        effect.startAnimation();

        return effect;
    }

    public static void playDeathAnimation()
    {
        // if(instance == null || instance.animationManager == null)
        // {
        //     return;
        // }

        // AnimationManager animationManager = getAnimationManager();

        // animationManager.playDeathAnimation();
    }

    public static void spawnGameOverPopUp()
    {
        if(instance != null && instance.gameOverPopUpButton != null)
        {
            instance.StartCoroutine(waitThenSpawnGameOverPopUpButton());
        }
    }

    private static IEnumerator waitThenSpawnGameOverPopUpButton()
    {
        float wait = 4f;
        float timeWaited = 0f;

        while(timeWaited <= wait)
        {
            yield return null;

            timeWaited += Time.deltaTime;
        }

        instance.gameOverPopUpButton.spawnPopUp();
    }

    public static void setSpriteSortingLayer(SortingLayerInfo sortingLayerInfo)
    {
        // if(instance == null || instance.playerSpriteRenderer == null)
        // {
        //     return;
        // }

        // sortingLayerInfo.setRendererSortingLayer(instance.playerSpriteRenderer);
    }

}
